using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projectOneApi_v1.Data;
using projectOneApi_v1.Dtos;
using projectOneApi_v1.Models;
using System.Net.Sockets;
using System.Text.Json;

namespace projectOneApi_v1.Services.TicketService
{
    public class TicketService : ITicketService
    {
        private readonly ApiDbContext _dbContext;

        public TicketService(ApiDbContext context)
        {

            _dbContext = context;
        }

        public ApiDbContext Context { get; }
        public Mapper Mapper { get; }

        public ServiceResponse<ActionResult> createTicket(Tickets ticket)
        {
            ServiceResponse<ActionResult> response = new ServiceResponse<ActionResult>();
            _dbContext.Tickets.Add(ticket);
            int amountSaves = _dbContext.SaveChanges();
            if(amountSaves > 0)
            {
                response.Message = "Ticket created successfully";
                response.Success = true; 


            } else
            {
                response.Message = "Unable to create ticket";
                response.Success=false;
            }

            return response; 
        }

        public ServiceResponse<List<Tickets>> getAllPendingTickets()
        {
                ServiceResponse<List<Tickets>> response = new ServiceResponse<List<Tickets>>();

            List<Tickets> tickets = (from ticket in _dbContext.Tickets
                           join employee in _dbContext.Employees on ticket.employeeId equals employee.Id
                           where ticket.Status == "pending"
                           select ticket).ToList(); 


                if (tickets.IsNullOrEmpty())
                {
                    response.Success = false;
                    response.Message = "No tickets to show";
                } else
                {
                response.Data = tickets;
                response.Message = "Pending Tickets retrieved";
                 }

            return response; 
            
      
        }

        public ServiceResponse<List<Tickets>> getTicketByEmployeeId(int id)
        {
            ServiceResponse<List<Tickets>> response = new ServiceResponse<List<Tickets>>();

            List<Tickets> ticketsFound = (from tickets in _dbContext.Tickets
                                    where tickets.employeeId == id
                                    select tickets).ToList();

            if(ticketsFound.IsNullOrEmpty())
            {
                response.Success = false;
                response.Message = "No tickets found";

            } else
            {
                response.Data = ticketsFound;
                response.Success = true;
                response.Message = "Tickets retrieved"; 
            }

            return response;

        }

        public ServiceResponse<ActionResult> updateTicketStatusById(string value, int id)
        {

            ServiceResponse<ActionResult> response = new ServiceResponse<ActionResult>();
            Tickets ticket = _dbContext.Tickets.Where(ticket => ticket.Id == id).First();
           
            
            if(ticket.Status != "pending")
            {
                response.Message = "Please choose a valid ticket!";
                response.Success = false;
                return response; 
            } 

            if (value.ToLower() == "approve" || value.ToLower() == "deny")
            {
                ticket.Status = value;
                int amountOfSaves = _dbContext.SaveChanges();
                if (amountOfSaves > 0)
                {
                    response.Message = "Successfully updated!";
                    response.Success = true;

                }
                else
                {

                    response.Message = "Unable to update ticket";
                    response.Success = false;
                }
                
            } else
            {
                response.Message = "You can only approve or deny tickets";
                response.Success = false;
                return response;
            }
            

            return response; 
            
        }
    }
}
