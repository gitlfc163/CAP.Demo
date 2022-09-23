
using CAP.Transport.RabbitMQ.MongoDB;
using CAP.Transport.RabbitMQ.MongoDB.Models;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Messages;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSetting = new AppSetting();
builder.Configuration.GetSection("AppSetting").Bind(appSetting);

//把AppSetting实体注入到容器,方便在构造函数里使用IOptionsSnapshot<AppSetting> options
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

//MongoDB 的客户端接口
builder.Services.AddSingleton<IMongoClient>(new MongoClient(appSetting.MongoDbSetting.Connection));

builder.Services.AddCap(config =>
{
    config.UseMongoDB(appSetting.MongoDbSetting.Connection);
    config.UseRabbitMQ(o =>
    {
        o.HostName = appSetting.RabbitMQSetting.HostName;
        o.UserName = appSetting.RabbitMQSetting.UserName;
        o.Password = appSetting.RabbitMQSetting.Password;
        o.Port = appSetting.RabbitMQSetting.Port;
        o.CustomHeaders = e => new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>(Headers.MessageId, SnowflakeId.Default().NextId().ToString()),
            new KeyValuePair<string, string>(Headers.MessageName, e.RoutingKey),
        };
        o.ConnectionFactoryOptions = opt => {
            //rabbitmq client ConnectionFactory config
        };
    });//配置RabbitMQ

    config.UseDashboard();//配置Dashboard

    config.FailedRetryCount = 5;
    config.FailedThresholdCallback = failed =>
    {
        var logger = failed.ServiceProvider.GetService<ILogger<Program>>();
        logger?.LogError($@"A message of type {failed.MessageType} failed after executing {config.FailedRetryCount} several times,requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();