using EmployeeManagement.Models;

namespace EmployeeManagement.Api.Models
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<IEnumerable<Employee>> SearchEmployee(string name, Gender? gender);
        Task<Employee> GetEmployee(int employeeId);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task<Employee> DeleteEmployee(int employeeId);
    }
}
