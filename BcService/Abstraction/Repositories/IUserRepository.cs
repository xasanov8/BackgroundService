using BcService.Models;

namespace BcService.Abstraction.Repositories
{
    public interface IUserRepository
    {
        public Task Add(UserModel user);
        public Task<List<UserModel>> GetAllUsers();
    }
}
