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
        // public async Task<List<Response<Role>>> LinkRolesToProject(int projectId, List<Role> roles)
        // {
        //     var createRoleTasks = new List<Task<Response<HeadcountAllocation.Domain.Role>>>();
        //     foreach (var role in roles){
        //         var skills = new ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill>(){};
        //         var languages = new ConcurrentDictionary<int, HeadcountAllocation.Domain.Language>(){};

        //         foreach(var skill in role.Skills)
        //         {
        //             skills.TryAdd(skill.SkillTypeId, (HeadcountAllocation.Domain.Skill)skill);
        //         }
        //         foreach(var language in role.ForeignLanguages)
        //         {
        //             languages.TryAdd(language.LanguageTypeId, (HeadcountAllocation.Domain.Language)language);
        //         }            

        //         createRoleTasks.Add(Task.Run(() =>_headCountService.AddRoleToProject(role.RoleName, projectId,
        //             GetValueById<TimeZones>(role.TimeZone), 
        //             languages, skills, 
        //             role.YearsExperience, role.JobPercentage, role.Description)));
        //     }
        //     await Task.WhenAll(createRoleTasks.ToArray());
        //     return createRoleTasks.Select(task => task.Result)
        //         .Select(role => Response<Role>
        //                     .FromValue(
        //                                 new Role
        //                                 {
        //                                     RoleId = role.Value.RoleId,
        //                                     RoleName = role.Value.RoleName,
        //                                     ProjectId = role.Value.ProjectId,
        //                                     EmployeeId = role.Value.EmployeeId,
        //                                     TimeZone = HeadcountAllocation.Domain.Enums.GetId(role.Value.TimeZone),
        //                                     ForeignLanguages = role.Value.ForeignLanguages.Values?.Select(language => new Language{
        //                                             LanguageId = language.LanguageID,
        //                                             LanguageTypeId = HeadcountAllocation.Domain.Enums.GetId(language.LanguageType),
        //                                             Level = language.Level                                                        
        //                                 }).ToList() ??new(),
        //                                     Skills = role.Value.Skills.Values?.Select(skill => new Skill{
        //                                             SkillTypeId = HeadcountAllocation.Domain.Enums.GetId(skill.SkillType),
        //                                             Level = skill.Level
        //                                 }).ToList() ??new(),
        //                                     YearsExperience = role.Value.YearsExperience,
        //                                     JobPercentage = role.Value.JobPercentage,
        //                                     Description = role.Value.Description
        //                                 })).ToList();
        // }

        public async Task<List<Response<Role>>> LinkRolesToProject(int projectId, List<Role> roles)
        {
            var createRoleTasks = roles.Select(role => Task.Run(() => CreateRoleAsync(projectId, role))).ToList();

            await Task.WhenAll(createRoleTasks);

            var results = new List<Response<Role>>();
            foreach (var task in createRoleTasks)
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("❌ Role creation task failed: " + task.Exception?.GetBaseException().Message);
                    results.Add(Response<Role>.FromError("Failed to create role"));
                }
                else if (task.Result?.Value == null)
                {
                    Console.WriteLine("❌ Role creation returned null value.");
                    results.Add(Response<Role>.FromError("Created role was null."));
                }
                else
                {
                    results.Add(ConvertToApiModel(task.Result));
                }
            }

            return results;
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
                Description = role.Description,
                StartDate = role.StartDate
            }).ToList();
        }

        private Response<Role> ConvertToApiModel(Response<HeadcountAllocation.Domain.Role> response)
        {
            var role = response.Value;
            return Response<Role>.FromValue(new Role
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                ProjectId = role.ProjectId,
                EmployeeId = role.EmployeeId,
                TimeZone = GetId(role.TimeZone),
                ForeignLanguages = role.ForeignLanguages.Values?.Select(l => new Language
                {
                    LanguageId = GetId(l.LanguageType),
                    LanguageTypeId = GetId(l.LanguageType),
                    Level = l.Level
                }).ToList() ?? new(),

                Skills = role.Skills.Values?.Select(s => new Skill
                {
                    SkillTypeId = GetId(s.SkillType),
                    Level = s.Level,
                    // Priority = s.Priority
                }).ToList() ?? new(),

                YearsExperience = role.YearsExperience,
                JobPercentage = role.JobPercentage,
                Description = role.Description,
                // StartDate = role.StartDate
            });
        }

        private ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill> ConvertSkills(List<Skill> skills)
        {
            var dict = new ConcurrentDictionary<int, HeadcountAllocation.Domain.Skill>();
            foreach (var skill in skills)
            {
                dict.TryAdd(skill.SkillTypeId, (HeadcountAllocation.Domain.Skill)skill);
            }
            return dict;
        }

        private ConcurrentDictionary<int, HeadcountAllocation.Domain.Language> ConvertLanguages(List<Language> languages)
        {
            var dict = new ConcurrentDictionary<int, HeadcountAllocation.Domain.Language>();
            foreach (var lang in languages)
            {
                dict.TryAdd(lang.LanguageTypeId, (HeadcountAllocation.Domain.Language)lang);
            }
            return dict;
        }

        private Response<HeadcountAllocation.Domain.Role> CreateRoleAsync(int projectId, Role role)
        {
            var skills = ConvertSkills(role.Skills);
            var languages = ConvertLanguages(role.ForeignLanguages);

            return _headCountService.AddRoleToProject(
                role.RoleName,
                projectId,
                GetValueById<TimeZones>(role.TimeZone),
                languages,
                skills,
                role.YearsExperience,
                role.JobPercentage,
                role.Description,
                role.StartDate
            );
        }


    }
}