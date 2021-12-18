using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Sample.Data;

namespace Sample.App
{
    partial class Program : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder();
            var configuration = builder.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            var connectionString = configuration.GetConnectionString(CONNECTION_STRING_DEFAULT);

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(typeof(Sample.Data.SqlServer.PlaceHolder).Assembly.FullName);
            });

            var dbContextOptions = dbContextOptionsBuilder.Options;

            return new AppDbContext(dbContextOptions);
        }
    }
}
