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
    public class EmployeeTests
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
        public void Constructor_ShouldInitializeEmployeeCorrectly()
        {
            // Arrange
            var email = new MailAddress("test@example.com");
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(0, new Language(Languages.English, 3));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(0, new Skill(Skills.API, 2, 1));

            // Act
            var employee = new Employee(
                "John Doe",
                1,
                "123456789",
                email,
                TimeZones.Flexible,
                languages,
                skills,
                5,
                100,
                "password123",
                false
            );

            // Assert
            Assert.AreEqual("John Doe", employee.UserName);
            Assert.AreEqual(1, employee.EmployeeId);
            Assert.AreEqual("123456789", employee.PhoneNumber);
            Assert.AreEqual(email, employee.Email);
            Assert.AreEqual(TimeZones.Flexible, employee.TimeZone);
            Assert.AreEqual(5, employee.YearsExperience);
            Assert.AreEqual(100, employee.JobPercentage);
            Assert.IsFalse(employee.IsManager);
            Assert.AreEqual(1, employee.ForeignLanguages.Count);
            Assert.AreEqual(1, employee.Skills.Count);
        }

        [TestMethod]
        public void EncryptPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var employee = CreateTestEmployee();

            // Act
            var hashedPassword = employee.EncryptPassword("mypassword");

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(hashedPassword));
            Assert.AreNotEqual("mypassword", hashedPassword);
        }

        [TestMethod]
        public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
        {
            // Arrange
            var employee = CreateTestEmployee();
            var rawPassword = "mypassword";
            var hashedPassword = employee.EncryptPassword(rawPassword);

            // Act
            var result = employee.VerifyPassword(rawPassword, hashedPassword);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldAddRole()
        {
            // Arrange
            var employee = CreateTestEmployee();
            var role = new Role("Test Role", 1, 1, TimeZones.Morning, new ConcurrentDictionary<int, Language>(), new ConcurrentDictionary<int, Skill>(), 2, 100, "Test");

            // Act
            employee.AssignEmployeeToRole(role);

            // Assert
            Assert.AreEqual(1, employee.Roles.Count);
            Assert.IsTrue(employee.Roles.ContainsKey(role.RoleId));
        }

        [TestMethod]
        public void EditMethods_ShouldUpdateProperties()
        {
            // Arrange
            var employee = CreateTestEmployee();

            // Act
            employee.EditEmail(new MailAddress("new@example.com"));
            employee.EditPhoneNumber("987654321");
            employee.EditTimeZone(TimeZones.Noon);
            employee.EditYearOfExpr(10);
            employee.EditJobPercentage(80);

            // Assert
            Assert.AreEqual("new@example.com", employee.Email.Address);
            Assert.AreEqual("987654321", employee.PhoneNumber);
            Assert.AreEqual(TimeZones.Noon, employee.TimeZone);
            Assert.AreEqual(10, employee.YearsExperience);
            Assert.AreEqual(80, employee.JobPercentage);
        }

        [TestMethod]
        public void AddAndRemoveSkills_ShouldWorkCorrectly()
        {
            // Arrange
            var employee = CreateTestEmployee();
            var skill = new Skill(Skills.Java, 3, 2);

            // Act
            employee.AddSkill(skill);
            bool existsAfterAdd = employee.Skills.ContainsKey(skill.SkillId);
            employee.RemoveSkill(skill.SkillId);
            bool existsAfterRemove = employee.Skills.ContainsKey(skill.SkillId);

            // Assert
            Assert.IsTrue(existsAfterAdd);
            Assert.IsFalse(existsAfterRemove);
        }

        [TestMethod]
        public void AddAndRemoveLanguages_ShouldWorkCorrectly()
        {
            // Arrange
            var employee = CreateTestEmployee();
            var language = new Language(Languages.Hebrew, 3);

            // Act
            employee.AddLanguage(language);
            bool existsAfterAdd = employee.ForeignLanguages.ContainsKey(language.LanguageID);
            employee.RemoveLanguage(language.LanguageID);
            bool existsAfterRemove = employee.ForeignLanguages.ContainsKey(language.LanguageID);

            // Assert
            Assert.IsTrue(existsAfterAdd);
            Assert.IsFalse(existsAfterRemove);
        }

        [TestMethod]
        public void Login_ShouldReturnIsManagerStatus()
        {
            // Arrange
            var employee = CreateTestEmployee();
            employee.IsManager = true;

            // Act
            var loginResult = employee.Login();

            // Assert
            Assert.IsTrue(loginResult);
        }

        private Employee CreateTestEmployee()
        {
            return new Employee(
                "John Doe",
                1,
                "123456789",
                new MailAddress("test@example.com"),
                TimeZones.Flexible,
                new ConcurrentDictionary<int, Language>(),
                new ConcurrentDictionary<int, Skill>(),
                5,
                100,
                "mypassword",
                false
            );
        }

        [TestMethod]
        public void AssignEmployeeToRole_NullRole_ShouldThrowException()
        {
            var employee = new Employee(
                "John Doe",
                1,
                "1234567890",
                new System.Net.Mail.MailAddress("johndoe@example.com"),
                TimeZones.Morning,
                new(),
                new(),
                5,
                1.0,
                "password123",
                false);

            Assert.ThrowsException<NullReferenceException>(() => employee.AssignEmployeeToRole(null));
        }

        [TestMethod]
        public void AddSkill_NullSkill_ShouldThrowException()
        {
            var employee = new Employee(
                "John Doe",
                1,
                "1234567890",
                new System.Net.Mail.MailAddress("johndoe@example.com"),
                TimeZones.Morning,
                new(),
                new(),
                5,
                1.0,
                "password123",
                false);

            Assert.ThrowsException<NullReferenceException>(() => employee.AddSkill(null));
        }

        [TestMethod]
        public void AddLanguage_NullLanguage_ShouldThrowException()
        {
            var employee = new Employee(
                "John Doe",
                1,
                "1234567890",
                new System.Net.Mail.MailAddress("johndoe@example.com"),
                TimeZones.Morning,
                new(),
                new(),
                5,
                1.0,
                "password123",
                false);

            Assert.ThrowsException<NullReferenceException>(() => employee.AddLanguage(null));
        }




    }
}