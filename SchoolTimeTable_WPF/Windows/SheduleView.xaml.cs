using Mysqlx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static SchoolTimeTable_WPF.ScheduleGenerator;

namespace SchoolTimeTable_WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для SheduleView.xaml
    /// </summary>
    public partial class SheduleView : UserControl
    {

        List<List<List<Lesson>>> schedules = ScheduleGenerator.schedules;
        DataTable dt;
        public SheduleView()
        {
            InitializeComponent();

            if (!(schedules[5]?.Any() ?? true) || SingletonSchedule.GetInstance().Input.StudyingSystem == 5)
            { sixDay.Visibility = Visibility.Collapsed; }
            else { sixDay.Visibility = Visibility.Visible; }

            bool IsColumnEmpty;

            if (schedules != null)
            {
                for (int a = 0; a < schedules.Count; a++)
                {
                    var dg = new DataGrid();
                    dt = new DataTable();
                    this.gridLabel(a).Children.Add(dg);

                    for (int c = 0; c < schedules[a].Count; c++)
                    {
                        dt.Columns.Add((c + 1).ToString() + " Класс");
                        if (schedules[a][c].Count > 0)
                        {
                            for (int i = 0; i < schedules[a][c].Count; i++)
                            {
                                DataRow row;
                                if (i < dt.Rows.Count)
                                {
                                    row = dt.Rows[i];
                                }
                                else
                                {
                                    row = dt.NewRow();
                                    dt.Rows.Add(row);
                                }

                                row[c] = schedules[a][c][i].Subject;
                            }
                        }
                    }
                    for (int i = dt.Columns.Count - 1; i >= 0; i--)
                    {
                        IsColumnEmpty = dt.AsEnumerable().All(dr => string.IsNullOrEmpty(dr[i].ToString()));

                        if (IsColumnEmpty)
                            dt.Columns.RemoveAt(i);
                    }
                    dg.ItemsSource = dt.DefaultView;
                }
            }
        }
        public Grid gridLabel(int i)
        {
            Grid f = grid1Day;
            switch (i)
            {
                case 0: f = grid1Day; break;
                case 1: f = grid2Day; break;
                case 2: f = grid3Day; break;
                case 3: f = grid4Day; break;
                case 4: f = grid5Day; break;
                case 5: f = grid6Day; break;
            }
            return f;
        }
    }
}

