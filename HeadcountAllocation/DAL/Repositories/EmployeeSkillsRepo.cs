using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class EmployeeSkillsRepo
    {
        private static Dictionary<int, Skill> EmployeesSkills;

        private object Lock;

        private static EmployeeSkillsRepo _employeedSkilldRepo = null;

        private EmployeeSkillsRepo()
        {
            EmployeesSkills = new();
            Lock = new object();
        }
        public static EmployeeSkillsRepo GetInstance()
        {
            _employeedSkilldRepo ??= new EmployeeSkillsRepo();
            return _employeedSkilldRepo;
        }

        public static void Dispose(){
            _employeedSkilldRepo = new EmployeeSkillsRepo();
        }

        public IEnumerable<Skill> getAll()
        {
            Load();
            return EmployeesSkills.Values;
        }

        public void Delete(Skill skill)
        {
            Delete(skill.SkillId);
        }

        public void Add(Skill skill)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            EmployeesSkills.Add(skill.SkillId, skill);
            try{
                lock (Lock)
                {
                    dbContext.EmployeeSkills.Add(new EmployeeSkillsDTO(skill));
                    
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
                    var dbEmployeeSkill = dbContext.EmployeeSkills.Find(id);
                    if(dbEmployeeSkill is not null) {
                        if (EmployeesSkills.ContainsKey(id))
                        {
                            Skill skill = EmployeesSkills[id];
                           
                            EmployeesSkills.Remove(id);
                        }

                        dbContext.EmployeeSkills.Remove(dbEmployeeSkill);
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
            return EmployeesSkills.Values.ToList();
        }

        public Skill GetById(int id)
        {
            if (EmployeesSkills.ContainsKey(id))
                return EmployeesSkills[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    EmployeeSkillsDTO mDto = dbContext.EmployeeSkills.Find(id);
                    if (mDto != null)
                    {
                        LoadEmployeeSkills(mDto);
                        return EmployeesSkills[id];
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
                EmployeesSkills[skill.SkillId] = skill;
                
                try{
                    lock (Lock)
                    {
                        EmployeeSkillsDTO employeesSkills = DBcontext.GetInstance().EmployeeSkills.Find(skill.SkillId);
                        if (employeesSkills != null){
                            employeesSkills.SkillTypeId = Enums.GetId(skill.SkillType);
                            if (skill.Level != 0) {                                
                                employeesSkills.Level = skill.Level;
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
            return EmployeesSkills.ContainsKey(id);
        }

        public bool ContainsValue(Skill employee)
        {
            return EmployeesSkills.ContainsKey(employee.SkillId);
        }

        public void Clear()
        {
            EmployeesSkills.Clear();

        }        

        public void ResetDomainData()
        {
            EmployeesSkills = new ();
            
        }

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<EmployeeSkillsDTO> skills = dbContext.EmployeeSkills.ToList();
            foreach (var skill in skills)
            {
                EmployeesSkills.TryAdd(skill.EmployeeId, new Skill(skill));
                
            }
        }

        private void LoadEmployeeSkills(EmployeeSkillsDTO EmployeeSkill)
        {
            var skill = new Skill(EmployeeSkill);
            EmployeesSkills[skill.SkillId] = skill;
            
        }
    }
}