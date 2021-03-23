using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengri.ServiceAccount
{
    public class SettingsAccount
    {
        private Tengri.DAL.liteDbEntity db = null;
        private ServiceUser.ServicesUser servicesUser = null;

        public SettingsAccount(string connectionString)
        {
            db = new DAL.liteDbEntity(connectionString);
            servicesUser = new ServiceUser.ServicesUser(connectionString);
        }

        /// <summary>
        /// Метод который возвращает список счетов пользователя
        /// </summary>
        /// <param name="userId">ID пользователя в системе</param>
        /// <returns>Коллекцию счетов типа List</returns>
        public List<Account> GetUserAccounts(int userId)
        {
            List<Account> accounts = new List<Account>();

            accounts = db.GetCollection<Account>();

            return accounts
                .Where(w => w.UserId == userId)
                .ToList();
        }

        public bool CreateAccount(int userId, out Account account)
        {
            account = new Account();
            string message = "";
            Random rnd = new Random();
            if (userId == 0 || userId < 0)
                throw new Exception("Пользователь не существует!");
            else if(servicesUser.IsExistUser(userId))
            {
                account.UserId = userId;
                account.IBAN = "KZ"+rnd.Next(1,100);
                

                if (db.Create<Account>(account, out message))
                {
                    return true;
                }

                else
                    throw new Exception(message);
                
            }

            return false;
        }

        public bool AddMoney(int accId, decimal sum)
        {
            string message = "";
            List<Account> accounts = db.GetCollection<Account>();
            Account count = accounts.Where(w=>w.Id == accId).FirstOrDefault();
            
            if(count != null)
            {
                count.Balance += sum;
            }

            return db.UpDate<Account>(count, out message);
        }

        public bool TransferMoney(int accIdFrom, int accIdTo, decimal sum)
        {
            string message = "";
            List<Account> accounts = db.GetCollection<Account>();
            Account accountFrom = accounts.Where(w => w.Id == accIdFrom).FirstOrDefault();
            Account accountTo = accounts.Where(w => w.Id == accIdTo).FirstOrDefault();

            if (accountFrom != null & accountTo != null & accountFrom.Balance >= sum)
            {
                accountFrom.Balance -= sum;
                accountTo.Balance += sum;
            }

            db.UpDate<Account>(accountTo, out message);
            return db.UpDate<Account>(accountFrom, out message);
        }
    }
}
