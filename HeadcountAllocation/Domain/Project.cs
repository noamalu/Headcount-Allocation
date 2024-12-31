namespace HeadcountAllocation.Domain{

    public class Project{

        public string? ProjectName{get;set;}

        public int ProjectId{get;set;}

        public string? Description{get;set;}

        public DateTime Date{get;set;}

        public int RequiredHours{get;set;}

        public Dictionary<int, Role> Roles{get;set;} = new();

        public Project(string projectName, int projectId, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles){
            ProjectName = projectName;
            ProjectId = projectId;
            Description = description;
            Date = date;
            RequiredHours = requiredHours;
            Roles = roles;
        }

        public void AddRoleToProject(Role role){
            if (Roles.ContainsKey(role.RoleId)){
                throw new Exception($"Role exists {role.RoleId}");
            }
            Roles.Add(role.RoleId, role);
        }

        public Dictionary<int, Role> GetAllRolesByProject(){
            return Roles;
        } 


    }


}