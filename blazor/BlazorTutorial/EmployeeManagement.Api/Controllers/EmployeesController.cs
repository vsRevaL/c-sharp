using EmployeeManagement.Api.Models;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                var created = await _employeeRepository.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = created.EmployeeId }, created);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                {
                    return BadRequest("Employee ID missmatch");
                }
                var employeeToUpdate = _employeeRepository.GetEmployee(id);
                if (employeeToUpdate == null)
                {
                    return NotFound("Employee with Id = " + id + "not found");
                }
                return await _employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var empToDelete = await _employeeRepository.GetEmployee(id);
                if (empToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} is not found.");
                }
                return await _employeeRepository.DeleteEmployee(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

        //https://localhost:7099/api/employees/search?name=John&gender=male
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try
            {
                var result = await _employeeRepository.SearchEmployee(name, gender);
                if (result != null && result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound($"Employee with Name = {name} is not found.");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
    }
}
