using AuthDemo.Core.Models;

namespace AuthDemo.Core.Services.Interfaces;

public interface IJwtHandler
{
    Tokens Create(Guid userId);
}