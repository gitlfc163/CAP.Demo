using Microsoft.AspNetCore.Mvc;

namespace CAP.Transport.RabbitMQ.MySql.Controllers;

/// <summary>
/// 基础控制器
/// </summary>
[Route("api/[area]/[controller]/[action]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
}