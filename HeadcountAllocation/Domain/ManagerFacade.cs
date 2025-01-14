using System.Collections.Concurrent;
using HeadcountAllocation.DAL.Repositories;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class ManagerFacade{

        public Dictionary<int, Project> Projects{get;set;} = new();

        public Dictionary<int, Employee> Employees{get;set;} = new();

        private readonly EmployeeRepo employeeRepo;

        private readonly ProjectRepo projectRepo;

        public int projectCount = 0;

        public int employeeCount = 0;

        public ManagerFacade()
        {
            projectRepo = ProjectRepo.GetInstance();
            employeeRepo = EmployeeRepo.GetInstance();
        }

        public void CreateProject(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles){
            Project project = new Project(projectName, projectCount++, description, date, requiredHours, roles);
            Projects.Add(projectCount, project);
            try{
                projectRepo.Add(project);
            }
            catch (Exception e){
                throw new Exception(e.Message);
            }
            
        }

        public void DeleteProject(int projectId){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null){
                throw new Exception($"No such project {projectId}");
            }
            Projects.Remove(projectCount);
            try{
                projectRepo.Delete(projectId);
            }
            catch (Exception e){
                throw new Exception($"No such project {projectId}");
            }
        }


        public void AddRoleToProject(string roleName, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].AddRoleToProject(roleName, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage);
        }

        public Dictionary<int, Role> GetAllRolesByProject(int projectId){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            return Projects[projectId].GetAllRolesByProject();
        } 

        public void AssignEmployeeToRole(int employeeId, Role role){
            if (!Employees.ContainsKey(employeeId)){
                throw new Exception($"No such employee {employeeId}");
            }
            bool RoleExists = false;
            foreach (Project project in Projects.Values){
                if (project.Roles.ContainsKey(role.RoleId)){
                    RoleExists = true;
                }
            }
            if (!RoleExists){
                throw new Exception($"No such role {role.RoleId}");
            }
            Employees[employeeId].AssignEmployeeToRole(role);
        }


    }
}