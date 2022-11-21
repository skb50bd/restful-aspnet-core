using BrotalApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrotalApiTemplate.Data.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.HasIndex(_ => _.Email)
            .IsUnique();

        builder.HasIndex(_ => _.UserName)
            .IsUnique();
        
        builder.Property(_ => _.FullName)
            .HasMaxLength(200);
    }
}