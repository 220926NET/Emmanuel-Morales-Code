using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectOneApi_v1.Data;
using projectOneApi_v1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace projectOneApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public LoginController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Logins loginUser)
        {
            Console.WriteLine(loginUser.Id);
            Console.WriteLine(loginUser.UserName);
            Console.WriteLine(loginUser.Password);

            try
            {
                Logins login = _dbContext.Logins.Where(login => login.UserName == loginUser.UserName && login.Password == loginUser.Password).First();
                Response.Headers.Add("id", login.Id.ToString());

                return Ok();
               
            } catch(InvalidOperationException e)
            {
                return NotFound("user not found");
            }
         
        }

        [HttpGet("{id}")]

        public IActionResult Get(int id)
        {
            
            Employees? employee = _dbContext.Employees.Find(id);
            
            if(employee != null)
            {
                Console.WriteLine(employee.Name);
                Console.WriteLine(employee.IsManager);
                Console.WriteLine(employee.Id);
                Console.WriteLine(employee.Login);
              
                return Ok(employee);
            }


            return BadRequest(); 

        }

    }

    
}
