namespace ATMApp.UI
{
    internal class Validator
    {
        public static bool IsValidCardNumber(string input) => !string.IsNullOrWhiteSpace(input) && input.All(char.IsDigit) && input.Length == 10;

        public static bool IsValidAccountNumber(string input) => !string.IsNullOrWhiteSpace(input) && input.All(char.IsDigit) && input.Length == 8;

        public static bool IsValidFullName(string input) => !string.IsNullOrWhiteSpace(input) && input.Trim().Length >= 3;

        public static bool IsValidRawPin(string input) => !string.IsNullOrWhiteSpace(input) && input.Length == 6;

        public static bool IsValidHashedPin(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            return (input.StartsWith("$2a$") || input.StartsWith("$2b$") || input.StartsWith("$2y$")) && input.Length >= 60;
        }

        public static bool IsValidPin(string input) => IsValidRawPin(input) || IsValidHashedPin(input);

        public static bool IsValidAmount(decimal input) => input > 0;
    }
}
