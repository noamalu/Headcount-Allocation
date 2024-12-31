

using System.Collections.Concurrent;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class Employee
    {
        public string? Name {get;set;}

        public int EmployeeId{get;set;}

        public string PhoneNumber{get;set;}

        public string? EmailAddress{get;set;}

        public int TimeZone{get;set;}
        
        public ConcurrentDictionary<int, Language> ForeignLanguages{get;set;} = new();

        public ConcurrentDictionary<int, Skill> Skills{get;set;} = new();

        public List<Role> Roles{get;set;} = new();

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}

    }
}