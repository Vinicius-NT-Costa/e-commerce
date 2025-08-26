using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string reqEmail);
    Task CreateAsync(User user);
    Task SaveChangesAsync();
}