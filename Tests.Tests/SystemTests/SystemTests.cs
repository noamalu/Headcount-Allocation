using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeadcountAllocation.Domain;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Net.Mail;
namespace Service.Tests
{
    [TestClass]
    public class SystemTests
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
        public void Login_ShouldSucceed()
        {
            var result = manager.CreateEmployee("AdminUser", "123456", "admin@example.com", TimeZones.Flexible, new(), new(), 10, 100, true);
            var createdEmployee = manager.GetAllEmployees().First();
            var loginResult = manager.Login(createdEmployee.UserName, result.Item2); // result.Item2 is the generated password
            Assert.IsNotNull(loginResult);
            Assert.AreEqual(createdEmployee.EmployeeId, loginResult);
        }

        [TestMethod]
        public void Login_ShouldFail_WrongPassword()
        {
            var result = manager.CreateEmployee("WrongPassUser", "123456", "wrongpass@example.com", TimeZones.Flexible, new(), new(), 5, 100, true);
            var createdEmployee = manager.GetAllEmployees().First();
            Assert.ThrowsException<Exception>(() => manager.Login(createdEmployee.UserName, "WrongPassword123"));
        }

        [TestMethod]
        public void Login_ShouldFail_UsernameNotFound()
        {
            Assert.ThrowsException<Exception>(() => manager.Login("nonexistentuser", "anyPassword"));
        }

