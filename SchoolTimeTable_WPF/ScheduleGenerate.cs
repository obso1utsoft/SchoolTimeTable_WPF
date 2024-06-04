using Newtonsoft.Json.Linq;
using SchoolTimeTable_WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static SchoolTimeTable_WPF.ScheduleGenerator;

namespace SchoolTimeTable_WPF
{
    class SheduleGen
    {
        public static void Generate()
        {

            SingletonSchedule schedule;
            schedule = SingletonSchedule.GetInstance();

            List<List<Subject>> subjects = new List<List<Subject>>();

            for (int a = 0; a < 11; a++)
                subjects.Add(new List<Subject>());

            foreach (var item in schedule.Input.Records)
            {
                var trimmed = Regex.Replace(item.ClassName, @"[A-Za-zА-Яа-я]+", "");
                foreach (var subject in item.Appointments)
                {
                    subjects[Int32.Parse(trimmed) - 1].Add(new Subject(subject.Subject + $" ({ subject.Teacher })", subject.NumOfHours, 2));
                }
            }

            ScheduleGenerator generator = new ScheduleGenerator(subjects);

            StartThread(generator);
        }
        private static void StartThread(ScheduleGenerator generator)
        {
            Thread t = new Thread(generator.GenerateSchedule);
            t.Start();
            if (!t.Join(10000)) // give the operation 10s to complete
            {
                // the thread did not complete on its own, so we will abort it now
                MessageBox.Show("Процесс выполнялся слишком долго и его пришлось остановить, попробуйте ввести другие данные");
                t.Abort();
            }
        }
    }


    public class ScheduleGenerator
    {

        private int DaysPerWeek = SingletonSchedule.GetInstance().Input.StudyingSystem;

        private List<List<Subject>> subjects;
        public static List<List<List<Lesson>>> schedules;
        private List<List<double>> totalHours;

        public ScheduleGenerator(List<List<Subject>> subjects)
        {
            this.subjects = subjects;
            schedules = new List<List<List<Lesson>>>(DaysPerWeek);
            totalHours = new List<List<double>>();
            for (int a = 0; a < 11; a++)
                totalHours.Add(new List<double>());
            for (int i = 0; i < DaysPerWeek; i++)
            {
                schedules.Add(new List<List<Lesson>>());
                for (int a = 0; a < 11; a++)
                    schedules[i].Add(new List<Lesson>());
            }
        }
        public void GenerateSchedule()
        {
            for (int d = 0; d < DaysPerWeek; d++)
            {
                for (int k = 0; k < subjects.Count; k++)
                {
                    if (subjects[k]?.Any() ?? false)
                    {
                        totalHours[k].Add(subjects[k].Sum(s => s.Hour));
                        double countSubject = totalHours[k][0] / DaysPerWeek;
                        for (int a = 0; a < Math.Ceiling(countSubject) && 0 < (int)totalHours[k].Last(); a++)
                        {
                            foreach (Subject subject in subjects[k])
                            {
                                if (subject.Count != 0)
                                {
                                    Lesson lesson = new Lesson(subject.Id);
                                    schedules[d][k].Add(lesson);
                                    subject.Hour--;
                                    subject.Count--;
                                    break;
                                }
                            }
                            subjects[k] = subjects[k].OrderByDescending(s => s.Hour).ToList();
                        }
                        foreach (Subject subject in subjects[k])
                        {
                            subject.Count = 2;
                        }
                    }
                }
                Sortize(d);
            }
        }
        public void Sortize(int d)
        {
        Restart:
            for (int v = 0; v < schedules[d].Count; v++)
            {
                while (!schedules[d][v]?.Any() ?? true)
                {
                    v++;
                    if (v >= schedules[d].Count)
                        break;
                }
                int x = 1;
                for (; x <= schedules[d].Count; x++)
                {
                    if (v + x >= schedules[d].Count)
                        break;
                    while (schedules[d][v + x]?.Any() ?? true)
                    {
                        for (int a = 0; a < schedules[d][v].Count; a++)
                        {
                            if ((a < schedules[d][v + x].Count) && (schedules[d][v][a].Subject == schedules[d][v + x][a].Subject))
                            {
                                List<Lesson> temp = new List<Lesson>(schedules[d][v]);
                                while (temp.SequenceEqual(schedules[d][v]))
                                    Shuffle(schedules[d][v]);
                                goto Restart;
                            }
                        }
                        x++;
                        if (v + x == schedules[d].Count)
                            break;
                    }
                }
            }
        }
        static List<T> Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        public class Lesson
        {
            public string Subject { get; set; }

            public Lesson(string subject)
            {
                Subject = subject;
            }
        }
        public class Subject
        {
            public string Id { get; set; }
            public int Hour { get; set; }
            public int Count { get; set; }

            public Subject(string id, int hour, int count)
            {
                Id = id;
                Hour = hour;
                Count = count;
            }
        }
    }
}
