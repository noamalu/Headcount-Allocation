using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class RoleLanguagesRepo
    {
        private static Dictionary<int, Language> RolesLanguages;

        private object Lock;

        private static RoleLanguagesRepo _rolesLanguagesdRepo = null;

        private RoleLanguagesRepo()
        {
            RolesLanguages = new();
            Lock = new object();
        }
        public static RoleLanguagesRepo GetInstance()
        {
            _rolesLanguagesdRepo ??= new RoleLanguagesRepo();
            return _rolesLanguagesdRepo;
        }

        public static void Dispose(){
            _rolesLanguagesdRepo = new RoleLanguagesRepo();
        }

        public IEnumerable<Language> getAll()
        {
            Load();
            return RolesLanguages.Values;
        }

        public void Delete(Language languages)
        {
            Delete(languages.LanguageID);
        }

        public void Add(Language language)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            RolesLanguages.Add(language.LanguageID, language);
            try{
                lock (Lock)
                {
                    dbContext.RoleLanguages.Add(new RoleLanguagesDTO(language));
                    
                    dbContext.SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Add employee Languages");
            }
            

        }

        public void Delete(int id)
        {
            try{
                lock (Lock)
                {
                    var dbContext = DBcontext.GetInstance();
                    var dbRoleLanguages = dbContext.RoleLanguages.Find(id);
                    if(dbRoleLanguages is not null) {
                        if (RolesLanguages.ContainsKey(id))
                        {
                            Language Languages = RolesLanguages[id];
                           
                            RolesLanguages.Remove(id);
                        }

                        dbContext.RoleLanguages.Remove(dbRoleLanguages);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Delete employee Languages");
            }
            
        }

        public List<Language> GetAll()
        {
            Load();
            return RolesLanguages.Values.ToList();
        }

        public Language GetById(int id)
        {
            if (RolesLanguages.ContainsKey(id))
                return RolesLanguages[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    RoleLanguagesDTO mDto = dbContext.RoleLanguages.Find(id);
                    if (mDto != null)
                    {
                        LoadRoleLanguages(mDto);
                        return RolesLanguages[id];
                    }
                    throw new ArgumentException("Invalid user ID.");
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get Languages");
                }
            }
        }

        public void Update(Language language)
        {
            if (ContainsValue(language))
            {
                RolesLanguages[language.LanguageID] = language;
                
                try{
                    lock (Lock)
                    {
                        RoleLanguagesDTO roleLanguages = DBcontext.GetInstance().RoleLanguages.Find(language.LanguageID);
                        if (roleLanguages != null){
                            roleLanguages.LanguageTypeId = Enums.GetId(language.LanguageType);
                            if (language.Level != 0) {                                
                                roleLanguages.Level = language.Level;
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
                throw new KeyNotFoundException($"Skill with ID {language.LanguageID} not found.");
            }
        }
     

       

        public bool ContainsID(int id)
        {
            return RolesLanguages.ContainsKey(id);
        }

        public bool ContainsValue(Language employee)
        {
            return RolesLanguages.ContainsKey(employee.LanguageID);
        }

        public void Clear()
        {
            RolesLanguages.Clear();

        }        

        public void ResetDomainData()
        {
            RolesLanguages = new ();
            
        }

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<RoleLanguagesDTO> languages = dbContext.RoleLanguages.ToList();
            foreach (var language in languages)
            {
                RolesLanguages.TryAdd(language.LanguageID, new Language(language));
                
            }
        }

        private void LoadRoleLanguages(RoleLanguagesDTO roleLanguage)
        {
            var language = new Language(roleLanguage);
            RolesLanguages[language.LanguageID] = language;
            
        }
    }
}