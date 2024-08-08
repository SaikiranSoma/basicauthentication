using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BasicAuthenticationNew
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("unathorized");
            }
            string authorizationheader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationheader))
            {
                return AuthenticateResult.Fail("unathorized");
            }

            if(!authorizationheader.StartsWith("basic ", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("unathorized");
            }

            var token = authorizationheader.Substring(6);
            var crediantialsAsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            var crediatials=crediantialsAsString.Split(':');
            if (crediatials?.Length != 2)
            {
                return AuthenticateResult.Fail("unathorized");
            }

            var username = crediatials[0];
            var password = crediatials[1];

            if (username == "sai" && password == "password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username)
                };
                var identity = new ClaimsIdentity(claims, "Basic");
                var claimPrincipal=new ClaimsPrincipal(identity);
                return AuthenticateResult.Success(new AuthenticationTicket(claimPrincipal,Scheme.Name));
            }
            return AuthenticateResult.Fail("Authentication failed (Invalid)");
        }
    }
}
