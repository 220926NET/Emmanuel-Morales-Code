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
        public ActionResult<ServiceResponse<string>> Post([FromBody] Login loginUser)
        {
          
            ServiceResponse<string> response = _loginService.Login(loginUser);

            if(response.Success){
                return Ok(response); 
            } else 
            {
                return Ok(response); 
            }
        
         
        }



    // [HttpGet("{id}")]
    // public ServiceResponse<Employee> GetEmployee(int id)
    // {

    //     ServiceResponse<Employees> response = _dbContext.getEmployee(id);
    //     return response; 
    // }

    


}