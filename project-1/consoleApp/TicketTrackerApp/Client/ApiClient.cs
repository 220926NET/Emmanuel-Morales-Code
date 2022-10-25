using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers; 
using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Models;
class ApiClient
{

    private static readonly HttpClient client = new HttpClient();

    private string _RegisterEmployeeEndPoint = "https://localhost:7021/Register";

    private string _LoginEndpoint = "https://localhost:7021/Login";

    private string _PostTicketEndPoint = "https://localhost:7021/Ticket/createTicket";

    private string _getPendintTicketsEndPoint = "https://localhost:7021/Ticket/getPendingTickets";

    private string _createTicketEndPoint = "https://localhost:7021/Ticket/createTicket";

    private string _UpdateTicketEndPoint = "https://localhost:7021/Ticket"; 

    private string _GetEmployeeTickets = "https://localhost:7021/Ticket/getMyTickets";


    public ResponseMessage<string> PostCreateUser(PostNewEmployee employee)
    {   
        
        var newPostJson = JsonConvert.SerializeObject(employee);
        var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");
        var result = client.PostAsync(_RegisterEmployeeEndPoint, httpContent).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        responseMessage = JsonConvert.DeserializeObject<ResponseMessage<string>>(content.Result)!;

        
        
        return responseMessage;
    }


    public ResponseMessage<Employee> PostLogin(Login login)
    {

        PostLogin postLogin = new PostLogin{
            userName = login.UserName!,
            password = login.getPassword()!
        }; 

        var newPostJson = JsonConvert.SerializeObject(postLogin);
        var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");
        var result = client.PostAsync(_LoginEndpoint, httpContent).Result;
        var content = result.Content.ReadAsStringAsync();

        
        
        ResponseMessage<string> responseMessage = JsonConvert.DeserializeObject<ResponseMessage<string>>(content.Result)!;
        ResponseMessage<Employee> newUser = new ResponseMessage<Employee>(); 

        if(responseMessage.success){
            ClaimsIdentity claims = new ClaimsIdentity();
            var jwt = responseMessage.data; 
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt); 
            int userId = int.Parse(token.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
            string userName = token.Claims.First(c => c.Type == "unique_name").Value.ToString();

            // build Employee 
            Employee Employee = new Employee{
                Id = userId,
                UserName = userName,
                Token = jwt!
            };
            newUser.data = Employee; 
            newUser.message = responseMessage.message; 
            newUser.success = responseMessage.success; 
            return newUser;


        } else {
            newUser.data = null;
            newUser.message = "Please create an account or verify your login credentials!"; 
            newUser.success = false; 
            return newUser; 
        }

    }


    public ResponseMessage<List<Ticket>> GetUserTickets(string token)
    {


        string uri = _GetEmployeeTickets;
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, uri);
        message.Headers.Authorization = new AuthenticationHeaderValue("bearer", token); 

        var result = client.SendAsync(message).Result;

        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<List<Ticket>> responseMessage = JsonConvert.DeserializeObject<ResponseMessage<List<Ticket>>>(content.Result)!;

        

        return responseMessage;



    }

    public ResponseMessage<List<Ticket>> GetPendingTickets(string token)
    {
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, _getPendintTicketsEndPoint);
        message.Headers.Authorization = new AuthenticationHeaderValue("bearer", token); 

        var result = client.SendAsync(message).Result;

        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<List<Ticket>> responseMessage = JsonConvert.DeserializeObject<ResponseMessage<List<Ticket>>>(content.Result)!;

        return responseMessage;

    }


    public ResponseMessage<string> PostTicket(Ticket ticket, string token)
    {
        HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _createTicketEndPoint);
        message.Headers.Authorization = new AuthenticationHeaderValue("bearer", token); 
       
        var newPostJson = JsonConvert.SerializeObject(ticket);

        message.Content = new StringContent(newPostJson, Encoding.UTF8, "application/json"); 

        var result = client.SendAsync(message).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        responseMessage = JsonConvert.DeserializeObject<ResponseMessage<string>>(content.Result)!;

        return responseMessage;
    }

    public ResponseMessage<string> UpdateTicket(string token,int ticketId, string? newStatus)
    {
         HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, _UpdateTicketEndPoint);
        message.Headers.Authorization = new AuthenticationHeaderValue("bearer", token); 

        PutTicket putTicket = new PutTicket{
            TicketId = ticketId,
            TicketStatus = newStatus,
        }; 

        var newPutJson = JsonConvert.SerializeObject(putTicket);
        message.Content = new StringContent(newPutJson, Encoding.UTF8, "application/json"); 

        var result = client.SendAsync(message).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<string> responseMessage = JsonConvert.DeserializeObject<ResponseMessage<string>>(content.Result)!;

        return responseMessage;

    }





}