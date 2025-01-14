using System.Collections.Concurrent;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.DAL.Repositories;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Project{

        public string? ProjectName{get;set;}

        public int ProjectId{get;set;}

        public string? Description{get;set;}

        public DateTime Date{get;set;}

        public int RequiredHours{get;set;}

        public Dictionary<int, Role> Roles{get;set;} = new();

        public RoleRepo RoleRepo;
        private int RoleCounter = 0;

        public Project(string projectName, int projectId, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles){
            ProjectName = projectName;
            ProjectId = projectId;
            Description = description;
            Date = date;
            RequiredHours = requiredHours;
            Roles = roles;
            RoleRepo = RoleRepo.GetInstance();
        }

        public Project(ProjectDTO projectDTO)
        {
           ProjectName = projectDTO.ProjectName;
            ProjectId = projectDTO.ProjectId;
            Description = projectDTO.Description;
            Date = projectDTO.Date;
            RequiredHours = projectDTO.RequiredHours;
            foreach (RoleDTO roleDTO in projectDTO.Roles){
                Roles[roleDTO.RoleId] = new Role(roleDTO);
            }
            RoleRepo = RoleRepo.GetInstance();
        }

        public void AddRoleToProject(string roleName, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage){
            Role role = new Role(roleName, RoleCounter++, ProjectId, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage);
            Roles.Add(role.RoleId, role);
            try{
                RoleRepo.Add(role);
            }
            catch (Exception e){
                throw new Exception(e.Message);
            }
            
        }

        public Dictionary<int, Role> GetAllRolesByProject(){
            return Roles;
        } 


    }


}