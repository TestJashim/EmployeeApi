using EmployeeApi.DAL.Interfaces;
using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _dbContext;
        private readonly string images;

        public EmployeeRepository(EmployeeContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            this._dbContext = dbContext;

            if (hostingEnvironment != null && hostingEnvironment.WebRootPath != null)
            {
                this.images = Path.Combine(hostingEnvironment.WebRootPath, "Images");
            }

        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task<List<Employee>> SearchEmployees(string searchString)
        {
            var employees = from e in _dbContext.Employees
                            select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Name.Contains(searchString) || e.Image.Contains(searchString));
            }

            return await employees.ToListAsync();
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
