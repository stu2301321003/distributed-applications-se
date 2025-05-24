using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using VacationManager.Database;

namespace VacationManager
{
    public class User
    {
        public int Id { get; set; }

        [Required, Length(2, 25)]
        public string Name { get; set; } = null!;

        [Required, Length(2, 35)]
        public string LastName { get; set; } = null!;

        [EmailAddress, Length(5,50)]
        public string Email { get; set; } = null!;

        [Phone, Length(5, 20)]
        public string PhoneNumber { get; set; } = null!;

        [Required, Length(5, 50)]
        public string Password { get; set; } = null!;

        [Required, Length(5, 50)]
        public string Salt { get; set; } = null!;

        public Roles Role { get; set; }
        public int? TeamId { get; set; }
        public Team? Team { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }

        [Required, Length(2, 35)]
        public string Name { get; set; } = null!;

        public int ManagerId { get; set; }
        public User Manager { get; set; } = new();

        public List<User> Users { get; set; } = [];
        public DateTime CreatedAt { get; set; }

    }

    public class Company
    {
        public int Id { get; set; }

        [Required, Length(2, 35)]
        public string Name { get; set; } = null!;

        public List<Team> Teams { get; set; } = [];

        public int CeoId { get; set; }
        public User Ceo { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    public class Leave
    {
        public int Id { get; set; }
        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = new();
        public LeaveType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum LeaveType
    {
        Paid,
        Unpaid,
        Sick,
        Overtime,
        CompensatoryTime,
        Parenthood
    }

    public enum Roles
    {
        CEO,
        Manager,
        Employee
    }

    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt) =>
            Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                    password: password!,
                                    salt: Encoding.UTF8.GetBytes(salt),
                                    prf: KeyDerivationPrf.HMACSHA256,
                                    iterationCount: 100000,
                                    numBytesRequested: 256 / 8));
    }

    public class AuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "MyAuthenticationScheme"; //todo get from appsettings
        public string TokenHeaderName { get; set; } = "MyToken";//todo get from appsettings
    }

    public class UserAuthenticationHandler
        (IOptionsMonitor<AuthenticationOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder urlEncoder,
        ISystemClock clock,
        AppDbContext context) : AuthenticationHandler<AuthenticationOptions>(options, loggerFactory, urlEncoder, clock)
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
            var claims = new List<Claim>()
            {
                new Claim("FirstName", "Juan")
            };

            var claimsIdentity = new ClaimsIdentity
                (claims, this.Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal
                (claimsIdentity);

            return AuthenticateResult.Success
                (new AuthenticationTicket(claimsPrincipal,
                this.Scheme.Name));
        }
    }

    public static class JwtHelper
    {

    }
}
