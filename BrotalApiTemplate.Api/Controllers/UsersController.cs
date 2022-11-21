using System.IdentityModel.Tokens.Jwt;
using BrotalApiTemplate.Core.Services;
using BrotalApiTemplate.Core.Validators;
using BrotalApiTemplate.Domain.DTOs;
using BrotalApiTemplate.Domain.Exceptions;
using BrotalApiTemplate.Domain.InputModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrotalApiTemplate.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public Task<ActionResult<UserDto>> Get() =>
        _userService
            .GetCurrentUser()
            .MatchAsync(
                user => Ok(user),
                () => Unauthorized()
            );

    [HttpPost("Register")]
    public Task<ActionResult<UserDto>> Register(UserRegistrationModel model) =>
        _userService
            .Register(model)
            .MatchAsync(
                user => CreatedAtAction(nameof(Get), new { }, user),
                exception => exception switch
                {
                    ValidationException ex => BadRequest(ex.ToProblemDetails()),
                    IdentityException ex   => BadRequest(ex.ToProblemDetails()),
                    _                      => StatusCode(500)
                }
            );

    [HttpPost("Token")]
    public Task<ActionResult<string>> GetToken(LoginModel model) =>
        _userService
            .CreateAccessToken(model)
            .MatchAsync<JwtSecurityToken, string>(
                token =>
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenString  = tokenHandler.WriteToken(token);
                    return Ok(tokenString);
                },
                exception => exception switch
                {
                    ValidationException ex => BadRequest(ex.ToProblemDetails()),
                    InvalidCredentials     => Unauthorized(),
                    _                      => StatusCode(500)
                }
            );
}