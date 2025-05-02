using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO.Alert;
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

        public List<EmployeeLanguagesDTO> ForeignLanguages {get; set;}

        public double JobPercentage {get; set;}
        public List<EmployeeSkillsDTO> Skills {get; set;}
        
         [NotMapped]
        public List <RoleDTO> Roles {get; set;}
        public int YearExp {get; set;}
        public string Password {get; set;}

        public bool IsManager {get; set;}
        public List<MessageDTO> Alerts {get; set;} = new ();

        public bool Alert{get; set;} = true;



        public EmployeeDTO() { }
        public EmployeeDTO(int employeeId, string name, string phoneNum, string email, int timeZone,
         List<EmployeeLanguagesDTO> foreignLanguages, double jobPercentage, List<EmployeeSkillsDTO> skills, List <RoleDTO> roles, int yearExp, string password, bool isManager)
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
            Password = password;
            IsManager = isManager;
        }

         public EmployeeDTO(Employee employee)
        {
            EmployeeId = employee.EmployeeId;
            UserName = employee.UserName;
            PhoneNumber = employee.PhoneNumber;
            Email = employee.Email.Address;
            TimeZone = Enums.GetId(employee.TimeZone);
            ForeignLanguages = new List<EmployeeLanguagesDTO>();
            foreach (var language in employee.ForeignLanguages)
            {
                EmployeeLanguagesDTO employeeLanguage = new EmployeeLanguagesDTO(language.Value);
                ForeignLanguages.Add(employeeLanguage);
            }
            JobPercentage = employee.JobPercentage;
            Skills = new List<EmployeeSkillsDTO>();
            foreach (var skill in employee.Skills)
            {
                EmployeeSkillsDTO employeeSkills = new EmployeeSkillsDTO(skill.Value);
                Skills.Add(employeeSkills);
            }
            Roles = new List<RoleDTO>();
            foreach (var role in employee.Roles.Values)
            {
                Roles.Add(new RoleDTO(role));
            }
            YearExp =employee.YearsExperience;
            Password = employee.Password;
            IsManager = employee.IsManager;
        }
    }
}