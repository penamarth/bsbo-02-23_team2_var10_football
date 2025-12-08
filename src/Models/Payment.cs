using System;

namespace FootballTicketSystem.Models
{
    public enum PaymentMethod
    {
        Cash,
        Card
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public class Payment
    {
        public string PaymentId { get; private set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; private set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }

        public Payment(decimal amount, PaymentMethod method)
        {
            PaymentId = $"P-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            Amount = amount;
            Method = method;
            PaymentDate = DateTime.Now;
            Status = PaymentStatus.Pending;
        }

        public bool Process()
        {
            // Симуляция обработки платежа
            Console.WriteLine($"Обработка платежа {PaymentId} на сумму {Amount} руб. ({Method})...");
            
            // В реальной системе здесь была бы интеграция с платежной системой
            Status = PaymentStatus.Completed;
            Console.WriteLine("Платеж успешно обработан!");
            return true;
        }
    }
}
