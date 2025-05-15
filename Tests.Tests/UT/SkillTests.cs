using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeadcountAllocation.Domain;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Net.Mail;
namespace UT.Tests
{
    [TestClass]
    public class SkillTests
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
        public void Skill_Constructor_ShouldInitializeFieldsCorrectly()
        {
            // Arrange
            var skillType = Skills.Python;
            var level = 3;
            var priority = 1;

            // Act
            var skill = new Skill(skillType, level, priority);

            // Assert
            Assert.AreEqual(GetId(skillType), skill.SkillId);
            Assert.AreEqual(skillType, skill.SkillType);
            Assert.AreEqual(level, skill.Level);
            Assert.AreEqual(priority, skill.Priority);
        }

        [TestMethod]
        public void Skill_FromEmployeeSkillsDTO_ShouldInitializeCorrectly()
        {
            // Arrange
            var dto = new EmployeeSkillsDTO
            {
                SkillTypeId = GetId(Skills.API),
                Level = 2,
                Priority = 3
            };

            // Act
            var skill = new Skill(dto);

            // Assert
            Assert.AreEqual(dto.SkillTypeId, skill.SkillId);
            Assert.AreEqual(Skills.API, skill.SkillType);
            Assert.AreEqual(2, skill.Level);
            Assert.AreEqual(3, skill.Priority);
        }

        [TestMethod]
        public void Skill_FromRoleSkillsDTO_ShouldInitializeCorrectly()
        {
            // Arrange
            var dto = new RoleSkillsDTO
            {
                SkillTypeId = GetId(Skills.Java),
                Level = 3,
                Priority = 2
            };

            // Act
            var skill = new Skill(dto);

            // Assert
            Assert.AreEqual(dto.SkillTypeId, skill.SkillId);
            Assert.AreEqual(Skills.Java, skill.SkillType);
            Assert.AreEqual(3, skill.Level);
            Assert.AreEqual(2, skill.Priority);
        }

        // ================= BAD / INVALID CASES ===================

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Skill_FromEmployeeSkillsDTO_ShouldFail_WhenSkillTypeIdIsInvalid()
        {
            // Arrange
            var dto = new EmployeeSkillsDTO
            {
                SkillTypeId = 9999, // Invalid Skill ID
                Level = 2,
                Priority = 1
            };

            // Act
            var skill = new Skill(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Skill_FromRoleSkillsDTO_ShouldFail_WhenSkillTypeIdIsInvalid()
        {
            // Arrange
            var dto = new RoleSkillsDTO
            {
                SkillTypeId = 9999, // Invalid Skill ID
                Level = 3,
                Priority = 2
            };

            // Act
            var skill = new Skill(dto);
        }
    }
}