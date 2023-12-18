using ArchiAgile.Client.Util;
using ArchiAgile.Shared.Personnel;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ArchiAgile.Client.Pages
{
    public partial class PersonnelAddEdit
    {
        [Inject] HttpClientHelper _httpClientHelper { get; set; }
        [Inject] DialogService _dialogService { get; set; }
        [Parameter] public PersonnelDTO _personnel { get; set; } = new PersonnelDTO();
        [Parameter] public List<PersonnelRoleDTO> _personnelRoleList { get; set; } = new List<PersonnelRoleDTO>();

        private void OnCancelClick()
        {
            _dialogService.Close(false);
        }

        private async Task OnSaveClick()
        {
            var request = new SavePersonnelRequest
            {
                Personnel = _personnel
            };
            var result = await _httpClientHelper.Post<SavePersonnelRequest, SavePersonnelResponse>("/SrvPersonnel/SavePersonnel", request, false, true, true);
            if (result.IsSuccess)
            {
                _dialogService.Close(true);
            }

        }
    }
}
