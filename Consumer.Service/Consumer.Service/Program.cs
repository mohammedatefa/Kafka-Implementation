using Consumer.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Consumer.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
         
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddDbContext<KafkaDataBase>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("KafkaContext"));
            },ServiceLifetime.Singleton);
            var host = builder.Build();
            host.Run();
        }
    }
}