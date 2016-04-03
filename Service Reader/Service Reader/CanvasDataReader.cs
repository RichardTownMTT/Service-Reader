using System.Windows;
using System.Linq;
using System.Xml.Linq;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;

namespace Service_Reader
{
    class CanvasDataReader
    {
        //XML Labels for fields
        public static string SUBMISSION_NUMBER = "SubmissionNumber";
        public static string SUBMISSION_VERSION = "Version";
        public static string USERNAME = "UserName";
        public static string FIRST_NAME = "FirstName";
        public static string SURNAME = "LastName";
        public static string CUSTOMER = "Customer";
        public static string ADDRESS_1 = "Address line 1";
        public static string ADDRESS_2 = "Address line 2";
        public static string TOWN_CITY = "Town/City";
        public static string POSTCODE = "Postcode";
        public static string CUSTOMER_CONTACT = "Customer contact";
        public static string CUSTOMER_PHONE = "Customer phone no.";
        public static string MACHINE_MAKE_MODEL = "Machine make and model";
        public static string MACHINE_CONTROL = "CNC control";
        public static string JOB_START_DATE = "Job start date";
        public static string CUSTOMER_ORDER = "Customer order no.";
        public static string MTT_JOB_NO = "MTT job no.";
        public static string JOB_DESC = "Job description";
        public static string TOTAL_TIME_ONSITE = "JobTotalTimeOnsite";
        public static string TOTAL_TRAVEL_TIME = "JobTotalTravelTime";
        public static string TOTAL_MILEAGE = "Total mileage";
        public static string TOTAL_DAILY_ALLOWANCES = "Total number of daily allowances";
        public static string TOTAL_OVERNIGHT_ALLOWANCES = "Total number of overnight allowances";
        public static string TOTAL_BARRIER_PAYMENTS = "Total number of barrier payments";
        public static string JOB_STATUS = "Job status";
        public static string FINAL_JOB_REPORT = "Final job report";
        public static string ADDITIONAL_FAULTS_FOUND = "Additional faults found";
        public static string QUOTE_REQUIRED = "Customer requires quote for follow-up work";
        public static string FOLLOWUP_PARTS = "Parts required for follow-up work";
        public static string IMAGE_1_URL = "Image 1";
        public static string IMAGE_2_URL = "Image 2";
        public static string IMAGE_3_URL = "Image 3";
        public static string IMAGE_4_URL = "Image 4";
        public static string IMAGE_5_URL = "Image 5";
        public static string CUSTOMER_SIGNATURE = "Customer signature";
        public static string CUSTOMER_NAME = "Customer name";
        public static string DATE_SIGNED = "Date Signed";
        public static string MTT_ENG_SIGNATURE = "MTT engineer signature";

        //XML tags for sections
        public static string SECTIONS = "Sections";
        public static string SECTION = "Section";
        public static string SECTION_NAME = "Name";
        public static string FORM = "Form";
        public static string SCREENS = "Screens";
        public static string SCREEN = "Screen";
        public static string RESPONSES = "Responses";

        public static string RESPONSE_GROUPS = "ResponseGroups";
        public static string JOB_DETAILS = "Job details";
        public static string TIME_SHEET = "Time Sheet";
        public static string JOB_SIGNOFF = "Job Signoff";

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

        public static BitmapImage getImage(string imageReference, string username, string password)
        {
            BitmapImage retval = null;

            string canvasUrl = "https://www.gocanvas.com/apiv2/images.xml?image_id=" + imageReference + "&username=" + username + "&password=" + password;


            try
            {
                WebClient wc = new WebClient();
                
                byte[] imageBytes = wc.DownloadData(canvasUrl);
                MemoryStream ms = new MemoryStream(imageBytes);
                retval = new BitmapImage();
                retval.BeginInit();
                retval.StreamSource = ms;
                retval.EndInit();

            }
            catch
            {
                MessageBox.Show("Catch Error");
            }

            return retval;
        }

        //Method to retrieve the result daata for a given label in the xml
        public static string xmlResult(string label, XElement xmlInput)
        {
            var resultXML = xmlInput.Elements("Response").Where(x => x.Element("Label").Value == label);
            XElement result = resultXML.First();

            string retval = "";

            object resultStr = result.Element("Value").Value;
            if (resultStr != null)
            {
                retval = (string)resultStr;
            }

            return retval;
        }
    }
}
