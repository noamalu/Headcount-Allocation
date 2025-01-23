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
        public async Task<List<Response<HeadcountAllocation.Domain.Role>>> LinkRolesToProject(int projectId, List<Role> roles)
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
                    role.YearsExperience, role.JobPercentage)));
            }
                await Task.WhenAll(createRoleTasks.ToArray());
                return createRoleTasks.Select(task => task.Result).ToList();
        }
    }
}