using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;
using ATMApp.Domain.Utilities;

namespace ATMApp.Tests;

public class AccountServicesTests
{
    private Account CreateTestAccount()
    {
        var account = new Account(id: 1, cardNumber: "1234567890", cardPin: Security.HashPin("123456"), accountNumber: "12345678", fullName: "Test User", accountBalance: 1000m);

        return account;
    }

    [Fact]
    public void Deposit_ValidAmount_IncreaseBalance()
    {
        var account = CreateTestAccount();
        bool result = account.Deposit(500m);

        Assert.True(result);
        Assert.Equal(1500m, account.AccountBalance);
    }

    [Fact]
    public void Deposit_NegativeAmount_ReturnsFalse()
    {
        var account = CreateTestAccount();

        bool result = account.Deposit(-100m);

        Assert.False(result);
        Assert.Equal(1000m, account.AccountBalance);
    }

    [Fact]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        var account = CreateTestAccount();
        bool result = account.Withdraw(400m);

        Assert.True(result);
        Assert.Equal(600m, account.AccountBalance);
    }

    [Fact]
    public void Withdraw_TooMuch_ReturnsFalse()
    {
        var account = CreateTestAccount();
        bool result = account.Withdraw(2000m);

        Assert.False(result);
        Assert.Equal(1000m, account.AccountBalance);
    }

    [Fact]
    public void SetFullName_ValidName_ReturnsTrue()
    {
        var account = new Account(1, "1234567890", Security.HashPin("123456"), "12345678", "Old Name");
        bool result = account.SetFullName("New Name", out string msg);

        Assert.True(result);
        Assert.Equal("New Name", account.FullName);
        Assert.Equal("Full name updated successfully.", msg);
    }

    [Fact]
    public void SetFullName_InvalidName_ReturnsFalse()
    {
        var account = new Account(1, "1234567890", Security.HashPin("123456"), "12345678", "Old Name");
        bool result = account.SetFullName("  ", out string msg);

        Assert.False(result);
        Assert.Equal("Old Name", account.FullName);
        Assert.Equal("Full name must be at least 3 characters.", msg);
    }

    [Fact]
    public void SetPin_ValidNewPin_ReturnsTrue()
    {
        var account = new Account(1, "1234567890", Security.HashPin("123456"), "12345678", "Test User");
        bool result = account.SetPin("654321", out string msg);

        Assert.True(result);
        Assert.True(Security.VerifyPin("654321", account.CardPin));
        Assert.Equal("PIN updated successfully.", msg);
    }

    [Fact]
    public void SetPin_InvalidLength_ReturnsFalse()
    {
        var account = new Account(1, "1234567890", Security.HashPin("123456"), "12345678", "Test User");
        bool result = account.SetPin("123", out string msg);

        Assert.False(result);
        Assert.True(Security.VerifyPin("123456", account.CardPin));
        Assert.Equal("PIN must be 6 digits.", msg);
    }

    [Fact]
    public void SetPin_SameAsOld_ReturnsFalse()
    {
        var account = new Account(1, "1234567890", Security.HashPin("123456"), "12345678", "Test User");
        bool result = account.SetPin("123456", out string msg);

        Assert.False(result);
        Assert.True(Security.VerifyPin("123456", account.CardPin));
        Assert.Equal("New PIN cannot be the same as the old PIN.", msg);
    }
}
