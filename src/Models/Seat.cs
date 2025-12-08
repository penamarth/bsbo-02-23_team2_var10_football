namespace FootballTicketSystem.Models
{
    public enum SeatCategory
    {
        VIP,
        Standard,
        Standing
    }

    public class Seat
    {
        public string SeatId { get; private set; }
        public Section Section { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public SeatCategory Category { get; set; }
        public bool IsAvailable { get; set; }

        public Seat(Section section, int row, int number, SeatCategory category)
        {
            SeatId = $"{section.SectionId}-{row}-{number}";
            Section = section;
            Row = row;
            Number = number;
            Category = category;
            IsAvailable = true;
        }

        public string GetFullNumber()
        {
            return $"Сектор {Section.Name}, Ряд {Row}, Место {Number}";
        }
    }
}
