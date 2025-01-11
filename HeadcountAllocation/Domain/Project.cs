using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.DAL.Repositories;

namespace HeadcountAllocation.Domain{

    public class Project{

        public string? ProjectName{get;set;}

        public int ProjectId{get;set;}

        public string? Description{get;set;}

        public DateTime Date{get;set;}

        public int RequiredHours{get;set;}

        public Dictionary<int, Role> Roles{get;set;} = new();

        public RoleRepo RoleRepo;

        public Project(string projectName, int projectId, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles){
            ProjectName = projectName;
            ProjectId = projectId;
            Description = description;
            Date = date;
            RequiredHours = requiredHours;
            Roles = roles;
            RoleRepo = RoleRepo.GetInstance();
        }

        public Project(ProjectDTO projectDTO)
        {
           ProjectName = projectDTO.ProjectName;
            ProjectId = projectDTO.ProjectId;
            Description = projectDTO.Description;
            Date = projectDTO.Date;
            RequiredHours = projectDTO.RequiredHours;
            foreach (RoleDTO roleDTO in projectDTO.Roles){
                Roles[roleDTO.RoleId] = new Role(roleDTO);
            }
            RoleRepo = RoleRepo.GetInstance();
        }

        public void AddRoleToProject(Role role){
            if (Roles.ContainsKey(role.RoleId)){
                throw new Exception($"Role exists {role.RoleId}");
            }
            Roles.Add(role.RoleId, role);
            try{
                RoleRepo.Add(role);
            }
            catch (Exception e){
                throw new Exception(e.Message);
            }
            
        }

        public Dictionary<int, Role> GetAllRolesByProject(){
            return Roles;
        } 


    }


}