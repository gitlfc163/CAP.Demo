using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAP.Transport.RabbitMQ.MySql.Controllers.MQ;

/// <summary>
/// 处理消息
/// </summary>
[ApiController]
public class ConsumerController : ControllerBase
{
    /// <summary>
    /// 订阅消费者-MySqlConnection
    /// </summary>
    /// <param name="value"></param>
    [NonAction]
    [CapSubscribe("sample.rabbitmq.mysql")]
    public void Subscriber(DateTime p)
    {
        Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
    }

    /// <summary>
    /// 订阅消费者-EF
    /// </summary>
    /// <param name="p"></param>
    /// <param name="header"></param>
    [NonAction]
    [CapSubscribe("sample.rabbitmq.mysql", Group = "group.test2")]
    public void Subscriber2(DateTime p, [FromCap] CapHeader header)
    {
        Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
    }
}
