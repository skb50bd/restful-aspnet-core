using System.IdentityModel.Tokens.Jwt;
using BrotalApiTemplate.Domain.DTOs;
using BrotalApiTemplate.Domain.InputModels;
using LanguageExt.Common;

namespace BrotalApiTemplate.Core.Services;

public interface IUserService
{
    Task<UserDto?> GetByEmail(string email);
    Task<UserDto?> GetByUserName(string userName);
    Task<Result<UserDto>> Register(UserRegistrationModel model);
    Task<UserDto?> GetCurrentUser();
    Task<Result<JwtSecurityToken>> CreateAccessToken(LoginModel login);
}