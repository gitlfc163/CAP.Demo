using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCap(config =>
{
    config.UseInMemoryMessageQueue(); //配置一个消息队列
    config.UseInMemoryStorage(); //配置一个事件存储
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


