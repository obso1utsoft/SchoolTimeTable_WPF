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

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            teacherPage teacherPage = new teacherPage();
            mainFrame.Content = teacherPage;
        }
        private void subjectsButton_Click(object sender, RoutedEventArgs e)
        {
            SubjectsPage subjectsPage = new SubjectsPage();
            mainFrame.Content = subjectsPage;
        }
        private void teacherButton_Click(object sender, RoutedEventArgs e)
        {
            teacherPage teacherPage = new teacherPage();
            mainFrame.Content = teacherPage;
        }
        private void classesButton_Click(object sender, RoutedEventArgs e)
        {
            ClassesPage classesPage = new ClassesPage();
            mainFrame.Content = classesPage;
        }
        private void usersButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddDataPage addDataPage = new AddDataPage();
            mainFrame.Content = addDataPage;
        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
