using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAP.InMemory.NET6.Controllers.CAP;

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

    [HttpGet]
    //[Route("/send")]
    public IActionResult SendMessage()
    {
        Console.WriteLine("SendMessage message time is:" + DateTime.Now);
        _capBus.Publish("test.show.time", DateTime.Now);

        return Ok();
    }

    [HttpGet]
    //[Route("/send.header")]
    public IActionResult SendMessageHeader()
    {
        var header = new Dictionary<string, string>()
        {
            ["my.header.first"] = "first",
            ["my.header.second"] = "second"
        };

        _capBus.Publish("test.show.time.header", DateTime.Now, header);

        return Ok();
    }
}
