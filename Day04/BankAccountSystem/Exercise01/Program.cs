using System;
using System.Collections.Generic;

namespace Exercise01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Corrected the class name typo
            BankAccount acc1 = new BankAccount("Arjun", 1000);
            BankAccount acc2 = new BankAccount("Shiva", 50000);
            acc1.Deposit(2000);
            acc1.Withdraw(2000);
            acc1.Transfer(1000, acc2);
            acc1.PrintStatement();
            acc2.PrintStatement();
        }
    }

    public class BankAccount
    {
        private readonly string accountNumber;
        private decimal balance;
        private string ownerName;
        private List<string> transactionHistory;

        public string AccountNumber => accountNumber;
        public decimal Balance => balance;

        public string OwnerName
        {
            get => ownerName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Owner name cannot be empty");
                ownerName = value;
            }
        }

        // Constructor
        public BankAccount(string owner, decimal initialBalance = 0)
        {
            if (initialBalance < 0)
            {
                throw new ArgumentException("Initial balance cannot be negative");
            }
            accountNumber = $"ACC{Random.Shared.Next(100000, 999999)}";
            OwnerName = owner;
            balance = initialBalance;
            transactionHistory = new List<string>();

            if (initialBalance > 0)
            {
                AddTransaction($"Initial deposit: {initialBalance:C}");
            }
        }

        // Deposit method
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive");
            }

            balance += amount;
            AddTransaction($"Deposit: {amount:C}");
            Console.WriteLine($"Deposited: {amount:C}, new Balance: {balance:C}");
        }

        // Withdraw method
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdraw amount must be positive");
            }
            if (amount > balance)
            {
                Console.WriteLine($"Insufficient funds. Balance: {balance:C}");
                return false;
            }
            balance -= amount;
            AddTransaction($"Withdraw: {amount:C}");
            Console.WriteLine($"Withdrawn: {amount:C}, new Balance: {balance:C}");
            return true;
        }

        // Transfer method
        public bool Transfer(decimal amount, BankAccount targetAccount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Transfer amount must be a positive value");
                return false;
            }

            if (targetAccount == null)
            {
                Console.WriteLine("Target account is null");
                return false;
            }

            if (balance < amount)
            {
                Console.WriteLine("Insufficient funds");
                return false;
            }

            balance -= amount;
            targetAccount.Deposit(amount);

            AddTransaction($"Transferred {amount:C} to account {targetAccount.accountNumber}");
            targetAccount.AddTransaction($"Received {amount:C} from account {this.accountNumber}");
            return true;
        }

        // Add a transaction to history
        public void AddTransaction(string transaction)
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            transactionHistory.Add($"[{timeStamp}] {transaction}");
        }

        // Get transaction history
        public List<string> GetTransactionHistory()
        {
            return new List<string>(transactionHistory);
        }

        // Print account statement
        public void PrintStatement()
        {
            Console.WriteLine($"\n=== Account Statement ===");
            Console.WriteLine($"Account: {AccountNumber}");
            Console.WriteLine($"Owner: {OwnerName}");
            Console.WriteLine($"Balance: {Balance:C}");
            Console.WriteLine($"\nTransaction History:");
            foreach (var transaction in transactionHistory)
            {
                Console.WriteLine(transaction);
            }
        }
    }
}