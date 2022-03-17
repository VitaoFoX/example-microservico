using GeekShopping.Email.Messages;
using GeekShopping.Email.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GeekShopping.Email.MessageConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly EmailRepository _repository;
        private IConnection _connection;
        private IModel _channel;
        private const string ExchangeName = "DirectPaymentUpdateExchange";
        private const string PaymentEmailUpdateQueueName = "PaymentEmailUpdateQueueName";

        public RabbitMQPaymentConsumer(EmailRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(PaymentEmailUpdateQueueName, false, false, false, null);
            _channel.QueueBind(PaymentEmailUpdateQueueName, ExchangeName, "PaymentEmail");


            //ModoFanout
            //_connection = factory.CreateConnection();
            //_channel = _connection.CreateModel();
            //_channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            //queueName = _channel.QueueDeclare().QueueName;
            //_channel.QueueBind(queueName, ExchangeName, "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (channel, ev) =>
            {
                var content = Encoding.UTF8.GetString(ev.Body.ToArray()); //Transformando o array dem string
                UpdatePaymentResultMessage message = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);
                ProcessLogs(message).GetAwaiter().GetResult();
                _channel.BasicAck(ev.DeliveryTag, false); //Remover a mensagem de lista
            };

            _channel.BasicConsume(PaymentEmailUpdateQueueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessLogs(UpdatePaymentResultMessage message)
        {
            try
            {
                await _repository.LogEmail(message);
            }
            catch (Exception)
            {
                //Log
                throw;
            }


        }
    }
}
