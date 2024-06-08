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
using static SchoolTimeTable_WPF.Windows.teacherPage;

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для ClassesPage.xaml
    /// </summary>
    public partial class ClassesPage : Page
    {
        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        MySqlDataReader dr;
        public ClassesPage()
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
                cmd.CommandText = $"SELECT Classes.class_name, GROUP_CONCAT(Lessons.lesson_name SEPARATOR ', ') AS lessons " +
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
                    string classes = "";
                    List<Classes> list = new List<Classes>();
                    for (int i = 0; i < dataGrid.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView selectedFile = (System.Data.DataRowView)dataGrid.SelectedItems[i];
                        classes = Convert.ToString(selectedFile.Row.ItemArray[0]);
                    }
                    cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT Classes.class_id, Lessons.lesson_id " +
                                      $"FROM Classes, Lesson_Class, Lessons " +
                                      $"WHERE Classes.class_id=Lesson_Class.class_id " +
                                      $"AND Lessons.lesson_id=Lesson_Class.lesson_id " +
                                      $"AND Classes.class_id = (" +
                                      $"SELECT Classes.class_id FROM Classes " +
                                      $"WHERE Classes.class_name = '{ classes }');";
                    con.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var classesid = new Classes
                        {
                            ClassesId = (int)dr["class_id"],
                            LessonId = (int)dr["lesson_id"]
                        };
                        list.Add(classesid);
                    }
                    dr.Close();
                    foreach (var item in list)
                    {
                        cmd.CommandText = $"DELETE FROM Lesson_Class WHERE Lesson_Class.lesson_id = '{item.LessonId}' AND Lesson_Class.class_id = '{item.ClassesId}'";
                        cmd.ExecuteNonQuery();
                    }
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
        public class Classes
        {
            public int ClassesId { get; set; }
            public int LessonId { get; set; }
        }
    }
}
