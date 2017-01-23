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
    /// Interaction logic for UcCanvasUser.xaml
    /// </summary>
    public partial class UserView : Window
    {
        public UserView()
        {
            InitializeComponent();
            //LayoutRoot.DataContext = this;
        }

        
        //public String userName
        //{
        //    get { return (String)GetValue(userNameProperty); }
        //    set { SetValue(userNameProperty, value); }
        //}

        ///// <summary>
        ///// Identified the Label dependency property
        ///// </summary>
        //public static readonly DependencyProperty userNameProperty =
        //    DependencyProperty.Register("userName", typeof(string),
        //      typeof(UcCanvasUser), new PropertyMetadata(""));


        //public String password
        //{
        //    get { return (String)GetValue(passwordProperty); }
        //    set { SetValue(passwordProperty, value); }
        //}
        
        //public static readonly DependencyProperty passwordProperty =
        //    DependencyProperty.Register("password", typeof(string),
        //      typeof(UcCanvasUser), new PropertyMetadata(""));

    }
}
