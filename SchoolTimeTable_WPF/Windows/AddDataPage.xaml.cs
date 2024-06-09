using MySql.Data.MySqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddDataPage.xaml
    /// </summary>
    public partial class AddDataPage : Page
    {
        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        MySqlDataReader dr;
        List<String> subjects;
        List<String> teachers;
        public AddDataPage()
        {
            InitializeComponent();
            DataContext = this;
            subjects = new List<String>();
            teachers = new List<String>();

            ListCategoriesTeachers();
            ListCategoriesClasses();
        }
        

        public ObservableCollection<ComboBoxSubject> Items { get { return _items; } }
        private ObservableCollection<ComboBoxSubject> _items = new ObservableCollection<ComboBoxSubject>();
        public ObservableCollection<ComboBoxTeacher> ItemsTeacher { get { return _itemsTeacher; } }
        private ObservableCollection<ComboBoxTeacher> _itemsTeacher = new ObservableCollection<ComboBoxTeacher>();

        private void addSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            Items.Add(new ComboBoxSubject());
            ListCategoriesSubject();
        }
        private void addTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsTeacher.Add(new ComboBoxTeacher());
            ListCategoriesTeacher();
        }
        private void ListCategoriesTeachers()
        {
            teacherComboBox.Items.Clear();
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"Select Teachers.teacher_name From Teachers;";
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    teacherComboBox.Items.Add(dr.GetString("teacher_name").ToString());
                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Произошла ошибка во время загрузки данных!");
            }
            finally
            {
                dr.Close();
                con.Close();
            }
        }
        private void ListCategoriesClasses()
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"Select Classes.class_name From Classes;";
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    classComboBox.Items.Add(dr.GetString("class_name").ToString());
                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Произошла ошибка во время загрузки данных!");
            }
            finally
            {
                dr.Close();
                con.Close();
            }
        }
        private void ListCategoriesSubject()
        {
            ComboBoxSubject.cbItems = new ObservableCollection<ComboBoxItem>();
            var cbItem = new ComboBoxItem { Content = "<--Select-->" };
            ComboBoxSubject.SelectedcbItem = cbItem;
            ComboBoxSubject.cbItems.Add(cbItem);
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"Select Lessons.lesson_name From Lessons " +
                                  $"WHERE Lessons.lesson_id not in " +
                                  $"(SELECT Lesson_Class.lesson_id FROM Lesson_Class, Classes " +
                                  $"WHERE Classes.class_name = '{ classComboBox.Text }' AND Classes.class_id = Lesson_Class.class_id);";
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ComboBoxSubject.cbItems.Add(new ComboBoxItem { Content = dr.GetString("lesson_name") });
                }

                dr.Close();
            }
            catch
            {
                con.Close();
                MessageBox.Show("Произошла ошибка во время загрузки данных!");
            }
            finally
            {
                con.Close();
            }
        }
        private void ListCategoriesTeacher()
        {
            ComboBoxTeacher.cbItemsTeacher = new ObservableCollection<ComboBoxItem>();
            var cbItem = new ComboBoxItem { Content = "<--Select-->" };
            ComboBoxTeacher.SelectedcbItemTeacher = cbItem;
            ComboBoxTeacher.cbItemsTeacher.Add(cbItem);
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"Select Lessons.lesson_name From Lessons " +
                                  $"WHERE Lessons.lesson_id not in " +
                                  $"(SELECT Lesson_Teacher.lesson_id FROM Lesson_Teacher, Teachers " +
                                  $"WHERE Teachers.teacher_name = '{ teacherComboBox.Text }' AND Teachers.teacher_id = Lesson_Teacher.teacher_id);";
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ComboBoxTeacher.cbItemsTeacher.Add(new ComboBoxItem { Content = dr.GetString("lesson_name") });
                }

                dr.Close();
            }
            catch
            {
                con.Close();
                MessageBox.Show("Произошла ошибка во время загрузки данных!");
            }
            finally
            {
                con.Close();
            }
        }

        private void ComboBoxTeacher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   if (ComboBoxTeacher.SelectedcbItemTeacher != null)
            {
                if (teachers == null || !teachers.Contains(ComboBoxTeacher.SelectedcbItemTeacher.Content.ToString()))
                    teachers.Add( ComboBoxTeacher.SelectedcbItemTeacher.Content.ToString());
            }
        }
        private void ComboBoxSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (subjects == null || !subjects.Contains(ComboBoxSubject.SelectedcbItem.Content.ToString()))
                subjects.Add(ComboBoxSubject.SelectedcbItem.Content.ToString());
        }

        private void deleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxTeacher.SelectedItem != null)
            {
                ItemsTeacher.Remove(listBoxTeacher.SelectedItem as ComboBoxTeacher);
            }
        }
        private void deleteSubject_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxSubject.SelectedItem != null)
            {
                Items.Remove(listBoxSubject.SelectedItem as ComboBoxSubject);
            }
        }
        private void ComboBox_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void classComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Items.Clear();
            subjects.Clear();
        }

        private void teacherComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemsTeacher.Clear();
            teachers.Clear();
        }

        private void addDataButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new MySqlCommand();
            cmd.Connection = con;
            string message = "";
            bool stop = false;
            if (!String.IsNullOrWhiteSpace(subjectTextBox.Text) && !String.IsNullOrWhiteSpace(teacherTextBox.Text))
            {
                cmd.CommandText = $"INSERT INTO Lessons (lesson_id, lesson_name) " +
                                  $"VALUES (NULL, '{subjectTextBox.Text}');" +
                                  $"INSERT INTO Teachers (teacher_id, teacher_name) " +
                                  $"VALUES (NULL, '{teacherTextBox.Text}')";
                message = "Предмет и преподаватель добавлены";
            }
            else if (!String.IsNullOrWhiteSpace(subjectTextBox.Text) && String.IsNullOrWhiteSpace(teacherTextBox.Text))
            {
                cmd.CommandText = $"INSERT INTO Lessons (lesson_id, lesson_name) " +
                                  $"VALUES (NULL, '{subjectTextBox.Text}');";
                message = "Предмет добавлен";
            }
            else if (String.IsNullOrWhiteSpace(subjectTextBox.Text) && !String.IsNullOrWhiteSpace(teacherTextBox.Text))
            {
                cmd.CommandText = $"INSERT INTO Teachers (teacher_id, teacher_name) " +
                                  $"VALUES (NULL, '{teacherTextBox.Text}')";
                message = "Преподаватель добавлен";
            }
            else
            {
                stop = true;
                MessageBox.Show("Заполните хотя бы одно поле ввода!", Title="Ошибка");
            }
            if (!stop)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(message, Title = "Успех");
                }
                catch
                {
                    MessageBox.Show("Произошка ошибка во время добавления данных!", Title = "Ошибка");
                }
                finally
                {
                    subjectTextBox.Text = null;
                    teacherTextBox.Text = null;
                    con.Close();
                    ListCategoriesTeachers();
                }
            }
        }

        private void addData2Button_Click(object sender, RoutedEventArgs e)
        {
            cmd = new MySqlCommand();
            cmd.Connection = con;
            string message = "";
            try
            {

                con.Open();
                if ((!String.IsNullOrWhiteSpace(teacherComboBox.Text) && teachers.Count != 0) && (!String.IsNullOrWhiteSpace(classComboBox.Text) && subjects.Count != 0))
                {
                    foreach (var item in teachers)
                    {
                        cmd.CommandText = $"INSERT INTO Lesson_Teacher (lesson_id, teacher_id) " +
                                          $"VALUES ((SELECT Lessons.lesson_id FROM Lessons " +
                                          $"WHERE Lessons.lesson_name='{item}'), (SELECT teacher_id " +
                                          $"FROM Teachers WHERE teacher_name='{teacherComboBox.Text}'));";
                        cmd.ExecuteNonQuery();
                    }
                    foreach (var item in subjects)
                    {
                        cmd.CommandText = $"INSERT INTO Lesson_Class (lesson_id, class_id) " +
                                          $"VALUES ((SELECT Lessons.lesson_id FROM Lessons " +
                                          $"WHERE Lessons.lesson_name='{item}'), " +
                                          $"(SELECT Classes.class_id FROM Classes " +
                                          $"WHERE Classes.class_name='{classComboBox.Text}'))";
                        cmd.ExecuteNonQuery();
                    }
                    message = "Предмет добавлен к преподавателю и предмет добавлен к классу";
                    MessageBox.Show(message, Title = "Успех");
                }
                else if ((!String.IsNullOrWhiteSpace(teacherComboBox.Text) && teachers.Count != 0) && (String.IsNullOrWhiteSpace(classComboBox.Text) || subjects.Count == 0))
                {
                    foreach (var item in teachers)
                    {
                        cmd.CommandText = $"INSERT INTO Lesson_Teacher (lesson_id, teacher_id) " +
                                          $"VALUES ((SELECT Lessons.lesson_id FROM Lessons " +
                                          $"WHERE Lessons.lesson_name='{item}'), (SELECT teacher_id " +
                                          $"FROM Teachers WHERE teacher_name='{teacherComboBox.Text}'));";
                        cmd.ExecuteNonQuery();
                    }
                    message = "Предмет добавлен к преподавателю";
                    MessageBox.Show(message, Title = "Успех");
                }
                else if ((String.IsNullOrWhiteSpace(teacherComboBox.Text) || teachers.Count == 0) && (!String.IsNullOrWhiteSpace(classComboBox.Text) && subjects.Count != 0))
                {
                    foreach (var item in subjects)
                    {
                        cmd.CommandText = $"INSERT INTO Lesson_Class (lesson_id, class_id) " +
                                          $"VALUES ((SELECT Lessons.lesson_id FROM Lessons " +
                                          $"WHERE Lessons.lesson_name='{item}'), " +
                                          $"(SELECT Classes.class_id FROM Classes " +
                                          $"WHERE Classes.class_name='{classComboBox.Text}'))";
                        cmd.ExecuteNonQuery();
                    }
                    message = "Предмет добавлен к классу";
                    MessageBox.Show(message, Title = "Успех");
                }
                else
                {
                    MessageBox.Show("Заполните хотя бы одно поле ввода!", Title = "Ошибка");
                }
            }
            catch
            {
                MessageBox.Show("Произошка ошибка во время добавления данных!", Title = "Ошибка");
            }
            finally
            {
                con.Close();
                teacherComboBox.SelectedItem = null;
                classComboBox.SelectedItem = null;
            }
            
        }
    }
    public class ComboBoxTeacher : DependencyObject
    {
        public static ComboBoxItem SelectedcbItemTeacher { get; set; }
        public static ObservableCollection<ComboBoxItem> cbItemsTeacher { get; set; }
    }
    public class ComboBoxSubject : DependencyObject
    {
        public static ComboBoxItem SelectedcbItem { get; set; }
        public static ObservableCollection<ComboBoxItem> cbItems { get; set; }
    }
}
