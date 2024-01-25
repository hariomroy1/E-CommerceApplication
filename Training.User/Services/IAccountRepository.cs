using DataLayer.Entities;

namespace Training.User.Services
{
    public interface IRegisterRepository
    {
        string Create(RegisterEntity user);
        RegisterEntity Login(LoginEntity user);
        Task<RegisterEntity> FindCurrentUser(string email);
        Task<RegisterEntity> FindCurrentUserById(int userId);
    }
}
