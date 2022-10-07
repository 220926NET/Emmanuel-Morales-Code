using Microsoft.AspNetCore.Mvc;
using projectOneApi_v1.Dtos;
using projectOneApi_v1.Models;

namespace projectOneApi_v1.Services
{
    public interface ILoginService
    {

        public ServiceResponse<LoginDto> Login(Logins loginUser);

        public ServiceResponse<Employees> getEmployee(int id);

    }
}
