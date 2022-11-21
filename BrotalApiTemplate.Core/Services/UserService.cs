using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BrotalApiTemplate.Data.Repositories;
using BrotalApiTemplate.Domain.DTOs;
using BrotalApiTemplate.Domain.Entities;
using BrotalApiTemplate.Domain.Exceptions;
using BrotalApiTemplate.Domain.InputModels;
using FluentValidation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BrotalApiTemplate.Core.Services;

public abstract class UserService : IUserService
{
    protected readonly IUserRepository Repo; 
    protected readonly IMapper Mapper;
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly IValidator<UserRegistrationModel> _registrationModelValidator;
    private readonly IValidator<LoginModel> _loginModelValidator;
    
    protected UserService(
        IUserRepository repo,
        IConfiguration config, 
        IUserStore<User> userStore,
        UserManager<User> userManager, 
        IValidator<LoginModel> loginModelValidator, 
        IValidator<UserRegistrationModel> registrationModelValidator, 
        IMapper mapper)
    {
        Repo                        = repo;
        _config                     = config;
        _userStore                  = userStore;
        _userManager                = userManager;
        _loginModelValidator        = loginModelValidator;
        _registrationModelValidator = registrationModelValidator;
        Mapper                      = mapper;
    }
    
    public abstract Task<UserDto?> GetCurrentUser();

    public async Task<UserDto?> GetByEmail(string email) =>
        Mapper.Map<UserDto>(await Repo.GetByEmail(email));

    public async Task<UserDto?> GetByUserName(string userName) =>
        Mapper.Map<UserDto>(await Repo.GetByUserName(userName));

    public async Task<Result<UserDto>> Register(UserRegistrationModel model)
    {
        var validationResult = await _registrationModelValidator.ValidateAsync(model);

        if (validationResult.IsValid is false)
        {
            return new(new ValidationException(validationResult.Errors));
        }

        var user = new User
        {
            FullName = model.Email
        };

        await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
        await _userStore.SetNormalizedUserNameAsync(user, model.Email, CancellationToken.None);
        await ((IUserEmailStore<User>)_userStore).SetEmailAsync(user, model.Email, CancellationToken.None);
        
        var identityResult = 
            await _userManager.CreateAsync(user, model.Password);

        if (identityResult.Succeeded is false)
        {
            return new(new IdentityException(identityResult.Errors));
        }

        return Mapper.Map<UserDto>(user);
    }
    
    public async Task<Result<JwtSecurityToken>> CreateAccessToken(LoginModel login)
    {
        var validationResult =
            await _loginModelValidator.ValidateAsync(login);

        if (validationResult.IsValid is false)
        {
            return new(new ValidationException(validationResult.Errors));
        }
        
        var user = 
            await Repo.GetByEmail(login.Email);

        var passwordMatches = 
            await _userManager.CheckPasswordAsync(user, login.Password);

        if (passwordMatches is false)
        {
            return new(StandardExceptions.InvalidCredentials);
        }
        
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new[] {    
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),    
            new Claim(JwtRegisteredClaimNames.Email, user.Email),     
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())    
        };  
        
        var token = 
            new JwtSecurityToken(
                issuer: issuer, 
                audience: audience, 
                signingCredentials: credentials,
                claims: claims);
        
        return token;
    }
}