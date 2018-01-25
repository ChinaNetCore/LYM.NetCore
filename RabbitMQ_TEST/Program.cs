using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ_TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义队列（hello为队列名）
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);

                    // var consumer1 = new QueueingBasicConsumer(channel);

                    channel.BasicConsume("hello", true, consumer);

                    Console.WriteLine(" [*] Waiting for messages." +
                                             "To exit press CTRL+C");
                    while (true)
                    {
                        consumer.Received += Consumer_Received;
                    }
                }
            }
            Console.WriteLine("Hello World");
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {

            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);

        }
    }
}
