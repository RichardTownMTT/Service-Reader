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
        private canvasUserModel currentUser;
        private List<ServiceSubmissionModel> submissionList;

        public List<ServiceSubmissionModel> Submissions
        {
            get { return submissionList; }
        }

        public winImageDownloadProgessBar(List<ServiceSubmissionModel> submissions, canvasUserModel currentUser) 
        {
            this.submissionList = submissions;
            this.currentUser = currentUser;

            InitializeComponent();

            setMaxMinProgressBar();
        }

        private void downloadData()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbDownloadStatus.Value = e.ProgressPercentage;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            int counter;
            int maxSubmissions = submissionList.Count;
            ServiceSubmissionModel currentSubmission;
            string downloadUrl;

            for (counter = 0; counter < maxSubmissions; counter++)
            {
                currentSubmission = submissionList[counter];
                downloadUrl = currentSubmission.MttEngSignatureUrl;
                Image imgEngSignature = CanvasDataReader.downloadImage(downloadUrl, currentUser);
                (sender as BackgroundWorker).ReportProgress(counter);
            }
        }

        private void setMaxMinProgressBar()
        {
            //Set the max to the number of submissions
            pbDownloadStatus.Minimum = 0;
            pbDownloadStatus.Maximum = submissionList.Count;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            downloadData();
        }
    }
}
