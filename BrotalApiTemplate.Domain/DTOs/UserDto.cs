namespace BrotalApiTemplate.Domain.DTOs;

public record UserDto(
    string Email,
    string FullName,
    string UserName,
    DateTimeOffset JoinedOn);