using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows;

namespace Service_Reader
{
    public class SubmissionViewModel : ObservableObject, IPageViewModel
    {
        private List<ServiceSubmissionModel> allServiceSubmissions;
        //private ServiceSubmissionModel[] allServiceSubmissions;
        private canvasUserModel currentUser;
        private ICommand getCanvasDataCommand;
        private DateTime fromDate;
        private DateTime toDate;
        private ServiceSubmissionModel selectedSubmission;
        private ICommand createPdfServiceSheetCommand;

        public string Name
        {
            get { return "Process Canvas Data"; }
        }

        public SubmissionViewModel()
        {
            currentUser = new canvasUserModel();
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

        private void getCanvasSubmissions()
        {
            string fromDateStr = fromDate.ToString("MM/dd/yy");
            string toDateStr = toDate.ToString("MM/dd/yy");
            getAllSubmissions = CanvasDataReader.downloadXml(currentUser.Username, currentUser.Password, fromDateStr, toDateStr);

            //If no submissions have been returned, then exit.  None available, or error has occured.
            if (getAllSubmissions == null)
            {
                return;
            }

            //RT 5/6/16 - adding in download of images
            winImageDownloadProgessBar downloadImagesScreen = new winImageDownloadProgessBar(getAllSubmissions, currentUser);
            bool? result = downloadImagesScreen.ShowDialog();
            getAllSubmissions = downloadImagesScreen.Submissions;
            MessageBox.Show("Add error catch.  Can't use messages in worker thread!");
        }

        public List<ServiceSubmissionModel> getAllSubmissions
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

        public ServiceSubmissionModel SelectedSubmission
        {
            get { return selectedSubmission; }
            set
            {
                if (value != selectedSubmission)
                {
                    //RT 25/7/16 - Get rid of any unsaved changes
                    if (selectedSubmission != null)
                    {
                        selectedSubmission.CancelEdit();
                    }
                    selectedSubmission = value;
                    onPropertyChanged("SelectedSubmission");
                    Console.WriteLine("Selected Submission Changed");
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

        public canvasUserModel CurrentUser
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
