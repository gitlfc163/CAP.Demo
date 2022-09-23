using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAP.Transport.Kafka.PostgreSql.Controllers.Kafka;

/// <summary>
/// 处理消息
/// </summary>
[ApiController]
public class ConsumerController : ControllerBase
{
    /// <summary>
    /// 订阅消费者
    /// </summary>
    /// <param name="value"></param>
    [NonAction]//表示控制器方法不是动作方法
    [CapSubscribe("sample.kafka.postgrsql")]
    public void TestKafka(DateTime value)
    {
        Console.WriteLine("Subscriber output message: " + value);
    }
}
