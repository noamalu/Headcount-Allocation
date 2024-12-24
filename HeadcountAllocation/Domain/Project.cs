namespace HeadcountAllocation.Domain{

    public class Project{

        public string? ProjectName{get;set;}

        public int ProjectId{get;set;}

        public string? Description{get;set;}

        public DateTime Date{get;set;}

        public int RequiredHours{get;set;}

        public Dictionary<int, Role> Roles{get;set;} = new();
    }

}