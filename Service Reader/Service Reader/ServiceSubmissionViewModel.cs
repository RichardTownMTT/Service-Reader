using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel;

namespace Service_Reader
{
    //ViewModel for the service Submission
    public class ServiceSubmissionViewModel : ObservableObject
    {
        private ServiceSubmissionModel currentServiceSubmission;

        public ServiceSubmissionModel getCurrentServiceSubmission
        {
            get { return currentServiceSubmission; }
            set
            {
                if (value != currentServiceSubmission)
                {
                    currentServiceSubmission = value;
                    onPropertyChanged("currentServiceSubmission");      
                }
            }
        }


        //Class creator that receives the XML data from canvas
        public static ServiceSubmissionModel createServiceSubmission(XElement xmlData)
        {
            ServiceSubmissionModel retval = new ServiceSubmissionModel();
            retval.SubmissionNo = Int32.Parse(xmlData.Element(CanvasDataReader.SUBMISSION_NUMBER).Value);
            //Submission version is in the form element
            XElement formDetailsXml = xmlData.Element(CanvasDataReader.FORM);
            retval.SubmissionVersion = Int32.Parse(formDetailsXml.Element(CanvasDataReader.SUBMISSION_VERSION).Value);
            retval.Username = xmlData.Element(CanvasDataReader.USERNAME).Value;
            retval.UserFirstName = xmlData.Element(CanvasDataReader.FIRST_NAME).Value;
            retval.UserSurname = xmlData.Element(CanvasDataReader.SURNAME).Value;

            XElement sectionsXml = xmlData.Element(CanvasDataReader.SECTIONS);
            // Loop through the sections
            foreach (XElement sectionXml in sectionsXml.Elements())
            {
                string sectionName = sectionXml.Element(CanvasDataReader.SECTION_NAME).Value;
                if (sectionName.Equals(CanvasDataReader.JOB_DETAILS))
                {
                    XElement screensXml = sectionXml.Element(CanvasDataReader.SCREENS);
                    XElement screenXml = screensXml.Element(CanvasDataReader.SCREEN);
                    XElement responsesXml = screenXml.Element(CanvasDataReader.RESPONSES);
                    //Parse the job details

                    retval.Customer = CanvasDataReader.xmlResult(CanvasDataReader.CUSTOMER, responsesXml);
                    retval.Address1 = CanvasDataReader.xmlResult(CanvasDataReader.ADDRESS_1, responsesXml);
                    retval.Address2 = CanvasDataReader.xmlResult(CanvasDataReader.ADDRESS_2, responsesXml);
                    retval.TownCity = CanvasDataReader.xmlResult(CanvasDataReader.TOWN_CITY, responsesXml);
                    retval.Postcode = CanvasDataReader.xmlResult(CanvasDataReader.POSTCODE, responsesXml);
                    retval.CustomerContact = CanvasDataReader.xmlResult(CanvasDataReader.CUSTOMER_CONTACT, responsesXml);
                    retval.CustomerPhone = CanvasDataReader.xmlResult(CanvasDataReader.CUSTOMER_PHONE, responsesXml);
                    retval.MachineMakeModel = CanvasDataReader.xmlResult(CanvasDataReader.MACHINE_MAKE_MODEL, responsesXml);
                    retval.MachineController = CanvasDataReader.xmlResult(CanvasDataReader.MACHINE_CONTROL, responsesXml);
                    string jobStartStr = CanvasDataReader.xmlResult(CanvasDataReader.JOB_START_DATE, responsesXml);
                    retval.JobStart = Convert.ToDateTime(jobStartStr);
                    retval.CustomerOrderNo = CanvasDataReader.xmlResult(CanvasDataReader.CUSTOMER_ORDER, responsesXml);
                    retval.MttJobNumber = CanvasDataReader.xmlResult(CanvasDataReader.MTT_JOB_NO, responsesXml);
                    retval.JobDescription = CanvasDataReader.xmlResult(CanvasDataReader.JOB_DESC, responsesXml);

                }
                else if (sectionName.Equals(CanvasDataReader.TIME_SHEET))
                {
                    XElement screensXml = sectionXml.Element(CanvasDataReader.SCREENS);
                    XElement screenXml = screensXml.Element(CanvasDataReader.SCREEN);
                    XElement responseGroupsXml = screenXml.Element(CanvasDataReader.RESPONSE_GROUPS);
                    retval.ServiceTimesheets = ServiceDay.createDays(responseGroupsXml);
                }
                else if (sectionName.Equals(CanvasDataReader.JOB_SIGNOFF))
                {
                    XElement screensXml = sectionXml.Element(CanvasDataReader.SCREENS);
                    XElement screenXml = screensXml.Element(CanvasDataReader.SCREEN);
                    XElement responsesXml = screenXml.Element(CanvasDataReader.RESPONSES);

                    retval.TotalTimeOnsite = Convert.ToDouble(CanvasDataReader.xmlResult(CanvasDataReader.TOTAL_TIME_ONSITE, responsesXml));
                    retval.TotalTravelTime = Convert.ToDouble(CanvasDataReader.xmlResult(CanvasDataReader.TOTAL_TRAVEL_TIME, responsesXml));
                    retval.TotalMileage = Convert.ToDouble(CanvasDataReader.xmlResult(CanvasDataReader.TOTAL_MILEAGE, responsesXml));
                    retval.TotalDailyAllowances = Convert.ToInt32(CanvasDataReader.xmlResult(CanvasDataReader.TOTAL_DAILY_ALLOWANCES, responsesXml));
                    retval.TotalOvernightAllowances = Convert.ToInt32(CanvasDataReader.xmlResult(CanvasDataReader.TOTAL_OVERNIGHT_ALLOWANCES, responsesXml));
                    retval.TotalBarrierPayments = Convert.ToInt32(CanvasDataReader.xmlResult(CanvasDataReader.TOTAL_BARRIER_PAYMENTS, responsesXml));
                    retval.JobStatus = CanvasDataReader.xmlResult(CanvasDataReader.JOB_STATUS, responsesXml);
                    retval.FinalJobReport = CanvasDataReader.xmlResult(CanvasDataReader.FINAL_JOB_REPORT, responsesXml);
                    retval.AdditionalFaultsFound = CanvasDataReader.xmlResult(CanvasDataReader.ADDITIONAL_FAULTS_FOUND, responsesXml);
                    retval.QuoteRequired = Convert.ToBoolean(CanvasDataReader.xmlResult(CanvasDataReader.QUOTE_REQUIRED, responsesXml));
                    retval.PartsForFollowup = CanvasDataReader.xmlResult(CanvasDataReader.FOLLOWUP_PARTS, responsesXml);
                    retval.Image1Url = CanvasDataReader.xmlResult(CanvasDataReader.IMAGE_1_URL, responsesXml);
                    retval.Image2Url = CanvasDataReader.xmlResult(CanvasDataReader.IMAGE_2_URL, responsesXml);
                    retval.Image3Url = CanvasDataReader.xmlResult(CanvasDataReader.IMAGE_3_URL, responsesXml);
                    retval.Image4Url = CanvasDataReader.xmlResult(CanvasDataReader.IMAGE_4_URL, responsesXml);
                    retval.Image5Url = CanvasDataReader.xmlResult(CanvasDataReader.IMAGE_5_URL, responsesXml);
                    retval.CustomerSignatureUrl = CanvasDataReader.xmlResult(CanvasDataReader.CUSTOMER_SIGNATURE, responsesXml);
                    retval.CustomerSignName = CanvasDataReader.xmlResult(CanvasDataReader.CUSTOMER_NAME, responsesXml);
                    retval.DtSigned = Convert.ToDateTime(CanvasDataReader.xmlResult(CanvasDataReader.DATE_SIGNED, responsesXml));
                    retval.MttEngSignatureUrl = CanvasDataReader.xmlResult(CanvasDataReader.MTT_ENG_SIGNATURE, responsesXml);
                }
            }

            return retval;
        }
    }
}
