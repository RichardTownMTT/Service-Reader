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
                return (ServiceSheetViewModel)GetValue(currentSubmissionVMProperty);
            }
            set
            {
                SetValue(currentSubmissionVMProperty, value);
            }
        }
        public static readonly DependencyProperty currentSubmissionVMProperty =
            DependencyProperty.Register("currentSubmissionVM", typeof(ServiceSheetViewModel), typeof(ServiceSubmissionDetails), new PropertyMetadata(null));

        //public ServiceSubmissionDetails(ServiceSubmission serviceSheet, string username, string password)
        public ServiceSubmissionDetails()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;


        }
    }
}
