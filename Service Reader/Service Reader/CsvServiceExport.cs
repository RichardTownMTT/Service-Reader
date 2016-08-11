using System;
using System.Collections.Generic;
using CsvHelper;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using System.Collections.ObjectModel;

namespace Service_Reader
{
    public class CsvServiceExport
    {
        //This class exports the finalised service report data to a csv file, for storage
        private TextWriter csvTextWriter;
        private CsvWriter csvWriterOutput;

        //Pass in multiple sheets
        public Boolean exportDataToCsv(List<ServiceSubmissionModel> submissionsToExport)
        {
            string outputFilename = createFilename();

            if (outputFilename.Equals(""))
            {
                //No filename selected - exit
                MessageBox.Show("No file selected. Exiting.");
                return false;
            }
            
            csvTextWriter = File.CreateText(outputFilename);
            csvWriterOutput = new CsvWriter(csvTextWriter);

            //Loop through the sheets 
            foreach (ServiceSubmissionModel submission in submissionsToExport)
            {
                //For each submission, create the csv export
                createExportLinesForSubmission(submission);
            }

            csvTextWriter.Close();
            //If we get to here, then all created successfully
            return true;
        }

        private void createExportLinesForSubmission(ServiceSubmissionModel submission)
        {
            ObservableCollection<ServiceDayModel> serviceDays = submission.ServiceTimesheets;
            int noOfDays = serviceDays.Count;

            for (int counter = 0; counter < noOfDays ; counter++)
            {
                //extract the sections from the service sheet that go into the CSV export
                //Start date and end date are not taken - They only appear in the csv export
                csvWriterOutput.WriteField("");
                csvWriterOutput.WriteField("");
                int submissionNo = submission.SubmissionNo;
                csvWriterOutput.WriteField(submissionNo);
                //Export app name - set manually
                csvWriterOutput.WriteField("Service Sheet");
                string username = submission.Username;
                csvWriterOutput.WriteField(username);
                string userLastName = submission.UserSurname;
                csvWriterOutput.WriteField(userLastName);
                string userFirstName = submission.UserFirstName;
                csvWriterOutput.WriteField(userFirstName);
                //Response id not set
                csvWriterOutput.WriteField("");
                //Response date/time is not set
                csvWriterOutput.WriteField("");
                //Device Date/Time - not set
                csvWriterOutput.WriteField("");
                //Submission form name set manually
                csvWriterOutput.WriteField("Service Sheet");
                int formVersion = submission.SubmissionVersion;
                csvWriterOutput.WriteField(formVersion);
                string customer = submission.Customer;
                csvWriterOutput.WriteField(customer);
                string address1 = submission.Address1;
                csvWriterOutput.WriteField(address1);
                string address2 = submission.Address2;
                csvWriterOutput.WriteField(address2);
                string townCity = submission.TownCity;
                csvWriterOutput.WriteField(townCity);
                string postcode = submission.Postcode;
                csvWriterOutput.WriteField(postcode);
                string customerContact = submission.CustomerContact;
                csvWriterOutput.WriteField(customerContact);
                string customerPhone = submission.CustomerPhone;
                csvWriterOutput.WriteField(customerPhone);
                string makeModel = submission.MachineMakeModel;
                csvWriterOutput.WriteField(makeModel);
                string serialNo = submission.MachineSerial;
                csvWriterOutput.WriteField(serialNo);
                string cncControl = submission.MachineController;
                csvWriterOutput.WriteField(cncControl);
                DateTime dtStart = submission.JobStart;
                csvWriterOutput.WriteField(dtStart);
                string orderNo = submission.CustomerOrderNo;
                csvWriterOutput.WriteField(orderNo);
                string mttJobNo = submission.MttJobNumber;
                csvWriterOutput.WriteField(mttJobNo);
                string jobDescription = submission.JobDescription;
                csvWriterOutput.WriteField(jobDescription);

                ServiceDayModel currentDay = serviceDays[counter];
                createExportLineForDay(currentDay);

                double totalTimeOnsite = submission.TotalTimeOnsite;
                csvWriterOutput.WriteField(totalTimeOnsite);
                double totalTravelTime = submission.TotalTravelTime;
                csvWriterOutput.WriteField(totalTravelTime);
                double mileage = submission.TotalMileage;
                csvWriterOutput.WriteField(mileage);
                double totalDailyAllowances = submission.TotalDailyAllowances;
                csvWriterOutput.WriteField(totalDailyAllowances);
                double totalOvernightAllowances = submission.TotalOvernightAllowances;
                csvWriterOutput.WriteField(totalOvernightAllowances);
                double totalBarrierPayments = submission.TotalBarrierPayments;
                csvWriterOutput.WriteField(totalBarrierPayments);
                string jobStatus = submission.JobStatus;
                csvWriterOutput.WriteField(jobStatus);
                string finalJobReport = submission.FinalJobReport;
                csvWriterOutput.WriteField(finalJobReport);
                //Follow up work field is blank
                csvWriterOutput.WriteField("");
                string additionalFaultsFound = submission.AdditionalFaultsFound;
                csvWriterOutput.WriteField(additionalFaultsFound);
                Boolean followupWorkQuote = submission.QuoteRequired;
                csvWriterOutput.WriteField(followupWorkQuote);
                string partsForFollowup = submission.PartsForFollowup;
                csvWriterOutput.WriteField(partsForFollowup);
                //Images for follow-up work - Doesn't need to be set
                csvWriterOutput.WriteField("");
                //RT 11/8/2016 - Adding in the start of the image url. 
                string imageUrlStart = "http://www.gocanvas.com/values/";
                string image1 = imageUrlStart + submission.Image1Url;
                csvWriterOutput.WriteField(image1);
                string image2 = imageUrlStart + submission.Image2Url;
                csvWriterOutput.WriteField(image2);
                string image3 = imageUrlStart + submission.Image3Url;
                csvWriterOutput.WriteField(image3);
                string image4 = imageUrlStart + submission.Image4Url;
                csvWriterOutput.WriteField(image4);
                string image5 = imageUrlStart + submission.Image5Url;
                csvWriterOutput.WriteField(image5);
                //Certify is next - don't set
                csvWriterOutput.WriteField("");
                string customerSignature = imageUrlStart + submission.CustomerSignatureUrl;
                csvWriterOutput.WriteField(customerSignature);
                string customerName = submission.CustomerSignName;
                csvWriterOutput.WriteField(customerName);
                DateTime dtSigned = submission.DtSigned;
                csvWriterOutput.WriteField(dtSigned);
                string mttSignature = imageUrlStart + submission.MttEngSignatureUrl;
                csvWriterOutput.WriteField(mttSignature);
                csvWriterOutput.NextRecord();
            }

        }

