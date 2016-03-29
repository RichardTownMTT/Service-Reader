using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for ServiceSubmissionDetails.xaml
    /// </summary>
    public partial class ServiceSubmissionDetails : Window
    {
           
        public ServiceSubmissionDetails(ServiceSubmission serviceSheet, string username, string password)
        {
            InitializeComponent();

            txtSubmissionNo.Text = Convert.ToString(serviceSheet.submissionNo);
            txtEngineer.Text = serviceSheet.userFirstName + " " + serviceSheet.userSurname;
            txtCustomer.Text = serviceSheet.customer;
            txtAddress1.Text = serviceSheet.address1;
            txtAddress2.Text = serviceSheet.address2;
            txtTownCity.Text = serviceSheet.townCity;
            txtPostcode.Text = serviceSheet.postcode;
            txtCustomerContact.Text = serviceSheet.customerContact;
            txtCustomerPhone.Text = serviceSheet.customerPhone;
            txtMakeModel.Text = serviceSheet.machineMakeModel;
            txtMachineSerial.Text = serviceSheet.machineSerial;
            txtMachineController.Text = serviceSheet.machineController;
            dtJobStart.SelectedDate = Convert.ToDateTime(serviceSheet.getJobStart);
            txtCustomerOrderNo.Text = serviceSheet.customerOrderNo;
            txtMttJobNo.Text = serviceSheet.mttJobNumber;
            txtJobDescription.Text = serviceSheet.jobDescription;

            ServiceDay[] allTimesheets;
            allTimesheets = serviceSheet.serviceTimesheets;
            displayTimesheets(allTimesheets);

            txtTotalTimeOnsite.Text = Convert.ToString(serviceSheet.totalTimeOnsite);
            txtTotalTravelTime.Text = Convert.ToString(serviceSheet.totalTravelTime);
            txtTotalMileage.Text = Convert.ToString(serviceSheet.totalMileage);
            txtTotalDailyAllowances.Text = Convert.ToString(serviceSheet.totalDailyAllowances);
            txtTotalOvernightAllowances.Text = Convert.ToString(serviceSheet.totalOvernightAllowances);
            txtTotalBarrierPayments.Text = Convert.ToString(serviceSheet.totalBarrierPayments);
            txtJobStatus.Text = serviceSheet.jobStatus;
            txtFinalReport.Text = serviceSheet.finalJobReport;
            txtAdditionalFaultsFound.Text = serviceSheet.additionalFaultsFound;
            chkQuoteRequired.IsChecked = serviceSheet.quoteRequired;
            txtPartsForFollowup.Text = serviceSheet.partsForFollowup;
            txtImage1.Text = serviceSheet.image1Url;
            txtImage2.Text = serviceSheet.image2Url;
            txtImage3.Text = serviceSheet.image3Url;
            txtImage4.Text = serviceSheet.image4Url;
            txtImage5.Text = serviceSheet.image5Url;
            txtCustomerSignature.Text = serviceSheet.customerSignatureUrl;
            txtCustomerSignName.Text = serviceSheet.customerSignName;
            dtSigned.SelectedDate = serviceSheet.dtSigned;
            txtEngineerSignature.Text = serviceSheet.mttEngSignatureUrl;

            //retrieveImages(serviceSheet);
            

            imgEngineerSignature.Source = CanvasDataReader.getImage(serviceSheet.mttEngSignatureUrl, username, password);

        }


        private void displayTimesheets(ServiceDay[] allTimesheets)
        {
            var itemList = new List<ServiceDay>();

            foreach (ServiceDay currentDay in allTimesheets)
            {
                itemList.Add(currentDay);
            }

            //link business data to CollectionViewSource
            CollectionViewSource ItemCollectionTimesheetView;
            ItemCollectionTimesheetView = (CollectionViewSource)(FindResource("ItemCollectionTimesheets"));
            ItemCollectionTimesheetView.Source = itemList;
        }
        
    }
}
