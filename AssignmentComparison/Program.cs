// Full comparison between greedy and scoring-based assignment using domain enums
using System;
using System.Collections.Generic;
using System.Linq;
using HeadcountAllocation.Domain;

namespace AssignmentComparison
{
    public class Skill
    {
        public int SkillId;
        public string SkillType;
        public int Level;
        public int Priority;
    }

    public class Language
    {
        public int LanguageID;
        public string LanguageType;
        public int Level;
    }

    public class Role
    {
        public int RoleId;
        public string RoleName;
        public int YearsExperience;
        public Dictionary<int, Skill> Skills = new();
        public Dictionary<int, Language> ForeignLanguages = new();
        public int JobPercentage;
    }

    public class Project
    {
        public string Name;
        public List<Role> Roles = new();
    }

    public class Employee
    {
        public int Id;
        public string Name;
        public int YearsExperience;
        public Dictionary<int, Skill> Skills = new();
        public Dictionary<int, Language> ForeignLanguages = new();
        public int JobPercentage;
    }

    public static class AssignmentAlgorithms
    {
        public static Dictionary<Employee, double> EmployeesToAssign(Role role, List<Employee> employees)
        {
            Dictionary<Employee, double> result = new();
            foreach (var employee in employees)
            {
                double score = 0;
                bool disqualified = employee.YearsExperience < role.YearsExperience;

                foreach (var roleLang in role.ForeignLanguages.Values)
                {
                    if (!employee.ForeignLanguages.TryGetValue(roleLang.LanguageID, out var empLang) || empLang.Level < roleLang.Level)
                        disqualified = true;
                }
                if (disqualified) continue;

                foreach (var roleSkill in role.Skills.Values)
                {
                    if (employee.Skills.TryGetValue(roleSkill.SkillId, out var empSkill))
                    {
                        if (empSkill.Level == roleSkill.Level)
                            score += 3 * (double)(role.Skills.Count - roleSkill.Priority + 1) / 10;
                        else if (empSkill.Level > roleSkill.Level)
                            score += 2 * (double)(role.Skills.Count - roleSkill.Priority + 1) / 10;
                        else if (empSkill.Level + 1 == roleSkill.Level)
                            score += 1 * (double)(role.Skills.Count - roleSkill.Priority + 1) / 10;
                    }
                }
                result[employee] = score;
            }
            return result.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static Dictionary<Role, Employee> SmartAssign(List<Role> roles, List<Employee> employees)
        {
            var assigned = new Dictionary<Role, Employee>();
            var used = new HashSet<int>();

            foreach (var role in roles)
            {
                var scored = EmployeesToAssign(role, employees);
                var best = scored.Keys.FirstOrDefault(e => !used.Contains(e.Id));
                if (best != null)
                {
                    assigned[role] = best;
                    used.Add(best.Id);
                }
            }
            return assigned;
        }

        public static Dictionary<Role, Employee> GreedyAssign(List<Role> roles, List<Employee> employees)
        {
            var assigned = new Dictionary<Role, Employee>();

            foreach (var role in roles)
            {
                var scored = EmployeesToAssign(role, employees);
                var best = scored.Keys.FirstOrDefault();
                if (best != null)
                {
                    assigned[role] = best;
                }
            }
            return assigned;
        }
    }

