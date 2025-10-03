using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;

namespace ATMApp.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var accounts = MockData.LoadAccountsFromJson("MockData/accounts.json");

            var loginService = new UserLoginService(accounts);

            var controller = new ATMAppController(loginService);
            controller.Run();
        }
    }
}
