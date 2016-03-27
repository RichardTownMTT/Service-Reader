using System;
using System.Linq;
using System.Xml.Linq;

namespace Service_Reader
{
    public class ServiceSubmission
    {
        public int submissionVersion {get; set;} = 0;
        public string username { get; set; } = "";
        public string userFirstName { get; set; } = "";
        public string userSurname { get; set; } = "";
        public int submissionNo { get; set; } = 0;
        public string customer { get; set; } = "";
        public string address1 { get; set; } = "";
        public string address2 { get; set; } = "";
        public string townCity { get; set; } = "";
        public string postcode { get; set; } = "";
        public string customerContact { get; set; } = "";
        public string customerPhone { get; set; } = "";
        public string machineMakeModel { get; set; } = "";
        public string machineSerial { get; set; } = "";
        public string machineController { get; set; } = "";
        private DateTime jobStart;
        public String getJobStart
        {
            get { return jobStart.ToString("dd/MM/yy"); }
        }
        public DateTime setJobStart
        {
            set { jobStart = value; }
        }
        public string customerOrderNo { get; set; } = "";
        public string mttJobNumber { get; set; } = "";
        public string jobDescription { get; set; } = "";
        public double totalTimeOnsite { get; set; } = 0;
        public double totalTravelTime { get; set; } = 0;
        public double totalMileage { get; set; } = 0;
        public double totalDailyAllowances { get; set; } = 0;
        public double totalOvernightAllowances { get; set; } = 0;
        public double totalBarrierPayments { get; set; } = 0;
        public string jobStatus { get; set; } = "";
        public string finalJobReport { get; set; } = "";
        public string additionalFaultsFound { get; set; } = "";
        public Boolean quoteRequired { get; set; } = false;
        public string partsForFollowup { get; set; } = "";
        public string image1Url { get; set; } = "";
        public string image2Url { get; set; } = "";
        public string image3Url { get; set; } = "";
        public string image4Url { get; set; } = "";
        public string image5Url { get; set; } = "";
        public string customerSignatureUrl { get; set; } = "";
        public string customerSignName { get; set; } = "";
        public DateTime dtSigned { get; set; }
        public string mttEngSignatureUrl = "";
        public ServiceDay[] serviceTimesheets { get; set; }


        //XML Labels
        private static string SUBMISSION_NUMBER = "SubmissionNumber";
        private static string SUBMISSION_VERSION = "Version";
        private static string USERNAME = "UserName";
        private static string FIRST_NAME = "FirstName";
        private static string SURNAME = "LastName";
        private static string CUSTOMER = "Customer";
        private static string ADDRESS_1 = "Address line 1";
        private static string ADDRESS_2 = "Address line 2";
        private static string TOWN_CITY = "Town/City";
        private static string POSTCODE = "Postcode";
        private static string CUSTOMER_CONTACT = "Customer contact";
        private static string CUSTOMER_PHONE = "Customer phone no.";
        private static string MACHINE_MAKE_MODEL = "Machine make and model";
        private static string MACHINE_CONTROL = "CNC control";
        private static string JOB_START_DATE = "Job start date";
        private static string CUSTOMER_ORDER = "Customer order no.";
        private static string MTT_JOB_NO = "MTT job no.";
        private static string JOB_DESC = "Job description";
        private static string TOTAL_TIME_ONSITE ="JobTotalTimeOnsite";
        private static string TOTAL_TRAVEL_TIME = "JobTotalTravelTime";
        private static string TOTAL_MILEAGE = "Total mileage";
        private static string TOTAL_DAILY_ALLOWANCES = "Total number of daily allowances";
        private static string TOTAL_OVERNIGHT_ALLOWANCES = "Total number of overnight allowances";
        private static string TOTAL_BARRIER_PAYMENTS = "Total number of barrier payments";
        private static string JOB_STATUS = "Job status";
        private static string FINAL_JOB_REPORT = "Final job report";
        private static string ADDITIONAL_FAULTS_FOUND = "Additional faults found";
        private static string QUOTE_REQUIRED = "Customer requires quote for follow-up work";
        private static string FOLLOWUP_PARTS = "Parts required for follow-up work";
        private static string IMAGE_1_URL = "Image 1";
        private static string IMAGE_2_URL = "Image 2";
        private static string IMAGE_3_URL = "Image 3";
        private static string IMAGE_4_URL = "Image 4";
        private static string IMAGE_5_URL = "Image 5";
        private static string CUSTOMER_SIGNATURE = "Customer signature";
        private static string CUSTOMER_NAME = "Customer name";
        private static string DATE_SIGNED = "Date Signed";
        private static string MTT_ENG_SIGNATURE = "MTT engineer signature";


        //XML tags
        private static string SECTIONS = "Sections";
        private static string SECTION = "Section";
        private static string SECTION_NAME = "Name";
        private static string FORM = "Form";
        private static string SCREENS = "Screens";
        private static string SCREEN = "Screen";
        private static string RESPONSES = "Responses";
        private static string RESPONSE_GROUPS = "ResponseGroups";
        private static string JOB_DETAILS = "Job details";
        private static string TIME_SHEET = "Time Sheet";
        private static string JOB_SIGNOFF = "Job Signoff";

