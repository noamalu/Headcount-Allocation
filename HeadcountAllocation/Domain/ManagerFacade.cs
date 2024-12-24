namespace HeadcountAllocation.Domain{

    public class ManagerFacade{

        public Dictionary<int, Project> Projects{get;set;} = new();

        public Dictionary<int, Employee> Employees{get;set;} = new();
    }
}