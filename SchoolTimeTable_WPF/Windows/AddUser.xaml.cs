using MySql.Data.MySqlClient;
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
using static SchoolTimeTable_WPF.Windows.teacherPage;

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        public AddUser()
        {
            InitializeComponent();
        }

        private void addDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(loginTextBox.Text) && !String.IsNullOrWhiteSpace(passTextBox.Text))
            {
                try
                {
                    cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"INSERT INTO Users (user_id, login, password, role) VALUES (NULL, '{ loginTextBox.Text }', '{ passTextBox.Text }', '{ roleComboBox.Text }')";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Пользователь добавлен!", Title = "Успех");
                }
                catch
                {
                    con.Close();
                    MessageBox.Show("Произошла ошибка при добавлении пользователя в базу данных!", Title = "Ошибка");
                }
                finally
                {
                    con.Close();
                    loginTextBox.Text = "";
                    passTextBox.Text = "";
                    roleComboBox.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля ввода!", Title = "Ошибка");
            }
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
    }
}
