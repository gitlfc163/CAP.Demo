using DotNetCore.CAP.Filter;

namespace CAP.Filter.Demo.Filter;

public class MyCapFilter : SubscribeFilter
{
    public override void OnSubscribeExecuting(ExecutingContext context)
    {
        // 订阅方法执行前
        Console.WriteLine("订阅方法执行前");
    }

    public override void OnSubscribeExecuted(ExecutedContext context)
    {
        // 订阅方法执行后
        Console.WriteLine("订阅方法执行后");
    }

    public override void OnSubscribeException(ExceptionContext context)
    {
        // 订阅方法执行异常
        Console.WriteLine("订阅方法执行异常");

        //忽略异常
        //context.ExceptionHandled = true;
    }

}

