using Models;
class RequestService : IRequest
{

    private IRequest _client;
    private InputValidator _inputvalidator;


    public RequestService()
    {
        _client = ConnectionFactory.getSqlConnection();
        _inputvalidator = new InputValidator();
    }

    public ResponseMessage<string> PostCreateUser(User newUser)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        bool isValidName = _inputvalidator.isValidName(newUser.Name);
        bool isValidUserName = _inputvalidator.IsValidUserName(newUser.login.UserName);
        bool isValidPass = _inputvalidator.IsValidPassword(newUser.login.Password);

        if (isValidName && isValidUserName && isValidPass)
        {
            User user = new User()
            {
                Name = newUser.Name,
                login = newUser.login
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

    public ResponseMessage<User> PostLogin(Login loginCred)
    {

        ResponseMessage<User> postLoginResponse = new ResponseMessage<User>();

        bool isValidUserName = _inputvalidator.IsValidUserName(loginCred.UserName);
        bool isValidPassword = _inputvalidator.IsValidPassword(loginCred.Password);

        if (isValidUserName && isValidPassword)
        {
            Login loginCredentials = new Login
            {
                UserName = loginCred.UserName!,
                Password = loginCred.Password!
            };

            postLoginResponse = _client.PostLogin(loginCredentials);

            if (postLoginResponse.success)
            {
                User user = new User
                {
                    Id = postLoginResponse.data.Id!,
                    Name = postLoginResponse.data.Name!,
                    IsManager = postLoginResponse.data.IsManager
                };
            }
            else
            {
                postLoginResponse.data = null;
                postLoginResponse.success = false;

            }

        }
        else
        {
            postLoginResponse.message = "Please ensure your password and username are correct.";
            postLoginResponse.data = null;
            return postLoginResponse;

        }

        return postLoginResponse;
    }


    public ResponseMessage<List<Ticket>> GetUserTickets(int id)
    {

        ResponseMessage<List<Ticket>> response = _client.GetUserTickets(id);
        return response;
    }

    public ResponseMessage<string> PostTicket(Ticket ticket)
    {
        InputValidator inputValidator = new InputValidator();
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        responseMessage = _client.PostTicket(ticket);

        return responseMessage;

    }

    public ResponseMessage<List<Ticket>> GetPendingTickets()
    {

        ResponseMessage<List<Ticket>> responseMessage = _client.GetPendingTickets();
        return responseMessage;
    }


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