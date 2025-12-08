using System.Collections.Generic;
using System.Linq;

namespace FootballTicketSystem.Models
{
    public class Stadium
    {
        public string StadiumId { get; private set; }
        public string Name { get; set; }
        public List<Section> Sections { get; private set; }

        public Stadium(string name)
        {
            StadiumId = System.Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            Name = name;
            Sections = new List<Section>();
        }

        public int GetTotalCapacity()
        {
            return Sections.Sum(s => s.Seats.Count);
        }

        public Section GetSection(string sectionId)
        {
            return Sections.FirstOrDefault(s => s.SectionId == sectionId);
        }

        public void AddSection(Section section)
        {
            Sections.Add(section);
        }
    }
}
