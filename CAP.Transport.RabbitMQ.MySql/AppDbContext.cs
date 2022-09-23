using CAP.Transport.RabbitMQ.MySql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CAP.Transport.RabbitMQ.MySql;


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
    //配置数据库连接
    public static string ConnectionString;

    public DbSet<Person> Persons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
    }
}
