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

        private static string SUBMISSION_NUMBER = "SubmissionNumber";
        private static string SUBMISSION_VERSION = "Version";
        private static string USERNAME = "UserName";
        private static string FIRST_NAME = "FirstName";
        private static string SURNAME = "LastName";
        private static string SECTIONS = "Sections";
        private static string SECTION = "Section";
        private static string SECTION_NAME = "Name";

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
            }



            return retval;
        }

    }
}
