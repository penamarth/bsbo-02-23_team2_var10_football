using System.Collections.Generic;
using System.Linq;

namespace FootballTicketSystem.Models
{
    public class Section
    {
        public string SectionId { get; set; }
        public string Name { get; set; }
        public List<Seat> Seats { get; private set; }

        public Section(string sectionId, string name)
        {
            SectionId = sectionId;
            Name = name;
            Seats = new List<Seat>();
        }

        public void AddSeat(Seat seat)
        {
            Seats.Add(seat);
        }

        public List<Seat> GetAvailableSeats()
        {
            return Seats.Where(s => s.IsAvailable).ToList();
        }
    }
}
