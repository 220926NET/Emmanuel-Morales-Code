using Models;
class Requests : IRequestService
{

    Client form = new Client();
    public ResponseMessage<string> postCreateUser(User user)
    {

        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        responseMessage = form.postCreateUser(user);

        return responseMessage;
    }

    public ResponseMessage<User> postLogin(Login login)
    {

        ResponseMessage<User> responseMessage = form.postLogin(login);

        return responseMessage;
    }


    public ResponseMessage<List<Ticket>> getUserTickets(int id)
    {

        ResponseMessage<List<Ticket>> response = form.getUserTickets(id);
        return response;
    }

    public ResponseMessage<string> postTicket(Ticket ticket)
    {
        InputValidator inputValidator = new InputValidator();
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        responseMessage = form.postTicket(ticket);

        return responseMessage;

    }

    public ResponseMessage<List<Ticket>> getPendingTickets()
    {

        ResponseMessage<List<Ticket>> responseMessage = form.getPendingTickets();
        return responseMessage;
    }


    public ResponseMessage<string> updateTicket(int id, string mgrDecision)
    {
        ResponseMessage<string> responseMessage = form.updateTicket(id, mgrDecision);

        return responseMessage;
    }
}