using CAP.Transport.Kafka.PostgreSql.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Text.Json;

namespace CAP.Transport.Kafka.PostgreSql.Controllers.Kafka;

/// <summary>
/// 发送消息
/// </summary>
public class PublishController : AreaController
{
    private readonly ICapPublisher _capBus;
    private readonly string _connectionString;
    public PublishController(ICapPublisher capBus, IOptionsSnapshot<AppSetting> options)
    {
        _capBus = capBus;
        _connectionString = options == null ? options.Value.PostgreSqlSetting.Connection : "";
    }

    /// <summary>
    /// 发布者
    /// </summary>
    /// <returns></returns>
    [HttpGet]//表示控制器方法不是动作方法
    //[Route("~/without/transaction")]
    public async Task<IActionResult> WithoutTransaction()
    {
        Console.WriteLine("Publish send message: " + DateTime.Now);
        await _capBus.PublishAsync("sample.kafka.postgrsql", DateTime.Now);

        return Ok();
    }

    /// <summary>
    /// 发布者
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Route("~/adonet/transaction")]
    public IActionResult AdonetWithTransaction()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            using var transaction = connection.BeginTransaction(_capBus, autoCommit: false);
            string sqlstr = "insert into test(cname) values('test')";
            using (NpgsqlCommand cmd = new(sqlstr, connection))
            {
                Console.WriteLine("即将执行SQL语句：   " + sqlstr);
                int resut = cmd.ExecuteNonQuery();
            }
            _capBus.Publish("sample.kafka.postgrsql", DateTime.Now);
            transaction.Commit();
        }

        return Ok();
    }
}
