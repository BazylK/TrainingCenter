using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TrainingCenter.Model;

namespace TrainingCenter.SideWindows
{
    /// <summary>
    /// Interaction logic for StudentsManagment.xaml
    /// </summary>
    public partial class StudentsManagment : Window
    {
        ObservableCollection<Account> listStudents;
        ObservableCollection<Account> listAccountsSearch;
        ObservableCollection<CourseStudents> listCourseStudents;
        ObservableCollection<CourseStudents> listCSsignedUp;
        ObservableCollection<CourseStudents> listCSApproved;
        CourseStudents selectedCStoAccept;
        Account selectedAccToAdd;
        CourseStudents selectedCStoRemove;
        Course currentcourse; 

        DatabaseManager db;
        public StudentsManagment()
        {
            InitializeComponent();
            db = new DatabaseManager();
            currentcourse = db.getCourseWithId(MainMenu.courseManagementId);
            refreshLists();
        }

        void refreshLists()
        {
            listCourseStudents = new ObservableCollection<CourseStudents>(db.getCourseStudentsList(currentcourse.CourseId));
            listCSsignedUp = new ObservableCollection<CourseStudents>
                (listCourseStudents.Where(x => x.Status.Equals("signed_up")));
            listCSApproved = new ObservableCollection<CourseStudents>
                (listCourseStudents.Where(x => x.Status.Equals("approved")));
            listStudents = new ObservableCollection<Account>
                (db.getAccountList().Where(x => x.AccountType.Equals("Student")));
            foreach (CourseStudents item in listCSApproved.ToList())
            {
                Account acc = listStudents.FirstOrDefault(a => a.Email == item.Student.Email);
                listStudents.Remove(acc);
            }
            listViewPendingSignUps.ItemsSource = listCSsignedUp;
            listViewUsersInCourse.ItemsSource = listCSApproved;
            searchAccount();
        }

        private void btAccept_Click(object sender, RoutedEventArgs e)
        {
            if (listViewPendingSignUps.SelectedValue != null)
            {
                selectedCStoAccept.Status = "approved";
                db.editObjInDB(selectedCStoAccept);
                refreshLists();
            }
            else
            {
                MessageBox.Show("Zaznacz użytkownika do akceptacji","Błąd");
            }
        }

        private void btDecline_Click(object sender, RoutedEventArgs e)
        {
            if (listViewPendingSignUps.SelectedValue != null)
            {
                selectedCStoAccept.Status = "declined";
                db.editObjInDB(selectedCStoAccept);
                refreshLists();
            }
            else
            {
                MessageBox.Show("Zaznacz użytkownika do odrzucenia","Błąd");
            }
        }

        private void listViewPendingSignUps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCStoAccept = listViewPendingSignUps.SelectedItem as CourseStudents;
        }

        private void btAddToCourse_Click(object sender, RoutedEventArgs e)
        {
            if (listViewUsersList.SelectedValue != null)
            {
                CourseStudents newCS = new CourseStudents
                {
                    Course = currentcourse,
                    Student = selectedAccToAdd,
                    Status = "approved"
                };
                db.removeSignUpIfExists(currentcourse, selectedAccToAdd);
                db.addObjToDB(newCS);
                refreshLists();
            }
            else
            {
                MessageBox.Show("Zaznacz użytkownika do dodania", "Błąd");
            }
        }

        private void btRemoveFromCourse_Click(object sender, RoutedEventArgs e)
        {
            if (listViewUsersInCourse.SelectedValue != null)
            {
                db.removeObjFromDB(selectedCStoRemove);
                refreshLists();
            }
            else
            {
                MessageBox.Show("Zaznacz użytkownika do usunięcia z kursu", "Błąd");
            }
        }

        private void listViewUsersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAccToAdd = listViewUsersList.SelectedItem as Account;
        }

        private void listViewUsersInCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCStoRemove = listViewUsersInCourse.SelectedItem as CourseStudents;
        }

        void searchAccount()
        {
            if (tbSearch.Text == "")
            {
                listViewUsersList.ItemsSource = listStudents;
            }
            else
            {
                listAccountsSearch = new ObservableCollection<Account>
                 (listStudents.Where(x => x.FirstName.Contains(tbSearch.Text)
                 || x.LastName.Contains(tbSearch.Text)
                 || x.Email.Contains(tbSearch.Text)));
                listViewUsersList.ItemsSource = listAccountsSearch;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchAccount();
        }
    }
}
