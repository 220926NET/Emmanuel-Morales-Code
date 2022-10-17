using Newtonsoft.Json;

using System.Text;
using Models;
class ApiClient : IRequest
{

    private static readonly HttpClient client = new HttpClient();

    private string _SignUpEndpoint = "https://localhost:7020/api/SignUp";

    private string _LoginEndpoint = "https://localhost:7020/api/Login";

    private string _PostTicketEndPoint = "https://localhost:7020/api/Ticket";

    private string _getPendintTicketsEndPoint = "https://localhost:7020/api/Ticket";

    private string _putTicketEndPoint = "https://localhost:7020/api/Ticket/";

    private string _getTicketByIdEndpoint = "https://localhost:7020/api/Ticket/";


    public ResponseMessage<string> PostCreateUser(User user)
    {
        var newPostJson = JsonConvert.SerializeObject(user);
        var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");
        var result = client.PostAsync(_SignUpEndpoint, httpContent).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        responseMessage = JsonConvert.DeserializeObject<ResponseMessage<string>>(content.Result)!;

        return responseMessage;
    }


    public ResponseMessage<User> PostLogin(Login login)
    {

        var newPostJson = JsonConvert.SerializeObject(login);
        var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");
        var result = client.PostAsync(_LoginEndpoint, httpContent).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<User> responseMessage = new ResponseMessage<User>();
        responseMessage = JsonConvert.DeserializeObject<ResponseMessage<User>>(content.Result)!;

        return responseMessage;


    }


    public ResponseMessage<List<Ticket>> GetUserTickets(int userId)
    {
        string uri = _getTicketByIdEndpoint + userId;
        var result = client.GetAsync(uri).Result;
        var content = result.Content.ReadAsStringAsync();


        ResponseMessage<List<Ticket>> responseMessage = JsonConvert.DeserializeObject<ResponseMessage<List<Ticket>>>(content.Result)!;

        return responseMessage;



    }

    public ResponseMessage<List<Ticket>> GetPendingTickets()
    {
        var result = client.GetAsync(_getPendintTicketsEndPoint).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<List<Ticket>> responseMessage = JsonConvert.DeserializeObject<ResponseMessage<List<Ticket>>>(content.Result)!;

        return responseMessage;

    }


    public ResponseMessage<string> PostTicket(Ticket ticket)
    {
        var newPostJson = JsonConvert.SerializeObject(ticket);
        var httpContent = new StringContent(newPostJson, Encoding.UTF8, "application/json");
        var result = client.PostAsync(_PostTicketEndPoint, httpContent).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        responseMessage = JsonConvert.DeserializeObject<ResponseMessage<string>>(content.Result)!;

        return responseMessage;
    }

    public ResponseMessage<string> UpdateTicket(int? id, string? newStatus)
    {

        string putReqUri = _putTicketEndPoint + id;
        var newPutJson = JsonConvert.SerializeObject(newStatus);
        var httpContent = new StringContent(newPutJson, Encoding.UTF8, "application/json");

        var result = client.PutAsync(putReqUri, httpContent).Result;
        var content = result.Content.ReadAsStringAsync();

        ResponseMessage<string> responseMessage = JsonConvert.DeserializeObject<ResponseMessage<string>>(content.Result)!;

        return responseMessage;

    }





}