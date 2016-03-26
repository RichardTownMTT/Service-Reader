using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Net;

namespace Service_Reader
{
    class ParseServiceSheets
    {
        public static ServiceSubmission[] downloadXml(string canvasUsername, string canvasPassword, string beginDate, string endDate)
        {
            string canvasUrl = "https://www.gocanvas.com/apiv2/submissions.xml?username=" + canvasUsername + "&password=" + canvasPassword + "&form_id=1285373&begin_date=" + beginDate + "&end_date=" + endDate;

            XElement rootElement;

            try
            {
                XDocument xDoc = XDocument.Load(canvasUrl);
                rootElement = xDoc.Root;
            }
            catch
            {
                MessageBox.Show("Unable to download data.");
                return null;
            }

            //Need to check for errors
            string errorCode = validateCanvasXml(rootElement);

            if (!errorCode.Equals(""))
            {
                return null;
            }
            
            XElement totalPagesNode = rootElement.Element("TotalPages");
            string noOfPages = totalPagesNode.Value;

            XElement submissions = rootElement.Element("Submissions");

            ServiceSubmission[] allSubmissions;
            allSubmissions = parseSubmissions(submissions);
            

            return allSubmissions;
        }

        private static string validateCanvasXml(XElement rootElement)
        {
            string retval = "";
            string errorMessage = "";
            XElement resultXML = rootElement.Element("Error");

            if (resultXML != null)
            {
                retval = resultXML.Element("ErrorCode").Value;
                errorMessage = resultXML.Element("Description").Value;
            }

            if (!retval.Equals(""))
            {
                MessageBox.Show(errorMessage);
            }

            return retval;
        }

        private static ServiceSubmission[] parseSubmissions(XElement submissions)
        {
            int noOfSubmissions = 0;
            noOfSubmissions = submissions.Descendants("Submission").Count();

            ServiceSubmission[] allSubmissions = new ServiceSubmission [noOfSubmissions];
            int submissionCounter = 0;

            foreach (XElement submissionXml in submissions.Elements())
            {
                //Load the submission
                ServiceSubmission currentSubmission = ServiceSubmission.createSubmissionForXml(submissionXml);

                allSubmissions[submissionCounter] = currentSubmission;

                submissionCounter++;
            }
            return allSubmissions;
        }
    }
}
