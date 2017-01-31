using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class DbEmployee
    {
        private string m_username;
        private string m_firstname;
        private string m_surname;

        public DbEmployee(string empUsername, string userFirstName, string userSurname)
        {
            this.Username = empUsername;
            this.Firstname = userFirstName;
            this.Surname = userSurname;
        }

        public string Username
        {
            get
            {
                return m_username;
            }

            set
            {
                m_username = value;
            }
        }

        public string Firstname
        {
            get
            {
                return m_firstname;
            }

            set
            {
                m_firstname = value;
            }
        }

        public string Surname
        {
            get
            {
                return m_surname;
            }

            set
            {
                m_surname = value;
            }
        }
    }
}
