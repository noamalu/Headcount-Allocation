using System.Collections.Concurrent;
using HeadcountAllocation.Domain;
using HeadcountAllocation.Services;
using static HeadcountAllocation.Domain.Enums;

namespace AT.Tests
{
    public class Proxy
    {
        private readonly HeadCountService _headCountService;

        public Proxy()
        {
            _headCountService = HeadCountService.GetInstance();
        }

        public void Dispose()
        {
            _headCountService.Dispose();
        }

        public bool CreateProject(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles)
        {
            var response = _headCountService.CreateProject(projectName, description, date, requiredHours, roles);
            return !response.ErrorOccured;
        }

        public bool AddRoleToProject(string roleName, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string description, DateTime startDate)
        {
            var response = _headCountService.AddRoleToProject(roleName, projectId, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage, description, startDate);
            return !response.ErrorOccured;
        }

        public Role AddRoleToProject_role(string roleName, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string description, DateTime startDate)
        {
            var response = _headCountService.AddRoleToProject(roleName, projectId, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage, description, startDate);
            return response.Value;
        }

        public List<Project> GetAllProjects()
        {
            var response = _headCountService.GetAllProjects();
            return response.Value;
        }

        public bool EditProjectName(int projectId, string projectName)
        {
            var response = _headCountService.EditProjectName(projectId, projectName);
            return !response.ErrorOccured;
        }

        public bool EditProjectDescription(int projectId, string projectDescription)
        {
            var response = _headCountService.EditProjectDescription(projectId, projectDescription);
            return !response.ErrorOccured;
        }

        public bool EditProjectDate(int projectId, DateTime date)
        {
            var response = _headCountService.EditProjectDate(projectId, date);
            return !response.ErrorOccured;
        }

        public bool EditProjectRequierdHours(int projectId, int requiredHours)
        {
            var response = _headCountService.EditProjectRequierdHours(projectId, requiredHours);
            return !response.ErrorOccured;
        }

        public bool DeleteProject(int projectId)
        {
            var response = _headCountService.DeleteProject(projectId);
            return !response.ErrorOccured;
        }

        public bool RemoveRole(int projectId, int roleId)
        {
            var response = _headCountService.RemoveRole(projectId, roleId);
            return !response.ErrorOccured;
        }

        public Dictionary<int, Role> GetAllRolesByProject(int projectId)
        {
            var response = _headCountService.GetAllRolesByProject(projectId);
            return response.Value;
        }

        public bool AssignEmployeeToRole(int employeeId, Role role)
        {
            var response = _headCountService.AssignEmployeeToRole(employeeId, role);
            return !response.ErrorOccured;
        }

        public bool AddEmployee(string name, string phoneNumber, string email,
        TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages,
        ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, bool isManager)
        {
            var response = _headCountService.AddEmployee(name, phoneNumber, email, timezone, foreignLanguages, skills, yearsExperience, jobPercentage, isManager);
            return !response.ErrorOccured;
        }

        public bool AddEmployee(string name, string password, string phoneNumber, string email,
        TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages,
        ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, bool isManager)
        {
            var response = _headCountService.AddEmployee(name, password, phoneNumber, email, timezone, foreignLanguages, skills, yearsExperience, jobPercentage, isManager);
            return !response.ErrorOccured;
        }

        public List<Employee> GetAllEmployees()
        {
            var response = _headCountService.GetAllEmployees();
            return response.Value;
        }

        public bool GetAllRolesByEmployee(int employeeId)
        {
            var response = _headCountService.GetAllRolesByEmployee(employeeId);
            return !response.ErrorOccured;
        }

        public Dictionary<Employee, double> EmployeesToAssign(Role role)
        {
            var response = _headCountService.EmployeesToAssign(role);
            return response.Value;
        }

        public bool DeleteEmployee(int employeeId)
        {
            var response = _headCountService.DeleteEmployee(employeeId);
            return !response.ErrorOccured;
        }

        public bool Login(string userName, string password)
        {
            var response = _headCountService.Login(userName, password);
            return !response.ErrorOccured;
        }

        public bool EditEmail(int userId, string newEmail)
        {
            var response = _headCountService.EditEmail(userId, newEmail);
            return !response.ErrorOccured;
        }

        public bool EditPhoneNumber(int userId, string newPhoneNumber)
        {
            var response = _headCountService.EditPhoneNumber(userId, newPhoneNumber);
            return !response.ErrorOccured;
        }

        public bool EditTimeZone(int userId, TimeZones newTimeZone)
        {
            var response = _headCountService.EditTimeZone(userId, newTimeZone);
            return !response.ErrorOccured;
        }

        public bool EditYearOfExpr(int userId, int newyearOfExpr)
        {
            var response = _headCountService.EditYearOfExpr(userId, newyearOfExpr);
            return !response.ErrorOccured;
        }

        public bool EditJobPercentage(int userId, double newJobPercentage)
        {
            var response = _headCountService.EditJobPercentage(userId, newJobPercentage);
            return !response.ErrorOccured;
        }

        public bool AddTicket(int employeeId, DateTime startDate, DateTime endDate, string description, Reason reason)
        {
            var response = _headCountService.AddTicket(employeeId, startDate, endDate, description, reason);
            return !response.ErrorOccured;
        }

        public bool CloseTicket(int ticketId)
        {
            var response = _headCountService.CloseTicket(ticketId);
            return !response.ErrorOccured;
        }

        public List<Ticket> GetOpensTickets()
        {
            var response = _headCountService.GetOpensTickets();
            return response.Value;
        }

        public List<Ticket> GetOpensTickets5Days()
        {
            var response = _headCountService.GetOpensTickets5days();
            return response.Value;
        }



    }
}
