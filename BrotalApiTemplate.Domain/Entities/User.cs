using Microsoft.AspNetCore.Identity;

namespace BrotalApiTemplate.Domain.Entities;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public override Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTimeOffset JoinedOn { get; private set; } = DateTimeOffset.UtcNow;
}