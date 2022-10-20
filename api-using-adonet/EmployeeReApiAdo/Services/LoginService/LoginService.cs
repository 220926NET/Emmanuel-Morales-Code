
using System.Security.Claims; 
using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens.Jwt; 
public class LoginService : ILoginService {

    private readonly DbContext _dbContext = new DbContext(); 
    private readonly IConfiguration _configuration; 
    public LoginService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public ServiceResponse<string> Login(Login loginUser){

        ServiceResponse<LoginDto> response = _dbContext.Login(loginUser);

        ServiceResponse<string> res = new ServiceResponse<string>();

        if(response.Success){
            // add token to response 
            res.Data = CreateToken(response.Data!.Id.ToString(), response.Data.Name); 
            res.Message = "success token created!"; 
            res.Success = true; 
        } else {
            res.Message = "Unable to log you in" ;
            res.Success = false; 
        }

        return res; 
    }


    private string CreateToken(string id, string employeeName){
        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Name, employeeName)
        }; 

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("MyTopSecretToken123"));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); 

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        }; 

        JwtSecurityTokenHandler tokenHandler  = new JwtSecurityTokenHandler(); 
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor); 



        return tokenHandler.WriteToken(token); 
    }

}