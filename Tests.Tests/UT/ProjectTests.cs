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
    public class ProjectTests
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

    [TestMethod]
        public void Constructor_WithProjectDTO_ShouldInitializeCorrectly()
        {
            // Arrange
            var dto = new ProjectDTO
            {
                ProjectId = 1,
                ProjectName = "Test Project",
                Description = "Test Desc",
                Date = DateTime.Today,
                RequiredHours = 100,
                Roles = new List<RoleDTO>()
            };

            // Act
            var project = new Project(dto);

            // Assert
            Assert.AreEqual(dto.ProjectId, project.ProjectId);
            Assert.AreEqual(dto.ProjectName, project.ProjectName);
            Assert.AreEqual(dto.Description, project.Description);
            Assert.AreEqual(dto.Date, project.Date);
            Assert.AreEqual(dto.RequiredHours, project.RequiredHours);
            Assert.AreEqual(0, project.Roles.Count); // No roles added yet
        }

        [TestMethod]
        public void AddRoleToProject_ShouldAddRoleSuccessfully()
        {
            // Arrange
            var project = new Project("Project A", 1, "Desc", DateTime.Today, 120, new());
            context.Projects.Add(new ProjectDTO(project));
            context.SaveChanges();
            var foreignLanguages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            // Act
            var newRole = project.AddRoleToProject(
                "Developer",
                TimeZones.Flexible,
                foreignLanguages,
                skills,
                2,
                100,
                "Coding tasks",
                0
            );

            // Assert
            Assert.IsTrue(project.Roles.ContainsKey(newRole.RoleId));
            Assert.AreEqual("Developer", newRole.RoleName);
            Assert.AreEqual(1, project.Roles.Count);
        }

        [TestMethod]
        public void RemoveRole_ShouldRemoveRoleSuccessfully()
        {
            // Arrange
            var project = new Project("Project B", 2, "Description", DateTime.Today, 80, new());
            context.Projects.Add(new ProjectDTO(project));
            context.SaveChanges();
            var foreignLanguages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            var role = project.AddRoleToProject(
                "Tester",
                TimeZones.Morning,
                foreignLanguages,
                skills,
                1,
                50,
                "Testing tasks",
                0
            );

            // Act
            project.RemoveRole(role.RoleId);

            // Assert
            Assert.IsFalse(project.Roles.ContainsKey(role.RoleId));
            Assert.AreEqual(0, project.Roles.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RemoveRole_ShouldThrow_WhenRoleDoesNotExist()
        {
            // Arrange
            var project = new Project("Project C", 3, "Desc", DateTime.Today, 100, new());

            // Act
            project.RemoveRole(999); // Non-existing RoleId
        }

        [TestMethod]
        public void EditProjectFields_ShouldUpdateValues()
        {
            // Arrange
            var project = new Project("Old Name", 4, "Old Desc", DateTime.Today, 40, new());
            var newDate = DateTime.Today.AddDays(5);

            // Act
            project.EditProjectName("New Name");
            project.EditProjectDescription("New Description");
            project.EditProjectDate(newDate);
            project.EditProjectRequierdHours(200);

            // Assert
            Assert.AreEqual("New Name", project.ProjectName);
            Assert.AreEqual("New Description", project.Description);
            Assert.AreEqual(newDate, project.Date);
            Assert.AreEqual(200, project.RequiredHours);
        }

        [TestMethod]
        public void GetRoles_ShouldReturnRolesDictionary()
        {
            // Arrange
            var project = new Project("Project D", 5, "Desc", DateTime.Today, 50, new());
            context.Projects.Add(new ProjectDTO(project));
            context.SaveChanges();
            var foreignLanguages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            var role = project.AddRoleToProject(
                "Analyst",
                TimeZones.Noon,
                foreignLanguages,
                skills,
                3,
                100,
                "Analysis tasks",
                0
            );

            // Act
            var roles = project.GetRoles();

            // Assert
            Assert.AreEqual(1, roles.Count);
            Assert.IsTrue(roles.ContainsKey(role.RoleId));
        }

        [TestMethod]
        public void RemoveRole_NonExistentRole_ShouldThrowException()
        {
            var project = new Project("Test Project", 1, "Test Description", DateTime.Now, 10, new Dictionary<int, Role>());

            var context = DBcontext.GetInstance();
            context.Projects.Add(new ProjectDTO
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                Description = project.Description,
                Date = project.Date,
                RequiredHours = project.RequiredHours
            });
            context.SaveChanges();

            Assert.ThrowsException<Exception>(() => project.RemoveRole(999)); // 999 does not exist
        }
    }

}