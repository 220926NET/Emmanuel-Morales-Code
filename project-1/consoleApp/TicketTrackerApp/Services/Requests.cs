using Models;
class Requests : IRequest
{

    Client form = new Client();
    public ResponseMessage<string> PostCreateUser(User user)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        responseMessage = form.PostCreateUser(user);

        return responseMessage;
    }

    public ResponseMessage<User> PostLogin(Login login)
    {

        ResponseMessage<User> responseMessage = form.PostLogin(login);

        return responseMessage;
    }


    public ResponseMessage<List<Ticket>> GetUserTickets(int id)
    {

        ResponseMessage<List<Ticket>> response = form.GetUserTickets(id);
        return response;
    }

    public ResponseMessage<string> PostTicket(Ticket ticket)
    {
        InputValidator inputValidator = new InputValidator();
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        responseMessage = form.PostTicket(ticket);

        return responseMessage;

    }

    public ResponseMessage<List<Ticket>> GetPendingTickets()
    {

        ResponseMessage<List<Ticket>> responseMessage = form.GetPendingTickets();
        return responseMessage;
    }


    public ResponseMessage<string> UpdateTicket(int id, string mgrDecision)
    {
        ResponseMessage<string> responseMessage = form.UpdateTicket(id, mgrDecision);

        return responseMessage;
    }
}