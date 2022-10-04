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
        public IActionResult Post([FromBody] Logins loginUser)
        {
            Console.WriteLine(loginUser.Id);
            Console.WriteLine(loginUser.UserName);
            Console.WriteLine(loginUser.Password);

            List<string> test = new List<string>() { loginUser.UserName, loginUser.Password };

            try
            {
                Logins login = _dbContext.Logins.Where(login => login.UserName == loginUser.UserName && login.Password == loginUser.Password).First();
                Console.WriteLine("user exists");
                Response.Headers.Add("id", login.Id.ToString());
                return Ok();
               
            } catch(Exception e)
            {
                Console.WriteLine("user was not found");
                return BadRequest("user not found");
            }


           
            
         
        }


       
    }
}
