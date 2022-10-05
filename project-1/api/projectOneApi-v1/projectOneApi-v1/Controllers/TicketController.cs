using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using projectOneApi_v1.Data;
using projectOneApi_v1.Models;
using System.Reflection.Metadata.Ecma335;


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
            //TODO add validation for ticket 
            _dbContext.Tickets.Add(ticket);
            _dbContext.SaveChanges();
        }

        [HttpPost("{id}")]

        public ActionResult Post([FromBody] string value,  int id)
        {

            Console.WriteLine("value is " + value);
            Console.WriteLine("id is " + id);

            try
            {
                Tickets ticket = _dbContext.Tickets.Where(ticket => ticket.Id == id).First();

                ticket.Status = value;

                _dbContext.SaveChanges();
                
                return Ok();
            } catch (Exception e)
            {
                return BadRequest();
            }
            


         }


        [HttpGet] 

        public ActionResult Get()
        {


            var tickets = (from ticket in _dbContext.Tickets
                           join employee in _dbContext.Employees on ticket.employeeId equals employee.Id
                           where ticket.Status == "pending"
                           select new { ticket.Id, employee.Name, ticket.Description, ticket.Amount, ticket.Status }) ;
            if(tickets == null)
            {
                return BadRequest();
            }
            return Ok(tickets); 
        }


        [HttpGet("{id}")]

        public ActionResult Get(int id)
        {
            var tickets = (from ticket in _dbContext.Tickets
                           where ticket.employeeId == id
                           select ticket);

            if(tickets.IsNullOrEmpty())
            {
                return NotFound();
               
            } else
            {
                return Ok(tickets);
            }

         
        }

        
       
    }
}
