using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

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
        //private ServiceSubmissionModel selectedSubmission;

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

        private void getCanvasSubmissions()
        {
            string fromDateStr = fromDate.ToString("MM/dd/yy");
            string toDateStr = toDate.ToString("MM/dd/yy");
            getAllSubmissions = CanvasDataReader.downloadXml(currentUser.Username, currentUser.Password, fromDateStr, toDateStr);
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

        //public ServiceSubmissionModel SelectedSubmission
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
