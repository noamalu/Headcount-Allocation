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
        public EmployeeOption TranslateEmployee(HeadcountAllocation.Domain.Employee employee)
        {
            var translatedEmployee = new EmployeeOption
            {
                EmployeeName = employee.UserName,
                EmployeeId = employee.EmployeeId,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email.Address,
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

        public bool IsAdmin(int employeeId)
        {
            var employee = _headCountService.GetEmployeeById(employeeId);

            return employee.Value.IsManager;            
        }

        public Response EditEmployee(HeadcountAllocation.Domain.Employee employee)
        {
            return _headCountService.UpdateEmployee(employee);
            
        }
    }
}