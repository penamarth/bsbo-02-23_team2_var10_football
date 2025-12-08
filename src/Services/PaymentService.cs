using System;
using FootballTicketSystem.Models;

namespace FootballTicketSystem.Services
{
    public class PaymentService
    {
        public Payment ProcessPayment(decimal amount, PaymentMethod method)
        {
            var payment = new Payment(amount, method);
            
            if (payment.Process())
            {
                return payment;
            }
            
            payment.Status = PaymentStatus.Failed;
            throw new InvalidOperationException("Ошибка обработки платежа");
        }

        public bool RefundPayment(Payment payment, decimal refundPercent = 0.9m)
        {
            if (payment.Status != PaymentStatus.Completed)
                return false;

            decimal refundAmount = payment.Amount * refundPercent;
            Console.WriteLine($"Возврат {refundAmount} руб. (90% от {payment.Amount} руб.)");
            
            return true;
        }
    }
}
