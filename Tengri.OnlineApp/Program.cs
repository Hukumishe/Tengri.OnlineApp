using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Tengri.ServiceUser;

namespace Tengri.OnlineApp
{
    class Program
    {
        private static ILog log = LogManager.GetLogger("LOGGER");

        const string pathForDb = @"C:\Users\nikfe\OneDrive\Рабочий стол\Tengri.OnlineApp-master\Tengri.OnlineApp\bin\Debug\bank.db";

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ServiceMenu.WelcomeMenu();

            string login = "";
            string password = "";
            ServiceMenu.EnterLoginMenu(out login, out password);
           
            try
            {
                ServicesUser service = new ServicesUser(pathForDb);
                ServiceAccount.SettingsAccount serviceAccount = new Tengri.ServiceAccount.SettingsAccount(pathForDb);
                User user = service.GetUser(login, password);
                if (user != null && user.status == 0)
                {
                    ServiceMenu.WelcomeUserMenu(serviceAccount,user);
                    ServiceMenu.AuthUserMenu(serviceAccount, user);
                }
                else if (user != null && user.status == 2)
                {
                    Console.WriteLine("учетная запись заблокирована");
                }
                else
                {
                    ServiceMenu.NotAuthUserMenu();
                    string choice = Console.ReadLine();

                    if (choice == "да")
                    {
                        Console.Clear();

                        user = new User();
                        Console.WriteLine("-------------------------");
                        Console.WriteLine("Форма регистрации");
                        Console.WriteLine("-------------------------");
                        Console.Write("Логин: ");
                        user.login = Console.ReadLine();

                        Console.Write("Пароль: ");
                        user.password = Console.ReadLine();

                        Console.Write("Полное имя: ");
                        user.fullname = Console.ReadLine();

                        if (service.Registration(user))
                        {
                            Console.Clear();
                            welcomeMsg(user.fullname);
                        }
                        else
                        {
                            Console.WriteLine("При регистрации возникла ошибка!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void welcomeMsg(string fullName)
        {
            Console.WriteLine("Welcome " + fullName + "!");
        }
    }
}
