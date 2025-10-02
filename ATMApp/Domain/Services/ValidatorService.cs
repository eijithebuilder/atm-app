using ATMApp.UI;
using System.Text;

namespace ATMApp.Domain.Services
{
    public static class ValidatorService
    {
        public static string ReadCardNumber()
        {
            while (true)
            {
                Console.WriteLine("Enter your card number (10 digits):");
                Console.Write("> ");
                string input = Console.ReadLine() ?? string.Empty;

                if (input.Length == 10 && input.All(char.IsDigit))
                {
                    return input;
                }

                Console.WriteLine("Invalid card number. Please try again.");
            }
        }

        public static string ReadPin()
        {
            StringBuilder input = new();
            Console.WriteLine("Enter your PIN (6 digits):");
            Console.Write("> ");
            while (true)
            {   
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 6) break;
                    
                    Console.WriteLine("\nPIN must be 6 digits.");
                    input.Clear();
                    Console.WriteLine("Enter your PIN (6 digits):");
                    Console.Write("> ");
                    continue;
                }
                
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (char.IsDigit(key.KeyChar) && input.Length < 6)
                {
                    input.Append(key.KeyChar);
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return input.ToString();
        }
    }
}
