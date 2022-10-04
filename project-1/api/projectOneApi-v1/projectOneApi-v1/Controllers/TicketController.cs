using Microsoft.AspNetCore.Mvc;
using projectOneApi_v1.Data;
using projectOneApi_v1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace projectOneApi_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        private ApiDbContext _dbContext; 

        public TicketController(ApiDbContext dbContext)
        {
            _dbContext = dbContext; 

        }

        [HttpPost]
        public void Post([FromBody] Tickets ticket)
        {
            Console.WriteLine(ticket.Amount);
            Console.WriteLine(ticket.Description);
            Console.WriteLine(ticket.Status);
            Console.WriteLine(ticket.employeeId);

            _dbContext.Tickets.Add(ticket);
            _dbContext.SaveChanges();
        }

        
    }
}
