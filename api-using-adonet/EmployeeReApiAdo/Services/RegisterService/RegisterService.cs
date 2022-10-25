using System.Text.Json; 

public class RegisterService : IRegisterService {

    private readonly DbContext _dbContext = new DbContext();
    private readonly InputValidator _inputValidator = new InputValidator(); 

    public ServiceResponse<string> SignUp(RegisterAccountDto newEmployee){

        
        ServiceResponse<String> isValidPasswordRes = _inputValidator.IsValidPassword(newEmployee.Password);
        ServiceResponse<String> isValidUserNameRes = _inputValidator.isValidUserName(newEmployee.UserName); 
        ServiceResponse<String> isValidNameRes = _inputValidator.isValidName(newEmployee.Name); 

        //maintain order name -> username -> password to notify user sequentially 
        if(!isValidNameRes.Success){
            return isValidNameRes; 
        }
        if(!isValidUserNameRes.Success){
            return isValidUserNameRes; 
        }
        if(!isValidPasswordRes.Success){
            return isValidPasswordRes; 
        }
        

        ServiceResponse<string> response = _dbContext.CreateUser(newEmployee);

        return response; 

    }

    


}