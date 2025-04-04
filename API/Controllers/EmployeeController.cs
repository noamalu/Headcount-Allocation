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
    public class EmployeeController : ControllerBase
    {
        private readonly HeadCountService _headCountService;
        private readonly EmployeeService _employeeService;

        public EmployeeController(HeadCountService headCountService, EmployeeService employeeService)
        {
            _headCountService = headCountService;
            _employeeService = employeeService;
        }

        [HttpPost("Login")]
        public ActionResult<Response> EmployeeLogin([FromQuery]string userName, [FromBody]string password)
        {
            try
            {
                var response = _headCountService.Login(userName, password);
                return Ok(Response<int?>.FromValue(response.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("{employeeId}/Assign")]
        public ActionResult<Response> AssignToRole([FromRoute] int employeeId, [FromBody] Role role)
        {
            try
            {
                var response = _headCountService.AssignEmployeeToRole(employeeId, (HeadcountAllocation.Domain.Role)role);
                return Ok(Response<bool>.FromValue(!response.ErrorOccured));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("All")]
        public ActionResult<Response> GetAllEmployees()
        {
            try
            {
                return Ok(Response<List<Employee>>.FromValue(_headCountService.GetAllEmployees().Value.Select(emp => (Employee)_employeeService.TranslateEmployee(emp)).ToList()));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
        
        [HttpGet("{employeeId}")]
        public ActionResult<Response> GetEmployeeById([FromRoute] int employeeId)
        {
            try
            {
                return Ok(Response<Employee>.FromValue(_employeeService.TranslateEmployee(_headCountService.GetEmployeeById(employeeId).Value)));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("{employeeId}/Ticket")]
        public ActionResult<Response> OpenTicket([FromRoute] int employeeId, [FromBody] Ticket ticket)
        {
            try
            {
                var response = _headCountService.AddTicket(employeeId, ticket.StartDate, ticket.EndDate, ticket.Description);   
                return Ok(Response<int>.FromValue(response.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
        
    }
}
