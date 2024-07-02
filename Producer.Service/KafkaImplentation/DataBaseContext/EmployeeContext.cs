using KafkaImplentation.DataBaseContext.Models;
using Microsoft.EntityFrameworkCore;

namespace KafkaImplentation.DataBaseContext
{
    public class EmployeeContext:DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
