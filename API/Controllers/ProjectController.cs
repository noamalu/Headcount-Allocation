using Microsoft.AspNetCore.Mvc;
using HeadcountAllocation.Services;
using static HeadcountAllocation.Domain.Enums;
using HeadcountAllocation.DAL.DTO;
using System.ComponentModel.DataAnnotations;
using API.Models;
using System.Collections.Concurrent;
using System;
using API.Services;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly HeadCountService _headCountService;
        private readonly ProjectService _projectService;


        public ProjectController(HeadCountService headcountService, ProjectService projectService)
        {
            _projectService = projectService;
            _headCountService = headcountService;
        }

        [HttpPost("Create")] 
        public ActionResult<Response<int>> Create([Required][FromBody]Project project)
        {            
            var projectId = _headCountService.CreateProject(project.ProjectName, project.Description, project.Deadline, project.RequiredHours, new());
            return Ok(projectId);
        }

        // [HttpGet("All")] 
        // public ActionResult<Response<int>> GetAllProjects()
        // {            
        //     var projects = _headCountService.GetAllProjects();
        //     return Ok(projects);
        // }

        // [HttpGet("/{projectId}")] 
        // public ActionResult<Response<Project>> GetProjectById([Required][FromRoute]int projectId)
        // {            
        //     var project = _headCountService.GetProjectById(projectId);
        //     return Ok(project);
        // }

        [HttpDelete("Delete/{projectId}")]
        public ActionResult<Response> Delete([Required][FromRoute]int projectId)
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
        public async Task<ActionResult> AddRole([Required][FromRoute]int projectId, 
            [Required][FromBody]List<Role> roles)
        {
            try
            {       
                return Ok(await _projectService.LinkRolesToProject(projectId, roles));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{projectId}/Roles")]
        public ActionResult<Response<Role>> GetRoles([Required][FromRoute]int projectId)
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

