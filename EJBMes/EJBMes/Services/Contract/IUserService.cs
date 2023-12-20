using Microsoft.EntityFrameworkCore;
using EJBMes.Models;

namespace EJBMes.Services.Contract
{
    public interface IUserService
    {
        Task<UserMes> GetUser(string username, string pwd);

        Task<UserMes> SaveUser(UserMes model);
    }
}
