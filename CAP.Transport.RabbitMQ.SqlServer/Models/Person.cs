namespace CAP.Transport.RabbitMQ.SqlServer.Models;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Age { get; set; }

    public override string ToString()
    {
        return "Name:" + Name + ", Age:" + Age;
    }
}