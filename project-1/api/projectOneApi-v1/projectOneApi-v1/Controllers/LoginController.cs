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

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] User loginUser)
        {
            Console.WriteLine(loginUser.id);
            Console.WriteLine(loginUser.userName);
            Console.WriteLine(loginUser.password);

            List<string> test = new List<string>() { loginUser.userName, loginUser.password };

            try
            {
                User user = _dbContext.Users.Where(u => u.userName == loginUser.userName && u.password == loginUser.password).First();
                Console.WriteLine("user exists");
                Response.Headers.Add("id", user.id.ToString());
                return Ok();
               
            } catch(Exception e)
            {
                Console.WriteLine("user was not found");
                return BadRequest("user not found");
            }


           
            
         
        }


       
    }
}
