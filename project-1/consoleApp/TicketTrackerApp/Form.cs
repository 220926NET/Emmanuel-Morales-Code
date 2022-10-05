using Newtonsoft.Json;
using System.Text;
using System.Net;
class Form{

    private static readonly HttpClient client = new HttpClient();

    private int _userId; 

    private User? _user; 
    private string _SignUpEndpoint = "https://localhost:7020/api/SignUp"; 

    private string _LoginEndpoint = "https://localhost:7020/api/Login";

    private string _PostTicketEndPoint = "https://localhost:7020/api/Ticket";

    private string _getUserEndPoint = "https://localhost:7020/api/Login/"; 
    
    private string _getTicketsEndPoint = "https://localhost:7020/api/Ticket";

    private string _putTicketEndPoint = "https://localhost:7020/api/Ticket/";

    private string _getTicketByIdEndpoint = "https://localhost:7020/api/Ticket/"; 

    private List<Ticket>? _pendingTickets; 

    private List<Ticket>? _submittedTickets; 

    // returns 1 if successful or  -1 for unsuccessful 
    public int postCreateUser(User user){
       // check if user already exists
       var newPostJson = JsonConvert.SerializeObject(user);
       var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");
    
       var result = client.PostAsync(_SignUpEndpoint, httpContent).Result;

        if(result.StatusCode == System.Net.HttpStatusCode.OK){
        
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
            setUserId(newId);
        }

        if(result.StatusCode == HttpStatusCode.OK){
            Console.WriteLine("succesfully logged in.");
            return 1;
        } else if (result.StatusCode == HttpStatusCode.NotFound){
            Console.WriteLine("Sorry no account with that username was found");
        }
        
    
    
        return -1; 

    }

    public int postTicket(Ticket ticket){
        var newPostJson = JsonConvert.SerializeObject(ticket);
        var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");

        var result = client.PostAsync(_PostTicketEndPoint, httpContent).Result;

        if(result.StatusCode == HttpStatusCode.OK){
            return 1; 
        } 
        return -1; 
    }

    public async Task getUserReq(){
        string getUser = _getUserEndPoint + _userId; 
        var response = await client.GetStringAsync(getUser);
        var user =  JsonConvert.DeserializeObject<User>(response);
        _user = user; 
        }
    
    public async Task setPendingTickets(){
        var response = await client.GetStringAsync(_getTicketsEndPoint);
        
        List<Ticket> tickets =  JsonConvert.DeserializeObject<List<Ticket>>(response);

        _pendingTickets = tickets;
        
    }

    public bool updateTicket(int id, bool approve){
        string newStatus = approve ? "approved" : "denied";
        string putReqUri = _putTicketEndPoint + id; 
        var newPutJson = JsonConvert.SerializeObject(newStatus);
        var httpContent = new StringContent(newPutJson, Encoding.UTF8, "application/json");

        Console.WriteLine("put req uri is " + putReqUri);
        // string content provides http content based on a string 
        // .Result allows us to wait for the response to be recieved 
        var response = client.PostAsync(putReqUri, httpContent).Result;
        if(response.StatusCode == HttpStatusCode.OK){
            
            return true; 
        }
        return false; 
    }

    // TODO given a user id implement get tickets 

    public void setTicketsById(int id){
        string uri = _getTicketByIdEndpoint + id; 
        var response =  client.GetAsync(uri).Result;
        if(response.StatusCode == HttpStatusCode.NotFound){
            Console.WriteLine("No tickets to show");
            
        } else {
            List<Ticket> tickets = JsonConvert.DeserializeObject<List<Ticket>>(response.Content.ToString());
            _submittedTickets = tickets;

        }
        

    }

    public List<Ticket> getPendingTicket(){
        return _pendingTickets; 
    }


    public List<Ticket> getSubmittedTickets(){
        return _submittedTickets;
    }
    public void setUserId(int id){
        _userId = id; 
    }

    public int getUserId(){
        return _userId;
    }

    public User getUser(){
        return _user;
    }
}