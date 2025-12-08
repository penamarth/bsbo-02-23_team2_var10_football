using FootballTicketSystem.Models;

namespace FootballTicketSystem.Patterns.Strategy
{
    public class StandardPricing : IPricingStrategy
    {
        public decimal CalculatePrice(decimal basePrice, SeatCategory category)
        {
            return category switch
            {
                SeatCategory.VIP => basePrice * 3m,
                SeatCategory.Standard => basePrice,
                SeatCategory.Standing => basePrice * 0.5m,
                _ => basePrice
            };
        }
    }
}
