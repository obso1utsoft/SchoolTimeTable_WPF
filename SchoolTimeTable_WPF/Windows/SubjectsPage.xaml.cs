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
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static SchoolTimeTable_WPF.Windows.ClassesPage;

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
                cmd.CommandText = $"SELECT lesson_id, lesson_name FROM Lessons";
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

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxResult result;

            result = MessageBox.Show("Вы уверены, что хотите удалить запись?", "Удаление", buttons,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string lessons = "";
                    for (int i = 0; i < dataGrid.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView selectedFile = (System.Data.DataRowView)dataGrid.SelectedItems[i];
                        lessons = Convert.ToString(selectedFile.Row.ItemArray[0]);
                    }
                    cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"DELETE FROM Lesson_Class WHERE Lesson_Class.lesson_id = '{ lessons }';" +
                                      $"DELETE FROM Lesson_Teacher WHERE Lesson_Teacher.lesson_id = '{ lessons }';" +
                                      $"DELETE FROM Lessons WHERE Lessons.lesson_id = '{lessons}';" +
                                      $"UPDATE Lessons SET Lessons.lesson_id = Lessons.lesson_id-1  WHERE Lessons.lesson_id>'{ lessons }';" +
                                      $"ALTER TABLE Lessons AUTO_INCREMENT = 1";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Запись удалена!", Title = "Успех");
                }
                catch
                {
                    con.Close();
                    MessageBox.Show("Произошла ошибка во время удаления записи!", Title = "Ошибка");
                }
                finally
                {
                    con.Close();
                }
                LoadDataGrid();
            }
        }
    }
}
