namespace ArchiAgile.Server.Interfaces
{
    public interface ICacheService
    {
        void Load();
        void Reload();
        string GetUserImagePath();
    }
}
