using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;

namespace API.Models
{
    public class Employee
    {
        public string? EmployeeName { get; set; }

        public int EmployeeId { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public int TimeZone { get; set; }

        public List<Language> ForeignLanguages { get; set; } = new();

        public List<Skill> Skills { get; set; } = new();

        public int YearsExperience { get; set; }

        public double JobPercentage { get; set; }

        public bool IsManager { get; set; } = false;

        public string? Password { get; set; }

        public static explicit operator HeadcountAllocation.Domain.Employee (Employee employee)
        {
            var foreignLanguages = employee.ForeignLanguages.ToDictionary(lang => lang.LanguageId, lang => new HeadcountAllocation.Domain.Language
            (
                HeadcountAllocation.Domain.Enums.GetValueById<HeadcountAllocation.Domain.Enums.Languages>(lang.LanguageTypeId),
                lang.Level
            ));

            var skills = employee.Skills.ToDictionary(skill => skill.SkillId, skill => new HeadcountAllocation.Domain.Skill
            (
                HeadcountAllocation.Domain.Enums.GetValueById<HeadcountAllocation.Domain.Enums.Skills>(skill.SkillTypeId),
                skill.Level,
                skill.Priority
            ));

            return new HeadcountAllocation.Domain.Employee
            (
                employee.EmployeeName,
                employee.EmployeeId,
                employee.PhoneNumber,
                new(employee.Email),
                GetValueById<TimeZones>(employee.TimeZone),
                new System.Collections.Concurrent.ConcurrentDictionary<int, HeadcountAllocation.Domain.Language>(foreignLanguages),
                new System.Collections.Concurrent.ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill>(skills),
                employee.YearsExperience,
                employee.JobPercentage,                
                employee.Password,
                employee.IsManager
            );
        }
       
    }    

    public class EmployeeOption : Employee
    {
        public double Score { get; set; }
    }
}