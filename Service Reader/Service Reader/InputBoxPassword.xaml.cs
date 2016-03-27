using System.Windows;

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
