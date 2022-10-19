using Microsoft.AspNetCore.Mvc;
public interface ITicketService {

     ServiceResponse<string> CreateTicket(Ticket ticket); 

     ServiceResponse<List<TicketDto>> GetPendingTickets(); 

     ServiceResponse<string> UpdateTicket(int ticketId, string newStatus); 

     ServiceResponse<List<TicketDto>> getTicketsById(int id); 

}