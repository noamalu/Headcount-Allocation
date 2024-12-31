namespace HeadcountAllocation.Domain{

    public class ManagerFacade{

        public Dictionary<int, Project> Projects{get;set;} = new();

        public Dictionary<int, Employee> Employees{get;set;} = new();

        public int projectCount = 0;

        public int employeeCount = 0;

        public void CreateProject(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles){
            Project project = new Project(projectName, projectCount++, description, date, requiredHours, roles);
            Projects.Add(projectCount, project);
            // add project to db
        }

        public void DeleteProject(int projectId){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            Projects.Remove(projectCount);
            // remove project from db
        }


        public void AddRoleToProject(int projectId, Role role){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].AddRoleToProject(role);
        }

        public Dictionary<int, Role> GetAllRolesByProject(int projectId){
            if (!Projects.ContainsKey(projectId)){
                throw new Exception($"No such project {projectId}");
            }
            return Projects[projectId].GetAllRolesByProject();
        } 

        public void AssignEmployeeToRole(int employeeId, Role role){
            if (!Employees.ContainsKey(employeeId)){
                throw new Exception($"No such employee {employeeId}");
            }
            bool RoleExists = false;
            foreach (Project project in Projects.Values){
                if (project.Roles.ContainsKey(role.RoleId)){
                    RoleExists = true;
                }
            }
            if (!RoleExists){
                throw new Exception($"No such role {role.RoleId}");
            }
            Employees[employeeId].AssignEmployeeToRole(role);
        }


    }
}