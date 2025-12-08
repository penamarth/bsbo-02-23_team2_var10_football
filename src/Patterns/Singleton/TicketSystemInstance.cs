using System;
using System.Collections.Generic;
using FootballTicketSystem.Models;
using FootballTicketSystem.Services;

namespace FootballTicketSystem.Patterns.Singleton
{
    public sealed class TicketSystemInstance
    {
        private static TicketSystemInstance instance = null;
        private static readonly object padlock = new object();

        public Stadium Stadium { get; private set; }
        public List<Match> Matches { get; private set; }
        public TicketService TicketService { get; private set; }

        private TicketSystemInstance()
        {
            InitializeSystem();
        }

        public static TicketSystemInstance GetInstance()
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TicketSystemInstance();
                    }
                }
            }
            return instance;
        }

        private void InitializeSystem()
        {
            Console.WriteLine("Инициализация системы продажи билетов...");
            
            // Создание стадиона
            Stadium = new Stadium("Центральный стадион");
            
            // Создание секторов
            var sectionA = new Section("A", "Сектор А (VIP)");
            var sectionB = new Section("B", "Сектор Б (Стандарт)");
            var sectionC = new Section("C", "Сектор В (Фанатский)");
            
            // Добавление мест в сектор А (VIP) - 5 рядов по 10 мест
            for (int row = 1; row <= 5; row++)
            {
                for (int seat = 1; seat <= 10; seat++)
                {
                    sectionA.AddSeat(new Seat(sectionA, row, seat, SeatCategory.VIP));
                }
            }
            
            // Добавление мест в сектор Б (Стандарт) - 10 рядов по 20 мест
            for (int row = 1; row <= 10; row++)
            {
                for (int seat = 1; seat <= 20; seat++)
                {
                    sectionB.AddSeat(new Seat(sectionB, row, seat, SeatCategory.Standard));
                }
            }
            
            // Добавление мест в сектор В (Стоячие) - 5 рядов по 30 мест
            for (int row = 1; row <= 5; row++)
            {
                for (int seat = 1; seat <= 30; seat++)
                {
                    sectionC.AddSeat(new Seat(sectionC, row, seat, SeatCategory.Standing));
                }
            }
            
            Stadium.AddSection(sectionA);
            Stadium.AddSection(sectionB);
            Stadium.AddSection(sectionC);
            
            // Создание матчей
            Matches = new List<Match>
            {
                new Match("Спартак", "ЦСКА", DateTime.Now.AddDays(7), Stadium),
                new Match("Спартак", "Зенит", DateTime.Now.AddDays(14), Stadium),
                new Match("Спартак", "Локомотив", DateTime.Now.AddDays(21), Stadium),
                new Match("Спартак", "Динамо", DateTime.Now.AddDays(28), Stadium)
            };
            
            // Инициализация сервисов
            TicketService = new TicketService();
            
            Console.WriteLine($"✓ Стадион '{Stadium.Name}' создан");
            Console.WriteLine($"✓ Вместимость: {Stadium.GetTotalCapacity()} мест");
            Console.WriteLine($"✓ Создано матчей: {Matches.Count}");
        }

        public Match GetMatch(string matchId)
        {
            return Matches.Find(m => m.MatchId == matchId);
        }
    }
}
