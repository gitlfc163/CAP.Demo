using CAP.Transport.Kafka.PostgreSql.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSetting = new AppSetting();
builder.Configuration.GetSection("AppSetting").Bind(appSetting);

//��AppSettingʵ��ע�뵽����,�����ڹ��캯����ʹ��IOptionsSnapshot<AppSetting> options
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

builder.Services.AddCap(config =>
{
    config.UsePostgreSql(opt => { 
        opt.ConnectionString = appSetting.PostgreSqlSetting.Connection; 
    }); //����һ��PostgreSql

    config.UseKafka(opt => {
        opt.Servers = appSetting.KafkaSetting.Servers;
        //KafkaOptions
    });//��ӻ��� Kafka ��������

    config.UseDashboard();//����һ��Dashboard
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
