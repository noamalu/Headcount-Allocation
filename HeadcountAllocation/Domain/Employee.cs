using System.Collections.Concurrent;
using System.Net.Mail;
using HeadcountAllocation.DAL.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Employee: User
    {

        PasswordHasher<object> passwordHasher;

        public int EmployeeId{get;set;}

        public string PhoneNumber{get;set;}

        public TimeZones TimeZone{get;set;}
        
        public ConcurrentDictionary<int, Language> ForeignLanguages{get;set;} = new();

        public ConcurrentDictionary<int, Skill> Skills{get;set;} = new();

        public Dictionary<int, Role> Roles{get;set;} = new();

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}

        public bool IsManager{get; set;}

        public Employee(string name, int employeeId, string phoneNumber, MailAddress email, 
        TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages, 
        ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string password, bool isManager){
            UserName = name;
            EmployeeId = employeeId;
            PhoneNumber = phoneNumber;
            Email = email;
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
            UserName = employeeDto.UserName;
            EmployeeId = employeeDto.EmployeeId;
            PhoneNumber = employeeDto.PhoneNumber;
            Email = new MailAddress(employeeDto.Email);
            TimeZone = Enums.GetValueById<TimeZones>(employeeDto.TimeZone);
            YearsExperience = employeeDto.YearExp;
            JobPercentage = employeeDto.JobPercentage;
            Password = employeeDto.Password;
            if (employeeDto.Roles != null){
                foreach (RoleDTO roleDTO in employeeDto.Roles){
                    Roles[roleDTO.RoleId] = new Role(roleDTO);
                }
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

        public double CalculateJobPercentage(){
            double sum =0;
            foreach (var role in Roles.Values){
                sum = sum + role.JobPercentage;
            }
            return sum;
        }

        public void AssignEmployeeToRole(Role role){
            Roles.Add(role.RoleId, role);
        }

        public bool Login(){
            return IsManager;
        }

        public void EditEmail(MailAddress newEmail){
            Email = newEmail;
        }

        public void EditPhoneNumber(string newPhoneNumber){
            PhoneNumber = newPhoneNumber;
        }

        public void EditTimeZone(TimeZones newTimeZone){
            TimeZone = newTimeZone;
        }

        public void EditYearOfExpr(int newyearOfExpr){
            YearsExperience = newyearOfExpr;
        }

        public void EditJobPercentage(double newJobPercentage){
            JobPercentage = newJobPercentage;
        }

        public ConcurrentDictionary<int, Skill> GetSkills(){
            return Skills;
        }

        public void AddSkill(Skill newSkill){
            Skills.TryAdd(newSkill.SkillId, newSkill);
        }

        public void RemoveSkill(int skillId){
            Skills.Remove(skillId, out _);
        }

        public ConcurrentDictionary<int, Language> GetLanguages(){
            return ForeignLanguages;
        }

        public void AddLanguage(Language newLanguage){
            ForeignLanguages.TryAdd(newLanguage.LanguageID, newLanguage);
        }

        public void RemoveLanguage(int languageID){
            ForeignLanguages.Remove(languageID, out _);
        }

    }
}