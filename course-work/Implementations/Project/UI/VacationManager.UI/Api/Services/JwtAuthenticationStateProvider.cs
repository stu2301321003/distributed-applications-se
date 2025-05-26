using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

public class JwtAuthenticationStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider
{
    private static readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsStringAsync("authToken");
        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(_anonymous);

        // Remove any surrounding quotes if accidentally stored
        token = token.Trim('\"');

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return new AuthenticationState(_anonymous);

        try
        {
            var jwt = handler.ReadJwtToken(token);
            var claims = jwt.Claims.ToList();

            if (jwt.ValidTo < DateTime.UtcNow)
            {
                return new AuthenticationState(_anonymous);
            }

            // Map the custom "role" claim into ClaimTypes.Role
            if (jwt.Payload.TryGetValue("role", out var rawRole))
            {
                if (rawRole is string r)
                    claims.Add(new Claim(ClaimTypes.Role, r));
                else if (rawRole is IEnumerable<object> roles)
                    claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x.ToString()!)));
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    public void NotifyUserAuthentication(string token)
    {
        token = token?.Trim('\"') ?? "";
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
            return;
        }

        var jwt = handler.ReadJwtToken(token);
        var claims = jwt.Claims.ToList();
        if (jwt.Payload.TryGetValue("role", out var rawRole))
        {
            if (rawRole is string r)
                claims.Add(new Claim(ClaimTypes.Role, r));
            else if (rawRole is IEnumerable<object> roles)
                claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x.ToString()!)));
        }

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    // New method to get the user's first name from the token
    public async Task<string?> GetUserFirstNameAsync()
    {
        var token = await localStorage.GetItemAsStringAsync("authToken");
        if (string.IsNullOrWhiteSpace(token))
            return null;

        token = token.Trim('\"');
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return null;

        try
        {
            var jwt = handler.ReadJwtToken(token);
            if (jwt.Payload.TryGetValue("Name", out var firstNameValue) && firstNameValue is string firstName)
            {
                return firstName;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    public async Task<int?> GetUserIdAsync()
    {
        var token = await localStorage.GetItemAsStringAsync("authToken");
        if (string.IsNullOrWhiteSpace(token))
            return null;

        token = token.Trim('\"');
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return null;

        try
        {
            var jwt = handler.ReadJwtToken(token);
            if (jwt.Payload.TryGetValue("nameid", out var id) && int.TryParse((string)id, out int idInt))
            {
                return idInt;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
