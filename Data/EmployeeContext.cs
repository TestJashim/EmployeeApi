using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id=-1,Name="Admin", Age=34, Gender="Male", Department="SI", IsFullTime=true, HireDate=new DateTime(2023, 01, 01), Image="/Images/default.jpg", ImageFile= null }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
