using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TicketController: ControllerBase{

    private readonly DbContext _dbContext = new DbContext(); 

    [HttpPost]
    public ActionResult<ServiceResponse<string>> createTicket(Ticket ticket)
        {

            ServiceResponse<string> response = new ServiceResponse<string>();

            response = _dbContext.createTicket(ticket); 

            if(response.Success == false){
                return BadRequest(response); 
            }

            return Ok(response); 
        }

    [HttpGet]
    public ActionResult<ServiceResponse<List<TicketDto>>> getPendingTicket() {

            ServiceResponse<List<TicketDto>> response = new ServiceResponse<List<TicketDto>>();
            
            response = _dbContext.GetPendingTickets(); 

            if(!response.Success){
                return BadRequest(response); 
            }

            return Ok(response); 

    }

    [HttpPut]
    public ActionResult<ServiceResponse<string>> Post([FromBody] string newStatus, int ticketId)
    {
        ServiceResponse<string> response = _dbContext.UpdateTicket(ticketId, newStatus);

        if(!response.Success){
            return BadRequest(response); 
        }
        return Ok(response);
    }

    [HttpGet("{id}")]

        public ActionResult<ServiceResponse<List<TicketDto>>> Get(int id)
        {

            ServiceResponse<List<TicketDto>> response = _dbContext.getTicketsById(id);

            if(!response.Success){
                return BadRequest(response.Message); 
            }
            return Ok(response); 
        }

    
}