using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeadcountAllocation.Domain;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using static HeadcountAllocation.Domain.Enums;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using HeadcountAllocation.Domain.Alert;
namespace IT.Tests
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

        [TestMethod]
        public void CreateProject_ShouldSucceed_WhenInputIsValid()
        {
            // Arrange
            var projectName = "Test Project";
            var description = "Test Description";
            var date = DateTime.Now;
            int requiredHours = 100;
            var roles = new Dictionary<int, Role>();

            // Act
            int projectId = manager.CreateProject(projectName, description, date, requiredHours, roles);

            // Assert
            var project = manager.GetProjectById(projectId);
            Assert.IsNotNull(project);
            Assert.AreEqual(projectName, project.ProjectName);
        }

        [TestMethod]
        public void CreateProject_ShouldFail_WhenProjectNameIsNull()
        {
            // Arrange
            string projectName = null;
            var description = "Test Description";
            var date = DateTime.Now;
            int requiredHours = 100;
            var roles = new Dictionary<int, Role>();

            // Act & Assert
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
        public void EditProjectName_ShouldFail_WhenProjectDoesNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.EditProjectName(999, "New Name"));
        }

        [TestMethod]
        public void EditProjectDescription_ShouldSucceed_WhenProjectExists()
        {
            var projectId = manager.CreateProject("Proj", "Old Desc", DateTime.Now, 100, new());
            manager.EditProjectDescription(projectId, "New Desc");
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual("New Desc", updatedProject.Description);
        }

        [TestMethod]
        public void EditProjectDescription_ShouldFail_WhenProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditProjectDescription(999, "New Desc"));
        }

        [TestMethod]
        public void EditProjectDate_ShouldSucceed_WhenProjectExists()
        {
            var projectId = manager.CreateProject("Proj", "Desc", DateTime.Now, 100, new());
            var newDate = DateTime.Now.AddDays(10);
            manager.EditProjectDate(projectId, newDate);
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual(newDate, updatedProject.Date);
        }

        [TestMethod]
        public void EditProjectDate_ShouldFail_WhenProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditProjectDate(999, DateTime.Now));
        }

        [TestMethod]
        public void EditProjectRequiredHours_ShouldSucceed_WhenProjectExists()
        {
            var projectId = manager.CreateProject("Proj", "Desc", DateTime.Now, 100, new());
            manager.EditProjectRequierdHours(projectId, 200);
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual(200, updatedProject.RequiredHours);
        }

        [TestMethod]
        public void EditProjectRequiredHours_ShouldFail_WhenProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.EditProjectRequierdHours(999, 300));
        }

        [TestMethod]
        public void DeleteProject_ShouldSucceed_WhenProjectExists()
        {
            var projectId = manager.CreateProject("Proj", "Desc", DateTime.Now, 100, new());
            manager.DeleteProject(projectId);
            Assert.IsNull(manager.GetProjectById(projectId));
        }

        [TestMethod]
        public void DeleteProject_ShouldFail_WhenProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.DeleteProject(999));
        }

        [TestMethod]
        public void AddRoleToProject_ShouldSucceed_WhenProjectExists()
        {
            // Arrange
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            // Act
            var role = manager.AddRoleToProject("Developer", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Description");

            // Assert
            Assert.IsNotNull(role);
            Assert.AreEqual("Developer", role.RoleName);
        }

        [TestMethod]
        public void AddRoleToProject_ShouldFail_WhenProjectDoesNotExist()
        {
            // Arrange
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            // Act & Assert
            Assert.ThrowsException<Exception>(() => 
                manager.AddRoleToProject("Dev", 999, TimeZones.Morning, languages, skills, 1, 100, "Role Desc")
            );
        }

        [TestMethod]
        public void RemoveRole_ShouldSucceed_WhenRoleExists()
        {
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            var role = manager.AddRoleToProject("Dev", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Desc");

            // Act
            manager.RemoveRole(projectId, role.RoleId);

            // Assert
            Assert.IsFalse(manager.GetAllRolesByProject(projectId).ContainsKey(role.RoleId));
        }

        [TestMethod]
        public void RemoveRole_ShouldFail_WhenProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.RemoveRole(999, 1));
        }

        [TestMethod]
        public void GetAllRolesByProject_ShouldReturnRoles_WhenProjectExists()
        {
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var roles = manager.GetAllRolesByProject(projectId);

            Assert.IsNotNull(roles);
            Assert.IsInstanceOfType(roles, typeof(Dictionary<int, Role>));
        }

        [TestMethod]
        public void GetAllRolesByProject_ShouldFail_WhenProjectDoesNotExist()
        {
            Assert.ThrowsException<Exception>(() => manager.GetAllRolesByProject(999));
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldSucceed_WhenEmployeeAndRoleExist()
        {
            // Arrange
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            var role = manager.AddRoleToProject("Dev", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Desc");

            var empResult = manager.CreateEmployee("John", "123", "john@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().FirstOrDefault();

            // Act
            manager.AssignEmployeeToRole(employee.EmployeeId, role);

            // Assert
            Assert.AreEqual(employee.EmployeeId, role.EmployeeId);
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldFail_WhenEmployeeDoesNotExist()
        {
            var projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            var languages = new ConcurrentDictionary<int, Language>();
            var skills = new ConcurrentDictionary<int, Skill>();

            var role = manager.AddRoleToProject("Dev", projectId, TimeZones.Morning, languages, skills, 1, 100, "Role Desc");

            Assert.ThrowsException<Exception>(() => manager.AssignEmployeeToRole(999, role));
        }

        [TestMethod]
        public void AssignEmployeeToRole_ShouldFail_WhenRoleDoesNotExist()
        {
            var empResult = manager.CreateEmployee("Jane", "321", "jane@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().FirstOrDefault();

            var fakeRole = new Role("FakeRole", 999, 0, TimeZones.Morning, new(), new(), 0, 100, "Desc");

            Assert.ThrowsException<Exception>(() => manager.AssignEmployeeToRole(employee.EmployeeId, fakeRole));
        }

        [TestMethod]
        public void AddTicket_ShouldSucceed_WhenEmployeeExists()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("John", "123", "john@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();

            // Act
            var ticketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "Sick Leave");

            // Assert
            Assert.IsTrue(manager.Tickets.ContainsKey(ticketId));
        }

        [TestMethod]
        public void AddTicket_ShouldFail_WhenEmployeeDoesNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => manager.AddTicket(999, DateTime.Now, DateTime.Now.AddDays(1), "Desc"));
        }

        [TestMethod]
        public void CloseTicket_ShouldSucceed_WhenTicketExists()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Jane", "123", "jane@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();

            var ticketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), "Vacation");

            // Act
            manager.CloseTicket(ticketId);

            // Assert
            Assert.IsFalse(manager.Tickets[ticketId].Open);
        }

        [TestMethod]
        public void CloseTicket_ShouldFail_WhenTicketDoesNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.CloseTicket(999));
        }

        [TestMethod]
        public void GetOpensTickets_ShouldReturnOnlyOpenTickets()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Mike", "123", "mike@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();

            var ticketId1 = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), "Ticket1");
            var ticketId2 = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(2), DateTime.Now.AddDays(4), "Ticket2");

            manager.CloseTicket(ticketId1);

            // Act
            var openTickets = manager.GetOpensTickets();

            // Assert
            Assert.AreEqual(1, openTickets.Count);
            Assert.AreEqual(ticketId2, openTickets.First().TicketId);
        }

        [TestMethod]
        public void GetOpensTickets5Days_ShouldReturnTicketsStartingSoon()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Sara", "123", "sara@example.com", TimeZones.Morning, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();

            var soonTicketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(3), DateTime.Now.AddDays(5), "Soon Ticket");
            var farTicketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(10), DateTime.Now.AddDays(12), "Far Ticket");

            // Act
            var soonTickets = manager.GetOpensTickets5days();

            // Assert
            Assert.IsTrue(soonTickets.Any(t => t.TicketId == soonTicketId));
            Assert.IsFalse(soonTickets.Any(t => t.TicketId == farTicketId));
        }

        [TestMethod]
        public void CreateEmployee_ShouldSucceed_WithValidData()
        {
            // Act
            var result = manager.CreateEmployee("Tom", "5551234", "tom@example.com", TimeZones.Morning, new(), new(), 2, 100, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Tom", result.Item1); // username
            Assert.AreEqual(8, result.Item2.Length); // password length
        }

        [TestMethod]
        public void CreateEmployee_ShouldFail_WithInvalidEmail()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.CreateEmployee("Tom", "5551234", "bad-email", TimeZones.Morning, new(), new(), 2, 100, true));
        }

        [TestMethod]
        public void EditEmployeeDetails_ShouldSucceed()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Eva", "999", "eva@example.com", TimeZones.Morning, new(), new(), 5, 80, false);
            var employee = manager.GetAllEmployees().First();

            // Act
            manager.EditEmail(employee.EmployeeId, "eva.new@example.com");
            manager.EditPhoneNumber(employee.EmployeeId, "123456789");
            manager.EditTimeZone(employee.EmployeeId, TimeZones.Evening);
            manager.EditYearOfExpr(employee.EmployeeId, 10);
            manager.EditJobPercentage(employee.EmployeeId, 90);

            // Assert
            var updatedEmployee = manager.GetEmployeeById(employee.EmployeeId);
            Assert.AreEqual("eva.new@example.com", updatedEmployee.Email.Address);
            Assert.AreEqual("123456789", updatedEmployee.PhoneNumber);
            Assert.AreEqual(TimeZones.Evening, updatedEmployee.TimeZone);
            Assert.AreEqual(10, updatedEmployee.YearsExperience);
            Assert.AreEqual(90, updatedEmployee.JobPercentage);
        }

        [TestMethod]
        public void EditEmployee_ShouldFail_WhenEmployeeNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.EditEmail(999, "noone@example.com"));
            Assert.ThrowsException<Exception>(() => manager.EditPhoneNumber(999, "0000000"));
            Assert.ThrowsException<Exception>(() => manager.EditTimeZone(999, TimeZones.Flexible));
            Assert.ThrowsException<Exception>(() => manager.EditYearOfExpr(999, 3));
            Assert.ThrowsException<Exception>(() => manager.EditJobPercentage(999, 50));
        }

        [TestMethod]
        public void AddSkillAndLanguage_ShouldSucceed()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Luke", "777", "luke@example.com", TimeZones.Noon, new(), new(), 1, 100, false);
            var employee = manager.GetAllEmployees().First();
            var skill = new Skill(Skills.Java, 2, 1);
            var language = new Language(Languages.English, 3);

            // Act
            manager.AddSkill(employee.EmployeeId, skill);
            manager.AddLanguage(employee.EmployeeId, language);

            // Assert
            var updatedEmployee = manager.GetEmployeeById(employee.EmployeeId);
            Assert.IsTrue(updatedEmployee.GetSkills().ContainsKey(skill.SkillId));
            Assert.IsTrue(updatedEmployee.GetLanguages().ContainsKey(language.LanguageID));
        }

        [TestMethod]
        public void AddSkillAndLanguage_ShouldFail_WhenAlreadyExists()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Anna", "777", "anna@example.com", TimeZones.Morning, new(), new(), 3, 100, false);
            var employee = manager.GetAllEmployees().First();
            var skill = new Skill(Skills.Python, 3, 2);
            var language = new Language(Languages.Hebrew, 2);
            manager.AddSkill(employee.EmployeeId, skill);
            manager.AddLanguage(employee.EmployeeId, language);

            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.AddSkill(employee.EmployeeId, skill));
            Assert.ThrowsException<Exception>(() => manager.AddLanguage(employee.EmployeeId, language));
        }

        [TestMethod]
        public void RemoveSkillAndLanguage_ShouldSucceed()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Mark", "888", "mark@example.com", TimeZones.Morning, new(), new(), 4, 100, true);
            var employee = manager.GetAllEmployees().First();
            var skill = new Skill(Skills.API, 2, 1);
            var language = new Language(Languages.English, 3);
            manager.AddSkill(employee.EmployeeId, skill);
            manager.AddLanguage(employee.EmployeeId, language);

            // Act
            manager.RemoveSkill(employee.EmployeeId, skill.SkillId);
            manager.RemoveLanguage(employee.EmployeeId, language.LanguageID);

            // Assert
            var updatedEmployee = manager.GetEmployeeById(employee.EmployeeId);
            Assert.IsFalse(updatedEmployee.GetSkills().ContainsKey(skill.SkillId));
            Assert.IsFalse(updatedEmployee.GetLanguages().ContainsKey(language.LanguageID));
        }

        [TestMethod]
        public void RemoveSkillAndLanguage_ShouldFail_WhenNotExists()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Leo", "123", "leo@example.com", TimeZones.Flexible, new(), new(), 2, 100, true);
            var employee = manager.GetAllEmployees().First();

            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.RemoveSkill(employee.EmployeeId, 999));
            Assert.ThrowsException<Exception>(() => manager.RemoveLanguage(employee.EmployeeId, 999));
        }

        [TestMethod]
        public void DeleteEmployee_ShouldSucceed()
        {
            // Arrange
            var createEmp = manager.CreateEmployee("Olivia", "999", "olivia@example.com", TimeZones.Noon, new(), new(), 5, 100, false);
            var employee = manager.GetAllEmployees().First();

            // Act
            manager.DeleteEmployee(employee.EmployeeId);

            // Assert
            Assert.IsNull(manager.GetEmployeeById(employee.EmployeeId));
        }

        [TestMethod]
        public void DeleteEmployee_ShouldFail_WhenEmployeeNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.DeleteEmployee(999));
        }

        [TestMethod]
        public void Login_ShouldSucceed_WhenManagerAndPasswordCorrect()
        {
            // Arrange
            var result = manager.CreateEmployee("AdminUser", "123456", "admin@example.com", TimeZones.Flexible, new(), new(), 10, 100, true);
            var createdEmployee = manager.GetAllEmployees().First();

            // Act
            var loginResult = manager.Login(createdEmployee.UserName, result.Item2); // result.Item2 is the generated password

            // Assert
            Assert.IsNotNull(loginResult);
            Assert.AreEqual(createdEmployee.EmployeeId, loginResult);
        }

        // [TestMethod]
        // public void Login_ShouldReturnNull_WhenEmployeeIsNotManager()
        // {
        //     // Arrange
        //     var result = manager.CreateEmployee("RegularUser", "123456", "user@example.com", TimeZones.Flexible, new(), new(), 5, 100, false);
        //     var createdEmployee = manager.GetAllEmployees().First();

        //     // Act
        //     var loginResult = manager.Login("RegularUser", result.Item2);

        //     // Assert
        //     Assert.IsNull(loginResult); // because Login returns null for regular employees
        // }

        [TestMethod]
        public void Login_ShouldFail_WhenWrongPassword()
        {
            // Arrange
            var result = manager.CreateEmployee("WrongPassUser", "123456", "wrongpass@example.com", TimeZones.Flexible, new(), new(), 5, 100, true);
            var createdEmployee = manager.GetAllEmployees().First();

            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.Login(createdEmployee.UserName, "WrongPassword123"));
        }

        [TestMethod]
        public void Login_ShouldFail_WhenUsernameNotFound()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.Login("nonexistentuser", "anyPassword"));
        }

        [TestMethod]
        public void AddTicket_ShouldCreateTicketSuccessfully()
        {
            // Arrange
            var result = manager.CreateEmployee("TicketUser", "123456", "ticketuser@example.com", TimeZones.Flexible, new(), new(), 2, 100, false);
            var employee = manager.GetAllEmployees().First();

            DateTime startDate = DateTime.Now.AddDays(2);
            DateTime endDate = startDate.AddDays(3);

            // Act
            int ticketId = manager.AddTicket(employee.EmployeeId, startDate, endDate, "Vacation Request");

            // Assert
            var openTickets = manager.GetOpensTickets();
            Assert.AreEqual(1, openTickets.Count);
            Assert.AreEqual(ticketId, openTickets.First().TicketId);
            Assert.AreEqual("Vacation Request", openTickets.First().Description);
        }

        [TestMethod]
        public void CloseTicket_ShouldMarkTicketAsClosed()
        {
            // Arrange
            var result = manager.CreateEmployee("CloseTicketUser", "123456", "closeticket@example.com", TimeZones.Flexible, new(), new(), 2, 100, false);
            var employee = manager.GetAllEmployees().First();
            int ticketId = manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), "Sick Leave");

            // Act
            manager.CloseTicket(ticketId);

            // Assert
            var openTickets = manager.GetOpensTickets();
            Assert.IsFalse(openTickets.Any(t => t.TicketId == ticketId));
        }

        [TestMethod]
        public void AddTicket_ShouldFail_WhenEmployeeNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<KeyNotFoundException>(() => manager.AddTicket(9999, DateTime.Now, DateTime.Now.AddDays(1), "Invalid Employee Ticket"));
        }

        [TestMethod]
        public void CloseTicket_ShouldFail_WhenTicketNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.CloseTicket(9999));
        }

        [TestMethod]
        public void GetOpensTickets5days_ShouldReturnOnlyTicketsWithin5Days()
        {
            // Arrange
            var result = manager.CreateEmployee("FiveDaysUser", "123456", "5days@example.com", TimeZones.Flexible, new(), new(), 2, 100, false);
            var employee = manager.GetAllEmployees().First();

            manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(2), DateTime.Now.AddDays(3), "Soon Ticket");
            manager.AddTicket(employee.EmployeeId, DateTime.Now.AddDays(10), DateTime.Now.AddDays(11), "Far Ticket");

            // Act
            var openTickets5days = manager.GetOpensTickets5days();

            // Assert
            Assert.AreEqual(1, openTickets5days.Count); // Only the "Soon Ticket" should be here
            Assert.AreEqual("Soon Ticket", openTickets5days.First().Description);
        }

        [TestMethod]
        public void CreateProject_ShouldCreateProjectSuccessfully()
        {
            // Act
            int projectId = manager.CreateProject("Project A", "Description A", DateTime.Now, 100, new());

            // Assert
            var project = manager.GetProjectById(projectId);
            Assert.IsNotNull(project);
            Assert.AreEqual("Project A", project.ProjectName);
        }

        [TestMethod]
        public void EditProjectName_ShouldUpdateProjectName()
        {
            // Arrange
            int projectId = manager.CreateProject("Old Name", "Desc", DateTime.Now, 100, new());

            // Act
            manager.EditProjectName(projectId, "New Name");

            // Assert
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual("New Name", updatedProject.ProjectName);
        }

        [TestMethod]
        public void EditProjectDescription_ShouldUpdateProjectDescription()
        {
            // Arrange
            int projectId = manager.CreateProject("Project", "Old Description", DateTime.Now, 100, new());

            // Act
            manager.EditProjectDescription(projectId, "New Description");

            // Assert
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual("New Description", updatedProject.Description);
        }

        [TestMethod]
        public void EditProjectDate_ShouldUpdateProjectDate()
        {
            // Arrange
            int projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());
            DateTime newDate = DateTime.Now.AddMonths(1);

            // Act
            manager.EditProjectDate(projectId, newDate);

            // Assert
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual(newDate.Date, updatedProject.Date.Date);
        }

        [TestMethod]
        public void EditProjectRequiredHours_ShouldUpdateRequiredHours()
        {
            // Arrange
            int projectId = manager.CreateProject("Project", "Desc", DateTime.Now, 100, new());

            // Act
            manager.EditProjectRequierdHours(projectId, 200);

            // Assert
            var updatedProject = manager.GetProjectById(projectId);
            Assert.AreEqual(200, updatedProject.RequiredHours);
        }

        [TestMethod]
        public void DeleteProject_ShouldRemoveProject()
        {
            // Arrange
            int projectId = manager.CreateProject("Project to delete", "Desc", DateTime.Now, 100, new());

            // Act
            manager.DeleteProject(projectId);

            // Assert
            var deletedProject = manager.GetProjectById(projectId);
            Assert.IsNull(deletedProject);
        }

        [TestMethod]
        public void CreateProject_ShouldFail_WithDuplicateProjectId()
        {
            // Arrange
            int projectId1 = manager.CreateProject("First Project", "Desc", DateTime.Now, 100, new());
            
            // Manually simulate inserting same ID again (normally prevented)

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => 
            {
                manager.Projects.Add(projectId1, new Project("Fake Project", projectId1, "Another", DateTime.Now, 50, new()));
                manager.CreateProject("Second Project", "Desc", DateTime.Now, 200, new());
            });
        }

        [TestMethod]
        public void EditProject_ShouldFail_WhenProjectDoesNotExist()
        {
            // Act & Assert
            Assert.ThrowsException<Exception>(() => manager.EditProjectName(9999, "Invalid Edit"));
            Assert.ThrowsException<Exception>(() => manager.EditProjectDescription(9999, "Invalid Desc"));
            Assert.ThrowsException<Exception>(() => manager.EditProjectDate(9999, DateTime.Now));
            Assert.ThrowsException<Exception>(() => manager.EditProjectRequierdHours(9999, 123));
        }

    }
}