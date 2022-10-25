using System.Security.Claims;

public interface IAuthService {

    public bool IsManger(ClaimsIdentity identity); 

    public int getEmployeeId(ClaimsIdentity identity); 
}