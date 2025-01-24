using System.Collections.Concurrent;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Services{

    public class HeadCountService : IHeadCountService
    {
        private static HeadCountService _headCountService = null;

        private ManagerFacade _managerFacade;

        public HeadCountService()
        {
            _managerFacade = ManagerFacade.GetInstance();
        }

        public static HeadCountService GetInstance(){
            if (_headCountService == null){
                _headCountService = new HeadCountService();
            }
            return _headCountService;
        }

        public void Dispose(){
            ManagerFacade.Dispose();
            _headCountService = null;
        }

        public Response<List<Project>> GetAllProjects(){
            try{
                return Response<List<Project>>.FromValue(_managerFacade.GetAllProjects());
            }
            catch (Exception e){
                return Response<List<Project>>.FromError(e.Message);
            }            
        }

        public Response<int> CreateProject(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles){
            try{
                return Response<int>.FromValue(_managerFacade.CreateProject(projectName, description, date, requiredHours, roles));
            }
            catch (Exception e){
                return Response<int>.FromError(e.Message);
            }            
        }

        public Response EditProjectName(int projectId, string projectName){
            try{
                _managerFacade.EditProjectName(projectId, projectName);
                return new Response();
            }
            catch (Exception e){
                return new Response(e.Message);
            }     
        }

        public Response EditProjectDescription(int projectId, string ProjectDescription){
            try{
                _managerFacade.EditProjectDescription(projectId, ProjectDescription);
                return new Response();
            }
            catch (Exception e){
                return new Response(e.Message);
            }     
        }


        public Response EditProjectDate(int projectId, DateTime date){
            try{
                _managerFacade.EditProjectDate(projectId, date);
                return new Response();
            }
            catch (Exception e){
                return new Response(e.Message);
            }     
        }

        public Response EditProjectRequierdHours(int projectId, int requiredHours){
            try{
                _managerFacade.EditProjectRequierdHours(projectId, requiredHours);
                return new Response();
            }
            catch (Exception e){
                return new Response(e.Message);
            }     
        }

        public Response DeleteProject(int projectId){
            try{
                _managerFacade.DeleteProject(projectId);
                return new Response();
            }
            catch (Exception e){
                return new Response(e.Message);
            }     
        }


        public Response<Role> AddRoleToProject(string roleName, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage){
            try{
                Console.WriteLine("got to manager facade");
                var role = _managerFacade.AddRoleToProject(roleName, projectId, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage);
                return Response<Role>.FromValue(role);
            }
            catch (Exception e){
                return Response<Role>.FromError(e.Message);
            }     
        }

        public Response RemoveRole(int projectId, int roleId){
            try{
                _managerFacade.RemoveRole(projectId, roleId);
                return new Response();
            }
            catch (Exception e){
                return new Response(e.Message);
            }     
        }

        public Response<Dictionary<int, Role>> GetAllRolesByProject(int projectId){
            try{
                Dictionary<int, Role> roles = _managerFacade.GetAllRolesByProject(projectId);
                return Response<Dictionary<int, Role>>.FromValue(roles);
            }
            catch (Exception e){
                return Response<Dictionary<int, Role>>.FromError(e.Message);
            }     
        } 

        public Response AssignEmployeeToRole(int employeeId, Role role){
            try{
                _managerFacade.AssignEmployeeToRole(employeeId, role);
                return new Response();
            }
            catch (Exception e){
                return new Response(e.Message);
            }
        }

        public Project GetProjectById(int projectId)
        {
            var project = _managerFacade.GetProjectById(projectId);
            return project;
        }
    }
}

