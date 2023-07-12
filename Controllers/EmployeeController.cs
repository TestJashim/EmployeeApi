using EmployeeApi.Data;
using EmployeeApi.Models.ViewModels;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeContext _dbContext;
        public EmployeeController(EmployeeContext dbContext)
        {
             _dbContext = dbContext;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _dbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromForm] EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new Employee entity
                var employee = new Employee
                {
                    Name = model.Name,
                    Age = model.Age,
                    Gender = model.Gender,
                    Department = model.Department,
                    IsFullTime = model.IsFullTime,
                    HireDate = model.HireDate
                };

                // Save the image to a file
                using (var stream = new FileStream(Path.Combine(@"Images", model.Image.FileName), FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                // Set the image file name on the employee entity
                employee.Image = model.Image.FileName;

                // Add the employee to the database
                _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }

            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }


        #region Update New Image But Also Keep Old Image

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateEmployee(int id, [FromForm] EmployeeViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Find the employee in the database
        //        var employee = await _dbContext.Employees.FindAsync(id);
        //        if (employee == null)
        //        {
        //            return NotFound();
        //        }

        //        // Update the employee properties with the provided data
        //        employee.Name = model.Name;
        //        employee.Age = model.Age;
        //        employee.Gender = model.Gender;
        //        employee.Department = model.Department;
        //        employee.IsFullTime = model.IsFullTime;
        //        employee.HireDate = model.HireDate;

        //        // If a new image was provided, save it to a file and update the employee entity
        //        if (model.Image != null)
        //        {
        //            // Generate a unique file name
        //            var fileName = "employee-" + id + "-" + model.Image.FileName;

        //            // Save the image to a file
        //            using (var stream = new FileStream(Path.Combine(@"Images", fileName), FileMode.Create))
        //            {
        //                await model.Image.CopyToAsync(stream);
        //            }

        //            // Set the new image file name on the employee entity
        //            employee.Image = fileName;
        //        }

        //        // Save the changes to the database
        //        _dbContext.Employees.Update(employee);
        //        await _dbContext.SaveChangesAsync();

        //        return Ok();
        //    }

        //    return BadRequest(ModelState);
        //}
        #endregion

        // To update the image for an employee and delete the old image from the uploads folder, you can modify the UpdateEmployee action as follows:

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromForm] EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the employee in the database
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                // Update the employee properties with the provided data
                employee.Name = model.Name;
                employee.Age = model.Age;
                employee.Gender = model.Gender;
                employee.Department = model.Department;
                employee.IsFullTime = model.IsFullTime;
                employee.HireDate = model.HireDate;

                // If a new image was provided, save it to a file and update the employee entity
                if (model.Image != null)
                {
                    // Generate a unique file name
                    var fileName = "employee-" + id + "-" + model.Image.FileName;

                    // Save the image to a file
                    using (var stream = new FileStream(Path.Combine(@"Images", fileName), FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    // Delete the old image file from the uploads folder
                    if (System.IO.File.Exists(Path.Combine(@"Images", employee.Image)))
                    {
                        System.IO.File.Delete(Path.Combine(@"Images", employee.Image));
                    }

                    // Set the new image file name on the employee entity
                    employee.Image = fileName;
                }

                // Save the changes to the database
                _dbContext.Employees.Update(employee);
                await _dbContext.SaveChangesAsync();

                return Ok();
            }

            return BadRequest(ModelState);
        }


        [HttpGet]
        [Route("SearchEmployee")]
        public async Task<IActionResult> SearchEmployees(string searchString)
        {
            var employees = from e in _dbContext.Employees
                            select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Name.Contains(searchString) || e.Image.Contains(searchString));
            }

            return Ok(await employees.ToListAsync());
        }
    }
}
