using System.Collections.Concurrent;
using System.Net.Mail;
using System.Text;
using HeadcountAllocation.DAL.Repositories;
using Microsoft.AspNetCore.Routing;
using static HeadcountAllocation.Domain.Enums;

namespace HeadcountAllocation.Domain
{

    public class ManagerFacade
    {

        private static ManagerFacade managerFacade = null;

        public Dictionary<int, Project> Projects { get; set; } = new();

        public Dictionary<int, Employee> Employees { get; set; } = new();
        public Dictionary<int, Ticket> Tickets { get; set; } = new();

        private readonly EmployeeRepo employeeRepo;

        private readonly ProjectRepo projectRepo;
        private readonly TicketRepo ticketRepo;
        public int projectCount = 0;

        public int employeeCount = 0;

        public int ticketCount = 0;
        public int roleCounter = 0;
        private static readonly object _ticketLock = new object();

        public ManagerFacade()
        {
            projectRepo = ProjectRepo.GetInstance();
            employeeRepo = EmployeeRepo.GetInstance();
            TicketReasonsRepo.GetInstance().GetAll();
            ticketRepo = TicketRepo.GetInstance();
            List<Ticket> TicketsList = ticketRepo.getAll();
            foreach (var ticket in TicketsList)
            {
                Tickets[ticket.TicketId] = ticket;
            }
            List<Employee> EmployeeList = employeeRepo.GetAll();
            foreach (var employee in EmployeeList)
            {
                Employees[employee.EmployeeId] = employee;
            }
            List<Project> ProjectList = projectRepo.GetAll();
            foreach (var project in ProjectList)
            {
                Projects[project.ProjectId] = project;
            }
            ticketCount = Tickets.Count;
            employeeCount = Employees.Count;
            projectCount = Projects.Count;
            roleCounter = Projects?.Values?.SelectMany(p => p?.Roles?.Values?.Select(role => role?.RoleId))?.Max() ?? 0;

        }

        public static ManagerFacade GetInstance()
        {
            if (managerFacade == null)
            {
                managerFacade = new ManagerFacade();
            }
            return managerFacade;
        }

        public static void Dispose()
        {
            EmployeeRepo.Dispose();
            ProjectRepo.Dispose();
            EmployeeLanguagesRepo.Dispose();
            EmployeeSkillsRepo.Dispose();
            RoleRepo.Dispose();
            RoleLanguagesRepo.Dispose();
            RoleSkillsRepo.Dispose();
            TicketRepo.Dispose();
            TicketReasonsRepo.Dispose();
            managerFacade = null;
        }

        public int CreateProject(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles)
        {
            if (projectName == null)
            {
                throw new Exception("Null projectName");
            }
            Project project = new Project(projectName, projectCount++, description, date, requiredHours, roles);
            Projects.Add(project.ProjectId, project);
            try
            {
                projectRepo.Add(project);
                return project.ProjectId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void EditProjectName(int projectId, string projectName)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null)
            {
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectName(projectName);
            projectRepo.Update(Projects[projectId]);
        }

        public void EditProjectDescription(int projectId, string ProjectDescription)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null)
            {
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectDescription(ProjectDescription);
            projectRepo.Update(Projects[projectId]);
        }


        public void EditProjectDate(int projectId, DateTime date)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null)
            {
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectDate(date);
            projectRepo.Update(Projects[projectId]);
        }

