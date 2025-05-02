using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.DAL.DTO.Alert;
using HeadcountAllocation.Domain;
using Microsoft.EntityFrameworkCore;

namespace HeadcountAllocation.DAL.Repositories
{
    public class EmployeeRepo
    {
         private static Dictionary<int, Employee> Employees;

         private static Dictionary<string, Employee> EmployeesNames;

        private object Lock;

        private static EmployeeRepo _employeeRepo = null;

        private EmployeeRepo()
        {
            Employees = new Dictionary<int, Employee>();
            Lock = new object();
        }
        public static EmployeeRepo GetInstance()
        {
            _employeeRepo ??= new EmployeeRepo();
            return _employeeRepo;
        }

        public static void Dispose(){
            _employeeRepo = new EmployeeRepo();
        }

        public IEnumerable<Employee> getAll()
        {
            Load();
            return Employees.Values;
        }

        public void Delete(Employee employee)
        {
            Delete(employee.EmployeeId);
        }

        public void Add(Employee employee)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            Employees.Add(employee.EmployeeId, employee);
            try{
                lock (Lock)
                {
                    EmployeeDTO empdto = new EmployeeDTO(employee);
                    dbContext.Employees.Add(empdto);
                    
                    dbContext.SaveChanges();
                }
            }
            catch(Exception e){
                throw new Exception($"There was a problem in Database use- Add employee + {e}");
            }
            

        }

        public void Delete(int id)
        {
            try{
                lock (Lock)
                {
                    var dbContext = DBcontext.GetInstance();
                    var dbMember = dbContext.Employees.Find(id);
                    if(dbMember is not null) {
                        if (Employees.ContainsKey(id))
                        {
                            Employee employee = Employees[id];
                           
                            Employees.Remove(id);
                        }

                        dbContext.Employees.Remove(dbMember);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Delete employee");
            }
            
        }
        public List<Employee> GetAll()
        {
            Load();
            return Employees.Values.ToList();
        }

        public Employee GetById(int id)
        {
            if (Employees.ContainsKey(id))
                return Employees[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    EmployeeDTO mDto = dbContext.Employees.Find(id);
                    if (mDto != null)
                    {
                        LoadEmployee(mDto);
                        return Employees[id];
                    }
                    throw new ArgumentException("Invalid user ID.");
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get Member");
                }
            }
        }

        public Employee GetByUserName(string userName)
        {
            EmployeesNames = Employees.ToDictionary(x => x.Value.UserName, x => x.Value);
            if (EmployeesNames.ContainsKey(userName))
                return EmployeesNames[userName];
            else
            {
                try{
                    EmployeeDTO empDto;
                    lock(Lock){
                        var dbContext = DBcontext.GetInstance();
                        empDto = dbContext.Employees.FirstOrDefault(m => m.UserName == userName);
                    }
                    if (empDto != null)
                    {
                        LoadEmployee(empDto);
                        return EmployeesNames[userName];
                    }
                    else
                    {
                        throw new ArgumentException("Invalid user name.");
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get Member");
                }
            }
        }

        public void Update(Employee employee)
        {
            if (ContainsValue(employee))
            {
                Employees[employee.EmployeeId] = employee;
                
                try{
                    lock (Lock)
                    {
                        EmployeeDTO p = DBcontext.GetInstance().Employees.Find(employee.EmployeeId);
                        if (p != null){
                            p.Password = employee.Password;
                            if (employee.Alerts != null) {
                                List<MessageDTO> Alerts = new List<MessageDTO>();
                                foreach (var message in employee.Alerts)
                                {
                                    Alerts.Add(new MessageDTO(message));
                                }
                                p.Alerts = Alerts;
                            }
                            p.Alert = employee.Alert;
                            if(employee.ForeignLanguages != null){
                                p.ForeignLanguages = new List<EmployeeLanguagesDTO>();
                                foreach (var language in employee.ForeignLanguages)
                                {
                                    var employeeLanguage = Enums.GetValueById<Enums.Languages>(language.Value.LanguageID);
                                    p.ForeignLanguages.Add(new(language.Value));
                                }
                            }
                            p.IsManager = employee.IsManager;
                            DBcontext.GetInstance().SaveChanges();
                        }
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Update employee");
                }
                
            }
            else{
                throw new KeyNotFoundException($"employee with ID {employee.EmployeeId} not found.");
            }
        }
     

       

        public bool ContainsID(int id)
        {
            return Employees.ContainsKey(id);
        }

        public bool ContainsValue(Employee employee)
        {
            return Employees.ContainsKey(employee.EmployeeId);
        }

        public void Clear()
        {
            Employees.Clear();

        }        

        public void ResetDomainData()
        {
            Employees = new Dictionary<int, Employee>();
            
        }

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<EmployeeDTO> employees = dbContext.Employees.Include(e => e.ForeignLanguages)
        .Include(e => e.Skills)
        .ToList();
            foreach (EmployeeDTO employee in employees)
            {
                Employees.TryAdd(employee.EmployeeId, new Employee(employee));
                
            }
        }

        private void LoadEmployee(EmployeeDTO employeeDto)
        {
            Employee employee = new Employee(employeeDto);
            Employees[employee.EmployeeId] = employee;
            
        }
    }
}