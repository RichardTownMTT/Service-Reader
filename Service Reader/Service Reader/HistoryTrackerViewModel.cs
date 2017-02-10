using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class HistoryTrackerViewModel : ObservableObject
    {
        private List<ServiceSheetViewModel> m_allServiceSheets;
        private ICommand m_downloadDatabaseCommand;
        private ICommand m_updateMonthCommand;

        private DateTime m_monthFirstDay = DateTime.Now;
        private DateTime m_rowOneStartDate;
        private ObservableCollection<CalendarRow> m_calendarRows;


        private List<DbEmployee> m_engineers;
        public HistoryTrackerViewModel()
        {
            updateCalendar();
        }

        private void createRows()
        {
            CalendarRows = new ObservableCollection<CalendarRow>();
            int startOfWeekMonth = MonthFirstDay.Month;
            DateTime currentRowStartOfWeek = RowOneStartDate;
            int rowNumber = 1;
            CalendarRow rowCreated;
            while (startOfWeekMonth <= MonthFirstDay.Month)
            {
                rowCreated = new CalendarRow(currentRowStartOfWeek, MonthFirstDay, rowNumber);
                CalendarRows.Add(rowCreated);

                rowNumber++;
                currentRowStartOfWeek = currentRowStartOfWeek.AddDays(7);
                startOfWeekMonth = currentRowStartOfWeek.Month;
            }
        }

        private void calculateRowOneStartDate()
        {
            //MonthFirstDay = DateTime.Now;
            int dayNumber = MonthFirstDay.Day;
            MonthFirstDay = MonthFirstDay.AddDays(-dayNumber + 1);
            int startOfMonthDay = (int)MonthFirstDay.DayOfWeek;
            if (startOfMonthDay == 0)
            {
                RowOneStartDate = MonthFirstDay.AddDays(-6);
            }
            else
            {
                RowOneStartDate = MonthFirstDay.AddDays(-startOfMonthDay + 1);
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

        public DateTime RowOneStartDate
        {
            get
            {
                return m_rowOneStartDate;
            }

            set
            {
                m_rowOneStartDate = value;
            }
        }

        public ObservableCollection<CalendarRow> CalendarRows
        {
            get
            {
                return m_calendarRows;
            }

            set
            {
                m_calendarRows = value;
                onPropertyChanged("CalendarRows");
            }
        }

        public List<ServiceSheetViewModel> AllServiceSheets
        {
            get
            {
                return m_allServiceSheets;
            }

            set
            {
                m_allServiceSheets = value;
            }
        }

       

        public ICommand UpdateMonthCommand
        {
            get
            {
                if (m_updateMonthCommand == null)
                {
                    m_updateMonthCommand = new RelayCommand(param => updateCalendar());
                }
                return m_updateMonthCommand;
            }

            set
            {
                m_updateMonthCommand = value;
            }
        }

        public ICommand DownloadDatabaseCommand
        {
            get
            {
                if (m_downloadDatabaseCommand == null)
                {
                    m_downloadDatabaseCommand = new RelayCommand(x => downloadDatabaseEntries());
                }
                return m_downloadDatabaseCommand;
            }

            set
            {
                m_downloadDatabaseCommand = value;
            }
        }

        private void downloadDatabaseEntries()
        {
            List<ServiceSheetViewModel> downloadedServiceSheets = DbServiceSheet.downloadAllServiceSheets();

            if (downloadedServiceSheets == null)
            {
                return;
            }

            //Create the list of possible days for each engineer
            Dictionary<DateTime, List<DbEmployee>> possibleDays = createPossibleEngineerDaysCalendar();
            if (possibleDays == null)
            {
                return;
            }

            //Copy the possible days to the missing.
            Dictionary<DateTime, List<DbEmployee>> missingDays = new Dictionary<DateTime, List<DbEmployee>>(possibleDays);

            //Create actual days list and update missing list
            updateActualandMissingCalendars(possibleDays, missingDays, downloadedServiceSheets);


        }

        private void updateActualandMissingCalendars(Dictionary<DateTime, List<DbEmployee>> possibleDays, Dictionary<DateTime, List<DbEmployee>> missingDays, List<ServiceSheetViewModel> downloadedServiceSheets)
        {
            var allDayVMs = downloadedServiceSheets.Select(days => days.AllServiceDays);
            var allServiceDays = allDayVMs.SelectMany(serviceDays => serviceDays.AllServiceDayVMs).OrderBy(days => days.DtReport);

            Dictionary<DateTime, List<DbEmployee>> actualDays = new Dictionary<DateTime, List<DbEmployee>>();

            foreach (var day in allServiceDays)
            {
                //Add to actual calendar and remove from missing calendar
                addItemToActualDaysCalendar(day, actualDays);
                removeFromMissingCalendar(day, missingDays);
            }
        }

        private void removeFromMissingCalendar(ServiceDayViewModel day, Dictionary<DateTime, List<DbEmployee>> missingDays)
        {
            DateTime currentDay = day.DtReport;
            DbEmployee employeeFound = getEmployeeForUsername(day.ParentServiceSheetVM.Username);

            List<DbEmployee> employeeListForDay;
            if (missingDays.TryGetValue(currentDay, out employeeListForDay))
            {
                employeeListForDay.Remove(employeeFound);
                missingDays[currentDay] = employeeListForDay;
            }
            
        }

        private void addItemToActualDaysCalendar(ServiceDayViewModel day, Dictionary<DateTime, List<DbEmployee>> actualDays)
        {
            DateTime currentDate = day.DtReport;

            //Find engineer for day
            string usernameDay = day.ParentServiceSheetVM.Username;
            DbEmployee employeeMatch = getEmployeeForUsername(usernameDay);

            List<DbEmployee> employeeListDay;
            if (actualDays.TryGetValue(currentDate, out employeeListDay))
            {
                employeeListDay.Add(employeeMatch);
                actualDays[currentDate] = employeeListDay;
            }
            else
            {
                employeeListDay = new List<DbEmployee>();
                employeeListDay.Add(employeeMatch);
                actualDays.Add(currentDate, employeeListDay);
            }
        }

        private DbEmployee getEmployeeForUsername(string usernameDay)
        {
            var retval = (from engineers in m_engineers
                         where engineers.Username == usernameDay
                         select engineers).Distinct().First();
            if (retval == null)
            {
                throw new Exception("User not found for: " + usernameDay);
            }

            return retval;
        }

        private Dictionary<DateTime, List<DbEmployee>> createPossibleEngineerDaysCalendar()
        {
            Dictionary<DateTime, List<DbEmployee>> possibleDays = new Dictionary<DateTime, List<DbEmployee>>();
            //Download all the engineers names

            m_engineers = DbServiceSheet.getAllUsers();
            if (m_engineers == null)
            {
                return null;
            }

            DateTime calendarStart = new DateTime(2017, 1, 1);
            DateTime calendarEnd = new DateTime(2017, 1, 31);

            for(DateTime currentDate = calendarStart; currentDate.Date <= calendarEnd.Date; currentDate = currentDate.AddDays(1))
            {
                possibleDays.Add(currentDate, new List<DbEmployee>(m_engineers));
            }

            return possibleDays;
        }

        private void updateCalendar()
        {
            Stopwatch renderTimer = new Stopwatch();
            renderTimer.Start();

            calculateRowOneStartDate();
            createRows();

            renderTimer.Stop();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Render Time: = " + renderTimer.ElapsedMilliseconds);
        }

    }
}
