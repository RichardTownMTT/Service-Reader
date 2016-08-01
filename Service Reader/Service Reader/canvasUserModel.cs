using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Service_Reader
{
    //Model for the canvas user login 
    //Stores the username and password
    public class CanvasUserModel : ObservableObject
    {
        private string m_username = "";
        private string m_password = "";

        public string Username
        {
            get
            {
                return m_username;
            }

            set
            {
                if (value != this.m_username)
                {
                    this.m_username = value;
                    onPropertyChanged("Username");
                }
            }
        }

        public string Password
        {
            get
            {
                return m_password;
            }

            set
            {
                if (value != this.m_password)
                {
                    this.m_password = value;
                    onPropertyChanged("Password");
                }
            }
        }
    }
}
