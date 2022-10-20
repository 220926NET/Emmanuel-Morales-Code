using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase {

    private readonly DbContext _dbContext = new DbContext(); 
    
    private IRegisterService _registerService; 
    public RegisterController(IRegisterService registerService)
    {
        _registerService = registerService; 
        
    }

    [HttpPost]
    public ActionResult<ServiceResponse<string>> SignUp(RegisterAccountDto newEmployee)
    {
        ServiceResponse<string> response = new ServiceResponse<string>();

        response = _registerService.SignUp(newEmployee); 

        return response; 
        
    }




    
    

}