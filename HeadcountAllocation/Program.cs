using System;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;
using System.Collections.Concurrent;
using HeadcountAllocation.Services;

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

// DBcontext.GetInstance().Database.EnsureDeleted();
DBcontext.GetInstance().Dispose();
var context = DBcontext.GetInstance();

TimeZonesDTO morning = new TimeZonesDTO(TimeZones.Morning);
TimeZonesDTO noon = new TimeZonesDTO(TimeZones.Noon);
TimeZonesDTO evening = new TimeZonesDTO(TimeZones.Evening);
TimeZonesDTO flexible = new TimeZonesDTO(TimeZones.Flexible);
context.TimeZones.Add(morning);
context.TimeZones.Add(noon);
context.TimeZones.Add(evening);
context.TimeZones.Add(flexible);

LanguageTypesDTO english = new LanguageTypesDTO(Languages.English);
LanguageTypesDTO hebrew = new LanguageTypesDTO(Languages.Hebrew);
context.LanguageTypes.Add(english);
context.LanguageTypes.Add(hebrew);

SkillTypesDTO python = new SkillTypesDTO(Skills.Python);
SkillTypesDTO sql = new SkillTypesDTO(Skills.SQL);
SkillTypesDTO api = new SkillTypesDTO(Skills.API);
SkillTypesDTO java = new SkillTypesDTO(Skills.Java);
SkillTypesDTO ui = new SkillTypesDTO(Skills.UI);
context.SkillTypes.Add(python);
context.SkillTypes.Add(sql);
context.SkillTypes.Add(api);
context.SkillTypes.Add(java);
context.SkillTypes.Add(ui);

// perfect employee
List<EmployeeLanguagesDTO> emp_languages = new List<EmployeeLanguagesDTO>();
emp_languages.Add(new EmployeeLanguagesDTO(0, 0, 1));
emp_languages.Add(new EmployeeLanguagesDTO(1, 0, 3));
List<EmployeeSkillsDTO> emp_skills = new List<EmployeeSkillsDTO>();
emp_skills.Add(new EmployeeSkillsDTO(0, 2, 0));
emp_skills.Add(new EmployeeSkillsDTO(2, 2, 0));
emp_skills.Add(new EmployeeSkillsDTO(3, 2, 0));
EmployeeDTO employee0 = new EmployeeDTO(0, "employee1", "123", "mail", 0, emp_languages, 1, emp_skills, new List<RoleDTO>(), 10);
context.Employees.Add(employee0);


// all over employee
List<EmployeeLanguagesDTO> emp_languages1 = new List<EmployeeLanguagesDTO>();
emp_languages1.Add(new EmployeeLanguagesDTO(0, 1, 1));
emp_languages1.Add(new EmployeeLanguagesDTO(1, 1, 3));
List<EmployeeSkillsDTO> emp_skills1 = new List<EmployeeSkillsDTO>();
emp_skills1.Add(new EmployeeSkillsDTO(0, 3, 0));
emp_skills1.Add(new EmployeeSkillsDTO(2, 3, 0));
emp_skills1.Add(new EmployeeSkillsDTO(3, 3, 0));
EmployeeDTO employee1 = new EmployeeDTO(1, "employee2", "123", "mail", 0, emp_languages1, 1, emp_skills1, new List<RoleDTO>(), 10);
context.Employees.Add(employee1);

// context.Projects.Add(new ProjectDTO
// {
//     ProjectId = 1,
//     ProjectName = "project1",
//     Description = "desc1",
//     Date = DateTime.Now,
//     RequiredHours = 10,
//     Roles = new List<RoleDTO>
//     {
//         new RoleDTO
//         {
//             RoleId = 1,
//             RoleName = "Role1",
//             ProjectId = 1,
//             EmployeeId = null,
//             TimeZoneId = 1,
//             ForeignLanguages = new List<RoleLanguagesDTO>
//             {
//                 new RoleLanguagesDTO { LanguageID = 1, LanguageTypeId = 1, Level = 10}
//             },
//             JobPercentage = 0.5,
//             Skills = new List<RoleSkillsDTO>
//             {
//                 new RoleSkillsDTO { SkillId = 1, SkillTypeId = 1, Level = 5 },
//                 new RoleSkillsDTO { SkillId = 2, SkillTypeId = 2, Level = 7 }
//             },

//             YearsExperience = 2
//         },
//         new RoleDTO
//         {
//             RoleId = 2,
//             RoleName = "Role1",
//             ProjectId = 1,
//             EmployeeId = 1,
//             TimeZoneId = 1,
//             ForeignLanguages = new List<RoleLanguagesDTO>
//             {
//                 new RoleLanguagesDTO { LanguageID = 2, LanguageTypeId = 1, Level = 10}
//             },
//             JobPercentage = 0.5,
//             Skills = new List<RoleSkillsDTO>
//             {
//                 new RoleSkillsDTO { SkillId = 3, SkillTypeId = 1, Level = 5 },
//                 new RoleSkillsDTO { SkillId = 4, SkillTypeId = 2, Level = 7 }
//             },

//             YearsExperience = 2
//         }
//     }
// });


// Console.WriteLine("added help data");
// ConcurrentDictionary<int, Language> languages = new ConcurrentDictionary<int, Language>();
// languages.TryAdd(1, new Language(0, Languages.English, 5));
// ConcurrentDictionary<int, Skill> skills = new ConcurrentDictionary<int, Skill>();
// skills.TryAdd(1, new Skill(0, Skills.Python, 10));

// ManagerFacade managerFacade = ManagerFacade.GetInstance();
// managerFacade.CreateProject("testProject", "desc", DateTime.Now, 12, new Dictionary<int, Role>());

// managerFacade.AddRoleToProject("testRole1", 1, TimeZones.Morning, languages, skills, 5, 1);

// ConcurrentDictionary<int, Language> languages2 = new ConcurrentDictionary<int, Language>();
// languages.TryAdd(1, new Language(1, Languages.English, 5));
// ConcurrentDictionary<int, Skill> skills2 = new ConcurrentDictionary<int, Skill>();
// skills.TryAdd(1, new Skill(1, Skills.Python, 10));

// HeadCountService headCountService = HeadCountService.GetInstance();
// headCountService.AddRoleToProject("test2", 1, TimeZones.Morning, languages2, skills2, 2, 0.5);
// Console.WriteLine("added service");


// Dictionary <Employee, double> employees = headCountService.EmployeesToAssign(role).Value;
// Console.WriteLine("here0");
// foreach (var entry in employees)
// {
//     var employee = entry.Key; // The Employee object
//     var score = entry.Value;  // The double value associated with the employee
//     Console.WriteLine("here1");
//     Console.WriteLine($"{employee.EmployeeId}, {employee.Name}, {score}");
// }








