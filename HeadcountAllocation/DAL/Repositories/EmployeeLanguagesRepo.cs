using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class EmployeeLanguagesRepo
    {
        private static Dictionary<int, Language> EmployeesLanguages;

        private object Lock;

        private static EmployeeLanguagesRepo _employeesLanguagesdRepo = null;

        private EmployeeLanguagesRepo()
        {
            EmployeesLanguages = new();
            Lock = new object();
        }
        public static EmployeeLanguagesRepo GetInstance()
        {
            _employeesLanguagesdRepo ??= new EmployeeLanguagesRepo();
            return _employeesLanguagesdRepo;
        }

        public static void Dispose(){
            _employeesLanguagesdRepo = new EmployeeLanguagesRepo();
        }

        public IEnumerable<Language> getAll()
        {
            Load();
            return EmployeesLanguages.Values;
        }

        public void Delete(Language languages)
        {
            Delete(languages.LanguageID);
        }

        public void Add(Language language)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            EmployeesLanguages.Add(language.LanguageID, language);
            try{
                lock (Lock)
                {
                    dbContext.EmployeeLanguages.Add(new EmployeeLanguagesDTO(language));
                    
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
                    var dbEmployeeLanguages = dbContext.EmployeeLanguages.Find(id);
                    if(dbEmployeeLanguages is not null) {
                        if (EmployeesLanguages.ContainsKey(id))
                        {
                            Language Languages = EmployeesLanguages[id];
                           
                            EmployeesLanguages.Remove(id);
                        }

                        dbContext.EmployeeLanguages.Remove(dbEmployeeLanguages);
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
            return EmployeesLanguages.Values.ToList();
        }

        public Language GetById(int id)
        {
            if (EmployeesLanguages.ContainsKey(id))
                return EmployeesLanguages[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    EmployeeLanguagesDTO mDto = dbContext.EmployeeLanguages.Find(id);
                    if (mDto != null)
                    {
                        LoadEmployeeLanguages(mDto);
                        return EmployeesLanguages[id];
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
                EmployeesLanguages[language.LanguageID] = language;
                
                try{
                    lock (Lock)
                    {
                        EmployeeLanguagesDTO employeeLanguages = DBcontext.GetInstance().EmployeeLanguages.Find(language.LanguageID);
                        if (employeeLanguages != null){
                            employeeLanguages.LanguageTypeId = Enums.GetId(language.LanguageType);
                            if (language.Level != 0) {                                
                                employeeLanguages.Level = language.Level;
                            }                            
                            DBcontext.GetInstance().SaveChanges();
                        }
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Update language");
                }
                
            }
            else{
                throw new KeyNotFoundException($"language with ID {language.LanguageID} not found.");
            }
        }
     

       

        public bool ContainsID(int id)
        {
            return EmployeesLanguages.ContainsKey(id);
        }

        public bool ContainsValue(Language employee)
        {
            return EmployeesLanguages.ContainsKey(employee.LanguageID);
        }

        public void Clear()
        {
            EmployeesLanguages.Clear();

        }        

        public void ResetDomainData()
        {
            EmployeesLanguages = new ();
            
        }

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<EmployeeLanguagesDTO> languages = dbContext.EmployeeLanguages.ToList();
            foreach (var language in languages)
            {
                EmployeesLanguages.TryAdd(language.LanguageID, new Language(language));
                
            }
        }

        private void LoadEmployeeLanguages(EmployeeLanguagesDTO employeeLanguage)
        {
            var language = new Language(employeeLanguage);
            EmployeesLanguages[language.LanguageID] = language;
            
        }
    }
}