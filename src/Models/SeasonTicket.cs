using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballTicketSystem.Models
{
    public class SeasonTicket
    {
        public string SeasonTicketId { get; private set; }
        public string Season { get; private set; }
        public Seat Seat { get; private set; }
        public Customer Customer { get; private set; }
        public decimal Price { get; private set; }
        public List<Match> Matches { get; private set; }
        public DateTime PurchaseDate { get; private set; }

        public SeasonTicket(string season, Seat seat, Customer customer, List<Match> matches, decimal price)
        {
            SeasonTicketId = $"ST-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            Season = season ?? throw new ArgumentNullException(nameof(season));
            Seat = seat ?? throw new ArgumentNullException(nameof(seat));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Matches = matches ?? throw new ArgumentNullException(nameof(matches));
            Price = price;
            PurchaseDate = DateTime.Now;
        }

        public bool IsValidForMatch(Match match)
        {
            return Matches.Any(m => m.MatchId == match.MatchId);
        }

        public string GetInfo()
        {
            return $"Абонемент #{SeasonTicketId}\n" +
                   $"Сезон: {Season}\n" +
                   $"Место: {Seat.GetFullNumber()}\n" +
                   $"Владелец: {Customer.FullName}\n" +
                   $"Матчей: {Matches.Count}\n" +
                   $"Цена: {Price} руб.\n" +
                   $"Дата покупки: {PurchaseDate:dd.MM.yyyy}";
        }
    }
}
