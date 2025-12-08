using System;

namespace FootballTicketSystem.Models
{
    public class Reservation
    {
        public string ReservationId { get; private set; }
        public Seat Seat { get; private set; }
        public Match Match { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        public Reservation(Seat seat, Match match, Customer customer)
        {
            ReservationId = $"R-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            Seat = seat ?? throw new ArgumentNullException(nameof(seat));
            Match = match ?? throw new ArgumentNullException(nameof(match));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            ExpiryDate = DateTime.Now.AddHours(24);
        }

        public bool IsExpired()
        {
            return DateTime.Now > ExpiryDate;
        }

        public Ticket ConvertToTicket(decimal price)
        {
            if (IsExpired())
                throw new InvalidOperationException("Бронирование истекло");

            return new Ticket(Match, Seat, Customer, price);
        }
    }
}
