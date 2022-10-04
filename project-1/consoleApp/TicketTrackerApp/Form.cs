using Newtonsoft.Json;
using System.Text;
using System.Net;
class Form{

    private static readonly HttpClient client = new HttpClient();

    private string _SignUpEndpoint = "https://localhost:7020/api/SignUp"; 

    private string _LoginEndpoint = "https://localhost:7020/api/Login";


    // returns 1 if successful or  -1 for unsuccessful 
    public int postCreateUser(User user){
       
       // check if user already exists
       var newPostJson = JsonConvert.SerializeObject(user);
       var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");
    
       
       var result = client.PostAsync(_SignUpEndpoint, httpContent).Result;


        if(result.StatusCode == System.Net.HttpStatusCode.OK){
            Console.WriteLine("success");
            return 1; 
        }
        
       return -1; 
    }

    public int login(Login login){
        var newPostJson = JsonConvert.SerializeObject(login);
        var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");

        var result = client.PostAsync(_LoginEndpoint, httpContent).Result;

        IEnumerable<string> id;
        bool success = result.Headers.TryGetValues("id", out id);
        if(success){
            int newId = int.Parse(id.First());
            Console.WriteLine("int id is " + id.First());
        }

        if(result.StatusCode == HttpStatusCode.OK){
            Console.WriteLine("succesfully logged in.");
            return 1;
        } 
        Console.WriteLine("There was an issue logging in. Please try again.");
        return -1; 

    }
}