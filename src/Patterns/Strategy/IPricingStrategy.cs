using FootballTicketSystem.Models;

namespace FootballTicketSystem.Patterns.Strategy
{
    public interface IPricingStrategy
    {
        decimal CalculatePrice(decimal basePrice, SeatCategory category);
    }
}
