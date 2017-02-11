using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class CalendarRow : ObservableObject
    {

        private DateTime m_currentRowStartOfWeek;
        private DateTime m_monthFirstDay;
        private int m_rowNumber;
        private ObservableCollection<CalendarDay> m_days;

        public CalendarRow(DateTime currentRowStartOfWeek, DateTime monthFirstDay, int rowNumberEntry)
        {
            CurrentRowStartOfWeek = currentRowStartOfWeek;
            MonthFirstDay = monthFirstDay;
            RowNumber = rowNumberEntry;

            createDays();
        }

        private void createDays()
        {
            DateTime endOfWeek = CurrentRowStartOfWeek.AddDays(6);
            DateTime currentDay = CurrentRowStartOfWeek;
            Days = new ObservableCollection<CalendarDay>();
            int counter = 0;
            CalendarDay day;
            while (counter < 7)
            {
                day = new CalendarDay(currentDay);
                Days.Add(day);
                counter++;
                currentDay = currentDay.AddDays(1);
            }
        }

        public DateTime CurrentRowStartOfWeek
        {
            get
            {
                return m_currentRowStartOfWeek;
            }

            set
            {
                m_currentRowStartOfWeek = value;
            }
        }

        public DateTime MonthFirstDay
        {
            get
            {
                return m_monthFirstDay;
            }

            set
            {
                m_monthFirstDay = value;
            }
        }

        public int RowNumber
        {
            get
            {
                return m_rowNumber;
            }

            set
            {
                m_rowNumber = value;
            }
        }

        public ObservableCollection<CalendarDay> Days
        {
            get
            {
                return m_days;
            }

            set
            {
                m_days = value;
                //onPropertyChanged("Days");
            }
        }

        public void updateOnsiteDays(Dictionary<DateTime, List<DbEmployee>> onsiteDaysForWeek)
        {
            foreach (var day in Days)
            {
                DateTime currentDay = day.CurrentDay.Date;
                var engineers = (from onsiteDays in onsiteDaysForWeek
                                where onsiteDays.Key == currentDay
                                select onsiteDays.Value).FirstOrDefault();

                //No engineers may have submitted for that day.
                if (engineers == null)
                {
                    continue;
                }

                foreach (var currentEng in engineers)
                {
                    string engInitials = currentEng.Firstname.Substring(0, 1) + currentEng.Surname.Substring(0, 1);
                    CalendarDay requiredDay = Days.Where(x => x.CurrentDay == currentDay).FirstOrDefault();

                    if (requiredDay == null)
                    {
                        throw new Exception("Cannot find calendar day " + currentDay);
                    }
                    requiredDay.EngineersInitials.Add(engInitials);
                }
            }
        }

        public void updateMissingDays(Dictionary<DateTime, List<DbEmployee>> missingDaysForWeek)
        {
            foreach (var day in Days)
            {
                DateTime currentDay = day.CurrentDay.Date;
                var engineers = (from missingDays in missingDaysForWeek
                                 where missingDays.Key == currentDay
                                 select missingDays.Value).FirstOrDefault();

                //No engineers may have submitted for that day.
                if (engineers == null)
                {
                    continue;
                }

                foreach (var currentEng in engineers)
                {
                    string engInitials = currentEng.Firstname.Substring(0, 1) + currentEng.Surname.Substring(0, 1);
                    CalendarDay requiredDay = Days.Where(x => x.CurrentDay == currentDay).FirstOrDefault();

                    if (requiredDay == null)
                    {
                        throw new Exception("Cannot find calendar day " + currentDay);
                    }
                    requiredDay.MissingEngineersInitials.Add(engInitials);
                }
            }
        }
    }
}
