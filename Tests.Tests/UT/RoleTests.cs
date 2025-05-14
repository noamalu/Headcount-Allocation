using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeadcountAllocation.Domain;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
namespace UT.Tests
{
    [TestClass]
    public class RoleTests
    {
        DBcontext context;
        [TestInitialize]
        public void Setup()
        {
            DBcontext.SetTestDatabase(); // Connect to test DB
            context = DBcontext.GetInstance();
            context.ClearDatabase();
            context.SeedStaticTables();     // Clean database before each test
        }

        private ConcurrentDictionary<int, Language> CreateLanguages()
        {
            return new ConcurrentDictionary<int, Language>
            {
                [GetId(Languages.English)] = new Language(Languages.English, 3),
                [GetId(Languages.Hebrew)] = new Language(Languages.Hebrew, 2)
            };
        }

        private ConcurrentDictionary<int, Skill> CreateSkills()
        {
            return new ConcurrentDictionary<int, Skill>
            {
                [GetId(Skills.Python)] = new Skill(Skills.Python, 3, 1),
                [GetId(Skills.API)] = new Skill(Skills.API, 2, 2)
            };
        }

        [TestMethod]
        public void Role_Constructor_ShouldInitializeFieldsCorrectly()
        {
            // Arrange
            var languages = CreateLanguages();
            var skills = CreateSkills();

            // Act
            var role = new Role("Developer", 1, 10, TimeZones.Flexible, languages, skills, 3, 100, "Develop stuff");

            // Assert
            Assert.AreEqual("Developer", role.RoleName);
            Assert.AreEqual(1, role.RoleId);
            Assert.AreEqual(10, role.ProjectId);
            Assert.AreEqual(TimeZones.Flexible, role.TimeZone);
            Assert.AreEqual(3, role.YearsExperience);
            Assert.AreEqual(100, role.JobPercentage);
            Assert.AreEqual("Develop stuff", role.Description);
            Assert.AreEqual(2, role.ForeignLanguages.Count);
            Assert.AreEqual(2, role.Skills.Count);
        }

        [TestMethod]
        public void RoleDTO_Constructor_ShouldInitializeFieldsCorrectly()
        {
            // Arrange
            var roleDTO = new RoleDTO
            {
                RoleId = 2,
                RoleName = "QA Tester",
                ProjectId = 5,
                TimeZoneId = GetId(TimeZones.Morning),
                YearsExperience = 2,
                JobPercentage = 0.5,
                Description = "Test applications",
                ForeignLanguages = new List<RoleLanguagesDTO>
                {
                    new RoleLanguagesDTO { LanguageTypeId = GetId(Languages.English), Level = 3}
                },
                Skills = new List<RoleSkillsDTO>
                {
                    new RoleSkillsDTO { SkillTypeId = GetId(Skills.Java), Level = 3, Priority = 1}
                }
            };

            // Act
            var role = new Role(roleDTO);

            // Assert
            Assert.AreEqual("QA Tester", role.RoleName);
            Assert.AreEqual(2, role.RoleId);
            Assert.AreEqual(5, role.ProjectId);
            Assert.AreEqual(TimeZones.Morning, role.TimeZone);
            Assert.AreEqual(2, role.YearsExperience);
            Assert.AreEqual(0.5, role.JobPercentage);
            Assert.AreEqual("Test applications", role.Description);
            Assert.AreEqual(1, role.ForeignLanguages.Count);
            Assert.AreEqual(1, role.Skills.Count);
        }

        [TestMethod]
        public void RemoveEmployeeAssign_ShouldSetEmployeeIdToNull()
        {
            // Arrange
            var role = new Role("Developer", 1, 10, TimeZones.Flexible, new(), new(), 2, 80, "Develop stuff");
            role.EmployeeId = 123; // assigned employee

            // Act
            role.RemoveEmployeeAssign();

            // Assert
            Assert.IsNull(role.EmployeeId);
        }

        // ================= BAD / INVALID CASES ===================

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RoleDTO_Constructor_ShouldFail_WhenForeignLanguagesIsNull()
        {
            // Arrange
            var roleDTO = new RoleDTO
            {
                RoleId = 3,
                RoleName = "InvalidRole",
                ProjectId = 1,
                TimeZoneId = GetId(TimeZones.Evening),
                YearsExperience = 1,
                JobPercentage = 100,
                Description = "Missing ForeignLanguages",
                ForeignLanguages = null,
                Skills = new List<RoleSkillsDTO>()
            };

            // Act
            var role = new Role(roleDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void RoleDTO_Constructor_ShouldFail_WhenSkillsIsNull()
        {
            // Arrange
            var roleDTO = new RoleDTO
            {
                RoleId = 4,
                RoleName = "AnotherInvalidRole",
                ProjectId = 1,
                TimeZoneId = GetId(TimeZones.Noon),
                YearsExperience = 1,
                JobPercentage = 100,
                Description = "Missing Skills",
                ForeignLanguages = new List<RoleLanguagesDTO>(),
                Skills = null 
            };

            // Act
            var role = new Role(roleDTO);
        }

    }
}