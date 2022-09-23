using CAP.Redis.SqlServer.Models;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Microsoft.AspNetCore.Mvc;

namespace CAP.Redis.SqlServer.Controllers.Redis;

/// <summary>
/// 处理消息
/// </summary>
[ApiController]
public class ConsumerController : ControllerBase
{
    private readonly ILogger<ConsumerController> _logger;
    public ConsumerController(ILogger<ConsumerController> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// 订阅消费者-存储在Redis里
    /// </summary>
    /// <param name="value"></param>
    [NonAction]
    [CapSubscribe("test-message")]
    [CapSubscribe("test-message-1")]
    [CapSubscribe("test-message-2")]
    [CapSubscribe("test-message-3")]
    public void Subscribe(Person p, [FromCap] CapHeader header)
    {
        _logger.LogInformation($"{header[Headers.MessageName]} subscribed with value --> " + p);
    }
}
