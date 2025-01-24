using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.Domain;
using Microsoft.Net.Http.Headers;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.DAL.DTO
{
    [Table("Roles")]
    public class RoleDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        
        [ForeignKey("Projects")]
        public int ProjectId { get; set; }
        
        [ForeignKey("Employees")]
        public int? EmployeeId { get; set; }
        public int TimeZoneId{get;set;}
        
        public List<RoleLanguagesDTO> ForeignLanguages {get; set;}

        public List<RoleSkillsDTO> Skills {get; set;}

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}

        public string Description{get;set;}


         public RoleDTO() { }
        public RoleDTO(int roleId, string roleName, int projectId, int employeeId, int timeZoneId,
         List<RoleLanguagesDTO> foreignLanguages, List<RoleSkillsDTO> skills, double jobPercentage, int yearExp, string description)
        {
            RoleId = roleId;
            RoleName = string.IsNullOrWhiteSpace(roleName) ? throw new ArgumentNullException("it is hereeeeeee") : roleName;
            ProjectId = projectId;
            EmployeeId = employeeId;
            TimeZoneId = timeZoneId;
            ForeignLanguages = foreignLanguages;
            JobPercentage = jobPercentage;
            Skills = skills;
            YearsExperience = yearExp;
            Description = description;
        }

         public RoleDTO(Role role)
        {
            RoleId = role.RoleId;
            RoleName = role.RoleName;
            ProjectId = role.ProjectId;
            EmployeeId = role.EmployeeId;
            TimeZoneId = Enums.GetId(role.TimeZone);
            List<RoleLanguagesDTO> roleLanguages = new List<RoleLanguagesDTO>();
            foreach (Language language in role.ForeignLanguages.Values){
                RoleLanguagesDTO roleLanguagesDTO = new RoleLanguagesDTO(language);
                roleLanguages.Add(roleLanguagesDTO);
            }
            ForeignLanguages = roleLanguages;
            JobPercentage = role.JobPercentage;
            List<RoleSkillsDTO> roleSkills = new List<RoleSkillsDTO>();
            foreach (Skill skill in role.Skills.Values){
                RoleSkillsDTO roleSkillDTO = new RoleSkillsDTO(skill);
                roleSkills.Add(roleSkillDTO);
            }
            Skills = roleSkills;
            YearsExperience = role.YearsExperience;
            Description = role.Description;
        }

    }


}