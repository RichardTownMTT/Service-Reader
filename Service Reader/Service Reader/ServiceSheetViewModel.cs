using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Service_Reader
{
    //This handles the loading, saving, validating, etc of the service sheets from the Database and Canvas
   public class ServiceSheetViewModel
    {
        private List<ServiceSheet> m_loadedSheets;
        private CanvasUserModel currentUser;
        private ICommand getCanvasDataCommand;
        private DateTime fromDate;
        private DateTime toDate;
        public void downloadServiceSheetsFromCanvas(string username, string password, DateTime dtStart, DateTime dtEnd)
        {
            //This loads the submissions from canvas
            LoadedSheets = CanvasDataReader.downloadXml(username, password, dtStart, dtEnd);
        }

        public List<ServiceSheet> LoadedSheets
        {
            get
            {
                return m_loadedSheets;
            }

            set
            {
                m_loadedSheets = value;
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
                    //onPropertyChanged("FromDate");
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
                    //onPropertyChanged("ToDate");
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
                    //onPropertyChanged("CurrentUser");
                }
            }
        }
    }
}
