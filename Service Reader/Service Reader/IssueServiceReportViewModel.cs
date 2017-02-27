using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class IssueServiceReportViewModel : ObservableObject
    {
        //RT 12/12/16 - Rewriting this class to use viewmodels

        private ObservableCollection<ServiceSheetViewModel> m_allServiceSheets;
        //private ICommand m_loadCsvCommand;
        private ICommand m_createServiceSheetCommand;
        private ServiceSheetViewModel m_selectedSubmission;
        //Adding database download
        private ICommand m_downloadSheetsDatabaseCommand;


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

        //public ICommand LoadCsvCommand
        //{
        //    get
        //    {
        //        if (m_loadCsvCommand == null)
        //        {
        //            m_loadCsvCommand = new RelayCommand(param => loadHistoricalDataFromCsv());
        //        }
        //        return m_loadCsvCommand;
        //    }

        //    set
        //    {
        //        m_loadCsvCommand = value;
        //    }
        //}

        public ServiceSheetViewModel SelectedSubmission
        {
            get
            {
                return m_selectedSubmission;
            }

            set
            {
                m_selectedSubmission = value;
                onPropertyChanged("SelectedSubmission");
            }
        }

        //private void loadHistoricalDataFromCsv()
        //{
        //    //RT - This calls the import csv and loads the csv file previously created from the Canvas Submissions screen.
        //    CsvServiceImport importer = new CsvServiceImport();
        //    bool result = importer.importCsvData();
        //    AllServiceSheets = importer.AllServiceSubmissions;
        //}


        public ICommand CreateServiceSheetCommand
        {
            get
            {
                if (m_createServiceSheetCommand == null)
                {
                    m_createServiceSheetCommand = new RelayCommand(param => createPdfServiceSheetForSubmission());
                }
                return m_createServiceSheetCommand;
            }

            set
            {
                m_createServiceSheetCommand = value;
            }
        }

        public ICommand DownloadSheetsDatabaseCommand
        {
            get
            {
                if (m_downloadSheetsDatabaseCommand == null)
                {
                    m_downloadSheetsDatabaseCommand = new RelayCommand(param => downloadDataFromDatabase());
                }
                return m_downloadSheetsDatabaseCommand;
            }

            set
            {
                m_downloadSheetsDatabaseCommand = value;
            }
        }

        private void downloadDataFromDatabase()
        {
            List<ServiceSheetViewModel> serviceSheetsDownloaded = DbServiceSheet.downloadAllServiceSheets();
            if (serviceSheetsDownloaded == null)
            {
                return;
            }
            AllServiceSheets = new ObservableCollection<ServiceSheetViewModel>(serviceSheetsDownloaded);

            //Now we need to download the images from Canvas
            //RT 7/2/17 - Moving to caching
            //UserViewModel canvasUserVM = new UserViewModel(UserViewModel.MODE_CANVAS);
            //UserView userView = new UserView();
            //userView.DataContext = canvasUserVM;
            //bool? userResult = userView.ShowDialog();

            ////RT 3/12/16 - The box may have been cancelled
            //if (userResult != true)
            //{
            //    MessageBox.Show("Unable to download images. Exiting.");
            //    AllServiceSheets = null;
            //    return;
            //}

            UserViewModel userResult = CanvasDataReader.getCanvasUser();

            //RT 3/12/16 - The box may have been cancelled
            if (userResult == null)
            {
                MessageBox.Show("Unable to download images. Exiting.");
                AllServiceSheets = null;
                return;
            }

            CanvasImageDownloadViewModel imageVM = new CanvasImageDownloadViewModel(AllServiceSheets.ToList(), userResult, true);
            CanvasImageDownloadView imageDownloadView = new CanvasImageDownloadView();
            imageDownloadView.DataContext = imageVM;
            bool? result = imageDownloadView.ShowDialog();
            //Set the servicesheets back to the result from the dialog

            if (result == true)
            {
                AllServiceSheets = new ObservableCollection<ServiceSheetViewModel>(imageVM.AllServices);
            }
            else
            {
                AllServiceSheets = new ObservableCollection<ServiceSheetViewModel>();
            }
        }

        private void createPdfServiceSheetForSubmission()
        {
            //Check if there is a current sheet
            if (SelectedSubmission == null)
            {
                MessageBox.Show("Error - no sheet selected");
                return;
            }
            PdfServiceSheet serviceSheetCreator = new PdfServiceSheet();
            Boolean sheetCreated = serviceSheetCreator.createPdfSheetForSubmission(SelectedSubmission);
            if (sheetCreated)
            {
                MessageBox.Show("Sheet created");
            }
            else
            {
                MessageBox.Show("Error creating service sheet");
            }
        }

        //This is used to issue the service reports.  The data is loaded from the csv file
        //public String Name
        //{
        //    get { return "Issue Service Reports"; }
        //}

        //private List<oldServiceSubmissionModel> allServiceSubmissions;
        //private oldServiceSubmissionModel selectedSubmission;
        //private ICommand loadCsvCommand;
        ////private ICommand createCostSheetCommand;
        //private ICommand createPdfServiceSheetCommand;
        //private CanvasUserModel currentUser;
        //private DateTime fromDate;
        //private DateTime toDate;

        //public ICommand CreatePdfServiceSheet
        //{
        //    get
        //    {
        //        if (createPdfServiceSheetCommand == null)
        //        {
        //            createPdfServiceSheetCommand = new RelayCommand(param => this.createPdfServiceSheetForSubmission());
        //        }
        //        return createPdfServiceSheetCommand;
        //    }
        //}



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

        //public ICommand loadCsv
        //{
        //    get
        //    {
        //        if (loadCsvCommand == null)
        //        {
        //            loadCsvCommand = new RelayCommand(param => this.loadCsvData());
        //        }
        //        return loadCsvCommand;
        //    }
        //}

        //private void loadCsvData()
        //{
        //    CsvServiceImport csvImporter = new CsvServiceImport();
        //    Boolean successful = csvImporter.importCsvData();
        //    MessageBox.Show("Need to check for success!");
        //    //AllServiceSubmissions = csvImporter.AllServiceSubmissions;
        //}

        //public List<oldServiceSubmissionModel> AllServiceSubmissions
        //{
        //    get
        //    {
        //        return allServiceSubmissions;
        //    }
        //    set
        //    {
        //        if (value != allServiceSubmissions)
        //        {
        //            allServiceSubmissions = value;
        //            onPropertyChanged("AllServiceSubmissions");
        //        }
        //    }
        //}

        //public oldServiceSubmissionModel SelectedSubmission
        //{
        //    get { return selectedSubmission; }
        //    set
        //    {
        //        if (value != selectedSubmission)
        //        {
        //            selectedSubmission = value;
        //            onPropertyChanged("SelectedSubmission");
        //        }
        //    }
        //}

        //public List<oldServiceSubmissionModel> getAllSubmissions
        //{
        //    get { return allServiceSubmissions; }
        //    set
        //    {
        //        if (value != allServiceSubmissions)
        //        {
        //            allServiceSubmissions = value;
        //            onPropertyChanged("getAllSubmissions");
        //        }
        //    }
        //}

        //public DateTime FromDate
        //{
        //    get
        //    {
        //        return fromDate;
        //    }

        //    set
        //    {
        //        if (fromDate != value)
        //        {
        //            fromDate = value;
        //            onPropertyChanged("FromDate");
        //        }
        //    }
        //}

        //public DateTime ToDate
        //{
        //    get
        //    {
        //        return toDate;
        //    }

        //    set
        //    {
        //        if (toDate != value)
        //        {
        //            toDate = value;
        //            onPropertyChanged("ToDate");
        //        }
        //    }
        //}

        //public CanvasUserModel CurrentUser
        //{
        //    get
        //    {
        //        return currentUser;
        //    }

        //    set
        //    {
        //        if (currentUser != value)
        //        {
        //            currentUser = value;
        //            onPropertyChanged("CurrentUser");
        //        }
        //    }
        //}


    }
}
