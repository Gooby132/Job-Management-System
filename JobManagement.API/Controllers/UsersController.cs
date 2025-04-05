using JobManagement.API.Contracts.Users.Requests;
using JobManagement.API.Contracts.Users.Responses;
using JobManagement.API.Helpers;
using JobManagement.Domain.Users;
using JobManagement.Domain.Users.ValueObjects;
using JobManagement.Infrastructure.Authorization.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobManagement.API.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{

    #region Fields

    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _repository;
    private readonly IAuthorizationProvider _authorizationProvider;

    #endregion

    #region Constructor

    public UsersController(
        ILogger<UsersController> logger,
        IUserRepository repository,
        IAuthorizationProvider authorizationProvider)
    {
        _logger = logger;
        _repository = repository;
        _authorizationProvider = authorizationProvider;
    }

    #endregion


    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request,
        CancellationToken token = default)
    {

        var userName = UserName.Create(request.UserName);
        var password = UserPassword.Create(request.Password);

        if (userName.IsFailed)
            return BadRequest(new LoginResponse
            {
                Errors = userName.Errors.ToDtos()
            });

        if (password.IsFailed)
            return BadRequest(new LoginResponse
            {
                Errors = password.Errors.ToDtos()
            });

        var user = await _repository.GetUserByName(userName.Value, token);

        if(user.IsFailed)
            return BadRequest(new LoginResponse
            {
                Errors = user.Errors.ToDtos()
            });

        if (user.Value is null)
            return NotFound(new LoginResponse
            {

            });

        if (!user.Value.IsPasswordsMatch(password.Value))
            return Unauthorized();

        var authToken = _authorizationProvider.AuthorizeAdmin(user.Value);

        return Ok(new LoginResponse
        {
            Token = authToken,
        });

    }

}
