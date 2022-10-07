using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectOneApi_v1.Data;
using projectOneApi_v1.Dtos;
using projectOneApi_v1.Models;
using projectOneApi_v1.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace projectOneApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;

        }

        [HttpPost]
        public ServiceResponse<LoginDto> Post([FromBody] Logins loginUser)
        {
            ServiceResponse<LoginDto> response = _loginService.Login(loginUser);
            return response; 

         
        }

        [HttpGet("{id}")]

        public ServiceResponse<Employees> Get(int id)
        {

            ServiceResponse<Employees> response = _loginService.getEmployee(id);
            return response; 
        }

    }

    
}
