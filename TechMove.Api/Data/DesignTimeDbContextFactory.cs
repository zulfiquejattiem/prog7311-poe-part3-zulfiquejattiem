using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TechMove.Api.Data
{
    // Provides a design-time factory for EF Core tools so migrations work regardless of startup project
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Mirror Program.cs connection fallback logic to avoid LocalDB on Linux/containers
            var defaultConn = configuration.GetConnectionString("DefaultConnection");
            var conn = defaultConn;
            if (string.IsNullOrWhiteSpace(conn) || (!OperatingSystem.IsWindows() && conn.Contains("(localdb", StringComparison.OrdinalIgnoreCase)))
            {
                conn = configuration["DockerConnection"]
                       ?? "Server=db;Database=TechMoveDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";
            }

            optionsBuilder.UseSqlServer(conn);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
