using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for winImageDownloadProgessBar.xaml
    /// </summary>
    public partial class winImageDownloadProgessBar : Window
    {
        //Class to show progress of downloads 
        //Updates when each submission has been processed. Some may have more images than other, so progress may not be linear
        private CanvasUserModel currentUser;
        private List<ServiceSheet> submissionList;

        public List<ServiceSheet> Submissions
        {
            get { return submissionList; }
        }

        public winImageDownloadProgessBar(List<ServiceSheet> submissions, CanvasUserModel currentUser)
        {
            this.submissionList = submissions;
            this.currentUser = currentUser;

            InitializeComponent();

           // setMaxMinProgressBar();
        }

        //    private void downloadData()
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.WorkerReportsProgress = true;
        //        worker.DoWork += worker_DoWork;
        //        worker.ProgressChanged += worker_ProgressChanged;
        //        worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        //        worker.RunWorkerAsync();
        //    }

        //    private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //    {
        //        pbDownloadStatus.Value = e.ProgressPercentage;
        //    }

        //    private void worker_DoWork(object sender, DoWorkEventArgs e)
        //    {
        //        int counter;
        //        int maxSubmissions = submissionList.Count;
        //        List<ServiceSheet> updatedSubmissions = submissionList;
        //        ServiceSheet currentSubmission;
        //        string downloadUrl;

        //        string customerSignatureUrl;
        //        string image1Url;
        //        string image2Url;
        //        string image3Url;
        //        string image4Url;
        //        string image5Url;

        //        for (counter = 0; counter < maxSubmissions; counter++)
        //        {
        //            currentSubmission = updatedSubmissions[counter];
        //            downloadUrl = currentSubmission.MttEngSignatureUrl;
        //            ImageSource imgEngSignature = CanvasDataReader.downloadImage(downloadUrl, currentUser);
        //            currentSubmission.MttEngineerSignature = imgEngSignature;

        //            //Download the customer signature
        //            customerSignatureUrl = currentSubmission.CustomerSignatureUrl;
        //            if (!customerSignatureUrl.Equals(""))
        //            {
        //                ImageSource imgCustSignature = CanvasDataReader.downloadImage(customerSignatureUrl, currentUser);
        //                currentSubmission.CustomerSignature = imgCustSignature;
        //            }

        //            //Download all the images, if they exist
        //            image1Url = currentSubmission.Image1Url;
        //            if (!image1Url.Equals(""))
        //            {
        //                ImageSource img1 = CanvasDataReader.downloadImage(image1Url, currentUser);
        //                currentSubmission.Image1 = img1;
        //            }

        //            image2Url = currentSubmission.Image2Url;
        //            if (!image2Url.Equals(""))
        //            {
        //                ImageSource img2 = CanvasDataReader.downloadImage(image2Url, currentUser);
        //                currentSubmission.Image2 = img2;
        //            }

        //            image3Url = currentSubmission.Image3Url;
        //            if (!image3Url.Equals(""))
        //            {
        //                ImageSource img3 = CanvasDataReader.downloadImage(image3Url, currentUser);
        //                currentSubmission.Image3 = img3;
        //            }

        //            image4Url = currentSubmission.Image4Url;
        //            if (!image4Url.Equals(""))
        //            {
        //                ImageSource img4 = CanvasDataReader.downloadImage(image4Url, currentUser);
        //                currentSubmission.Image4 = img4;
        //            }

        //            image5Url = currentSubmission.Image5Url;
        //            if (!image5Url.Equals(""))
        //            {
        //                ImageSource img5 = CanvasDataReader.downloadImage(image5Url, currentUser);
        //                currentSubmission.Image5 = img5;
        //            }

        //            updatedSubmissions[counter] = currentSubmission;
        //            (sender as BackgroundWorker).ReportProgress(counter);
        //        }

        //        //Return the list of submissions with the images
        //        e.Result = updatedSubmissions;
        //    }

        //    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //    {
        //        submissionList = (List<oldServiceSubmissionModel>)e.Result;
        //        this.DialogResult = true;
        //    }

        //    private void setMaxMinProgressBar()
        //    {
        //        //Set the max to the number of submissions
        //        pbDownloadStatus.Minimum = 0;
        //        pbDownloadStatus.Maximum = submissionList.Count-1;
        //    }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //downloadData();
        }
    }
}
