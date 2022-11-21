using Microsoft.AspNetCore.Identity;

namespace BrotalApiTemplate.Domain.Exceptions;

public static class StandardExceptions
{
    public static InvalidCredentials InvalidCredentials { get; } = new();
}

public class InvalidCredentials : Exception { }

public class IdentityException : Exception
{
    public List<IdentityError> Errors { get; private set; }

    public IdentityException(IEnumerable<IdentityError> errors)
    {
        Errors = errors.ToList();
    }
}