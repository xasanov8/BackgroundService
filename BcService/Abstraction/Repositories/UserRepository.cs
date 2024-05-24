using BcService.Infrostructure;
using BcService.Models;
using Microsoft.EntityFrameworkCore;

namespace BcService.Abstraction.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(UserModel user)
        {
            var res = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (res != null)
            {
                return;
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
