using System.Collections.Concurrent;
using System.IO.Pipelines;
using HeadcountAllocation.Domain;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Services
{

    public class HeadCountService : IHeadCountService
    {
        private static HeadCountService _headCountService = null;

        private ManagerFacade _managerFacade;

        public HeadCountService()
        {
            _managerFacade = ManagerFacade.GetInstance();
        }

        public static HeadCountService GetInstance()
        {
            if (_headCountService == null)
            {
                _headCountService = new HeadCountService();
            }
            return _headCountService;
        }

        public void Dispose()
        {
            ManagerFacade.Dispose();
            _headCountService = null;
        }

        public Response<List<Project>> GetAllProjects()
        {
            try
            {
                return Response<List<Project>>.FromValue(_managerFacade.GetAllProjects());
            }
            catch (Exception e)
            {
                return Response<List<Project>>.FromError(e.Message);
            }
        }

        public Response<int> CreateProject(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles)
        {
            try
            {
                return Response<int>.FromValue(_managerFacade.CreateProject(projectName, description, date, requiredHours, roles));
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }

        public Response EditProjectName(int projectId, string projectName)
        {
            try
            {
                _managerFacade.EditProjectName(projectId, projectName);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response EditProjectDescription(int projectId, string ProjectDescription)
        {
            try
            {
                _managerFacade.EditProjectDescription(projectId, ProjectDescription);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        public Response EditProjectDate(int projectId, DateTime date)
        {
            try
            {
                _managerFacade.EditProjectDate(projectId, date);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response EditProjectRequierdHours(int projectId, int requiredHours)
        {
            try
            {
                _managerFacade.EditProjectRequierdHours(projectId, requiredHours);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response DeleteProject(int projectId)
        {
            try
            {
                _managerFacade.DeleteProject(projectId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        public Response<Role> AddRoleToProject(string roleName, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string description, DateTime startDate)
        {
            try
            {
                Console.WriteLine("got to manager facade");
                var role = _managerFacade.AddRoleToProject(roleName, projectId, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage, description, startDate);
                return Response<Role>.FromValue(role);
            }
            catch (Exception e)
            {
                return Response<Role>.FromError(e.Message);
            }
        }

        public Response RemoveRole(int projectId, int roleId)
        {
            try
            {
                _managerFacade.RemoveRole(projectId, roleId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Dictionary<int, Role>> GetAllRolesByProject(int projectId)
        {
            try
            {
                Dictionary<int, Role> roles = _managerFacade.GetAllRolesByProject(projectId);
                return Response<Dictionary<int, Role>>.FromValue(roles);
            }
            catch (Exception e)
            {
                return Response<Dictionary<int, Role>>.FromError(e.Message);
            }
        }

        public Response AssignEmployeeToRole(int employeeId, Role role)
        {
            try
            {
                _managerFacade.AssignEmployeeToRole(employeeId, role);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<List<Role>> GetAllRolesByEmployee(int employeeId)
        {
            try
            {
                List<Role> roles = _managerFacade.GetAllRolesByEmployee(employeeId);
                return Response<List<Role>>.FromValue(roles);
            }
            catch (Exception e)
            {
                return Response<List<Role>>.FromError(e.Message);
            }
        }


        public Response<Dictionary<Employee, double>> EmployeesToAssign(Role role)
        {
            try
            {
                Dictionary<Employee, double> employees = _managerFacade.EmployeesToAssign(role);
                return Response<Dictionary<Employee, double>>.FromValue(employees);
            }
            catch (Exception e)
            {
                return Response<Dictionary<Employee, double>>.FromError(e.Message);
            }
        }

        public Response<List<Employee>> GetAllEmployees()
        {
            try
            {
                return Response<List<Employee>>.FromValue(_managerFacade.GetAllEmployees());
            }
            catch (Exception e)
            {
                return Response<List<Employee>>.FromError(e.Message);
            }
        }

        public Response<Employee> GetEmployeeById(int employeeId)
        {
            try
            {
                return Response<Employee>.FromValue(_managerFacade.GetEmployeeById(employeeId));
            }
            catch (Exception e)
            {
                return Response<Employee>.FromError(e.Message);
            }
        }

        public Response<Tuple<string, string>> AddEmployee(string name, string phoneNumber, string email,
        TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages,
        ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, bool isManager)
        {
            try
            {
                return Response<Tuple<string, string>>.FromValue(_managerFacade.CreateEmployee(name, phoneNumber, email, timezone, foreignLanguages, skills, yearsExperience, jobPercentage, isManager));
            }
            catch (Exception e)
            {
                return Response<Tuple<string, string>>.FromError(e.Message);
            }
        }

        public Response<int> AddEmployee(string name, string password, string phoneNumber, string email,
            TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages,
            ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, bool isManager)
        {
            try
            {
                return Response<int>.FromValue(_managerFacade.CreateEmployee(name, password, phoneNumber, email, timezone, foreignLanguages, skills, yearsExperience, jobPercentage, isManager));
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }

        public Response DeleteEmployee(int employeeId)
        {
            try
            {
                _managerFacade.DeleteEmployee(employeeId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<int?> Login(string userName, string password)
        {
            try
            {
                return Response<int?>.FromValue(_managerFacade.Login(userName, password));
            }
            catch (Exception e)
            {
                return Response<int?>.FromError(e.Message);
            }
        }

        public Response EditEmail(int userId, string newEmail)
        {
            try
            {
                _managerFacade.EditEmail(userId, newEmail);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response EditPhoneNumber(int userId, string newPhoneNumber)
        {
            try
            {
                _managerFacade.EditPhoneNumber(userId, newPhoneNumber);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response EditTimeZone(int userId, TimeZones newTimeZone)
        {
            try
            {
                _managerFacade.EditTimeZone(userId, newTimeZone);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response EditYearOfExpr(int userId, int newyearOfExpr)
        {
            try
            {
                _managerFacade.EditYearOfExpr(userId, newyearOfExpr);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response EditJobPercentage(int userId, double newJobPercentage)
        {
            try
            {
                _managerFacade.EditJobPercentage(userId, newJobPercentage);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response AddSkill(int userId, Skill newSkill)
        {
            try
            {
                _managerFacade.AddSkill(userId, newSkill);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response RemoveSkill(int userId, int SkillId)
        {
            try
            {
                _managerFacade.RemoveSkill(userId, SkillId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response AddLanguage(int userId, Language newLanguage)
        {
            try
            {
                _managerFacade.AddLanguage(userId, newLanguage);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response RemoveLanguage(int userId, int languageID)
        {
            try
            {
                _managerFacade.RemoveLanguage(userId, languageID);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<int> AddTicket(int employeeId, DateTime startDate, DateTime endDate, string description, Reason reason)
        {
            try
            {
                return Response<int>.FromValue(_managerFacade.AddTicket(employeeId, startDate, endDate, description, reason));
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }

        public Response CloseTicket(int ticketId)
        {
            try
            {
                _managerFacade.CloseTicket(ticketId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<List<Ticket>> GetOpensTickets()
        {
            try
            {
                return Response<List<Ticket>>.FromValue(_managerFacade.GetOpensTickets());
            }
            catch (Exception e)
            {
                return Response<List<Ticket>>.FromError(e.Message);
            }
        }

        public Response<List<Ticket>> GetTickets()
        {
            try
            {
                return Response<List<Ticket>>.FromValue(_managerFacade.GetTickets());
            }
            catch (Exception e)
            {
                return Response<List<Ticket>>.FromError(e.Message);
            }
        }


        public Response<List<Ticket>> GetOpensTickets5days()
        {
            try
            {
                return Response<List<Ticket>>.FromValue(_managerFacade.GetOpensTickets5days());
            }
            catch (Exception e)
            {
                return Response<List<Ticket>>.FromError(e.Message);
            }
        }

        public Response UpdateEmployee(Employee employee)
        {
            try
            {
                _managerFacade.UpdateEmployee(employee);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateRole(int projectId, int roleId, Role role)
        {
            try
            {
                _managerFacade.UpdateRole(projectId, roleId, role);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response DeleteRole(int projectId, int roleId)
        {
            try
            {
                _managerFacade.DeleteRole(projectId, roleId);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response EditTicket(int employeeId, Ticket ticket)
        {
            try
            {
                _managerFacade.EditTicket(employeeId, ticket);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }


        public Response<Dictionary<string, List<Employee>>> GetEmployeesJobPre()
        {
            try
            {
                return Response<Dictionary<string, List<Employee>>>.FromValue(_managerFacade.GetEmployeesJobPre());
            }
            catch (Exception e)
            {
                return Response<Dictionary<string, List<Employee>>>.FromError(e.Message);
            }
        }

        public Response<List<Project>> GetProjectsThatEndThisMonth()
        {
            try
            {
                return Response<List<Project>>.FromValue(_managerFacade.GetProjectsThatEndThisMonth());
            }
            catch (Exception e)
            {
                return Response<List<Project>>.FromError(e.Message);
            }
        }

        public Response<Dictionary<Project, int>> GetNumEmployeesInProject()
        {
            try
            {
                return Response<Dictionary<Project, int>>.FromValue(_managerFacade.GetNumEmployeesInProject());
            }
            catch (Exception e)
            {
                return Response<Dictionary<Project, int>>.FromError(e.Message);
            }
        }

        public Response<List<Employee>> GetEmployeesThatInVacationThisMonth()
        {
            try
            {
                return Response<List<Employee>>.FromValue(_managerFacade.GetEmployeesThatInVacationThisMonth());
            }
            catch (Exception e)
            {
                return Response<List<Employee>>.FromError(e.Message);
            }
        }

        public Response<Dictionary<Project, double>> GetProjectHourRatio()
        {
            try
            {
                return Response<Dictionary<Project, double>>.FromValue(_managerFacade.GetProjectHourRatio());
            }
            catch (Exception e)
            {
                return Response<Dictionary<Project, double>>.FromError(e.Message);
            }
        }

        public Response<Dictionary<Enums.Reasons, List<Employee>>> GetEmployeesThatInVacationThisMonthAndReason()
        {
            try
            {
                return Response<Dictionary<Enums.Reasons, List<Employee>>>.FromValue(_managerFacade.GetEmployeesThatInVacationThisMonthAndReason());
            }
            catch (Exception e)
            {
                return Response<Dictionary<Enums.Reasons, List<Employee>>>.FromError(e.Message);
            }
        }

        public void DeleteTicket(int ticketId)
        {
            try
            {
                _managerFacade.DeleteTicket(ticketId);
            }
            catch (Exception e)
            {
                throw new Exception($"Error deleting ticket with ID {ticketId}: {e.Message}");
            }
        }
    }
}

