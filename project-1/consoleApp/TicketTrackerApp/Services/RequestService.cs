using Models;


/// <summary>
/// Class <c>RequestService</c> deals with validating Employee input and sending information to a client for persistence.
/// </summary>
class RequestService{

    private ApiClient _client = new ApiClient();
   
    public RequestService()
    {

   
    }
    /// <summary>
    /// <method><c>PostCreateUser<c></method> 
    /// <para>
    /// This method validates Employee data and sends it to a client. 
    /// </para> 
    ///<returns>
    /// A <c>ResponseMessage</c> object containing a success flag, message string and requested data
    /// </returns> 
    /// </summary>
    public ResponseMessage<string> CreateUser(string? name, string? userName, string? password)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        
            PostNewEmployee employee = new PostNewEmployee{
                Name = name!, 
                UserName = userName!,
                Password = password!
            }; 
            responseMessage = _client.PostCreateUser(employee);
        


        return responseMessage;
    }  
    /// <summary>
    /// <method><c>PostLogin<c></method> 
    /// <para>
    /// This method validates login credentials and sends data to a client. 
    /// </para> 
    ///<returns>
    /// A <c>ResponseMessage</c> object containing a success flag, message string and requested data
    /// </returns> 
    /// </summary>

    public ResponseMessage<Employee> PostLogin(string? username, string? password)
    {

        
        ResponseMessage<Employee> postLoginResponse = new ResponseMessage<Employee>();

       
        Login loginCredentials = new Login(username!, password!);
            

        postLoginResponse = _client.PostLogin(loginCredentials);


        return postLoginResponse;
    }

    /// <summary>
    /// <method><c>GetUserTickets<c></method> 
    /// <para>
    /// This method retrieves Employee Tickets based on their id. 
    /// </para> 
    ///<returns>
    /// A <c>ResponseMessage</c> object containing a success flag, message string and requested data
    /// </returns> 
    /// </summary>
    public ResponseMessage<List<Ticket>> GetUserTickets(string token) 
    {

        ResponseMessage<List<Ticket>> response = _client.GetUserTickets(token);
        return response;
    }


    /// <summary>
    /// <method><c>PostTicket<c></method> 
    /// <para>
    /// This method creates new Employee Tickets using their userId. 
    /// </para> 
    /// </summary>
    public ResponseMessage<string> PostTicket(string description, decimal amount, string token)
    {
       
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        
        Ticket newTicket = new Ticket(description, amount);
        responseMessage = _client.PostTicket(newTicket, token);
        return responseMessage;
    }


    /// <summary>
    /// <method><c>GetPendingTickets<c></method> 
    /// <para>
    /// This method returns pending tickets from the client. 
    /// </para> 
    ///<returns>
    /// A <c>ResponseMessage</c> object containing a success flag, message string and requested data
    /// </returns> 
    /// </summary>
    public ResponseMessage<List<Ticket>> GetPendingTickets(string token)
    {

        ResponseMessage<List<Ticket>> responseMessage = _client.GetPendingTickets(token);
        return responseMessage;
    }

    /// <summary>
    /// <method><c>PostLogin<c></method> 
    /// <para>
    /// This method validates new ticket status and id before sending it for persistence. 
    /// </para> 
    ///<returns>
    /// A <c>ResponseMessage</c> object containing a success flag, message string and requested data
    /// </returns> 
    /// </summary>
    public ResponseMessage<string> UpdateTicket(string token,int ticketId,  string? newStatus)
    {
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        responseMessage = _client.UpdateTicket(token,ticketId, newStatus);
        return responseMessage;
       
    }
}