using ArchiAgile.Client.Util;
using ArchiAgile.Shared;
using ArchiAgile.Shared.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ArchiAgile.Client.Services
{
    public class CustomStateProvider : AuthenticationStateProvider
    {
        private CurrentUserDTO _currentUser;
        private HttpClientHelper _httpClientHelper;
        private AuthenticationState _authenticationState;
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public CustomStateProvider(HttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            var userInfo = await GetCurrentUser();
            if ((userInfo?.IsAuthenticated) ?? false)
            {
                var claims = _currentUser.Claims.Select(c => new Claim(c.Key, c.Value)).ToArray();
                identity = new ClaimsIdentity(claims, "Server authentication");
            }

            _authenticationState = new AuthenticationState(new ClaimsPrincipal(identity));
            return _authenticationState;
        }

        public AuthenticationState GetAuthenticationState()
        {
            return _authenticationState;
        }

        public async Task<CurrentUserDTO> GetCurrentUser()
        {
            if (_currentUser != null)
            {
                return _currentUser;
            }

            await semaphoreSlim.WaitAsync();
            try
            {
                if (_currentUser != null)
                {
                    return _currentUser;
                }

                var result = await _httpClientHelper.Post<string, CurrentUserDTO>("/SrvAccount/CurrentUserInfo", null, false, false, false);
                if (result.IsSuccess)
                {
                    _currentUser = result.Response;
                    return _currentUser;
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }

            return _currentUser;
        }

        public async Task SignOut()
        {
            var result = await _httpClientHelper.Post<string, bool>("/SrvAccount/SignOut", "", false, false, true);
            if (result.IsSuccess)
            {
                _currentUser = null;
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }
        public async Task<bool> SignIn(SignInRequest signInRequest)
        {
            bool retVal = false;
            var result = await _httpClientHelper.Post<SignInRequest, SignInResponse>("/SrvAccount/SignIn", signInRequest, false);
            if (result.IsSuccess)
            {
                retVal = string.IsNullOrWhiteSpace( result.Response.ResponseMessage);
                _currentUser = null;
                await GetCurrentUser();
                if (retVal)
                {
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                }
            }
            return retVal;
        }

    }
}
