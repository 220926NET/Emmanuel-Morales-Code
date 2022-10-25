
using System.Security.Claims;
public interface ITicketService {

     ServiceResponse<string> CreateTicket(CreateTicketDto ticketDto,ClaimsIdentity claimsIdentity); 

     ServiceResponse<List<TicketDto>> GetPendingTickets(); 

     ServiceResponse<string> UpdateTicket(UpdateTicketDto createTicketDto); 

     ServiceResponse<List<TicketDto>> getTicketsById(int id); 

}