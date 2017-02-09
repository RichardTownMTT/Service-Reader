using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader 
{
    public class CalendarDay : ObservableObject
    {
        private DateTime m_currentDay;
        private ObservableCollection<string> m_engineersInitials;
        private ObservableCollection<string> m_missingEngineersInitials;

        public CalendarDay(DateTime currentDayEntry)
        {
            CurrentDay = currentDayEntry;
            EngineersInitials = new ObservableCollection<string>();
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");
            EngineersInitials.Add("AH");

            MissingEngineersInitials = new ObservableCollection<string>();
            MissingEngineersInitials.Add("RH");
            MissingEngineersInitials.Add("RH");
            MissingEngineersInitials.Add("RH");
            MissingEngineersInitials.Add("RH");
            MissingEngineersInitials.Add("RH");
            MissingEngineersInitials.Add("RH");
            MissingEngineersInitials.Add("RH");

        }

        public DateTime CurrentDay
        {
            get
            {
                return m_currentDay;
            }

            set
            {
                m_currentDay = value;
            }
        }

        public ObservableCollection<string> EngineersInitials
        {
            get
            {
                return m_engineersInitials;
            }

            set
            {
                m_engineersInitials = value;
                //onPropertyChanged("EngineersInitials");
            }
        }

        public ObservableCollection<string> MissingEngineersInitials
        {
            get
            {
                return m_missingEngineersInitials;
            }

            set
            {
                m_missingEngineersInitials = value;
                //onPropertyChanged("MissingEngineersInitials");
            }
        }

        public string Title
        {
            get
            {
                return CurrentDay.ToShortDateString() + " - " + CurrentDay.DayOfWeek.ToString();
            }
        }
    }
}
