using EmployeeManagement.Models;
using Microsoft.AspNetCore.Components;

namespace EmployeeManagement.Web.Pages
{
    public class DataBindingDemoBase : ComponentBase
    {
        public string Name { get; set; } = "John";
        public string Gender { get; set; } = "Male";
        public string Description { get; set; } = "";
    }
}
