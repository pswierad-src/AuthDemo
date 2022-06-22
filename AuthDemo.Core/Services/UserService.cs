using AuthDemo.Core.Models;
using AuthDemo.Core.Services.Interfaces;
using AuthDemo.Mongo.Documents;
using AuthDemo.Mongo.Repositories;

namespace AuthDemo.Core.Services;

public class UserService : IUserService
{
    private readonly IMongoRepository<UserDocument> _userRepository;
    private readonly IJwtHandler _jwtHandler;

    public UserService(IMongoRepository<UserDocument> userRepository, IJwtHandler jwtHandler)
    {
        _userRepository = userRepository;
        _jwtHandler = jwtHandler;
    }

    public async Task<IEnumerable<UserResponse>> GetAsync()
    {
        var users = await _userRepository.GetAsync();

        return users.Select(x => new UserResponse
        {
            Username = x.Username,
            FirstName = x.FirstName,
            LastName = x.LastName,
            DateCreated = x.DateAdded,
            DateModified = x.DateModified
        });
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == id);

        return new UserResponse
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateCreated = user.DateAdded,
            DateModified = user.DateModified
        };
    }
    
    public async Task<UserResponse> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Username == username);

        return new UserResponse
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DateCreated = user.DateAdded,
            DateModified = user.DateModified
        };
    }

    public async Task AddAsync(UserRequest request)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user is not null)
        {
            throw new Exception("User already exists.");
        }

        await _userRepository.AddAsync(new UserDocument
        {
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = request.Password
        });
    }

    public async Task<Tokens> LoginAsync(AuthRequest request)
    {
        var user = await _userRepository.FirstOrDefaultAsync(x => x.Username == request.Username 
                                                                  && x.PasswordHash == request.Password);

        if (user is null)
        {
            throw new Exception("Invalid username or password");
        }

        var tokens = _jwtHandler.Create(user.Id);

        return tokens;
    }
}