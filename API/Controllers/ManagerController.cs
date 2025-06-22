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

        [HttpPost("Employees")]
        public ActionResult<Response<int>> Create([FromBody] Employee employee)
        {
            var foreignLanguages = employee.ForeignLanguages.ToDictionary(lang => lang.LanguageTypeId, lang => new HeadcountAllocation.Domain.Language
            (
                HeadcountAllocation.Domain.Enums.GetValueById<HeadcountAllocation.Domain.Enums.Languages>(lang.LanguageTypeId),
                lang.Level
            ));

            var skills = employee.Skills.ToDictionary(skill => skill.SkillId, skill => new HeadcountAllocation.Domain.Skill
            (
                HeadcountAllocation.Domain.Enums.GetValueById<HeadcountAllocation.Domain.Enums.Skills>(skill.SkillTypeId),
                skill.Level,
                skill.Priority
            ));

            var employeeId = _headCountService.AddEmployee(
                employee.EmployeeName,
                employee.Password,
                employee.PhoneNumber,
                employee.Email,
                (HeadcountAllocation.Domain.Enums.TimeZones)employee.TimeZone,
                new System.Collections.Concurrent.ConcurrentDictionary<int, HeadcountAllocation.Domain.Language>(foreignLanguages),
                new System.Collections.Concurrent.ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill>(skills),
                employee.YearsExperience,
                employee.JobPercentage,
                false
            );
            return Ok(employeeId);
        }

        [HttpDelete("Employees")]
        public ActionResult<Response<int>> Delete([FromQuery] int employeeId)
        {
            var response = _headCountService.DeleteEmployee(employeeId);
            return Ok(Response<bool>.FromValue(!response.ErrorOccured));
        }

        [HttpPost("Employees/Admin")]
        public ActionResult<Response<int>> CreateAdmin([FromBody] Employee employee)
        {
            var foreignLanguages = employee.ForeignLanguages.ToDictionary(lang => lang.LanguageId, lang => new HeadcountAllocation.Domain.Language
            (
                HeadcountAllocation.Domain.Enums.GetValueById<HeadcountAllocation.Domain.Enums.Languages>(lang.LanguageTypeId),
                lang.Level
            ));

            var skills = employee.Skills.ToDictionary(skill => skill.SkillId, skill => new HeadcountAllocation.Domain.Skill
            (
                HeadcountAllocation.Domain.Enums.GetValueById<HeadcountAllocation.Domain.Enums.Skills>(skill.SkillTypeId),
                skill.Level,
                skill.Priority
            ));

            var employeeId = _headCountService.AddEmployee(
                employee.EmployeeName,
                employee.Password,
                employee.PhoneNumber,
                employee.Email,
                (HeadcountAllocation.Domain.Enums.TimeZones)employee.TimeZone,
                new System.Collections.Concurrent.ConcurrentDictionary<int, HeadcountAllocation.Domain.Language>(foreignLanguages),
                new System.Collections.Concurrent.ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill>(skills),
                employee.YearsExperience,
                employee.JobPercentage,
                true
            );
            return Ok(employeeId);
        }

        [HttpDelete("Tickets/{ticketId}")]
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

        [HttpGet("Employees/Utilization")]
        public ActionResult GetEmployeesJobPre()
        {
            var response = _headCountService.GetEmployeesJobPre();
            if (response.ErrorOccured)
            {
                return BadRequest(new { error = response.ErrorMessage });
            }
            if (response.Value == null)
            {
                return NotFound();
            }
            var employeeUtilization = response.Value.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(e => new Employee
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.UserName,
                    PhoneNumber = e.PhoneNumber,
                    Email = e.Email.Address,
                    Skills = [.. e.Skills.Values.Select(s => new Skill
                    {
                        SkillId = HeadcountAllocation.Domain.Enums.GetId(s.SkillType),
                        Level = s.Level,
                        Priority = s.Priority
                    })],
                    ForeignLanguages = [.. e.ForeignLanguages.Values.Select(l => new Language
                    {
                        LanguageId = HeadcountAllocation.Domain.Enums.GetId(l.LanguageType),
                        Level = l.Level
                    })],
                    TimeZone = HeadcountAllocation.Domain.Enums.GetId(e.TimeZone),
                    YearsExperience = e.YearsExperience,
                    JobPercentage = e.JobPercentage,
                    IsManager = e.IsManager
                }).ToList()
            );
            return Ok(employeeUtilization);
        }

        [HttpGet("Projects/Deadlines")]
        public ActionResult<Response<List<Project>>> GetProjectsThatEndThisMonth()
        {
            var response = _headCountService.GetProjectsThatEndThisMonth();
            var projects = response.Value.Select(p => new Project
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                Description = p.Description,
                Deadline = p.Date,
                RequiredHours = p.RequiredHours
            }).ToList();
            return Ok(projects);
        }

        [HttpGet("Projects/Employees/Count")]
        public ActionResult<Response<Dictionary<Project, int>>> GetNumEmployeesInProject()
        {
            var response = _headCountService.GetNumEmployeesInProject();
            var projectEmployeeCount = response.Value.Select(kvp => new
            {
                Project = new Project
                {
                    ProjectId = kvp.Key.ProjectId,
                    ProjectName = kvp.Key.ProjectName,
                    Description = kvp.Key.Description,
                    Deadline = kvp.Key.Date,
                    RequiredHours = kvp.Key.RequiredHours
                },
                Count = kvp.Value
            }).ToList();
            return Ok(projectEmployeeCount);
        }

        [HttpGet("Employees/Vacation")]
        public ActionResult<Response<List<Employee>>> GetEmployeesThatInVacationThisMonth()
        {
            var response = _headCountService.GetEmployeesThatInVacationThisMonth();
            var employees = response.Value.Select(e => new Employee
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.UserName,
                PhoneNumber = e.PhoneNumber,
                Email = e.Email.Address,
                Skills = [.. e.Skills.Values.Select(s => new Skill
                {
                    SkillId = HeadcountAllocation.Domain.Enums.GetId(s.SkillType),
                    Level = s.Level,
                    Priority = s.Priority
                })],
                ForeignLanguages = [.. e.ForeignLanguages.Values.Select(l => new Language
                {
                    LanguageId = HeadcountAllocation.Domain.Enums.GetId(l.LanguageType),
                    Level = l.Level
                })],
                TimeZone = HeadcountAllocation.Domain.Enums.GetId(e.TimeZone),
                YearsExperience = e.YearsExperience,
                JobPercentage = e.JobPercentage,
                IsManager = e.IsManager
            }).ToList();
            return Ok(response);
        }

        [HttpGet("Projects/Hours")]
        public ActionResult GetProjectHourRatio()
        {
            var response = _headCountService.GetProjectHourRatio();
            var projectHourRatio = response.Value.Select(kvp => new
            {
                Project = new Project
                {
                    ProjectId = kvp.Key.ProjectId,
                    ProjectName = kvp.Key.ProjectName,
                    Description = kvp.Key.Description,
                    Deadline = kvp.Key.Date,
                    RequiredHours = kvp.Key.RequiredHours
                },
                Hours = kvp.Value is double.NaN ? 0 : kvp.Value 
            }).ToList();
            return Ok(projectHourRatio);
        }

        [HttpGet("Employees/Vacation/Reasons")]
        public ActionResult<Response<Dictionary<string, List<Employee>>>> GetEmployeesThatInVacationThisMonthAndReason()
        {
            try
            {
                var response = _headCountService.GetEmployeesThatInVacationThisMonthAndReason();
                if (response.ErrorOccured)
                {
                    return BadRequest(new { error = response.ErrorMessage });
                }
                if (response.Value == null)
                {
                    return NotFound();
                }
                var employeeReasons = response.Value.ToDictionary(
                    kvp => kvp.Key.ToString(),
                    kvp => kvp.Value.Select(e => new Employee
                    {
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.UserName,
                        PhoneNumber = e.PhoneNumber,
                        Email = e.Email.Address,
                        Skills = [.. e.Skills.Values.Select(s => new Skill
                        {
                            SkillId = HeadcountAllocation.Domain.Enums.GetId(s.SkillType),
                            Level = s.Level,
                            Priority = s.Priority
                        })],
                        ForeignLanguages = [.. e.ForeignLanguages.Values.Select(l => new Language
                        {
                            LanguageId = HeadcountAllocation.Domain.Enums.GetId(l.LanguageType),
                            Level = l.Level
                        })],
                        TimeZone = HeadcountAllocation.Domain.Enums.GetId(e.TimeZone),
                        YearsExperience = e.YearsExperience,
                        JobPercentage = e.JobPercentage,
                        IsManager = e.IsManager
                    }).ToList()
                );
                return Ok(employeeReasons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
        
    }
}
