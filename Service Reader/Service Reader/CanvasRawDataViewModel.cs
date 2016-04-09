using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Service_Reader
{
    public class CanvasRawDataViewModel : ObservableObject, IPageViewModel
    {
        private List<ServiceSubmissionModel> allServiceSubmissions;
        //private ServiceSubmissionModel[] allServiceSubmissions;
        private canvasUserModel currentUser;
        private ICommand getCanvasDataCommand;
        private DateTime fromDate;
        private DateTime toDate;

        public string Name
        {
            get { return "Import Data"; }
        }

        public CanvasRawDataViewModel()
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

        public DateTime FromDate
        {
            get
            {
                return fromDate;
            }

            set
            {
                fromDate = value;
                onPropertyChanged("FromDate");
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
                toDate = value;
                onPropertyChanged("ToDate");
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
