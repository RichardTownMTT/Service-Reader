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
using System.Net;
using System.Xml;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DownloadCanvasData_Click(object sender, RoutedEventArgs e)
        {
            DateTime dtSubmissionsStart = new DateTime();
            DateTime dtSubmissionsEnd = new DateTime();
            if (dtSubmissionsFrom.SelectedDate == null)
            {
                MessageBox.Show("You must select a start date");
                return;
            }
            else
            {
                dtSubmissionsStart = (DateTime)dtSubmissionsFrom.SelectedDate;
            }
            if (dtSubmissionsTo.SelectedDate == null)
            {
                MessageBox.Show("You must select an end date");
                return;
            }
            else
            {
                dtSubmissionsEnd = (DateTime)dtSubmissionsTo.SelectedDate;
            }

            if (dtSubmissionsStart > dtSubmissionsEnd)
            {
                MessageBox.Show("Start date must be before end date.");
                return;
            }

            //Now we need to download all the data for the selected range
            string canvasUsername = "";
            InputBox usernameInput = new InputBox("Please enter your canvas username:");
            usernameInput.ShowDialog();
            canvasUsername = usernameInput.getReturnValue;
            if (canvasUsername == "")
            {
                MessageBox.Show("You must enter a username");
                return;
            }
            string canvasPassword = "";
            InputBoxPassword passwordInput = new InputBoxPassword("Please enter your canvas password:");
            passwordInput.ShowDialog();
            canvasPassword = passwordInput.getReturnValue;
            if (canvasPassword == "")
            {
                MessageBox.Show("You must enter a password");
                return;
            }

            XmlDocument canvasXML = new XmlDocument();
            canvasXML = CanvasDownload.downloadXml(canvasUsername, canvasPassword);


            


        }
    }
}
