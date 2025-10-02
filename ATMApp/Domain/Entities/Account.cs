using ATMApp.Domain.Utilities;
using ATMApp.UI;
using System.Text.Json.Serialization;

namespace ATMApp.Domain.Entities
{
    public enum AccountStatus
    {
        Active,
        Locked,
    }

    public class Account
    {
        public int Id { get; }
        public string CardNumber { get; } = string.Empty; // 10 digits
        public string AccountNumber { get; } = string.Empty; // 8 digits

        public string CardPin { get; private set; } = string.Empty; // hashed pin
        public string FullName { get; private set; } = string.Empty;
        public decimal AccountBalance { get; private set; } = 0m;
        public int FailedLoginAttempts { get; private set; } = 0;
        public AccountStatus Status { get; private set; } = AccountStatus.Active;

        public bool CanLogin()
        {
            return Status == AccountStatus.Active;
        }

        public bool IsLocked()
        {
            return Status == AccountStatus.Locked;
        }

        public Account(int id, string cardNumber, string cardPin, string accountNumber, string fullName)
        {
            if (!Validator.IsValidCardNumber(cardNumber))
            {
                throw new ArgumentException("Card number must be 10 digits.", nameof(cardNumber));
            }

            if (!Validator.IsValidAccountNumber(accountNumber))
            {
                throw new ArgumentException("Account number must be 8 digits.", nameof(accountNumber));
            }

            if (!Validator.IsValidFullName(fullName))
            {
                throw new ArgumentException("Invalid full name.", nameof(accountNumber));
            }

            if (!Validator.IsValidPin(cardPin))
            {
                throw new ArgumentException("Invalid card pin", nameof(cardPin));
            }

            Id = id;
            CardNumber = cardNumber;
            CardPin = cardPin;
            AccountNumber = accountNumber;
            FullName = fullName;
        }

        public Account(int id, string cardNumber, string cardPin, string accountNumber, string fullName, decimal balance)
        {
            if (!Validator.IsValidCardNumber(cardNumber))
            {
                throw new ArgumentException("Card number must be 10 digits.", nameof(cardNumber));
            }

            if (!Validator.IsValidAccountNumber(accountNumber))
            {
                throw new ArgumentException("Account number must be 8 digits.", nameof(accountNumber));
            }

            if (!Validator.IsValidFullName(fullName))
            {
                throw new ArgumentException("Invalid full name.", nameof(accountNumber));
            }

            if (!Validator.IsValidPin(cardPin))
            {
                throw new ArgumentException("Invalid card pin", nameof(cardPin));
            }

            Id = id;
            CardNumber = cardNumber;
            CardPin = cardPin;
            AccountNumber = accountNumber;
            FullName = fullName;
            AccountBalance = balance;
        }

        [JsonConstructor]
        public Account (int id, string cardNumber, string cardPin, string accountNumber, string fullName, decimal accountBalance, int failedLoginAttempts, AccountStatus status)
        {
            if (!Validator.IsValidCardNumber(cardNumber))
            {
                throw new ArgumentException("Card number must be 10 digits.", nameof(cardNumber));
            }

            if (!Validator.IsValidAccountNumber(accountNumber))
            {
                throw new ArgumentException("Account number must be 8 digits.", nameof(accountNumber));
            }

            if (!Validator.IsValidFullName(fullName))
            {
                throw new ArgumentException("Invalid full name.", nameof(accountNumber));
            }

            if (!Validator.IsValidPin(cardPin))
            {
                throw new ArgumentException("Invalid card pin", nameof(cardPin));
            }

            if (accountBalance < 0)
            {
                throw new ArgumentException("Balance cannot be negative.", nameof(accountBalance));
            }

            Id = id;
            CardNumber = cardNumber;
            CardPin = cardPin;
            AccountNumber = accountNumber;
            FullName = fullName;
            AccountBalance = accountBalance;
            FailedLoginAttempts = failedLoginAttempts;
            Status = status;
        }

        public bool SetFullName(string newFullname, out string message)
        {
            if (!Validator.IsValidFullName(newFullname))
            {
                message = "Full name must be at least 3 characters.";
                return false;
            }
            FullName = newFullname.Trim();
            message = "Full name updated successfully.";
            return true;
        }

        public bool SetPin(string newRawPin, out string message)
        {
            if (!Validator.IsValidRawPin(newRawPin))
            {
                message = "PIN must be 6 digits.";
                return false;
            }

            if (Security.VerifyPin(newRawPin, CardPin))
            {
                message = "New PIN cannot be the same as the old PIN.";
                return false;
            }
            CardPin = Security.HashPin(newRawPin);
            message = "PIN updated successfully.";
            return true;
        }

        public bool Deposit(decimal amount)
        {
            if (!Validator.IsValidAmount(amount))
            {
                return false;
            }

            AccountBalance += amount;
            return true;
        }

        public bool Withdraw(decimal amount)
        {
            if (!Validator.IsValidAmount(amount) || amount > AccountBalance)
            {
                return false;
            }

            AccountBalance -= amount;
            return true;
        }

        public bool ChangePin(string newHashedPin)
        {
            if (!Validator.IsValidPin(newHashedPin))
            {
                return false;
            }
            CardPin = newHashedPin;
            return true;
        }

        public void IncreaseFailedLogin()
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= 10)
            {
                LockAccount();
            }
        }

        public void ResetFailedLogin()
        {
            FailedLoginAttempts = 0;
        }

        public void LockAccount()
        {
            Status = AccountStatus.Locked;
        }

        public void UnlockAccount()
        {
            Status = AccountStatus.Active;
            FailedLoginAttempts = 0;
        }
    }
}
