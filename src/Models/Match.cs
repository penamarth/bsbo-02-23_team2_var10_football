using System;

namespace FootballTicketSystem.Models
{
    public class Match
    {
        public string MatchId { get; private set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime DateTime { get; set; }
        public Stadium Stadium { get; set; }

        public Match(string homeTeam, string awayTeam, DateTime dateTime, Stadium stadium)
        {
            MatchId = $"M-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            HomeTeam = homeTeam ?? throw new ArgumentNullException(nameof(homeTeam));
            AwayTeam = awayTeam ?? throw new ArgumentNullException(nameof(awayTeam));
            DateTime = dateTime;
            Stadium = stadium ?? throw new ArgumentNullException(nameof(stadium));
        }

        public string GetMatchInfo()
        {
            return $"{HomeTeam} - {AwayTeam} ({DateTime:dd.MM.yyyy HH:mm})";
        }
    }
}
