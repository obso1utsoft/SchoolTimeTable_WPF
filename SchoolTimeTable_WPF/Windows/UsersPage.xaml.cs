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
using static Mysqlx.Datatypes.Scalar.Types;

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        MySqlConnection con = new MySqlConnection("Server=127.0.0.1;Database=school_schedule;user=root;charset=utf8");
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        public UsersPage()
        {
            InitializeComponent();
            LoadDataGrid();
        }
        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }
        public void LoadDataGrid()
        {
            try
            {
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT user_id, login, password, role FROM Users";
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

            string user = "";
            for (int i = 0; i < dataGrid.SelectedItems.Count; i++)
            {
                System.Data.DataRowView selectedFile = (System.Data.DataRowView)dataGrid.SelectedItems[i];
                user = Convert.ToString(selectedFile.Row.ItemArray[0]);
            }
            if (user != "1" )
            {
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result;

                result = MessageBox.Show("Вы уверены, что хотите удалить запись?", "Удаление", buttons,
                MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        cmd = new MySqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = $"DELETE FROM Users WHERE Users.user_id = '{user}';" +
                                          $"UPDATE Users SET Users.user_id = Users.user_id-1  WHERE Users.user_id>'{user}';" +
                                          $"ALTER TABLE Users AUTO_INCREMENT = 1";
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
            else
            {
                MessageBox.Show("Нельзя удалить главного администратора!", Title = "Ошибка");
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddUser addUser = new AddUser();
            addUser.ShowDialog();
        }
    }
}
