using ATMApp.Domain.Entities;

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
            Thread.Sleep(200);
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
            Console.WriteLine("=== Settings ===");
            Console.WriteLine("1. Change full name");
            Console.WriteLine("2. Change PIN");
            Console.WriteLine("3. Back");
        }

        public void ShowMessage(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{message}\n");
            Console.ResetColor();
        }
    }
}
