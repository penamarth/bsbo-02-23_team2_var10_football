using System;
using System.Linq;
using FootballTicketSystem.Models;
using FootballTicketSystem.Patterns.Facade;
using FootballTicketSystem.Patterns.Observer;
using FootballTicketSystem.Services;

namespace FootballTicketSystem
{
    class Program
    {
        static TicketSystemFacade facade;
        static NotificationService notificationService;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("СИСТЕМА ПРОДАЖИ БИЛЕТОВ НА ФУТБОЛ");
            Console.WriteLine();

            facade = new TicketSystemFacade();
            notificationService = new NotificationService();
            
            // Подключаем наблюдателей (кассы)
            notificationService.Attach(new SeatAvailabilityObserver("1"));
            notificationService.Attach(new SeatAvailabilityObserver("2"));

            bool exit = false;
            while (!exit)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BuyTicketProcess();
                        break;
                    case "2":
                        ReturnTicketProcess();
                        break;
                    case "3":
                        BuySeasonTicketProcess();
                        break;
                    case "4":
                        ReserveTicketProcess();
                        break;
                    case "5":
                        ViewAvailableSeatsProcess();
                        break;
                    case "6":
                        ViewAllMatches();
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine("│          ГЛАВНОЕ МЕНЮ                  │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine("│ 1. Купить билет                        │");
            Console.WriteLine("│ 2. Вернуть билет                       │");
            Console.WriteLine("│ 3. Купить абонемент                    │");
            Console.WriteLine("│ 4. Забронировать билет                 │");
            Console.WriteLine("│ 5. Посмотреть доступные места          │");
            Console.WriteLine("│ 6. Список матчей                       │");
            Console.WriteLine("│ 0. Выход                               │");
            Console.WriteLine("└────────────────────────────────────────┘");
            Console.Write("Выберите действие: ");
        }

