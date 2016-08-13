using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using CsvHelper;
using System.IO;

namespace Service_Reader
{
    public class CsvServiceImport
    {
        private TextReader csvTextReader;
        private CsvReader csvReaderInput;
        private ServiceSubmissionModel currentServiceSubmission;
        List<ServiceSubmissionModel> importedSubmissions;

        //This class imports the historical data from a csv file and displays it in the application

        public Boolean importCsvData()
        {
            //First get the user to select the file with the data
            string csvFilename = openFilename();
            csvTextReader = File.OpenText(csvFilename);
            csvReaderInput = new CsvReader(csvTextReader);
            csvReaderInput.Configuration.Delimiter = ";";

            //For each record in the csv file, we need to create a submission with all the days
            //Loop through all the rows in the csv import

            //If the row submission no matches the current submission number, then we are loading days.
            //If not, then it it a new submssion

            int currentReadSubmissionNo = -1;
            importedSubmissions = new List<ServiceSubmissionModel>();

            while (csvReaderInput.Read())
            {
                var row = csvReaderInput.CurrentRecord;
                int submissionNo = Convert.ToInt32(row[2]);

                //If first time through, then set the current submission no
                //Load the first row
                if (currentReadSubmissionNo == -1)
                {
                    currentReadSubmissionNo = submissionNo;
                    currentServiceSubmission = new ServiceSubmissionModel();
                    loadNewSubmission(row);
                    continue;
                }

                if (currentReadSubmissionNo == submissionNo)
                {
                    loadDayForSubmission(row);
                }
                else
                {
                    importedSubmissions.Add(currentServiceSubmission);
                    currentReadSubmissionNo = submissionNo;
                    currentServiceSubmission = new ServiceSubmissionModel();
                    loadNewSubmission(row);
                }
                
            }


            return false;
        }

        private void loadNewSubmission(string[] row)
        {
            //First two dates aren't read
            currentServiceSubmission.SubmissionNo = Convert.ToInt32(row[2]);
            //App Name isn't used
            currentServiceSubmission.Username = row[4];
            currentServiceSubmission.UserSurname = row[5];
            currentServiceSubmission.UserFirstName = row[6];
            currentServiceSubmission.ResponseId = row[7];
            string responseDate = row[8];
            currentServiceSubmission.DtResponse = Convert.ToDateTime(responseDate);
            string deviceDate = row[9];
            currentServiceSubmission.DtDevice = Convert.ToDateTime(deviceDate);
            //Submission Form name not used
            currentServiceSubmission.SubmissionVersion = Convert.ToInt32(row[11]);
            currentServiceSubmission.Customer = row[12];
            currentServiceSubmission.Address1 = row[13];
            currentServiceSubmission.Address2 = row[14];
            currentServiceSubmission.TownCity = row[15];
            currentServiceSubmission.Postcode = row[16];
            currentServiceSubmission.CustomerContact = row[17];
            currentServiceSubmission.CustomerPhone = row[18];
            currentServiceSubmission.MachineMakeModel = row[19];
            currentServiceSubmission.MachineSerial = row[20];
            currentServiceSubmission.MachineController = row[21];
            string jobStartDate = row[22];
            currentServiceSubmission.JobStart = Convert.ToDateTime(jobStartDate);
            currentServiceSubmission.CustomerOrderNo = row[23];
            currentServiceSubmission.MttJobNumber = row[24];
            currentServiceSubmission.JobDescription = row[25];

            //Need to load the days

            currentServiceSubmission.TotalTimeOnsite = Convert.ToDouble(row[41]);

        }

        private void loadDayForSubmission(string[] row)
        {
            
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
    }
}
