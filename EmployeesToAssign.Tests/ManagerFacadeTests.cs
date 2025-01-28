using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using HeadcountAllocation.Domain;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;

namespace EmployeesToAssign.Tests
{
    public class ManagerFacadeTests
    {
        [Fact]
        public void EmployeesToAssign_ShouldSuggestCorrectEmployeeForUIRole()
        {
            // Arrange
            var managerFacade = ManagerFacade.GetInstance();

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
                1, // Minimum 1 year experience
                100, // Full-time
                "UI development"
            );

            var matchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 1,
                UserName = "Jane Doe",
                YearExp = 2,
                Skills = new List<EmployeeSkillsDTO>(),
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 },
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.Hebrew), Level = 3 }
                },
                Roles = new()
            });

            var nonMatchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 2,
                UserName = "John Smith",
                YearExp = 0,
                Skills = new List<EmployeeSkillsDTO>(),
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 2 }
                },
                Roles = new()
            });

            managerFacade.Employees.Add(matchingEmployee.EmployeeId, matchingEmployee);
            managerFacade.Employees.Add(nonMatchingEmployee.EmployeeId, nonMatchingEmployee);

            // Act
            var result = managerFacade.EmployeesToAssign(uiRole);

            // Assert
            Assert.Single(result); // Only one employee qualifies
            Assert.Equal(matchingEmployee, result.Keys.First());
        }

        [Fact]
        public void EmployeesToAssign_ShouldSuggestCorrectEmployeeForAPIRole()
        {
            // Arrange
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
                    [Enums.GetId(Skills.API)] = new Skill(Skills.API, 3, 10),
                    [Enums.GetId(Skills.Java)] = new Skill(Skills.Java, 1, 5),
                    [Enums.GetId(Skills.SQL)] = new Skill(Skills.SQL, 2, 8)
                },
                0, // No minimum experience
                50, // Part-time
                "Develop APIs"
            );

            var matchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 3,
                UserName = "Alice Johnson",
                YearExp = 5,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.API), Level = 3, Priority = 10 },
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Java), Level = 1, Priority = 5 },
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.SQL), Level = 2, Priority = 8 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                },
                Roles = new()
            });

            var nonMatchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 4,
                UserName = "Bob Brown",
                YearExp = 1,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.API), Level = 2, Priority = 10 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 2 }
                },
                Roles = new()
            });
            managerFacade.Employees = new()
            {
                { matchingEmployee.EmployeeId, matchingEmployee },
                { nonMatchingEmployee.EmployeeId, nonMatchingEmployee }
            };

            // Act
            var result = managerFacade.EmployeesToAssign(apiRole);

            // Assert
            Assert.Single(result); // Only one employee qualifies
            Assert.Equal(matchingEmployee, result.Keys.First());
        }

        [Fact]
        public void EmployeesToAssign_ShouldReturnNoEmployeeForPythonRole()
        {
            // Arrange
            var managerFacade = ManagerFacade.GetInstance();

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
                    [Enums.GetId(Skills.Python)] = new Skill(Skills.Python, 3, 10)
                },
                3, // Minimum 3 years experience
                100, // Full-time
                "Develop Python applications"
            );

            var nonMatchingEmployee = new Employee(new EmployeeDTO
            {
                EmployeeId = 5,
                UserName = "Chris Green",
                YearExp = 1,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Python), Level = 2, Priority = 10 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>
                {
                    new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                },
                Roles = new()
            });

            managerFacade.Employees.Add(nonMatchingEmployee.EmployeeId, nonMatchingEmployee);

            // Act
            var result = managerFacade.EmployeesToAssign(pythonRole);

            // Assert
            Assert.Empty(result); // No employee qualifies
        }

        [Fact]
        public void EmployeesToAssign_ShouldReturnEmployeesSortedByScore()
        {
            // Arrange
            var managerFacade = ManagerFacade.GetInstance();

            var role = new Role(
                "General Role",
                4,
                1,
                TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>(),
                new ConcurrentDictionary<int, Skill>
                {
                    [Enums.GetId(Skills.Java)] = new Skill(Skills.Java, 3, 10)
                },
                0, // No minimum experience
                100, // Full-time
                "General tasks"
            );

            var employee1 = new Employee(new EmployeeDTO
            {
                EmployeeId = 6,
                UserName = "Best Canidadate",
                YearExp = 5,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Java), Level = 3, Priority = 10 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>(),
                Roles = new()
            });

            var employee2 = new Employee(new EmployeeDTO
            {
                EmployeeId = 7,
                UserName = "Should be second",
                YearExp = 5,
                Skills = new List<EmployeeSkillsDTO>
                {
                    new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Java), Level = 1, Priority = 10 }
                },
                ForeignLanguages = new List<EmployeeLanguagesDTO>(),
                Roles = new()
            });

            managerFacade.Employees = new()
            {
                { employee1.EmployeeId, employee1 },
                { employee2.EmployeeId, employee2 }
            };

            // Act
            var result = managerFacade.EmployeesToAssign(role);

            // Assert
            Assert.Equal(2, result.Count); // Two employees qualify
            Assert.Equal(employee1.EmployeeId, result.Keys.First().EmployeeId); // Employee1 has the highest score
        }

        

        /// <summary>
        /// The scenario involves a project named "Yield", 
        /// which has three roles: UI Developer, API Developer, and Python Developer, 
        /// each with specific skill, language, and experience requirements. 
        /// The UI Developer role requires at least one year of experience, 
        /// full-time commitment, and fluency in both English and Hebrew at level 3. 
        /// The API Developer role demands proficiency in API (level 3), Java (level 1), 
        /// SQL (level 2), English (level 3), and a part-time commitment. 
        /// The Python Developer role requires Python skills at level 3, 
        /// English at level 3, and a minimum of three years of experience, 
        /// with a full-time commitment. 
        /// The project includes a diverse set of employees with varying levels of skills, 
        /// languages, and experience to match or fail the role requirements.
        /// </summary>
        [Fact]
        public void EmployeesToAssign_ShouldRankCandidatesForAllRolesCorrectly()
        {
            // Arrange
            var managerFacade = ManagerFacade.GetInstance();

            // Define the roles
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
                1, // Minimum 1 year experience
                100, // Full-time
                "UI development"
            );

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
                    [Enums.GetId(Skills.API)] = new Skill(Skills.API, 3, 10),
                    [Enums.GetId(Skills.Java)] = new Skill(Skills.Java, 1, 5),
                    [Enums.GetId(Skills.SQL)] = new Skill(Skills.SQL, 2, 8)
                },
                0, // No minimum experience
                50, // Part-time
                "Develop APIs"
            );

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
                    [Enums.GetId(Skills.Python)] = new Skill(Skills.Python, 3, 10)
                },
                3, // Minimum 3 years experience
                100, // Full-time
                "Develop Python applications"
            );

            // Define employees
            var employees = new List<Employee>
            {
                // UI Role candidates
                new Employee(new EmployeeDTO
                {
                    EmployeeId = 1,
                    UserName = "Alice",
                    YearExp = 2,
                    Skills = new List<EmployeeSkillsDTO>(),
                    ForeignLanguages = new List<EmployeeLanguagesDTO>
                    {
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 },
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.Hebrew), Level = 3 }
                    },
                    Roles = new()
                }),
                new Employee(new EmployeeDTO
                {
                    EmployeeId = 2,
                    UserName = "Bob",
                    YearExp = 1,
                    Skills = new List<EmployeeSkillsDTO>(),
                    ForeignLanguages = new List<EmployeeLanguagesDTO>
                    {
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 },
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.Hebrew), Level = 3 }
                    },
                    Roles = new()
                }),
                new Employee(new EmployeeDTO
                {
                    EmployeeId = 3,
                    UserName = "Charlie",
                    YearExp = 0, // Does not qualify for UI Role
                    Skills = new List<EmployeeSkillsDTO>(),
                    ForeignLanguages = new List<EmployeeLanguagesDTO>
                    {
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                    },
                    Roles = new()
                }),

                // Python Role candidates
                new Employee(new EmployeeDTO
                {
                    EmployeeId = 4,
                    UserName = "David",
                    YearExp = 3,
                    Skills = new List<EmployeeSkillsDTO>
                    {
                        new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Python), Level = 3, Priority = 10 }
                    },
                    ForeignLanguages = new List<EmployeeLanguagesDTO>
                    {
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                    },
                    Roles = new()
                }),
                new Employee(new EmployeeDTO
                {
                    EmployeeId = 5,
                    UserName = "Eve",
                    YearExp = 4,
                    Skills = new List<EmployeeSkillsDTO>
                    {
                        new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Python), Level = 3, Priority = 10 }
                    },
                    ForeignLanguages = new List<EmployeeLanguagesDTO>
                    {
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                    },
                    Roles = new()
                }),
                new Employee(new EmployeeDTO
                {
                    EmployeeId = 6,
                    UserName = "Frank",
                    YearExp = 2, // Does not qualify for Python Role
                    Skills = new List<EmployeeSkillsDTO>
                    {
                        new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Python), Level = 2, Priority = 10 }
                    },
                    ForeignLanguages = new List<EmployeeLanguagesDTO>
                    {
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                    },
                    Roles = new()
                }),

                // API Role candidate
                new Employee(new EmployeeDTO
                {
                    EmployeeId = 7,
                    UserName = "Grace",
                    YearExp = 5,
                    Skills = new List<EmployeeSkillsDTO>
                    {
                        new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.API), Level = 3, Priority = 10 },
                        new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.Java), Level = 1, Priority = 5 },
                        new EmployeeSkillsDTO { SkillTypeId = Enums.GetId(Skills.SQL), Level = 2, Priority = 8 }
                    },
                    ForeignLanguages = new List<EmployeeLanguagesDTO>
                    {
                        new EmployeeLanguagesDTO { LanguageTypeId = Enums.GetId(Languages.English), Level = 3 }
                    },
                    Roles = new()
                })
            };

            managerFacade.Employees = new();
            foreach (var employee in employees)
            {
                managerFacade.Employees.Add(employee.EmployeeId, employee);
            }

            // Act
            var uiResult = managerFacade.EmployeesToAssign(uiRole);
            var pythonResult = managerFacade.EmployeesToAssign(pythonRole);
            var apiResult = managerFacade.EmployeesToAssign(apiRole);

            // Assert
            // UI Role
            Assert.Equal(2, uiResult.Count); // Two employees qualify for the UI Role
            Assert.Equal(1, uiResult.Keys.First().EmployeeId); // Alice should rank first for UI Role

            // Python Role
            Assert.Equal(3, pythonResult.Count); // Three employees qualify for the Python Role
            Assert.Equal(7, pythonResult.Keys.Last().EmployeeId); // Grace should rank last for Python Role
            Assert.True(pythonResult.Values.Distinct().Count() == pythonResult.Values.Count - 1); // Eve and David should score the same for Python Role

            // API Role
            Assert.Equal(7, apiResult.Keys.First().EmployeeId); // Grace should rank first for API Role
        }

    }
}
