using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class EmployeeRepo
    {
         private static Dictionary<int, Employee> Employees;

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
                    dbContext.Employees.Add(new EmployeeDTO(employee));
                    
                    dbContext.SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Add employee");
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

        // public void Update(Employee employee)
        // {
        //     if (ContainsValue(employee))
        //     {
        //         Employees[employee.EmployeeId] = employee;
                
        //         try{
        //             lock (Lock)
        //             {
        //                 EmployeeDTO p = DBcontext.GetInstance().Employees.Find(employee.EmployeeId);
        //                 if (p != null){
        //                     p.Password = employee.Password;
        //                     if (employee.alerts != null) {
        //                         List<MessageDTO> Alerts = new List<MessageDTO>();
        //                         foreach (Message message in item.alerts)
        //                         {
        //                             Alerts.Add(new MessageDTO(message));
        //                         }
        //                         p.Alerts = Alerts;
        //                     }
        //                     p.IsNotification = item.IsNotification;
        //                     if (item.OrderHistory != null){
        //                         List<ShoppingCartHistoryDTO> OrderHistory = new ();
        //                         foreach (var order in item.OrderHistory.Values.ToList())
        //                         OrderHistory.Add(new ShoppingCartHistoryDTO(order));
        //                         p.OrderHistory = OrderHistory;
        //                     }
        //                     p.IsSystemAdmin = item.IsSystemAdmin;
        //                     DBcontext.GetInstance().SaveChanges();
        //                 }
        //             }
        //         }
        //         catch(Exception){
        //         throw new Exception("There was a problem in Database use- Update Member");
        //         }
                
        //     }
        //     else{
        //         throw new KeyNotFoundException($"Client with ID {item.Id} not found.");
        //     }
        // }
     

       

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
            List<EmployeeDTO> employees = dbContext.Employees.ToList();
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