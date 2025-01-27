using Microsoft.AspNetCore.Mvc;
using System;
using HeadcountAllocation.Services;
using HeadcountAllocation.DAL.DTO;
using API.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly HeadCountService _headCountService;

        public EmployeeController(HeadCountService headCountService)
        {
            _headCountService = headCountService;
        }

        [HttpPost("{employeeId}/Assign")]
        public ActionResult<Response> AssignToRole([FromRoute]int employeeId, [FromBody]Role role)
        {            
            try
            {
                return Ok(_headCountService.AssignEmployeeToRole(employeeId, (HeadcountAllocation.Domain.Role)role));
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message, stackTrace = ex.StackTrace});
            }
        }

        [HttpGet("All")]
        public ActionResult<Response> GetAllEmployees()
        {            
            try
            {
                return Ok(_headCountService.GetAllEmployees());
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message, stackTrace = ex.StackTrace});
            }
        }
    }
}
