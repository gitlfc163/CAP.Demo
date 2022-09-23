using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CAP.Filter.Demo.Controllers.Order;

/// <summary>
/// 处理消息
/// </summary>
[ApiController]
public class ConsumerController : ControllerBase
{
    /// <summary>
    /// 订阅方法-商品数量扣除处理
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [NonAction]//表示控制器方法不是动作方法
    [CapSubscribe("place.order.qty.deducted")]
    public object DeductProductQty(JsonElement param)
    {
        Console.WriteLine("商品数量扣除处理被调用");
        var orderId = param.GetProperty("OrderId").GetInt32();
        var productId = param.GetProperty("ProductId").GetInt32();
        var qty = param.GetProperty("Qty").GetInt32();

        //business logic 

        return new { OrderId = orderId, IsSuccess = true };
    }
}
