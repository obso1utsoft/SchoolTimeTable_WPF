using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using Google.Protobuf.Compiler;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для SecondStep.xaml
    /// </summary>
    public partial class SecondStep : UserControl
    {
        public delegate void ReloadList();
        public event ReloadList Reload;

        public delegate void ChangeProgress();
        public event ChangeProgress Increase;
        public event ChangeProgress Decrease;

        public static string serialized;

        SingletonSchedule schedule;
        private int n;
        private int i = 1;
        SheduleView sheduleView;

        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        MySqlDataReader dr;


        public SecondStep()
        {
            InitializeComponent();
            schedule = SingletonSchedule.GetInstance();
        }

        private void ListCategories()
        {
            tb_subject.Items.Clear();
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"Select Lesson_Class.lesson_id, Lesson_Class.class_id, Lessons.lesson_name From Lesson_Class, Lessons " +
                                  $"Where Lesson_Class.lesson_id=Lessons.lesson_id AND Lesson_Class.class_id='{ classNumber.Text }';";
                con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var lessons = new Lessons
                    {
                        LessonId = dr.GetInt32("lesson_id"),
                        LessonName = dr.GetString("lesson_name")
                    };
                    tb_subject.Items.Add(lessons);
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


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (Reload != null)
                Reload();

            if (Decrease != null)
                Decrease();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (className.Text != "")
            {
                errorLabel.Visibility = Visibility.Hidden;
                schedule.Input.Records[i - 1].ClassName += className.Text;

                if (i < n)
                {
                    schedule.Input.Records[i].Appointments = new List<Appointment>();
                    LoadClassNumber(i);
                    i++;
                }
                else
                {
                    serialized = JsonConvert.SerializeObject(schedule.Input);

                    SheduleGen.Generate();

                    button2.Visibility = Visibility.Hidden;

                    sheduleView = new SheduleView();

                    userControl.Children.Add(sheduleView);

                    if (Increase != null)
                        Increase();
                }
            }
            
            else
            {
                errorLabel.Visibility = Visibility.Visible;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            SaveAttachment(i);

            tb_subject.Text = "";
            tb_teacher.Text = "";
            iud_hours.Value = 1;
        }

        public void SaveAttachment(int index)
        {
            Appointment a = new Appointment();
            a.Subject = tb_subject.Text;
            a.Teacher = tb_teacher.Text;
            a.NumOfHours = iud_hours.Value ?? 0;

            dg_attachments.Items.Add(a);

            schedule.Input.Records[i - 1].Appointments.Add(a);
        }

        public void LoadClassNumber(int position)
        {
            classNumber.Text = schedule.Input.Records[position].ClassName.ToString();
            tb_subject.Text = "";
            tb_teacher.Text = "";
            iud_hours.Value = 1;
            className.Text = "";
            
            while (dg_attachments.Items.Count > 0)
            {
                dg_attachments.Items.RemoveAt(0);
            }

            ListCategories();
        }

        public void SetN(int n)
        {
            schedule.Input.Records[0].Appointments = new List<Appointment>();
            this.n = n;
        }


        private void tb_subject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_teacher.Items.Clear();
            if (tb_subject.SelectedItem is Lessons lessons)
            {
                try
                {
                    cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"Select Teachers.teacher_id, Teachers.teacher_name From Teachers, Lesson_Teacher " +
                                      $"Where Teachers.teacher_id=Lesson_Teacher.teacher_id " +
                                      $"AND Lesson_Teacher.lesson_id=(Select Lessons.lesson_id From Lessons Where Lessons.lesson_name='{ lessons.LessonName }')";
                    con.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var teacher = new Teacher
                        {
                            TeacherId = dr.GetInt32("teacher_id"),
                            TeacherName = dr.GetString("teacher_name")
                        };
                        tb_teacher.Items.Add(teacher);
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
            
        }
    }
    public class Lessons
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }

        public override string ToString()
        {
            return LessonName;
        }
    }
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }

        public override string ToString()
        {
            return TeacherName;
        }
    }
    
}
