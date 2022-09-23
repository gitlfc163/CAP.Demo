using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CAP.Filter.Demo.Controllers.Order;

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
    /// 订单处理示例
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult PublisherMessage()
    {
        Console.WriteLine("订单处理示例");
        //商品数量扣除调用模拟的ConsumerController的place.order.qty.deducted
        _capBus.Publish("place.order.qty.deducted",
                         contentObj: new { OrderId = 1234, ProductId = 23255, Qty = 1 },
                         //当商品数量成功扣除时将状态标记为 succeeded ，否则为 failed
                         callbackName: "place.order.mark.status");

        return Ok();
    }

    /// <summary>
    /// 当商品数量成功扣除时将状态标记为 succeeded ，否则为 failed
    /// </summary>
    /// <param name="param"></param>
    [NonAction]//表示控制器方法不是动作方法
    [CapSubscribe("place.order.mark.status")]
    public void MarkOrderStatus(JsonElement param)
    {
        var orderId = param.GetProperty("OrderId").GetInt32();
        var isSuccess = param.GetProperty("IsSuccess").GetBoolean();

        if (isSuccess)
        {
            // mark order status to succeeded
        }
        else
        {
            // mark order status to failed
        }
    }
}
