using Microsoft.AspNetCore.Mvc;
using HeadcountAllocation.Domain;
using System;
using HeadcountAllocation.Services;
using HeadcountAllocation.DAL.DTO;

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
        public ActionResult<Response> AssignToRole(int employeeId, Role role)
        {            
            try
            {
                return Ok(_headCountService.AssignEmployeeToRole(employeeId, role));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
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
                return NotFound(ex.Message);
            }
        }
    }
}
