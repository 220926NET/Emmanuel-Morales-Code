using Models;

public interface IRequest
{

    public ResponseMessage<string> PostCreateUser(User user);

    public ResponseMessage<User> PostLogin(Login login);

    public ResponseMessage<string> PostTicket(Ticket ticket);

    public ResponseMessage<List<Ticket>> GetUserTickets(int id);

    public ResponseMessage<List<Ticket>> GetPendingTickets();

    public ResponseMessage<string> UpdateTicket(int? id, string? newStatus);




}