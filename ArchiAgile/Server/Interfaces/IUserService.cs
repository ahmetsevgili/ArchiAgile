using ArchiAgile.Shared.User;

namespace ArchiAgile.Server.Interfaces
{
    public interface IUserService
    {
        SaveUserResponse SaveUser(SaveUserRequest request);
        GetUserOnInitializedResponse GetUserOnInitialized(int userId);
    }
}
