using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAP.InMemory.Demo.Controllers.CAP;

/// <summary>
/// 处理消息
/// </summary>
[ApiController]
public class ConsumerController : ControllerBase
{
    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="time"></param>
    [NonAction]
    [CapSubscribe("test.show.time")]
    public void ReceiveMessage(DateTime time)
    {
        Console.WriteLine("ReceiveMessage message time is:" + time);
    }

    /// <summary>
    /// 处理包含头信息的消息
    /// </summary>
    /// <param name="time"></param>
    [NonAction]
    [CapSubscribe("test.show.time.header")]
    public void ReceiveMessage(DateTime time, [FromCap] CapHeader header)
    {
        Console.WriteLine("ReceiveMessageHeader message time is:" + time);
        Console.WriteLine("message firset header :" + header["my.header.first"]);
        Console.WriteLine("message second header :" + header["my.header.second"]);
    }
}
