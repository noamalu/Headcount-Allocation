using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;
using System.Collections.Concurrent;
using HeadcountAllocation.DAL;

namespace AT.Tests
{
    [TestClass]
    public class ServiceAT
    {
        private Proxy _proxy;

        [TestInitialize]
        public void Setup()
        {
            _proxy = new Proxy();
            DBcontext.SetTestDatabase(); // Connect to test DB
            var context = DBcontext.GetInstance();
            context.ClearDatabase();     // Clean database before each test
            context.SeedStaticTables();
        }

        [TestCleanup]
        public void CleanUp()
        {
            DBcontext.GetInstance().Dispose();
            _proxy.Dispose();
        }

        [TestMethod]
        public void CreateProject_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            var result = _proxy.CreateProject("Project A", "Description for Project A", DateTime.Now, 100, roles);
            Assert.IsTrue(result, "Failed to create project.");
        }

        [TestMethod]
        public void CreateProject_Fail_InvalidData()
        {
            var result = _proxy.CreateProject("name", "Description for Project A", DateTime.Now, 100, null); // Invalid project name
            Assert.IsFalse(result, "Project creation should have failed due to invalid data.");
        }

        [TestMethod]
        public void AddRoleToProject_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var result = _proxy.AddRoleToProject("Developer", 0, TimeZones.Morning, new ConcurrentDictionary<int, Language>(), new ConcurrentDictionary<int, Skill>(), 3, 50, "Develop the system", DateTime.Now);
            Assert.IsTrue(result, "Failed to add role to project.");
        }

        [TestMethod]
        public void GetAllProjects_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var projects = _proxy.GetAllProjects();
            Assert.IsNotNull(projects, "Projects should not be null.");
            Assert.IsTrue(projects.Count > 0, "There should be at least one project.");
        }

        [TestMethod]
        public void EditProjectName_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var res = _proxy.EditProjectName(0, "new_name");
            Assert.IsTrue(res, "The edit should succeed");
            Assert.AreEqual("new_name", _proxy.GetAllProjects().First().ProjectName);
        }

        [TestMethod]
        public void EditProjectDescription_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var res = _proxy.EditProjectDescription(0, "new_desc");
            Assert.IsTrue(res, "The edit should succeed");
            Assert.AreEqual("new_desc", _proxy.GetAllProjects().First().Description);
        }

        [TestMethod]
        public void EditProjectDate_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var res = _proxy.EditProjectDate(0, DateTime.Now.AddDays(5));
            Assert.IsTrue(res, "The edit should succeed");
            Assert.AreEqual(DateTime.Now.AddDays(5).Date, _proxy.GetAllProjects().First().Date.Date);
        }


        [TestMethod]
        public void EditProjectRequierdHours_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var res = _proxy.EditProjectRequierdHours(0, 5);
            Assert.IsTrue(res, "The edit should succeed");
            Assert.AreEqual(5, _proxy.GetAllProjects().First().RequiredHours);
        }

        [TestMethod]
        public void DeleteProject_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var res = _proxy.DeleteProject(0);
            Assert.IsTrue(res, "The edit should succeed");
            Assert.AreEqual(0, _proxy.GetAllProjects().Count);
        }

        [TestMethod]
        public void DeleteRole_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            _proxy.AddRoleToProject("Developer", 0, TimeZones.Morning, new ConcurrentDictionary<int, Language>(), new ConcurrentDictionary<int, Skill>(), 3, 50, "Develop the system", DateTime.Now);
            var res = _proxy.RemoveRole(0, 0);
            Assert.IsTrue(res, "The edit should succeed");
            Assert.AreEqual(0, _proxy.GetAllProjects().First().GetAllRolesByProject().Count);
        }

        [TestMethod]
        public void GetAllRolesByProject_Success()
        {
            var roles = new Dictionary<int, Role>(); // Set up roles as required
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            _proxy.AddRoleToProject("Developer", 0, TimeZones.Morning, new ConcurrentDictionary<int, Language>(), new ConcurrentDictionary<int, Skill>(), 3, 50, "Develop the system", DateTime.Now);
            var res = _proxy.GetAllRolesByProject(0);
            Assert.AreEqual(1, res.Count);
        }

        [TestMethod]
        public void AssignEmployeeToRole_Success()
        {
            var roles = new Dictionary<int, Role>();
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, new ConcurrentDictionary<int, Language>(),
            new ConcurrentDictionary<int, Skill>(), 5, 1, false);
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var role = _proxy.AddRoleToProject_role("Developer", 0, TimeZones.Morning, new ConcurrentDictionary<int, Language>(), new ConcurrentDictionary<int, Skill>(), 3, 50, "Develop the system", DateTime.Now);
            var res = _proxy.AssignEmployeeToRole(0, role);
            Assert.IsTrue(res, "The assign should succeed");
            Assert.IsTrue(_proxy.GetAllEmployees().First().Roles.ContainsKey(0));
        }

        [TestMethod]
        public void GetAllRolesByEmployee_Success()
        {
            var roles = new Dictionary<int, Role>();
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, new ConcurrentDictionary<int, Language>(),
            new ConcurrentDictionary<int, Skill>(), 5, 1, false);
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var role = _proxy.AddRoleToProject_role("Developer", 0, TimeZones.Morning, new ConcurrentDictionary<int, Language>(), new ConcurrentDictionary<int, Skill>(), 3, 50, "Develop the system", DateTime.Now);
            _proxy.AssignEmployeeToRole(0, role);
            var res = _proxy.GetAllRolesByEmployee(0);
            Assert.IsTrue(res, "The assign should succeed");
            Assert.IsTrue(_proxy.GetAllEmployees().First().Roles.ContainsKey(0));
        }

        [TestMethod]
        public void EmployeesToAssign_Success()
        {
            var roles = new Dictionary<int, Role>();
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(0, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(0, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            _proxy.CreateProject("name", "desc", DateTime.Now, 10, roles);
            var role = _proxy.AddRoleToProject_role("Developer", 0, TimeZones.Morning, languages, skills, 3, 0.5, "Develop the system", DateTime.Now);
            var res = _proxy.EmployeesToAssign(role);
            Assert.AreEqual(res.Count, 1);
            Assert.AreEqual(res.First().Value, 1);
        }

        [TestMethod]
        public void DeleteEmployee_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.DeleteEmployee(0);
            Assert.IsTrue(res, "should be able to delete employee");
            Assert.AreEqual(_proxy.GetAllEmployees().Count, 0);
        }

        [TestMethod]
        public void Login_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "pass", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.Login("emp", "pass");
            Assert.IsTrue(res, "should be able to login employee");
        }

        [TestMethod]
        public void EditEmail_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.EditEmail(0, "newMail@mail.com");
            Assert.IsTrue(res, "should be able to edit mail");
            Assert.AreEqual(_proxy.GetAllEmployees().First().Email.Address, "newMail@mail.com");
        }

        [TestMethod]
        public void EditPhoneNumber_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.EditPhoneNumber(0, "0000000000");
            Assert.IsTrue(res, "should be able to edit number");
            Assert.AreEqual(_proxy.GetAllEmployees().First().PhoneNumber, "0000000000");
        }

        [TestMethod]
        public void EditTimeZone_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.EditTimeZone(0, TimeZones.Evening);
            Assert.IsTrue(res, "should be able to edit timezone");
            Assert.AreEqual(_proxy.GetAllEmployees().First().TimeZone, TimeZones.Evening);
        }

        [TestMethod]
        public void EditYearOfExpr_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.EditYearOfExpr(0, 10);
            Assert.IsTrue(res, "should be able to edit years");
            Assert.AreEqual(_proxy.GetAllEmployees().First().YearsExperience, 10);
        }

        [TestMethod]
        public void EditJobPercentage_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.EditJobPercentage(0, 0.5);
            Assert.IsTrue(res, "should be able to edit job percantage");
            Assert.AreEqual(_proxy.GetAllEmployees().First().JobPercentage, 0.5);
        }

        [TestMethod]
        public void AddTicket_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            var res = _proxy.AddTicket(0, DateTime.Now, DateTime.Now.AddDays(10), "vacation", new Reason(Reasons.LongVacation));
            Assert.IsTrue(res, "should be able to add ticket");
            Assert.AreEqual(_proxy.GetOpensTickets().Count, 1);
        }

        [TestMethod]
        public void CloseTicket_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            _proxy.AddTicket(0, DateTime.Now, DateTime.Now.AddDays(10), "vacation", new Reason(Reasons.LongVacation));
            var res = _proxy.CloseTicket(0);
            Assert.IsTrue(res, "should be able to close ticket");
            Assert.AreEqual(_proxy.GetOpensTickets().Count, 0);
        }

        [TestMethod]
        public void GetOpensTickets5Days_Success()
        {
            var languages = new ConcurrentDictionary<int, Language>();
            languages.TryAdd(1, new Language(Languages.English, 1));
            var skills = new ConcurrentDictionary<int, Skill>();
            skills.TryAdd(1, new Skill(Skills.Python, 1, 1));
            _proxy.AddEmployee("emp", "0504459876", "mail@mail.com", TimeZones.Morning, languages,
            skills, 5, 1, false);
            _proxy.AddTicket(0, DateTime.Now.AddDays(10), DateTime.Now.AddDays(20), "vacation", new Reason(Reasons.LongVacation));
            _proxy.AddTicket(0, DateTime.Now, DateTime.Now.AddDays(2), "army", new Reason(Reasons.ReserveDuty));
            var res = _proxy.GetOpensTickets5Days();
            Assert.AreEqual(res.Count, 1);
            Assert.AreEqual(res.First().Reason.ReasonType, Reasons.ReserveDuty);
        }


        


        
    }
}
