using System;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
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

var context = DBcontext.GetInstance();

//Add test data
context.Employees.Add(new EmployeeDTO
{
    EmployeeId = 1,
    UserName = "John Doe",
    PhoneNumber = "0545598789",
    Email = "hp1@gmail.com",
    TimeZone = 1,
    ForeignLanguages = new List<LanguagesDTO>
    {
        new LanguagesDTO { LanguageID = 1, LanguageTypeId = 1, Level = 10}
    },
    JobPercentage = 0.5,
    // Skills = new List<SkillDTO>
    // {
    //     new SkillDTO { SkillId = 1, SkillType = Skills.Programing, Level = 5 },
    //     new SkillDTO { SkillId = 2, SkillType = Skills.Programing, Level = 7 }
    // },
    // Roles = new List<RoleDTO>{},
    YearExp = 2
});


context.SaveChanges();
Console.WriteLine("Test data added successfully!");


