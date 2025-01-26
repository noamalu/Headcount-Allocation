using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using HeadcountAllocation.Services;

namespace API.Services
{
    public class EmployeeService
    {
        private readonly HeadCountService _headCountService;

        public EmployeeService(HeadCountService headcountService)
        {
            _headCountService = headcountService;
        }
        public Employee TranslateEmployee(HeadcountAllocation.Domain.Employee employee)
        {
            var translatedEmployee = new Employee
            {
                EmployeeName = employee.Name,
                EmployeeId = employee.EmployeeId,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.EmailAddress,
                TimeZone = (int)employee.TimeZone,
                YearsExperience = employee.YearsExperience,
                JobPercentage = employee.JobPercentage,
                ForeignLanguages = employee.ForeignLanguages.Values?.Select(language => new Language
                {
                    LanguageId = language.LanguageID,
                    LanguageTypeId = HeadcountAllocation.Domain.Enums.GetId(language.LanguageType),
                    Level = language.Level
                }).ToList() ?? new(),
                Skills = employee.Skills.Values?.Select(skill => new Skill
                {
                    SkillTypeId = HeadcountAllocation.Domain.Enums.GetId(skill.SkillType),
                    Level = skill.Level
                }).ToList() ?? new()
            };

            return translatedEmployee;                        
        }
    }
}