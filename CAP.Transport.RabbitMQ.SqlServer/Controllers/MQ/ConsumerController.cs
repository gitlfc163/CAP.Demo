using CAP.Transport.RabbitMQ.SqlServer.Models;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Microsoft.AspNetCore.Mvc;

namespace CAP.Transport.RabbitMQ.SqlServer.Controllers.MQ;

/// <summary>
/// 处理消息
/// </summary>
[ApiController]
public class ConsumerController : ControllerBase
{

    /// <summary>
    /// 订阅消费者-
    /// </summary>
    /// <param name="value"></param>
    [NonAction]
    [CapSubscribe("sample.rabbitmq.sqlserver")]
    public void Subscriber(Person p)
    {
        Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
    }

    /// <summary>
    /// 订阅消费者-EF
    /// </summary>
    /// <param name="p"></param>
    /// <param name="header"></param>
    [NonAction]
    [CapSubscribe("sample.rabbitmq.sqlserver", Group = "group.test2")]
    public void Subscriber2(Person p, [FromCap] CapHeader header)
    {
        var id = header[Headers.MessageId];

        Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
    }
}
