using Confluent.Kafka;
using KafkaImplentation.DataBaseContext;
using KafkaImplentation.DataBaseContext.Models;
using KafkaImplentation.kafkaServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KafkaImplentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(ILogger<EmployeeController> logger, EmployeeContext dataBaseContext,IConfiguration configuration) : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger = logger;
        private readonly EmployeeContext _dataBaseContext = dataBaseContext;
        private readonly IConfiguration configuration = configuration;

        [HttpGet]
        public  async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            _logger.LogInformation("request all employee");
            return await _dataBaseContext.Employees.ToListAsync();  
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(string name,string department)
        {
            var emp=new Employee(new Guid(),name,department);
            await _dataBaseContext.Employees.AddAsync(emp);
            await _dataBaseContext.SaveChangesAsync();

            #region normal way to produce
            //var message = new Message<string, string>()
            //{
            //    Key = emp.Id.ToString(),
            //    Value = JsonSerializer.Serialize(emp)
            //};

            //var producerConfig = new ProducerConfig()
            //{
            //    BootstrapServers = "localhost:29092",
            //    Acks = Acks.All,
            //};

            //var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            //await producer.ProduceAsync("EmployeeTopic", message);
            //producer.Dispose() 
            #endregion; 

            var producer= new KafkaProducer<string, Employee>(configuration);
             await producer.ProduceAsync("EmployeeTopic", emp.Id.ToString(), emp);
            return CreatedAtAction(nameof(CreateEmployee), emp);

        }
    }
}
