using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase {

    
    private IEmployeeService _employeeService; 
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService; 
    }
    

    [HttpPost]
    [Route("addEmployeeDetails")]
    public ActionResult<ServiceResponse<string>> addEmployeeDetails(EmployeeDetailsDto employeeDetails)
    {
        ServiceResponse<string> response = new ServiceResponse<string>();

        response = _employeeService.addEmployeeDetails(employeeDetails); 

        return response; 
        
    }




    
    

}