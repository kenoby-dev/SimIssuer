using SimIssuer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SimIssuer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new LoginModel()
            {
                Realm = HttpContext.Request.QueryString["wtrealm"],
                Reply = HttpContext.Request.QueryString["wreply"],
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            SignIn(model);
            return View(new LoginModel() { Realm = model.Realm, UserId = model.UserId });
        }

        public ActionResult Help()
        {
            return View();
        }

        private void SignIn(LoginModel model)
        {
            var claims = GetClaims(model.UserId);
            var config = new SecurityTokenServiceConfiguration(
                "SimIssuer",
                new X509SigningCredentials(
                    new X509Certificate2(Server.MapPath(VirtualPathUtility.ToAbsolute("~/app_data/simissuer.pfx")), "SimIssuer", X509KeyStorageFlags.MachineKeySet),
                    "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"));
            var tokenService = new TokenService(config);
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims.Select(x => new System.Security.Claims.Claim(String.Concat(x.Namespace, "/", x.Name), x.Value))));
            var requestUri = new Uri(BuildRequestUrl(model));
            tokenService.ProcessSignIn(principal, requestUri);
        }

        private string BuildRequestUrl(LoginModel model)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(HttpContext.Request.Url);
            urlBuilder.Append("?wa=wsignin1.0");
            urlBuilder.AppendFormat("&wtrealm={0}", Server.UrlEncode(model.Realm));

            if (!string.IsNullOrEmpty(model.Reply))
            {
                urlBuilder.AppendFormat("&wreply={0}", Server.UrlEncode(model.Reply));
            }

            return urlBuilder.ToString();
        }

        private IEnumerable<Models.Claim> GetClaims(string userId)
        {
            var json = System.IO.File.ReadAllText(Server.MapPath(VirtualPathUtility.ToAbsolute("~/Content/users.json")));
            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<User>>(json);
            var user = users.FirstOrDefault(x => x.Uid.Equals(userId, StringComparison.OrdinalIgnoreCase));

            return user.Claims;
        }

    }
}