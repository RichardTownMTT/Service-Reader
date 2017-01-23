using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Service_Reader
{
    public class UserViewModel : ObservableObject
    {
        private int displayMode = -1;
        public static int DISPLAY_MODE_CANVAS = 1;
        public static int DISPLAY_MODE_DATABASE = 2;

        public UserViewModel(int displayModeSet)
        {
            DisplayMode = displayModeSet;
            User = new UserModel();
        }

        private UserModel m_user;
        private ICommand m_setUserCommand;

        public ICommand SetUserCommand
        {
            get
            {
                if (m_setUserCommand == null)
                {
                    m_setUserCommand = new RelayCommand(param => setUser());
                }
                return m_setUserCommand;
            }
        }

        private void setUser()
        {
            foreach (Window win in App.Current.Windows)
            {
                if (win.GetType() == typeof(UserView))
                {
                    UserView userView = (UserView)win;
                    PasswordBoxObj = userView.txtPassword;
                    userView.DialogResult = true;
                }
            }
            
        }

        private UserModel User
        {
            get
            {
                return m_user;
            }

            set
            {
                m_user = value;
                onPropertyChanged("User");
            }
        }

        public string UserName
        {
            get
            {
                return User.Username;
            }
            set
            {
                User.Username = value;
                onPropertyChanged("UserName");
            }
        }

        public PasswordBox PasswordBoxObj
        {
            get
            {
                return User.PasswordBoxEntry;
            }
            set
            {
                User.PasswordBoxEntry = value;
            }
        }

        public int DisplayMode
        {
            get
            {
                return displayMode;
            }

            set
            {
                displayMode = value;
                onPropertyChanged("DisplayMode");
                //Update the display message
                onPropertyChanged("DisplayMessage");
            }
        }

        public string DisplayMessage
        {
            get
            {
                if (displayMode == DISPLAY_MODE_CANVAS)
                {
                    return "Enter your Canvas username and password.";
                }
                else if(displayMode == DISPLAY_MODE_DATABASE)
                {
                    return "Enter your service database username and password";
                }
                else
                {
                    throw new Exception("Display mode not found for " + displayMode);
                }
            }
        }
    }
}
