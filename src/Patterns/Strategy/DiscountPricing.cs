using System;
using FootballTicketSystem.Models;

namespace FootballTicketSystem.Patterns.Strategy
{
    public class DiscountPricing : IPricingStrategy
    {
        private decimal discountPercent;

        public DiscountPricing(decimal discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%");
            
            this.discountPercent = discountPercent;
        }

        public decimal CalculatePrice(decimal basePrice, SeatCategory category)
        {
            var standardPrice = category switch
            {
                SeatCategory.VIP => basePrice * 3m,
                SeatCategory.Standard => basePrice,
                SeatCategory.Standing => basePrice * 0.5m,
                _ => basePrice
            };

            return standardPrice * (1 - discountPercent / 100m);
        }
    }
}
