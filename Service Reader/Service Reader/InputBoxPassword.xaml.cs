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
    /// Interaction logic for InputBoxPassword.xaml
    /// </summary>
    public partial class InputBoxPassword : Window
    {
        public InputBoxPassword(string lblText)
        {
            InitializeComponent();
            lblInputText.Content = lblText;
            txtInputData.Focus();
        }

        private string returnValueStr = "";
        public string getReturnValue
        {
            get { return this.returnValueStr; }
        }


        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            returnValueStr = txtInputData.Password;
            this.Close();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            returnValueStr = "";
            this.Close();
        }
    }
}
