using ArchiAgile.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen.Blazor;
using ArchiAgile.Shared.User;

namespace ArchiAgile.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] private CustomStateProvider _customStateProvider { get; set; }
        [Inject] ThemeState _themeState { get; set; }
        [Inject] NavigationManager _uriHelper { get; set; }
        [Inject] IJSRuntime _jsRuntime { get; set; }
        [CascadingParameter] Task<AuthenticationState> AuthenticationState { get; set; }
        private UserDTO user = new UserDTO();
        bool sidebarExpanded = true;
        bool bodyExpanded = false;
        RadzenSidebar sidebar0;
        RadzenBody body0;

        dynamic themes = new[]
        {
                new { Text = "Default Theme", Value = "default"},
                new { Text = "Dark Theme", Value="dark" },
                new { Text = "Software Theme", Value = "software"},
                new { Text = "Humanistic Theme", Value = "humanistic" },
                new { Text = "Standard Theme", Value = "standard" },
                new { Text = "Material Theme", Value = "material" }
        };
        string Theme => $"{_themeState.CurrentTheme}.css";

        protected override async Task OnInitializedAsync()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var currentUser = await _customStateProvider.GetCurrentUser();
            if (currentUser == null)
            {
                return;
            }
            user.Name = currentUser.Name;
            user.Surname = currentUser.Surname;
            user.Image = currentUser.Image;
            if (string.IsNullOrWhiteSpace(user.Image))
            {
                user.Image = "assets/images/account_circle_ic_icon.png";
            }

        }
        public async Task ProfileMenuClick(RadzenProfileMenuItem item)
        {
            if (item.Text == "Logout")
            {
                await _customStateProvider.SignOut();
            }
        }
    }
}
