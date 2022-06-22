using AuthDemo.Core.Models;
using AuthDemo.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserRequest request)
    {
        await _userService.AddAsync(request);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(AuthRequest request)
    {
        var tokens = await _userService.LoginAsync(request);

        return Ok(tokens);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAsync()
    {
        var users = await _userService.GetAsync();

        return Ok(users);
    }

}