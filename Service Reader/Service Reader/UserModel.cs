﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;

namespace Service_Reader
{
    //Model for the canvas user login 
    //Stores the username and password
    public class UserModel : ObservableObject
    {
        private string m_username = "";
        //RT 21/10/16 - Changing to use a password box
        //private string m_password = "";
        //private String m_password;
        //RT 26/11/16 - Changing password to be a password box.
        private PasswordBox m_passwordBoxEntry;

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

        //public string Password
        //{
        //    get
        //    {
        //        return Password1;
        //    }

        //    set
        //    {
        //        if (value != this.Password1)
        //        {
        //            this.Password1 = value;
        //            onPropertyChanged("Password");
        //        }
        //    }
        //}

        //public String Password
        //{
        //    get
        //    {
        //        return m_password;
        //    }

        //    set
        //    {
        //        m_password = value;
        //    }
        //}

        public PasswordBox PasswordBoxEntry
        {
            get
            {
                return m_passwordBoxEntry;
            }

            set
            {
                m_passwordBoxEntry = value;
                onPropertyChanged("PasswordBox");
            }
        }
    }
}
