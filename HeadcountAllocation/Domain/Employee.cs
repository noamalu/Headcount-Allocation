

using System.Collections.Concurrent;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Employee
    {

        public Employee(EmployeeDTO employeeDto)
        {
            Name = employeeDto.UserName;
            EmployeeId = employeeDto.EmployeeId;
            PhoneNumber = employeeDto.PhoneNumber;
            EmailAddress = employeeDto.Email;
            TimeZone = Enums.GetValueById<TimeZones>(employeeDto.TimeZone);
            YearsExperience = employeeDto.YearExp;
            JobPercentage = employeeDto.JobPercentage;
            foreach (RoleDTO roleDTO in employeeDto.Roles){
                Roles[roleDTO.RoleId] = new Role(roleDTO);
            }
            foreach (EmployeeSkillsDTO skillDTO in employeeDto.Skills){
                Skills[skillDTO.SkillId] = new Skill(skillDTO);
            }
            foreach (EmployeeLanguagesDTO LanguagesDTO in employeeDto.ForeignLanguages){
            ForeignLanguages[LanguagesDTO.LanguageID] = new Language(LanguagesDTO);
          }

        }

        public string? Name {get;set;}

        public int EmployeeId{get;set;}

        public string PhoneNumber{get;set;}

        public string? EmailAddress{get;set;}

        public TimeZones TimeZone{get;set;}
        
        public ConcurrentDictionary<int, Language> ForeignLanguages{get;set;} = new();

        public ConcurrentDictionary<int, Skill> Skills{get;set;} = new();

        public Dictionary<int, Role> Roles{get;set;} = new();

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}



        public void AssignEmployeeToRole(Role role){
            Roles.Add(role.RoleId, role);
        }

    }
}