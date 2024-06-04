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
    /// Логика взаимодействия для FirstStep_1.xaml
    /// </summary>
    public partial class FirstStep : UserControl
    {
        public delegate void Reload();
        public event Reload ReloadStep;

        public delegate void ChangeProgress();
        public event ChangeProgress Increase;
        public event ChangeProgress Decrease;

        SingletonSchedule schedule = SingletonSchedule.GetInstance();
        SecondStep secondStep;

        public FirstStep()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (Decrease != null)
                Decrease();

            if (ReloadStep != null)
                ReloadStep();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            secondStep = new SecondStep();
            secondStep.Reload += ReloadList;
            secondStep.Increase += ProgressIncrease;
            secondStep.Decrease += ProgressDecrease;
            
            if (checkCollection.Children
                .OfType<CheckBox>()
                .All(c => c.IsChecked == false))
                errorLabel.Visibility = Visibility.Visible;
            else
            {
                errorLabel.Visibility = Visibility.Hidden;

                schedule.Input.Records = new List<Record>();

                foreach (var num in numCollection.Children
                    .OfType<Xceed.Wpf.Toolkit.IntegerUpDown>()
                    .Where(n => n.IsVisible == true))
                {
                    while (num.Value > 0)
                    {
                        Record r = new Record();
                       
                        r.ClassName = num.Name.Remove(0, 3);
                        schedule.Input.Records.Add(r);
                        num.Value--;
                    }
                }

                secondStep.SetN(schedule.Input.Records.Count);

                userControl.Children.Add(secondStep);

                if (Increase != null)
                    Increase();

                secondStep.LoadClassNumber(0);
            }
        }

        public void Load(bool input)
        {
            checkBox1.IsEnabled = input;
        }

        public void ReloadList()
        {
            userControl.Children.Remove(secondStep);
            schedule.Input.Records.Clear();

            foreach (var checkBox in checkCollection.Children
                .OfType<CheckBox>()
                .Where (cb => cb.IsChecked == true))
            {
                checkBox.IsChecked = false;
            }

            foreach (var num in numCollection.Children
                .OfType<Xceed.Wpf.Toolkit.IntegerUpDown>())
            {
                num.Value = 1;
                num.Visibility = Visibility.Hidden;
            }
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            foreach (var checkBox in checkCollection.Children
                .OfType<CheckBox>()
                .Where(cb => cb.Name != "checkBoxAll"))
            {
                var numName = $"num{checkBox.Content}";

                var num = numCollection.Children
                     .OfType<Xceed.Wpf.Toolkit.IntegerUpDown>()
                     .Where(n => n.Name == numName)
                     .Single();

               if (checkBox.IsChecked == true)
               {
                    num.Visibility = Visibility.Visible;
               }
               else
               {
                    num.Visibility = Visibility.Hidden;
               }

               if (checkCollection.Children
                    .OfType<CheckBox>()
                    .All(c => c.IsChecked == false))
                    label.Visibility = Visibility.Hidden;
               else
                    label.Visibility = Visibility.Visible;
            }
        }

        private void checkBoxAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var checkBox in checkCollection.Children
                .OfType<CheckBox>()
                .Where(cb => cb.Name != "checkBoxAll" && cb.IsEnabled))
            {
                checkBox.IsChecked = checkBoxAll.IsChecked; 
            }
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
