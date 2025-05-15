using System.Collections.Concurrent;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Role{


        public string RoleName{get;set;}

        public int RoleId{get;set;}

        public int? EmployeeId{get;set;}

        public int ProjectId{get;set;}

        public TimeZones TimeZone{get;set;}
        
        public ConcurrentDictionary<int, Language> ForeignLanguages{get;set;} = new();

        public ConcurrentDictionary<int, Skill> Skills{get;set;} = new();

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}

        public string Description{get;set;}

        public Role(string roleName, int roleId, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string description)
        {
          RoleName = roleName;
          RoleId = roleId;
          ProjectId = projectId;
          TimeZone = timeZone;
          ForeignLanguages = foreignLanguages;
          Skills = skills;
          YearsExperience = yearsExperience;
          JobPercentage = jobPercentage;
          Description = description;
        }

         public Role(RoleDTO roleDTO)
        {
          RoleName = roleDTO.RoleName;
          RoleId =  roleDTO.RoleId;
          EmployeeId = roleDTO.EmployeeId;
          ProjectId = roleDTO.ProjectId;
          TimeZone = Enums.GetValueById<TimeZones>(roleDTO.TimeZoneId);
          foreach (RoleLanguagesDTO roleLanguagesDTO in roleDTO.ForeignLanguages){
            ForeignLanguages[roleLanguagesDTO.LanguageTypeId]=new Language(roleLanguagesDTO);
          }
          TimeZone = Enums.GetValueById<TimeZones>(roleDTO.TimeZoneId);
          YearsExperience = roleDTO.YearsExperience;
          foreach (RoleSkillsDTO skillDTO in roleDTO.Skills){
                Skills[skillDTO.SkillTypeId] = new Skill(skillDTO);
          }
          JobPercentage = roleDTO.JobPercentage;
          Description = roleDTO.Description;
          

        }

        public void RemoveEmployeeAssign(){
          EmployeeId = null;
        }
    }
}