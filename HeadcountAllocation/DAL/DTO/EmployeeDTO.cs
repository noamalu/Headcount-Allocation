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
    [Table("Employees")]
    public class EmployeeDTO
    {
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmployeeId { get; set; }
        public string UserName {get; set;}
        public string PhoneNumber { get; set; }

        public string Email {get; set;}

        public int TimeZone {get; set;}

        public List<LanguagesDTO> ForeignLanguages {get; set;}

        public double JobPercentage {get; set;}
        public List<SkillDTO> Skills {get; set;}
        public List <RoleDTO> Roles {get; set;}
        public int YearExp {get; set;}

        public EmployeeDTO() { }
        public EmployeeDTO(int employeeId, string name, string phoneNum, string email, int timeZone,
         List<LanguagesDTO> foreignLanguages, double jobPercentage, List<SkillDTO> skills, List <RoleDTO> roles, int yearExp )
        {
            EmployeeId = employeeId;
            UserName = name;
            PhoneNumber = phoneNum;
            Email = email;
            TimeZone = timeZone;
            ForeignLanguages = foreignLanguages;
            JobPercentage = jobPercentage;
            Skills = skills;
            Roles = roles;
            YearExp = yearExp;
        }

         public EmployeeDTO(Employee employee)
        {
            EmployeeId = employee.EmployeeId;
            UserName = employee.Name;
            PhoneNumber = employee.PhoneNumber;
            Email = employee.EmailAddress;
            TimeZone = employee.TimeZone;
            ForeignLanguages = new List<LanguagesDTO>();
            foreach (var language in employee.ForeignLanguages)
            {
                ForeignLanguages.Add(new LanguagesDTO(language.Value));
            }
            JobPercentage = employee.JobPercentage;
            Skills = new List<SkillDTO>();
            foreach (var skill in employee.Skills)
            {
                Skills.Add(new SkillDTO(skill.Value));
            }
            Roles = new List<RoleDTO>();
            foreach (var role in employee.Roles)
            {
                Roles.Add(new RoleDTO(role));
            }
            YearExp =employee.YearsExperience;
        }
    }
}