        public void EditProjectRequierdHours(int projectId, int requiredHours)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null)
            {
                throw new Exception($"No such project {projectId}");
            }
            Projects[projectId].EditProjectRequierdHours(requiredHours);
            projectRepo.Update(Projects[projectId]);
        }

        public void DeleteProject(int projectId)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            if (projectRepo.GetById(projectId) == null)
            {
                throw new Exception($"No such project {projectId}");
            }
            //remove roles of project from employees
            Dictionary<int, Role> projectRoles = Projects[projectId].Roles;
            foreach (var role in projectRoles)
            {
                int? employeeId = role.Value.EmployeeId;
                if (employeeId != null)
                {
                    Employees[(int)employeeId].Roles.Remove(role.Key);
                }
            }
            Projects.Remove(projectId);
            try
            {
                projectRepo.Delete(projectId);
            }
            catch (Exception e)
            {
                throw new Exception($"No such project {projectId} " + $"{e}");
            }
        }


        public Role AddRoleToProject(string roleName, int projectId, TimeZones timeZone, ConcurrentDictionary<int, Language> foreignLanguages,
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string description, DateTime startDate)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            return Projects[projectId].AddRoleToProject(roleName, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage, description, roleCounter++, startDate);
        }

        public void RemoveRole(int projectId, int roleId)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            int? employeeId = Projects[projectId].Roles[roleId].EmployeeId;
            if (employeeId != null)
            {
                Employees[(int)employeeId].Roles.Remove(roleId);
            }
            Projects[projectId].RemoveRole(roleId);
        }

        public Dictionary<int, Role> GetAllRolesByProject(int projectId)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            return Projects[projectId].GetAllRolesByProject();
        }

        public void AssignEmployeeToRole(int employeeId, Role role)
        {
            if (!Employees.ContainsKey(employeeId))
            {
                throw new Exception($"No such employee {employeeId}");
            }
            bool RoleExists = false;
            foreach (Project project in Projects.Values)
            {
                if (project.Roles.ContainsKey(role.RoleId))
                {
                    RoleExists = true;
                }
            }
            if (!RoleExists)
            {
                throw new Exception($"No such role {role.RoleId}");
            }
            role.EmployeeId = employeeId;
            Employees[employeeId].AssignEmployeeToRole(role);
            Projects[role.ProjectId].Roles[role.RoleId] = role;
            Projects[role.ProjectId].AssignEmployeeToRole(role);

        }

        public List<Project> GetAllProjects()
        {
            return Projects.Values.ToList();
        }
        public Dictionary<Employee, double> EmployeesToAssign(Role role)
        {
            Console.WriteLine("intoFacade");
            Dictionary<Employee, double> employees_score = new Dictionary<Employee, double>();
            Dictionary<Employee, double> employees_job_per = new Dictionary<Employee, double>();
            foreach (Employee employee in Employees.Values)
            {
                int sum = 0;
                double score = 0;
                bool disqualified = false;
                if (employee.YearsExperience < role.YearsExperience)
                    disqualified = true;

                foreach (Language language in role.ForeignLanguages.Values)
                {
                    //Language roleLanguage = role.ForeignLanguages[language.LanguageID];
                    if (employee.ForeignLanguages.TryGetValue(language.LanguageID, out var employeeLang))
                    {
                        if (employeeLang.Level < language.Level)
                            disqualified = true;
                    }
                    else
                    {
                        disqualified = true;
                    }
                }
                if (disqualified == true)
                    continue;

                foreach (Skill skill in role.Skills.Values)
                {
                    if (employee.Skills.ContainsKey(skill.SkillId))
                    {
                        Skill employeeSkill = employee.Skills[skill.SkillId];
                        if (employeeSkill.Level == skill.Level)
                        {
                            score = score + 3 * (double)(role.Skills.Count - skill.Priority + 1) / 10;
                            sum = sum + 1;
                        }
                        else if (employeeSkill.Level > skill.Level)
                        {
                            score = score + 2 * (double)(role.Skills.Count - skill.Priority + 1) / 10;
                            sum = sum + 1;
                        }
                        else if (employeeSkill.Level + 1 == skill.Level)
                            score = score + 1 * (double)(role.Skills.Count - skill.Priority + 1) / 10;
                    }
                }
                if (sum == role.Skills.Count)
                {
                    if (employee.CalculateJobPercentage() + role.JobPercentage < 100)
                        employees_job_per[employee] = employee.CalculateJobPercentage();
                }
                else
                {
                    employees_score[employee] = score;
                }
            }
            Dictionary<Employee, double> sortedEmployeesScore = employees_score.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value); ;
            Dictionary<Employee, double> sortedEmployeesJobPer = employees_job_per.OrderBy(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value); ;
            var combinedDictionary = sortedEmployeesJobPer.Concat(sortedEmployeesScore).ToDictionary(kv => kv.Key, kv => kv.Value);
            return combinedDictionary;
        }




        public Project GetProjectById(int projectId)
        {
            return Projects.TryGetValue(projectId, out Project project) ? project : null;
        }

        public List<Employee> GetAllEmployees()
        {
            return Employees.Values.ToList();
        }

        public Employee GetEmployeeById(int employeeId)
        {
            return Employees.TryGetValue(employeeId, out Employee employee) ? employee : null;
        }

        public int AddTicket(int employeeId, DateTime startDate, DateTime endDate, string description, Reason reason, bool reminder = true)
        {
            Employee employee = Employees[employeeId] ?? throw new Exception($"No such employee {employeeId}");
            lock (_ticketLock)
            {
                Ticket ticket = new Ticket(ticketCount++, employeeId, employee.UserName, startDate, endDate, description, reason);
                Tickets.Add(ticket.TicketId, ticket);
                try
                {
                    ticketRepo.Add(ticket);
                    var managers = Employees.Values.Where(employee => employee.IsManager);
                    foreach (var manager in managers)
                    {
                        manager.Notify(ticket, reminder);
                    }
                    return ticket.TicketId;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public void CloseTicket(int ticketId)
        {
            Ticket ticket = ticketRepo.GetById(ticketId);
            try
            {
                Tickets[ticketId].CloseTicket();
                ticket.CloseTicket();
                ticketRepo.Update(ticket);
            }
            catch (Exception e)
            {
                throw new Exception("ticket is not exist");
            }
        }

        public List<Ticket> GetOpensTickets()
        {
            List<Ticket> OpensTickets = new List<Ticket>();
            foreach (Ticket ticket in Tickets.Values)
            {
                if (ticket.Open == true)
                    OpensTickets.Add(ticket);
            }

            return OpensTickets;
        }

        public List<Ticket> GetOpensTickets5days()
        {
            List<Ticket> OpensTickets = new List<Ticket>();
            DateTime now = DateTime.Now;

            foreach (Ticket ticket in Tickets.Values)
            {
                if (ticket.Open && (ticket.StartDate - now).TotalDays <= 5)
                {
                    OpensTickets.Add(ticket);
                }
            }

            return OpensTickets;
        }


        public Dictionary<string, List<Employee>> GetEmployeesJobPre()
        {
            Dictionary<string, List<Employee>> JobPerEmployees = new Dictionary<string, List<Employee>>();
            foreach (Employee employee in Employees.Values)
            {
                double percent = employee.CalculateJobPercentage() * 100;
                string key = null;
                if (percent > 100)
                    key = "above 100%";
                else if (percent > 80)
                    key = "between 80% and 100%";
                else if (percent >= 50)
                    key = "between 50% and 80%";
                else if (percent < 50)
                    key = "under 50%";

                if (key != null)
                {
                    if (!JobPerEmployees.ContainsKey(key))
                        JobPerEmployees[key] = new List<Employee>();
                    JobPerEmployees[key].Add(employee);
                }
            }
            return JobPerEmployees;
        }

        public List<Project> GetProjectsThatEndThisMonth()
        {
            List<Project> projects = new List<Project>();
            foreach (Project project in Projects.Values)
            {
                if ((project.Date - DateTime.Now).TotalDays <= 30)
                {
                    projects.Add(project);
                }
            }
            return projects;
        }

        public Dictionary<Project, int> GetNumEmployeesInProject()
        {
            Dictionary<Project, int> EmployeesInProject = new Dictionary<Project, int>();
            foreach (Project project in Projects.Values)
            {
                EmployeesInProject[project] = project.Roles.Count();
            }
            return EmployeesInProject;
        }

        public List<Employee> GetEmployeesThatInVacationThisMonth()
        {
            List<Employee> EmployeesInVacation = new List<Employee>();
            foreach (Ticket ticket in Tickets.Values)
            {
                if ((ticket.StartDate - DateTime.Now).TotalDays <= 30)
                {
                    EmployeesInVacation.Add(Employees[ticket.EmployeeId]);
                }
            }
            return EmployeesInVacation;
        }

        public Dictionary<Project, Double> GetProjectHourRatio()
        {
            Dictionary<Project, Double> ProjectHourRatio = new Dictionary<Project, Double>();
            foreach (Project project in Projects.Values)
            {
                Double sum = 0;
                foreach (Role role in project.Roles.Values)
                {
                    double numOfMonths = (DateTime.Now - role.StartDate).TotalDays / 30;
                    sum = sum + (role.JobPercentage * 100 * 180) * numOfMonths;
                }
                ProjectHourRatio[project] = sum / project.RequiredHours;
            }
            return ProjectHourRatio;
        }

        public Dictionary<Enums.Reasons, List<Employee>> GetEmployeesThatInVacationThisMonthAndReason()
        {
            Dictionary<Enums.Reasons, List<Employee>> EmployeesInVacation = new Dictionary<Enums.Reasons, List<Employee>>();
            foreach (Ticket ticket in Tickets.Values)
            {
                if (((ticket.StartDate - DateTime.Now).TotalDays <= 30) || (ticket.StartDate <= DateTime.Now && ticket.EndDate >= DateTime.Now))
                {
                    var reason = ticket.Reason.ReasonType;
                    if (!EmployeesInVacation.ContainsKey(reason))
                        EmployeesInVacation[reason] = new List<Employee>();
                    EmployeesInVacation[reason].Add(Employees[ticket.EmployeeId]);
                    EmployeesInVacation[reason] = EmployeesInVacation[reason].Distinct().ToList(); // Ensure no duplicates
                }
            }
            return EmployeesInVacation;
        }


        public static string GeneratePassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder password = new StringBuilder(8);

            for (int i = 0; i < 8; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        public Tuple<string, string> CreateEmployee(string name, string phoneNumber, string email,
        TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages,
        ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, bool isManager)
        {
            string password = GeneratePassword();
            try
            {
                var mailParsed = ValidateEmail(email);
                Employee employee = new Employee(name, employeeCount++, phoneNumber, mailParsed, timezone, foreignLanguages, skills, yearsExperience, jobPercentage, password, isManager);
                Employees.Add(employee.EmployeeId, employee);
                employeeRepo.Add(employee);
                Tuple<string, string> userNamePass = new Tuple<string, string>(name, password);
                return userNamePass;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int CreateEmployee(string name, string password, string phoneNumber, string email,
        TimeZones timezone, ConcurrentDictionary<int, Language> foreignLanguages,
        ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, bool isManager)
        {
            try
            {
                if (Employees.Values.Select(emp => emp.UserName).Contains(name))
                {
                    throw new Exception($"Employee with name {name} already exists.");
                }

                var mailParsed = ValidateEmail(email);
                Employee employee = new Employee(name, employeeCount++, phoneNumber, mailParsed, timezone, foreignLanguages, skills, yearsExperience, jobPercentage, password, isManager);
                Employees.Add(employee.EmployeeId, employee);
                employeeRepo.Add(employee);
                return employee.EmployeeId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"🔥 Unhandled Exception: {e.Message}\n{e.StackTrace}");
                throw new Exception(e.Message);
            }
        }


        private MailAddress ValidateEmail(string email)
        {
            try
            {
                return new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Email address is not valid.");
            }

        }

        public void DeleteEmployee(int employeeId)
        {
            if (!Employees.ContainsKey(employeeId))
            {
                throw new Exception($"No such employee {employeeId}");
            }
            if (employeeRepo.GetById(employeeId) == null)
            {
                throw new Exception($"No such employee {employeeId}");
            }
            //remove assign roles of employee
            foreach (var project in Projects.Values)
            {
                Dictionary<int, Role> roles = project.GetRoles();
                foreach (var role in roles.Values)
                {
                    if (role.EmployeeId == employeeId)
                    {
                        role.RemoveEmployeeAssign();
                    }
                }
            }
            Employees.Remove(employeeId);
            try
            {
                employeeRepo.Delete(employeeId);
            }
            catch (Exception e)
            {
                throw new Exception($"No such employee {employeeId} " + $"{e}");
            }
        }

        public int? Login(string userName, string password)
        {
            try
            {
                var employee = employeeRepo.GetByUserName(userName);
                if (!employee.VerifyPassword(password, employee.Password))
                {
                    throw new Exception("Wrong password");
                }
                // if(employee.Login())//fix here ...
                //     return employee.EmployeeId;

                return employee.EmployeeId;
            }
            catch (Exception)
            {
                throw;

            }
        }

        public void EditEmail(int userId, string newEmail)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            try
            {
                var emailAddress = ValidateEmail(newEmail);
                employee.EditEmail(emailAddress);
                employeeRepo.Update(employee);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public void EditPhoneNumber(int userId, string newPhoneNumber)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            employee.EditPhoneNumber(newPhoneNumber);
            employeeRepo.Update(employee);
        }

        public void EditTimeZone(int userId, TimeZones newTimeZone)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            employee.EditTimeZone(newTimeZone);
            employeeRepo.Update(employee);
        }

        public void EditYearOfExpr(int userId, int newyearOfExpr)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            employee.EditYearOfExpr(newyearOfExpr);
            employeeRepo.Update(employee);
        }

        public void EditJobPercentage(int userId, double newJobPercentage)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            employee.EditJobPercentage(newJobPercentage);
            employeeRepo.Update(employee);
        }

        public void AddSkill(int userId, Skill newSkill)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            if (employee.GetSkills().ContainsKey(newSkill.SkillId))
            {
                throw new Exception($"Skill {newSkill.SkillId} exists in employee {userId}");
            }
            employee.AddSkill(newSkill);
        }

        public void RemoveSkill(int userId, int skillId)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            if (!employee.GetSkills().ContainsKey(skillId))
            {
                throw new Exception($"Skill {skillId} does not exists in employee {userId}");
            }
            employee.RemoveSkill(skillId);
        }

        public void AddLanguage(int userId, Language newLanguage)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            if (employee.GetLanguages().ContainsKey(newLanguage.LanguageID))
            {
                throw new Exception($"Language {newLanguage.LanguageID} exists in employee {userId}");
            }
            employee.AddLanguage(newLanguage);
        }

        public void RemoveLanguage(int userId, int languageID)
        {
            if (!Employees.ContainsKey(userId))
            {
                throw new Exception($"No such employee {userId}");
            }
            var employee = employeeRepo.GetById(userId);
            if (employee == null)
            {
                throw new Exception($"No such employee {userId}");
            }
            if (!employee.GetLanguages().ContainsKey(languageID))
            {
                throw new Exception($"Language {languageID} does not exists in employee {userId}");
            }
            employee.RemoveLanguage(languageID);
        }

        public List<Role> GetAllRolesByEmployee(int employeeId)
        {
            var roles = Projects.Values
                .SelectMany(p => p.Roles.Values.Where(r => r.EmployeeId == employeeId))
                .ToList();
            return roles;
        }

        public void UpdateEmployee(Employee employee)
        {
            if (!Employees.ContainsKey(employee.EmployeeId))
            {
                throw new Exception($"No such employee {employee.EmployeeId}");
            }

            Employees[employee.EmployeeId] = employee;
            try
            {
                employeeRepo.Update(employee);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateRole(int projectId, int roleId, Role role)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            if (!Projects[projectId].Roles.ContainsKey(roleId))
            {
                throw new Exception($"No such role {roleId} in project {projectId}");
            }
            Projects[projectId].Roles[roleId] = role;
            try
            {
                projectRepo.Update(Projects[projectId]);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteRole(int projectId, int roleId)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            if (!Projects[projectId].Roles.ContainsKey(roleId))
            {
                throw new Exception($"No such role {roleId} in project {projectId}");
            }
            Projects[projectId].RemoveRole(roleId);
            try
            {
                projectRepo.Update(Projects[projectId]);
                RoleRepo.GetInstance().Delete(RoleRepo.GetInstance().GetById(roleId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void EditTicket(int employeeId, Ticket ticket)
        {
            if (!Tickets.ContainsKey(ticket.TicketId))
            {
                throw new Exception($"No such ticket {ticket.TicketId}");
            }
            if (Tickets[ticket.TicketId].EmployeeId != employeeId)
            {
                throw new Exception($"No such employee {employeeId} for ticket {ticket.TicketId}");
            }
            Tickets[ticket.TicketId] = ticket;
            try
            {
                ticketRepo.Update(ticket);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}