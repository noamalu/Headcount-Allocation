using Microsoft.AspNetCore.Mvc;
using HeadcountAllocation.Services;
using static HeadcountAllocation.Domain.Enums;
using HeadcountAllocation.DAL.DTO;
using System.ComponentModel.DataAnnotations;
using API.Models;
using System.Collections.Concurrent;

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
        public ActionResult<Response> Create([Required][FromBody] Project project)
        {
            return Ok(_headCountService.CreateProject(project.ProjectName, project.Description, project.Deadline, project.RequiredHours, new()));
        }

        [HttpDelete("Delete/{projectId}")]
        public ActionResult<Response> Delete([Required][FromRoute] int projectId)
        {
            try
            {
                return Ok(_headCountService.DeleteProject(projectId));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{projectId}/Roles")]
        public ActionResult<Response> AddRole([Required][FromRoute] int projectId,
            [Required][FromBody] Role role)
        {
            try
            {
                // _headCountService.AddRoleToProject(role.RoleName, projectId,
                //     GetValueById<TimeZones>(role.TimeZone), 
                //     role.ForeignLanguages, role.Skills, 
                //     role.YearsExperience, role.JobPercentage);
                return Ok("Role added to project successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{projectId}/Roles")]
        public ActionResult<Response<Role>> GetRoles([Required][FromRoute] int projectId)
        {
            try
            {
                return Ok(_headCountService.GetAllRolesByProject(projectId));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
