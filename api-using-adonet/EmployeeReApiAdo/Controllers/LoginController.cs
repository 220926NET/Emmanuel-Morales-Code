using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
 {

        private readonly DbContext _dbContext = new DbContext(); 

    
        [HttpPost]
        public ServiceResponse<LoginDto> Post([FromBody] Login loginUser)
        {
            
            ServiceResponse<LoginDto> response = _dbContext.Login(loginUser);
            return response; 
         
        }

    // [HttpGet("{id}")]
    // public ServiceResponse<Employee> GetEmployee(int id)
    // {

    //     ServiceResponse<Employees> response = _dbContext.getEmployee(id);
    //     return response; 
    // }

    


}