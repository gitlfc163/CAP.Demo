using CAP.Transport.RabbitMQ.SqlServer.Models;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CAP.Transport.RabbitMQ.SqlServer;

public class AppDbContext : DbContext
{
    /// <summary>
    /// AppDbContext
    /// builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(IOptionsSnapshot<AppSetting> options)
    {
        ConnectionString = options != null ? options.Value.SqlServerSetting.Connection : "";
    }
    public static string ConnectionString;

    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString);
    }
}
