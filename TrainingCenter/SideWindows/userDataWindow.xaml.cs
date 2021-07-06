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
        String emailBefore; //zapisuje poprzedni email zeby wiedziec czy jest inny
        Account logAcc;

        public userDataWindow()
        {
            InitializeComponent();
            db = new DatabaseManager();
            FillTextBoxes();
        }

        void FillTextBoxes()
        {
            if (MainWindow.zalogowany == true)
            {
                this.Title = "Zmiana danych";
                btCreate.Content = "Zapisz";
                lbTitle.Content = "Zmień dane swojego konta";
                logAcc = db.findAccountWithEmail(MainWindow.logedInAccount.Email);
                tbEmail.Text = logAcc.Email;
                tbFirstName.Text = logAcc.FirstName;
                tbLastName.Text = logAcc.LastName;
                tbStreet.Text = logAcc.AdressStreet;
                tbHouseNr.Text = logAcc.AdressNumber;
                tbCity.Text = logAcc.City;
                tbCountry.Text = logAcc.Country;
                emailBefore = logAcc.Email;
            }
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
                Account editedAccount = createAccountFromTB();
                if (tbPassword1.Password != tbPassword2.Password)
                {
                    MessageBox.Show("Hasła muszą być takie same", "Błąd");
                    
                }
                else if (tbEmail.Text != emailBefore && !db.isEmailAvailable(tbEmail.Text))
                {
                    MessageBox.Show("Email jest zajęty", "Błąd");
                }
                else
                {
                    if (tbPassword1.Password!=null)
                    {
                        editedAccount.Password = PasswordHasher.Hash(tbPassword1.Password);
                    }
                    else
                    {
                        editedAccount.Password = logAcc.Password;
                    }
                    editedAccount.SignUpDate = logAcc.SignUpDate;
                    editedAccount.AccountId = logAcc.AccountId;
                    db.editAccount(editedAccount);
                    MessageBox.Show("Pomyślnie zmieniono dane!");
                    this.Close();
                }

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
