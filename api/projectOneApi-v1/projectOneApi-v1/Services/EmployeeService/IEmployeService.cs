using Microsoft.AspNetCore.Mvc;
using projectOneApi_v1.Models;
namespace projectOneApi_v1.Services.EmployeeService
 
{
    public interface IEmployeService
    {
        public ServiceResponse<ActionResult> SignUp(Employees employee);

    }
}
    