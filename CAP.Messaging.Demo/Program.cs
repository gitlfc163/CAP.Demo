using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);

// ��һ�����̳����У�������ʼ״̬Ϊ pending������Ʒ�����ɹ��۳�ʱ��״̬���Ϊ succeeded ������Ϊ failed

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCap(config =>
{
    config.UseInMemoryMessageQueue(); //����һ����Ϣ����
    config.UseInMemoryStorage(); //����һ���¼��洢
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
