using Consumer.DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace Consumer.DataBase
{
    public class KafkaDataBase : DbContext
    {
       public KafkaDataBase(DbContextOptions<KafkaDataBase> options) : base(options) { }

       public DbSet<EmployeeReport> Employees { get; set; }
    }
}
