using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows;

namespace Service_Reader
{
    public class oldSubmissionViewModel : ObservableObject
    {
        private List<ServiceSheet> allServiceSubmissions;
        //private ServiceSubmissionModel[] allServiceSubmissions;
        private UserModel currentUser;
        private ICommand getCanvasDataCommand;
        private DateTime fromDate;
        private DateTime toDate;
        private ServiceSheet selectedSubmission;
        //private ICommand createPdfServiceSheetCommand;
        private ICommand editSubmissionCommand;
        private ICommand saveEditSubmissionCommand;
        private ICommand cancelEditSubmissionCommand;
        private ICommand exportToCsvCommand;

        //public string Name
        //{
        //    get { return "Process Canvas Data"; }
        //}

        public oldSubmissionViewModel()
        {
            currentUser = new UserModel();
            fromDate = DateTime.Now.AddDays(-7);
            toDate = DateTime.Now;
        }

        public ICommand GetXmlDataCommand
        {
            get
            {
                if (getCanvasDataCommand == null)
                {
                    
                    getCanvasDataCommand = new RelayCommand(param => this.getCanvasSubmissions());
                }
                return getCanvasDataCommand;
            }
        }

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

        //RT 3/8/16 - adding commands for the save, edit and cancel commands.  These also toggle the readonly properties on the service submission
        //public ICommand editSubmission
        //{
        //    get
        //    {
        //        if (editSubmissionCommand == null)
        //        {
        //           editSubmissionCommand = new RelayCommand(param => this.beginEdit());
        //        }
        //        return editSubmissionCommand;
        //    }
        //}

       //private void beginEdit()
       // {
       //     if (selectedSubmission == null)
       //     {
       //         MessageBox.Show("No submission selected.");
       //         return;
       //     }
       //     selectedSubmission.BeginEdit();  
       // }

        //public ICommand saveSubmission
        //{
        //    get
        //    {
        //        if (saveEditSubmissionCommand == null)
        //        {
        //            saveEditSubmissionCommand = new RelayCommand(param => this.saveEdit());
        //        }
        //        return saveEditSubmissionCommand;
        //    }
        //}

        //private void saveEdit()
        //{
        //    if (selectedSubmission == null)
        //    {
        //        MessageBox.Show("No submission selected.");
        //        return;
        //    }
        //    selectedSubmission.EndEdit();
        //}

        //public ICommand cancelEditSubmission
        //{
        //    get
        //    {
        //        if (cancelEditSubmissionCommand == null)
        //        {
        //            cancelEditSubmissionCommand = new RelayCommand(param => this.cancelEdit());
        //        }
        //        return cancelEditSubmissionCommand; ;
        //    }
        //}

        //private void cancelEdit()
        //{
        //    if (selectedSubmission == null)
        //    {
        //        MessageBox.Show("No submission selected.");
        //        return;
        //    }
        //    selectedSubmission.CancelEdit();
        //}

        public ICommand exportToCsv
        {
            get
            {
                if (exportToCsvCommand == null)
                {
                    exportToCsvCommand = new RelayCommand(param => this.csvExport());
                }
                return exportToCsvCommand;
            }
        }

        private void csvExport()
        {
            //CsvServiceExport exporter = new CsvServiceExport();
            //Boolean successful = exporter.exportDataToCsv(allServiceSubmissions);
        }

        //private void createPdfServiceSheetForSubmission()
        //{
        //    //Check if there is a current sheet
        //    if (selectedSubmission == null)
        //    {
        //        MessageBox.Show("Error - no sheet selected");
        //        return;
        //    }
        //    PdfServiceSheet serviceSheetCreator = new PdfServiceSheet();
        //    Boolean sheetCreated = serviceSheetCreator.createPdfSheetForSubmission(selectedSubmission);
        //    if (sheetCreated)
        //    {
        //        MessageBox.Show("Sheet created");
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error creating service sheet");
        //    }
        //}

        private void getCanvasSubmissions()
        {
            //string fromDateStr = fromDate.ToString("MM/dd/yy");
            //string toDateStr = toDate.ToString("MM/dd/yy");
            //getAllSubmissions = CanvasDataReader.downloadXml(currentUser.Username, currentUser.Password, FromDate, ToDate);

            ////If no submissions have been returned, then exit.  None available, or error has occured.
            //if (getAllSubmissions == null)
            //{
            //    return;
            //}

            ////RT 5/6/16 - adding in download of images
            ////winImageDownloadProgessBar downloadImagesScreen = new winImageDownloadProgessBar(getAllSubmissions, currentUser);
            ////bool? result = downloadImagesScreen.ShowDialog();
            ////getAllSubmissions = downloadImagesScreen.Submissions;
            //MessageBox.Show("Add error catch.  Can't use messages in worker thread!");
        }

        public List<ServiceSheet> getAllSubmissions
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

        //public ServiceSheet SelectedSubmission
        //{
        //    get { return selectedSubmission; }
        //    set
        //    {
        //        if (value != selectedSubmission)
        //        {
        //            //RT 25/7/16 - Get rid of any unsaved changes
        //            if (selectedSubmission != null)
        //            {
        //                selectedSubmission.CancelEdit();
        //            }
        //            selectedSubmission = value;
        //            onPropertyChanged("SelectedSubmission");
        //            Console.WriteLine("Selected Submission Changed");
        //        }
        //    }
        //}

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

        public UserModel CurrentUser
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
