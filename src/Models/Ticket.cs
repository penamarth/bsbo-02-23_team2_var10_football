using System;

namespace FootballTicketSystem.Models
{
    public enum TicketStatus
    {
        Active,
        Cancelled,
        Used
    }

    public class Ticket
    {
        public string TicketId { get; private set; }
        public Match Match { get; private set; }
        public Seat Seat { get; private set; }
        public Customer Customer { get; private set; }
        public decimal Price { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public TicketStatus Status { get; set; }

        public Ticket(Match match, Seat seat, Customer customer, decimal price)
        {
            TicketId = $"T-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            Match = match ?? throw new ArgumentNullException(nameof(match));
            Seat = seat ?? throw new ArgumentNullException(nameof(seat));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Price = price;
            PurchaseDate = DateTime.Now;
            Status = TicketStatus.Active;
        }

        public string GetInfo()
        {
            return $"Билет #{TicketId}\n" +
                   $"Матч: {Match.GetMatchInfo()}\n" +
                   $"Место: {Seat.GetFullNumber()}\n" +
                   $"Владелец: {Customer.FullName}\n" +
                   $"Цена: {Price} руб.\n" +
                   $"Дата покупки: {PurchaseDate:dd.MM.yyyy HH:mm}\n" +
                   $"Статус: {Status}";
        }

        public bool Cancel()
        {
            if (Status != TicketStatus.Active)
                return false;

            // Проверка: можно отменить только за 24 часа до матча
            if ((Match.DateTime - DateTime.Now).TotalHours < 24)
                return false;

            Status = TicketStatus.Cancelled;
            Seat.IsAvailable = true;
            return true;
        }
    }
}
