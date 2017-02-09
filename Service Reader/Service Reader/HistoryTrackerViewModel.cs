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
        private ObservableCollection<ServiceSheetViewModel> m_allServiceSheets;
        private ICommand loadCsvCommand;
        private ICommand m_updateMonthCommand;

        private DateTime m_monthFirstDay = DateTime.Now;
        private DateTime m_rowOneStartDate;
        private ObservableCollection<CalendarRow> m_calendarRows;

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

        public ObservableCollection<ServiceSheetViewModel> AllServiceSheets
        {
            get
            {
                return m_allServiceSheets;
            }

            set
            {
                m_allServiceSheets = value;
                onPropertyChanged("AllServiceSheets");
            }
        }

        public ICommand LoadCsvCommand
        {
            get
            {
                if (loadCsvCommand == null)
                {
                    loadCsvCommand = new RelayCommand(param => loadHistoricalDataFromCsv());
                }
                return loadCsvCommand;
            }

            set
            {
                loadCsvCommand = value;
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

        private void loadHistoricalDataFromCsv()
        {
            //RT - This calls the import csv and loads the csv file previously created from the Canvas Submissions screen.
            CsvServiceImport importer = new CsvServiceImport();
            bool result = importer.importCsvData();
            AllServiceSheets = importer.AllServiceSubmissions;
        }
    }
}
