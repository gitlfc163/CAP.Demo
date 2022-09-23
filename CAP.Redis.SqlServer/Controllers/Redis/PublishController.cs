using CAP.Redis.SqlServer.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAP.Redis.SqlServer.Controllers.Redis;

/// <summary>
/// 发送消息
/// </summary>
public class PublishController : AreaController
{
    private readonly ICapPublisher _capBus;
    private readonly ILogger<PublishController> _logger;
    public PublishController(ILogger<PublishController> logger, ICapPublisher capBus)
    {
        _logger = logger;
        _capBus = capBus;
    }
    /// <summary>
    /// 发布者
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task Publish([FromQuery] string message = "test-message")
    {
        await _capBus.PublishAsync(message, new Person() { Age = 11, Name = "James" });
    }
}
