namespace HeadcountAllocation.Domain{

    public class ManagerFacade{

        public Dictionary<int, Project> projects{get;set;} = new();

        public Dictionary<int, User> Users{get;set;} = new();
    }
}