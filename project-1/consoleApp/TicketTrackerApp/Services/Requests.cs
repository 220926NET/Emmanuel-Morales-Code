using Models;
class Requests
{

    Form form = new Form();
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

        Console.WriteLine(response.data);
        return response;
    }

    public ResponseMessage<string> postTicket(int userId, string? description, string? amountStr)
    {
        InputValidator inputValidator = new InputValidator();
        ResponseMessage<string> responseMessage = new ResponseMessage<string>();

        bool isValidDescription = inputValidator.isValidDescriptioon(description);
        bool isValidAmount = inputValidator.isValidAmount(amountStr);
        if (!isValidAmount || !isValidDescription)
        {
            responseMessage.message = "Please ensure Amount contains only numbers and Description contains characters!";
            return responseMessage;
        }

        int amount = int.Parse(amountStr!);

        Ticket ticket = new Ticket(description!, amount!, userId);

        responseMessage = form.postTicket(ticket);

        return responseMessage;

    }

    public ResponseMessage<List<Ticket>> getPendingTickets()
    {

        ResponseMessage<List<Ticket>> responseMessage = form.getPendingTickets();
        return responseMessage;
    }


    public ResponseMessage<string> updateTicket(int id, bool approve)
    {
        string newStatus = approve ? "approved" : "denied";
        ResponseMessage<string> responseMessage = form.updateTicket(id, newStatus);

        return responseMessage;
    }
}