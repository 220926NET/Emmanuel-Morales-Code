using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectOneApi_v1.Data;
using projectOneApi_v1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace projectOneApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {

        public  ApiDbContext _dbContext;

        public SignUpController(ApiDbContext dbContext){
            _dbContext = dbContext; 
            }

        

        // POST api/<SignUpController>
        [HttpPost]
        public void Post([FromBody]Employees employee)
        {
            Console.WriteLine(employee.user.userName);
            Console.WriteLine(employee.name);
            
            Employees newEmployee = new Employees() { name = employee.name };

            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
        }


    }
}
