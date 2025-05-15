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
    public class ManagerFacaseTests
    {

        private ManagerFacade manager;

        [TestInitialize]
        public void Setup()
        {
            DBcontext.SetTestDatabase(); // Connect to test DB
            var context = DBcontext.GetInstance();
            context.ClearDatabase();     // Clean database before each test
            context.SeedStaticTables();
            ManagerFacade.Dispose();
            manager = ManagerFacade.GetInstance();
        }

        [TestCleanup]
        public void Cleanup()
        {
            ManagerFacade.Dispose();
        }

        [TestMethod]
        public void CreateProject_ShouldAddProject()
        {
            var roles = new Dictionary<int, Role>();
            int projectId = manager.CreateProject("Test Project", "Description", DateTime.Now, 40, roles);
            var project = manager.GetProjectById(projectId);

            Assert.IsNotNull(project);
            Assert.AreEqual("Test Project", project.ProjectName);
        }

        [TestMethod]
        public void CreateEmployee_ShouldAddEmployee()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            var result = manager.CreateEmployee("John Doe", "050-1234567", "john.doe@test.com",
                TimeZones.Flexible, languages, skills, 5, 100, false);

            Assert.IsNotNull(result);
            Assert.AreEqual("John Doe", result.Item1);
        }

        [TestMethod]
        public void AddTicket_ShouldAddTicket()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            var employee = manager.CreateEmployee("Ticket Guy", "050-0000000", "ticket.guy@test.com", 
                TimeZones.Flexible, languages, skills, 2, 100, false);

            var employeeId = manager.GetAllEmployees()[0].EmployeeId;
            int ticketId = manager.AddTicket(employeeId, DateTime.Today, DateTime.Today.AddDays(5), "Vacation");

            Assert.IsTrue(ticketId >= 0);
            var openTickets = manager.GetOpensTickets();
            Assert.AreEqual(1, openTickets.Count);
        }

        // ========== Edit Tests ==========

        [TestMethod]
        public void EditProjectName_ShouldChangeProjectName()
        {
            int projectId = manager.CreateProject("Old Name", "Desc", DateTime.Now, 40, new());
            manager.EditProjectName(projectId, "New Name");

            var project = manager.GetProjectById(projectId);
            Assert.AreEqual("New Name", project.ProjectName);
        }

        [TestMethod]
        public void EditPhoneNumber_ShouldChangePhoneNumber()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            var result = manager.CreateEmployee("Phone User", "000", "phone.user@test.com", 
                TimeZones.Flexible, languages, skills, 2, 100, false);

            var employeeId = manager.GetAllEmployees()[0].EmployeeId;
            manager.EditPhoneNumber(employeeId, "123456789");

            Assert.AreEqual("123456789", manager.GetEmployeeById(employeeId).PhoneNumber);
        }

        [TestMethod]
        public void EditEmail_ShouldChangeEmail()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            manager.CreateEmployee("Mail User", "050", "mail.user@test.com",
                TimeZones.Flexible, languages, skills, 1, 100, false);

            var employeeId = manager.GetAllEmployees()[0].EmployeeId;
            manager.EditEmail(employeeId, "new.mail@test.com");

            Assert.AreEqual("new.mail@test.com", manager.GetEmployeeById(employeeId).Email.Address);
        }

        // ========== Role Management ==========

        [TestMethod]
        public void AddRoleToProject_ShouldAddRoleSuccessfully()
        {
            var roles = new Dictionary<int, Role>();
            int projectId = manager.CreateProject("Role Project", "Role Desc", DateTime.Now, 40, roles);

            var foreignLanguages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            Role role = manager.AddRoleToProject("Developer", projectId, TimeZones.Flexible, foreignLanguages, skills, 1, 100, "Dev role");
            Assert.IsNotNull(role);
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldAssignCorrectly()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            manager.CreateEmployee("Employee", "050", "employee@test.com", TimeZones.Flexible, languages, skills, 3, 100, false);
            int employeeId = manager.GetAllEmployees()[0].EmployeeId;

            int projectId = manager.CreateProject("Proj", "desc", DateTime.Now, 40, new());
            Role role = manager.AddRoleToProject("Developer", projectId, TimeZones.Flexible, new(), new(), 1, 100, "Dev role");

            manager.AssignEmployeeToRole(employeeId, role);

            var employee = manager.GetEmployeeById(employeeId);
            Assert.AreEqual(role.RoleId, employee.Roles[role.RoleId].RoleId);
        }

        // ========== Delete Tests ==========

        [TestMethod]
        public void DeleteProject_ShouldRemoveProject()
        {
            int projectId = manager.CreateProject("Delete Me", "desc", DateTime.Now, 40, new());
            manager.DeleteProject(projectId);

            var project = manager.GetProjectById(projectId);
            Assert.IsNull(project);
        }

        [TestMethod]
        public void DeleteEmployee_ShouldRemoveEmployee()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            manager.CreateEmployee("Delete User", "050", "delete.user@test.com", TimeZones.Flexible, languages, skills, 2, 100, false);

            int employeeId = manager.GetAllEmployees()[0].EmployeeId;
            manager.DeleteEmployee(employeeId);

            var employee = manager.GetEmployeeById(employeeId);
            Assert.IsNull(employee);
        }

        // ========== Ticket Management Tests ==========

        [TestMethod]
        public void CloseTicket_ShouldMarkTicketClosed()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            manager.CreateEmployee("TicketCloser", "050", "closer@test.com", TimeZones.Flexible, languages, skills, 2, 100, false);

            int employeeId = manager.GetAllEmployees()[0].EmployeeId;
            int ticketId = manager.AddTicket(employeeId, DateTime.Today, DateTime.Today.AddDays(5), "Closure");

            manager.CloseTicket(ticketId);

            var openTickets = manager.GetOpensTickets();
            Assert.AreEqual(0, openTickets.Count);
        }

        // ========== Fail (Bad) Tests ==========

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AssignEmployeeToRole_NonExistingEmployee_ShouldThrow()
        {
            int projectId = manager.CreateProject("Proj", "desc", DateTime.Now, 40, new());
            Role role = manager.AddRoleToProject("Dev", projectId, TimeZones.Flexible, new(), new(), 1, 100, "Dev");

            manager.AssignEmployeeToRole(9999, role); // Employee doesn't exist
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddRoleToNonExistingProject_ShouldThrow()
        {
            manager.AddRoleToProject("Invalid Role", 9999, TimeZones.Flexible, new(), new(), 1, 100, "desc");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteNonExistingEmployee_ShouldThrow()
        {
            manager.DeleteEmployee(9999);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddDuplicateSkill_ShouldThrow()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            manager.CreateEmployee("Skill User", "050", "skill.user@test.com", TimeZones.Flexible, languages, skills, 2, 100, false);

            int employeeId = manager.GetAllEmployees()[0].EmployeeId;
            var skill = new Skill(Skills.API, 2, 1);

            manager.AddSkill(employeeId, skill);
            manager.AddSkill(employeeId, skill); // Adding duplicate
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateEmployee_InvalidEmail_ShouldThrow()
        {
            var skills = new ConcurrentDictionary<int, Skill>();
            var languages = new ConcurrentDictionary<int, Language>();
            manager.CreateEmployee("Bad Email", "050", "bademail", TimeZones.Flexible, languages, skills, 2, 100, false);
        }


    }
}