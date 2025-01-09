using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.Domain;
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


         public RoleDTO() { }
        public RoleDTO(int roleId, string RoleName, int projectId, int employeeId, int timeZoneId,
         List<RoleLanguagesDTO> foreignLanguages, List<RoleSkillsDTO> skills, double jobPercentage, int yearExp )
        {
            RoleId = roleId;
            RoleName = RoleName;
            ProjectId = projectId;
            EmployeeId = employeeId;
            TimeZoneId = timeZoneId;
            ForeignLanguages = foreignLanguages;
            JobPercentage = jobPercentage;
            Skills = skills;
   
            YearsExperience = yearExp;
        }

         public RoleDTO(Role role)
        {
            RoleId = role.RoleId;
            ProjectId = role.ProjectId;
            EmployeeId = role.EmployeeId;
            //TimeZoneId = role.TimeZone;
            // ForeignLanguages = role.ForeignLanguages;
            JobPercentage = role.JobPercentage;
            // Skills = new List<SkillDTO>();
            // foreach (var skill in role.Skills)
            // {
            //     Skills.Add(new SkillDTO(skill.Value));
            // }
            YearsExperience = role.YearsExperience;
        }

    }


}