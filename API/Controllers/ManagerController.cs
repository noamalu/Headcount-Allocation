using Microsoft.AspNetCore.Mvc;
using System;
using HeadcountAllocation.Services;
using HeadcountAllocation.DAL.DTO;
using API.Models;
using API.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly HeadCountService _headCountService;
        private readonly EmployeeService _employeeService;

        public ManagerController(HeadCountService headCountService, EmployeeService employeeService)
        {
            _headCountService = headCountService;
            _employeeService = employeeService;
        }

        [HttpDelete("Ticket/{ticketId}")]
        public ActionResult<Response> CloseTicket([FromRoute] int ticketId)
        {
            try
            {
                var response = _headCountService.CloseTicket(ticketId);   
                return Ok(Response<bool>.FromValue(!response.ErrorOccured));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("Tickets")]
        public ActionResult<Response> GetOpenTickets()
        {
            try
            {
                var response = _headCountService.GetOpensTickets();   
                var tickets = response.Value.Select(ticket => new Ticket
                {
                    TicketId = ticket.TicketId,
                    EmployeeId = ticket.EmployeeId,
                    EmployeeName = ticket.EmployeeName,
                    StartDate = ticket.StartDate,
                    EndDate = ticket.EndDate,
                    Description = ticket.Description
                }).ToList();
                return Ok(Response<List<Ticket>>.FromValue(tickets));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }        
        
    }
}
