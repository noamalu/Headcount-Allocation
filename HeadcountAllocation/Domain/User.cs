
using System.Net.Mail;
using HeadcountAllocation.Domain.Alert;

namespace HeadcountAllocation.Domain{
    public class User{

        public string UserName{get; set;}

        public string Password{get; set;}

        public MailAddress Email{get; set;}
        public bool Alert{get; set;}
        public List<Message> Alerts {get; set;} = new ();
        public AlertManager _alertManager = AlertManager.GetInstance();


        public void Notify(string msg)
        {
            var message = new Message(msg);

            if (Alert)
            {
                _alertManager.SendAlert(msg, UserName);
                message.Seen = true;
            }
            else{
                Alerts.Add(message);            
            }        
        }
    }
}