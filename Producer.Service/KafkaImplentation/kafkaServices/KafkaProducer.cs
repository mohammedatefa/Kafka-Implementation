using Confluent.Kafka;
using System.Text.Json;

namespace KafkaImplentation.kafkaServices
{
    public  class KafkaProducer<TKey, TValue>(IConfiguration configuration) where TValue
        : class
    {
        private readonly IConfiguration configuration = configuration;

        public Task ProduceAsync(string topic, TKey key, TValue value)
        {      
            var message = new Message<TKey, string>()
            {
                Key = key,
                Value = JsonSerializer.Serialize(value)
            };
            var producerConfig=new ProducerConfig()
            {
                BootstrapServers = configuration["KafkaConfiguration:BootstrapServers"],
                ////SaslMechanism = Enum.Parse<SaslMechanism>(configuration["KafkaConfiguration:SecurityProtocol"]),
                //SaslUsername = configuration["KafkaConfiguration:SaslUsername"],
                //SaslPassword = configuration["KafkaConfiguration:SaslPassword"],
                Acks =Acks.All
            };
            var produce=new ProducerBuilder<TKey,string>(producerConfig).Build();
            produce.ProduceAsync(topic, message);
            produce.Flush();
            produce.Dispose();
            return Task.CompletedTask;
        }
    }
}
