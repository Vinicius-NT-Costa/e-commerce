using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Data;

public class UserRepository(UserServiceContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant()
                && !u.IsDeleted);
    }

    public async Task<User> CreateAsync(User user)
    {
        await context.Users.AddAsync(user);
        return user;
    }

    public async Task SaveChangesAsync() =>
        await context.SaveChangesAsync();
}