using System;
using System.Collections.Generic;
using System.Linq;
using FootballTicketSystem.Models;
using FootballTicketSystem.Services;
using FootballTicketSystem.Patterns.Strategy;
using FootballTicketSystem.Patterns.Singleton;

namespace FootballTicketSystem.Patterns.Facade
{
    public class TicketSystemFacade
    {
        private TicketService ticketService;
        private PaymentService paymentService;
        private NotificationService notificationService;
        private IPricingStrategy pricingStrategy;
        private TicketSystemInstance systemInstance;

        public TicketSystemFacade()
        {
            systemInstance = TicketSystemInstance.GetInstance();
            ticketService = systemInstance.TicketService;
            paymentService = new PaymentService();
            notificationService = new NotificationService();
            pricingStrategy = new StandardPricing();
        }

        public void SetPricingStrategy(IPricingStrategy strategy)
        {
            pricingStrategy = strategy;
        }

        public Ticket BuyTicket(string matchId, string seatId, Customer customer, PaymentMethod paymentMethod)
        {
            try
            {
                var match = systemInstance.GetMatch(matchId);
                if (match == null)
                    throw new ArgumentException("Матч не найден");

                var seat = FindSeat(seatId);
                if (seat == null)
                    throw new ArgumentException("Место не найдено");

                if (!seat.IsAvailable)
                    throw new InvalidOperationException("Место уже занято");

                decimal price = pricingStrategy.CalculatePrice(1000m, seat.Category);
                
                var payment = paymentService.ProcessPayment(price, paymentMethod);
                
                var ticket = ticketService.BuyTicket(match, seat, customer, price);
                
                notificationService.Notify($"Место {seat.GetFullNumber()} продано на матч {match.GetMatchInfo()}");
                
                return ticket;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при покупке билета: {ex.Message}");
                throw;
            }
        }

        public bool ReturnTicket(string ticketId)
        {
            try
            {
                var ticket = ticketService.GetAllTickets().FirstOrDefault(t => t.TicketId == ticketId);
                if (ticket == null)
                    throw new ArgumentException("Билет не найден");

                if (ticketService.ReturnTicket(ticketId))
                {
                    paymentService.RefundPayment(new Payment(ticket.Price, PaymentMethod.Cash));
                    notificationService.Notify($"Билет {ticketId} возвращен, место {ticket.Seat.GetFullNumber()} снова доступно");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при возврате билета: {ex.Message}");
                return false;
            }
        }

        public SeasonTicket BuySeasonTicket(string season, string seatId, Customer customer, PaymentMethod paymentMethod)
        {
            try
            {
                var seat = FindSeat(seatId);
                if (seat == null)
                    throw new ArgumentException("Место не найдено");

                var matches = systemInstance.Matches;
                decimal price = pricingStrategy.CalculatePrice(10000m, seat.Category);
                
                var payment = paymentService.ProcessPayment(price, paymentMethod);
                
                var seasonTicket = ticketService.BuySeasonTicket(season, seat, customer, matches, price);
                
                notificationService.Notify($"Абонемент на место {seat.GetFullNumber()} продан");
                
                return seasonTicket;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при покупке абонемента: {ex.Message}");
                throw;
            }
        }

        public Reservation ReserveTicket(string matchId, string seatId, Customer customer)
        {
            try
            {
                var match = systemInstance.GetMatch(matchId);
                if (match == null)
                    throw new ArgumentException("Матч не найден");

                var seat = FindSeat(seatId);
                if (seat == null)
                    throw new ArgumentException("Место не найдено");

                var reservation = ticketService.ReserveTicket(match, seat, customer);
                
                notificationService.Notify($"Место {seat.GetFullNumber()} забронировано на матч {match.GetMatchInfo()}");
                
                return reservation;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при бронировании: {ex.Message}");
                throw;
            }
        }

        public List<Seat> ViewAvailableSeats(string matchId)
        {
            var match = systemInstance.GetMatch(matchId);
            if (match == null)
                return new List<Seat>();

            return ticketService.GetAvailableSeats(match);
        }

        public List<Match> GetAllMatches()
        {
            return systemInstance.Matches;
        }

        private Seat FindSeat(string seatId)
        {
            foreach (var section in systemInstance.Stadium.Sections)
            {
                var seat = section.Seats.FirstOrDefault(s => s.SeatId == seatId);
                if (seat != null)
                    return seat;
            }
            return null;
        }
    }
}
