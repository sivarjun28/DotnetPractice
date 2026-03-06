using System;
namespace Exercise04
{

    /*
Base class: BankAccount
- Properties: AccountNumber, HolderName, Balance
- Methods: Deposit(), Withdraw() [virtual], GetAccountInfo()

Account types:
1. SavingsAccount
   - Properties: InterestRate
   - Withdraw: Cannot withdraw if balance < 100 (minimum balance)
   - Method: CalculateInterest()
   
2. CheckingAccount
   - Properties: OverdraftLimit
   - Withdraw: Can overdraft up to limit (negative balance)
   - Property: AvailableBalance (Balance + OverdraftLimit)
   
3. FixedDepositAccount (extends SavingsAccount)
   - Properties: MaturityDate, PenaltyRate
   - Withdraw: Cannot withdraw before maturity (penalty applies)
*/

    internal class Program
    {
        static void Main(string[] args)
        {
            SavingsAccount savings = new("SAV001", "Alice", 1000, 0.03m);
            savings.Withdraw(950);  // Should fail (minimum balance)
            savings.CalculateInterest();

            CheckingAccount checking = new("CHK001", "Bob", 500, 200);
            checking.Withdraw(600);  // Should succeed (overdraft)
            Console.WriteLine($"Available: {checking.AvailableBalance:C}");

            FixedDepositAccount fd = new("FD001", "Carol", 10000, 0.05m, DateTime.Now.AddMonths(6),0.06m);
            fd.Withdraw(1000);  // Should fail or apply penalty
        }
    }
    public class BankAccount
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string HolderName { get; set; } = string.Empty;
        protected decimal balance;

        public decimal Balance => balance;

        public BankAccount(string accountNumber, string holderName, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            HolderName = holderName;
            balance = initialBalance;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                System.Console.WriteLine("Deposit amount must be positive");
                return;
            }
            balance += amount;
            System.Console.WriteLine($"Deposited {amount:C}. New Balance : {balance:C}");

        }

        public virtual bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                System.Console.WriteLine("Withdrawl amount must be positive");
                return false;
            }
            if (amount > balance)
            {
                System.Console.WriteLine("Insufficient Funds");
                return false;
            }
            balance -= amount;
            System.Console.WriteLine($"Withdral: {amount:C} new Balance: {balance:C}");
            return true;

        }

        public virtual void GetAccountInfo()
        {
            System.Console.WriteLine($"AccountNumber: {AccountNumber}");
            System.Console.WriteLine($"Holder Name: {HolderName}");
            System.Console.WriteLine($"balance: {balance:C}");
        }


    }
    // TODO: Implement SavingsAccount
    public class SavingsAccount : BankAccount
    {
        public decimal InterestRate { get; set; }
        public SavingsAccount(string accountNumber, string holderName, decimal initialBalance, decimal interestRate) :
                base(accountNumber, holderName, initialBalance)
        {
            InterestRate = interestRate;
        }

        public override bool Withdraw(decimal amount)
        {
            if (Balance - amount < 100)
            {
                System.Console.WriteLine("Cannot withdraw: minimum balance of 100 must maintained");
                return false;
            }
            return base.Withdraw(amount);
        }

        public decimal CalculateInterest()
        {
            decimal interest = balance * InterestRate;
            System.Console.WriteLine($"Interest Earned:{interest:C}");
            return interest;
        }
        public override void GetAccountInfo()
        {
            base.GetAccountInfo();
            System.Console.WriteLine($"Interest Rate: {InterestRate:P}");
        }
    }
    // TODO: Implement CheckingAccount

    public class CheckingAccount : BankAccount
    {
        public decimal OverdraftLimit { get; set; } // max overdraft allowed

        public CheckingAccount(string accountNumber, string holderName, decimal initialBalance, decimal overdraftLimit)
            : base(accountNumber, holderName, initialBalance)
        {
            OverdraftLimit = overdraftLimit;
        }
        public decimal AvailableBalance => balance + OverdraftLimit;
        public override bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be positive");
                return false;
            }
            if (amount > AvailableBalance)
            {
                Console.WriteLine("Cannot withdraw: exceeds overdraft limit");
                return false;
            }
            balance -= amount;
            Console.WriteLine($"Withdrawal: {amount:C}. New Balance: {balance:C} (Available: {AvailableBalance:C})");
            return true;
        }

        public override void GetAccountInfo()
        {
            base.GetAccountInfo();
            Console.WriteLine($"Overdraft Limit: {OverdraftLimit:C}");
            Console.WriteLine($"Available Balance: {AvailableBalance:C}");
        }

    }
    // TODO: Implement FixedDepositAccount
    // FixedDepositAccount class (inherits SavingsAccount)
    public class FixedDepositAccount : SavingsAccount
    {
        public DateTime MaturityDate { get; set; }
        public decimal PenaltyRate { get; set; } // e.g., 0.02 for 2%

        public FixedDepositAccount(string accountNumber, string holderName, decimal initialBalance, decimal interestRate,
            DateTime maturityDate, decimal penaltyRate)
            : base(accountNumber, holderName, initialBalance, interestRate)
        {
            MaturityDate = maturityDate;
            PenaltyRate = penaltyRate;
        }

        public override bool Withdraw(decimal amount)
        {
            if (DateTime.Now < MaturityDate)
            {
                decimal penalty = amount * PenaltyRate;
                decimal totalDeduction = amount + penalty;
                if (totalDeduction > balance)
                {
                    Console.WriteLine("Cannot withdraw: Insufficient funds including penalty.");
                    return false;
                }
                balance -= totalDeduction;
                Console.WriteLine($"Early withdrawal: {amount:C} + penalty {penalty:C}. New Balance: {balance:C}");
                return true;
            }
            // After maturity, normal savings withdrawal rules apply
            return base.Withdraw(amount);
        }

        public override void GetAccountInfo()
        {
            base.GetAccountInfo();
            Console.WriteLine($"Maturity Date: {MaturityDate:yyyy-MM-dd}");
            Console.WriteLine($"Penalty Rate: {PenaltyRate:P}");
        }
    }
}
