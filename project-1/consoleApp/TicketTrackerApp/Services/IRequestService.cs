using Models;

public interface IRequestService {

    public ResponseMessage<string> postCreateUser(User user);

    public ResponseMessage<User> postLogin(Login login);

    public ResponseMessage<string> postTicket(Ticket ticket);

    public ResponseMessage<List<Ticket>> getUserTickets(int id);

    public ResponseMessage<List<Ticket>> getPendingTickets();

    public ResponseMessage<string> updateTicket(int id, string newStatus);




}