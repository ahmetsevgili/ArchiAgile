using ArchiAgile.Client.Services;
using ArchiAgile.Shared;
using Microsoft.AspNetCore.Components;

namespace ArchiAgile.Client.Pages
{
    public partial class Login
    {
        private bool busy = false;
        [Inject] private CustomStateProvider _customStateProvider { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        private SignInRequest _signInRequest = new SignInRequest();
        private async Task SignIn()
        {
            busy = true;
            var result = await _customStateProvider.SignIn(_signInRequest);
            if (result)
            {
                var authState = _customStateProvider.GetAuthenticationState();
                if (authState.User.IsInRole("Admin"))
                {

                    _navigationManager.NavigateTo("");
                }
                else
                {
                    _navigationManager.NavigateTo("order");
                }
            }
            busy = false;
        }
    }
}
