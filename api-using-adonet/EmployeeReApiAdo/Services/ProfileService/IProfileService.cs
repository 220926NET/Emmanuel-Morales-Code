
using System.Security.Claims;
public interface IProfileService
{

    ServiceResponse<string> addEmployeeDetails(EmployeeDetails employeeDetails, ClaimsIdentity identity);

    ServiceResponse<string> getEmployeePhoto(ClaimsIdentity identity);

    Task<ServiceResponse<string>> UploadEmployeePhoto(IFormFile image, ClaimsIdentity identity);

    Task<ServiceResponse<string>> deleteEmployeePhoto(ClaimsIdentity identity);
}