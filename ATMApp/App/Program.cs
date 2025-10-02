using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;
using ATMApp.UI;
using BCrypt.Net;
namespace ATMApp.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var accounts = MockData.LoadAccountsFromJson("MockData/accounts.json");
            //IAccountService accountService = new AccountService();
            var loginService = new UserLoginService(accounts);

            var controller = new ATMAppController(loginService);
            controller.Run();
        }
    }
}
