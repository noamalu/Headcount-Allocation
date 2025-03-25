
using System.Collections.Concurrent;
using HeadcountAllocation.DAL;

namespace HeadcountAllocation.Domain.Notification
{
    public class EventManager
    {
        private int _shopId;
        private ConcurrentDictionary<string, SynchronizedCollection<Project>> _listeners;

        public ConcurrentDictionary<string, SynchronizedCollection<Project>> Listeners { get => _listeners; set => _listeners = value; }

        public EventManager(int shopID){
            _shopId = shopID;
            _listeners = new ConcurrentDictionary<string, SynchronizedCollection<Project>>();
            _listeners.TryAdd("Product Sell Event", new SynchronizedCollection<Project>());
            _listeners.TryAdd("Remove Appointment Event", new SynchronizedCollection<Project>());
            _listeners.TryAdd("Add Appointment Event", new SynchronizedCollection<Project>());
            _listeners.TryAdd("Store Closed Event", new SynchronizedCollection<Project>());
            _listeners.TryAdd("Store Open Event", new SynchronizedCollection<Project>());
            _listeners.TryAdd("Message Event", new SynchronizedCollection<Project>());
            // UploadEventsFromContext();
        }

        private void UploadEventsFromContext()
        {
            DBcontext context = DBcontext.GetInstance();
            // List<EventDTO> events =  context.Events.Where((e) => e.StoreId == _shopId).ToList();
            // List<MemberDTO> members = context.Members.Where((m)=>m.IsSystemAdmin==true).ToList();
            // foreach(EventDTO e in events)
            // {
            //     _listeners[e.Name].Add(ClientRepositoryRAM.GetInstance().GetById(e.Listener.Id));
            // }
            // foreach(MemberDTO m in members)
            // {
            //     Member member = ClientRepositoryRAM.GetInstance().GetById(m.Id);
            //     if (!_listeners["Report Event"].Contains(member))
            //         _listeners["Report Event"].Add(member);
            // }
        }

        // public void Subscribe(Member member, Event e)
        // {
        //     if (!_listeners[e.Name].Contains(member))
        //     {
        //         _listeners[e.Name].Add(member);
        //         DBcontext.GetInstance().Events.Add(new EventDTO(e.Name, _shopId, DBcontext.GetInstance().Members.Find(member.Id)));
        //         DBcontext.GetInstance().SaveChanges();
        //     }
        //     else throw new Exception("User already sign to this event.");
        // }

        // public void Unsubscribe(Member member, Event e)
        // {
        //     if (_listeners[e.Name].Contains(member))
        //     {
        //         _listeners[e.Name].Remove(member);
        //         EventDTO eventDTO = DBcontext.GetInstance().Events
        //             .Where((e)=>e.Listener.Id == member.Id && e.StoreId==_shopId).FirstOrDefault();
        //         DBcontext.GetInstance().Events.Remove(eventDTO);
        //         DBcontext.GetInstance().SaveChanges();
        //     }
        //     else throw new Exception("User already not sign to this event.");
        // }

        // public void SubscribeToAll(Member member)
        // {
        //     List<string> events = _listeners.Keys.ToList<string>();
        //     foreach (string eventName in events)
        //     {
        //         _listeners[eventName].Add(member);
        //         DBcontext.GetInstance().Events.Add(new EventDTO(eventName, _shopId, DBcontext.GetInstance().Members.Find(member.Id)));
        //         DBcontext.GetInstance().SaveChanges();
        //     }
        // }

        // public void UnsubscribeToAll(Member member)
        // {
        //     foreach (string eventName in _listeners.Keys)
        //     {
        //         if (_listeners[eventName].Contains(member))
        //         {
        //             _listeners[eventName].Remove(member);
        //             EventDTO eventDTO = DBcontext.GetInstance().Events
        //                 .Where((e) => e.Listener.Id == member.Id && e.StoreId == _shopId).FirstOrDefault();
        //             DBcontext.GetInstance().Events.Remove(eventDTO);
        //         }
        //     }
        // }

        // public void NotifySubscribers(Event e)
        // {
        //     foreach (Member mamber in _listeners[e.Name])
        //     {
        //         e.Update(mamber);
        //     }
        // }

    }
}