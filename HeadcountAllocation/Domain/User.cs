

using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain{

    public class User
    {
        public string? UserName{get;set;}

        public int Id{get;set;}

        public string? PhoneNumber{get;set;}

        public string? EmailAddress{get;set;}

        public TimeZones TimeZone{get;set;}
        
        public List<string>? ForeignLanguages{get;set;} = new();

        public Dictionary<Skills, int> Skills{get;set;} = new();

        public List<Role> Roles{get;set;} = new();

        public int YearsExperience{get;set;}

        public double JobPercentage{get;set;}

        public string check;

    }
}