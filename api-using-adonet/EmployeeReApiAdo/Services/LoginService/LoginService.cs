
using System.Security.Claims; 
using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens.Jwt; 
using System.Text; 
public class LoginService : ILoginService {

    private readonly DbContext _dbContext; 
    private readonly InputValidator _inputValidator = new InputValidator(); 
    private readonly IConfiguration _configuration; 
    public LoginService(IConfiguration configuration, DbContext dbContext) 
    {
        _dbContext = dbContext; 
        _configuration = configuration;
    }
    public ServiceResponse<string> Login(Login loginUser){

        
        ServiceResponse<LoginDto> insertToDbRes = _dbContext.Login(loginUser);
        ServiceResponse<string> createTokenRes = new ServiceResponse<string>();

        if(insertToDbRes.Success){
            // add token to response 
            createTokenRes.Data = CreateToken(insertToDbRes.Data!.Id.ToString(), insertToDbRes.Data.UserName); 
            createTokenRes.Message = "success token created!"; 
            createTokenRes.Success = true; 
        } else {
            createTokenRes.Message = "That account does not exist." ;
            createTokenRes.Success = false; 
        }

        return createTokenRes; 
    }


    private string CreateToken(string id, string userName){
        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.Sid, id),
            new Claim(ClaimTypes.Name, userName)
        }; 

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyTopSecretToken123"));

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