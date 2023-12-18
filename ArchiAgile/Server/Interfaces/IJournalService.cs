using ArchiAgile.Shared.Journal;

namespace ArchiAgile.Server.Interfaces
{
    public interface IJournalService
    {
        GetJournalPaginationResponse GetJournalPagination(GetJournalPaginationRequest request, int userID);
        GetJournalOnInitializedResponse GetJournalOnInitialized();
        SaveJournalResponse SaveJournal(SaveJournalRequest request, int userID);
    }
}
