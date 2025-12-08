using System;
using System.Collections.Generic;
using FootballTicketSystem.Patterns.Observer;

namespace FootballTicketSystem.Services
{
    public class NotificationService
    {
        private List<IObserver> observers;

        public NotificationService()
        {
            observers = new List<IObserver>();
        }

        public void Attach(IObserver observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in observers)
            {
                observer.Update(message);
            }
        }
    }
}
