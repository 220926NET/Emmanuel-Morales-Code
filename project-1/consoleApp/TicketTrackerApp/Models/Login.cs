namespace Models;


public class Login
{
    public string UserName {get; set;}
    private string _password {get; set;}

    public Login(string userName, string password)
    {
        UserName = userName; 
        _password = password; 
    }

    public string getPassword(){
        return _password; 
    }
}