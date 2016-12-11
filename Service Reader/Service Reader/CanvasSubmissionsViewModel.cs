using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Service_Reader
{
    public class CanvasSubmissionsViewModel : ObservableObject
    {
        //Stores the login details for Canvas
        private CanvasUserViewModel m_canvasUser;
        private DateTime m_dtStartSubmissionsDownload;
        private DateTime m_dtEndSubmissionsDownload;
        private ServiceSheetViewModel m_selectedSubmission;
        //RT 11/12/16 - Adding previous submission.  If editing and select submission changes, need to revert changes
        private ServiceSheetViewModel m_previousSubmission;

        //These are all the loaded canvas sheets
        private ObservableCollection<ServiceSheetViewModel> m_allServiceSheets;

        //Command to download the canvas data
        private ICommand m_canvasDataDownloadCommand;
        //RT 23/11/16 - Adding command to create CSV from data
        private ICommand m_exportCsvCommand;
        //RT 11/12/16 - Adding edit, save and cancel buttons
        private ICommand m_editSubmissionCommand;
        private ICommand m_saveSubmissionCommand;
        private ICommand m_cancelEditCommand;

        //Creator for the class.  Sets the defaults, e.g. start/end date
        public CanvasSubmissionsViewModel()
        {
            DtStartSubmissionsDownload = DateTime.Today;
            DtEndSubmissionsDownload = DateTime.Today;
            //Set the start to be a week ago by default
            DtStartSubmissionsDownload = DtStartSubmissionsDownload.AddDays(-7);

            //CanvasUserVM = new CanvasUserViewModel();
        }

        public CanvasUserViewModel CanvasUserVM
        {
            get
            {
                return m_canvasUser;
            }

            set
            {
                m_canvasUser = value;
                onPropertyChanged("CanvasUser");
            }
        }

        public DateTime DtStartSubmissionsDownload
        {
            get
            {
                return m_dtStartSubmissionsDownload;
            }

            set
            {
                m_dtStartSubmissionsDownload = value;
                onPropertyChanged("DtStartSubmissionsDownload");
            }
        }

        public DateTime DtEndSubmissionsDownload
        {
            get
            {
                return m_dtEndSubmissionsDownload;
            }

            set
            {
                m_dtEndSubmissionsDownload = value;
                onPropertyChanged("DtEndSubmissionsDownload");
            }
        }

        public ICommand CanvasDataDownloadCommand
        {
            get
            {
                if (m_canvasDataDownloadCommand == null)
                {
                    m_canvasDataDownloadCommand = new RelayCommand(downloadCanvasData);
                }
                return m_canvasDataDownloadCommand;
            }

            set
            {
                m_canvasDataDownloadCommand = value;
            }
        }

        public ICommand CsvExportCommand
        {
            get
            {
                if (m_exportCsvCommand == null)
                {
                    m_exportCsvCommand = new RelayCommand(param => exportCsvData());
                }
                return m_exportCsvCommand;
            }
            set
            {
                m_exportCsvCommand = value;
            }
        }

        private void exportCsvData()
        {
            //Need to go through the submissions and check that each has been approved.
            foreach (ServiceSheetViewModel sheet in AllServiceSheets)
            {
                if(!sheet.OfficeApproval)
                {
                    MessageBox.Show("Service sheets need approving before they can be exported.", "Error");
                    return;
                }
            }

            int noOfSheets = AllServiceSheets.Count;
            MessageBoxResult result = MessageBox.Show(noOfSheets.ToString() + " sheets will be exported to csv.  Do you want to continue?", "Export", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            //This exports all the downloaded service sheets to csv
            CsvServiceExport exporter = new CsvServiceExport();
            bool success = exporter.exportDataToCsv(AllServiceSheets);

            if (success)
            {
                MessageBox.Show("Canvas data exported to CSV", "Exported");
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

        public ServiceSheetViewModel SelectedSubmission
        {
            get
            {
                return m_selectedSubmission;
            }

            set
            {
                //RT 11/12/16 - Need to reset any changes on the previous submission
                if (PreviousSubmission != null)
                {
                    discardChangesPreviousSubmission();
                }
                //First time, we need to set to this value
                PreviousSubmission = value;
                
                m_selectedSubmission = value;
                onPropertyChanged("SelectedSubmission");
            }
        }

        private void discardChangesPreviousSubmission()
        {
            if (PreviousSubmission.EditMode)
            {
                PreviousSubmission.CancelEdit();
            }
        }

        public ICommand EditSubmissionCommand
        {
            get
            {
                if (m_editSubmissionCommand == null)
                {
                    m_editSubmissionCommand = new RelayCommand(param => editSubmissionMode());
                }
                return m_editSubmissionCommand;
            }

            set
            {
                m_editSubmissionCommand = value;
            }
        }

        public ICommand SaveSubmissionCommand
        {
            get
            {
                if (m_saveSubmissionCommand == null)
                {
                    m_saveSubmissionCommand = new RelayCommand(param => saveSubmission());
                }
                return m_saveSubmissionCommand;
            }

            set
            {
                m_saveSubmissionCommand = value;
            }
        }

        public ICommand CancelEditCommand
        {
            get
            {
                if (m_cancelEditCommand == null)
                {
                    m_cancelEditCommand = new RelayCommand(param => cancelEditSubmission());
                }
                return m_cancelEditCommand;
            }

            set
            {
                m_cancelEditCommand = value;
            }
        }

        public ServiceSheetViewModel PreviousSubmission
        {
            get
            {
                return m_previousSubmission;
            }

            set
            {
                m_previousSubmission = value;
            }
        }

        private void cancelEditSubmission()
        {
            if (SelectedSubmission == null)
            {
                MessageBox.Show("Please select a submission");
                return;
            }

            //If not in edit mode, then nothing to save.
            if (SelectedSubmission.EditMode == false)
            {
                MessageBox.Show("This submission has not been edited.");
                return;
            }

            SelectedSubmission.CancelEdit();
        }

        private void saveSubmission()
        {
            if (SelectedSubmission == null)
            {
                MessageBox.Show("Please select a submission");
                return;
            }

            //If not in edit mode, then nothing to save.
            if (SelectedSubmission.EditMode == false)
            {
                MessageBox.Show("Nothing to save. This submission has not been edited.");
                return;
            }
            SelectedSubmission.Save();
        }

        private void editSubmissionMode()
        {
            if (SelectedSubmission == null)
            {
                MessageBox.Show("Please select a submission");
                return;
            }
            SelectedSubmission.EditMode = true;
        }

        private void downloadCanvasData(object canvasPasswordBox)
        {
            CanvasUserVM = new CanvasUserViewModel();
            CanvasUserView userView = new CanvasUserView();
            userView.DataContext = CanvasUserVM;
            bool? userResult = userView.ShowDialog();

            //RT 3/12/16 - The box may have been cancelled
            if (userResult != true)
            {
                return;
            }
            
            //CanvasUserVM.CanvasPasswordBox = (PasswordBox)canvasPasswordBox;
            //RT 26/11/16 - Changing the password to use a PasswordBox for security
            //AllServiceSheets = CanvasDataReader.downloadXml(CanvasUser.Username, CanvasUser.Password, DtStartSubmissionsDownload, DtEndSubmissionsDownload);
            AllServiceSheets = CanvasDataReader.downloadXml(CanvasUserVM, DtStartSubmissionsDownload, DtEndSubmissionsDownload);

            //If no submissions have been returned, then exit.  None available, or error has occured.  Error will have been shown already
            if (AllServiceSheets == null)
            {
                return;
            }

            //Now we need to download the images from Canvas, using a progress bar
            CanvasImageDownloadView imageDownloadView = new CanvasImageDownloadView();
            List<ServiceSheetViewModel> serviceSheetList = new List<ServiceSheetViewModel>(AllServiceSheets);
            CanvasImageDownloadViewModel imageVM = new CanvasImageDownloadViewModel(serviceSheetList, CanvasUserVM);
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
    }
}
