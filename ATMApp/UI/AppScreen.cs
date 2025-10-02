using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;
using System;

namespace ATMApp.UI
{
    public class AppScreen
    {
        public void ShowLoading(int spins)
        {
            char[] spinner = ['|', '/', '-', '\\'];
            for (int i = 0; i < spins; i++)
            {
                Console.Write(spinner[i % spinner.Length]);
                Thread.Sleep(50);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        public void Welcome()
        {
            Console.Clear();
            Console.Title = "CredibilityBank";
            Console.WriteLine("══════════════════════════════════════════════");
            Console.WriteLine("\tWelcome to CredibilityBank!");
            Console.WriteLine("\tYour money is our treasure");
            Console.WriteLine("══════════════════════════════════════════════\n");
        }

        public (string cardNumber, string pin) LoginScreen()
        {
            Console.WriteLine("Enter your card number (10 digits):");
            Console.Write("> ");
            string cardNumber = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter your PIN (6 digits):");
            Console.Write("> ");
            string pin = Console.ReadLine() ?? string.Empty;

            return (cardNumber, pin);
        }

        public void ShowMainMenu(Account account)
        {
            Console.WriteLine("Loading servies...");
            ShowLoading(5);
            Thread.Sleep(500);

            Console.Clear();
            Console.WriteLine($"Greetings! {account.FullName}\n");
            Console.WriteLine($"Account number: {account.AccountNumber}");
            Console.WriteLine($"Balance: {account.AccountBalance:C}\n");

            Console.WriteLine("=== ATM Menu ===");
            Console.WriteLine("1. Settings");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Exit");
        }

        public void ShowSettingsMenu(Account account)
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("=== Settings ===");
                Console.WriteLine("1. Change full name");
                Console.WriteLine("2. Change PIN");
                Console.WriteLine("3. Back");

                var choice = ReadMenuChoice(3);

                switch (choice)
                {
                    case 1: HandleChangeFullname(account); break;
                    case 2: HandleChangePin(account); break;
                    case 3: ShowMainMenu(account); return;
                }
            }
        }

        public int ReadMenuChoice(int option)
        {
            while (true)
            {
                Console.WriteLine("\nSelect option:");
                Console.Write("> ");

                string? input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= option)
                {
                    return choice;
                }

                Console.WriteLine("\nInvalid choice. Please try again.");
            }
        }

        public void HandleChangeFullname(Account account)
        {
            Console.WriteLine("\nEnter new full name:");
            Console.Write("> ");
            string? newFullname = Console.ReadLine()?.Trim();

            if (!account.SetFullName(newFullname!, out string message))
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }

            ShowLoading(5);
            Thread.Sleep(500);
        }

        public void HandleChangePin(Account account)
        {
            Console.WriteLine("\nEnter new PIN (6 digits):");
            Console.Write("> ");
            string newPin = ValidatorService.ReadPin();

            if (!account.SetPin(newPin, out string message))
            {
                Console.WriteLine(message);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }
                
            
            ShowLoading(5);
            Thread.Sleep(500);
        }

        public void HandleDeposit(Account account)
        {
            Console.WriteLine("Enter amount to deposit: ");
            Console.Write("> ");

            if (decimal.TryParse(Console.ReadLine() ?? "", out decimal amount) && Validator.IsValidAmount(amount))
            {
                if (account.Deposit(amount))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nDeposit successful.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nDeposit failed.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\nInvalid amount.");
            }
        }

        public void HandleWithdraw(Account account)
        {
            Console.WriteLine("Enter amount to withdraw: ");
            Console.Write("> ");

            if (decimal.TryParse(Console.ReadLine() ?? "", out decimal amount) && Validator.IsValidAmount(amount))
            {
                if (account.Withdraw(amount))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nWithdrawal successful.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInsufficient balance or invalid amount.");
                    Console.ResetColor();
                } 
            }
            else
            {
                Console.WriteLine("\nInvalid input.");
            }
        }

        public void HandleExit()
        {
            Console.WriteLine("\nExiting...");
            ShowLoading(5);
            Console.WriteLine("Happy to service ^_^");
        }
    }
}
