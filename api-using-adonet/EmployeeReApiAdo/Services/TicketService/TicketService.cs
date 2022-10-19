

public class TicketService : ITicketService {

    private readonly DbContext _dbContext = new DbContext(); 

    public ServiceResponse<string> CreateTicket(Ticket ticket){
        ServiceResponse<string> response = new ServiceResponse<string>();

        response = _dbContext.createTicket(ticket); 

        return response; 

     }

     public ServiceResponse<List<TicketDto>> GetPendingTickets(){

        ServiceResponse<List<TicketDto>> response = new ServiceResponse<List<TicketDto>>();
        
        response = _dbContext.GetPendingTickets(); 

        return response; 
     }

    public ServiceResponse<string> UpdateTicket(int ticketId, string newStatus){
        
        ServiceResponse<string> response = _dbContext.UpdateTicket(ticketId, newStatus);

        return response; 
        
    }

    public ServiceResponse<List<TicketDto>> getTicketsById(int id){
        ServiceResponse<List<TicketDto>> response = _dbContext.getTicketsById(id);

        return response; 
    }
}