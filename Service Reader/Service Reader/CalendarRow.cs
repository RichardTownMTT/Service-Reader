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
    }
}
