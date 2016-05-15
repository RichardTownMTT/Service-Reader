using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for ViewEditServiceDay.xaml
    /// </summary>
    public partial class ServiceDayView : UserControl
    {

        //public ServiceDayViewModel currentTimesheet
        //{
        //    get
        //    {
        //        return (ServiceDayViewModel)GetValue(currentTimesheetDP);
        //    }
        //    set
        //    {
        //        SetValue(currentTimesheetDP, value);
        //    }
        //}
        //public static readonly DependencyProperty currentTimesheetDP =
        //    DependencyProperty.Register("currentSubmission", typeof(ServiceDayViewModel), typeof(ServiceDayView), new PropertyMetadata(null));



        //public object travelStart
        //{
        //    get { return GetValue(dpTravelStart); }
        //    set { SetValue(dpTravelStart, value); }
        //}

        //public static readonly DependencyProperty dpTravelStart = DependencyProperty.Register("travelStart", typeof(object), typeof(ViewEditServiceDay), new PropertyMetadata(""));

        public ServiceDayView()
        { 
            InitializeComponent();
            //LayoutRoot.DataContext = this;
        }
    }
}
