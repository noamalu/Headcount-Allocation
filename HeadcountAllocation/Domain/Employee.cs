using System.Collections.Concurrent;
using HeadcountAllocation.DAL.DTO;
using Microsoft.AspNetCore.Identity;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Employee: User
    {

        public Employee(string name, int employeeId, string phoneNumber, string email, 
        TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages, 
        ConcurrentDictionary<int, Skill> skills, int yearsExperience, int jobPercentage, string password, bool isManager){
            Name = name;
            EmployeeId = employeeId;
            PhoneNumber = phoneNumber;
            EmailAddress = email;
            TimeZone = timezone;
            ForeignLanguages = foreignLanguages;
            Skills = skills;
            YearsExperience = yearsExperience;
            JobPercentage = jobPercentage;
            Password = EncryptPassword(password);
            IsManager = isManager;
        }

        public Employee(EmployeeDTO employeeDto)
        {
            Name = employeeDto.UserName;
            EmployeeId = employeeDto.EmployeeId;
            PhoneNumber = employeeDto.PhoneNumber;
            EmailAddress = employeeDto.Email;
            TimeZone = Enums.GetValueById<TimeZones>(employeeDto.TimeZone);
            YearsExperience = employeeDto.YearExp;
            JobPercentage = employeeDto.JobPercentage;
            Password = employeeDto.Password;
            foreach (RoleDTO roleDTO in employeeDto.Roles){
                Roles[roleDTO.RoleId] = new Role(roleDTO);
            }
            foreach (EmployeeSkillsDTO skillDTO in employeeDto.Skills){
                Skills[skillDTO.SkillTypeId] = new Skill(skillDTO);
            }
            foreach (EmployeeLanguagesDTO LanguagesDTO in employeeDto.ForeignLanguages){
            ForeignLanguages[LanguagesDTO.LanguageTypeId] = new Language(LanguagesDTO);
            }
            IsManager = employeeDto.IsManager;

        }

        public string EncryptPassword(string password)
        {
            passwordHasher = new PasswordHasher<object>();
            return passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string rawPassword, string hashedPassword)
        {
            passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, rawPassword);
            return result == PasswordVerificationResult.Success;
        }

        PasswordHasher<object> passwordHasher;

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

        public bool IsManager{get; set;}



        public void AssignEmployeeToRole(Role role){
            Roles.Add(role.RoleId, role);
        }

    }
}