using System.Security.Claims;

public class TicketService : ITicketService
{

    private readonly DbContext _dbContext = new DbContext();
    private readonly InputValidator _inputValidator = new InputValidator();

    public ServiceResponse<string> CreateTicket(CreateTicketDto ticketDto, ClaimsIdentity identity)
    {
        ServiceResponse<string> response = new ServiceResponse<string>();
        ServiceResponse<string> validTicketRes = new ServiceResponse<string>();

        validTicketRes = _inputValidator.IsValidDescriptionAndAmount(ticketDto.Description, ticketDto.Amount);

        if (!validTicketRes.Success)
        {
            return validTicketRes;
        }

        IEnumerable<Claim> claims = identity!.Claims;
        int id = int.Parse(identity.FindFirst(c => c.Type == ClaimTypes.Sid)!.Value);
        string userName = identity.FindFirst(c => c.Type == ClaimTypes.Name)!.Value;

        Ticket ticket = new Ticket
        {
            EmployeeId = id,
            Description = ticketDto.Description!,
            Amount = (decimal)ticketDto.Amount!,
        };

        response = _dbContext.createTicket(ticket);

        return response;

    }

    public ServiceResponse<List<TicketDto>> GetPendingTickets()
    {

        ServiceResponse<List<TicketDto>> response = new ServiceResponse<List<TicketDto>>();

        response = _dbContext.GetPendingTickets();

        return response;
    }

    public ServiceResponse<string> UpdateTicket(UpdateTicketDto updateTicketDto)
    {

        ServiceResponse<string> validateTicketStatusRes = _inputValidator.ValidateUpdateTicket(updateTicketDto.TicketStatus, updateTicketDto.TicketId);

        if (!validateTicketStatusRes.Success)
        {
            return validateTicketStatusRes;
        }
        ServiceResponse<string> response = _dbContext.UpdateTicket(updateTicketDto);

        return response;

    }

    public ServiceResponse<List<TicketDto>> getTicketsById(int id)
    {
        ServiceResponse<List<TicketDto>> response = _dbContext.getTicketsById(id);

        return response;
    }
}