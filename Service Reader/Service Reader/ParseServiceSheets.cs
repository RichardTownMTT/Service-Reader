using System;
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
        public static XmlDocument downloadXml(string canvasUsername, string canvasPassword, string beginDate, string endDate)
        {
            XmlDocument retval = new XmlDocument();

            string canvasUrl = "https://www.gocanvas.com/apiv2/submissions.xml?username=" + canvasUsername + "&password=" + canvasPassword + "&form_id=1285373&begin_date=" + beginDate + "&end_date=" + endDate;

            XDocument xDoc = XDocument.Load(canvasUrl);
            XElement rootElement = xDoc.Root;
            
            XElement totalPagesNode = rootElement.Element("TotalPages");
            string noOfPages = totalPagesNode.Value;

            XElement submissions = rootElement.Element("Submissions");

            parseSubmissions(submissions);
            

            return retval;
        }

        private static void parseSubmissions(XElement submissions)
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

        }
    }
}
