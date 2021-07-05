using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter.Model;

namespace TrainingCenter.DAL
{
    class DatabaseManager
    {
        CenterContext db;

        public DatabaseManager()
        {
            db = new CenterContext();
        }

        public List<Account> getAccountList()
        {
            var accounts = (from p in db.Accounts select p).ToList();
            return accounts;
        }
        public void addAccount(Account account)
        {
            db.Accounts.Add(account);
            db.SaveChanges();
        }
        public void removeAccount(Account account)
        {
            db.Accounts.Remove(account);
            db.SaveChanges();
        }
        public void editAccount(Account account)
        {
            db.Accounts.AddOrUpdate(account);
            db.SaveChanges();
        }
    }
}
