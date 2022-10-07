using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using projectOneApi_v1.Data;
using projectOneApi_v1.Models;
using projectOneApi_v1.Services.TicketService;
using System.Reflection.Metadata.Ecma335;


namespace projectOneApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService ticketService;

        public TicketController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        [HttpPost]

        public ServiceResponse<ActionResult> Post([FromBody] Tickets ticket)
        {
            ServiceResponse<ActionResult> response = ticketService.createTicket(ticket);
            return response;
        }


        [HttpPut("{id}")]

        public ServiceResponse<ActionResult> Post([FromBody] string value, int id)
        {
            ServiceResponse<ActionResult> response = ticketService.updateTicketStatusById(value, id);
            return response;
        }


        [HttpGet]
        public ServiceResponse<List<Tickets>> Get()
        {

            ServiceResponse<List<Tickets>> response = ticketService.getAllPendingTickets();
            return response;
        }



        [HttpGet("{id}")]

        public ServiceResponse<List<Tickets>> Get(int id)
        {

            ServiceResponse<List<Tickets>> response = ticketService.getTicketByEmployeeId(id);
            return response; 
        }

       
       

    }
}
