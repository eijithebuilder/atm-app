using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;
using ATMApp.Domain.Utilities;
using Xunit;

namespace ATMApp.Tests
{
    public class UserLoginServiceTests
    {
        private Account CreateTestAccount()
        {
            var account = new Account(id: 1, cardNumber: "1234567890", cardPin: Security.HashPin("123456"), accountNumber: "12345678", fullName: "Test User", balance: 1000m);

            return account;
        }

        [Fact]
        public void Authenticate_CorrectPin_ReturnsAccount()
        {
            var account = CreateTestAccount();

            var service = new UserLoginService(new List<Account> { account });
            var result = service.Authenticate(account, "123456", out string message);

            Assert.NotNull(result);
            Assert.Equal("Login successful.", message);
            Assert.Equal(0, account.FailedLoginAttempts);
        }

        [Fact]
        public void Authenticate_WrongPin_IncrementsFailedAttempts()
        {
            var account = CreateTestAccount();

            var service = new UserLoginService(new List<Account> { account });
            var result = service.Authenticate(account, "000000", out string message);

            Assert.Null(result);
            Assert.Equal(1, account.FailedLoginAttempts);
            Assert.Equal("Authentication failed.", message);
        }

        [Fact]
        public void Authenticate_LockedAccount_ReturnsNull()
        {
            var account = CreateTestAccount();
            account.LockAccount();

            var service = new UserLoginService(new List<Account> { account });
            var result = service.Authenticate(account, "123456", out string message);

            Assert.Null(result);
            Assert.Contains("Login failed", message);
        }
    }
}
