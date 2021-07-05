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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrainingCenter.Model;
using TrainingCenter.DAL;
using TrainingCenter.AdditionalClasses;

namespace TrainingCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Account logedInAccount;
        DatabaseManager db;
        
        public MainWindow()
        {
            InitializeComponent();
            db = new DatabaseManager();
        }
        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            if (db.isAccountInDatabase(tbEmail.Text))
            {
                Account accountCheck = db.findAccountWithEmail(tbEmail.Text);
                if (PasswordHasher.Verify(pbPassword.Password, accountCheck.Password))
                {
                    logedInAccount = accountCheck;
                    MainMenu newWindow = new MainMenu();
                    this.Close();
                    newWindow.Show();
                }
                else
                {
                    MessageBox.Show("Złe hasło");
                }
            }
            else
            {
                MessageBox.Show("Niepoprawne dane");
            }
        }

        private void btNewAccount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
