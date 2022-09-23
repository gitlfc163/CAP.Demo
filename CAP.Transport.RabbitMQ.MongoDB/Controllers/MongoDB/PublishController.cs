using CAP.Transport.RabbitMQ.MongoDB.Models;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace CAP.Transport.RabbitMQ.MongoDB.Controllers.MongoDB;

/// <summary>
/// 发送消息
/// </summary>
public class PublishController : AreaController
{
    private readonly IMongoClient _client;
    private readonly ICapPublisher _capBus;
    private readonly MongoDbSetting _mongoDbSetting;
    public PublishController(IMongoClient client, ICapPublisher capBus, IOptionsSnapshot<AppSetting> options)
    {
        _client = client;
        _capBus = capBus;
        _mongoDbSetting = options != null ? options.Value.MongoDbSetting:new MongoDbSetting();
    }

    /// <summary>
    /// 发布者-无事务
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult WithoutTransaction()
    {
        _capBus.PublishAsync("sample.rabbitmq.mongodb", DateTime.Now);

        return Ok();
    }

    /// <summary>
    /// 发布者-发布不自动提交
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult PublishNotAutoCommit()
    {
        //注意：MongoDB 不能在事务中创建数据库和集合，所以你需要单独创建它们，模拟一条记录插入则会自动创建
        var mycollection = _client.GetDatabase("test").GetCollection<BsonDocument>("test.collection");
        mycollection.InsertOne(new BsonDocument { { "test", "test" } });

        using (var session = _client.StartTransaction(_capBus, autoCommit: false))
        {
            //var collection = _client.GetDatabase("test").GetCollection<BsonDocument>("test.collection");
            //collection.InsertOne(session, new BsonDocument { { "hello", "world" } });

            _capBus.Publish("sample.rabbitmq.mongodb", DateTime.Now);

            session.CommitTransaction();
        }
        return Ok();
    }

    /// <summary>
    /// 发布者--
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult PublishWithoutTrans()
    {
        //注意：MongoDB 不能在事务中创建数据库和集合，所以你需要单独创建它们，模拟一条记录插入则会自动创建
        var mycollection = _client.GetDatabase("test").GetCollection<BsonDocument>("test.collection");
        mycollection.InsertOne(new BsonDocument { { "test", "test" } });

        using (var session = _client.StartTransaction(_capBus, autoCommit: true))
        {
            //var collection = _client.GetDatabase("test").GetCollection<BsonDocument>("test.collection");
            //collection.InsertOne(session, new BsonDocument { { "hello", "world" } });

            _capBus.Publish("sample.rabbitmq.mongodb", DateTime.Now);
        }

        return Ok();
    }

}
