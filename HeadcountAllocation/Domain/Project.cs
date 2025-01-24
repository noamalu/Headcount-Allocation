using System.Collections.Concurrent;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.DAL.Repositories;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Project{

        public string? ProjectName{get;set;}

        public int ProjectId{get;set;}

        public string? Description{get;set;}

        public DateTime Deadline{get;set;}

        public int RequiredHours{get;set;}

        public Dictionary<int, Role> Roles{get;set;} = new();

        public RoleRepo RoleRepo;
        private int RoleCounter = 1;

        public Project(string projectName, int projectId, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles){
            ProjectName = projectName;
            ProjectId = projectId;
            Description = description;
            Deadline = date;
            RequiredHours = requiredHours;
            Roles = roles;
            RoleRepo = RoleRepo.GetInstance();
        }

        public Project(ProjectDTO projectDTO)
        {
           ProjectName = projectDTO.ProjectName;
            ProjectId = projectDTO.ProjectId;
            Description = projectDTO.Description;
            Deadline = projectDTO.Date;
            RequiredHours = projectDTO.RequiredHours;
            foreach (RoleDTO roleDTO in projectDTO.Roles){
                Roles[roleDTO.RoleId] = new Role(roleDTO);
            }
            RoleRepo = RoleRepo.GetInstance();
        }

        public Role AddRoleToProject(string roleName, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string description){
            Role role = new Role(roleName, RoleCounter++, ProjectId, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage, description);
            Roles.Add(role.RoleId, role);
            try{
                Roles.Add(role.RoleId, role);
                RoleRepo.Add(role);
                return role;
            }
            catch (Exception e){
                throw new Exception(e.Message);
            }
            
        }

        public void RemoveRole(int roleId){
            if (!Roles.ContainsKey(roleId)){
                throw new Exception($"No such role {roleId}");
            }
            try{
                RoleRepo.Delete(Roles[roleId]);
            }
            catch (Exception e){
                throw new Exception(e.Message);
            }
            Roles.Remove(roleId);
        }

        public Dictionary<int, Role> GetAllRolesByProject(){
            return Roles;
        }

        public void EditProjectName(string projectName){
            ProjectName = projectName;   
        } 

        public void EditProjectDescription(string projectDescription){
            Description = projectDescription;   
        } 

        public void EditProjectDate(DateTime date){
            Deadline = date;   
        } 

        public void EditProjectRequierdHours(int requiredHours){
            RequiredHours = requiredHours;   
        } 


    }


}