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
    public partial class CanvasImageDownloadView : Window
    {
        //Class to show progress of downloads 
        //Updates when each submission has been processed. Some may have more images than other, so progress may not be linear
        

        public CanvasImageDownloadView()
        { 

            InitializeComponent();
        }
    }
}
