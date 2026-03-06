using System;
using System.Security.Cryptography.X509Certificates;
namespace Exercise01
{



    /*
Interfaces:
1. IPaymentMethod
   - ProcessPayment(amount): bool
   - ValidatePayment(): bool
   - GetTransactionFee(amount): decimal

2. IRefundable
   - ProcessRefund(amount): bool
   - GetRefundFee(amount): decimal

Payment Methods:
1. CreditCardPayment (IPaymentMethod, IRefundable)
   - CardNumber, CVV, ExpiryDate
   - Transaction fee: 2.5%
   - Refund fee: 1%

2. PayPalPayment (IPaymentMethod, IRefundable)
   - Email, Password
   - Transaction fee: 3%
   - Refund fee: 0%

3. CryptoPayment (IPaymentMethod)
   - WalletAddress, PrivateKey
   - Transaction fee: 1%
   - No refunds

4. BankTransferPayment (IPaymentMethod)
   - AccountNumber, RoutingNumber
   - Transaction fee: $5 flat fee
   - Processing takes 2-3 days
*/

    public interface IPaymentMethod
    {

        string PaymentType { get; }
        bool ProcessPayment(decimal amount);
        bool ValidatePayment();
        decimal GetTransactionFee(decimal amount);

    }

    public interface IRefundable
    {
        bool ProcessRefund(decimal amount);
        decimal GetRefundFee(decimal amount);
    }

    public class CreditCardPayment : IPaymentMethod, IRefundable
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }

        public string PaymentType => "Credit card";

        public bool ValidatePayment()
        {
            // Validate card number, CVV, and expiry date
            bool isCardValid = !string.IsNullOrEmpty(CardNumber);
            bool isCVVValid = !string.IsNullOrEmpty(CVV);
            bool isExpiryValid = ExpiryDate > DateTime.Now;  // Check if the expiry date is in the future

            // Return true if all conditions are valid
            return isCardValid && isCVVValid && isExpiryValid;
        }
        public decimal GetTransactionFee(decimal amount)
        {
            return amount * 0.25m;
        }

        public bool ProcessPayment(decimal amount)
        {
            if (!ValidatePayment())
            {
                System.Console.WriteLine("Payment Validation failed");
                return false;
            }

            decimal fee = GetTransactionFee(amount);
            decimal total = amount + fee;

            System.Console.WriteLine($"Processing credit card Payments: {total:C} (fee: {fee})");
            return true;
        }

        public decimal GetRefundFee(decimal amount)
        {
            return amount * 0.01m;
        }
        public bool ProcessRefund(decimal amount)
        {
            decimal fee = GetRefundFee(amount);
            decimal refundAmount = amount - fee;
            System.Console.WriteLine($"Processing Refund: {refundAmount:C} (fee: {fee:C})");
            return true;
        }
    }

    public class PayPalPayment : IPaymentMethod, IRefundable
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string PaymentType => "Paypal";
        public bool ValidatePayment()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        }
        public decimal GetTransactionFee(decimal amount)
        {
            return amount * 0.03m;
        }
        public bool ProcessPayment(decimal amount)
        {
            if (!ValidatePayment())
            {
                System.Console.WriteLine("Payment validation failed");
                return false;
            }
            decimal fee = GetTransactionFee(amount);
            decimal total = amount + fee;
            System.Console.WriteLine($"processing paypal payment {total:C} (fee: {fee:C})");
            return true;
        }
        public decimal GetRefundFee(decimal amount)
        {
            return amount * 0.02m;
        }

        public bool ProcessRefund(decimal amount)
        {
            decimal fee = GetRefundFee(amount);
            decimal refundAmount = amount - fee;
            System.Console.WriteLine($"Processing paypal Refund: {refundAmount:C} (fee:{fee:C})");
            return true;
        }
    }

    public class CryptoPayment : IPaymentMethod
    {
        public string WalletAddress { get; set; } = string.Empty;

        public string PaymentType => "Crypto Payment";

        public bool ValidatePayment()
        {
            return !string.IsNullOrEmpty(WalletAddress);
        }
        public decimal GetTransactionFee(decimal amount)
        {
            return amount * 0.015m;
        }

        public bool ProcessPayment(decimal amount)
        {
            if (!ValidatePayment())
            {
                Console.WriteLine("Payment validation failed");
                return false;
            }

            decimal fee = GetTransactionFee(amount);
            decimal total = amount + fee;

            Console.WriteLine($"Processing crypto payment: {total:C} (fee: {fee:C})");
            // Simulate payment processing
            return true;
        }
    }

    public class BankTransferPayment : IPaymentMethod
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string RoutingNumber { get; set; } = string.Empty;

        public string PaymentType => "Bank Trasfer";

        public bool ValidatePayment()
        {
            return !string.IsNullOrEmpty(AccountNumber) && !string.IsNullOrEmpty(RoutingNumber);
        }
        public decimal GetTransactionFee(decimal amount)
        {
            return amount * 0.01m;
        }
        public bool ProcessPayment(decimal amount)
        {
            if (!ValidatePayment())
            {
                Console.WriteLine("Payment validation failed");
                return false;
            }

            decimal fee = GetTransactionFee(amount);
            decimal total = amount + fee;

            Console.WriteLine($"Processing bank transfer payment: {total:C} (fee: {fee:C})");
            // Simulate payment processing
            return true;
        }
    }

    // Test code
class PaymentProcessor
{
    public static void ProcessOrder(IPaymentMethod payment, decimal amount)
    {
        Console.WriteLine($"\n--- Processing {payment.PaymentType} ---");
        
        if (payment.ProcessPayment(amount))
        {
            Console.WriteLine("Payment successful!");
            
            // Check if refundable
            if (payment is IRefundable refundable)
            {
                Console.WriteLine("This payment method supports refunds");
                decimal refundFee = refundable.GetRefundFee(amount);
                Console.WriteLine($"Refund fee would be: {refundFee:C}");
            }
            else
            {
                Console.WriteLine("This payment method does NOT support refunds");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<IPaymentMethod> payments = new()
            {
                new CreditCardPayment
                {
                    CardNumber = "1234 5678 9010",
                    CVV = "789",
                    ExpiryDate = DateTime.Now.AddYears(2)
                },
                new PayPalPayment
                {
                    Email = "arjunyadav@gmail.com",
                    Password = "Shiva@2345"
                },
                new CryptoPayment
                {
                    WalletAddress = "hghgy87joko09090jnjlk"
                },
                new BankTransferPayment
                {
                    AccountNumber = "1234567890",
                    RoutingNumber = "111000023"
                }
            };
            decimal orderAmount = 100m;
            foreach (var payment in payments)
            {
                ProcessOrder(payment,orderAmount);
            }
        }
    }

}
}