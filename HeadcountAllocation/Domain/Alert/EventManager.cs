
using System.Collections.Concurrent;
using HeadcountAllocation.DAL;
using HeadcountAllocation.DAL.DTO;
using HeadcountAllocation.DAL.DTO.Alert;
using HeadcountAllocation.DAL.Repositories;
using HeadcountAllocation.Domain.Notification;

namespace HeadcountAllocation.Domain.Alert
{
    public class EventManager
    {
        private int _projectId;
        public ConcurrentDictionary<string, SynchronizedCollection<User>> _listeners {get; set; }

        public EventManager(int shopID){
            _projectId = shopID;
            _listeners = new ConcurrentDictionary<string, SynchronizedCollection<User>>();
            _listeners.TryAdd("Employee Reported Leave", new SynchronizedCollection<User>());            
            _listeners.TryAdd("Message Event", new SynchronizedCollection<User>());
            // UploadEventsFromContext();
        }

        private void UploadEventsFromContext()
        {
            DBcontext context = DBcontext.GetInstance();
            List<EventDTO> events =  context.Events.Where((e) => e.ProjectId == _projectId).ToList();
            List<EmployeeDTO> users = context.Employees.ToList();
            foreach(EventDTO e in events)
            {
                _listeners[e.Name].Add(EmployeeRepo.GetInstance().GetById(e.Listener.EmployeeId));
            }
            foreach(EmployeeDTO user in users)
            {
                User member = EmployeeRepo.GetInstance().GetById(user.EmployeeId);
                if (!_listeners["Report Event"].Contains(member))
                    _listeners["Report Event"].Add(member);
            }
        }

        public void Subscribe(User user, Event e)
        {
            if (!_listeners[e.Name].Contains(user))
            {
                _listeners[e.Name].Add(user);
                DBcontext.GetInstance().Events.Add(new EventDTO(e.Name, DBcontext.GetInstance().Employees.Find(new EmployeeDTO((Employee)user).EmployeeId)));
                DBcontext.GetInstance().SaveChanges();
            }
            else throw new Exception("User already sign to this event.");
        }

        public void Unsubscribe(User user, Event e)
        {
            if (_listeners[e.Name].Contains(user))
            {
                _listeners[e.Name].Remove(user);
                EventDTO eventDTO = DBcontext.GetInstance().Events
                    .Where((e)=>e.Listener.Id == ((Employee)user).EmployeeId && e.ProjectId == _projectId).FirstOrDefault();
                DBcontext.GetInstance().Events.Remove(eventDTO);
                DBcontext.GetInstance().SaveChanges();
            }
            else throw new Exception("User already not sign to this event.");
        }

        public void SubscribeToAll(User user)
        {
            List<string> events = _listeners.Keys.ToList<string>();
            foreach (string eventName in events)
            {
                _listeners[eventName].Add(user);
                DBcontext.GetInstance().Events.Add(new EventDTO(eventName, DBcontext.GetInstance().Employees.Find(new EmployeeDTO((Employee)user).EmployeeId)));
                DBcontext.GetInstance().SaveChanges();
            }
        }

        public void UnsubscribeToAll(User user)
        {
            foreach (string eventName in _listeners.Keys)
            {
                if (_listeners[eventName].Contains(user))
                {
                    _listeners[eventName].Remove(user);
                    EventDTO eventDTO = DBcontext.GetInstance().Events
                        .Where((e) => e.Listener.Id == ((Employee)user).EmployeeId && e.StoreId == _projectId).FirstOrDefault();
                    DBcontext.GetInstance().Events.Remove(eventDTO);
                }
            }
        }

        public void NotifySubscribers(Event e)
        {
            foreach (User user in _listeners[e.Name])
            {
                e.Update(user);
            }
        }

    }
}