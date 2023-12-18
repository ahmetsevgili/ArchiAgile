using ArchiAgile.Client.Services;
using ArchiAgile.Client.Util;
using ArchiAgile.Shared.User;
using Microsoft.AspNetCore.Components;

namespace ArchiAgile.Client.Pages
{
    public partial class Profile
    {
        [Inject] HttpClientHelper _httpClientHelper { get; set; }
        [Inject] private CustomStateProvider _customStateProvider { get; set; }
        private UserDTO _user = new UserDTO();
        private bool isBusy = false;

        protected override async Task OnInitializedAsync()
        {
            var currentUser = await _customStateProvider.GetCurrentUser();
            _user.Name = currentUser.Name;
            _user.Surname = currentUser.Surname;
            _user.Image = currentUser.Image;
            _user.RecordID = currentUser.UserID;
            _user.Username = "";
            _user.Password = "";
            _user.Repassword = "";
            _user.IsActive = true;
            if (string.IsNullOrWhiteSpace(_user.Image))
            {
                _user.Image = "assets/images/account_circle_ic_icon.png";
            }
        }
        private void OnFileInputChange(string value, string name)
        {
            _user.Image = value;
        }

        private async Task OnSaveClick()
        {
            isBusy = true;
            var request = new SaveUserRequest
            {
                User = _user
            };
            var result = await _httpClientHelper.Post<SaveUserRequest, SaveUserResponse>("/SrvUser/SaveUser", request, true, true, true);
            if (result.IsSuccess)
            {

            }
            isBusy = false;
        }
    }
}
