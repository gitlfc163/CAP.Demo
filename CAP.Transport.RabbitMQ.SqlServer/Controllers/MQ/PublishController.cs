
using CAP.Transport.RabbitMQ.SqlServer.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CAP.Transport.RabbitMQ.SqlServer.Controllers.MQ;

/// <summary>
/// 发送消息
/// </summary>
public class PublishController : AreaController
{
    ICapPublisher _capBus;
    public PublishController(ICapPublisher capBus)
    {
        _capBus = capBus;
    }

    /// <summary>
    /// 发布者-
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> WithoutTransaction()
    {
        await _capBus.PublishAsync("sample.rabbitmq.sqlserver", new Person()
        {
            Id = 123,
            Name = "Bar"
        });

        return Ok();
    }

    /// <summary>
    /// 发布者-SqlConnection
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult AdonetWithTransaction(Person person)
    {
        using (var connection = new SqlConnection(AppDbContext.ConnectionString))
        {
            using (var transaction = connection.BeginTransaction(_capBus, true))
            {
                //your business code
                string sqlstr = string.Format("insert into persons(Id,name) values({0},'{1}')", person.Id, person.Name);
                using (SqlCommand cmd = new(sqlstr, connection))
                {
                    Console.WriteLine("即将执行SQL语句：   " + sqlstr);
                    int resut = cmd.ExecuteNonQuery();
                }

                _capBus.Publish("sample.rabbitmq.sqlserver", new Person()
                {
                    Id = 123,
                    Name = "Bar"
                });


            }
        }

        return Ok();
    }

    /// <summary>
    /// 发布者-EF
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult EntityFrameworkWithTransaction([FromServices] AppDbContext dbContext, Person person)
    {
        using (dbContext.Database.BeginTransaction(_capBus, autoCommit: true))
        {
            dbContext.Persons.Add(new Person() { Id = person.Id, Name = person.Name + "ef" });
            dbContext.SaveChanges();
            _capBus.Publish("sample.rabbitmq.sqlserver", new Person()
            {
                Id = 123,
                Name = "Bar"
            });
        }
        return Ok();
    }
}
