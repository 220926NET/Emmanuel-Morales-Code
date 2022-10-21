

public class EmployeeService : IEmployeeService {

    private readonly DbContext _dbContext = new DbContext(); 
    public ServiceResponse<string> addEmployeeDetails(EmployeeDetailsDto employeeDetails){
        ServiceResponse<string> addEmployeeDetailsRes = _dbContext.AddEmployeeDetails(employeeDetails); 
        return addEmployeeDetailsRes;
    }

}