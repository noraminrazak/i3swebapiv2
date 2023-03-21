using i3sAuth;
using I3SwebAPIv2.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace I3SwebAPIv2.Class
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApplicationAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
            //context.Validated();
        }
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Rijndael auth = new Rijndael();
            AuthRepository authRepository = new AuthRepository();
            string salt = ConfigurationManager.AppSettings["passPhrase"];

            return Task.Factory.StartNew(() =>
            {
                if (context.UserName != null && context.Password != null)
                {
                    LoginDataModel data = authRepository.ValidateUser(context.UserName, auth.EncryptRijndael(context.Password, salt));
                    if (data.Success)
                    {
                        var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                        identity.AddClaim(new Claim("username", context.UserName));
                        identity.AddClaim(new Claim("password", auth.EncryptRijndael(context.Password, salt)));

                        var dic = new Dictionary<string, string>
                        {
                            { "last_login", data.Message}
                        };
                        var properties = new AuthenticationProperties(dic);

                        var ticket = new AuthenticationTicket(identity, properties);
                        context.Validated(ticket);
                    }
                    else
                    {
                        context.SetError(data.Code, data.Message);
                        //return;
                    }
                }
                else
                {
                    context.SetError("missing_params", "Missing parameters.");
                    //return;
                }
            });

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}