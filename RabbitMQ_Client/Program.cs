using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义队列（hello为队列名）
                    channel.QueueDeclare("hello", false, false, false, null);
                    
                    //发送到队列的消息，包含时间戳
                    string message = "Hello World!" + "_" + DateTime.Now.ToString();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", "hello", null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}
