using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ISyncLocalStorageService localStorage;

    public AuthStateProvider(ISyncLocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    private AuthenticationState? authenticationState;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (this.authenticationState == null)
        {
            string tokenResult = localStorage.GetItem<string>("token");
            var identity = GetClaimsIdentity(tokenResult);
            this.authenticationState = new AuthenticationState(new ClaimsPrincipal(identity));
            NotifyAuthenticationStateChanged(Task.FromResult(this.authenticationState));
        }

        return this.authenticationState!;
    }

    public async Task<string?> GetTokenAsync()
    {
        string? tokenResult = null;
        try
        {
            tokenResult = localStorage.GetItem<string>("token");

            if (tokenResult == null)
            {
                LogOut();
                tokenResult = null;
            }
        }
        catch (Exception ex)
        {
            //TODO: Add error handling
            Console.WriteLine("GetTokenAsync unsuccessfull - " + ex.ToString());
        }

        return tokenResult;
    }

    public int GetuserId()
    {
        try
        {
            var id = localStorage.GetItem<int>("userId");
            return id;
        }
        catch (Exception ex)
        {
            //TODO: Add error handling
            Console.WriteLine("GetuserId unsuccessfull - " + ex.ToString());
            return 0;
        }
    }

    public void Login(string tokenResult, int employeeId)
    {
        if (tokenResult != null)
        {
            var identity = GetClaimsIdentity(tokenResult);

            authenticationState = new AuthenticationState(new ClaimsPrincipal(identity));

            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

            localStorage.SetItem("token", tokenResult);
            localStorage.SetItem("userId", employeeId);
        }
        else
        {
            localStorage.RemoveItem("token");
            localStorage.RemoveItem("userId");
        }
    }

    public void LogOut()
    {
        localStorage.RemoveItem("token");
        authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    }

    private ClaimsIdentity GetClaimsIdentity(string tokenResult)
    {
        var result = new ClaimsIdentity();

        if (tokenResult != null)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenResult);
            result = new ClaimsIdentity(jwtSecurityToken.Claims, "LoginAuthentication");
        }

        return result;
    }
}