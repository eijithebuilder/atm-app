using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public class MenuHandler
    {
        private readonly AppScreen _screen;

        public MenuHandler(AppScreen screen)
        {
            _screen = screen;
        }

        public int ReadMenuChoice(int option)
        {
            while (true)
            {
                Console.WriteLine("\nSelect option:");
                Console.Write("> ");

                string? input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= option)
                    return choice;

                _screen.ShowMessage("Invalid choice. Try again.", ConsoleColor.Red);
            }
        }

        public string HandleChangeFullname(Account account)
        {
            Console.WriteLine("\nEnter new full name:");
            Console.Write("> ");
            string? newFullname = Console.ReadLine()?.Trim();

            account.SetFullName(newFullname!, out string message);
            return message;
        }

        public string HandleChangePin(Account account)
        {
            Console.WriteLine("\nEnter new PIN (6 digits):");
            Console.Write("> ");
            string newPin = ValidatorService.ReadPin();

            bool success = account.SetPin(newPin, out string message);
            return message;
        }

        public string HandleDeposit(Account account)
        {
            Console.WriteLine("Enter amount to deposit: ");
            Console.Write("> ");

            if (decimal.TryParse(Console.ReadLine() ?? "", out decimal amount) && Validator.IsValidAmount(amount))
            {
                if (account.Deposit(amount))
                    return "Deposit successful.";
                return "Deposit failed.";
            }
            return "Invalid amount.";
        }

        public string HandleWithdraw(Account account)
        {
            Console.WriteLine("Enter amount to withdraw: ");
            Console.Write("> ");

            if (decimal.TryParse(Console.ReadLine() ?? "", out decimal amount) && Validator.IsValidAmount(amount))
            {
                if (account.Withdraw(amount))
                    return "Withdrawal successful.";
                return "Insufficient balance or invalid amount.";
            }
            return "Invalid input.";
        }

        public string HandleExit()
        {
            return "\nExiting...\nThank you for using CredibilityBank.";
        }
    }
}
