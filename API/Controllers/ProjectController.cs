using Microsoft.AspNetCore.Mvc;
using HeadcountAllocation.Services;
using static HeadcountAllocation.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using API.Models;
using System.Collections.Concurrent;
using System;
using API.Services;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly HeadCountService _headCountService;
        private readonly ProjectService _projectService;
        private readonly EmployeeService _employeeService;


        public ProjectController(HeadCountService headcountService, ProjectService projectService, EmployeeService employeeService)
        {
            _projectService = projectService;
            _headCountService = headcountService;
            _employeeService = employeeService;
        }

        [HttpPost("Create")]
        public ActionResult<Response<int>> Create([Required][FromBody] Project project)
        {
            var projectId = _headCountService.CreateProject(project.ProjectName, project.Description, project.Deadline, project.RequiredHours, new());
            return Ok(projectId);
        }

        [HttpGet("All")]
        public ActionResult<Response> GetAllProjects()
        {
            var projects = _headCountService.GetAllProjects().Value.Select(Project => new Project
            {
                ProjectName = Project.ProjectName,
                ProjectId = Project.ProjectId,
                Description = Project.Description,
                Deadline = Project.Date,
                RequiredHours = Project.RequiredHours
            }).ToList();
            return Ok(Response<List<Project>>.FromValue(projects));
        }



        // [HttpGet("/{projectId}")] 
        // public ActionResult<Response<Project>> GetProjectById([Required][FromRoute]int projectId)
        // {            
        //     var project = _headCountService.GetProjectById(projectId);
        //     return Ok(project);
        // }

        [HttpPut("/{projectId}/Edit")]
        public async Task<ActionResult<Response>> EditProject([Required][FromRoute] int projectId, [Required][FromBody] Project project)
        {
            var tasks = new Task[]
            {
                    Task.Run(() => _headCountService.EditProjectDate(projectId, project.Deadline)),
                    Task.Run(() => _headCountService.EditProjectDescription(projectId, project.Description)),
                    Task.Run(() => _headCountService.EditProjectName(projectId, project.ProjectName)),
                    Task.Run(() => _headCountService.EditProjectRequierdHours(projectId, project.RequiredHours))
            };

            await Task.WhenAll(tasks);

            return Ok(new());
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
                return BadRequest(new {error = ex.Message, stackTrace = ex.StackTrace});
            }
        }

        [HttpPost("{projectId}/Roles")]
        public async Task<ActionResult> AddRole([Required][FromRoute] int projectId,
            [Required][FromBody] List<Role> roles)
        {
            try
            {
                return Ok(await _projectService.LinkRolesToProject(projectId, roles));
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message, stackTrace = ex.StackTrace});
            }
        }

        [HttpGet("{projectId}/Roles")]//TODO: ****Check if this is correct, it should be a GET request to get the roles in a project
        public ActionResult<Response<Role>> GetRoles([Required][FromRoute] int projectId)
        {
            try
            {
                var roles = _projectService.GetRolesByProject(projectId);
                return Ok(Response<List<Role>>.FromValue(roles));
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message, stackTrace = ex.StackTrace});
            }
        }

        [HttpGet("{projectId}/Roles/{roleId}/Assign")]
        public ActionResult<Response<List<EmployeeOption>>> GetMatchedEmployees([Required][FromRoute] int projectId, [Required][FromRoute] int roleId)
        {
            try
            {
                var roles = _headCountService.GetAllRolesByProject(projectId);
                roles.Value.TryGetValue(roleId, out HeadcountAllocation.Domain.Role role);
                var employees = _headCountService.EmployeesToAssign(role).Value
                    .OrderByDescending(kvp => kvp.Value)
                    .Select(kvp => {var emp = (EmployeeOption)_employeeService.TranslateEmployee(kvp.Key); emp.Score = kvp.Value; return emp;})
                    .ToList();
                return Ok(Response<List<EmployeeOption>>.FromValue(employees));
            }
            catch (Exception ex)
            {
                return BadRequest(new {error = ex.Message, stackTrace = ex.StackTrace});
            }
        }        

    }
}

