using AuthDemo.Core.Models;

namespace AuthDemo.Core.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAsync();
    Task<UserResponse> GetByIdAsync(Guid id);
    Task<UserResponse> GetByUsernameAsync(string username);
    Task AddAsync(UserRequest request);
    Task<Tokens> LoginAsync(AuthRequest request);
}