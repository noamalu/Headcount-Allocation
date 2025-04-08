using System.Collections.Concurrent;
using System.Net.Mail;
using System.Text;
using HeadcountAllocation.DAL.Repositories;
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

        public ManagerFacade()
        {
            projectRepo = ProjectRepo.GetInstance();
            employeeRepo = EmployeeRepo.GetInstance();
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
            managerFacade = null;
        }

        public int CreateProject(string projectName, string description, DateTime date, int requiredHours, Dictionary<int, Role> roles)
        {
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
                    ConcurrentDictionary<int, Skill> skills, int yearsExperience, double jobPercentage, string description)
        {
            if (!Projects.ContainsKey(projectId))
            {
                throw new Exception($"No such project {projectId}");
            }
            return Projects[projectId].AddRoleToProject(roleName, timeZone, foreignLanguages, skills, yearsExperience, jobPercentage, description, roleCounter++);
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
            Dictionary<Employee, double> employees = new Dictionary<Employee, double>();
            foreach (Employee employee in Employees.Values)
            {
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
                            score = score + 3 * (double)(role.Skills.Count - skill.Priority + 1) / 10;
                        else if (employeeSkill.Level > skill.Level)
                            score = score + 2 * (double)(role.Skills.Count - skill.Priority + 1) / 10;
                        else if (employeeSkill.Level + 1 == skill.Level)
                            score = score + 1 * (double)(role.Skills.Count - skill.Priority + 1) / 10;
                    }
                }

                employees[employee] = score;
            }

            Dictionary<Employee, double> sortedEmployees = employees.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value); ;
            return sortedEmployees;
        }


        public Project GetProjectById(int projectId)
        {
            return Projects.TryGetValue(projectId, out Project project) ? project : null;
        }

        internal List<Employee> GetAllEmployees()
        {
            return Employees.Values.ToList();
        }

        internal Employee GetEmployeeById(int employeeId)
        {
            return Employees.TryGetValue(employeeId, out Employee employee) ? employee : null;
        }

        public int AddTicket(int employeeId, DateTime startDate, DateTime endDate, string description)
        {
            Employee employee = Employees[employeeId] ?? throw new Exception($"No such employee {employeeId}");
            Ticket ticket = new Ticket(ticketCount++, employeeId, employee.UserName, startDate, endDate, description);
            Tickets.Add(ticket.TicketId, ticket);
            try
            {
                ticketRepo.Add(ticket);
                return ticket.TicketId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                if(Employees.Values.Select(emp => emp.UserName).Contains(name))
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
    }
}