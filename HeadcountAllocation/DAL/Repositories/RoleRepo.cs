using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class RoleRepo
    {
          public ConcurrentDictionary<int,  Role> roles; //<storeId, <memberId, Role>>
        private static RoleRepo roleRepositoryRAM = null;

        private object _lock;
        

        private RoleRepo()
        {
            roles = new ConcurrentDictionary<int,  Role>();
            _lock = new object();
        }

        public static RoleRepo GetInstance()
        {
            if (roleRepositoryRAM == null)
                roleRepositoryRAM = new RoleRepo();
            return roleRepositoryRAM;
        }

        public static void Dispose(){
            roleRepositoryRAM = new RoleRepo();
        }

       public void Add(Role role)
        {
            roles.TryAdd(role.RoleId, role);
            try{
                lock(_lock){
                    if (!DBcontext.GetInstance().TimeZones.Any(tz => tz.TimeZoneId == Enums.GetId(role.TimeZone)))
                    throw new Exception($"TimeZoneId {Enums.GetId(role.TimeZone)} does not exist.");

                    foreach (var language in role.ForeignLanguages.Values)
                    {
                        if (!DBcontext.GetInstance().LanguageTypes.Any(lt => lt.LanguageTypeId == Enums.GetId(language.LanguageType)))
                            throw new Exception($"LanguageTypeId {Enums.GetId(language.LanguageType)} does not exist.");
                    }

                    foreach (var skill in role.Skills.Values)
                    {
                        if (!DBcontext.GetInstance().SkillTypes.Any(st => st.SkillTypeId == Enums.GetId(skill.SkillType)))
                            throw new Exception($"SkillTypeId {skill.SkillType} does not exist.");
                    }
                    
                    DBcontext.GetInstance().Roles.Add(new RoleDTO(role));
                    DBcontext.GetInstance().SaveChanges();
                }
            }
            catch(Exception e){
                throw new Exception("There was a problem in Database use- Add Role" + $" {e}");
            }
        }

        public void Delete(Role role)
        {
        try{
            lock (_lock)
            {
                bool shopInDomain = roles.TryRemove(role.RoleId, out _);
                DBcontext context = DBcontext.GetInstance();
                RoleDTO roleDTO = context.Roles.Find(role.RoleId);
                if (shopInDomain)
                {
                    context.Roles.Remove(roleDTO);
                    context.SaveChanges();
                }
                else if (roleDTO != null)
                {
                    context.Roles.Remove(roleDTO);
                    context.SaveChanges();
                }
            }
        }
        catch(Exception){
                throw new Exception("There was a problem in Database use- Delete Role");
        }
    }

    public Role GetById(int id)
        {
            if (roles.ContainsKey(id))
            {
                return roles[id];
            }
            else{
                try{
                    lock (_lock)
                    {
                        RoleDTO roleDTO = DBcontext.GetInstance().Roles.Find(id);
                        if (roleDTO != null)
                        {
                            Role role = new Role(roleDTO);
                            roles.TryAdd(id, role);
                            //role.Initialize(roleDTO);
                            return role;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get Role");
                }
                
            }
        }

        public void Update(Role role)
        {
            try{
                roles[role.RoleId] = role;
                lock(_lock){
                    RoleDTO roleDTO = DBcontext.GetInstance().Roles.Find(role.RoleId);
                    RoleDTO newRole = new RoleDTO(role);
                    if (roleDTO != null)
                    {
                        roleDTO.RoleId = newRole.RoleId;
                        roleDTO.ProjectId = newRole.ProjectId;
                        roleDTO.EmployeeId = newRole.EmployeeId;
                        roleDTO.TimeZoneId = newRole.TimeZoneId;
                        roleDTO.ForeignLanguages = newRole.ForeignLanguages;
                        roleDTO.Skills = newRole.Skills;
                        roleDTO.YearsExperience = newRole.YearsExperience;
                        roleDTO.JobPercentage = newRole.JobPercentage;
                        roleDTO.Description = newRole.Description;
                        roleDTO.StartDate = newRole.StartDate;
                    }
                    else DBcontext.GetInstance().Roles.Add(newRole);
                    DBcontext.GetInstance().SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Update Role");
            }
            
        }

    }
}