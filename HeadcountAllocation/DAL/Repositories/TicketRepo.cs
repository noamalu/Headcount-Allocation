using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.Domain;

namespace HeadcountAllocation.DAL.Repositories
{
    public class TicketRepo
    {
        private static Dictionary<int, Ticket> Tickets;

        private object Lock;

        private static TicketRepo _ticketRepo = null;

        private TicketRepo()
        {
            Tickets = new Dictionary<int, Ticket>();
            Lock = new object();
        }
        public static TicketRepo GetInstance()
        {
            _ticketRepo ??= new TicketRepo();
            return _ticketRepo;
        }

        public static void Dispose(){
            _ticketRepo = new TicketRepo();
        }

        public IEnumerable<Ticket> getAll()
        {
            Load();
            return Tickets.Values;
        }

        public void Delete(Ticket ticket)
        {
            Delete(ticket.TicketId);
        }

        public void Add(Ticket ticket)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            Tickets.Add(ticket.TicketId, ticket);
            try{
                lock (Lock)
                {
                    dbContext.Tickets.Add(new TicketDTO(ticket));
                    
                    dbContext.SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Add ticket");
            }
            

        }

        public void Delete(int id)
        {
            try{
                lock (Lock)
                {
                    var dbContext = DBcontext.GetInstance();
                    var dbMember = dbContext.Tickets.Find(id);
                    if(dbMember is not null) {
                        if (Tickets.ContainsKey(id))
                        {
                            Ticket ticket = Tickets[id];
                           
                            Tickets.Remove(id);
                        }

                        dbContext.Tickets.Remove(dbMember);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Delete employee");
            }
            
        }
        public List<Ticket> GetAll()
        {
            Load();
            return Tickets.Values.ToList();
        }

        public Ticket GetById(int id)
        {
            if (Tickets.ContainsKey(id))
                return Tickets[id];
            else
            {
                try{
                    var dbContext = DBcontext.GetInstance();
                    TicketDTO mDto = dbContext.Tickets.Find(id);
                    if (mDto != null)
                    {
                        LoadTicket(mDto);
                        return Tickets[id];
                    }
                    throw new ArgumentException("Invalid ticket ID.");
                }
                catch(Exception){
                throw new Exception("There was a problem in Database use- Get ticket");
                }
            }
        }         
     

       

        public bool ContainsID(int id)
        {
            return Tickets.ContainsKey(id);
        }

        public bool ContainsValue(Ticket ticket)
        {
            return Tickets.ContainsKey(ticket.TicketId);
        }

        public void Clear()
        {
            Tickets.Clear();

        }        

        public void ResetDomainData()
        {
            Tickets = new Dictionary<int, Ticket>();
            
        }

        private void Load()
        {
            var dbContext = DBcontext.GetInstance();
            List<TicketDTO> tickets = dbContext.Tickets.ToList();
            foreach (TicketDTO ticket in tickets)
            {
                Tickets.TryAdd(ticket.TicketId, new Ticket(ticket));
                
            }
        }

        private void LoadTicket(TicketDTO ticketDto)
        {
            Ticket ticket = new Ticket(ticketDto);
            Tickets[ticket.TicketId] = ticket;
            
        }

        public void Update(Ticket ticket)
        {
            try{
                Tickets[ticket.TicketId] = ticket;
                lock(Lock){
                    TicketDTO ticketDTO = DBcontext.GetInstance().Tickets.Find(ticket.TicketId);
                    TicketDTO newTicket = new TicketDTO(ticket);
                    if (ticketDTO != null)
                    {
                        ticketDTO.TicketId = newTicket.TicketId;
                        ticketDTO.EmployeeId = newTicket.EmployeeId;
                        ticketDTO.EmployeeName = newTicket.EmployeeName;
                        ticketDTO.StartDate = newTicket.StartDate;
                        ticketDTO.EndDate = newTicket.EndDate;
                        ticketDTO.Description = newTicket.Description;
                        ticketDTO.Open = newTicket.Open;
                    }
                    else DBcontext.GetInstance().Tickets.Add(newTicket);
                    DBcontext.GetInstance().SaveChanges();
                }
            }
            catch(Exception){
                throw new Exception("There was a problem in Database use- Update Role");
            }
            
        }
    }
}