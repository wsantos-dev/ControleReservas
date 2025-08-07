using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ControleReservas.Infrastructure.Persistence;

public class ControleReservasDbContextFactory : IDesignTimeDbContextFactory<ControleReservasDbContext>
{
    public ControleReservasDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "ControleReservas.API"
        );


        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("ControleReservasConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ControleReservasDbContext>();
        optionsBuilder.UseSqlServer(connectionString); 

        return new ControleReservasDbContext(optionsBuilder.Options);
    }
}
