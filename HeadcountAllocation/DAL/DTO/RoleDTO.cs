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
        public int RoleId { get; }
        
        [Key, Column(Order = 0)]
        [ForeignKey("Projects")]
        public int ProjectId { get; }
        
        [ForeignKey("Employees")]
        public int? EmployeeId { get; }
        public TimeZones TimeZone{get;set;}
        
        // public List<string>? ForeignLanguages{get;set;}

        // public List<SkillDTO> Skills{get;set;}

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}


         public RoleDTO() { }
        public RoleDTO(int roleId, int projectId, int employeeId, TimeZones timeZone,
         List<String> foreignLanguages, double jobPercentage, List<SkillDTO> skills, int yearExp )
        {
            RoleId = roleId;
            ProjectId = projectId;
            EmployeeId = employeeId;
            TimeZone = timeZone;
            // ForeignLanguages = foreignLanguages;
            JobPercentage = jobPercentage;
            // Skills = skills;
   
            YearsExperience = yearExp;
        }

         public RoleDTO(Role role)
        {
            RoleId = role.RoleId;
            ProjectId = role.ProjectId;
            EmployeeId = role.EmployeeId;
            TimeZone = role.TimeZone;
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