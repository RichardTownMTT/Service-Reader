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
        private int mode = -1;
        public static int MODE_CANVAS = 1;
        public static int MODE_DATABASE = 2;

        public static readonly string CACHE_DATABASE_USER = "DbUserDetails";
        public static readonly string CACHE_CANVAS_USER = "CanvasUserDetails";

        public UserViewModel(int modeSet)
        {
            Mode = modeSet;
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

        public UserModel User
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

        public int Mode
        {
            get
            {
                return mode;
            }

            set
            {
                mode = value;
                onPropertyChanged("Mode");
                //Update the display message
                onPropertyChanged("DisplayMessage");
            }
        }

        public string DisplayMessage
        {
            get
            {
                if (mode == MODE_CANVAS)
                {
                    return "Enter your Canvas username and password";
                }
                else if(mode == MODE_DATABASE)
                {
                    return "Enter your service database username and password";
                }
                else
                {
                    throw new Exception("Display mode not found for " + mode);
                }
            }
        }
    }
}
