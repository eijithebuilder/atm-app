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
            var handler = new MenuHandler(screen);

            screen.Welcome();
            Console.WriteLine("Press [Enter] to continue.");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("=====================================");
            Console.WriteLine("          ATM Login Portal         ");
            Console.WriteLine("=====================================\n");
            // load mock data
            var loginService = new UserLoginService(MockData.LoadAccountsFromJson("MockData/accounts.json"));
            Account? account = AuthenticateUser(screen, loginService);

            if (account == null) return;

            // handle login
            while (true)
            {
                screen.ShowMainMenu(account);
                int choice = handler.ReadMenuChoice(4, () => screen.ShowMainMenu(account));

                switch (choice)
                {
                    case 1:
                        HandleSettingsMenu(handler, screen, account);
                        break;

                    case 2:
                        var (depositSuccess, depositMsg) = handler.HandleDeposit(account);
                        screen.ShowMessage(depositMsg, depositSuccess ? ConsoleColor.Green : ConsoleColor.Red);
                        Pause();
                        break;

                    case 3:
                        var (withdrawSuccess, withdrawMsg) = handler.HandleWithdraw(account);
                        screen.ShowMessage(withdrawMsg, withdrawSuccess ? ConsoleColor.Green : ConsoleColor.Red);
                        Pause();
                        break;

                    case 4:
                        screen.ShowMessage(handler.HandleExit(), ConsoleColor.Yellow);
                        screen.ShowLoading(5);
                        return;
                }
            }
        }

        private Account? AuthenticateUser(AppScreen screen, UserLoginService loginService)
        {
            while (true)
            {
                string cardNumber = ValidatorService.ReadCardNumber();
                screen.ShowLoading(5);

                var found = loginService.FindAccount(cardNumber);
                if (found == null)
                {
                    screen.ShowMessage("Card not found. Try again.", ConsoleColor.Red);
                    continue;
                }

                if (found.Status == AccountStatus.Locked)
                {
                    screen.ShowMessage("Your account has been temporarily locked for suspicious activity. Please call us at 111-000-111 to vertify your identity.", ConsoleColor.Yellow);
                    return null;
                }

                string pin = ValidatorService.ReadPin();
                screen.ShowLoading(5);

                var account = loginService.Authenticate(found, pin, out string message);
                screen.ShowMessage(message, account != null ? ConsoleColor.Green : ConsoleColor.Red);

                if (account == null && message.Contains("locked"))
                    return null;

                if (account != null)
                    return account;
            }
        }

        private void HandleSettingsMenu(MenuHandler handler, AppScreen screen, Account account)
        {
            bool back = false;
            while (!back)
            {
                screen.ShowSettingsMenu(account);
                int settingChoice = handler.ReadMenuChoice(3, () => screen.ShowSettingsMenu(account));

                switch (settingChoice)
                {
                    case 1:
                        var (success, message) = handler.HandleChangeFullname(account);
                        screen.ShowMessage(message, success ? ConsoleColor.Green : ConsoleColor.Red);
                        break;

                    case 2:
                        var (pinSuccess, pinMsg) = handler.HandleChangePin(account);
                        screen.ShowMessage(pinMsg, pinSuccess ? ConsoleColor.Green : ConsoleColor.Red);
                        break;

                    case 3:
                        back = true;
                        continue;
                }

                Pause();
            }
        }

        private void Pause()
        {
            Console.WriteLine("Press [Enter] to continue...");
            Console.ReadLine();
        }
    }
}
