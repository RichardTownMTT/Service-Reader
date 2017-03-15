using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    interface IUser 
    {
        IUser IUser(int mode);
        IUser retrieveUser(int mode);
        void clearCredentials(int mode);
        int getMode();


    }
}
