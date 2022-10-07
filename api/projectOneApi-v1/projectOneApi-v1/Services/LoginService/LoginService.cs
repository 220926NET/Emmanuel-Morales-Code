using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectOneApi_v1.Data;
using projectOneApi_v1.Dtos;
using projectOneApi_v1.Models;

namespace projectOneApi_v1.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly ApiDbContext _dbContext;

        public LoginService(ApiDbContext context)
        {
            _dbContext = context;
        }
        public ServiceResponse<Employees> getEmployee(int id)
        {
            ServiceResponse<Employees> response = new ServiceResponse<Employees>();
            Employees? employee = _dbContext.Employees.Find(id);

            if (employee != null)
            {


                response.Data = employee;
                response.Success = true;
                response.Message = "Found employee";
            } else
            {
                response.Success = false;
                response.Message = "Employee does not exist";
            }


            return response; 

        }

        public ServiceResponse<LoginDto> Login(Logins loginUser)
        {
            ServiceResponse<LoginDto> response = new ServiceResponse<LoginDto>();

            try
            {
                Logins login = _dbContext.Logins.Where(login => login.UserName == loginUser.UserName && login.Password == loginUser.Password).First();
                // return id, name , isManger 

                Employees employeeInfo = (from employee in _dbContext.Employees
                            where employee.Id == login.Id
                            select employee).First();

                LoginDto loginDto = new LoginDto
                {
                    id = employeeInfo.Id,
                    name = employeeInfo.Name,
                    isManager = employeeInfo.IsManager

                };
                

                response.Data = loginDto;
                response.Success = true;
                response.Message = "Successfully logged in";

            }
            catch (InvalidOperationException e)
            {
                response.Success = false;
                response.Message = "User not found"; 
                
            }

            return response; 
        }
    }
}
