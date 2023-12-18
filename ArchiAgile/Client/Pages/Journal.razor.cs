using ArchiAgile.Client.Util;
using ArchiAgile.Shared.Journal;
using ArchiAgile.Shared.Personnel;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace ArchiAgile.Client.Pages
{
    public partial class Journal
    {
        [Inject] HttpClientHelper _httpClientHelper { get; set; }
        [Inject] DialogService _dialogService { get; set; }
        private IEnumerable<JournalDTO> _journalList = new List<JournalDTO>();
        private bool _isLoading = false;
        private int _count;
        private RadzenDataGrid<JournalDTO> _grid;
        private List<PersonnelDTO> _personnelList = new List<PersonnelDTO>();
        private List<PersonnelRoleDTO> _personnelRoleList = new List<PersonnelRoleDTO>();
        protected override async Task OnInitializedAsync()
        {
            var result = await _httpClientHelper.Post<string, GetJournalOnInitializedResponse>("/SrvJournal/GetJournalOnInitialized", "", false, false, false);

            if (result.IsSuccess)
            {
                _personnelList = result.Response.PersonnelList;
                _personnelRoleList = result.Response.PersonnelRoleList;
                await _grid.Reload();
            }
        }
        private async Task LoadData(LoadDataArgs args)
        {
            _isLoading = true;
            var request = new GetJournalPaginationRequest
            {
                Filter = args.Filter,
                OrderBy = args.OrderBy,
                Skip = args.Skip,
                Top = args.Top,
            };
            var result = await _httpClientHelper.Post<GetJournalPaginationRequest, GetJournalPaginationResponse>("/SrvJournal/GetJournalPagination", request, false, false, true);

            if (result.IsSuccess)
            {
                _journalList = result.Response.JournalList;
                _count = result.Response.Count;
            }
            _isLoading = false;
        }

        private async Task OnJournalAddClick()
        {
            var journal = new JournalDTO
            {
                IsActive = true,
            };
            var result = await _dialogService.OpenAsync<JournalAddEdit>($"Journal Add",
               new Dictionary<string, object>() {
                   { "_journal", journal },
                   { "_personnelList",_personnelList},
                   { "_personnelRoleList",_personnelRoleList }
               },
               new DialogOptions() { Width = "1000px", Height = "570px", });
            if (result != null && result)
            {
                await _grid.Reload();
            }
        }

        private async Task OnJournalEditClick(int noteID)
        {
            var journal = _journalList.FirstOrDefault(x => x.RecordID == noteID);
            var result = await _dialogService.OpenAsync<JournalAddEdit>($"Journal Edit",
               new Dictionary<string, object>() {
                   { "_journal", journal },
                   { "_personnelList",_personnelList},
                   { "_personnelRoleList",_personnelRoleList }
               },
               new DialogOptions() { Width = "1000px", Height = "570px", });

            if (result != null && result)
            {
                await _grid.Reload();
            }
        }
    }
}
