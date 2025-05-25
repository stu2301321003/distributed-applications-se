using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using VacationManager.Database;

namespace VacationManager.Auth.Services
{
    public class UserAuthenticationHandler
         (IOptionsMonitor<Models.AuthenticationOptions> options,
         ILoggerFactory loggerFactory,
         UrlEncoder urlEncoder,
         ISystemClock clock,
         AppDbContext context) : AuthenticationHandler<Models.AuthenticationOptions>(options, loggerFactory, urlEncoder, clock)
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
                return AuthenticateResult.Fail($"Missing header: {Options.TokenHeaderName}");

            string token = Request.Headers[Options.TokenHeaderName]!;

            //usually, this is where you decrypt a token and/or lookup a database.
            if (token != "secretCode") //todo get from app settings
            {
                return AuthenticateResult.Fail($"Invalid token.");
            }

            //Success! Add details here that identifies the user
            List<Claim> claims = new List<Claim>()
            {
                new Claim("FirstName", "Juan")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity
                (claims, Scheme.Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal
                (claimsIdentity);

            return AuthenticateResult.Success
                (new AuthenticationTicket(claimsPrincipal,
                Scheme.Name));
        }
    }
}
