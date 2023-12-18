using ArchiAgile.Client.Util;
using ArchiAgile.Shared.User;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ArchiAgile.Client.Pages
{
    public partial class User
    {
        [Inject] HttpClientHelper _httpClientHelper { get; set; }
        [Inject] DialogService _dialogService { get; set; }
        private IEnumerable<UserDTO> _userList = new List<UserDTO>();
        private List<UserRoleDTO> _userRoleList = new List<UserRoleDTO>();
        protected override async Task OnInitializedAsync()
        {
            var result = await _httpClientHelper.Post<string, GetUserOnInitializedResponse>("/SrvUser/GetUserOnInitialized", "", false, false, false);

            if (result.IsSuccess)
            {
                _userList = result.Response.UserList;
                _userRoleList = result.Response.UserRoleList;
            }
        }

        private async Task OnUserAddClick()
        {
            var user = new UserDTO
            {
                IsActive = true,
                UserRoleID = 2
            };
            var result = await _dialogService.OpenAsync<UserAddEdit>($"User Add",
               new Dictionary<string, object>() {
                   { "_user", user },
                   { "_userRoleList", _userRoleList },
               },
               new DialogOptions() { Width = "700px", Height = "570px", });
            if (result != null && result)
            {
                await OnInitializedAsync();
            }
        }

        private async Task OnUserEditClick(int userID)
        {
            var user = _userList.FirstOrDefault(x => x.RecordID == userID);
            var result = await _dialogService.OpenAsync<UserAddEdit>($"User Edit",
               new Dictionary<string, object>() {
                   { "_user", user },
                   { "_userRoleList", _userRoleList },
               },
               new DialogOptions() { Width = "700px", Height = "570px", });

            if (result != null && result)
            {
                await OnInitializedAsync();
            }
        }

        //private async Task OnUserDeleteClick(int userID)
        //{
        //    var isConfirm = await _dialogService.Confirm("Are you sure?", "User will be deleted",
        //        new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No", });
        //    if (isConfirm ?? false)
        //    {
        //        var user = _userList.FirstOrDefault(x => x.RecordID == userID);
        //        var request = new DeleteUserRequest
        //        {
        //            UserID = user.RecordID
        //        };
        //        var result = await _httpClientHelper.Post<DeleteUserRequest, DeleteUserResponse>("/SrvAccount/DeleteUser", request, true, true, true);

        //        if (result.IsSuccess)
        //        {
        //            await OnInitializedAsync();
        //        }
        //    }
        //}
    }
}
