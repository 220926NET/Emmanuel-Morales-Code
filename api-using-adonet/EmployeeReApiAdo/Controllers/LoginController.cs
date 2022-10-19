using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
 {

        private readonly ILoginService _loginService; 

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService; 
        }
    
        [HttpPost]
        public ServiceResponse<LoginDto> Post([FromBody] Login loginUser)
        {
            
            ServiceResponse<LoginDto> response = _loginService.Login(loginUser);
            return response; 
         
        }
    // [HttpGet("{id}")]
    // public ServiceResponse<Employee> GetEmployee(int id)
    // {

    //     ServiceResponse<Employees> response = _dbContext.getEmployee(id);
    //     return response; 
    // }

    


}