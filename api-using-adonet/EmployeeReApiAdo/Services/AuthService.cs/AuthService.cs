
using System.Security.Claims;
public class AuthService : IAuthService {

    private readonly DbContext _dbcontext = new DbContext(); 
    public bool IsManger(ClaimsIdentity identity){
        
            IEnumerable<Claim> claims = identity!.Claims; 
            int id = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
        
            bool isManger = _dbcontext.IsManager(id); 

            return isManger; 
    }

    public int getEmployeeId(ClaimsIdentity identity){
            IEnumerable<Claim> claims = identity!.Claims; 

            int id = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
    

            return id; 
    }
}