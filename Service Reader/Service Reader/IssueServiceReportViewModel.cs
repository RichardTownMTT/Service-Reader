using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class IssueServiceReportViewModel : ObservableObject
    {
        //This is used to issue the service reports.  The data is loaded from the csv file
        //public String Name
        //{
        //    get { return "Issue Service Reports"; }
        //}

        private List<oldServiceSubmissionModel> allServiceSubmissions;
        private oldServiceSubmissionModel selectedSubmission;
        private ICommand loadCsvCommand;
        //private ICommand createCostSheetCommand;
        private ICommand createPdfServiceSheetCommand;
        private CanvasUserModel currentUser;
        private DateTime fromDate;
        private DateTime toDate;

        public ICommand CreatePdfServiceSheet
        {
            get
            {
                if (createPdfServiceSheetCommand == null)
                {
                    createPdfServiceSheetCommand = new RelayCommand(param => this.createPdfServiceSheetForSubmission());
                }
                return createPdfServiceSheetCommand;
            }
        }

        private void createPdfServiceSheetForSubmission()
        {
            //Check if there is a current sheet
            if (selectedSubmission == null)
            {
                MessageBox.Show("Error - no sheet selected");
                return;
            }
            PdfServiceSheet serviceSheetCreator = new PdfServiceSheet();
            Boolean sheetCreated = serviceSheetCreator.createPdfSheetForSubmission(selectedSubmission);
            if (sheetCreated)
            {
                MessageBox.Show("Sheet created");
            }
            else
            {
                MessageBox.Show("Error creating service sheet");
            }
        }

        //RT 12/10/16 - Not used
        //private void getCanvasSubmissions()
        //{
        //    string fromDateStr = fromDate.ToString("MM/dd/yy");
        //    string toDateStr = toDate.ToString("MM/dd/yy");
        //    getAllSubmissions = CanvasDataReader.downloadXml(currentUser.Username, currentUser.Password, fromDateStr, toDateStr);

        //    //If no submissions have been returned, then exit.  None available, or error has occured.
        //    if (getAllSubmissions == null)
        //    {
        //        return;
        //    }

        //    //RT 5/6/16 - adding in download of images
        //    winImageDownloadProgessBar downloadImagesScreen = new winImageDownloadProgessBar(getAllSubmissions, currentUser);
        //    bool? result = downloadImagesScreen.ShowDialog();
        //    getAllSubmissions = downloadImagesScreen.Submissions;
        //    MessageBox.Show("Add error catch.  Can't use messages in worker thread!");
        //}

        //public ICommand createCostSheet
        //{
        //    get
        //    {
        //        if (createCostSheetCommand == null)
        //        {
        //            createCostSheetCommand = new RelayCommand(param => this.costSheetCreator());
        //        }
        //        return createCostSheetCommand;
        //    }
        //}

        //private void costSheetCreator()
        //{
        //    CreateCostSheet costSheetExporter = new CreateCostSheet();
        //    bool success = costSheetExporter.exportDataToCostSheet(selectedSubmission);
        //    MessageBox.Show("Need to check for success");
        //}

        public ICommand loadCsv
        {
            get
            {
                if (loadCsvCommand == null)
                {
                    loadCsvCommand = new RelayCommand(param => this.loadCsvData());
                }
                return loadCsvCommand;
            }
        }

        private void loadCsvData()
        {
            CsvServiceImport csvImporter = new CsvServiceImport();
            Boolean successful = csvImporter.importCsvData();
            MessageBox.Show("Need to check for success!");
            AllServiceSubmissions = csvImporter.AllServiceSubmissions;
        }

        public List<oldServiceSubmissionModel> AllServiceSubmissions
        {
            get
            {
                return allServiceSubmissions;
            }
            set
            {
                if (value != allServiceSubmissions)
                {
                    allServiceSubmissions = value;
                    onPropertyChanged("AllServiceSubmissions");
                }
            }
        }

        public oldServiceSubmissionModel SelectedSubmission
        {
            get { return selectedSubmission; }
            set
            {
                if (value != selectedSubmission)
                {
                    selectedSubmission = value;
                    onPropertyChanged("SelectedSubmission");
                }
            }
        }

        public List<oldServiceSubmissionModel> getAllSubmissions
        {
            get { return allServiceSubmissions; }
            set
            {
                if (value != allServiceSubmissions)
                {
                    allServiceSubmissions = value;
                    onPropertyChanged("getAllSubmissions");
                }
            }
        }

        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                if (fromDate != value)
                {
                    fromDate = value;
                    onPropertyChanged("FromDate");
                }
            }
        }

        public DateTime ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                if (toDate != value)
                {
                    toDate = value;
                    onPropertyChanged("ToDate");
                }
            }
        }

        public CanvasUserModel CurrentUser
        {
            get
            {
                return currentUser;
            }

            set
            {
                if (currentUser != value)
                {
                    currentUser = value;
                    onPropertyChanged("CurrentUser");
                }
            }
        }
    }
}
