using Microsoft.EntityFrameworkCore;
using EJBMes.Models;
using EJBMes.Services.Contract;

namespace EJBMes.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly EjbproductionReportContext _dbContext;

        public UserService(EjbproductionReportContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserMes> GetUser(string userId, string pwd)
        {
            UserMes userFound = await _dbContext.UserMes.Where(u => u.UserId == userId && u.Password == pwd).FirstOrDefaultAsync();

            return userFound;
        }

        public async Task<UserMes> SaveUser(UserMes model)
        {
            _dbContext.UserMes.Add(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
