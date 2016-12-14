using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows;

namespace Service_Reader
{
    public class CsvServiceImport
    {
        private TextReader csvTextReader;
        private CsvReader csvReaderInput;
        private ServiceSheetViewModel currentServiceSubmission;
        ObservableCollection<ServiceSheetViewModel> importedSubmissions;

        //This class imports the historical data from a csv file and displays it in the application

        public Boolean importCsvData()
        {
            //First get the user to select the file with the data
            string csvFilename = openFilename();
            //RT 18/8/16 - csv filename can return empty string if cancelled.
            if (csvFilename.Equals(""))
            {
                MessageBox.Show("No csv file selected");
                return false;
            }
            csvTextReader = File.OpenText(csvFilename);
            csvReaderInput = new CsvReader(csvTextReader);
            csvReaderInput.Configuration.Delimiter = ",";

            //For each record in the csv file, we need to create a submission with all the days
            //Loop through all the rows in the csv import

            //If the row submission no matches the current submission number, then we are loading days.
            //If not, then it it a new submssion

            int currentReadSubmissionNo = -1;
            importedSubmissions = new ObservableCollection<ServiceSheetViewModel>();

            while (csvReaderInput.Read())
            {
                var row = csvReaderInput.CurrentRecord;
                int submissionNo = Convert.ToInt32(row[2]);

                //If first time through, then set the current submission no
                //Load the first row
                if (currentReadSubmissionNo == -1)
                {
                    currentReadSubmissionNo = submissionNo;
                    currentServiceSubmission = new ServiceSheetViewModel();
                    loadNewSubmission(row);
                    continue;
                }

                if (currentReadSubmissionNo == submissionNo)
                {
                    loadDayForSubmission(row, currentServiceSubmission);
                }
                else
                {
                    //RT - 20/8/16 - Need to recalculate the times as we have added the last day
                    //RT - 24/11/16 - No we don't, this is done at the end!
                    //currentServiceSubmission.updateTimes();
                    importedSubmissions.Add(currentServiceSubmission);
                    Console.WriteLine("Submission: " + currentReadSubmissionNo + " created");
                    currentReadSubmissionNo = submissionNo;
                    currentServiceSubmission = new ServiceSheetViewModel();
                    loadNewSubmission(row);
                    
                }
                
            }

            //RT - 20/8/16 - Need to recalculate the times as we have added the last day
            currentServiceSubmission.updateAllTimes();

            //RT 18/8/16 - Need to save the last submssion
            importedSubmissions.Add(currentServiceSubmission);

            return true;
        }

