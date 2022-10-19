

public class LoginService : ILoginService {

    private readonly DbContext _dbContext = new DbContext(); 


    public ServiceResponse<LoginDto> Login(Login loginUser){

        ServiceResponse<LoginDto> response = _dbContext.Login(loginUser);

        return response; 
    }

}