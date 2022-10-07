using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectOneApi_v1.Data;
using projectOneApi_v1.Models;
using projectOneApi_v1.Services.EmployeeService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace projectOneApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {

        private readonly IEmployeService _employeeService;

    
        public SignUpController(IEmployeService employeeService){
            _employeeService = employeeService; 
          }



        /*
        [HttpPost]
        public IActionResult Post([FromBody]Employees employee)
        {
            // TODO 
            // check if user exists if so return error status code
            bool containsLogin = _dbContext.Logins.Any(login => login.UserName == employee.Login.UserName);
            if (containsLogin)
            {
                return BadRequest("user already exists");
            }


            Console.WriteLine(employee.Login.UserName);
            Console.WriteLine(employee.Name);
            
            
            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();

            return Ok();
        }
        */
        [HttpPost]
        public ServiceResponse<ActionResult> Post([FromBody] Employees employee)
        {
           ServiceResponse<ActionResult> response = _employeeService.SignUp(employee);

            return response;
        }

        
    }
}
