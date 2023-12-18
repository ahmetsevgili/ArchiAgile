using ArchiAgile.Shared.Personnel;

namespace ArchiAgile.Server.Interfaces
{
    public interface IPersonnelService
    {
        SavePersonnelResponse SavePersonnel(SavePersonnelRequest request);
        GetPersonnelOnInitializedResponse GetPersonnelOnInitialized();
    }
}