        static void BuyTicketProcess()
        {
            Console.WriteLine("\nПОКУПКА БИЛЕТА\n");

            var matches = facade.GetAllMatches();
            if (matches.Count == 0)
            {
                Console.WriteLine("Нет доступных матчей");
                return;
            }

            Console.WriteLine("Доступные матчи:");
            for (int i = 0; i < matches.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {matches[i].GetMatchInfo()}");
            }

            Console.Write("\nВыберите номер матча: ");
            if (!int.TryParse(Console.ReadLine(), out int matchIndex) || matchIndex < 1 || matchIndex > matches.Count)
            {
                Console.WriteLine("Неверный номер матча");
                return;
            }

            var selectedMatch = matches[matchIndex - 1];
            var availableSeats = facade.ViewAvailableSeats(selectedMatch.MatchId);

            if (availableSeats.Count == 0)
            {
                Console.WriteLine("Нет доступных мест на этот матч");
                return;
            }

            Console.WriteLine($"\nДоступно мест: {availableSeats.Count}");
            Console.WriteLine("Примеры доступных мест:");
            for (int i = 0; i < Math.Min(10, availableSeats.Count); i++)
            {
                Console.WriteLine($"{i + 1}. {availableSeats[i].GetFullNumber()} [{availableSeats[i].Category}]");
            }

            Console.Write("\nВведите ID места (например, A-1-5): ");
            string seatId = Console.ReadLine();

            Console.Write("Введите ФИО покупателя: ");
            string fullName = Console.ReadLine();

            Console.Write("Введите телефон: ");
            string phone = Console.ReadLine();

            var customer = new Customer(fullName, phone);

            Console.WriteLine("\nСпособ оплаты:");
            Console.WriteLine("1. Наличные");
            Console.WriteLine("2. Карта");
            Console.Write("Выберите: ");

            PaymentMethod paymentMethod = Console.ReadLine() == "1" ? PaymentMethod.Cash : PaymentMethod.Card;

            try
            {
                var ticket = facade.BuyTicket(selectedMatch.MatchId, seatId, customer, paymentMethod);
                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine(ticket.GetInfo());
                Console.WriteLine(new string('=', 50));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        static void ReturnTicketProcess()
        {
            Console.WriteLine("\n=== ВОЗВРАТ БИЛЕТА ===\n");
            Console.Write("Введите номер билета: ");
            string ticketId = Console.ReadLine();

            if (facade.ReturnTicket(ticketId))
            {
                Console.WriteLine("Билет успешно возвращен, возврат 90% стоимости");
            }
            else
            {
                Console.WriteLine("Не удалось вернуть билет (билет не найден или возврат невозможен)");
            }
        }

        static void BuySeasonTicketProcess()
        {
            Console.WriteLine("\n=== ПОКУПКА АБОНЕМЕНТА ===\n");
            Console.WriteLine("Доступные сектора:");

            var matches = facade.GetAllMatches();
            if (matches.Count > 0)
            {
                var firstMatch = matches[0];
                var availableSeats = facade.ViewAvailableSeats(firstMatch.MatchId);

                if (availableSeats.Count == 0)
                {
                    Console.WriteLine("Нет доступных мест");
                    return;
                }

                Console.WriteLine("Примеры доступных мест:");
                for (int i = 0; i < Math.Min(10, availableSeats.Count); i++)
                {
                    Console.WriteLine($"{i + 1}. {availableSeats[i].GetFullNumber()} [{availableSeats[i].Category}]");
                }
            }

            Console.Write("\nВведите ID места для абонемента (например, A-1-5): ");
            string seatId = Console.ReadLine();

            Console.Write("Введите ФИО покупателя: ");
            string fullName = Console.ReadLine();

            Console.Write("Введите телефон: ");
            string phone = Console.ReadLine();

            var customer = new Customer(fullName, phone);

            Console.WriteLine("\nСпособ оплаты:");
            Console.WriteLine("1. Наличные");
            Console.WriteLine("2. Карта");
            Console.Write("Выберите: ");

            PaymentMethod paymentMethod = Console.ReadLine() == "1" ? PaymentMethod.Cash : PaymentMethod.Card;

            try
            {
                var seasonTicket = facade.BuySeasonTicket("2024-2025", seatId, customer, paymentMethod);
                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine(seasonTicket.GetInfo());
                Console.WriteLine(new string('=', 50));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        static void ReserveTicketProcess()
        {
            Console.WriteLine("\nБРОНИРОВАНИЕ БИЛЕТА\n");

            var matches = facade.GetAllMatches();
            Console.WriteLine("Доступные матчи:");
            for (int i = 0; i < matches.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {matches[i].GetMatchInfo()}");
            }

            Console.Write("\nВыберите номер матча: ");
            if (!int.TryParse(Console.ReadLine(), out int matchIndex) || matchIndex < 1 || matchIndex > matches.Count)
            {
                Console.WriteLine("Неверный номер матча");
                return;
            }

            var selectedMatch = matches[matchIndex - 1];
            var availableSeats = facade.ViewAvailableSeats(selectedMatch.MatchId);

            Console.WriteLine($"\nДоступно мест: {availableSeats.Count}");
            Console.WriteLine("Примеры:");
            for (int i = 0; i < Math.Min(10, availableSeats.Count); i++)
            {
                Console.WriteLine($"{i + 1}. {availableSeats[i].GetFullNumber()}");
            }

            Console.Write("\nВведите ID места: ");
            string seatId = Console.ReadLine();

            Console.Write("Введите ФИО: ");
            string fullName = Console.ReadLine();

            Console.Write("Введите телефон: ");
            string phone = Console.ReadLine();

            var customer = new Customer(fullName, phone);

            try
            {
                var reservation = facade.ReserveTicket(selectedMatch.MatchId, seatId, customer);
                Console.WriteLine($"\nБронирование {reservation.ReservationId} создано");
                Console.WriteLine($"Действительно до: {reservation.ExpiryDate:dd.MM.yyyy HH:mm}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        static void ViewAvailableSeatsProcess()
        {
            Console.WriteLine("\nПРОСМОТР ДОСТУПНЫХ МЕСТ\n");

            var matches = facade.GetAllMatches();
            Console.WriteLine("Выберите матч:");
            for (int i = 0; i < matches.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {matches[i].GetMatchInfo()}");
            }

            Console.Write("\nНомер матча: ");
            if (!int.TryParse(Console.ReadLine(), out int matchIndex) || matchIndex < 1 || matchIndex > matches.Count)
            {
                Console.WriteLine("Неверный номер");
                return;
            }

            var selectedMatch = matches[matchIndex - 1];
            var availableSeats = facade.ViewAvailableSeats(selectedMatch.MatchId);

            Console.WriteLine($"\nДоступно мест: {availableSeats.Count}");
            
            var vipSeats = availableSeats.Count(s => s.Category == SeatCategory.VIP);
            var standardSeats = availableSeats.Count(s => s.Category == SeatCategory.Standard);
            var standingSeats = availableSeats.Count(s => s.Category == SeatCategory.Standing);

            Console.WriteLine($"VIP: {vipSeats} мест");
            Console.WriteLine($"Стандарт: {standardSeats} мест");
            Console.WriteLine($"Стоячие: {standingSeats} мест");
        }

        static void ViewAllMatches()
        {
            Console.WriteLine("\nСПИСОК МАТЧЕЙ\n");
            var matches = facade.GetAllMatches();
            
            for (int i = 0; i < matches.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {matches[i].GetMatchInfo()}");
            }
        }
    }
}
