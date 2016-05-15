using System.Windows;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;
using System;
using System.Collections.ObjectModel;

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
        public static string JOB_TOTAL_TIME_ONSITE = "JobTotalTimeOnsite";
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

        //Service day tags
        private static string DATE = "Date";
        private static string TRAVEL_START = "Travel time start";
        private static string ARRIVE_ONSITE = "Arrival time on site";
        private static string DEPART_SITE = "Departure time from site";
        private static string TRAVEL_END = "Travel end time";
        private static string MILEAGE = "Mileage";
        private static string DAILY_ALLOWANCE = "Daily allowance";
        private static string OVERNIGHT_ALLOWANCE = "Overnight allowance";
        private static string BARRIER_PAYMENT = "Barrier payment";
        private static string TRAVEL_TO_SITE = "Travel time to site";
        private static string TRAVEL_FROM_SITE = "Travel time from site";
        private static string TOTAL_TRAVEL = "Total travel time";
        private static string TOTAL_TIME_ONSITE = "Total time onsite";
        private static string DAILY_REPORT = "Daily report";
        private static string PARTS_SUPPLIED = "Parts supplied today";

        //XML tags for sections
        public static string SECTIONS = "Sections";
        public static string SECTION = "Section";
        public static string SECTION_NAME = "Name";
        public static string FORM = "Form";
        public static string SCREENS = "Screens";
        public static string SCREEN = "Screen";
        public static string RESPONSES = "Responses";
        private static string RESPONSE_GROUP = "ResponseGroup";

        public static string RESPONSE_GROUPS = "ResponseGroups";
        public static string JOB_DETAILS = "Job details";
        public static string TIME_SHEET = "Time Sheet";
        public static string JOB_SIGNOFF = "Job Signoff";

        //public static ServiceSubmissionModel[] downloadXml(string canvasUsername, string canvasPassword, string beginDate, string endDate)
        public static List<ServiceSubmissionModel> downloadXml(string canvasUsername, string canvasPassword, string beginDate, string endDate)
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

            //ServiceSubmissionModel[] allSubmissions;
            List<ServiceSubmissionModel> allSubmissions;
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

        //private static ServiceSubmissionModel[] parseSubmissions(XElement submissions)
        private static List<ServiceSubmissionModel> parseSubmissions(XElement submissions)
        {
            //int noOfSubmissions = 0;
            //noOfSubmissions = submissions.Descendants("Submission").Count();

            List<ServiceSubmissionModel> allSubmissions = new List<ServiceSubmissionModel>();
            //int submissionCounter = 0;

            foreach (XElement submissionXml in submissions.Elements())
            {
                //Load the submission
                ServiceSubmissionModel currentSubmission = createSubmissionForXml(submissionXml);

                allSubmissions.Add(currentSubmission);

                //submissionCounter++;
            }
            return allSubmissions;
        }

        public static ServiceSubmissionModel createSubmissionForXml(XElement submissionXml)
        {
            ServiceSubmissionModel retval = new ServiceSubmissionModel();
            retval.SubmissionNo = Int32.Parse(submissionXml.Element(SUBMISSION_NUMBER).Value);
            //Submission version is in the form element
            XElement formDetailsXml = submissionXml.Element(FORM);
            retval.SubmissionVersion = Int32.Parse(formDetailsXml.Element(SUBMISSION_VERSION).Value);
            retval.Username = submissionXml.Element(USERNAME).Value;
            retval.UserFirstName = submissionXml.Element(FIRST_NAME).Value;
            retval.UserSurname = submissionXml.Element(SURNAME).Value;

            XElement sectionsXml = submissionXml.Element(SECTIONS);
            // Loop through the sections
            foreach (XElement sectionXml in sectionsXml.Elements())
            {
                string sectionName = sectionXml.Element(SECTION_NAME).Value;
                if (sectionName.Equals(JOB_DETAILS))
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responsesXml = screenXml.Element(RESPONSES);
                    //Parse the job details

                    retval.Customer = xmlResult(CUSTOMER, responsesXml);
                    retval.Address1 = xmlResult(ADDRESS_1, responsesXml);
                    retval.Address2 = xmlResult(ADDRESS_2, responsesXml);
                    retval.TownCity = xmlResult(TOWN_CITY, responsesXml);
                    retval.Postcode = xmlResult(POSTCODE, responsesXml);
                    retval.CustomerContact = xmlResult(CUSTOMER_CONTACT, responsesXml);
                    retval.CustomerPhone = xmlResult(CUSTOMER_PHONE, responsesXml);
                    retval.MachineMakeModel = xmlResult(MACHINE_MAKE_MODEL, responsesXml);
                    retval.MachineController = xmlResult(MACHINE_CONTROL, responsesXml);
                    string jobStartStr = xmlResult(JOB_START_DATE, responsesXml);
                    retval.JobStart = Convert.ToDateTime(jobStartStr);
                    retval.CustomerOrderNo = xmlResult(CUSTOMER_ORDER, responsesXml);
                    retval.MttJobNumber = xmlResult(MTT_JOB_NO, responsesXml);
                    retval.JobDescription = xmlResult(JOB_DESC, responsesXml);

                }
                else if (sectionName.Equals(TIME_SHEET))
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responseGroupsXml = screenXml.Element(RESPONSE_GROUPS);
                    //retval.ServiceTimesheets = ServiceDay.createDays(responseGroupsXml);
                    retval.ServiceTimesheets = createDays(responseGroupsXml);
                }
                else if (sectionName.Equals(JOB_SIGNOFF))
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responsesXml = screenXml.Element(RESPONSES);

                    retval.TotalTimeOnsite = Convert.ToDouble(xmlResult(JOB_TOTAL_TIME_ONSITE, responsesXml));
                    retval.TotalTravelTime = Convert.ToDouble(xmlResult(TOTAL_TRAVEL_TIME, responsesXml));
                    retval.TotalMileage = Convert.ToDouble(xmlResult(TOTAL_MILEAGE, responsesXml));
                    //retval.TotalDailyAllowances = Convert.ToDouble(xmlResult(TOTAL_DAILY_ALLOWANCES, responsesXml));
                    //retval.TotalOvernightAllowances = Convert.ToDouble(xmlResult(TOTAL_OVERNIGHT_ALLOWANCES, responsesXml));
                    //retval.TotalBarrierPayments = Convert.ToDouble(xmlResult(TOTAL_BARRIER_PAYMENTS, responsesXml));
                    retval.JobStatus = xmlResult(JOB_STATUS, responsesXml);
                    retval.FinalJobReport = xmlResult(FINAL_JOB_REPORT, responsesXml);
                    retval.AdditionalFaultsFound = xmlResult(ADDITIONAL_FAULTS_FOUND, responsesXml);
                    retval.QuoteRequired = Convert.ToBoolean(xmlResult(QUOTE_REQUIRED, responsesXml));
                    retval.PartsForFollowup = xmlResult(FOLLOWUP_PARTS, responsesXml);
                    retval.Image1Url = xmlResult(IMAGE_1_URL, responsesXml);
                    retval.Image2Url = xmlResult(IMAGE_2_URL, responsesXml);
                    retval.Image3Url = xmlResult(IMAGE_3_URL, responsesXml);
                    retval.Image4Url = xmlResult(IMAGE_4_URL, responsesXml);
                    retval.Image5Url = xmlResult(IMAGE_5_URL, responsesXml);
                    retval.CustomerSignatureUrl = xmlResult(CUSTOMER_SIGNATURE, responsesXml);
                    retval.CustomerSignName = xmlResult(CUSTOMER_NAME, responsesXml);
                    retval.DtSigned = Convert.ToDateTime(xmlResult(DATE_SIGNED, responsesXml));
                    retval.MttEngSignatureUrl = xmlResult(MTT_ENG_SIGNATURE, responsesXml);
                }
                else
                {
                    new Exception("Unknown Canvas Data Section");
                }
                
            }
            return retval;
        }

        public static ObservableCollection<ServiceDayModel> createDays(XElement allDays)
        {
            int totalDays;
            totalDays = allDays.Descendants(RESPONSE_GROUP).Count();
            ObservableCollection<ServiceDayModel> retval = new ObservableCollection<ServiceDayModel>();

            foreach (XElement responseGroupXml in allDays.Elements())
            {
                ServiceDayModel dayOfService = new ServiceDayModel();
                string dtServiceStr = xmlResult(DATE, responseGroupXml);
                dayOfService.DtServiceDay = Convert.ToDateTime(dtServiceStr);
                XElement sectionXml = responseGroupXml.Element(SECTION);
                XElement screensXml = sectionXml.Element(SCREENS);
                XElement screenXml = screensXml.Element(SCREEN);
                XElement responsesXml = screenXml.Element(RESPONSES);

                string travelStartStr = xmlResult(TRAVEL_START, responsesXml);
                dayOfService.TravelStartTime = Convert.ToDateTime(dtServiceStr + " " + travelStartStr);
                string arrivalOnsiteStr = xmlResult(ARRIVE_ONSITE, responsesXml);
                dayOfService.ArrivalOnsiteTime = Convert.ToDateTime(dtServiceStr + " " + arrivalOnsiteStr);
                string departSiteStr = xmlResult(DEPART_SITE, responsesXml);
                dayOfService.DepartSiteTime = Convert.ToDateTime(dtServiceStr + " " + departSiteStr);
                string travelEndStr = xmlResult(TRAVEL_END, responsesXml);
                dayOfService.TravelEndTime = Convert.ToDateTime(dtServiceStr + " " + travelEndStr);
                dayOfService.Mileage = Convert.ToDouble(xmlResult(MILEAGE, responsesXml));
                dayOfService.DailyAllowance = Convert.ToDouble(xmlResult(DAILY_ALLOWANCE, responsesXml));
                dayOfService.OvernightAllowance = Convert.ToDouble(xmlResult(OVERNIGHT_ALLOWANCE, responsesXml));
                dayOfService.BarrierPayment = Convert.ToDouble(xmlResult(BARRIER_PAYMENT, responsesXml));
                dayOfService.TravelTimeToSite = Convert.ToDouble(xmlResult(TRAVEL_TO_SITE, responsesXml));
                dayOfService.TravelTimeFromSite = Convert.ToDouble(xmlResult(TRAVEL_FROM_SITE, responsesXml));
                dayOfService.TotalTravelTime = Convert.ToDouble(xmlResult(TOTAL_TRAVEL, responsesXml));
                dayOfService.TotalTimeOnsite = Convert.ToDouble(xmlResult(TOTAL_TIME_ONSITE, responsesXml));
                dayOfService.DailyReport = xmlResult(DAILY_REPORT, responsesXml);
                dayOfService.PartsSupplied = xmlResult(PARTS_SUPPLIED, responsesXml);

                retval.Add(dayOfService);
            }

            return retval;
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