        private void createExportLineForDay(ServiceDayModel currentDay)
        {
            //This adds the day to the export
            DateTime travelStart = currentDay.TravelStartTime;
            csvWriterOutput.WriteField(travelStart);
            DateTime arrivalOnsite = currentDay.ArrivalOnsiteTime;
            csvWriterOutput.WriteField(arrivalOnsite);
            DateTime departureFromSite = currentDay.DepartSiteTime;
            csvWriterOutput.WriteField(departureFromSite);
            DateTime travelEnd = currentDay.TravelEndTime;
            csvWriterOutput.WriteField(travelEnd);
            double mileage = currentDay.Mileage;
            csvWriterOutput.WriteField(mileage);
            Boolean dailyAllowance = currentDay.DailyAllowance;
            csvWriterOutput.WriteField(dailyAllowance);
            Boolean overnightAllowance = currentDay.OvernightAllowance;
            csvWriterOutput.WriteField(overnightAllowance);
            Boolean barrierPayment = currentDay.BarrierPayment;
            csvWriterOutput.WriteField(barrierPayment);
            double travelTimeToSite = currentDay.TravelTimeToSite;
            csvWriterOutput.WriteField(travelTimeToSite);
            double travelTimeFromSite = currentDay.TravelTimeFromSite;
            csvWriterOutput.WriteField(travelTimeFromSite);
            double totalTravelTime = currentDay.TotalTravelTime;
            csvWriterOutput.WriteField(totalTravelTime);
            double totalTimeOnsite = currentDay.TotalTimeOnsite;
            csvWriterOutput.WriteField(totalTimeOnsite);
            string dailyReport = currentDay.DailyReport;
            csvWriterOutput.WriteField(dailyReport);
            string partsSupplied = currentDay.PartsSupplied;
            csvWriterOutput.WriteField(partsSupplied);
            DateTime dtTimesheet = currentDay.DtServiceDay;
            csvWriterOutput.WriteField(dtTimesheet);
        }

        private string createFilename()
        {
            string retval;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "csv files (*.csv)|*.csv";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                retval = saveDialog.FileName;    
            }
            else
            {
                retval = "";
            }

            return retval;
        }



       

        //Add a new row to the export with the data from the sheet and day.  One row per day

        //Save the export
    }
}
