using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolTimeTable_WPF.Model
{
    public class Plan
    {
        public Plan()
        {
            LessonsViewModels = new List<LessonsViewModel>();
        }

        public int DaysPerWeek { get; set; }
        public int HoursPerDay { get; set; }
        public ICollection<LessonsViewModel> LessonsViewModels { get; set; }
    }

    public class LessonsViewModel
    {
        public int Day { get; set; }
        public int Hour { get; set; }
        public string Class { get; set; }
        public string Teacher { get; set; }
        public string Subject { get; set; }
    }
}
