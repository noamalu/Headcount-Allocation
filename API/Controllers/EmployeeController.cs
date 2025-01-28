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
                return Ok(Response<List<Employee>>.FromValue(_headCountService.GetAllEmployees().Value.Select(_employeeService.TranslateEmployee).ToList()));
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
    }
}
