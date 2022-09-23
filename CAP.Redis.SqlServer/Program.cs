
//�ѷ�����Ϣ�ͽ��յ���Ϣ�洢��SqlServer��
//���ķ������������ƻ򽻻�·������Կ�洢��Redis
using CAP.Redis.SqlServer.Models;
using DotNetCore.CAP.Messages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSetting = new AppSetting();
builder.Configuration.GetSection("AppSetting").Bind(appSetting);

//��AppSettingʵ��ע�뵽����,�����ڹ��캯����ʹ��IOptionsSnapshot<AppSetting> options
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

builder.Services.AddCap(config =>
{
    config.UseRedis(appSetting.RedisSetting.Connection);

    config.UseSqlServer(appSetting.SqlServerSetting.Connection);

    config.UseDashboard();//����Dashboard

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

app.UseAuthorization();

app.MapControllers();

app.Run();
