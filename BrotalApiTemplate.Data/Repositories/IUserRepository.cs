using BrotalApiTemplate.Domain.Entities;

namespace BrotalApiTemplate.Data.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUserName(string userName);
}