using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class RoleSkillsRepo
    {
        private static Dictionary<int, Skill> RolesSkills;

        private object Lock;

        private static RoleSkillsRepo _rolesSkilldRepo = null;

        private RoleSkillsRepo()
        {
            RolesSkills = new();
            Lock = new object();
        }
        public static RoleSkillsRepo GetInstance()
        {
            _rolesSkilldRepo ??= new RoleSkillsRepo();
            return _rolesSkilldRepo;
        }

        public static void Dispose(){
            _rolesSkilldRepo = new RoleSkillsRepo();
        }

        public IEnumerable<Skill> getAll()
        {
            Load();
            return RolesSkills.Values;
        }

        public void Delete(Skill skill)
        {
            Delete(skill.SkillId);
        }

        public void Add(Skill skill)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            RolesSkills.Add(skill.SkillId, skill);
            try{
                lock (Lock)
                {
                    dbContext.RoleSkills.Add(new RoleSkillsDTO(skill));
                    
                    dbContext.SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Add employee skill");
            }
            

        }

        public void Delete(int id)
        {
            try{
                lock (Lock)
                {
                    var dbContext = DBcontext.GetInstance();
                    var dbRoleSkill = dbContext.RoleSkills.Find(id);
                    if(dbRoleSkill is not null) {
                        if (RolesSkills.ContainsKey(id))
                        {
                            Skill skill = RolesSkills[id];
                           
                            RolesSkills.Remove(id);
                        }

                        dbContext.RoleSkills.Remove(dbRoleSkill);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Delete employee skill");
            }
            
        }

        public List<Skill> GetAll()
        {
            Load();
            return RolesSkills.Values.ToList();
        }

        public Skill GetById(int id)
        {
            if (RolesSkills.ContainsKey(id))
                return RolesSkills[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    RoleSkillsDTO mDto = dbContext.RoleSkills.Find(id);
                    if (mDto != null)
                    {
                        LoadRoleSkills(mDto);
                        return RolesSkills[id];
                    }
                    throw new ArgumentException("Invalid user ID.");
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get skill");
                }
            }
        }

        public void Update(Skill skill)
        {
            if (ContainsValue(skill))
            {
                RolesSkills[skill.SkillId] = skill;
                
                try{
                    lock (Lock)
                    {
                        RoleSkillsDTO rolesSkills = DBcontext.GetInstance().RoleSkills.Find(skill.SkillId);
                        if (rolesSkills != null){
                            rolesSkills.SkillTypeId = Enums.GetId(skill.SkillType);
                            if (skill.Level != 0) {                                
                                rolesSkills.Level = skill.Level;
                            }                            
                            DBcontext.GetInstance().SaveChanges();
                        }
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Update skill");
                }
                
            }
            else{
                throw new KeyNotFoundException($"Skill with ID {skill.SkillId} not found.");
            }
        }
     

       

        public bool ContainsID(int id)
        {
            return RolesSkills.ContainsKey(id);
        }

        public bool ContainsValue(Skill employee)
        {
            return RolesSkills.ContainsKey(employee.SkillId);
        }

        public void Clear()
        {
            RolesSkills.Clear();

        }        

        public void ResetDomainData()
        {
            RolesSkills = new ();
            
        }

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<RoleSkillsDTO> skills = dbContext.RoleSkills.ToList();
            foreach (var skill in skills)
            {
                RolesSkills.TryAdd(skill.SkillTypeId, new Skill(skill));
                
            }
        }

        private void LoadRoleSkills(RoleSkillsDTO roleSkill)
        {
            var skill = new Skill(roleSkill);
            RolesSkills[skill.SkillId] = skill;
            
        }
    }
}