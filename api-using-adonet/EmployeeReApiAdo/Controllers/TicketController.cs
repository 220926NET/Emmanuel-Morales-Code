using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 

[Authorize]
[ApiController]
[Route("[controller]")]
public class TicketController: ControllerBase{

    private readonly DbContext _dbContext = new DbContext(); 

    private ITicketService _ticketService; 
    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService; 
    }
    [HttpPost]
    public ActionResult<ServiceResponse<string>> createTicket(Ticket ticket)
        {
    
            ServiceResponse<string> response = new ServiceResponse<string>();

            response = _ticketService.CreateTicket(ticket); 

            if(response.Success == false){
                return BadRequest(response); 
            }

            return Ok(response); 
        }

    [HttpGet]
    [Route("getPendingTickets")]
    public ActionResult<ServiceResponse<List<TicketDto>>> getPendingTicket() {

            ServiceResponse<List<TicketDto>> response = new ServiceResponse<List<TicketDto>>();
            
            response = _ticketService.GetPendingTickets(); 

            if(!response.Success){
                return BadRequest(response); 
            }

            return Ok(response); 

    }

    [HttpPut]
    public ActionResult<ServiceResponse<string>> updateTicket([FromBody] string newStatus, int ticketId)
    {
        ServiceResponse<string> response = _ticketService.UpdateTicket(ticketId, newStatus);

        if(!response.Success){
            return BadRequest(response); 
        }
        return Ok(response);
    }

    [HttpGet("{id}")]

        public ActionResult<ServiceResponse<List<TicketDto>>> getTicketsById(int id)
        {

            ServiceResponse<List<TicketDto>> response = _ticketService.getTicketsById(id);

            if(!response.Success){
                return BadRequest(response.Message); 
            }
            return Ok(response); 
        }


    

    
}