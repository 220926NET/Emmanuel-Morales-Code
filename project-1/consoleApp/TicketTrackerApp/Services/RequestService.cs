using Models;


/// <summary>
/// Class <c>RequestService</c> deals with validating user input and sending information to a client for persistence.
/// </summary>
class RequestService{

    private IRequest _client;
    private InputValidator _inputvalidator;
    public RequestService()
    {
        _client = ConnectionFactory.getSqlConnection();
        _inputvalidator = new InputValidator();
    }
    /// <summary>
    /// <method><c>PostCreateUser<c></method> 
    /// <para>
    /// This method validates user data and sends it to a client. 
    /// </para> 
    ///<returns>
    /// A <c>ResponseMessage</c> object containing a success flag, message string and requested data
    /// </returns> 
    /// </summary>
    public ResponseMessage<string> CreateUser(string? name, string? userName, string? password)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        bool isValidName = _inputvalidator.isValidName(name);
        bool isValidUserName = _inputvalidator.isValidName(userName);
        bool isValidPass = _inputvalidator.IsValidPassword(password);

        if (isValidName && isValidUserName && isValidPass)
        {
            User user = new User()
            {
                Name = name, 
                login = new Login(userName!, password!)
                   
            };
            responseMessage = _client.PostCreateUser(user);
        }
        else
        {
            responseMessage.success = false;
            responseMessage.message = "Please ensure your include a valid name, username and password!";
        }


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

    public ResponseMessage<User> PostLogin(string? username, string? password)
    {

        ResponseMessage<User> postLoginResponse = new ResponseMessage<User>();

        bool isValidUserName = _inputvalidator.isValidName(username);
        bool isValidPassword = _inputvalidator.IsValidPassword(password);

        if (isValidUserName && isValidPassword)
        {
            Login loginCredentials = new Login(username!, password!);
            

            postLoginResponse = _client.PostLogin(loginCredentials);

            if (postLoginResponse.success)
            {
                User user = new User
                {
                    Id = postLoginResponse.data!.Id,
                    Name = postLoginResponse.data.Name!,
                    IsManager = postLoginResponse.data.IsManager
                };
            }
            else
            {
                postLoginResponse.data = null;
                postLoginResponse.message = "Sorry unable to log you in, please ensure your username and password are correct!"; 
                postLoginResponse.success = false;

            }

        }
        else
        {
            postLoginResponse.message = "Please ensure your password and username are valid.";
            postLoginResponse.data = null;
            return postLoginResponse;

        }

        return postLoginResponse;
    }

    /// <summary>
    /// <method><c>GetUserTickets<c></method> 
    /// <para>
    /// This method retrieves user Tickets based on their id. 
    /// </para> 
    ///<returns>
    /// A <c>ResponseMessage</c> object containing a success flag, message string and requested data
    /// </returns> 
    /// </summary>
    public ResponseMessage<List<Ticket>> GetUserTickets(int id)
    {

        ResponseMessage<List<Ticket>> response = _client.GetUserTickets(id);
        return response;
    }


    /// <summary>
    /// <method><c>PostTicket<c></method> 
    /// <para>
    /// This method creates new user Tickets using their userId. 
    /// </para> 
    /// </summary>
    public ResponseMessage<string> PostTicket(string description, decimal amount, int userId)
    {
        InputValidator inputValidator = new InputValidator();
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();
        
        Ticket newTicket = new Ticket(description, amount, userId);
        responseMessage = _client.PostTicket(newTicket);
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
    public ResponseMessage<List<Ticket>> GetPendingTickets()
    {

        ResponseMessage<List<Ticket>> responseMessage = _client.GetPendingTickets();
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
    public ResponseMessage<string> UpdateTicket(int? id, string? newStatus)
    {
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        bool isValidManagerChocie = _inputvalidator.IsValidManagerChoice(id, newStatus);

        if (isValidManagerChocie)
        {

            responseMessage = _client.UpdateTicket(id, newStatus);
            return responseMessage;
        }
        else
        {
            responseMessage.message = "Please ensure that you input your response correctly!";
            responseMessage.success = false;
        }

        return responseMessage;
    }
}