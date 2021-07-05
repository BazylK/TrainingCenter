using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TrainingCenter.Model;
using TrainingCenter.AdditionalClasses;

namespace TrainingCenter.DAL
{
    class SchoolInitializer : DropCreateDatabaseIfModelChanges<CenterContext>
    {
        protected override void Seed(CenterContext context)
        {
            var users = new List<Account>
            {
                new Account{Email="admin",Password=PasswordHasher.Hash("admin"),FirstName="admin",LastName="admin",
                    SignUpDate=new DateTime(2000,5,1,8,30,52), AdressStreet="adminowa",AdressNumber="123"
                    ,Country="Poland",City="Adminowo", AccountType= "admin"},
                new Account{Email="a@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Marek",LastName="Wojecki",
                    SignUpDate=new DateTime(2001,2,4,8,12,24), AdressStreet="Mickiewicza",AdressNumber="12"
                    ,Country="Poland",City="Rzeszów", AccountType= "normal"},
                new Account{Email="b@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Wojciech",LastName="Torecki",
                    SignUpDate=new DateTime(2006,1,12,20,44,30), AdressStreet="Sikorskiego",AdressNumber="133/14"
                    ,Country="Poland",City="Rzeszów", AccountType= "normal"},
                new Account{Email="c@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="John",LastName="Smith",
                    SignUpDate=new DateTime(2012,10,22,10,1,30), AdressStreet="15th",AdressNumber="16"
                    ,Country="England",City="London", AccountType= "normal"},
                new Account{Email="d@wp.pl",Password=PasswordHasher.Hash("123"),FirstName="Vladimir",LastName="Putin",
                    SignUpDate=new DateTime(2016,1,22,10,1,30), AdressStreet="Tverskaya",AdressNumber="1"
                    ,Country="Russia",City="Moscow", AccountType= "normal"},
            };
            users.ForEach(s => context.Accounts.Add(s));

            context.SaveChanges();
        }
    }
}
