using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для SubjectsPage.xaml
    /// </summary>
    public partial class SubjectsPage : Page
    {
        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        public SubjectsPage()
        {
            InitializeComponent();
            LoadDataGrid();
        }
        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }
        private void LoadDataGrid()
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT Classes.class_id, Classes.class_name, GROUP_CONCAT(Lessons.lesson_name SEPARATOR ', ') AS lessons " +
                                  $"FROM Classes, Lesson_Class, Lessons WHERE Classes.class_id=Lesson_Class.class_id " +
                                  $"AND Lessons.lesson_id=Lesson_Class.lesson_id GROUP BY Classes.class_id;";
                con.Open();
                dt = new DataTable();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch
            {
                con.Close();
                MessageBox.Show("Произошла ошибка во время загрузки данных!", Title = "Ошибка");
            }
            finally
            {
                con.Close();
            }
        }
    }
}
