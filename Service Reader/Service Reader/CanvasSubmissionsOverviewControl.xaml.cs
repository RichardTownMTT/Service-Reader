using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UcCanvasSubmissionsOverview : UserControl
    {
       // private string canvasUsername = "";
       // private string canvasPassword = "";

        //private ServiceSubmission[] allSubmissions;
        public UcCanvasSubmissionsOverview()
        {
            InitializeComponent();
            
        }

        private void dgvSubmissions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ServiceSubmissionModel currentItem = (ServiceSubmissionModel)dataGrid.CurrentItem;
            //ucSubmissions.currentSubmission = currentItem;
        }


        //private void DownloadCanvasData_Click(object sender, RoutedEventArgs e)
        //{
        //    DateTime dtSubmissionsStart = new DateTime();
        //    DateTime dtSubmissionsEnd = new DateTime();
        //    if (dtSubmissionsFrom.SelectedDate == null)
        //    {
        //        MessageBox.Show("You must select a start date");
        //        return;
        //    }
        //    else
        //    {
        //        dtSubmissionsStart = (DateTime)dtSubmissionsFrom.SelectedDate;
        //    }
        //    if (dtSubmissionsTo.SelectedDate == null)
        //    {
        //        MessageBox.Show("You must select an end date");
        //        return;
        //    }
        //    else
        //    {
        //        dtSubmissionsEnd = (DateTime)dtSubmissionsTo.SelectedDate;
        //    }

        //    if (dtSubmissionsStart > dtSubmissionsEnd)
        //    {
        //        MessageBox.Show("Start date must be before end date.");
        //        return;
        //    }

        //    //Now we need to download all the data for the selected range
        //    InputBox usernameInput = new InputBox("Please enter your canvas username:");
        //    usernameInput.ShowDialog();
        //    canvasUsername = usernameInput.getReturnValue;
        //    if (canvasUsername == "")
        //    {
        //        MessageBox.Show("You must enter a username");
        //        return;
        //    }

        //    InputBoxPassword passwordInput = new InputBoxPassword("Please enter your canvas password:");
        //    passwordInput.ShowDialog();
        //    canvasPassword = passwordInput.getReturnValue;
        //    if (canvasPassword == "")
        //    {
        //        MessageBox.Show("You must enter a password");
        //        return;
        //    }

        //    string fromDate = "";
        //    fromDate = dtSubmissionsStart.Month + "/" + dtSubmissionsStart.Day + "/" + dtSubmissionsStart.Year;
        //    string endDate = "";
        //    endDate = dtSubmissionsEnd.Month + "/" + dtSubmissionsEnd.Day + "/" + dtSubmissionsEnd.Year;


        //    allSubmissions = CanvasDataReader.downloadXml(canvasUsername, canvasPassword, fromDate, endDate);

        //    //If allsubmissions is null, then an error has occured
        //    if (allSubmissions != null)
        //    {
        //        displaySubmissions(allSubmissions);
        //    }


        //}

        //private void displaySubmissions(ServiceSubmission[] allSubmissions)
        //{
        //    if (allSubmissions.Length == 0)
        //    {
        //        MessageBox.Show("No submissions to display");
        //        return;
        //    }
        //    var itemList = new List<ServiceSubmission>();

        //    foreach (ServiceSubmission currentSubmission in allSubmissions)
        //    {
        //        itemList.Add(currentSubmission);
        //    }


        //    CollectionViewSource itemCollectionViewSource;
        //    itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
        //    itemCollectionViewSource.Source = itemList;
        //}

        //private void btnViewJobDetails_Click(object sender, RoutedEventArgs e)
        //{
        //    ServiceSubmission selectedService = (ServiceSubmission)JobSheetOverview.SelectedItem;
        //    ServiceSubmissionDetails viewSubmission = new ServiceSubmissionDetails(selectedService, canvasUsername, canvasPassword);
        //    viewSubmission.Show();

        //}
    }
}
