using CAP.Transport.RabbitMQ.MySql.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;

namespace CAP.Transport.RabbitMQ.MySql.Controllers.MQ;

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
    /// 发布者
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> WithoutTransaction()
    {
        Console.WriteLine("Publish send message: " + DateTime.Now);
        await _capBus.PublishAsync("sample.rabbitmq.mysql", DateTime.Now);

        return Ok();
    }

    /// <summary>
    /// 发布者-MySqlConnection
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public IActionResult AdonetWithTransaction(Person person)
    {
        if (person == null || person.Id <= 0) return Ok();

        using (var connection = new MySqlConnection(AppDbContext.ConnectionString))
        {
            using (var transaction = connection.BeginTransaction(_capBus, true))
            {
                string sqlstr = string.Format("insert into persons(Id,name) values({0},'{1}')", person.Id, person.Name);
                using (MySqlCommand cmd = new(sqlstr, connection))
                {
                    Console.WriteLine("即将执行SQL语句：   " + sqlstr);
                    int resut = cmd.ExecuteNonQuery();
                }

                //for (int i = 0; i < 5; i++)
                //{
                _capBus.Publish("sample.rabbitmq.mysql", DateTime.Now);
                //}
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
        using (var trans = dbContext.Database.BeginTransaction(_capBus, autoCommit: false))
        {
            dbContext.Persons.Add(new Person() { Id=person.Id,Name = person.Name+"ef" }) ;

            for (int i = 0; i < 1; i++)
            {
                _capBus.Publish("sample.rabbitmq.mysql", DateTime.Now);
            }

            dbContext.SaveChanges();

            trans.Commit();
        }
        return Ok();
    }

}
