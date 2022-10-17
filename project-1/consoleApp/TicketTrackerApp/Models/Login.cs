namespace Models;


public class Login
{
    public string UserName; 
    private string _password;

    public Login(string userName, string password)
    {
        UserName = userName; 
        _password = password; 
    }

    public string getPassword(){
        return _password; 
    }
}