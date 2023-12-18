using ArchiAgile.Client.Util;
using ArchiAgile.Shared.Personnel;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ArchiAgile.Client.Pages
{
    public partial class Personnel
    {
        [Inject] HttpClientHelper _httpClientHelper { get; set; }
        [Inject] DialogService _dialogService { get; set; }
        private IEnumerable<PersonnelDTO> _personnelList = new List<PersonnelDTO>();
        private List<PersonnelRoleDTO> _personnelRoleList = new List<PersonnelRoleDTO>();
        protected override async Task OnInitializedAsync()
        {
            var result = await _httpClientHelper.Post<string, GetPersonnelOnInitializedResponse>("/SrvPersonnel/GetPersonnelOnInitialized", "", false, false, false);

            if (result.IsSuccess)
            {
                _personnelList = result.Response.PersonnelList;
                _personnelRoleList = result.Response.PersonnelRoleList;
            }
        }

        private async Task OnPersonnelAddClick()
        {
            var personnel = new PersonnelDTO
            {
                IsActive = true,
            };
            var result = await _dialogService.OpenAsync<PersonnelAddEdit>($"Personnel Add",
               new Dictionary<string, object>() {
                   { "_personnel", personnel },
                   { "_personnelRoleList", _personnelRoleList },
               },
               new DialogOptions() { Width = "700px", Height = "570px", });
            if (result != null && result)
            {
                await OnInitializedAsync();
            }
        }

        private async Task OnPersonnelEditClick(int personnelID)
        {
            var user = _personnelList.FirstOrDefault(x => x.RecordID == personnelID);
            var result = await _dialogService.OpenAsync<PersonnelAddEdit>($"Personnel Edit",
               new Dictionary<string, object>() {
                   { "_personnel", user },
                   { "_personnelRoleList", _personnelRoleList },
               },
               new DialogOptions() { Width = "700px", Height = "570px", });

            if (result != null && result)
            {
                await OnInitializedAsync();
            }
        }
    }
}
