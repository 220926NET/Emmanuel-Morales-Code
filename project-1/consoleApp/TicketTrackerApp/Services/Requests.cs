using Models;
class Requests : IRequest
{

    private IRequest _client;


    public Requests(IRequest client)
    {
        _client = client;
    }


    public ResponseMessage<string> PostCreateUser(User user)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        responseMessage = _client.PostCreateUser(user);

        return responseMessage;
    }

    public ResponseMessage<User> PostLogin(Login login)
    {

        ResponseMessage<User> responseMessage = _client.PostLogin(login);

        return responseMessage;
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


    public ResponseMessage<string> UpdateTicket(int id, string mgrDecision)
    {
        ResponseMessage<string> responseMessage = _client.UpdateTicket(id, mgrDecision);

        return responseMessage;
    }
}