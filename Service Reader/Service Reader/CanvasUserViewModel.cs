using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Service_Reader
{
    public class CanvasUserViewModel : ObservableObject
    {
        public CanvasUserViewModel()
        {
            CanvasUser = new CanvasUserModel();
        }

        private CanvasUserModel m_canvasUser;

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
