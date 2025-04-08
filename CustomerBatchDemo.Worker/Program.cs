using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);
    var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(json);

    Console.WriteLine($"📥 Nhận được {customers.Count} khách:");
    foreach (var customer in customers)
    {
        Console.WriteLine($"  ➤ Id: {customer.Id}, Name: {customer.Name}");
    }

    // Giả lập xử lý hệ thống B
    Console.WriteLine("✅ Đã xử lý xong batch.\n");
};

channel.BasicConsume(queue: "customerQueue",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine("📡 Đang lắng nghe tin nhắn từ RabbitMQ...");
Console.WriteLine("Nhấn Ctrl+C để thoát.");
await Task.Delay(-1);
