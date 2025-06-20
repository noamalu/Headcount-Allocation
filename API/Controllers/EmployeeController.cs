using Microsoft.AspNetCore.Mvc;
using System;
using HeadcountAllocation.Services;
using HeadcountAllocation.DAL.DTO;
using API.Models;
using API.Services;
using WebSocketSharp.Server;
using HeadcountAllocation.Domain.Alert;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly WebSocketServer AlertServer;
        private readonly HeadCountService _headCountService;
        private readonly EmployeeService _employeeService;
        private readonly ProjectService _projectService;
        public EmployeeController(HeadCountService headCountService, EmployeeService employeeService, ProjectService projectService, WebSocketServer alerts)
        {
            AlertServer = alerts;
            _headCountService = headCountService;
            _employeeService = employeeService;
            _projectService = projectService;
            AlertManager.GetInstance(AlertServer);

        }

        private class AlertService : WebSocketBehavior
        {

        }

        [HttpPost("Login")]
        public ActionResult<Response> EmployeeLogin([FromQuery] string userName, [FromBody] string password)
        {
            string relativePath = $"/{userName}-alerts";
            try
            {
                if (AlertServer.WebSocketServices[relativePath] == null)
                {
                    AlertServer.AddWebSocketService<AlertService>(relativePath);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("alert server issue");
            }

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

        [HttpGet("All/Assign")]
        public ActionResult<Response> GetEmployeesToAssign()
        {
            try
            {
                return Ok(Response<List<Employee>>.FromValue(_headCountService.GetAllEmployees().Value.Select(emp => (Employee)_employeeService.TranslateEmployee(emp)).Where(emp => !emp.IsManager).ToList()));
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

        [HttpPut("{employeeId}")]
        public ActionResult<Response> EditEmployee([FromRoute] int employeeId, [FromBody] Employee employee)
        {
            try
            {
                var doomainEmployee = _headCountService.GetEmployeeById(employeeId).Value;
                return Ok(_employeeService.EditEmployee((HeadcountAllocation.Domain.Employee)employee));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{employeeId}/Admin")]
        public ActionResult<Response> IsAdmin([FromRoute] int employeeId)
        {
            try
            {
                return Ok(Response<bool>.FromValue(_employeeService.IsAdmin(employeeId)));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("{employeeId}/Ticket")]
        public ActionResult<Response> OpenTicket([FromRoute] int employeeId, [FromBody] Ticket ticket)
        {
            var reasonToParse = ticket.AbsenceReason.Replace(" ", "");
            var reason = Enum.TryParse(reasonToParse, out HeadcountAllocation.Domain.Enums.Reasons parsedReason)
                ? parsedReason
                : HeadcountAllocation.Domain.Enums.Reasons.Other;
            try
            {
                var response = _headCountService.AddTicket(employeeId, ticket.StartDate, ticket.EndDate, ticket.Description, new(reason));
                return Ok(Response<int>.FromValue(response.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{employeeId}/Ticket")]
        public ActionResult<Response> EditTicket([FromRoute] int employeeId, [FromBody] Ticket ticket)
        {
            try
            {
                var response = _headCountService.EditTicket(employeeId, (HeadcountAllocation.Domain.Ticket)ticket);
                return Ok(new Response());
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{employeeId}/Ticket")]
        public ActionResult<Response> CloseTicket([FromRoute] int employeeId, [FromBody] Ticket ticket)
        {
            try
            {
                _headCountService.CloseTicket(ticket.TicketId);
                return Ok(new Response());
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{employeeId}/Ticket")] //TODO: ****Check if this is correct, it should be a GET request to get the tickets of an employee
        public ActionResult<Response> GetTickets([FromRoute] int employeeId)
        {
            try
            {
                var response = _headCountService.GetOpensTickets().Value
                    .Where(ticket => ticket.EmployeeId == employeeId)
                    .Select(ticket =>
                    {
                        return new Ticket
                        {
                            TicketId = ticket.TicketId,
                            EmployeeId = ticket.EmployeeId,
                            EmployeeName = ticket.EmployeeName,
                            StartDate = ticket.StartDate,
                            EndDate = ticket.EndDate,
                            AbsenceReason = ticket.Reason.ReasonType.ToString(),
                            Description = ticket.Description
                        };
                    }).ToList();
                return Ok(Response<List<Ticket>>.FromValue(response));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{employeeId}/Roles")]//TODO: ****Check if this is correct, it should be a GET request to get the roles of an employee in a project
        public ActionResult<Response<Role>> GetRoles([FromRoute] int employeeId)
        {
            try
            {
                var roles = _headCountService.GetAllRolesByEmployee(employeeId).Value
                      .Select(role => new Role
                      {
                          RoleId = role.RoleId,
                          RoleName = role.RoleName,
                          ProjectId = role.ProjectId,
                          EmployeeId = role.EmployeeId,
                          TimeZone = HeadcountAllocation.Domain.Enums.GetId(role.TimeZone),
                          ForeignLanguages = role.ForeignLanguages.Values?.Select(language => new Language
                          {
                              LanguageId = language.LanguageID,
                              LanguageTypeId = HeadcountAllocation.Domain.Enums.GetId(language.LanguageType),
                              Level = language.Level
                          }).ToList() ?? new(),
                          Skills = role.Skills.Values?.Select(skill => new Skill
                          {
                              SkillTypeId = HeadcountAllocation.Domain.Enums.GetId(skill.SkillType),
                              Level = skill.Level,
                              Priority = skill.Priority
                          }).ToList() ?? new(),
                          YearsExperience = role.YearsExperience,
                          JobPercentage = role.JobPercentage,
                          Description = role.Description
                      }).ToList();
                return Ok(Response<List<Role>>.FromValue(roles));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

    }
}