using ArchiAgile.Client.Util;
using ArchiAgile.Shared.Journal;
using ArchiAgile.Shared.Personnel;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ArchiAgile.Client.Pages
{
    public partial class JournalAddEdit
    {
        [Inject] HttpClientHelper _httpClientHelper { get; set; }
        [Inject] DialogService _dialogService { get; set; }
        [Parameter] public JournalDTO _journal { get; set; } = new JournalDTO();
        [Parameter] public List<PersonnelRoleDTO> _personnelRoleList { get; set; } = new List<PersonnelRoleDTO>();
        [Parameter] public List<PersonnelDTO> _personnelList { get; set; } = new List<PersonnelDTO>();

        private void OnCancelClick()
        {
            _dialogService.Close(false);
        }

        private async Task OnSaveClick()
        {
            var request = new SaveJournalRequest
            {
                Journal = _journal
            };
            var result = await _httpClientHelper.Post<SaveJournalRequest, SaveJournalResponse>("/SrvJournal/SaveJournal", request, false, true, true);
            if (result.IsSuccess)
            {
                _dialogService.Close(true);
            }

        }
        private async Task OnConferPersonnelChange(object args)
        {
            var selectedID = (int)args;
            var personnel = _personnelList.FirstOrDefault(x=>x.RecordID == selectedID);
            _journal.ConferPersonnelRoleID = personnel.PersonnelRoleID;
        }
    }
}
