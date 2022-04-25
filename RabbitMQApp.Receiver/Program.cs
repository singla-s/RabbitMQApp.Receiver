using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQApp.Receiver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "newBooks",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    var myConsumer = new EventingBasicConsumer(channel);

                    //event handler - callback when a message is received

                    myConsumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.Span);
                        Console.WriteLine($"Message Received {message}");
                    };

                    channel.BasicConsume(queue: "newBooks", autoAck: true, consumer: myConsumer);
                    Console.WriteLine("Hit <Enter> to Exit.");
                    Console.ReadLine();

                }
        }
    }
}
