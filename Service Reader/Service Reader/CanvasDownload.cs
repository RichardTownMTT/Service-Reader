using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace Service_Reader
{
    class CanvasDownload
    {
        public static XmlDocument downloadXml(string canvasUsername, string canvasPassword)
        {
            XmlDocument retval = new XmlDocument();

            string canvasUrl = "https://www.gocanvas.com/apiv2/submissions.xml?username=" + canvasUsername + "&password=" + canvasPassword + "&form_id=1285373";

            string canvasResults = null;
            WebClient downloadClient = new WebClient();
            canvasResults = downloadClient.DownloadString(canvasUrl);
            retval.LoadXml(canvasResults);

            return retval;
        }
    }
}
