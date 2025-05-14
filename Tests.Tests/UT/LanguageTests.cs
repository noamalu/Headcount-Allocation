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
    public class LanguageTests
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
        public void Constructor_WithLanguagesEnum_ShouldInitializeCorrectly()
        {
            // Arrange
            var languageType = Languages.English;
            var level = 3;

            // Act
            var language = new Language(languageType, level);

            // Assert
            Assert.AreEqual(GetId(languageType), language.LanguageID);
            Assert.AreEqual(languageType, language.LanguageType);
            Assert.AreEqual(level, language.Level);
        }

        [TestMethod]
        public void Constructor_WithEmployeeLanguagesDTO_ShouldInitializeCorrectly()
        {
            // Arrange
            var dto = new EmployeeLanguagesDTO
            {
                LanguageTypeId = GetId(Languages.Hebrew),
                Level = 2
            };

            // Act
            var language = new Language(dto);

            // Assert
            Assert.AreEqual(dto.LanguageTypeId, language.LanguageID);
            Assert.AreEqual(Languages.Hebrew, language.LanguageType);
            Assert.AreEqual(dto.Level, language.Level);
        }

        [TestMethod]
        public void Constructor_WithRoleLanguagesDTO_ShouldInitializeCorrectly()
        {
            // Arrange
            var dto = new RoleLanguagesDTO
            {
                LanguageTypeId = GetId(Languages.English),
                Level = 1
            };

            // Act
            var language = new Language(dto);

            // Assert
            Assert.AreEqual(dto.LanguageTypeId, language.LanguageID);
            Assert.AreEqual(Languages.English, language.LanguageType);
            Assert.AreEqual(dto.Level, language.Level);
        }

        [TestMethod]
        public void Language_NegativeLevel_ShouldCreateSuccessfully()
        {
            // Language allows negative levels (not forbidden in constructor), but you might want to add validation!
            var language = new Language(Languages.English, -1);

            Assert.AreEqual(-1, language.Level);
        }
    }
}