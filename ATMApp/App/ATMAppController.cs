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
            Account? account = null;
            // handle login
            while (account == null)
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
                    return;
                }

                string pin = ValidatorService.ReadPin();
                screen.ShowLoading(5);

                account = loginService.Authenticate(found, pin, out string message);
                screen.ShowMessage(message, account != null ? ConsoleColor.Green : ConsoleColor.Red);

                if (account == null && message.Contains("locked"))
                    return;
            }

            int choice;

            do
            {
                screen.ShowMainMenu(account);
                choice = handler.ReadMenuChoice(4);

                switch (choice)
                {
                    case 1:
                        bool back = false;
                        while (!back)
                        {
                            screen.ShowSettingsMenu(account);
                            int settingChoice = handler.ReadMenuChoice(3);
                            string resultMessage = "";

                            switch (settingChoice)
                            {
                                case 1: resultMessage = handler.HandleChangeFullname(account); break;
                                case 2: resultMessage = handler.HandleChangePin(account); break;
                                case 3: back = true; break;
                            }

                            screen.ShowLoading(5);

                            if (!string.IsNullOrWhiteSpace(resultMessage))
                            {
                                screen.ShowMessage(resultMessage, ConsoleColor.Green);
                                Console.WriteLine("Press [Enter] to continue...");
                                Console.ReadLine();
                            }
                        }
                        break;

                    case 2:
                        string depositMsg = handler.HandleDeposit(account);
                        screen.ShowMessage(depositMsg, depositMsg.Contains("successful") ? ConsoleColor.Green : ConsoleColor.Red);
                        Console.WriteLine("Press [Enter] to continue...");
                        Console.ReadLine();
                        break;

                    case 3:
                        string withdrawMsg = handler.HandleWithdraw(account);
                        screen.ShowMessage(withdrawMsg, withdrawMsg.Contains("successful") ? ConsoleColor.Green : ConsoleColor.Red);
                        Console.WriteLine("Press [Enter] to continue...");
                        Console.ReadLine();
                        break;

                    case 4:
                        string exitMsg = handler.HandleExit();
                        screen.ShowMessage(exitMsg, ConsoleColor.Yellow);
                        screen.ShowLoading(5);
                        return;
                }
            }
            while (true);
        }
    }
}
