using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Service_Reader
{
    public class CanvasImageDownloadViewModel : ObservableObject
    {
        private static bool m_fullUrl;
        public CanvasImageDownloadViewModel(List<ServiceSheetViewModel> serviceVMs, UserViewModel canvasUserEntered, bool fullUrlSet)
        {
            FullUrl = fullUrlSet;
            AllServices = serviceVMs;
            CanvasUser = canvasUserEntered;
            setMaxMinProgressBar();
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private UserViewModel m_canvasUser;
        private List<ServiceSheetViewModel> m_allServices;
        private int m_minimumDownloadedItems;
        private int m_maximumDownloadedItems;
        private int m_currentStatus;

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AllServices = (List<ServiceSheetViewModel>)e.Result;
            //Close the current dialog
            foreach (Window currentWindow in Application.Current.Windows)
            {
                if (currentWindow.GetType() == typeof(CanvasImageDownloadView))
                {
                    if (AllServices == null)
                    {
                        currentWindow.DialogResult = false;
                    }
                    else
                    {
                        currentWindow.DialogResult = true;
                    }
                    //currentWindow.Close();
                    //return;
                }
            }
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
                ImageSource imgEngSignature = CanvasDataReader.downloadImage(downloadUrl, CanvasUser, FullUrl);

                //If there has been an error with the image download, then return an empty collection
                if (imgEngSignature == null)
                {
                    updatedSubmissions = null;
                    return;
                }
                
                currentSubmission.MttEngineerSignature = imgEngSignature;
                currentSubmission.MttEngineerSignature.Freeze();
                

                //Download the customer signature
                customerSignatureUrl = currentSubmission.CustomerSignatureUrl;
                if (!customerSignatureUrl.Equals(""))
                {
                    ImageSource imgCustSignature = CanvasDataReader.downloadImage(customerSignatureUrl, CanvasUser, FullUrl);
                    
                    //If there has been an error with the image download, then return an empty collection
                    if (imgCustSignature == null)
                    {
                        updatedSubmissions = null;
                        return;
                    }

                    currentSubmission.CustomerSignature = imgCustSignature;
                    currentSubmission.CustomerSignature.Freeze();
                }

                //Download all the images, if they exist
                image1Url = currentSubmission.Image1Url;
                if (!image1Url.Equals(""))
                {
                    ImageSource img1 = CanvasDataReader.downloadImage(image1Url, CanvasUser, FullUrl);

                    //If there has been an error with the image download, then return an empty collection
                    if (img1 == null)
                    {
                        updatedSubmissions = null;
                        return;
                    }

                    currentSubmission.Image1 = img1;
                    currentSubmission.Image1.Freeze();
                }

                image2Url = currentSubmission.Image2Url;
                if (!image2Url.Equals(""))
                {
                    ImageSource img2 = CanvasDataReader.downloadImage(image2Url, CanvasUser, FullUrl);

                    //If there has been an error with the image download, then return an empty collection
                    if (img2 == null)
                    {
                        updatedSubmissions = null;
                        return;
                    }

                    currentSubmission.Image2 = img2;
                    currentSubmission.Image2.Freeze();
                }

                image3Url = currentSubmission.Image3Url;
                if (!image3Url.Equals(""))
                {
                    ImageSource img3 = CanvasDataReader.downloadImage(image3Url, CanvasUser, FullUrl);

                    //If there has been an error with the image download, then return an empty collection
                    if (img3 == null)
                    {
                        updatedSubmissions = null;
                        return;
                    }

                    currentSubmission.Image3 = img3;
                    currentSubmission.Image3.Freeze();
                }

                image4Url = currentSubmission.Image4Url;
                if (!image4Url.Equals(""))
                {
                    ImageSource img4 = CanvasDataReader.downloadImage(image4Url, CanvasUser, FullUrl);

                    //If there has been an error with the image download, then return an empty collection
                    if (img4 == null)
                    {
                        updatedSubmissions = null;
                        return;
                    }

                    currentSubmission.Image4 = img4;
                    currentSubmission.Image4.Freeze();
                }

                image5Url = currentSubmission.Image5Url;
                if (!image5Url.Equals(""))
                {
                    ImageSource img5 = CanvasDataReader.downloadImage(image5Url, CanvasUser, FullUrl);

                    //If there has been an error with the image download, then return an empty collection
                    if (img5 == null)
                    {
                        updatedSubmissions = null;
                        return;
                    }

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

        public UserViewModel CanvasUser
        {
            get
            {
                return m_canvasUser;
            }
            set
            {
                m_canvasUser = value;
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

        public static bool FullUrl
        {
            get
            {
                return m_fullUrl;
            }

            set
            {
                m_fullUrl = value;
            }
        }
    }
}