    class Program
    {
        static void Main()
        {
            int py = Enums.GetId(Enums.Skills.Python);
            int sql = Enums.GetId(Enums.Skills.SQL);
            int api = Enums.GetId(Enums.Skills.API);
            int java = Enums.GetId(Enums.Skills.Java);
            int ui = Enums.GetId(Enums.Skills.UI);
            int eng = Enums.GetId(Enums.Languages.English);
            int heb = Enums.GetId(Enums.Languages.Hebrew);
            int spa = Enums.GetId(Enums.Languages.Spanish);

            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Alice", YearsExperience = 3, Skills = new() { { py, new Skill { SkillId = py, SkillType = "Python", Level = 2, Priority = 1 } }, { sql, new Skill { SkillId = sql, SkillType = "SQL", Level = 3, Priority = 2 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 3 } } } },
                new Employee { Id = 2, Name = "Bob", YearsExperience = 6, Skills = new() { { java, new Skill { SkillId = java, SkillType = "Java", Level = 3, Priority = 1 } }, { api, new Skill { SkillId = api, SkillType = "API", Level = 1, Priority = 2 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 3 } }, { spa, new Language { LanguageID = spa, LanguageType = "Spanish", Level = 2 } } } },
                new Employee { Id = 3, Name = "Charlie", YearsExperience = 3, Skills = new() { { ui, new Skill { SkillId = ui, SkillType = "UI", Level = 2, Priority = 1 } }, { java, new Skill { SkillId = java, SkillType = "Java", Level = 2, Priority = 2 } } }, ForeignLanguages = new() { { heb, new Language { LanguageID = heb, LanguageType = "Hebrew", Level = 2 } }, { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 2 } } } },
                new Employee { Id = 4, Name = "Dana", YearsExperience = 6, Skills = new() { { py, new Skill { SkillId = py, SkillType = "Python", Level = 3, Priority = 1 } }, { sql, new Skill { SkillId = sql, SkillType = "SQL", Level = 3, Priority = 2 } }, { api, new Skill { SkillId = api, SkillType = "API", Level = 2, Priority = 3 } }, { java, new Skill { SkillId = java, SkillType = "Java", Level = 3, Priority = 1 } }, { ui, new Skill { SkillId = ui, SkillType = "UI", Level = 2, Priority = 2 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 3 } }, { spa, new Language { LanguageID = spa, LanguageType = "Spanish", Level = 1 } }, { heb, new Language { LanguageID = heb, LanguageType = "Hebrew", Level = 2 } } } },
                new Employee { Id = 5, Name = "Eli", YearsExperience = 5, Skills = new() { { java, new Skill { SkillId = java, SkillType = "Java", Level = 2, Priority = 1 } }, { ui, new Skill { SkillId = ui, SkillType = "UI", Level = 3, Priority = 2 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 3 } }, { heb, new Language { LanguageID = heb, LanguageType = "Hebrew", Level = 2 } } } },
                new Employee { Id = 6, Name = "Tamar", YearsExperience = 4, Skills = new() { { py, new Skill { SkillId = py, SkillType = "Python", Level = 2, Priority = 1 } }, { sql, new Skill { SkillId = sql, SkillType = "SQL", Level = 2, Priority = 2 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 3 } } } },
            };

            var roles = new List<Role>
            {
                new Role { RoleId = 1, RoleName = "Data Engineer", YearsExperience = 4, Skills = new() { { py, new Skill { SkillId = py, SkillType = "Python", Level = 3, Priority = 0 } }, { sql, new Skill { SkillId = sql, SkillType = "SQL", Level = 3, Priority = 1 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 3 } } } },
                new Role { RoleId = 2, RoleName = "Backend Developer", YearsExperience = 6, Skills = new() { { java, new Skill { SkillId = java, SkillType = "Java", Level = 3, Priority = 0 } }, { api, new Skill { SkillId = api, SkillType = "API", Level = 2, Priority = 1 } } }, ForeignLanguages = new() { { spa, new Language { LanguageID = spa, LanguageType = "Spanish", Level = 2 } } } },
                new Role { RoleId = 3, RoleName = "UI/UX Expert", YearsExperience = 3, Skills = new() { { ui, new Skill { SkillId = ui, SkillType = "UI", Level = 2, Priority = 0 } }, { java, new Skill { SkillId = java, SkillType = "Java", Level = 2, Priority = 1 } } }, ForeignLanguages = new() { { heb, new Language { LanguageID = heb, LanguageType = "Hebrew", Level = 2 } } } },
                new Role { RoleId = 4, RoleName = "API Integrator", YearsExperience = 5, Skills = new() { { api, new Skill { SkillId = api, SkillType = "API", Level = 2, Priority = 0 } }, { sql, new Skill { SkillId = sql, SkillType = "SQL", Level = 3, Priority = 1 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 3 } } } },
                new Role { RoleId = 5, RoleName = "Junior Developer", YearsExperience = 2, Skills = new() { { java, new Skill { SkillId = java, SkillType = "Java", Level = 2, Priority = 0 } } }, ForeignLanguages = new() { { eng, new Language { LanguageID = eng, LanguageType = "English", Level = 2 } } } },
            };

            Console.WriteLine("--- Greedy Assignment ---");
            var greedy = AssignmentAlgorithms.GreedyAssign(roles, employees);
            foreach (var pair in greedy)
                Console.WriteLine($"Role {pair.Key.RoleName} -> {pair.Value.Name}");

            Console.WriteLine("\n--- Smart Assignment (no duplicates) ---");
            var smart = AssignmentAlgorithms.SmartAssign(roles, employees);
            foreach (var pair in smart)
                Console.WriteLine($"Role {pair.Key.RoleName} -> {pair.Value.Name}");
        }
    }
}