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
    public class ParallelTests
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
        [TestCategory("Parallel")]
        public async Task Parallel_CreateProjects_ShouldCreateAllSuccessfully()
        {
            // Arrange
            int numberOfProjects = 10;
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < numberOfProjects; i++)
            {
                int projectIndex = i;
                tasks.Add(Task.Run(() =>
                {
                    manager.CreateProject(
                        $"Project {projectIndex}",
                        $"Description {projectIndex}",
                        DateTime.Now.AddDays(projectIndex),
                        100 + projectIndex,
                        new());
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            var allProjects = manager.GetAllProjects();
            Assert.IsTrue(allProjects.Count == numberOfProjects);
        }

        [TestMethod]
        [TestCategory("Parallel")]
        public async Task Parallel_CreateEmployees_ShouldCreateAllSuccessfully()
        {
            // Arrange
            int numberOfEmployees = 10;
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < numberOfEmployees; i++)
            {
                int employeeIndex = i;
                tasks.Add(Task.Run(() =>
                {
                    manager.CreateEmployee(
                        $"Employee {employeeIndex}",
                        "0500000000",
                        $"employee{employeeIndex}@test.com",
                        TimeZones.Flexible,
                        new(),
                        new(),
                        1,
                        100,
                        false);
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            var allEmployees = manager.GetAllEmployees();
            Assert.IsTrue(allEmployees.Count >= numberOfEmployees);
        }

        [TestMethod]
        [TestCategory("Parallel")]
        public async Task Parallel_AddTickets_ShouldAddAllSuccessfully()
        {
            // Arrange
            var employee = manager.CreateEmployee(
                "Ticket Employee",
                "0500000000",
                "ticketemployee@test.com",
                TimeZones.Noon,
                new(),
                new(),
                2,
                100,
                false);

            int employeeId = manager.GetAllEmployees().First().EmployeeId;
            int numberOfTickets = 10;
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < numberOfTickets; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    manager.AddTicket(
                        employeeId,
                        DateTime.Now.AddDays(1),
                        DateTime.Now.AddDays(5),
                        $"Ticket {i}", new Reason(Reasons.Other), false);
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            var openTickets = manager.GetOpensTickets();
            Assert.IsTrue(openTickets.Count >= numberOfTickets);
        }

        [TestMethod]
        [TestCategory("Parallel")]
        public async Task Parallel_AssignRoles_ShouldAssignAllSuccessfully()
        {
            // Arrange
            int projectId = manager.CreateProject("Parallel Role Project", "Desc", DateTime.Now, 100, new());
            
            // Create 10 Roles
            var roles = new List<Role>();
            for (int i = 0; i < 10; i++)
            {
                var role = manager.AddRoleToProject(
                    $"Role {i}",
                    projectId,
                    TimeZones.Morning,
                    new(),
                    new(),
                    1,
                    100,
                    "Developer",
                    DateTime.Now);
                roles.Add(role);
            }

            // Create 10 Employees
            var employees = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                manager.CreateEmployee(
                    $"RoleEmployee {i}",
                    "0500000000",
                    $"roleemployee{i}@test.com",
                    TimeZones.Flexible,
                    new(),
                    new(),
                    1,
                    100,
                    false);
            }
            employees = manager.GetAllEmployees().Select(e => e.EmployeeId).ToList();

            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < 10; i++)
            {
                int index = i;
                tasks.Add(Task.Run(() =>
                {
                    manager.AssignEmployeeToRole(employees[index], roles[index]);
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            var updatedRoles = manager.GetAllRolesByProject(projectId);
            foreach (var role in updatedRoles.Values)
            {
                Assert.IsNotNull(role.EmployeeId);
            }
        }

        

    }
}