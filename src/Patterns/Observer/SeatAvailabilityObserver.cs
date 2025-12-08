using System;

namespace FootballTicketSystem.Patterns.Observer
{
    public class SeatAvailabilityObserver : IObserver
    {
        private string cashierId;

        public SeatAvailabilityObserver(string cashierId)
        {
            this.cashierId = cashierId;
        }

        public void Update(string message)
        {
            Console.WriteLine($"[Касса {cashierId}] Получено уведомление: {message}");
        }
    }
}
