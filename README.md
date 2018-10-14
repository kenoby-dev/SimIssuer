# SimIssuer

A simple simulation issuer for WS-Fed protocol written in C#/.NET.

## Overview

The **SimIssuer** is designed to work as a test harness when using the WS-Federation (ws-fed) authentication protocol.  See http://docs.oasis-open.org/wsfed/federation/v1.2/ws-federation.html.

The **SimIssuer** solution acts as the authentication provider/issuer.

There is no authentication per se.  Rather, a list of test users is provided for selection on the SimIssuer homepage.  Select a user from the list and their list of claims will be displayed on the page.  Click the **Login** button to create the token and post it to the relying party application.

## List of Users

To customise the list of users available in the SimIssuer application, edit the `content\users.json` file, adding the users and claims required.

The `users.json` file follows the following format:

```
[
  {
    "uid": "{Identifier of the user - displayed in the user select drop down list}",
    // List of claims of the user.
    "claims": [
        // Repeat for each user claim.
      {
        "name": "{Name of the claim, i.e. name}",
        "namespace": "{Namespace of the claim, i.e. http://schemas.microsoft.com/ws/2008/06/identity/claims}",
        "value": "{Value of the claim}"
      },
```

## Certificate

A signing certificate is provided in the SimIssuer application.  It is contained in the `App_Data` folder and is named `simissuer.pfx`.  The password for this certificate is 'SimIssuer'.

**DO NOT** use this certificate in production systems.  It is for testing purposes only.  Provide your own certificate if you do not want to use the one included in source control.  Keep the name and location of the certificate the same (i.e. App_Data\simissuer.pfx).

The Relying Party applications must trust the SimIssuer before accepting the token and user claims.  This trust is handled by the signing certificate.  A Relying Party application should include the public key copy of this certificate (simissuer.cer) in the relying party application and validate the token based on the public key.

The public key copy of the certificate can be found at `content\simissuer.cer` and is available for download on the SimIssuer homepage.  The thumbprint of this certificate is `678C2654C4E490CBD597D41CF9BFF44046632BC1`.

## Relying Parties

A couple of **Relying Party** applications have been provided for testing purposes.

The first relying party is a .NET web application, the second being a node.js application.  Both use the SimIssuer to perform the authentication duties.
