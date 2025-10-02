using ATMApp.Domain.Entities;
using ATMApp.Domain.Utilities;

namespace ATMApp.Domain.Services
{
    public class UserLoginService
    {
        private readonly List<Account> _accounts;

        public UserLoginService(List<Account> accounts)
        {
            _accounts = accounts;
        }

        public Account? FindAccount(string cardNumber)
        {
            return _accounts.FirstOrDefault(a => a.CardNumber == cardNumber);
        }

        public Account? Authenticate(Account account, string rawPin, out string message)
        {
            if (!account.CanLogin())
            {
                message = $"Login failed.\nYour account status is {account.Status}.";
                return null;
            }

            if (!Security.VerifyPin(rawPin, account.CardPin))
            {
                account.IncreaseFailedLogin();
                // Lock account after 10 attempts
                if (account.IsLocked())
                {
                    message = "Account locked due to many failed attempts.";
                }
                else
                {
                    message = "Authentication failed.";
                }
                    
                return null;
            }
            // reset counter when login successful
            account.ResetFailedLogin();
            message = "Login successful.";
            return account;
        }   
    }
}
