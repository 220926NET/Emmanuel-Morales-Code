
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ManagerController : ControllerBase
{

    private readonly ITicketService _ticketService;

    private readonly IAuthService _authService;

    private readonly DbContext _dbcontext = new DbContext();
    public ManagerController(ITicketService ticketService, IAuthService authService)
    {
        _ticketService = ticketService;
        _authService = authService;
    }

    [HttpPut]
    [Route("ApproveDenyTicket")]
    public ActionResult<ServiceResponse<string>> updateTicket(UpdateTicketDto updateTicket)
    {
        ServiceResponse<string> response = _ticketService.UpdateTicket(updateTicket);

        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }


    [HttpPost("changeRoleToManger/{employeeId}")]
    public ActionResult<ServiceResponse<string>> ChangeRoleToManager(int employeeId)
    {
        ServiceResponse<string> changeRoleRes = new ServiceResponse<string>();

        var identity = HttpContext.User.Identity as ClaimsIdentity;

        bool isManager = _authService.IsManger(identity!);

        if (!isManager)
        {
            changeRoleRes.Success = false;
            changeRoleRes.Message = "You are not authorized to access this endpoint, managers only!";
            return BadRequest(changeRoleRes);
        }


        changeRoleRes = _dbcontext.ChangeRoleToManger(employeeId);

        if (!changeRoleRes.Success)
        {
            return NoContent();
        }

        return Ok(changeRoleRes);
    }



}