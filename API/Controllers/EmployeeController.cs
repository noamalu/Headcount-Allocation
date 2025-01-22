using Microsoft.AspNetCore.Mvc;
using HeadcountAllocation.Domain;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ManagerFacade _managerFacade;

        public EmployeeController(ManagerFacade managerFacade)
        {
            _managerFacade = managerFacade;
        }

        [HttpPost("{employeeId}/Assign")]
        public IActionResult AssignToRole(int employeeId, Role role)
        {
            try
            {
                _managerFacade.AssignEmployeeToRole(employeeId, role);
                return Ok("Employee assigned to role successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
