using Confluent.Kafka;
using Consumer.DataBase;
using Consumer.DataBase.Entities;
using Consumer.Shared;
using System.Text.Json;

namespace Consumer.Service
{
    public class Worker(ILogger<Worker> logger, KafkaDataBase kafkaDataBase) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly KafkaDataBase _kafkaDataBase = kafkaDataBase;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                //consumer config
                var consumerConfig = new ConsumerConfig()
                {
                    BootstrapServers = "localhost:29092",
                    ClientId="MyConsumerClient",
                    GroupId="MyCompanyConsumerGroup",
                    AutoOffsetReset=AutoOffsetReset.Latest,  
                };

                //consumer builder
                using(var consumer=new ConsumerBuilder<string,string>(consumerConfig).Build())
                {
                    consumer.Subscribe("EmployeeTopic");
                    var consumerData=consumer.Consume(stoppingToken);

                    if (consumerData != null)
                    {
                        var employee=JsonSerializer.Deserialize<Employee>(consumerData.Message.Value);
                        var employeeReport=new EmployeeReport(Guid.NewGuid(), employee.Id,employee.Name,employee.DepartmentName);
                        _logger.LogInformation("you consumed employee");
                        await _kafkaDataBase.Employees.AddAsync(employeeReport);
                        await _kafkaDataBase.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogInformation("there are not messages to be consumed");
                    }
                    consumer.Close();
                }
                //await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
