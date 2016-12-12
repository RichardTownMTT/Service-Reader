using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;

namespace Service_Reader
{
    //This is used to export the service sheet data to the costing sheet in excel
    public class CreateCostSheet
    {
        private Excel._Application excelApplication;
        private Excel._Workbook excelWorkbook;
        private Excel.Worksheet excelWorksheet;

        //RT 20/8/16 - Changing to multiple rows
        //public bool exportDataToCostSheet(ServiceSubmissionModel submission)
        public bool exportDataToCostSheet(ObservableCollection<ServiceSheetViewModel> submissions)
        {
            string filename = openFilename();
            if (filename.Equals(""))
            {
                MessageBox.Show("No file selected.");
                return false;
            }


            //RT 20/8/16 - Changing to deal with multiple sheets
            //bool success = createJobCostingSheet(submission, filename);
            bool success = createCostingSheetForSubmission(submissions, filename);


            return success;
        }

        private bool createCostingSheetForSubmission(ObservableCollection<ServiceSheetViewModel> submissions, string filename)
        {
            int lineNumber = -1;
            //Loop through the submissions and add to the sheet.
            //First one will set the job details
            bool firstSubmission = true;

            foreach (ServiceSheetViewModel submission in submissions)
            {
                lineNumber = createJobCostingSheet(submission, filename, firstSubmission, lineNumber);
                firstSubmission = false;
            }

            if (lineNumber > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return success;
        }

        //private bool createJobCostingSheet(ServiceSubmissionModel submission, string filename)
        private int createJobCostingSheet(ServiceSheetViewModel submission, string filename, bool firstSubmission, int lineNumber)
        {
            Excel.Range range;
            //First Open excel

            if (firstSubmission)
            {
                excelApplication = new Excel.Application();
                excelApplication.Visible = true;
                excelWorkbook = excelApplication.Workbooks.Open(filename);

                excelWorksheet = excelWorkbook.ActiveSheet;

                //Add the job title
                string customer = submission.Customer;
                string machineMake = submission.MachineMakeModel;
                string serialNumber = submission.MachineSerial;
                string jobDescription = submission.JobDescription;

                range = excelWorksheet.Cells[13, 1];
                range.Value2 = string.Concat(customer, " - ", machineMake, " - S/N: ", serialNumber, " - ", jobDescription);

                range = excelWorksheet.Cells[11, 9];
                range.Value2 = submission.MttJobNumber;
            }
            //Load all the days and loop through them. Output to the sheet
            AllServiceDayViewModels serviceDays = submission.AllServiceDays;

            //Need the engineers initials for each row
            string engFirstName = submission.UserFirstName;
            string engSurname = submission.UserSurname;
            string initials = string.Concat(engFirstName[0], engSurname[0]);

            int sheetNo = submission.SubmissionNumber;

            int currentSpreadsheetRow;

            if (lineNumber == -1)
            {
                currentSpreadsheetRow = 17;
            }
            else
            {
                currentSpreadsheetRow = lineNumber++;
            }


            foreach (ServiceDayViewModel currentDay in serviceDays.AllServiceDayVMs)
            {
                range = excelWorksheet.Cells[currentSpreadsheetRow, 1];
                range.Value2 = currentDay.DtReport;

                string day = currentDay.DtReport.DayOfWeek.ToString();
                range = excelWorksheet.Cells[currentSpreadsheetRow, 2];
                range.Value2 = day;

                double standardHours = calculateStandardHours(currentDay);
                range = excelWorksheet.Cells[currentSpreadsheetRow, 3];
                range.Value2 = standardHours;

                double overtimeHours = calculateOvertimeHours(currentDay);
                range = excelWorksheet.Cells[currentSpreadsheetRow, 4];
                range.Value2 = overtimeHours;

                range = excelWorksheet.Cells[currentSpreadsheetRow, 5];
                range.Value2 = currentDay.TotalTravelTime;

                bool dailyAllowance = currentDay.DailyAllowance;
                int dailyAllowanceValue = convertBoolToIntForAllowances(dailyAllowance);
                range = excelWorksheet.Cells[currentSpreadsheetRow, 6];
                range.Value2 = dailyAllowanceValue;

                bool overNight = currentDay.OvernightAllowance;
                int overnightAllowanceValue = convertBoolToIntForAllowances(overNight);
                range = excelWorksheet.Cells[currentSpreadsheetRow, 7];
                range.Value2 = overnightAllowanceValue;

                range = excelWorksheet.Cells[currentSpreadsheetRow, 8];
                range.Value2 = currentDay.Mileage;

                range = excelWorksheet.Cells[currentSpreadsheetRow, 9];
                range.Value2 = initials;

                range = excelWorksheet.Cells[currentSpreadsheetRow, 10];
                range.Value2 = sheetNo;

                currentSpreadsheetRow++;
            }

            MessageBox.Show("Need to handle bank holidays");

            return currentSpreadsheetRow;
        }

        private int convertBoolToIntForAllowances(bool booleanValue)
        {
            int retval;
            if (booleanValue == true)
            {
                retval = 1;
            }
            else
            {
                retval = 0;
            }

            return retval;
        }

        private double calculateOvertimeHours(ServiceDayViewModel currentDay)
        {
            //Monday - Thursday are the first 8 hours onsite.
            //Friday is the first 6
            //Weekends and Bank holidays are all

            double retval = 0;
            double hoursOnsite = currentDay.TotalOnsiteTime;

            double standardHoursLimitNormal = 8;
            double standardHoursFriday = 6;

            DayOfWeek day = currentDay.DtReport.DayOfWeek;
            if ((DayOfWeek.Monday <= day) && (day <= DayOfWeek.Thursday))
            {
                if (hoursOnsite > standardHoursLimitNormal)
                {
                    retval = hoursOnsite - standardHoursLimitNormal;
                }
                else
                {
                    retval = 0;
                }
            }

            if (day == DayOfWeek.Friday)
            {
                if (hoursOnsite > standardHoursFriday)
                {
                    retval = hoursOnsite - standardHoursFriday;
                }
                else
                {
                    retval = 0;
                }
            }

            if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
            {
                retval = hoursOnsite;
            }

            return retval;
        }

        private double calculateStandardHours(ServiceDayViewModel currentDay)
        {
            //Monday - Thursday are the first 8 hours onsite.
            //Friday is the first 6
            //Weekends and Bank holidays are all

            double retval = 0;
            double hoursOnsite = currentDay.TotalOnsiteTime;

            double standardHoursLimitNormal = 8;
            double standardHoursFriday = 6;

            DayOfWeek day = currentDay.DtReport.DayOfWeek;
            if ((DayOfWeek.Monday <= day) && (day <= DayOfWeek.Thursday))
            {
                if (hoursOnsite > standardHoursLimitNormal)
                {
                    retval = standardHoursLimitNormal;
                }
                else
                {
                    retval = hoursOnsite;
                }
            }

            if (day == DayOfWeek.Friday)
            {
                if (hoursOnsite > standardHoursFriday)
                {
                    retval = standardHoursFriday;
                }
                else
                {
                    retval = hoursOnsite;
                }
            }

            if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
            {
                retval = 0;
            }

            return retval;
        }

        private string openFilename()
        {
            string retval;
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
            {
                retval = openDialog.FileName;
            }
            else
            {
                retval = "";
            }

            return retval;
        }
    }
}
