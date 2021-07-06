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
using TrainingCenter.DAL;
using TrainingCenter.AdditionalClasses;
using TrainingCenter.Model;
using System.Collections.ObjectModel;
using TrainingCenter.SideWindows;

namespace TrainingCenter
{
    public partial class MainMenu : Window
    {
        Account selectedAccount;

        ObservableCollection<Account> listAccounts;
        ObservableCollection<Account> listAccountsSearch;

        DatabaseManager db;

        public MainMenu()
        {
            InitializeComponent();
            db = new DatabaseManager();

            lbWelcomeMessage.Content = "Witaj ponownie, " + MainWindow.logedInAccount.FirstName + " " + MainWindow.logedInAccount.LastName + "!";

            refreshAccountList();
        }

        void refreshAccountList()
        {
            listAccounts = new ObservableCollection<Account>(db.getAccountList());
            listViewAccounts.ItemsSource = listAccounts;
            searchAccount();
        } //odswieza listview z kontami

        private void listViewAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAccount = listViewAccounts.SelectedItem as Account;
            this.DataContext = selectedAccount;
        }

        Account createAccountFromTextBox()
        {
            Account acc = new Account
            {
                Email = tbEmail.Text,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                AccountType = cbAccountType.Text,
                SignUpDate = DateTime.Now,
                AdressStreet = tbStreet.Text,
                AdressNumber = tbHouseNr.Text,
                City = tbCity.Text,
                Country = tbCountry.Text,
                Notes = tbNotes.Text
            };
            return acc;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            db.addAccount(createAccountFromTextBox());
            refreshAccountList();
        }

        private void btEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listViewAccounts.SelectedValue != null)
            {
                Account editedAccount = createAccountFromTextBox();
                editedAccount.SignUpDate = selectedAccount.SignUpDate;
                editedAccount.Password = selectedAccount.Password;
                editedAccount.AccountId = selectedAccount.AccountId;
                db.editAccount(editedAccount);
                refreshAccountList();
            }
            else
            {
                MessageBox.Show("Wybierz pozycję do edycji", "Błąd");
            }
        }

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            if (listViewAccounts.SelectedValue != null)
            {
                db.removeAccount(selectedAccount);
                refreshAccountList();
            }
            else
            {
                MessageBox.Show("Wybierz pozycję do usunięcia", "Błąd");
            }
        }

        void searchAccount()
        {
            if (tbSearchAccount.Text == "")
            {
                listViewAccounts.ItemsSource = listAccounts;
            }
            else
            {
                listAccountsSearch = new ObservableCollection<Account>
                 (listAccounts.Where(x => x.FirstName.Contains(tbSearchAccount.Text)
                 || x.LastName.Contains(tbSearchAccount.Text)
                 || x.Email.Contains(tbSearchAccount.Text)));
                listViewAccounts.ItemsSource = listAccountsSearch;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           searchAccount();
        }

        private void byLogOut_Click(object sender, RoutedEventArgs e)
        {//wylogowanie
            MainWindow newWindow = new MainWindow();
            this.Close();
            newWindow.Show();
        }

        private void btEditData_Click(object sender, RoutedEventArgs e)
        {//zmiej swoje dane
            userDataWindow uDW = new userDataWindow();
            uDW.ShowDialog();
        }
    }
}
