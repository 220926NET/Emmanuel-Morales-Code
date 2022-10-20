using System.Text.Json; 

public class RegisterService : IRegisterService {

    private readonly DbContext _dbContext = new DbContext();

    public ServiceResponse<string> SignUp(RegisterAccountDto newEmployee){
        
        ServiceResponse<string> response = _dbContext.CreateUser(newEmployee);

        return response; 

    }

    


}