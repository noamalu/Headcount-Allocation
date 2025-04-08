using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int ProjectId { get; set; }

        public int? EmployeeId { get; set; }
        public int TimeZone { get; set; }

        public List<Language> ForeignLanguages { get; set; }

        public List<Skill> Skills { get; set; }

        public int YearsExperience { get; set; }

        public double JobPercentage { get; set; }

        public string Description { get; set; }

        public static explicit operator HeadcountAllocation.Domain.Role(Role role)
        {
            return new HeadcountAllocation.Domain.Role(
                role.RoleName,
                role.RoleId,
                role.ProjectId,
                HeadcountAllocation.Domain.Enums.GetValueById<HeadcountAllocation.Domain.Enums.TimeZones>(role.TimeZone),
                new ConcurrentDictionary<int, HeadcountAllocation.Domain.Language>(
                    role.ForeignLanguages.Select(language =>
                        new KeyValuePair<int, HeadcountAllocation.Domain.Language>(
                            language.LanguageTypeId, (HeadcountAllocation.Domain.Language)language)
                    )
                ),
                new ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill>(
                    role.Skills.Select(skill =>
                        new KeyValuePair<int, HeadcountAllocation.Domain.Skill>(
                            skill.SkillTypeId, (HeadcountAllocation.Domain.Skill)skill)
                    )
                ),
                role.YearsExperience,
                role.JobPercentage,
                role.Description
            );
        }

    }
}