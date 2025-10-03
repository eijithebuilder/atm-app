using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;

namespace ATMApp.UI
{
    public class MenuHandler
    {
        private readonly AppScreen _screen;

        public MenuHandler(AppScreen screen)
        {
            _screen = screen;
        }

        public int ReadMenuChoice(int option, Action showMenu)
        {
            while (true)
            {
                Console.WriteLine("\nSelect option:");
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= option)
                    return choice;

                _screen.ShowMessage("Invalid choice. Try again.", ConsoleColor.Red);
                Console.WriteLine("Press [Enter] to continue...");
                Console.ReadLine();
                showMenu();
            }
        }

        public (bool success, string message) HandleChangeFullname(Account account)
        {
            Console.WriteLine("\nEnter new full name:");
            Console.Write("> ");
            string? newFullname = Console.ReadLine()?.Trim() ?? "";

            bool success = account.SetFullName(newFullname, out string message);
            return (success, message);
        }

        public (bool success, string message) HandleChangePin(Account account)
        {
            Console.WriteLine("\nEnter new PIN (6 digits):");
            Console.Write("> ");
            string newPin = ValidatorService.ReadPin();

            bool success = account.SetPin(newPin, out string message);
            return (success, message);
        }

        public (bool success, string message) HandleDeposit(Account account)
        {
            Console.WriteLine("Enter amount to deposit: ");
            Console.Write("> ");

            if (decimal.TryParse(Console.ReadLine() ?? "", out decimal amount) && Validator.IsValidAmount(amount))
            {
                if (account.Deposit(amount))
                    return (true, "Deposit successful.");
                return (false, "Deposit failed.");
            }
            return (false, "Invalid amount.");
        }

        public (bool success, string message) HandleWithdraw(Account account)
        {
            Console.WriteLine("Enter amount to withdraw: ");
            Console.Write("> ");

            if (decimal.TryParse(Console.ReadLine() ?? "", out decimal amount) && Validator.IsValidAmount(amount))
            {
                if (account.Withdraw(amount))
                    return (true, "Withdrawal successful.");
                return (false, "Insufficient balance.");
            }
            return (false, "Invalid amount.");
        }

        public string HandleExit()
        {
            return "\nExiting...\nThank you for using CredibilityBank.";
        }
    }
}
