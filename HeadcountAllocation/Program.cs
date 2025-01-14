using System;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// try
//         {
//             // Get DB context instance
//             var context = DBcontext.GetInstance();

//             // Test database connection
//             if (context.Database.CanConnect())
//             {
//                 Console.WriteLine("Database connection is successful!");

//                 // Check existing records
//                 var employeeCount = context.Employees.Count();
//                 Console.WriteLine($"There are {employeeCount} employees in the database.");

//                 var projectCount = context.Projects.Count();
//                 Console.WriteLine($"There are {projectCount} projects in the database.");
//             }
//             else
//             {
//                 Console.WriteLine("Failed to connect to the database.");
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"An error occurred: {ex.Message}");
//         }

DBcontext.GetInstance().Dispose();
var context = DBcontext.GetInstance();

//Add test data
context.Employees.Add(new EmployeeDTO
{
    EmployeeId = 1,
    UserName = "John Doe",
    PhoneNumber = "0545598789",
    Email = "hp1@gmail.com",
    TimeZone = 1,
    ForeignLanguages = new List<EmployeeLanguagesDTO>
    {
        new EmployeeLanguagesDTO { LanguageID = 1, LanguageTypeId = 1, Level = 10}
    },
    JobPercentage = 0.5,
    Skills = new List<EmployeeSkillsDTO>
    {
        new EmployeeSkillsDTO { SkillId = 1, SkillTypeId = 1, Level = 5 },
        new EmployeeSkillsDTO { SkillId = 2, SkillTypeId = 2, Level = 7 }
    },
    YearExp = 2
});

context.SaveChanges();
Console.WriteLine("Test data added successfully!");


// context.SaveChanges();
// Console.WriteLine("Test data added successfully!");

context.Projects.Add(new ProjectDTO
{
    ProjectId = 1,
    ProjectName = "project1",
    Description = "desc1",
    Date = DateTime.Now,
    RequiredHours = 10,
    Roles = new List<RoleDTO>
    {
        new RoleDTO
        {
            RoleId = 1,
            RoleName = "Role1",
            ProjectId = 1,
            EmployeeId = null,
            TimeZoneId = 1,
            ForeignLanguages = new List<RoleLanguagesDTO>
            {
                new RoleLanguagesDTO { LanguageID = 1, LanguageTypeId = 1, Level = 10}
            },
            JobPercentage = 0.5,
            Skills = new List<RoleSkillsDTO>
            {
                new RoleSkillsDTO { SkillId = 1, SkillTypeId = 1, Level = 5 },
                new RoleSkillsDTO { SkillId = 2, SkillTypeId = 2, Level = 7 }
            },
   
            YearsExperience = 2
        },
        new RoleDTO
        {
            RoleId = 2,
            RoleName = "Role1",
            ProjectId = 1,
            EmployeeId = 1,
            TimeZoneId = 1,
            ForeignLanguages = new List<RoleLanguagesDTO>
            {
                new RoleLanguagesDTO { LanguageID = 2, LanguageTypeId = 1, Level = 10}
            },
            JobPercentage = 0.5,
            Skills = new List<RoleSkillsDTO>
            {
                new RoleSkillsDTO { SkillId = 3, SkillTypeId = 1, Level = 5 },
                new RoleSkillsDTO { SkillId = 4, SkillTypeId = 2, Level = 7 }
            },
   
            YearsExperience = 2
        }
    }
});

context.SaveChanges();
Console.WriteLine("Test data added successfully!");

ManagerFacade managerFacade = new ManagerFacade();
managerFacade.CreateProject("testProject", "desc", DateTime.Now, 12, new Dictionary<int, Role>());



