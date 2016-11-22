using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Service_Reader
{
    public class CanvasImageDownloadViewModel : ObservableObject
    {
        public CanvasImageDownloadViewModel(List<ServiceSheetViewModel> serviceVMs, CanvasUserModel user)
        {
            AllServices = serviceVMs;
            CurrentUser = user;
            setMaxMinProgressBar();
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private CanvasUserModel m_currentUser;
        private List<ServiceSheetViewModel> m_allServices;
        private int m_minimumDownloadedItems;
        private int m_maximumDownloadedItems;
        private int m_currentStatus;
        private bool m_dialogComplete = false;

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AllServices = (List<ServiceSheetViewModel>)e.Result;
            DialogComplete = true;
        }

        private void setMaxMinProgressBar()
        {
            //Set the max to the number of submissions
            MinimumDownloadedItems = 0;
            MaximumDownloadedItems = AllServices.Count - 1;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentStatus = e.ProgressPercentage;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int maxSubmissions = AllServices.Count;
            List<ServiceSheetViewModel> updatedSubmissions = new List<ServiceSheetViewModel>();
            string downloadUrl;

            string customerSignatureUrl;
            string image1Url;
            string image2Url;
            string image3Url;
            string image4Url;
            string image5Url;

            foreach (ServiceSheetViewModel currentSubmission in AllServices.ToList())
            {
                downloadUrl = currentSubmission.MttEngSignatureUrl;
                ImageSource imgEngSignature = CanvasDataReader.downloadImage(downloadUrl, CurrentUser);
                currentSubmission.MttEngineerSignature = imgEngSignature;
                currentSubmission.MttEngineerSignature.Freeze();

                //Download the customer signature
                customerSignatureUrl = currentSubmission.CustomerSignatureUrl;
                if (!customerSignatureUrl.Equals(""))
                {
                    ImageSource imgCustSignature = CanvasDataReader.downloadImage(customerSignatureUrl, CurrentUser);
                    currentSubmission.CustomerSignature = imgCustSignature;
                    currentSubmission.CustomerSignature.Freeze();
                }

                //Download all the images, if they exist
                image1Url = currentSubmission.Image1Url;
                if (!image1Url.Equals(""))
                {
                    ImageSource img1 = CanvasDataReader.downloadImage(image1Url, CurrentUser);
                    currentSubmission.Image1 = img1;
                    currentSubmission.Image1.Freeze();
                }

                image2Url = currentSubmission.Image2Url;
                if (!image2Url.Equals(""))
                {
                    ImageSource img2 = CanvasDataReader.downloadImage(image2Url, CurrentUser);
                    currentSubmission.Image2 = img2;
                    currentSubmission.Image2.Freeze();
                }

                image3Url = currentSubmission.Image3Url;
                if (!image3Url.Equals(""))
                {
                    ImageSource img3 = CanvasDataReader.downloadImage(image3Url, CurrentUser);
                    currentSubmission.Image3 = img3;
                    currentSubmission.Image3.Freeze();
                }

                image4Url = currentSubmission.Image4Url;
                if (!image4Url.Equals(""))
                {
                    ImageSource img4 = CanvasDataReader.downloadImage(image4Url, CurrentUser);
                    currentSubmission.Image4 = img4;
                    currentSubmission.Image4.Freeze();
                }

                image5Url = currentSubmission.Image5Url;
                if (!image5Url.Equals(""))
                {
                    ImageSource img5 = CanvasDataReader.downloadImage(image5Url, CurrentUser);
                    currentSubmission.Image5 = img5;
                    currentSubmission.Image5.Freeze();
                }

                updatedSubmissions.Add(currentSubmission);
                CurrentStatus = CurrentStatus + 1;
                (sender as BackgroundWorker).ReportProgress(CurrentStatus);
            }

            //Return the list of submissions with the images
            e.Result = updatedSubmissions;
        }

        public CanvasUserModel CurrentUser
        {
            get
            {
                return m_currentUser;
            }
            set
            {
                m_currentUser = value;
                onPropertyChanged("CurrentUser");
            }
        }

        public List<ServiceSheetViewModel> AllServices
        {
            get
            {
                return m_allServices;
            }

            set
            {
                m_allServices = value;
                onPropertyChanged("AllServices");
            }
        }

        public int MinimumDownloadedItems
        {
            get
            {
                return m_minimumDownloadedItems;
            }

            set
            {
                m_minimumDownloadedItems = value;
                onPropertyChanged("MinimumDownloadedItems");
            }
        }

        public int MaximumDownloadedItems
        {
            get
            {
                return m_maximumDownloadedItems;
            }

            set
            {
                m_maximumDownloadedItems = value;
                onPropertyChanged("MaximumDownloadedItems");
            }
        }

        public int CurrentStatus
        {
            get
            {
                return m_currentStatus;
            }

            set
            {
                m_currentStatus = value;
                onPropertyChanged("CurrentStatus");
            }
        }

        public bool DialogComplete
        {
            get
            {
                return m_dialogComplete;
            }

            set
            {
                m_dialogComplete = value;
                onPropertyChanged("DialogComplete");
            }
        }
    }
}
