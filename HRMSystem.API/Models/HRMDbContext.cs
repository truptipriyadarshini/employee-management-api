using Microsoft.EntityFrameworkCore;

namespace HRMSystem.API.Models
{
    public class HRMDbContext:DbContext
    {
        public HRMDbContext(DbContextOptions<HRMDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
