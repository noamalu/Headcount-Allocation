using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HeadcountAllocation.DAL{

public class DBcontextFactory : IDesignTimeDbContextFactory<DBcontext>
{
    public DBcontext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // <-- this is what EF tooling uses
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DBcontext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

        return new DBcontext(optionsBuilder.Options);
    }
}
}
