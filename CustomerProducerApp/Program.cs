using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

// Tạo 10 khách hàng mẫu
var customers = new List<CustomerDto>();
for (int i = 1; i <= 10; i++)
{
    customers.Add(new CustomerDto
    {
        Id = i,
        Name = $"Customer {i}"
    });
}

// Kết nối RabbitMQ
var factory = new ConnectionFactory()
{
    HostName = "10.19.195.184",
    Port = 5672,
    UserName = "admin",
    Password = "ezJupiterDev@rabbit@1980"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "customerQueue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

// Gửi theo batch 3 khách/lần
int batchSize = 3;
for (int i = 0; i < customers.Count; i += batchSize)
{
    var batch = customers.Skip(i).Take(batchSize).ToList();
    var json = JsonConvert.SerializeObject(batch);
    var body = Encoding.UTF8.GetBytes(json);

    channel.BasicPublish(exchange: "",
                         routingKey: "customerQueue",
                         basicProperties: null,
                         body: body);

    Console.WriteLine($"📤 Đã gửi batch {i / batchSize + 1}: {batch.Count} khách");
}

Console.WriteLine("✅ Gửi xong tất cả khách hàng.");