        [TestMethod]
        public void CreateEmployee_ShouldSucceed()
        {
            var result = manager.CreateEmployee("Tom", "5551234", "tom@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            Assert.IsNotNull(result);
            Assert.AreEqual("Tom", result.Item1); // username
            Assert.AreEqual(8, result.Item2.Length); // password length
        }

        [TestMethod]
        public void CreateEmployee_ShouldFail_InvalidEmail()
        {
            Assert.ThrowsException<Exception>(() => manager.CreateEmployee("Tom", "5551234", "bad-email", TimeZones.Morning, new(), new(), 2, 100, true));
        }

        [TestMethod]
        public void CreateEmployee_ShouldFail_ExsistsEmployee()
        {
            manager.CreateEmployee("Tom", "5551234", "tom@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            Assert.ThrowsException<Exception>(() => manager.CreateEmployee("Tom", "5551234", "tom@example.com", TimeZones.Morning, new(), new(), 2, 100, true));
        }

        [TestMethod]
        public void CreateProject_ShouldSucceed()
        {
            var projectName = "Test Project";
            var description = "Test Description";
            var date = DateTime.Now;
            int requiredHours = 100;
            var roles = new Dictionary<int, Role>();
            int projectId = manager.CreateProject(projectName, description, date, requiredHours, roles);
            var project = manager.GetProjectById(projectId);
            Assert.IsNotNull(project);
            Assert.AreEqual(projectName, project.ProjectName);
        }

        [TestMethod]
        public void CreateProject_ShouldFail_ProjectNameIsNull()
        {
            string projectName = null;
            var description = "Test Description";
            var date = DateTime.Now;
            int requiredHours = 100;
            var roles = new Dictionary<int, Role>();
            Assert.ThrowsException<Exception>(() =>
            {
                manager.CreateProject(projectName, description, date, requiredHours, roles);
            });
        }

        [TestMethod]
        public void EditProjectName_ShouldSucceed_WhenProjectExists()
        {
            // Arrange
            var projectId = manager.CreateProject("Original", "Desc", DateTime.Now, 100, new());

            // Act
            manager.EditProjectName(projectId, "Updated Name");

            // Assert
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual("Updated Name", updatedProject.ProjectName);
        }

        [TestMethod]
        public void AddRoleToProject_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();
            var role = manager.AddRoleToProject("Developer", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Description", DateTime.Now);
            Assert.IsNotNull(role);
            Assert.AreEqual("Developer", role.RoleName);
        }

        [TestMethod]
        public void AddRoleToProject_ShouldFail_ProjectDoesNotExist()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();
            Assert.ThrowsException<Exception>(() =>
                manager.AddRoleToProject("Dev", 999, TimeZones.Morning, languages, skills, 1, 100, "Role Desc", DateTime.Now)
            );
        }

        [TestMethod]
        public void EditEmployeeDetails_Succeed()
        {
            var createEmp = manager.CreateEmployee("Eva", "999", "eva@example.com", TimeZones.Morning, new(), new(), 5, 80, false);
            var employee = manager.GetAllEmployees().First();
            manager.EditEmail(employee.EmployeeId, "eva.new@example.com");
            manager.EditPhoneNumber(employee.EmployeeId, "123456789");
            manager.EditTimeZone(employee.EmployeeId, TimeZones.Evening);
            manager.EditYearOfExpr(employee.EmployeeId, 10);
            manager.EditJobPercentage(employee.EmployeeId, 90);
            var updatedEmployee = manager.GetEmployeeById(employee.EmployeeId);
            Assert.AreEqual("eva.new@example.com", updatedEmployee.Email.Address);
            Assert.AreEqual("123456789", updatedEmployee.PhoneNumber);
            Assert.AreEqual(TimeZones.Evening, updatedEmployee.TimeZone);
            Assert.AreEqual(10, updatedEmployee.YearsExperience);
            Assert.AreEqual(90, updatedEmployee.JobPercentage);
        }

        [TestMethod]
        public void EditEmployee_ShouldFail_EmployeeNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditEmail(999, "noone@example.com"));
            Assert.ThrowsException<Exception>(() => manager.EditPhoneNumber(999, "0000000"));
            Assert.ThrowsException<Exception>(() => manager.EditTimeZone(999, TimeZones.Flexible));
            Assert.ThrowsException<Exception>(() => manager.EditYearOfExpr(999, 3));
            Assert.ThrowsException<Exception>(() => manager.EditJobPercentage(999, 50));
        }

        [TestMethod]
        public void EditProjectName_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Original", "Desc", DateTime.Now, 100, new());
            manager.EditProjectName(projectId, "Updated Name");
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual("Updated Name", updatedProject.ProjectName);
        }

        [TestMethod]
        public void EditProjectName_ShouldFail_ProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditProjectName(999, "New Name"));
        }

        [TestMethod]
        public void EditProjectDescription_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Proj", "Old Desc", DateTime.Now, 100, new());
            manager.EditProjectDescription(projectId, "New Desc");
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual("New Desc", updatedProject.Description);
        }

        [TestMethod]
        public void EditProjectDescription_ShouldFail_ProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditProjectDescription(999, "New Desc"));
        }

        [TestMethod]
        public void EditProjectDate_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Proj", "Desc", DateTime.Now, 100, new());
            var newDate = DateTime.Now.AddDays(10);
            manager.EditProjectDate(projectId, newDate);
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual(newDate, updatedProject.Date);
        }

        [TestMethod]
        public void EditProjectDate_ShouldFail_ProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditProjectDate(999, DateTime.Now));
        }

        [TestMethod]
        public void EditProjectRequiredHours_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Proj", "Desc", DateTime.Now, 100, new());
            manager.EditProjectRequierdHours(projectId, 200);
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual(200, updatedProject.RequiredHours);
        }

        [TestMethod]
        public void EditProjectRequiredHours_ShouldFail_ProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditProjectRequierdHours(999, 300));
        }

        [TestMethod]
        public void DeleteEmployee_ShouldSucceed()
        {
            var createEmp = manager.CreateEmployee("Olivia", "999", "olivia@example.com", TimeZones.Noon, new(), new(), 5, 100, false);
            var employee = manager.GetAllEmployees().First();
            manager.DeleteEmployee(employee.EmployeeId);
            Assert.IsNull(manager.GetEmployeeById(employee.EmployeeId));
        }

        [TestMethod]
        public void DeleteEmployee_ShouldFail_EmployeeNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.DeleteEmployee(999));
        }

        [TestMethod]
        public void DeleteProject_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Proj", "Desc", DateTime.Now, 100, new());
            manager.DeleteProject(projectId);
            Assert.IsNull(manager.GetProjectById(projectId));
        }

        [TestMethod]
        public void DeleteProject_ShouldFailProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.DeleteProject(999));
        }

        [TestMethod]
        public void RemoveRole_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            var role = manager.AddRoleToProject("Dev", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Desc", DateTime.Now);
            manager.RemoveRole(projectId, role.RoleId);
            Assert.IsFalse(manager.GetAllRolesByProject(projectId).ContainsKey(role.RoleId));
        }

        [TestMethod]
        public void RemoveRole_ShouldFail_ProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.RemoveRole(999, 1));
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldSucceed()
        {
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();
            var role = manager.AddRoleToProject("Dev", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Desc", DateTime.Now);
            var empResult = manager.CreateEmployee("John", "123", "john@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().FirstOrDefault();
            manager.AssignEmployeeToRole(employee.EmployeeId, role);
            Assert.AreEqual(employee.EmployeeId, role.EmployeeId);
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldFail_EmployeeDoesNotExist()
        {
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();
            var role = manager.AddRoleToProject("Dev", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Desc", DateTime.Now);
            Assert.ThrowsException<Exception>(() => manager.AssignEmployeeToRole(999, role));
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldFail_RoleDoesNotExist()
        {
            var empResult = manager.CreateEmployee("Jane", "321", "jane@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().FirstOrDefault();
            var fakeRole = new Role("FakeRole", 999, 0, TimeZones.Morning, new(), new(), 0, 100, "Desc", DateTime.Now);
            Assert.ThrowsException<Exception>(() => manager.AssignEmployeeToRole(employee.EmployeeId, fakeRole));
        }

        [TestMethod]
        public void AddTicket_ShouldSucceed()
        {
            var createEmp = manager.CreateEmployee("John", "123", "john@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();
            var ticketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "Sick Leave", new Reason(Reasons.ReserveDuty), false);
            Assert.IsTrue(manager.Tickets.ContainsKey(ticketId));
        }

        [TestMethod]
        public void AddTicket_ShouldFail_EmployeeDoesNotExist()
        {
            Assert.ThrowsException<KeyNotFoundException>(() => manager.AddTicket(999, DateTime.Now, DateTime.Now.AddDays(1), "Desc", new Reason(Reasons.ReserveDuty)));
        }

        [TestMethod]
        public void CloseTicket_ShouldSucceed_TicketExists()
        {
            var createEmp = manager.CreateEmployee("Jane", "123", "jane@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();
            var ticketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), "Vacation", new Reason(Reasons.LongVacation), false);
            manager.CloseTicket(ticketId);
            Assert.IsFalse(manager.Tickets[ticketId].Open);
        }

        [TestMethod]
        public void CloseTicket_ShouldFail_TicketDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.CloseTicket(999));
        }

        [TestMethod]
        public void GetOpensTickets_ShouldReturnOnlyOpenTickets()
        {
            var createEmp = manager.CreateEmployee("Mike", "123", "mike@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();
            var ticketId1 = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), "Ticket1", new Reason(Reasons.Other), false);
            var ticketId2 = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(2), DateTime.Now.AddDays(4), "Ticket2", new Reason(Reasons.Other), false);
            manager.CloseTicket(ticketId1);
            var openTickets = manager.GetOpensTickets();
            Assert.AreEqual(1, openTickets.Count);
            Assert.AreEqual(ticketId2, openTickets.First().TicketId);
        }

        [TestMethod]
        public void GetOpensTickets5Days_ShouldReturnTicketsStartingSoon()
        {
            var createEmp = manager.CreateEmployee("Sara", "123", "sara@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();
            var soonTicketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(3), DateTime.Now.AddDays(5), "Soon Ticket", new Reason(Reasons.Other), false);
            var farTicketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(10), DateTime.Now.AddDays(12), "Far Ticket", new Reason(Reasons.Other), false);
            var soonTickets = manager.GetOpensTickets5days();
            Assert.IsTrue(soonTickets.Any(t => t.TicketId == soonTicketId));
            Assert.IsFalse(soonTickets.Any(t => t.TicketId == farTicketId));
        }


        



        

    }
}