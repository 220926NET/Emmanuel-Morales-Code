using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectOneApi_v1.Data;
using projectOneApi_v1.Models;

namespace projectOneApi_v1.Services.EmployeeService
{
    public class EmployeeService : IEmployeService
    {

        private readonly ApiDbContext _dbContext;
        public EmployeeService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
     

        public ServiceResponse<ActionResult> SignUp(Employees employee)
        {
            ServiceResponse<ActionResult> response = new ServiceResponse<ActionResult>();

            bool containsLogin = _dbContext.Logins.Any(login => login.UserName == employee.Login.UserName);
            if (!containsLogin)
            {
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges();
                response.Message = "Succesfully created user";
            } else
            {

                response.Message = "User already exists";
                response.Success = false; 
            }

            return response; 
 
            
        }
    }
}
