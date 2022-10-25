using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly DbContext _dbContext;
    private ITicketService _ticketService;
    private IAuthService _authService;
    public TicketController(ITicketService ticketService, IAuthService authService, DbContext dbContext)
    {
        _ticketService = ticketService;
        _authService = authService;
        _dbContext = dbContext;
    }


    [HttpPost]
    [Route("createTicket")]

    public ActionResult<ServiceResponse<string>> createTicket(CreateTicketDto ticketDto)
    {

        var identity = HttpContext.User.Identity as ClaimsIdentity;

        ServiceResponse<string> response = new ServiceResponse<string>();

        response = _ticketService.CreateTicket(ticketDto, identity!);

        if (response.Success == false)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    [Route("getPendingTickets")]
    public ActionResult<ServiceResponse<List<TicketDto>>> getPendingTicket()
    {
        ServiceResponse<List<TicketDto>> response = new ServiceResponse<List<TicketDto>>();
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        // check that user is a manager
        if (!_authService.IsManger(identity!))
        {
            response.Data = null;
            response.Success = false;
            response.Message = "You are not a manager! Your Unable to get pending tickets!";
            return Unauthorized(response);
        }

        response = _ticketService.GetPendingTickets();

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);

    }




    [HttpGet("getMyTickets")]

    public ActionResult<ServiceResponse<List<TicketDto>>> getMyTickets()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        ServiceResponse<List<TicketDto>> response = _ticketService.getTicketsById(_authService.getEmployeeId(identity!));

        if (!response.Success)
        {

            return BadRequest(response);
        }
        return Ok(response);
    }





}