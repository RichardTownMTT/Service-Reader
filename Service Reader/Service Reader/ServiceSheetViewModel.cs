using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    //This handles the loading, saving, validating, etc of the service sheets from the Database and Canvas
   public class ServiceSheetViewModel
    {
        private List<ServiceSheet> m_loadedSheets;

        public void downloadServiceSheetsFromCanvas(string username, string password, DateTime dtStart, DateTime dtEnd)
        {
            //This loads the submissions from canvas
            LoadedSheets = CanvasDataReader.downloadXml(username, password, dtStart, dtEnd);
        }

        public List<ServiceSheet> LoadedSheets
        {
            get
            {
                return m_loadedSheets;
            }

            set
            {
                m_loadedSheets = value;
            }
        }
    }
}
