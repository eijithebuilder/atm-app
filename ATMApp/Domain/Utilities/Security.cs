namespace ATMApp.Domain.Utilities
{
    public class Security
    {
        public static string HashPin(string rawPin)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPin);
        }

        public static bool VerifyPin(string rawPin, string hashedPin)
        {
            return BCrypt.Net.BCrypt.Verify(rawPin, hashedPin);
        }
    }
}
