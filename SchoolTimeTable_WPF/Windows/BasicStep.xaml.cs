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

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для BasicStep.xaml
    /// </summary>
    public partial class BasicStep : UserControl
    {
        public delegate void ChangeProgress();
        public event ChangeProgress Increase;
        public event ChangeProgress Decrease;

        FirstStep firstStep;
        SingletonSchedule schedule = SingletonSchedule.GetInstance();

        public BasicStep()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            schedule.Input.StudyingSystem = 5;
            firstStep = new FirstStep();
            firstStep.Load(true);
            userControl.Children.Add(firstStep);
            firstStep.ReloadStep += Reload;
            firstStep.Increase += ProgressIncrease;
            firstStep.Decrease += ProgressDecrease;

            if (Increase != null)
                Increase();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            schedule.Input.StudyingSystem = 6;
            firstStep = new FirstStep();
            firstStep.Load(false);
            userControl.Children.Add(firstStep);
            firstStep.ReloadStep += Reload;
            firstStep.Increase += ProgressIncrease;
            firstStep.Decrease += ProgressDecrease;

            if (Increase != null)
                Increase();
        }

        public void Reload()
        {
            userControl.Children.Remove(firstStep);
        }

        public void ProgressIncrease()
        {
            if (Increase != null)
                Increase();
        }

        public void ProgressDecrease()
        {
            if (Decrease != null)
                Decrease();
        }
    }
}
