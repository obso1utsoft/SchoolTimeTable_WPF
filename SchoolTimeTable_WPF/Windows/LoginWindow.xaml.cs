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

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        MySqlDataReader dr;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(loginBox.Text) || String.IsNullOrWhiteSpace(passBox.Text))
            {
                hintText.Visibility = Visibility.Visible;
            }
            else
            {
                hintText.Visibility = Visibility.Hidden;
                try
                {
                    cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = $"Select * From Users Where login='{loginBox.Text}' And password='{passBox.Text}'";
                    con.Open();
                    dr = cmd.ExecuteReader();
                    if (dr.Read() && dr["role"].ToString() == "Администратор")
                    {
                        MessageBox.Show($"Вы вошли как {dr["role"].ToString().ToLower()}", Title = "Успех!");
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Вы вошли как {dr["role"].ToString().ToLower()}", Title = "Успех!");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                }
                catch
                {
                    con.Close();
                    MessageBox.Show("Неверный логин или пароль!", Title = "Ошибка!");
                }
                finally
                {
                    dr.Close();
                    con.Close();
                }
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

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }
    }
}
