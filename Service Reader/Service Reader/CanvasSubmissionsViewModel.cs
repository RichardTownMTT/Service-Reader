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
    class CanvasSubmissionsViewModel : ObservableObject
    {
        //Stores the login details for Canvas
        private CanvasUserModel m_canvasUser;
        private DateTime m_dtStartSubmissionsDownload;
        private DateTime m_dtEndSubmissionsDownload;

        //These are all the loaded canvas sheets
        private ObservableCollection<ServiceSheet> m_allServiceSheets;

        //Command to download the canvas data
        private ICommand m_canvasDataDownloadCommand;

        //Creator for the class.  Sets the defaults, e.g. start/end date
        public CanvasSubmissionsViewModel()
        {
            DtStartSubmissionsDownload = DateTime.Today;
            DtEndSubmissionsDownload = DateTime.Today;
            //Set the start to be a week ago by default
            DtStartSubmissionsDownload = DtStartSubmissionsDownload.AddDays(-7);

            CanvasUser = new CanvasUserModel();
        }

        public CanvasUserModel CanvasUser
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
                    m_canvasDataDownloadCommand = new RelayCommand(param => downloadCanvasData());
                }
                return m_canvasDataDownloadCommand;
            }

            set
            {
                m_canvasDataDownloadCommand = value;
            }
        }

        public ObservableCollection<ServiceSheet> AllServiceSheets
        {
            get
            {
                return m_allServiceSheets;
            }

            set
            {
                m_allServiceSheets = value;
            }
        }

        private void downloadCanvasData()
        {
            AllServiceSheets = CanvasDataReader.downloadXml(CanvasUser.Username, CanvasUser.Password, DtStartSubmissionsDownload, DtEndSubmissionsDownload);

            //If no submissions have been returned, then exit.  None available, or error has occured.  Error will have been shown already
            if (AllServiceSheets == null)
            {
                return;
            }
            
            MessageBox.Show("Add error catch.  Can't use messages in worker thread!");
        }
    }
}
