using AutoMapper;
using BrotalApiTemplate.Core.Services;
using BrotalApiTemplate.Data.Repositories;
using BrotalApiTemplate.Domain.DTOs;
using BrotalApiTemplate.Domain.Entities;
using BrotalApiTemplate.Domain.InputModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BrotalApiTemplate.Api;

public class HttpUserService : UserService, IUserService
{
    private readonly IHttpContextAccessor _httpCtxAccessor;
    
    public HttpUserService(
            IUserRepository repo, 
            IConfiguration config,
            UserManager<User> userManager, 
            IUserStore<User> userStore,
            IValidator<LoginModel> loginModelValidator, 
            IValidator<UserRegistrationModel> registrationModelValidator, 
            IMapper mapper, 
            IHttpContextAccessor httpCtxAccessor) 
        : base(
            repo, 
            config, 
            userStore,
            userManager, 
            loginModelValidator, 
            registrationModelValidator, 
            mapper)
    {
        _httpCtxAccessor = httpCtxAccessor;
    }

    public override async Task<UserDto?> GetCurrentUser()
    {
        var userEmail = 
            _httpCtxAccessor.HttpContext.User.Claims
                .FirstOrDefault(claim =>
                    claim.Type is JwtRegisteredClaimNames.Email)
                ?.Value;

        if (userEmail is null)
        {
            return null;
        }

        var user = await Repo.GetByEmail(userEmail);
        return Mapper.Map<UserDto>(user);
    }
}