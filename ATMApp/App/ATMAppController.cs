using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMApp.Domain.Entities;
using ATMApp.Domain.Services;
using ATMApp.UI;

namespace ATMApp.App
{
    public class ATMAppController
    {
        private readonly UserLoginService _loginService;

        public ATMAppController(UserLoginService loginService)
        {
            _loginService = loginService;
        }

        public void Run()
        {
            var screen = new AppScreen();
            screen.Welcome();
            Console.WriteLine("Press [Enter] to continue.");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("=====================================");
            Console.WriteLine("          ATM Login Portal         ");
            Console.WriteLine("=====================================\n");
            // load mock data
            var loginService = new UserLoginService(MockData.LoadAccountsFromJson("F:\\accounts.json"));
            Account? account = null;
            // handle login
            while (account == null)
            {
                string cardNumber = ValidatorService.ReadCardNumber();
                screen.ShowLoading(5);
                var found = loginService.FindAccount(cardNumber);

                if (found == null)
                {
                    Console.WriteLine("Card not found. Try again.");
                    continue;
                }

                if (found.Status == AccountStatus.Locked)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Your account has been temporarily locked for suspicious activity. Please call us at 111-000-111 to vertify your identity.");
                    Console.ResetColor();

                    return;
                }

                string pin = ValidatorService.ReadPin();
                screen.ShowLoading(5);
                account = loginService.Authenticate(found, pin, out string message);
                Console.WriteLine(message);

                if (account == null && message.Contains("locked"))
                {
                    return;
                }
            }

            if (account != null)
            {
                screen.ShowMainMenu(account);

                int choice;

                do
                {
                    choice = screen.ReadMenuChoice(4);

                    switch (choice)
                    {
                        case 1:
                            screen.ShowSettingsMenu(account);
                            break;

                        case 2:
                            screen.HandleDeposit(account);
                            break;

                        case 3:
                            screen.HandleWithdraw(account);
                            break;

                        case 4:
                            screen.HandleExit();
                            return;
                    }
                }
                while (choice >= 1 && choice <= 4);
            }
        }
    }
}
