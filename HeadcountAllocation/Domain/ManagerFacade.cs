using System.Collections.Concurrent;
using HeadcountAllocation.DAL.Repositories;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class ManagerFacade{

        private static ManagerFacade managerFacade = null;

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

        public static ManagerFacade GetInstance(){
            if (managerFacade == null){
                managerFacade = new ManagerFacade();
            }
            return managerFacade;
        }

        public static void Dispose(){
            EmployeeRepo.Dispose();
            ProjectRepo.Dispose();
            EmployeeLanguagesRepo.Dispose();
            EmployeeSkillsRepo.Dispose();
            RoleRepo.Dispose();
            RoleLanguagesRepo.Dispose();
            RoleSkillsRepo.Dispose();
            managerFacade = null;
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

        public void EditProjectName(int projectId, string projectName){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectName(projectName);
            projectRepo.Update(Projects[projectId]);
        }

        public void EditProjectDescription(int projectId, string ProjectDescription){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectDescription(ProjectDescription);
            projectRepo.Update(Projects[projectId]);
        }


        public void EditProjectDate(int projectId, DateTime date){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectDate(date);
            projectRepo.Update(Projects[projectId]);
        }

        public void EditProjectRequierdHours(int projectId, int requiredHours){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectRequierdHours(requiredHours);
            projectRepo.Update(Projects[projectId]);
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
                throw new Exception($"No such project {projectId} " + $"{e}");
            }
        }


        public void AddRoleToProject(string roleName, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].AddRoleToProject(roleName, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage);
        }

        public void RemoveRole(int projectId, int roleId){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].RemoveRole(roleId);
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

        public Dictionary <Employee, double> EmployeesToAssign(Role role){
           Dictionary<Employee, double> employees = new Dictionary<Employee, double>();
           foreach (Employee employee in Employees.Values){
            double score = 0;
            bool disqualified = false; 
                if(employee.YearsExperience < role.YearsExperience)
                    disqualified = true;
                
                foreach (Language language in role.ForeignLanguages.Values){
                    //Language roleLanguage = role.ForeignLanguages[language.LanguageID];
                    if(employee.ForeignLanguages.TryGetValue(language.LanguageID, out var employeeLang))
                        if(employeeLang.Level < language.Level)
                            disqualified = true;
                    else
                        disqualified = true;
                }
                if (disqualified == true)
                    continue;
            
                foreach (Skill skill in role.Skills.Values){
                    if (employee.Skills.ContainsKey(skill.SkillId)){
                        Skill employeeSkill = employee.Skills[skill.SkillId];
                        if (employeeSkill.Level == skill.Level)
                            score = score +3 * skill.Priority/10;
                        else if (employeeSkill.Level > skill.Level)
                            score = score +2 * skill.Priority/10;
                        else if (employeeSkill.Level+1 == skill.Level)
                            score = score +1 * skill.Priority/10;
                    }
                }

                employees[employee] = score;
           }

           Dictionary<Employee, double> sortedEmployees= employees.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);;
           return sortedEmployees;
        }


    }
}