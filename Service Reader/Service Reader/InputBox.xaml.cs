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
using System.Windows.Shapes;

namespace Service_Reader
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        private string returnValueStr = "";
        public string getReturnValue
        {
            get { return this.returnValueStr; }
        }

        public InputBox(string lblText) 
        {
            InitializeComponent();
            lblInputText.Content = lblText;
            txtInputData.Focus();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            returnValueStr = txtInputData.Text;
            this.Close();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            returnValueStr = "";
            this.Close();
        }
    }
}
