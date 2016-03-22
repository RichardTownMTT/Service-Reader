using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service_Reader
{
    class ServiceSubmission
    {
        private int submissionVersion = 0;
        private string username = "";
        private string userFirstName = "";
        private string userSurname = "";
        private int submissionNo = 0;
        private string customer = "";
        private string address1 = "";
        private string address2 = "";
        private string townCity = "";
        private string postcode = "";
        private string customerContact = "";
        private string customerPhone = "";
        private string machineMakeModel = "";
        private string machineSerial = "";
        private string machineController = "";
        private DateTime jobStart;
        private string customerOrderNo = "";
        private string mttJobNumber = "";
        private string jobDescription = "";


        private static string SUBMISSION_NUMBER = "SubmissionNumber";
        private static string SUBMISSION_VERSION = "Version";
        private static string USERNAME = "UserName";
        private static string FIRST_NAME = "FirstName";
        private static string SURNAME = "LastName";

        //XML tags
        private static string SECTIONS = "Sections";
        private static string SECTION = "Section";
        private static string SECTION_NAME = "Name";
        private static string JOB_DETAILS = "Job details";
        private static string TIME_SHEET = "Time Sheet";
        private static string JOB_SIGNOFF = "Job Signoff";

        public static ServiceSubmission createSubmissionForXml(XElement submissionXml)
        {
            ServiceSubmission retval = new ServiceSubmission();
            retval.submissionNo = Int32.Parse(submissionXml.Element(SUBMISSION_NUMBER).Value);
            //Submission version is in the form element
            XElement formDetailsXml = submissionXml.Element("Form");
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
                    XElement screensXml = sectionXml.Element("Screens");
                    XElement screenXml = screensXml.Element("Screen");
                    XElement responsesXml = screenXml.Element("Responses");
                    //Parse the job details

                    var customerXML = responsesXml.Elements("Response").Where(x => x.Element("Label").Value == "Customer");
                    XElement result = customerXML.First();

                    retval.customer = result.Element("Value").Value;
                }
                else if (sectionName.Equals(TIME_SHEET))
                {

                }
                else if (sectionName .Equals(JOB_SIGNOFF))
                {

                }
            }



            return retval;
        }

    }
}
