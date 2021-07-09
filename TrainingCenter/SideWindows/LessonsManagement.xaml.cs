using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for LessonsManagement.xaml
    /// </summary>
    public partial class LessonsManagement : Window
    {
        ObservableCollection<Lesson> listLessons;
        Lesson selectedLesson;
        DatabaseManager db;
        public LessonsManagement()
        {
  
            InitializeComponent();
            db = new DatabaseManager();
            refreshUI();
            //tworzenie triggerow zeby podswietlic odpowiednie daty
            List<DateTime> wyroznionedaty = listLessons.Select(a => a.LessonStart).ToList();
            Style s = (Style)this.Resources["cdbStyle"];
            foreach (var a in wyroznionedaty)
            {
                DateTime holidayDate = a.Date;
                DataTrigger dataTrigger = new DataTrigger() { Binding = new Binding("Date"), Value = holidayDate };
                dataTrigger.Setters.Add(new Setter(CalendarDayButton.BackgroundProperty, Brushes.SandyBrown));
                s.Triggers.Add(dataTrigger);
            }
        }

        void refreshUI()
        {
            listLessons = new ObservableCollection<Lesson>(db.getLessonsForCourse(MainMenu.courseManagementId)
                .OrderBy(i => i.LessonStart.Date));
            listViewLessons.ItemsSource = listLessons;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        Lesson createLessonFromTb()
        {

            TimeSpan tsStart = TimeSpan.Parse(tbStartHour.Text + ":" + tbStartMin.Text);
            TimeSpan tsEnd = TimeSpan.Parse(tbEndHour.Text + ":" + tbEndMin.Text);
            DateTime dateTimeStart = (DateTime)datePickerLesson.SelectedDate + tsStart;
            MessageBox.Show(dateTimeStart.ToString());
            DateTime dateTimeEnd = (DateTime)datePickerLesson.SelectedDate + tsEnd;
            Lesson newLesson = new Lesson()
            {
                CourseLesson = db.getCourseWithId(MainMenu.courseManagementId),
                RoomNr = tbRoomNr.Text,
                LessonStart = dateTimeStart,
                LessonEnd = dateTimeEnd,
            };
            return newLesson;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            db.addObjToDB(createLessonFromTb());
            refreshUI();
        }
        

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            if (listViewLessons.SelectedValue != null)
            {
                Lesson editedLesson = createLessonFromTb();
                editedLesson.LessonId = selectedLesson.LessonId;
                db.editObjInDB(editedLesson);
                refreshUI();
            }
            else
            {
                MessageBox.Show("Wybierz pozycję do edycji", "Błąd");
            }
        }

        private void btRemove_Click(object sender, RoutedEventArgs e)
        {
            if (listViewLessons.SelectedValue != null)
            {
                db.removeObjFromDB(selectedLesson);
                refreshUI();
            }
            else
            {
                MessageBox.Show("Wybierz pozycję do usunięcia", "Błąd");
            }
        }

        private void listViewLessons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewLessons.SelectedValue != null)
            {
                selectedLesson = listViewLessons.SelectedItem as Lesson;
                this.DataContext = selectedLesson;
                datePickerLesson.SelectedDate = selectedLesson.LessonStart.Date;
            }
        }

        private void calendarControl_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            datePickerLesson.SelectedDate = calendarControl.SelectedDate;
            if (listLessons.Any(a => a.LessonStart.Date == calendarControl.SelectedDate))
            {
                selectedLesson = listLessons.FirstOrDefault(a => a.LessonStart.Date == calendarControl.SelectedDate);
                listViewLessons.SelectedItem = selectedLesson;
                this.DataContext = selectedLesson;
            }
            Mouse.Capture(null);
        }
    }
}
