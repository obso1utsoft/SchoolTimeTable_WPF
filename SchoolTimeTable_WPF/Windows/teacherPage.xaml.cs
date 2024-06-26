﻿using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
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
    /// Логика взаимодействия для teacherPage.xaml
    /// </summary>
    public partial class teacherPage : Page
    {
        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        MySqlDataReader dr;
        public teacherPage()
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
                cmd.CommandText = $"SELECT Teachers.teacher_name, GROUP_CONCAT(Lessons.lesson_name SEPARATOR ', ') AS lessons " +
                                  $"FROM Teachers, Lesson_Teacher, Lessons WHERE Teachers.teacher_id=Lesson_Teacher.teacher_id " +
                                  $"AND Lessons.lesson_id=Lesson_Teacher.lesson_id GROUP BY Teachers.teacher_id;";
                con.Open();
                dt = new DataTable();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch
            {
                con.Close();
                MessageBox.Show("Произошла ошибка во время загрузки данных!", Title="Ошибка");
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
                    string teacher = "";
                    List<Teachers> list = new List<Teachers>();
                    for (int i = 0; i < dataGrid.SelectedItems.Count; i++)
                    {
                        System.Data.DataRowView selectedFile = (System.Data.DataRowView)dataGrid.SelectedItems[i];
                        teacher = Convert.ToString(selectedFile.Row.ItemArray[0]);
                    }
                    cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT Teachers.teacher_id, Lessons.lesson_id " +
                                      $"FROM Teachers, Lesson_Teacher, Lessons " +
                                      $"WHERE Teachers.teacher_id=Lesson_Teacher.teacher_id " +
                                      $"AND Lessons.lesson_id=Lesson_Teacher.lesson_id " +
                                      $"AND Teachers.teacher_id = (" +
                                      $"SELECT Teachers.teacher_id FROM Teachers " +
                                      $"WHERE Teachers.teacher_name = '{teacher}')";
                    con.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var teachers = new Teachers
                        {
                            TeacherId = (int)dr["teacher_id"],
                            LessonId = (int)dr["lesson_id"]
                        };
                        list.Add(teachers);
                    }
                    dr.Close();
                    foreach (var item in list)
                    {
                        cmd.CommandText = $"DELETE FROM Lesson_Teacher WHERE Lesson_Teacher.lesson_id = '{item.LessonId}' AND Lesson_Teacher.teacher_id = '{item.TeacherId}';" +
                                          $"DELETE FROM Teachers WHERE Teachers.teacher_id = '{list[0].TeacherId}';";
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
        public class Teachers
        {
            public int TeacherId { get; set; }
            public int LessonId { get; set; }
        }
    }
}
