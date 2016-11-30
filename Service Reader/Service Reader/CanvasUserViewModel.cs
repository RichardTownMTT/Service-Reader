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
    public class CanvasUserViewModel : ObservableObject
    {
        public CanvasUserViewModel()
        {
            CanvasUser = new CanvasUserModel();
        }

        private CanvasUserModel m_canvasUser;
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
                if (win.GetType() == typeof(CanvasUserView))
                {
                    CanvasUserView userView = (CanvasUserView)win;
                    CanvasPasswordBox = userView.txtPassword;
                    userView.DialogResult = true;
                }
            }
            
        }

        private CanvasUserModel CanvasUser
        {
            get
            {
                return m_canvasUser;
            }

            set
            {
                m_canvasUser = value;
                onPropertyChanged("CanvasUser");
            }
        }

        public string UserName
        {
            get
            {
                return CanvasUser.Username;
            }
            set
            {
                CanvasUser.Username = value;
                onPropertyChanged("UserName");
            }
        }

        public PasswordBox CanvasPasswordBox
        {
            get
            {
                return CanvasUser.PasswordBoxEntry;
            }
            set
            {
                CanvasUser.PasswordBoxEntry = value;
            }
        }
    }
}
