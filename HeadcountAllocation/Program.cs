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
context.SaveChanges();

LanguageTypesDTO english = new LanguageTypesDTO(Languages.English);
LanguageTypesDTO hebrew = new LanguageTypesDTO(Languages.Hebrew);
context.LanguageTypes.Add(english);
context.LanguageTypes.Add(hebrew);
context.SaveChanges();

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
context.SaveChanges();

// perfect employee
List<EmployeeLanguagesDTO> emp_languages = new List<EmployeeLanguagesDTO>();
emp_languages.Add(new EmployeeLanguagesDTO(0, 0, 1));
emp_languages.Add(new EmployeeLanguagesDTO(1, 0, 3));
List<EmployeeSkillsDTO> emp_skills = new List<EmployeeSkillsDTO>();
emp_skills.Add(new EmployeeSkillsDTO(0, 2, 0));
emp_skills.Add(new EmployeeSkillsDTO(2, 2, 0));
emp_skills.Add(new EmployeeSkillsDTO(3, 2, 0));
EmployeeDTO employee0 = new EmployeeDTO(0, "employee1", "123", "mail@gmail.com", 0, emp_languages, 1, emp_skills, new List<RoleDTO>(), 10, "pass", false);
context.Employees.Add(employee0);
context.SaveChanges();


// all over employee
List<EmployeeLanguagesDTO> emp_languages1 = new List<EmployeeLanguagesDTO>();
emp_languages1.Add(new EmployeeLanguagesDTO(0, 1, 1));
emp_languages1.Add(new EmployeeLanguagesDTO(1, 1, 3));
List<EmployeeSkillsDTO> emp_skills1 = new List<EmployeeSkillsDTO>();
emp_skills1.Add(new EmployeeSkillsDTO(0, 3, 0));
emp_skills1.Add(new EmployeeSkillsDTO(2, 3, 0));
emp_skills1.Add(new EmployeeSkillsDTO(3, 3, 0));
EmployeeDTO employee1 = new EmployeeDTO(1, "employee2", "123", "mail@gmail.com", 0, emp_languages1, 1, emp_skills1, new List<RoleDTO>(), 10, "pass", false);
context.Employees.Add(employee1);
context.SaveChanges();

// all below employee
List<EmployeeLanguagesDTO> emp_languages2 = new List<EmployeeLanguagesDTO>();
emp_languages2.Add(new EmployeeLanguagesDTO(0, 2, 1));
emp_languages2.Add(new EmployeeLanguagesDTO(1, 2, 3));
List<EmployeeSkillsDTO> emp_skills2 = new List<EmployeeSkillsDTO>();
emp_skills2.Add(new EmployeeSkillsDTO(0, 1, 0));
emp_skills2.Add(new EmployeeSkillsDTO(2, 1, 0));
emp_skills2.Add(new EmployeeSkillsDTO(3, 1, 0));
EmployeeDTO employee2 = new EmployeeDTO(2, "employee3", "123", "mail@gmail.com", 0, emp_languages2, 1, emp_skills2, new List<RoleDTO>(), 10, "pass", false);
context.Employees.Add(employee2);
context.SaveChanges();

// no languages employee
List<EmployeeLanguagesDTO> emp_languages3 = new List<EmployeeLanguagesDTO>();
emp_languages3.Add(new EmployeeLanguagesDTO(0, 3, 1));
emp_languages3.Add(new EmployeeLanguagesDTO(1, 3, 1));
List<EmployeeSkillsDTO> emp_skills3 = new List<EmployeeSkillsDTO>();
emp_skills3.Add(new EmployeeSkillsDTO(0, 2, 0));
emp_skills3.Add(new EmployeeSkillsDTO(2, 2, 0));
emp_skills3.Add(new EmployeeSkillsDTO(3, 2, 0));
EmployeeDTO employee3 = new EmployeeDTO(3, "employee4", "123", "mail@gmail.com", 0, emp_languages3, 1, emp_skills3, new List<RoleDTO>(), 10, "pass", false);
context.Employees.Add(employee3);
context.SaveChanges();

// all below employee
List<EmployeeLanguagesDTO> emp_languages4 = new List<EmployeeLanguagesDTO>();
emp_languages4.Add(new EmployeeLanguagesDTO(0, 4, 1));
emp_languages4.Add(new EmployeeLanguagesDTO(1, 4, 3));
List<EmployeeSkillsDTO> emp_skills4 = new List<EmployeeSkillsDTO>();
emp_skills4.Add(new EmployeeSkillsDTO(0, 3, 0));
emp_skills4.Add(new EmployeeSkillsDTO(2, 3, 0));
EmployeeDTO employee4 = new EmployeeDTO(4, "employee5", "123", "mail@gmail.com", 0, emp_languages4, 1, emp_skills4, new List<RoleDTO>(), 10, "pass", false);
context.Employees.Add(employee4);
context.SaveChanges();

ManagerFacade managerFacade = ManagerFacade.GetInstance();
Employee emp0 = new Employee(employee0);
Employee emp1 = new Employee(employee1);
Employee emp2 = new Employee(employee2);
Employee emp3 = new Employee(employee3);
Employee emp4 = new Employee(employee4);
managerFacade.Employees.TryAdd(emp0.EmployeeId, emp0);
managerFacade.Employees.TryAdd(emp1.EmployeeId, emp1);
managerFacade.Employees.TryAdd(emp2.EmployeeId, emp2);
managerFacade.Employees.TryAdd(emp3.EmployeeId, emp3);
managerFacade.Employees.TryAdd(emp4.EmployeeId, emp4);


Console.WriteLine("added help data");
ConcurrentDictionary<int, Language> languages = new ConcurrentDictionary<int, Language>();
languages.TryAdd(Enums.GetId(Languages.English), new Language(Languages.English, 1));
languages.TryAdd(Enums.GetId(Languages.Hebrew), new Language(Languages.Hebrew, 3));
ConcurrentDictionary<int, Skill> skills = new ConcurrentDictionary<int, Skill>();
skills.TryAdd(Enums.GetId(Skills.Python), new Skill(Skills.Python, 2, 1));
skills.TryAdd(Enums.GetId(Skills.API), new Skill(Skills.API, 2, 1));
skills.TryAdd(Enums.GetId(Skills.Java), new Skill(Skills.Java, 2, 1));

HeadCountService headCountService = HeadCountService.GetInstance();
headCountService.CreateProject("testProject", "desc", DateTime.Now, 12, new Dictionary<int, Role>());

Role role = headCountService.AddRoleToProject("testRole1", 0, TimeZones.Morning, languages, skills, 5, 0.5, "role").Value;

headCountService.AddRoleToProject("test2", 0, TimeZones.Flexible, languages, skills, 2, 0.5, "role");
Console.WriteLine("added service");


Dictionary<Employee, double> employees = headCountService.EmployeesToAssign(role).Value;
Console.WriteLine("here0");
foreach (var entry in employees)
{
    var employee = entry.Key; // The Employee object
    var score = entry.Value;  // The double value associated with the employee
    Console.WriteLine("here1");
    Console.WriteLine($"{employee.EmployeeId}, {employee.UserName}, {score}");
}








