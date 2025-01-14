    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using HeadcountAllocation.Domain;

    namespace HeadcountAllocation.DAL.DTO
    {
        [Table("Projects")]
        public class ProjectDTO
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]

            public int ProjectId{get;set;}
            public string? ProjectName{get;set;}

            public string? Description{get;set;}

            public DateTime Date {get;set;}

            public int RequiredHours{get;set;}

            public List<RoleDTO> Roles{get;set;}

            public ProjectDTO() { }
            public ProjectDTO(int projectId, string projectName, string description, DateTime date,
            int requiredHours, List <RoleDTO> roles)
            {
                ProjectId = projectId;
                ProjectName = projectName;
                Description = description;
                Date = date;
                RequiredHours = requiredHours;
                Roles = roles;
            }

            public ProjectDTO(Project project)
            {
                ProjectId = project.ProjectId;
                ProjectName = project.ProjectName;
                Description = project.Description;
                Date = project.Date;
                RequiredHours = project.RequiredHours;
                Roles = new List<RoleDTO>();
                foreach (var role in project.Roles)
                {
                    Roles.Add(new RoleDTO(role.Value));
                }
            }
        }


        

                

        
    }