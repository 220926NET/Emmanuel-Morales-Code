
using Microsoft.AspNetCore.Mvc;
using projectOneApi_v1.Dtos;
using projectOneApi_v1.Models;

namespace projectOneApi_v1.Services.TicketService
{
    public interface ITicketService
    {

        public ServiceResponse<List<Tickets>> getTicketByEmployeeId(int id);


        public ServiceResponse<ActionResult> updateTicketStatusById(string value, int id);

        public ServiceResponse<ActionResult> createTicket(Tickets ticket);

        public ServiceResponse<List<Tickets>> getAllPendingTickets();

    }
}
