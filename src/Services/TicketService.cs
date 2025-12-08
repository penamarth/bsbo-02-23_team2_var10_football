using System;
using System.Collections.Generic;
using System.Linq;
using FootballTicketSystem.Models;

namespace FootballTicketSystem.Services
{
    public class TicketService
    {
        private List<Ticket> tickets;
        private List<Reservation> reservations;
        private List<SeasonTicket> seasonTickets;

        public TicketService()
        {
            tickets = new List<Ticket>();
            reservations = new List<Reservation>();
            seasonTickets = new List<SeasonTicket>();
        }

        public Ticket BuyTicket(Match match, Seat seat, Customer customer, decimal price)
        {
            if (!seat.IsAvailable)
                throw new InvalidOperationException("Место недоступно для покупки");

            var ticket = new Ticket(match, seat, customer, price);
            seat.IsAvailable = false;
            tickets.Add(ticket);
            
            Console.WriteLine($"✓ Билет {ticket.TicketId} успешно создан");
            return ticket;
        }

        public bool ReturnTicket(string ticketId)
        {
            var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket == null)
                return false;

            if (ticket.Cancel())
            {
                Console.WriteLine($"✓ Билет {ticketId} успешно возвращен");
                return true;
            }

            return false;
        }

        public Reservation ReserveTicket(Match match, Seat seat, Customer customer)
        {
            if (!seat.IsAvailable)
                throw new InvalidOperationException("Место недоступно для бронирования");

            var reservation = new Reservation(seat, match, customer);
            seat.IsAvailable = false;
            reservations.Add(reservation);
            
            Console.WriteLine($"✓ Бронирование {reservation.ReservationId} создано до {reservation.ExpiryDate:dd.MM.yyyy HH:mm}");
            return reservation;
        }

        public List<Seat> GetAvailableSeats(Match match)
        {
            var availableSeats = new List<Seat>();
            foreach (var section in match.Stadium.Sections)
            {
                availableSeats.AddRange(section.GetAvailableSeats());
            }
            return availableSeats;
        }

        public SeasonTicket BuySeasonTicket(string season, Seat seat, Customer customer, List<Match> matches, decimal price)
        {
            var seasonTicket = new SeasonTicket(season, seat, customer, matches, price);
            seasonTickets.Add(seasonTicket);
            
            Console.WriteLine($"✓ Абонемент {seasonTicket.SeasonTicketId} успешно создан");
            return seasonTicket;
        }

        public List<Ticket> GetAllTickets() => tickets;
        public List<Reservation> GetAllReservations() => reservations;
        public List<SeasonTicket> GetAllSeasonTickets() => seasonTickets;
    }
}
