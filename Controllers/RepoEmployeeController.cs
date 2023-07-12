using EmployeeApi.DAL.Interfaces;
using EmployeeApi.Models.ViewModels;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepoEmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RepoEmployeeController(IEmployeeRepository repository, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _repository.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet]
        [Route("SearchEmployeeRepo")]
        public async Task<IActionResult> SearchEmployees(string searchString)
        {
            var employees = await _repository.SearchEmployees(searchString);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromForm] EmployeeViewModel model)
        {
            // Validate the model and save the employee
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the view model properties to the employee entity
            var employee = new Employee
            {
                Name = model.Name,
                Age = model.Age,
                Gender = model.Gender,
                Department = model.Department,
                IsFullTime = model.IsFullTime,
                HireDate = model.HireDate
            };

            // If a new image was provided, save it to a file and set the file name on the employee entity
            if (model.Image != null && model.Image.Length > 0)
            {
                // Generate a unique file name
                var fileName = "employee-" + Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

                // Save the image to a file
                using (var stream = new FileStream(Path.Combine(@"Images", fileName), FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                employee.Image = fileName;
            }
            else
            {
                // Set a default image if no image was provided
                employee.Image = "default.jpg";
            }

            employee = await _repository.AddEmployee(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromForm] EmployeeViewModel model)
        {
            // Validate the model and update the employee
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Map the view model properties to the employee entity
            employee.Name = model.Name;
            employee.Age = model.Age;
            employee.Gender = model.Gender;
            employee.Department = model.Department;
            employee.IsFullTime = model.IsFullTime;
            employee.HireDate = model.HireDate;

            // If a new image was provided, save it to a file and set the file name on the employee entity
            if (model.Image != null && model.Image.Length > 0)
            {
                // Generate a unique file name
                var fileName = "employee-" + employee.Id + "-" + Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

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

            employee = await _repository.UpdateEmployee(employee);
            return Ok(employee);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Delete the employee entity and the corresponding image file from the Images folder
            await _repository.DeleteEmployee(id);
            if (System.IO.File.Exists(Path.Combine(@"Images", employee.Image)))
            {
                System.IO.File.Delete(Path.Combine(@"Images", employee.Image));
            }

            return NoContent();
        }

    }
}
