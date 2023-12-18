using ArchiAgile.Client.Util;
using ArchiAgile.Shared.User;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ArchiAgile.Client.Pages
{
    public partial class UserAddEdit
    {
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] HttpClientHelper _httpClientHelper { get; set; }
        [Inject] DialogService _dialogService { get; set; }
        [Inject] NotificationService _notificationService { get; set; }
        [Parameter] public UserDTO _user { get; set; } = new UserDTO();
        [Parameter] public List<UserRoleDTO> _userRoleList { get; set; } = new List<UserRoleDTO>();

        protected override void OnInitialized()
        {
        }
        private void OnFileInputChange(string value, string name)
        {
            _user.Image = value;
        }

        private void OnCancelClick()
        {
            _dialogService.Close(false);
        }

        private async Task OnSaveClick()
        {
            if (!string.IsNullOrWhiteSpace(_user.Password))
            {
                if (_user.Password != _user.Repassword)
                {
                    _notificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error! ", Detail = "Password and Repassword mismached!", Duration = 4000 });
                }
            }
            var request = new SaveUserRequest
            {
                User = _user
            };
            var result = await _httpClientHelper.Post<SaveUserRequest, SaveUserResponse>("/SrvUser/SaveUser", request, false, true, true);
            if (result.IsSuccess)
            {
                _dialogService.Close(true);
            }

        }

        private void OnChange(object value, string name)
        {
        }
    }
}
