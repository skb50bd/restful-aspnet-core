using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BrotalApiTemplate.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private const string CONNECTION_STRING = "Data Source=/Users/shakibharis/BrotalApiTemplate.db";
    
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlite(CONNECTION_STRING);
        return new AppDbContext(optionsBuilder.Options);
    }
}