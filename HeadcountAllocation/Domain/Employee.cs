

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
            TimeZone = employeeDto.TimeZone;
            ////...
        }

        public string? Name {get;set;}

        public int EmployeeId{get;set;}

        public string PhoneNumber{get;set;}

        public string? EmailAddress{get;set;}

        public int TimeZone{get;set;}
        
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