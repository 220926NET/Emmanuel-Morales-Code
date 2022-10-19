using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase {

    private readonly DbContext _dbContext = new DbContext(); 
    

    [HttpPost]
     public ActionResult<ServiceResponse<string>> SignUp(Employee employee)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            response = _dbContext.CreateUser(employee); 

            return response; 
            
        }
    
    

}