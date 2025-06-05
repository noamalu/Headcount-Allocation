using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class TicketReasonsRepo
    {
        private static Dictionary<int, Reason> TicketReasons;

        private object Lock;

        private static TicketReasonsRepo _ticketReasonsRepo = null;

        private TicketReasonsRepo()
        {
            TicketReasons = new();
            Lock = new object();
        }
        public static TicketReasonsRepo GetInstance()
        {
            _ticketReasonsRepo ??= new TicketReasonsRepo();
            return _ticketReasonsRepo;
        }

        public static void Dispose(){
            _ticketReasonsRepo = new TicketReasonsRepo();
        }

        public IEnumerable<Reason> getAll()
        {
            Load();
            return TicketReasons.Values;
        }

        public void Delete(Reason reason)
        {
            Delete(reason.ReasonId);
        }

        public void Add(Reason reason)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            TicketReasons.Add(reason.ReasonId, reason);
            try{
                lock (Lock)
                {
                    dbContext.TicketReasons.Add(new TicketReasonsDTO(reason));
                    
                    dbContext.SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Add reason");
            }
            

        }

        public void Delete(int id)
        {
            try{
                lock (Lock)
                {
                    var dbContext = DBcontext.GetInstance();
                    var dbTicketReason = dbContext.TicketReasons.Find(id);
                    if(dbTicketReason is not null) {
                        if (TicketReasons.ContainsKey(id))
                        {
                            Reason reason = TicketReasons[id];
                           
                            TicketReasons.Remove(id);
                        }

                        dbContext.TicketReasons.Remove(dbTicketReason);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Delete reason");
            }
            
        }

        public List<Reason> GetAll()
        {
            Load();
            return TicketReasons.Values.ToList();
        }

        public Reason GetById(int id)
        {
            if (TicketReasons.ContainsKey(id))
                return TicketReasons[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    TicketReasonsDTO mDto = dbContext.TicketReasons.Find(id);
                    if (mDto != null)
                    {
                        LoadTicketReasons(mDto);
                        return TicketReasons[id];
                    }
                    throw new ArgumentException("Invalid user ID.");
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get reasons");
                }
            }
        }

        public void Update(Reason reason)
        {
            if (ContainsValue(reason))
            {
                TicketReasons[reason.ReasonId] = reason;
                
                try{
                    lock (Lock)
                    {
                        TicketReasonsDTO ticketReasons = DBcontext.GetInstance().TicketReasons.Find(reason.ReasonId);
                        if (ticketReasons != null){
                            ticketReasons.ReasonTypeId = Enums.GetId(reason.ReasonType);                    
                            DBcontext.GetInstance().SaveChanges();
                        }
                    }
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Update reason");
                }
                
            }
            else{
                throw new KeyNotFoundException($"Reason with ID {reason.ReasonId} not found.");
            }
        }
     

       

        public bool ContainsID(int id)
        {
            return TicketReasons.ContainsKey(id);
        }

        public bool ContainsValue(Reason reason)
        {
            return TicketReasons.ContainsKey(reason.ReasonId);
        }

        public void Clear()
        {
            TicketReasons.Clear();

        }        

        public void ResetDomainData()
        {
            TicketReasons = new ();
            
        }

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<TicketReasonsDTO> reasons = dbContext.TicketReasons.ToList();
            foreach (var reason in reasons)
            {
                TicketReasons.TryAdd(reason.ReasonTypeId, new Reason(reason));
                
            }
        }

        private void LoadTicketReasons(TicketReasonsDTO ticketReason)
        {
            var reason = new Reason(ticketReason);
            TicketReasons[reason.ReasonId] = reason;
            
        }
    }
}