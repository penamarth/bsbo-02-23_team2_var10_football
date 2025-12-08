using System;

namespace FootballTicketSystem.Models
{
    public class Customer
    {
        public string CustomerId { get; private set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Customer(string fullName, string phone, string email = "")
        {
            CustomerId = $"C-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Email = email;
        }
    }
}
