using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolTimeTable_WPF
{
    class SingletonSchedule
    {
        private Input input = new Input();

        private static readonly Lazy<SingletonSchedule> lazy = new Lazy<SingletonSchedule>(() => new SingletonSchedule());

        internal Input Input { get => input; set => input = value; }

        public static SingletonSchedule GetInstance()
        {
            return lazy.Value;
        }
    }

    class Input
    {
        public int StudyingSystem { get; set; }
        public List<Record> Records { get; set; }
    }

    class Record
    {
        public string ClassName { get; set; }
        public List<Appointment> Appointments { get; set; }
    }

    class Appointment
    {
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public int NumOfHours { get; set; }
    }
}
