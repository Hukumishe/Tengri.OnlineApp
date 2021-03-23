using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengri.ServiceAccount;
using Tengri.ServiceUser;

namespace Tengri.OnlineApp
{
    public static class ServiceMenu
    {
        public static void WelcomeMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Добро пожалось на платформу Тенгри банка");
            Console.WriteLine("----------------------------------------");
        }

        public static void EnterLoginMenu(out string login, out string password)
        {
            Console.Write("Введите логин: ");
            login = Console.ReadLine();

            Console.Write("Введите пароль: ");
            password = Console.ReadLine();
        }

        public static void WelcomeUserMenu(ServiceAccount.SettingsAccount serviceAccount, User user)
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать "+ user .fullname+ "!");
            Console.WriteLine("----------------------------------------");
            AuthUserMenu(serviceAccount,user);
        }

        public static void NotAuthUserMenu()
        {
            Console.WriteLine("Вы ввели некорректный логин или пароль!");
            Console.WriteLine("");
            Console.Write("Забыли пароль (да/нет): ");
            string ch = Console.ReadLine();
            if(ch == "да")
            {

            }
            else
            {
                Console.Write("Желаете пройти регистрацию (да/нет): ");
            }            
        }

        public static void AuthUserMenu(ServiceAccount.SettingsAccount serviceAccount, User user)
        {
            Console.WriteLine("1. Список счетов");
            Console.WriteLine("2. Пополнить счёт");
            Console.WriteLine("3. Перевести деньги");
            Console.WriteLine("4. Создать счёт");
            Console.WriteLine("5. Выход");

            int userInput = int.Parse(Console.ReadLine());


            switch (userInput)
            {
                case 1:
                    {
                        Console.Clear();
                        foreach (Account acc in serviceAccount.GetUserAccounts(user.id))
                        {
                            Console.WriteLine(acc.Id + "." + acc.IBAN + " - "+ acc.Balance);
                        }
                    }
                    break;

                case 2:
                    {
                        foreach (Account acc in serviceAccount.GetUserAccounts(user.id))
                        {
                            Console.WriteLine(acc.Id + "." + acc.IBAN + " - " + acc.Balance);
                        }

                        Console.WriteLine("Выберите счёт: ");
                        int accId = int.Parse(Console.ReadLine());

                        
                        Console.WriteLine("Введите сумму пополнения счета: ");
                        decimal addMoney = Convert.ToDecimal(Console.ReadLine());

                        serviceAccount.AddMoney(accId, addMoney);
                    }
                    break;

                case 3:
                    {
                        foreach (Account acc in serviceAccount.GetUserAccounts(user.id))
                        {
                            Console.WriteLine(acc.Id + "." + acc.IBAN + " - " + acc.Balance);
                        }

                        Console.WriteLine("Выберите счёт с которого хотите перевести деньги: ");
                        int accIdFrom = int.Parse(Console.ReadLine());

                        Console.WriteLine("Выберите счёт который хотите пополнить: ");
                        int accIdTo = int.Parse(Console.ReadLine());


                        Console.WriteLine("Введите сумму пополнения счета: ");
                        decimal sum = Convert.ToDecimal(Console.ReadLine());

                        serviceAccount.TransferMoney(accIdFrom, accIdTo, sum);
                    }
                    break;

                case 4:
                    if (serviceAccount.CreateAccount(user.id, out ServiceAccount.Account account))
                        Console.WriteLine("Поздравляем! Ваш счет успешно создан!");

                    else
                        Console.WriteLine("Вы ввели неверные данные :( Попробуйте снова");
                    break;

                case 5:
                    {
                        Environment.Exit(1);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
