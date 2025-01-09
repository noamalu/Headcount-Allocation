using Microsoft.AspNetCore.Mvc;
using HeadcountAllocation.Domain;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ManagerFacade _managerFacade;

        public ProjectController(ManagerFacade managerFacade)
        {
            _managerFacade = managerFacade;
        }

        [HttpPost("Create")]
        public IActionResult Create(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles)
        {
            _managerFacade.CreateProject(projectName, description, date, requiredHours, roles);
            return Ok("Project created successfully");
        }

        [HttpDelete("Delete/{projectId}")]
        public IActionResult Delete(int projectId)
        {
            try
            {
                _managerFacade.DeleteProject(projectId);
                return Ok("Project deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{projectId}/Roles/Add")]
        public IActionResult AddRole(int projectId, Role role)
        {
            try
            {
                _managerFacade.AddRoleToProject(projectId, role);
                return Ok("Role added to project successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{projectId}/Roles")]
        public ActionResult<Role> GetRoles(int projectId)
        {
            try
            {
                var roles = _managerFacade.GetAllRolesByProject(projectId);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }    

    }
}
