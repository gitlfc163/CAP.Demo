using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Microsoft.AspNetCore.Mvc;

namespace CAP.Transport.RabbitMQ.MongoDB.Controllers.MongoDB;

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
    [CapSubscribe("sample.rabbitmq.mongodb")]
    public void ReceiveMessage(DateTime time)
    {
        Console.WriteLine($@"{DateTime.Now}, Subscriber invoked, Sent time:{time}");
    }
}
