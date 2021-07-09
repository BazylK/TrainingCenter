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
        Course selectedCourse;
        Course selectedCourseToSign;

        ObservableCollection<Account> listAccounts;
        ObservableCollection<Account> listAccountsSearch;
        ObservableCollection<Account> listTeachers;
        ObservableCollection<Course> listCourses;
        ObservableCollection<Course> listCoursesToSignUp;
        ObservableCollection<Course> listCoursesAddSearch1;
        ObservableCollection<Course> listCoursesAddSearch2;
        ObservableCollection<CourseStudents> listMyCourses;
        ObservableCollection<Lesson> listMyLessons;



        DatabaseManager db;
        public static int courseManagementId;

        public MainMenu()
        {
            InitializeComponent();
            db = new DatabaseManager();

            

            refreshAccountList();
            refreshCoursesAddList();
            SetUIElements();
        }



        void SetUIElements()
        {
            lbWelcomeMessage.Content = "Witaj ponownie, " + 
                MainWindow.logedInAccount.FirstName + " " + 
                MainWindow.logedInAccount.LastName + "!";
            if (MainWindow.logedInAccount.AccountType=="Admin")
            {
                listBoxCoursesMyCourses.IsEnabled = false;
            }
            if (MainWindow.logedInAccount.AccountType=="Teacher")
            {
                listBoxCoursesMyCourses.IsEnabled = false;
                tabAccounts.Visibility = Visibility.Collapsed;
                btAddCourse.IsEnabled = false;
                btEditCourse.IsEnabled = false;
                btRemoveCourse.IsEnabled = false;
            }
        }

        ////////////////
        ////Obsługa list do listview i listboxow
        ///////////
        void refreshAccountList()
        {
            //lista kont glowna
            listAccounts = new ObservableCollection<Account>(db.getAccountList());

            // pobieram z listy kont liste nauczucieli zeby je mozna wybrac w comboboxie
            listTeachers = new ObservableCollection<Account>(listAccounts.Where
                (x => x.AccountType.Equals("Teacher") || x.AccountType.Equals("Admin")));

            cbLeadingTeacher.ItemsSource = listTeachers;
            listViewAccounts.ItemsSource = listAccounts;
            searchAccount();
        } //odswieza listview z kontami
        /// <summary>
        /// ZARZADZANIE KONTAMI TAB1
        /// </summary>
        void refreshCoursesAddList()
        {
            //lista kursow na ktore sie mozna zapisac do zakladki kursy zapisz sie
            listCoursesToSignUp = new ObservableCollection<Course>(db.getCourseList());
            if (MainWindow.logedInAccount.AccountType == "Admin")
            { //lista kursow dla admina (wszystkie)
                listCourses = new ObservableCollection<Course>(db.getCourseList());
                //w zakladcie MojeKursy admin widzi te ktore prowadzi
                //listMyCourses = new ObservableCollection<CourseStudents>
                    //(db.getMyCoursesImLeading(MainWindow.logedInAccount.Email));
            }
            else if (MainWindow.logedInAccount.AccountType == "Teacher")
            {//lista kursow dla nauczyciela (tylko te ktore prowadzi)
                listCourses = new ObservableCollection<Course>(db.getCourseList
                    (MainWindow.logedInAccount.Email));

                //w zakladcie MojeKursy nauczuciel widzi te ktore prowadzi
                //listMyCourses = new ObservableCollection<CourseStudents>
                    //(db.getMyCoursesImLeading(MainWindow.logedInAccount.Email));
            }
            else if (MainWindow.logedInAccount.AccountType == "Student")
            {
                listCourses = new ObservableCollection<Course>(db.getCourseList
                    (MainWindow.logedInAccount.Email));
                listMyCourses = new ObservableCollection<CourseStudents>(
                    db.getMyCourses(MainWindow.logedInAccount.Email));
                listBoxCoursesMyCourses.ItemsSource = listMyCourses;

            }

            listMyLessons = new ObservableCollection<Lesson>(
                db.getMyLessons(MainWindow.logedInAccount.AccountId));
            listBoxMyLessons.ItemsSource = listMyLessons;
            listBoxCoursesSignUp.ItemsSource = listCoursesToSignUp;
            searchCourseAdd();
        }

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
            db.addObjToDB(createAccountFromTextBox());
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
                db.editObjInDB(editedAccount);
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
                db.removeObjFromDB(selectedAccount);
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

        /// <summary>----------------------------------------------------------------
        /// KURSY ADD/REMOVE TAB2
        /// </summary>
        
        void searchCourseAdd()
        { //najpierw sprawdzam czy checkbox jest klikniety i filtruje wedlug niego
            if(checkBoxPokazZakonczone.IsChecked == false)
            {
                listCoursesAddSearch1 = new ObservableCollection<Course>
                    (listCourses.Where(x => x.Status.Equals("In progress") ||
                    x.Status.Equals("New")));
            }
            else
            {
                listCoursesAddSearch1 = new ObservableCollection<Course>(listCourses);
            }
            //drugi filtr to sprawdzenie po tym co jest wpisane w search boxie
            if (tbSearchCourseAdd.Text == "")
            {
                listViewCoursesAdd.ItemsSource = listCoursesAddSearch1;
            }
            else
            {
                listCoursesAddSearch2 = new ObservableCollection<Course>
                 (listCoursesAddSearch1.Where(x => x.Title.Contains(tbSearchCourseAdd.Text)
                 || x.LeadingTeacher.LastName.Contains(tbSearchCourseAdd.Text)
                 || x.Status.Contains(tbSearchCourseAdd.Text)));
                listViewCoursesAdd.ItemsSource = listCoursesAddSearch2;
            }
        }

        Course createCourseFromTextBox()
        {
            Course course = new Course
            {
                Title = tbCourseName.Text,
                Description = tbCourseDescription.Text,
                Status = cbCourseStatus.Text,
                LeadingTeacher = (Account)cbLeadingTeacher.SelectedItem,
            };
            return course;
        }
        private void btAddCourse_Click(object sender, RoutedEventArgs e)
        {
            db.addObjToDB(createCourseFromTextBox());
            refreshCoursesAddList();
        }
        
        private void btEditCourse_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCoursesAdd.SelectedValue != null)
            {
                Course editedCourse = createCourseFromTextBox();
                editedCourse.CourseId = selectedCourse.CourseId;
                db.editObjInDB(editedCourse);
                refreshCoursesAddList();
            }
            else
            {
                MessageBox.Show("Wybierz pozycję do edycji", "Błąd");
            }
        }

        private void btRemoveCourse_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCoursesAdd.SelectedValue != null)
            {
                db.removeObjFromDB(selectedCourse);
                refreshCoursesAddList();
            }
            else
            {
                MessageBox.Show("Wybierz pozycję do usunięcia", "Błąd");
            }
        }

        private void listViewCoursesAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewCoursesAdd.SelectedValue != null)
            {
                selectedCourse = listViewCoursesAdd.SelectedItem as Course;
                this.DataContext = selectedCourse;
                courseManagementId = selectedCourse.CourseId;
            }
        }

        private void tbSearchCourseAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchCourseAdd();
        }
        private void checkBoxPokazZakonczone_Click(object sender, RoutedEventArgs e)
        {
            searchCourseAdd();
        }

        private void btLessonsManagement_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCoursesAdd.SelectedValue != null)
            {
                LessonsManagement lmWindow = new LessonsManagement();
                lmWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Zaznacz kurs żeby sprawdzić listę zajęć", "Błąd");
            }
        }

        private void btStudentsManagement_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCoursesAdd.SelectedValue != null)
            {
                StudentsManagment smWindow = new StudentsManagment();
                smWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Zaznacz kurs żeby sprawdzić listę uczestników", "Błąd");
            }
        }

        private void listBoxCoursesSignUp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxCoursesSignUp.SelectedValue !=null)
            {
                selectedCourseToSign = listBoxCoursesSignUp.SelectedItem as Course;
                tbRequestNote.IsEnabled = true;
            }
            else
            {
                tbRequestNote.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (db.isUserSignedUpAlready(selectedCourseToSign.CourseId, MainWindow.logedInAccount.AccountId))
            {
                MessageBox.Show("Już jesteś zapisany/a na ten kurs", "Błąd");
            }
            else
            {
                CourseStudents newCS = new CourseStudents
                {
                    Course = selectedCourseToSign,
                    Student = db.getAccountWithId(MainWindow.logedInAccount.AccountId),
                    Status = "signed_up",
                    Notes = tbRequestNote.Text,
                };
                db.addObjToDB(newCS);
                MessageBox.Show("Wysłano zgłoszenie na kurs");
            }
        }
    }
}
