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
    public class TicketTests
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
        public void Ticket_Constructor_ShouldInitializeFieldsCorrectly()
        {
            // Arrange
            int ticketId = 1;
            int employeeId = 10;
            string employeeName = "Alice";
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(5);
            string description = "Vacation request";

            // Act
            var ticket = new Ticket(ticketId, employeeId, employeeName, startDate, endDate, description);

            // Assert
            Assert.AreEqual(ticketId, ticket.TicketId);
            Assert.AreEqual(employeeId, ticket.EmployeeId);
            Assert.AreEqual(employeeName, ticket.EmployeeName);
            Assert.AreEqual(startDate, ticket.StartDate);
            Assert.AreEqual(endDate, ticket.EndDate);
            Assert.AreEqual(description, ticket.Description);
            Assert.IsTrue(ticket.Open);
        }

        [TestMethod]
        public void Ticket_FromDTO_ShouldInitializeCorrectly()
        {
            // Arrange
            var dto = new TicketDTO
            {
                TicketId = 2,
                EmployeeId = 20,
                EmployeeName = "Bob",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(7),
                Description = "Sick leave",
                Open = true
            };

            // Act
            var ticket = new Ticket(dto);

            // Assert
            Assert.AreEqual(dto.TicketId, ticket.TicketId);
            Assert.AreEqual(dto.EmployeeId, ticket.EmployeeId);
            Assert.AreEqual(dto.EmployeeName, ticket.EmployeeName);
            Assert.AreEqual(dto.StartDate, ticket.StartDate);
            Assert.AreEqual(dto.EndDate, ticket.EndDate);
            Assert.AreEqual(dto.Description, ticket.Description);
            Assert.IsTrue(ticket.Open);
        }

        [TestMethod]
        public void Ticket_CloseTicket_ShouldSetOpenToFalse()
        {
            // Arrange
            var ticket = new Ticket(3, 30, "Charlie", DateTime.Today, DateTime.Today.AddDays(3), "Business trip");

            // Act
            ticket.CloseTicket();

            // Assert
            Assert.IsFalse(ticket.Open);
        }

    }
}