using System.Windows;

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
