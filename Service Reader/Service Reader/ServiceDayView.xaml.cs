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

        public ServiceDayViewModel serviceDayVM
        {
            get
            {
                return (ServiceDayViewModel)GetValue(ServiceDayDP);
            }
            set
            {
                SetValue(ServiceDayDP, value);
            }
        }

        public static DependencyProperty ServiceDayDP = DependencyProperty.Register("serviceDayVM", typeof(ServiceDayViewModel), typeof(ServiceDayView), new PropertyMetadata(null));

        
        public ServiceDayView()
        { 
            InitializeComponent();
            //LayoutRoot.DataContext = this;
        }

        private void test(object sender, MouseEventArgs e)
        {
            Console.WriteLine(UcServiceDayView.DataContext.ToString());
        }
    }
}
