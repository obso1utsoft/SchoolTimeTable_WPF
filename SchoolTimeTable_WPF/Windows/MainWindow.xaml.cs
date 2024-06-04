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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SchoolTimeTable_WPF.Windows;

namespace SchoolTimeTable_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BasicStep basicStep = new BasicStep();
        public MainWindow()
        {
            InitializeComponent();
            userControl.Children.Add(basicStep);

            basicStep.Increase += IncreaseProgress;
            basicStep.Decrease += DecreaseProgress;
        }

        public void IncreaseProgress()
        {
            progressBar.Value += 30;
        }

        public void DecreaseProgress()
        {
            progressBar.Value -= 30;
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