        private void loadNewSubmission(string[] row)
        {
            //RT 6/12/16 - If the first date is blank, then we are loading a csv created with this application
            bool serviceReaderCsv = false;
            string startDateStr = row[0];
            if (String.IsNullOrEmpty(startDateStr))
            {
                serviceReaderCsv = true;
            }

            //First two dates aren't read
            currentServiceSubmission.SubmissionNumber = Convert.ToInt32(row[2]);
            //App Name isn't used
            currentServiceSubmission.Username = row[4];
            currentServiceSubmission.UserSurname = row[5];
            currentServiceSubmission.UserFirstName = row[6];
            currentServiceSubmission.CanvasResponseId = row[7];
            string dateFormatMinutes = "d/M/yyyy HH:mm";
            string dateFormatSecondsUSA = "M/d/yyyy HH:mm:ss";
            string responseDate = row[8];
            //RT 15/8/16 - The time is either includes minutes or does not. 
            //RT 6/12/16 - Changng this to use one of the two methods based on what created the row
            if (serviceReaderCsv)
            {
                currentServiceSubmission.DtResponse = DateTime.ParseExact(responseDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            } 
            else
            {
                currentServiceSubmission.DtResponse = DateTime.ParseExact(responseDate, dateFormatSecondsUSA, CultureInfo.InvariantCulture);
            }
            //try
            //{
            //    currentServiceSubmission.DtResponse = DateTime.ParseExact(responseDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            //}
            //catch
            //{
            //    currentServiceSubmission.DtResponse = DateTime.ParseExact(responseDate, dateFormatSeconds, CultureInfo.InvariantCulture);
            //}
            string deviceDate = row[9];

            if (serviceReaderCsv)
            {
                currentServiceSubmission.DtDevice = DateTime.ParseExact(deviceDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                currentServiceSubmission.DtDevice = DateTime.ParseExact(deviceDate, dateFormatSecondsUSA, CultureInfo.InvariantCulture);
            }
            //try
            //{
            //    currentServiceSubmission.DtDevice = DateTime.ParseExact(deviceDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            //}
            //catch
            //{
            //    currentServiceSubmission.DtDevice = DateTime.ParseExact(deviceDate, dateFormatSeconds, CultureInfo.InvariantCulture);
            //}
            //Submission Form name not used
            currentServiceSubmission.SubmissionFormVersion = Convert.ToInt32(row[11]);
            currentServiceSubmission.Customer = row[12];
            currentServiceSubmission.AddressLine1 = row[13];
            currentServiceSubmission.AddressLine2 = row[14];
            currentServiceSubmission.TownCity = row[15];
            currentServiceSubmission.Postcode = row[16];
            currentServiceSubmission.CustomerContact = row[17];
            currentServiceSubmission.CustomerPhoneNo = row[18];
            currentServiceSubmission.MachineMakeModel = row[19];
            currentServiceSubmission.MachineSerial = row[20];
            currentServiceSubmission.CncControl = row[21];
            string jobStartDate = row[22];
            //RT 6/12/16 - From canvas csv, this is a date, datetime from service reader
            if (serviceReaderCsv)
            {
                currentServiceSubmission.DtJobStart = DateTime.ParseExact(jobStartDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                currentServiceSubmission.DtJobStart = DateTime.ParseExact(jobStartDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            //currentServiceSubmission.DtJobStart = DateTime.ParseExact(jobStartDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            currentServiceSubmission.CustomerOrderNo = row[23];
            currentServiceSubmission.MttJobNumber = row[24];
            currentServiceSubmission.JobDescription = row[25];

            //Need to load the days
            //First set the timesheets up on the current service submission
            //currentServiceSubmission.AllServiceDayVMs = new ObservableCollection<ServiceDayViewModel>();
            loadDayForSubmission(row, currentServiceSubmission);

            currentServiceSubmission.JobTotalTimeOnsite = Convert.ToDouble(row[41]);
            currentServiceSubmission.JobTotalTravelTime = Convert.ToDouble(row[42]);
            currentServiceSubmission.JobTotalMileage = Convert.ToInt32(row[43]);
            currentServiceSubmission.TotalDailyAllowances = Convert.ToInt32(row[44]);
            currentServiceSubmission.TotalOvernightAllowances = Convert.ToInt32(row[45]);
            currentServiceSubmission.TotalBarrierPayments = Convert.ToInt32(row[46]);
            currentServiceSubmission.JobStatus = row[47];
            currentServiceSubmission.FinalJobReport = row[48];
            currentServiceSubmission.AdditionalFaults = row[50];
            currentServiceSubmission.QuoteRequired = Convert.ToBoolean(row[51]);
            currentServiceSubmission.FollowUpPartsRequired = row[52];
            currentServiceSubmission.Image1Url = row[54];
            currentServiceSubmission.Image2Url = row[55];
            currentServiceSubmission.Image3Url = row[56];
            currentServiceSubmission.Image4Url = row[57];
            currentServiceSubmission.Image5Url = row[58];
            currentServiceSubmission.CustomerSignatureUrl = row[60];
            currentServiceSubmission.CustomerName = row[61];
            string signedDate = row[62];
            //RT 6/12/16 - From canvas csv, this is a date, datetime from service reader
            if (serviceReaderCsv)
            {
                currentServiceSubmission.DtSigned = DateTime.ParseExact(signedDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                currentServiceSubmission.DtSigned = DateTime.ParseExact(signedDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            //currentServiceSubmission.DtSigned = DateTime.ParseExact(signedDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            currentServiceSubmission.MttEngSignatureUrl = row[63];
        }

        //RT 28/11/16 - Rewriting this to use the MVVM pattern
        //private void loadDayForSubmission(string[] row, ServiceSheetViewModel currentSubmission)
        //{
        //    string dateFormatMinutes = "d/M/yyyy HH:mm";
        //    string dateFormatSeconds = "d/M/yyyy H:mm:ss";

        //    //Need to set the submission on the service day
        //    ServiceDayViewModel currentDay = new ServiceDayViewModel(currentSubmission);
        //    //The times may be with / without the date, depending on when they were imported.
        //    //Need to load the service date first, in case we need it for the times
        //    string serviceDate = row[40];
        //    currentDay.DtReport = DateTime.ParseExact(serviceDate, "d/M/yyyy", CultureInfo.InvariantCulture);

        //    string travelStartTime = row[26];
        //    try
        //    {
        //        currentDay.TravelStartTime = DateTime.ParseExact(travelStartTime, dateFormatMinutes, CultureInfo.InvariantCulture);
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            currentDay.TravelStartTime = DateTime.ParseExact(travelStartTime, dateFormatSeconds, CultureInfo.InvariantCulture);
        //        }
        //        catch
        //        {
        //            string travelStartIncDate = serviceDate + " " + travelStartTime;
        //            currentDay.TravelStartTime = DateTime.ParseExact(travelStartIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
        //        }
        //    }

        //    string arrivalTimeOnsite = row[27];
        //    try
        //    {
        //        currentDay.ArrivalOnsiteTime = DateTime.ParseExact(arrivalTimeOnsite, dateFormatMinutes, CultureInfo.InvariantCulture);
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            currentDay.ArrivalOnsiteTime = DateTime.ParseExact(arrivalTimeOnsite, dateFormatSeconds, CultureInfo.InvariantCulture);
        //        }
        //        catch
        //        {
        //            string arrivalOnsiteIncDate = serviceDate + " " + arrivalTimeOnsite;
        //            currentDay.ArrivalOnsiteTime = DateTime.ParseExact(arrivalOnsiteIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
        //        }
        //    }

        //    string departureTime = row[28];
        //    try
        //    {
        //        currentDay.DepartSiteTime = DateTime.ParseExact(departureTime, dateFormatMinutes, CultureInfo.InvariantCulture);
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            currentDay.DepartSiteTime = DateTime.ParseExact(departureTime, dateFormatSeconds, CultureInfo.InvariantCulture);
        //        }
        //        catch
        //        {
        //            string departureIncDate = serviceDate + " " + departureTime;
        //            currentDay.DepartSiteTime = DateTime.ParseExact(departureIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
        //        }
        //    }

        //    string travelEndTime = row[29];
        //    try
        //    {
        //        currentDay.TravelEndTime = DateTime.ParseExact(travelEndTime, dateFormatMinutes, CultureInfo.InvariantCulture);
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            currentDay.TravelEndTime = DateTime.ParseExact(travelEndTime, dateFormatSeconds, CultureInfo.InvariantCulture);
        //        }
        //        catch
        //        {
        //            string travelEndIncDate = serviceDate + " " + travelEndTime;
        //            currentDay.TravelEndTime = DateTime.ParseExact(travelEndIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
        //        }
        //    }

        //    currentDay.Mileage = Convert.ToInt32(row[30]);
        //    //try
        //    //{
        //    //    currentDay.ServiceDay.DailyAllowance = Convert.ToBoolean(row[31]);
        //    //}
        //    //catch
        //    //{
        //    //If this fails, then it nust be an integer
        //    //    int dailyAllowance = Convert.ToInt32(row[31]);
        //    //    if (dailyAllowance == 1)
        //    //    {
        //    //        currentDay.ServiceDay.DailyAllowance = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        currentDay.ServiceDay.DailyAllowance = false;
        //    //    }
        //    ////}

        //    currentDay.DailyAllowance = Convert.ToBoolean(row[31]);

        //    //try
        //    //{
        //    //    currentDay.ServiceDay.OvernightAllowance = Convert.ToBoolean(row[32]);
        //    //}
        //    //catch
        //    //{
        //    //    //If this fails, then it nust be an integer
        //    //    int overnightAllowance = Convert.ToInt32(row[32]);
        //    //    if (overnightAllowance == 1)
        //    //    {
        //    //        currentDay.ServiceDay.OvernightAllowance = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        currentDay.ServiceDay.OvernightAllowance = false;
        //    //    }
        //    //}
        //    currentDay.OvernightAllowance = Convert.ToBoolean(row[32]);
        //    //try
        //    //{
        //    //    currentDay.ServiceDay.BarrierPayment = Convert.ToBoolean(row[33]);
        //    //}
        //    //catch
        //    //{
        //    //    //If this fails, then it nust be an integer
        //    //    int barrierPayment = Convert.ToInt32(row[33]);
        //    //    if (barrierPayment == 1)
        //    //    {
        //    //        currentDay.ServiceDay.BarrierPayment = true;
        //    //    }
        //    //    else
        //    //    {
        //    //        currentDay.ServiceDay.BarrierPayment = false;
        //    //    }
        //    //}
        //    currentDay.BarrierPayment = Convert.ToBoolean(row[33]);

        //    currentDay.TravelTimeToSite = Convert.ToDouble(row[34]);
        //    currentDay.TravelTimeFromSite = Convert.ToDouble(row[35]);
        //    currentDay.TotalTravelTime = Convert.ToDouble(row[36]);
        //    currentDay.TotalTimeOnsite = Convert.ToDouble(row[37]);
        //    currentDay.DailyReport = row[38];
        //    currentDay.PartsSupplied = row[39];

        //    //RT 16/8/16 - Saving the timesheet
        //    //currentSubmission.AllServiceDayVMs.
        //}
        private void loadDayForSubmission(string[] row, ServiceSheetViewModel currentSubmission)
        {
            //RT 6/12/16 - If the first date is blank, then we are loading a csv created with this application
            bool serviceReaderCsv = false;
            string startDateStr = row[0];
            if (String.IsNullOrEmpty(startDateStr))
            {
                serviceReaderCsv = true;
            }

            string dateFormatMinutes = "d/M/yyyy HH:mm";
            //RT 6/12/16 - varable no longer needed.  Added by accident
            //string dateFormatSeconds = "d/M/yyyy H:mm:ss";

            //Need to set the submission on the service day
            //ServiceDayViewModel currentDay = new ServiceDayViewModel(currentSubmission);
            //The times may be with / without the date, depending on when they were imported.
            //Need to load the service date first, in case we need it for the times
            string serviceDate = row[40];
            //RT 6/12/16 - From canvas csv, this is a date, datetime from service reader
            DateTime dtReport;
            if (serviceReaderCsv)
            {
                dtReport = DateTime.ParseExact(serviceDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                dtReport = DateTime.ParseExact(serviceDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            }

            string travelStartTime = row[26];

            //RT 6/12/16 - Canvas created csv's are time only.  Service Reader ones include 
            DateTime dtTravelStart;
            if (serviceReaderCsv)
            {
                dtTravelStart = DateTime.ParseExact(travelStartTime, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                string travelStartIncDate = serviceDate + " " + travelStartTime;
                dtTravelStart = DateTime.ParseExact(travelStartIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            //try
            //{
            //    dtTravelStart = DateTime.ParseExact(travelStartTime, dateFormatMinutes, CultureInfo.InvariantCulture);
            //}
            //catch
            //{
            //    try
            //    {
            //        dtTravelStart = DateTime.ParseExact(travelStartTime, dateFormatSeconds, CultureInfo.InvariantCulture);
            //    }
            //    catch
            //    {
            //        string travelStartIncDate = serviceDate + " " + travelStartTime;
            //        dtTravelStart = DateTime.ParseExact(travelStartIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            //    }
            //}

            string arrivalTimeOnsite = row[27];
            DateTime dtArrivalOnsite;
            if (serviceReaderCsv)
            {
                dtArrivalOnsite = DateTime.ParseExact(arrivalTimeOnsite, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                string arrivalOnsiteIncDate = serviceDate + " " + arrivalTimeOnsite;
                dtArrivalOnsite = DateTime.ParseExact(arrivalOnsiteIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }

            //try
            //{
            //    dtArrivalOnsite = DateTime.ParseExact(arrivalTimeOnsite, dateFormatMinutes, CultureInfo.InvariantCulture);
            //}
            //catch
            //{
            //    try
            //    {
            //        dtArrivalOnsite = DateTime.ParseExact(arrivalTimeOnsite, dateFormatSeconds, CultureInfo.InvariantCulture);
            //    }
            //    catch
            //    {
            //        string arrivalOnsiteIncDate = serviceDate + " " + arrivalTimeOnsite;
            //        dtArrivalOnsite = DateTime.ParseExact(arrivalOnsiteIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            //    }
            //}

            string departureTime = row[28];
            DateTime dtDeparture;
            if (serviceReaderCsv)
            {
                dtDeparture = DateTime.ParseExact(departureTime, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                string departureIncDate = serviceDate + " " + departureTime;
                dtDeparture = DateTime.ParseExact(departureIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            //try
            //{
            //    dtDeparture = DateTime.ParseExact(departureTime, dateFormatMinutes, CultureInfo.InvariantCulture);
            //}
            //catch
            //{
            //    try
            //    {
            //        dtDeparture = DateTime.ParseExact(departureTime, dateFormatSeconds, CultureInfo.InvariantCulture);
            //    }
            //    catch
            //    {
            //        string departureIncDate = serviceDate + " " + departureTime;
            //        dtDeparture = DateTime.ParseExact(departureIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            //    }
            //}

            string travelEndTime = row[29];
            DateTime dtTravelEnd;
            if (serviceReaderCsv)
            {
                dtTravelEnd = DateTime.ParseExact(travelEndTime, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            else
            {
                string travelEndIncDate = serviceDate + " " + travelEndTime;
                dtTravelEnd = DateTime.ParseExact(travelEndIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            }
            //try
            //{
            //    dtTravelEnd = DateTime.ParseExact(travelEndTime, dateFormatMinutes, CultureInfo.InvariantCulture);
            //}
            //catch
            //{
            //    try
            //    {
            //        dtTravelEnd = DateTime.ParseExact(travelEndTime, dateFormatSeconds, CultureInfo.InvariantCulture);
            //    }
            //    catch
            //    {
            //        string travelEndIncDate = serviceDate + " " + travelEndTime;
            //        dtTravelEnd = DateTime.ParseExact(travelEndIncDate, dateFormatMinutes, CultureInfo.InvariantCulture);
            //    }
            //}

            int mileage = Convert.ToInt32(row[30]);
            //RT 6/12/16  - The allowances are 1/0.  Need to convert to bool
            string daStr = row[31];
            bool dailyAllowance;
            if (daStr.Equals("1"))
            {
                dailyAllowance = true;
            }
            else
            {
                dailyAllowance = false;
            }
            //bool dailyAllowance = Convert.ToBoolean(row[31]);
            bool overnightAllowance;
            string onStr = row[32];
            if (onStr.Equals("1"))
            {
                overnightAllowance = true;
            }
            else
            {
                overnightAllowance = false;
            }
            //bool overnightAllowance = Convert.ToBoolean(row[32]);
            bool barrierPayment;
            string bpStr = row[33];
            if (bpStr.Equals("1"))
            {
                barrierPayment = true;
            }
            else
            {
                barrierPayment = false;
            }
            //bool barrierPayment = Convert.ToBoolean(row[33]);
            double travelTimeToSite = Convert.ToDouble(row[34]);
            double travelTimeFromSite = Convert.ToDouble(row[35]);
            double totalTravelTime = Convert.ToDouble(row[36]);
            double totalTimeOnsite = Convert.ToDouble(row[37]);
            string dailyReport = row[38];
            string partsSupplied = row[39];
            //Now create the serviceDayVM
            ServiceDayViewModel retval = new ServiceDayViewModel(dtTravelStart, dtArrivalOnsite, dtDeparture, dtTravelEnd, mileage, dailyAllowance, overnightAllowance, barrierPayment,
                travelTimeToSite, travelTimeFromSite, totalTravelTime, totalTimeOnsite, dailyReport, partsSupplied, dtReport, currentSubmission);
            currentSubmission.AddServiceDayViewModel(retval);
            //This doesn't need to return anything, as the day has been set on the submission.
        }

        private string openFilename()
        {
            string retval;
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "csv files (*.csv)|*.csv";
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

        public ObservableCollection<ServiceSheetViewModel> AllServiceSubmissions
        {
            get
            {
                return importedSubmissions;
            }
        }
    }
}
