using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter.Model;
using System.Net.Mail;

namespace TrainingCenter.AdditionalClasses
{
    public class DataCheck
    {
        public static bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool isUserValid(Account account)
        {
            bool test = false;
            test = account.GetType().GetProperties()
               .Where(a => a.GetValue(account) is string)
               .Select(a => (string)a.GetValue(account))
               .Any(value => String.IsNullOrEmpty(value));
            return test;
        }
    }
}
