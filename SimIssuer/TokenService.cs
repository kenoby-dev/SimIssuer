using System;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Services;
using System.Security.Claims;
using System.Web;

namespace SimIssuer
{
    public class TokenService : SecurityTokenService
    {

        public TokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            // Retrieve the authentication realm settings.
            Scope scope = new Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials);
            scope.TokenEncryptionRequired = false;

            // Validate the reply to address.
            if (IsValidReplyAddress(request.ReplyTo, scope))
            {
                // Set the ReplyTo address for the WS-Federation passive protocol (wreply). This is the address to which responses will be directed. 
                scope.ReplyToAddress = request.ReplyTo;
            }
            else
            {
                // If reply to address is not valid, then override with the applies to address.
                scope.ReplyToAddress = scope.AppliesToAddress;
            }

            return scope;
        }

        private bool IsValidReplyAddress(string replyTo, Scope scope)
        {
            return (!string.IsNullOrEmpty(replyTo));
        }

        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            return principal.Identity as ClaimsIdentity;
        }

        public virtual void ProcessSignIn(ClaimsPrincipal principal)
        {
            HttpContext context = HttpContext.Current;
            ProcessSignIn(principal, context.Request.Url);
        }

        public virtual void ProcessSignIn(ClaimsPrincipal principal, Uri requestUri)
        {
            // Get the current context.
            HttpContext context = HttpContext.Current;

            // Create the signin request message based on the current request context.
            SignInRequestMessage requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(requestUri);

            // Create the signin response message based on the processing the signin request.
            SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, principal, this);

            // Process the signin response.
            FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, context.Response);
        }

    }
}