using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeadcountAllocation.Domain;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
namespace IT.Tests
{
    [TestClass]
    public class AlgoritmTests
    {
        [TestInitialize]
        public void Setup()
        {
            DBcontext.SetTestDatabase(); // Connect to test DB
            var context = DBcontext.GetInstance();
            context.ClearDatabase();     // Clean database before each test
            context.SeedStaticTables();
        }

        [TestMethod]
        public void EmployeesToAssign_ShouldSuggestCorrectEmployeeForUIRole()
        {
            var managerFacade = ManagerFacade.GetInstance();
            managerFacade.Employees.Clear(); 

            var uiRole = new Role(
                "UI Developer",
                1,
                1,
                TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>
                {
                    [Enums.GetId(Languages.English)] = new Language(Languages.English, 3),
                    [Enums.GetId(Languages.Hebrew)] = new Language(Languages.Hebrew, 3)
                },
                new ConcurrentDictionary<int, Skill>(),
                1,
                1,
                "UI development",
                DateTime.Now
            );

            var matchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 1,
                UserName = "Jane Doe",
                Email = "jane.doe@example.com", 
                YearExp = 2,
                Skills = new List<EmployeeSkillsDTO>(),
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 },
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.Hebrew), Level = 3 }
                },
                Roles = new(),
                JobPercentage = 1
            });

            var nonMatchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 2,
                UserName = "John Smith",
                Email = "jane.doe@example.com",
                YearExp = 0,
                Skills = new List<EmployeeSkillsDTO>(),
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 2 }
                },
                Roles = new(),
                JobPercentage = 1
            });

            managerFacade.Employees.Add(matchingEmployee.EmployeeId, matchingEmployee);
            managerFacade.Employees.Add(nonMatchingEmployee.EmployeeId, nonMatchingEmployee);

            var result = managerFacade.EmployeesToAssign(uiRole);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(matchingEmployee, result.Keys.First());
        }

        [TestMethod]
        public void EmployeesToAssign_ShouldSuggestCorrectEmployeeForAPIRole()
        {
            var managerFacade = ManagerFacade.GetInstance();

            var apiRole = new Role(
                "API Developer",
                2,
                1,
                TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>
                {
                    [Enums.GetId(Languages.English)] = new Language(Languages.English, 3)
                },
                new ConcurrentDictionary<int, Skill>
                {
                    [Enums.GetId(Skills.API)] = new Skill(Skills.API, 3, 1),
                    [Enums.GetId(Skills.Java)] = new Skill(Skills.Java, 1, 3),
                    [Enums.GetId(Skills.SQL)] = new Skill(Skills.SQL, 2, 2)
                },
                0,
                0.5,
                "Develop APIs",
                DateTime.Now
            );

            var matchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 3,
                UserName = "Alice Johnson",
                Email = "jane.doe@example.com",
                YearExp = 5,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.API), Level = 3, Priority = 1 },
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Java), Level = 1, Priority = 3 },
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.SQL), Level = 2, Priority = 2 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                },
                Roles = new(),
                JobPercentage = 0.5
            });

            var nonMatchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 4,
                UserName = "Bob Brown",
                Email = "jane.doe@example.com",
                YearExp = 1,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.API), Level = 2, Priority = 1 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 2 }
                },
                Roles = new(),
                JobPercentage = 0.5
            });

            managerFacade.Employees = new()
            {
                { matchingEmployee.EmployeeId, matchingEmployee },
                { nonMatchingEmployee.EmployeeId, nonMatchingEmployee }
            };

            var result = managerFacade.EmployeesToAssign(apiRole);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(matchingEmployee, result.Keys.First());
        }

        [TestMethod]
        public void EmployeesToAssign_ShouldReturnNoEmployeeForPythonRole()
        {
            var managerFacade = ManagerFacade.GetInstance();
            managerFacade.Employees.Clear();

            var pythonRole = new Role(
                "Python Developer",
                3,
                1,
                TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>
                {
                    [Enums.GetId(Languages.English)] = new Language(Languages.English, 3)
                },
                new ConcurrentDictionary<int, Skill>
                {
                    [Enums.GetId(Skills.Python)] = new Skill(Skills.Python, 3, 1)
                },
                3,
                1,
                "Develop Python applications",
                DateTime.Now
            );

            var nonMatchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 5,
                UserName = "Chris Green",
                Email = "jane.doe@example.com",
                YearExp = 1,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Python), Level = 2, Priority = 1 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                },
                Roles = new(),
                JobPercentage = 1
            });

            managerFacade.Employees.Add(nonMatchingEmployee.EmployeeId, nonMatchingEmployee);

            var result = managerFacade.EmployeesToAssign(pythonRole);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void EmployeesToAssign_ShouldReturnEmployeesSortedByScore()
        {
            var managerFacade = ManagerFacade.GetInstance();

            var role = new Role(
                "General Role",
                4,
                1,
                TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>(),
                new ConcurrentDictionary<int, Skill>
                {
                    [Enums.GetId(Skills.Java)] = new Skill(Skills.Java, 3, 1)
                },
                0,
                1,
                "General tasks",
                DateTime.Now
            );

            var employee1 = new Employee(new EmployeeDTO
            {
                EmployeeId = 6,
                UserName = "Best Candidate",
                Email = "jane.doe@example.com",
                YearExp = 5,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Java), Level = 3, Priority = 1 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>(),
                Roles = new(), 
                JobPercentage = 1
            });

            var employee2 = new Employee(new EmployeeDTO
            {
                EmployeeId = 7,
                UserName = "Second Best",
                Email = "jane.doe@example.com",
                YearExp = 5,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Java), Level = 1, Priority = 1 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>(),
                Roles = new(), 
                JobPercentage = 1
            });

            managerFacade.Employees = new()
            {
                { employee1.EmployeeId, employee1 },
                { employee2.EmployeeId, employee2 }
            };

            var result = managerFacade.EmployeesToAssign(role);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(employee1.EmployeeId, result.Keys.First().EmployeeId);
        }

        [TestMethod]
        public void EmployeesToAssign_ShouldRankCandidatesForAllRolesCorrectly()
        {
            var managerFacade = ManagerFacade.GetInstance();

            var uiRole = new Role(
                "UI Developer", 1, 1, TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>
                {
                    [Enums.GetId(Languages.English)] = new Language(Languages.English, 3),
                    [Enums.GetId(Languages.Hebrew)] = new Language(Languages.Hebrew, 3)
                },
                new ConcurrentDictionary<int, Skill>(), 1, 1, "UI development", DateTime.Now
            );

            var apiRole = new Role(
                "API Developer", 2, 1, TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>
                {
                    [Enums.GetId(Languages.English)] = new Language(Languages.English, 3)
                },
                new ConcurrentDictionary<int, Skill>
                {
                    [Enums.GetId(Skills.API)] = new Skill(Skills.API, 3, 1),
                    [Enums.GetId(Skills.Java)] = new Skill(Skills.Java, 1, 3),
                    [Enums.GetId(Skills.SQL)] = new Skill(Skills.SQL, 2, 2)
                },
                0, 0.5, "Develop APIs", DateTime.Now
            );

            var pythonRole = new Role(
                "Python Developer", 3, 1, TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>
                {
                    [Enums.GetId(Languages.English)] = new Language(Languages.English, 3)
                },
                new ConcurrentDictionary<int, Skill>
                {
                    [Enums.GetId(Skills.Python)] = new Skill(Skills.Python, 3, 1)
                },
                3, 1, "Develop Python applications", DateTime.Now
            );

            var employees = new List<Employee>
            {
                new Employee(new EmployeeDTO { EmployeeId = 1, UserName = "Alice", YearExp = 2, Email = "jane.doe@example.com", ForeignLanguages = new() { new EmployeeLanguagesDTO { LanguageTypeId = 0, Level = 3 }, new EmployeeLanguagesDTO { LanguageTypeId = 1, Level = 3 } }, Skills = new(), Roles = new(), JobPercentage = 1 }),
                new Employee(new EmployeeDTO { EmployeeId = 2, UserName = "Bob", YearExp = 1, Email = "jane.doe@example.com", ForeignLanguages = new() { new EmployeeLanguagesDTO { LanguageTypeId = 0, Level = 3 }, new EmployeeLanguagesDTO { LanguageTypeId = 1, Level = 3 } }, Skills = new(), Roles = new(), JobPercentage = 1 }),
                new Employee(new EmployeeDTO { EmployeeId = 3, UserName = "Charlie", YearExp = 0, Email = "jane.doe@example.com", ForeignLanguages = new() { new EmployeeLanguagesDTO { LanguageTypeId = 0, Level = 3 } }, Skills = new(), Roles = new() , JobPercentage = 1}),
                new Employee(new EmployeeDTO { EmployeeId = 4, UserName = "David", YearExp = 3, Email = "jane.doe@example.com", ForeignLanguages = new() { new EmployeeLanguagesDTO { LanguageTypeId = 0, Level = 3 } }, Skills = new() { new EmployeeSkillsDTO { SkillTypeId = 0, Level = 3, Priority = 1 } }, Roles = new(), JobPercentage = 1 }),
                new Employee(new EmployeeDTO { EmployeeId = 5, UserName = "Eve", YearExp = 4, Email = "jane.doe@example.com", ForeignLanguages = new() { new EmployeeLanguagesDTO { LanguageTypeId = 0, Level = 3 } }, Skills = new() { new EmployeeSkillsDTO { SkillTypeId = 0, Level = 3, Priority = 1 } }, Roles = new(), JobPercentage = 1 }),
                new Employee(new EmployeeDTO { EmployeeId = 6, UserName = "Frank", YearExp = 2, Email = "jane.doe@example.com", ForeignLanguages = new() { new EmployeeLanguagesDTO { LanguageTypeId = 0, Level = 3 } }, Skills = new() { new EmployeeSkillsDTO { SkillTypeId = 0, Level = 2, Priority = 1 } }, Roles = new(), JobPercentage = 1 }),
                new Employee(new EmployeeDTO { EmployeeId = 7, UserName = "Grace", YearExp = 5, Email = "jane.doe@example.com", ForeignLanguages = new() { new EmployeeLanguagesDTO { LanguageTypeId = 0, Level = 3 } }, Skills = new() { new EmployeeSkillsDTO { SkillTypeId = 2, Level = 3, Priority = 1 }, new EmployeeSkillsDTO { SkillTypeId = 3, Level = 1, Priority = 3 }, new EmployeeSkillsDTO { SkillTypeId = 1, Level = 2, Priority = 2 } }, Roles = new(), JobPercentage = 1 })
            };

            managerFacade.Employees = new();
            foreach (var emp in employees)
                managerFacade.Employees.Add(emp.EmployeeId, emp);

            var uiResult = managerFacade.EmployeesToAssign(uiRole);
            var pythonResult = managerFacade.EmployeesToAssign(pythonRole);
            var apiResult = managerFacade.EmployeesToAssign(apiRole);

            Assert.AreEqual(2, uiResult.Count);
            Assert.AreEqual(1, uiResult.Keys.First().EmployeeId);

            Assert.AreEqual(3, pythonResult.Count);
            Assert.AreEqual(4, pythonResult.Keys.First().EmployeeId);

            Assert.AreEqual(7, apiResult.Keys.First().EmployeeId);
        }
    }
}
