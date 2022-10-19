

public class RegisterService : IRegisterService {

    private readonly DbContext _dbContext = new DbContext();

    public ServiceResponse<string> SignUp(Employee employee){
        
        ServiceResponse<string> response = _dbContext.CreateUser(employee);

        return response; 

    }

}