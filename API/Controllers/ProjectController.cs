using Microsoft.AspNetCore.Mvc;
using HeadcountAllocation.Domain;
using HeadcountAllocation.Services;
using static HeadcountAllocation.Domain.Enums;
using HeadcountAllocation.DAL.DTO;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly HeadCountService _headCountService;

        public ProjectController(HeadCountService headcountService)
        {
            _headCountService = headcountService;
        }

        [HttpPost("Create")] 
        public IActionResult Create([Required][FromBody]Project project)
        {
            _headCountService.CreateProject(project.ProjectName, project.Description, project.Deadline, project.RequiredHours, new());
            return Ok("Project created successfully");
        }

        [HttpDelete("Delete/{projectId}")]
        public IActionResult Delete(int projectId)
        {
            try
            {
                _headCountService.DeleteProject(projectId);
                return Ok("Project deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // [HttpPost("{projectId}/Roles/Add")]
        // public IActionResult AddRole(int projectId, RoleDTO role)
        // {
        //     try
        //     {
        //         _headCountService.AddRoleToProject(role.RoleName, projectId, Enums.GetValueById<TimeZones>(role.TimeZoneId), role.ForeignLanguages, role.Skills, role.YearsExperience, role.JobPercentage);
        //         return Ok("Role added to project successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        // }

        [HttpGet("{projectId}/Roles")]
        public ActionResult<Role> GetRoles(int projectId)
        {
            try
            {
                var roles = _headCountService.GetAllRolesByProject(projectId);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }    

    }
}
