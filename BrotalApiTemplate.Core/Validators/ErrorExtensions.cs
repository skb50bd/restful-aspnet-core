using BrotalApiTemplate.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BrotalApiTemplate.Core.Validators;

public static class ErrorExtensions
{
    private const string BadRequestDesc = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    private const int BadRequestCode = 400;
    
    public static ValidationProblemDetails ToProblemDetails(
        this ValidationException exception)
    {
        var error = new ValidationProblemDetails
        {
            Type = BadRequestDesc,
            Status = BadRequestCode
        };

        foreach (var validationFailure in exception.Errors)  
        {
            if (error.Errors.ContainsKey(validationFailure.PropertyName))
            {
                error.Errors[validationFailure.PropertyName] =
                    error.Errors[validationFailure.PropertyName]
                        .Concat(new[] { validationFailure.ErrorMessage })
                        .ToArray();
            }
            else
            {
                error.Errors.Add(
                    new KeyValuePair<string, string[]>(
                        validationFailure.PropertyName, 
                        new[] { validationFailure.ErrorMessage })    
                );
            }
        }

        return error;
    }

    public static ValidationProblemDetails ToProblemDetails(
        this IdentityException exception)
    {
        var error = new ValidationProblemDetails
        {
            Type = BadRequestDesc,
            Status = BadRequestCode
        };

        foreach (var identityError in exception.Errors)  
        {
            if (error.Errors.ContainsKey(identityError.Code))
            {
                error.Errors[identityError.Code] =
                    error.Errors[identityError.Code]
                        .Concat(new[] { identityError.Description })
                        .ToArray();
            }
            else
            {
                error.Errors.Add(
                    new KeyValuePair<string, string[]>(
                        identityError.Code, 
                        new[] { identityError.Description })    
                );
            }
        }

        return error;
    }
}