        public static ServiceSubmission createSubmissionForXml(XElement submissionXml)
        {
            ServiceSubmission retval = new ServiceSubmission();
            retval.submissionNo = Int32.Parse(submissionXml.Element(SUBMISSION_NUMBER).Value);
            //Submission version is in the form element
            XElement formDetailsXml = submissionXml.Element(FORM);
            retval.submissionVersion = Int32.Parse(formDetailsXml.Element(SUBMISSION_VERSION).Value);
            retval.username = submissionXml.Element(USERNAME).Value;
            retval.userFirstName = submissionXml.Element(FIRST_NAME).Value;
            retval.userSurname = submissionXml.Element(SURNAME).Value;

            XElement sectionsXml = submissionXml.Element(SECTIONS);
            // Loop through the sections
            foreach (XElement sectionXml in sectionsXml.Elements())
            {
                string sectionName = sectionXml.Element(SECTION_NAME).Value;
                if (sectionName.Equals(JOB_DETAILS) ) 
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responsesXml = screenXml.Element(RESPONSES);
                    //Parse the job details

                    retval.customer = xmlResult(CUSTOMER, responsesXml);
                    retval.address1 = xmlResult(ADDRESS_1, responsesXml);
                    retval.address2 = xmlResult(ADDRESS_2, responsesXml);
                    retval.townCity = xmlResult(TOWN_CITY, responsesXml);
                    retval.postcode = xmlResult(POSTCODE, responsesXml);
                    retval.customerContact = xmlResult(CUSTOMER_CONTACT, responsesXml);
                    retval.customerPhone = xmlResult(CUSTOMER_PHONE, responsesXml);
                    retval.machineMakeModel = xmlResult(MACHINE_MAKE_MODEL, responsesXml);
                    retval.machineController = xmlResult(MACHINE_CONTROL, responsesXml);
                    string jobStartStr = xmlResult(JOB_START_DATE, responsesXml);
                    retval.jobStart = Convert.ToDateTime(jobStartStr);
                    retval.customerOrderNo = xmlResult(CUSTOMER_ORDER, responsesXml);
                    retval.mttJobNumber = xmlResult(MTT_JOB_NO, responsesXml);
                    retval.jobDescription = xmlResult(JOB_DESC, responsesXml);

                }
                else if (sectionName.Equals(TIME_SHEET))
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responseGroupsXml = screenXml.Element(RESPONSE_GROUPS);
                    retval.serviceTimesheets = ServiceDay.createDays(responseGroupsXml);
                }
                else if (sectionName .Equals(JOB_SIGNOFF))
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responsesXml = screenXml.Element(RESPONSES);

                    retval.totalTimeOnsite = Convert.ToDouble(xmlResult(TOTAL_TIME_ONSITE, responsesXml));
                    retval.totalTravelTime = Convert.ToDouble(xmlResult(TOTAL_TRAVEL_TIME, responsesXml));
                    retval.totalMileage = Convert.ToDouble(xmlResult(TOTAL_MILEAGE, responsesXml));
                    retval.totalDailyAllowances = Convert.ToDouble(xmlResult(TOTAL_DAILY_ALLOWANCES, responsesXml));
                    retval.totalOvernightAllowances = Convert.ToDouble(xmlResult(TOTAL_OVERNIGHT_ALLOWANCES, responsesXml));
                    retval.totalBarrierPayments = Convert.ToDouble(xmlResult(TOTAL_BARRIER_PAYMENTS, responsesXml));
                    retval.jobStatus = xmlResult(JOB_STATUS, responsesXml);
                    retval.finalJobReport = xmlResult(FINAL_JOB_REPORT, responsesXml);
                    retval.additionalFaultsFound = xmlResult(ADDITIONAL_FAULTS_FOUND, responsesXml);
                    retval.quoteRequired = Convert.ToBoolean(xmlResult(QUOTE_REQUIRED, responsesXml));
                    retval.partsForFollowup = xmlResult(FOLLOWUP_PARTS, responsesXml);
                    retval.image1Url = xmlResult(IMAGE_1_URL, responsesXml);
                    retval.image2Url = xmlResult(IMAGE_2_URL, responsesXml);
                    retval.image3Url = xmlResult(IMAGE_3_URL, responsesXml);
                    retval.image4Url = xmlResult(IMAGE_4_URL, responsesXml);
                    retval.image5Url = xmlResult(IMAGE_5_URL, responsesXml);
                    retval.customerSignatureUrl = xmlResult(CUSTOMER_SIGNATURE, responsesXml);
                    retval.customerSignName = xmlResult(CUSTOMER_NAME, responsesXml);
                    retval.dtSigned = Convert.ToDateTime(xmlResult(DATE_SIGNED, responsesXml));
                    retval.mttEngSignatureUrl = xmlResult(MTT_ENG_SIGNATURE, responsesXml);
                }
            }



            return retval;
        }

        private static string xmlResult(string label, XElement xmlInput)
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
