using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using HeadcountAllocation.Services;
using static HeadcountAllocation.Domain.Enums;

namespace API.Services
{
    public class ProjectService
    {
        private readonly HeadCountService _headCountService;

        public ProjectService(HeadCountService headcountService)
        {
            _headCountService = headcountService;
        }
        public async Task<List<Response<Role>>> LinkRolesToProject(int projectId, List<Role> roles)
        {
            var createRoleTasks = new List<Task<Response<HeadcountAllocation.Domain.Role>>>();
            foreach (var role in roles){
                var skills = new ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill>(){};
                var languages = new ConcurrentDictionary<int, HeadcountAllocation.Domain.Language>(){};

                foreach(var skill in role.Skills)
                {
                    skills.TryAdd(skill.SkillTypeId, (HeadcountAllocation.Domain.Skill)skill);
                }
                foreach(var language in role.ForeignLanguages)
                {
                    languages.TryAdd(language.LanguageTypeId, (HeadcountAllocation.Domain.Language)language);
                }            

                createRoleTasks.Add(Task.Run(() =>_headCountService.AddRoleToProject(role.RoleName, projectId,
                    GetValueById<TimeZones>(role.TimeZone), 
                    languages, skills, 
                    role.YearsExperience, role.JobPercentage, role.Description)));
            }
            await Task.WhenAll(createRoleTasks.ToArray());
            return createRoleTasks.Select(task => task.Result)
                .Select(role => Response<Role>
                            .FromValue(
                                        new Role
                                        {
                                            RoleId = role.Value.RoleId,
                                            RoleName = role.Value.RoleName,
                                            ProjectId = role.Value.ProjectId,
                                            EmployeeId = role.Value.EmployeeId,
                                            TimeZone = HeadcountAllocation.Domain.Enums.GetId(role.Value.TimeZone),
                                            ForeignLanguages = role.Value.ForeignLanguages.Values?.Select(language => new Language{
                                                    LanguageId = language.LanguageID,
                                                    LanguageTypeId = HeadcountAllocation.Domain.Enums.GetId(language.LanguageType),
                                                    Level = language.Level                                                        
                                        }).ToList() ??new(),
                                            Skills = role.Value.Skills.Values?.Select(skill => new Skill{
                                                    SkillTypeId = HeadcountAllocation.Domain.Enums.GetId(skill.SkillType),
                                                    Level = skill.Level
                                        }).ToList() ??new(),
                                            YearsExperience = role.Value.YearsExperience,
                                            JobPercentage = role.Value.JobPercentage,
                                            Description = role.Value.Description
                                        })).ToList();
        }

        public List<Role> GetRolesByProject(int projectId)
        {
            return _headCountService.GetAllRolesByProject(projectId).Value.Values.Select(role => new Role
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    ProjectId = role.ProjectId,
                    EmployeeId = role.EmployeeId,
                    TimeZone = GetId(role.TimeZone),
                    ForeignLanguages = role.ForeignLanguages.Values?.Select(language => new Language
                    {
                        LanguageId = language.LanguageID,
                        LanguageTypeId = GetId(language.LanguageType),
                        Level = language.Level
                    }).ToList() ?? new(),
                    Skills = role.Skills.Values?.Select(skill => new Skill
                    {
                        SkillTypeId = GetId(skill.SkillType),
                        Level = skill.Level,
                        Priority = skill.Priority
                    }).ToList() ?? new(),
                    YearsExperience = role.YearsExperience,
                    JobPercentage = role.JobPercentage,
                    Description = role.Description
                }).ToList();
        }
    }
}