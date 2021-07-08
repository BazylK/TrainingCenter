using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for LessonsManagement.xaml
    /// </summary>
    public partial class LessonsManagement : Window
    {
        ObservableCollection<Lesson> listLessons;
        DatabaseManager db;
        public LessonsManagement()
        {
            InitializeComponent();
            db = new DatabaseManager();
            refreshUI();
        }

        void refreshUI()
        {
            listLessons = new ObservableCollection<Lesson>(db.getLessonsForCourse(MainMenu.courseManagementId));
            listViewLessons.ItemsSource = listLessons;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
