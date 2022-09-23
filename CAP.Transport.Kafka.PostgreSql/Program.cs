using CAP.Transport.Kafka.PostgreSql.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSetting = new AppSetting();
builder.Configuration.GetSection("AppSetting").Bind(appSetting);

//把AppSetting实体注入到容器,方便在构造函数里使用IOptionsSnapshot<AppSetting> options
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

builder.Services.AddCap(config =>
{
    config.UsePostgreSql(opt => { 
        opt.ConnectionString = appSetting.PostgreSqlSetting.Connection; 
    }); //配置一个PostgreSql

    config.UseKafka(opt => {
        opt.Servers = appSetting.KafkaSetting.Servers;
        //KafkaOptions
    });//添加基于 Kafka 的配置项

    config.UseDashboard();//配置一个Dashboard
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
