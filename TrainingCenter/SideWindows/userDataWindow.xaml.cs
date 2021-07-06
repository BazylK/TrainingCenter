using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrainingCenter.AdditionalClasses;
using TrainingCenter.Model;
using TrainingCenter.DAL;

namespace TrainingCenter.SideWindows
{
    /// <summary>
    /// Interaction logic for userDataWindow.xaml
    /// </summary>
    public partial class userDataWindow : Window
    {
        DatabaseManager db;

        public userDataWindow()
        {
            if (MainWindow.zalogowany ==true)
            { 
                this.Title = "Zmiana danych";
                lbTitle.Content = "Zmień swoje dane osobowe";
                Account logAcc = db.findAccountWithEmail(MainWindow.logedInAccount.Email);
                tbEmail.Text = logAcc.Email;
                tbFirstName.Text = logAcc.FirstName;
                tbLastName.Text = logAcc.LastName;
            }
            else
            {

            }
            InitializeComponent();
            db = new DatabaseManager();
        }

         Account createAccountFromTB()
        {
            Account newAcc = new Account()
            {
                Email = tbEmail.Text,
                Password = PasswordHasher.Hash(tbPassword1.Password),
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                SignUpDate = DateTime.Now,
                AdressStreet = tbStreet.Text,
                AdressNumber = tbHouseNr.Text,
                City = tbCity.Text,
                Country = tbCountry.Text,
                AccountType = "Student",
            };
            return newAcc;
        }

        void CreateUser()
        {
            Account newAcc = createAccountFromTB();
            if (tbPassword1.Password != tbPassword2.Password)
            {
                MessageBox.Show("Hasła muszą być takie same", "Błąd");
            }
            else if (!DataCheck.IsEmailValid(tbEmail.Text))
            {
                MessageBox.Show("Niepoprawny adres email", "Błąd");
            }
            else if (DataCheck.isUserValid(newAcc))
            {
                MessageBox.Show("Wypełnij wszystkie pola", "Błąd");
            }
            else if (!db.isEmailAvailable(tbEmail.Text))
            {
                MessageBox.Show("Email jest zajęty", "Błąd");
            }
            else
            {
                db.addAccount(newAcc);
                MessageBox.Show("Konto utworzono", "Sukces");
                this.Close();
            }
        }

        private void btCreate_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.zalogowany==true) //jak zalogowany to edytuje dane, jak nie to tworzy
            {

            }
            else
            {
                CreateUser();
            }
           
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
