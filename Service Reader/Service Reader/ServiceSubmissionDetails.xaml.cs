using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for ServiceSubmissionDetails.xaml
    /// </summary>
    public partial class ServiceSubmissionDetails : UserControl
    {
        //private CanvasSubmissionsViewModel m_submissionVM;
        public ServiceSheetViewModel currentSubmissionVM
        {
            get
            {
                return (ServiceSheetViewModel)GetValue(currentSubmissionDP);
            }
            set
            {
                SetValue(currentSubmissionDP, value);
            }
        }
        public static readonly DependencyProperty currentSubmissionDP =
            DependencyProperty.Register("currentSubmissionVM", typeof(ServiceSheetViewModel), typeof(ServiceSubmissionDetails), new PropertyMetadata(null));

        //public ServiceSubmissionDetails(ServiceSubmission serviceSheet, string username, string password)
        public ServiceSubmissionDetails()
        {
           InitializeComponent();
           LayoutRoot.DataContext = this;
           

    }

        private void test(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (currentSubmissionVM == null)
            {
                CanvasSubmissionsViewModel submissionsVM = (CanvasSubmissionsViewModel)LayoutRoot.DataContext;
                Console.WriteLine(submissionsVM.SelectedSubmission.ServiceSubmission.CanvasResponseId);
            }
            Console.WriteLine(currentSubmissionVM.ServiceSubmission.CanvasResponseId);
        }

        
    }
}
