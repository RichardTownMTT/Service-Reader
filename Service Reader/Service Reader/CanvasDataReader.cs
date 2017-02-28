using System.Windows;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using System.Text;
using System.Runtime.Caching;

namespace Service_Reader
{
    class CanvasDataReader
    {
        //XML Labels for fields
        public static string SUBMISSION_NUMBER = "SubmissionNumber";
        public static string SUBMISSION_VERSION = "Version";
        public static string SUBMISSION_FORM_NAME = "Name";
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

        //RT 11/8/16 - Adding in the machine serial number
        public static string SERIAL_NUMBER = "Machine serial no.";
        //Adding response id, response date time and device date time
        public static string RESPONSE_ID = "ResponseID";
        public static string RESPONSE_DATE_TIME = "Date";
        public static string DEVICE_DATE_TIME = "DeviceDate";

        public static ImageSource downloadImage(string downloadUrl, UserViewModel currentUser, bool fullUrl)
        {
            //RT 27/1/17 - Canvas string changes, depending on if the full url is provided
            string canvasUrl = "";
            if (fullUrl)
            {
                //Get everything from the last /
                int lastSlash = downloadUrl.LastIndexOf("/");
                string imageId = downloadUrl.Substring(lastSlash + 1);
                canvasUrl = "https://www.gocanvas.com/apiv2/images.xml?image_id=" + imageId + "&username=" + currentUser.UserName + "&password=" + currentUser.PasswordBoxObj.Password;
            }
            else
            {
                canvasUrl = "https://www.gocanvas.com/apiv2/images.xml?image_id=" + downloadUrl + "&username=" + currentUser.UserName + "&password=" + currentUser.PasswordBoxObj.Password;
            }
            //string canvasUrl = "https://www.gocanvas.com/apiv2/images.xml?image_id=" + downloadUrl + "&username=" + currentUser.UserName +"&password=" + currentUser.PasswordBoxObj.Password;

            ImageSource returnedImage;

            try
            {
                WebClient wc = new WebClient();
                byte[] downloadedData = wc.DownloadData(canvasUrl);

                //RT 23/11/16 - Adding Canvas standard error checks
                string errorCode = canvasErrorCode(downloadedData);
                if (!errorCode.Equals(""))
                {
                    return null;
                }
                
                MemoryStream ms = new MemoryStream(downloadedData);

                BitmapImage bmImage = new BitmapImage();
                bmImage.BeginInit();
                bmImage.StreamSource = ms;
                bmImage.EndInit();

                returnedImage = bmImage;
            }
            catch (Exception ex)
            {
                clearCacheCanvasUser();
                MessageBox.Show("Unable to download data.");
                Console.WriteLine(ex.ToString());
                return null;
            }

            //Need to check for errors
            //string errorCode = validateCanvasXml(rootElement);

            //if (!errorCode.Equals(""))
            //{
            //    return null;
            //}
            return returnedImage;
        }

        private static void clearCacheCanvasUser()
        {
            //If an error has occured, canvas login may be incorrect
            ObjectCache objCache = MemoryCache.Default;
            objCache.Remove(UserViewModel.CACHE_CANVAS_USER);
            
        }

        private static string canvasErrorCode(byte[] downloadedData)
        {
            //This method returns the canvas error code, or null if the data is correct.
            try
            {
                string response = Encoding.ASCII.GetString(downloadedData);
                XDocument xDoc = XDocument.Parse(response);
                return validateCanvasXml(xDoc.Root);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //Unable to parse canvas error, so return null.
                return "";
            }
        }

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
        //private static string RESPONSE_GROUP = "ResponseGroup";

        public static string RESPONSE_GROUPS = "ResponseGroups";
        public static string JOB_DETAILS = "Job details";
        public static string TIME_SHEET = "Time Sheet";
        public static string JOB_SIGNOFF = "Job Signoff";

        //RT 12/10/16 - Rewriting to use new classes of servicesheet, serviceDay
        //public static List<oldServiceSubmissionModel> downloadXml(string canvasUsername, string canvasPassword, string beginDate, string endDate)
        //RT 26/11/16 - Changing to use canvasUser
        //public static ObservableCollection<ServiceSheetViewModel> downloadXml(string canvasUsername, string canvasPassword, DateTime beginDate, DateTime endDate)
        public static ObservableCollection<ServiceSheetViewModel> downloadXml(UserViewModel canvasUserVM, DateTime beginDate, DateTime endDate)
        {
            //RT 13/10/16 - Date must be in the USA format
            string startDateStr = beginDate.ToString("MM/dd/yyyy", null);
            string endDateStr = endDate.ToString("MM/dd/yyyy", null);
            string canvasUrl = "https://www.gocanvas.com/apiv2/submissions.xml?username=" + canvasUserVM.UserName + "&password=" + canvasUserVM.PasswordBoxObj.Password + "&form_id=1285373&begin_date=" + startDateStr + "&end_date=" + endDateStr;

            XElement rootElement;

            rootElement = loadElements(canvasUrl);
            if (rootElement == null)
            {
                //Failed validation or download failed.  Error will have been shown
                return null;
            }

            XElement totalPagesNode = rootElement.Element("TotalPages");
            int noOfPages = Convert.ToInt32(totalPagesNode.Value);

            //RT 13/10/16 - Adding support for multiple pages from Canvas
            int pageCounter = 1;

            List<ServiceSheetViewModel> allSubmissions = new List<ServiceSheetViewModel>();

            while (pageCounter <= noOfPages)
            {
                if (pageCounter == 1)
                {
                    XElement submissions = rootElement.Element("Submissions");

                    //ServiceSubmissionModel[] allSubmissions;
                    allSubmissions.AddRange(parseSubmissions(submissions));
                }
                else
                {
                    canvasUrl = "https://www.gocanvas.com/apiv2/submissions.xml?username=" + canvasUserVM.UserName + "&password=" + canvasUserVM.PasswordBoxObj.Password + "&form_id=1285373&begin_date=" + startDateStr + "&end_date=" + endDateStr + "&page=" +pageCounter;

                    rootElement = loadElements(canvasUrl);
                    if (rootElement == null)
                    {
                        //Failed validation or download failed.  Error will have been shown
                        return null;
                    }

                    XElement submissions = rootElement.Element("Submissions");

                    //ServiceSubmissionModel[] allSubmissions;
                    allSubmissions.AddRange(parseSubmissions(submissions));
                }
                pageCounter++;
            }

            ObservableCollection<ServiceSheetViewModel> retval = new ObservableCollection<ServiceSheetViewModel>(allSubmissions);
            return retval;
        }

        private static XElement loadElements(string canvasUrl)
        {
            XElement rootElement;
            try
            {
                XDocument xDoc = XDocument.Load(canvasUrl);
                rootElement = xDoc.Root;
            }
            catch
            {
                clearCacheCanvasUser();
                MessageBox.Show("Unable to download data.");
                return null;
            }

            //Need to check for errors
            string errorCode = validateCanvasXml(rootElement);

            if (!errorCode.Equals(""))
            {
                clearCacheCanvasUser();
                return null;
            }
            return rootElement;
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

        public static UserViewModel getCanvasUser()
        {
            ObjectCache objCache = MemoryCache.Default;
            if (objCache.Contains(UserViewModel.CACHE_CANVAS_USER))
            {
                return (UserViewModel)objCache.Get(UserViewModel.CACHE_CANVAS_USER);
            }

            UserViewModel canvasUserVM = new UserViewModel(UserViewModel.MODE_CANVAS);
            UserView userView = new UserView();
            userView.DataContext = canvasUserVM;
            bool? userResult = userView.ShowDialog();

            //RT - The box may have been cancelled
            if (userResult != true)
            {
                return null;
            }

            //Add the item to the cache.  We don't know if the username and password are correct.
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5);

            objCache.Add(UserViewModel.CACHE_CANVAS_USER, canvasUserVM, policy);

            return canvasUserVM;
        }

        //private static ServiceSubmissionModel[] parseSubmissions(XElement submissions)
        //RT 12/10/16 - Rewriting to use new EF models
        //private static List<oldServiceSubmissionModel> parseSubmissions(XElement submissions)
        private static List<ServiceSheetViewModel> parseSubmissions(XElement submissions)
        {
            //int noOfSubmissions = 0;
            //noOfSubmissions = submissions.Descendants("Submission").Count();

            List<ServiceSheetViewModel> allSubmissions = new List<ServiceSheetViewModel>();
            //int submissionCounter = 0;

            foreach (XElement submissionXml in submissions.Elements())
            {
                //Load the submission
                ServiceSheetViewModel currentSubmission = createSubmissionForXml(submissionXml);

                allSubmissions.Add(currentSubmission);

                //submissionCounter++;
            }
            return allSubmissions;
        }

        //public static oldServiceSubmissionModel createSubmissionForXml(XElement submissionXml)
        //RT  - 29/11/16 - Rewriting to use the VM
        public static ServiceSheetViewModel createSubmissionForXml(XElement submissionXml)
        {
            //ServiceSheetViewModel retval = new ServiceSheetViewModel();
            //ServiceSheet serviceSheetReturn = new ServiceSheet();
            int submissionNumberEntered = Int32.Parse(submissionXml.Element(SUBMISSION_NUMBER).Value);
            //RT 11/8/16 - Adding in the response id
            string canvasResponseIdEntered = submissionXml.Element(RESPONSE_ID).Value;
            //Adding response date time
            string responseDateStr = submissionXml.Element(RESPONSE_DATE_TIME).Value;
            DateTime dtResponseEntered = Convert.ToDateTime(responseDateStr);
            //Adding device date
            string deviceDateStr = submissionXml.Element(DEVICE_DATE_TIME).Value;
            DateTime dtDeviceEntered = Convert.ToDateTime(deviceDateStr);
            //Submission version is in the form element
            XElement formDetailsXml = submissionXml.Element(FORM);
            //RT 23/1/17 - Adding app and form names
            //The app name isn't contained in the XML, but won't change.
            string submissionAppName = "Service Sheet";
            string submissionFormName = formDetailsXml.Element(SUBMISSION_FORM_NAME).Value;
            int submissionFormVersionEntered = Int32.Parse(formDetailsXml.Element(SUBMISSION_VERSION).Value);
            string usernameEntered = submissionXml.Element(USERNAME).Value;
            string userFirstNameEntered = submissionXml.Element(FIRST_NAME).Value;
            string userSurnameEntered = submissionXml.Element(SURNAME).Value;

            string customerEntered = "";
            string addressLine1Entered = "";
            string addressLine2Entered = "";
            string townCityEntered = "";
            string postcodeEntered = "";
            string customerContactEntered = "";
            string customerPhoneNoEntered = "";
            string machineMakeModelEntered = "";
            string machineSerialEntered = "";
            string cncControlEntered = "";
            string jobStartStr = "";
            DateTime dtJobStartEntered = new DateTime();
            string customerOrderNoEntered = "";
            string mttJobNumberEntered = "";
            string jobDescriptionEntered = "";
            double jobTotalTimeOnsiteEntered;
            double jobTotalTravelTimeEntered;
            int mileageEntered;
            int totalDaEntered;
            int totalOaEntered;
            int totalBpEntered;
            string jobStatusEntered = "";
            string finalJobReportEntered = "";
            string additionalFaultsEntered = "";
            bool quoteRequiredEntered;
            string followUpPartsRequiredEntered = "";
            string image1UrlEntered = "";
            string image2UrlEntered = "";
            string image3UrlEntered = "";
            string image4UrlEntered = "";
            string image5UrlEntered = "";
            string customerSignatureUrlEntered = "";
            string customerNameEntered = "";
            DateTime dtSignedEntered;
            string mttEngSignatureUrlEntered = "";
            List<ServiceDayViewModel> dayVMs = null;

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

                    customerEntered = xmlResult(CUSTOMER, responsesXml);
                    addressLine1Entered = xmlResult(ADDRESS_1, responsesXml);
                    addressLine2Entered = xmlResult(ADDRESS_2, responsesXml);
                    townCityEntered = xmlResult(TOWN_CITY, responsesXml);
                    postcodeEntered = xmlResult(POSTCODE, responsesXml);
                    customerContactEntered = xmlResult(CUSTOMER_CONTACT, responsesXml);
                    customerPhoneNoEntered = xmlResult(CUSTOMER_PHONE, responsesXml);
                    machineMakeModelEntered = xmlResult(MACHINE_MAKE_MODEL, responsesXml);
                    machineSerialEntered = xmlResult(SERIAL_NUMBER, responsesXml);

                    cncControlEntered = xmlResult(MACHINE_CONTROL, responsesXml);
                    jobStartStr = xmlResult(JOB_START_DATE, responsesXml);
                    dtJobStartEntered = Convert.ToDateTime(jobStartStr);
                    customerOrderNoEntered = xmlResult(CUSTOMER_ORDER, responsesXml);
                    mttJobNumberEntered = xmlResult(MTT_JOB_NO, responsesXml);
                    jobDescriptionEntered = xmlResult(JOB_DESC, responsesXml);

                }
                else if (sectionName.Equals(TIME_SHEET))
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responseGroupsXml = screenXml.Element(RESPONSE_GROUPS);
                    //retval.ServiceTimesheets = ServiceDay.createDays(responseGroupsXml);
                    dayVMs = createDays(responseGroupsXml);
                }
                else if (sectionName.Equals(JOB_SIGNOFF))
                {
                    XElement screensXml = sectionXml.Element(SCREENS);
                    XElement screenXml = screensXml.Element(SCREEN);
                    XElement responsesXml = screenXml.Element(RESPONSES);

                    jobTotalTimeOnsiteEntered = Convert.ToDouble(xmlResult(JOB_TOTAL_TIME_ONSITE, responsesXml));
                    jobTotalTravelTimeEntered = Convert.ToDouble(xmlResult(TOTAL_TRAVEL_TIME, responsesXml));

                    //RT 13/10/16 - Mileage, daily/overnight and BP are int, although canvas returns a string like a double.  There will never be a decimal point in them
                    double mileageDouble = Convert.ToDouble(xmlResult(TOTAL_MILEAGE, responsesXml));
                    mileageEntered = (int)mileageDouble;
                    double totalDaDouble = Convert.ToDouble(xmlResult(TOTAL_DAILY_ALLOWANCES, responsesXml));
                    totalDaEntered = (int)totalDaDouble;
                    double totalOaDouble = Convert.ToDouble(xmlResult(TOTAL_OVERNIGHT_ALLOWANCES, responsesXml));
                    totalOaEntered = (int)totalOaDouble;
                    double totalBpDouble = Convert.ToDouble(xmlResult(TOTAL_BARRIER_PAYMENTS, responsesXml));
                    totalBpEntered = (int)totalBpDouble;
                    jobStatusEntered = xmlResult(JOB_STATUS, responsesXml);
                    finalJobReportEntered = xmlResult(FINAL_JOB_REPORT, responsesXml);
                    additionalFaultsEntered = xmlResult(ADDITIONAL_FAULTS_FOUND, responsesXml);
                    quoteRequiredEntered = Convert.ToBoolean(xmlResult(QUOTE_REQUIRED, responsesXml));
                    followUpPartsRequiredEntered = xmlResult(FOLLOWUP_PARTS, responsesXml);
                    image1UrlEntered = xmlResult(IMAGE_1_URL, responsesXml);
                    image2UrlEntered = xmlResult(IMAGE_2_URL, responsesXml);
                    image3UrlEntered = xmlResult(IMAGE_3_URL, responsesXml);
                    image4UrlEntered = xmlResult(IMAGE_4_URL, responsesXml);
                    image5UrlEntered = xmlResult(IMAGE_5_URL, responsesXml);
                    customerSignatureUrlEntered = xmlResult(CUSTOMER_SIGNATURE, responsesXml);
                    customerNameEntered = xmlResult(CUSTOMER_NAME, responsesXml);
                    dtSignedEntered = Convert.ToDateTime(xmlResult(DATE_SIGNED, responsesXml));
                    mttEngSignatureUrlEntered = xmlResult(MTT_ENG_SIGNATURE, responsesXml);

                    //Now we create the Service Sheet VM
                    ServiceSheetViewModel retval = new ServiceSheetViewModel(submissionNumberEntered, submissionAppName, userFirstNameEntered, userSurnameEntered, canvasResponseIdEntered, dtResponseEntered,
                        dtDeviceEntered, submissionFormName, submissionFormVersionEntered, customerEntered, addressLine1Entered, addressLine2Entered, townCityEntered, postcodeEntered, customerContactEntered,
                        customerPhoneNoEntered, machineMakeModelEntered, machineSerialEntered, cncControlEntered, dtJobStartEntered, customerOrderNoEntered, mttJobNumberEntered, jobDescriptionEntered,
                        jobTotalTimeOnsiteEntered, jobTotalTravelTimeEntered, mileageEntered, totalDaEntered, totalOaEntered, totalBpEntered, jobStatusEntered, finalJobReportEntered, additionalFaultsEntered,
                        quoteRequiredEntered, followUpPartsRequiredEntered, image1UrlEntered, image2UrlEntered, image3UrlEntered, image4UrlEntered, image5UrlEntered, customerSignatureUrlEntered, customerNameEntered, dtSignedEntered,
                        mttEngSignatureUrlEntered, dayVMs, usernameEntered);
                    retval.SaveToModel();

                    return retval;
                }
                else
                {
                    new Exception("Unknown Canvas Data Section");
                }

            }

            //If we get to here, then an error has occurred.
            return null;
            //retval.ServiceSubmission = serviceSheetReturn;
            //return retval;
        }
        //public static ServiceSheetViewModel createSubmissionForXml(XElement submissionXml)
        //{
        //    ServiceSheetViewModel retval = new ServiceSheetViewModel();
        //    ServiceSheet serviceSheetReturn = new ServiceSheet();
        //    serviceSheetReturn.SubmissionNumber = Int32.Parse(submissionXml.Element(SUBMISSION_NUMBER).Value);
        //    //RT 11/8/16 - Adding in the response id
        //    serviceSheetReturn.CanvasResponseId = submissionXml.Element(RESPONSE_ID).Value;
        //    //Adding response date time
        //    string responseDateStr = submissionXml.Element(RESPONSE_DATE_TIME).Value;
        //    serviceSheetReturn.DtResponse = Convert.ToDateTime(responseDateStr);
        //    //Adding device date
        //    string deviceDateStr = submissionXml.Element(DEVICE_DATE_TIME).Value;
        //    serviceSheetReturn.DtDevice = Convert.ToDateTime(deviceDateStr);
        //    //Submission version is in the form element
        //    XElement formDetailsXml = submissionXml.Element(FORM);
        //    serviceSheetReturn.SubmissionFormVersion = Int32.Parse(formDetailsXml.Element(SUBMISSION_VERSION).Value);
        //    serviceSheetReturn.Username = submissionXml.Element(USERNAME).Value;
        //    serviceSheetReturn.UserFirstName = submissionXml.Element(FIRST_NAME).Value;
        //    serviceSheetReturn.UserSurname = submissionXml.Element(SURNAME).Value;

        //    XElement sectionsXml = submissionXml.Element(SECTIONS);
        //    // Loop through the sections
        //    foreach (XElement sectionXml in sectionsXml.Elements())
        //    {
        //        string sectionName = sectionXml.Element(SECTION_NAME).Value;
        //        if (sectionName.Equals(JOB_DETAILS))
        //        {
        //            XElement screensXml = sectionXml.Element(SCREENS);
        //            XElement screenXml = screensXml.Element(SCREEN);
        //            XElement responsesXml = screenXml.Element(RESPONSES);
        //            //Parse the job details

        //            serviceSheetReturn.Customer = xmlResult(CUSTOMER, responsesXml);
        //            serviceSheetReturn.AddressLine1 = xmlResult(ADDRESS_1, responsesXml);
        //            serviceSheetReturn.AddressLine2 = xmlResult(ADDRESS_2, responsesXml);
        //            serviceSheetReturn.TownCity = xmlResult(TOWN_CITY, responsesXml);
        //            serviceSheetReturn.Postcode = xmlResult(POSTCODE, responsesXml);
        //            serviceSheetReturn.CustomerContact = xmlResult(CUSTOMER_CONTACT, responsesXml);
        //            serviceSheetReturn.CustomerPhoneNo = xmlResult(CUSTOMER_PHONE, responsesXml);
        //            serviceSheetReturn.MachineMakeModel = xmlResult(MACHINE_MAKE_MODEL, responsesXml);
        //            //RT 11/8/16 - Adding serial number
        //            serviceSheetReturn.MachineSerial = xmlResult(SERIAL_NUMBER, responsesXml);

        //            serviceSheetReturn.CncControl = xmlResult(MACHINE_CONTROL, responsesXml);
        //            string jobStartStr = xmlResult(JOB_START_DATE, responsesXml);
        //            serviceSheetReturn.DtJobStart = Convert.ToDateTime(jobStartStr);
        //            serviceSheetReturn.CustomerOrderNo = xmlResult(CUSTOMER_ORDER, responsesXml);
        //            serviceSheetReturn.MttJobNumber = xmlResult(MTT_JOB_NO, responsesXml);
        //            serviceSheetReturn.JobDescription = xmlResult(JOB_DESC, responsesXml);

        //        }
        //        else if (sectionName.Equals(TIME_SHEET))
        //        {
        //            XElement screensXml = sectionXml.Element(SCREENS);
        //            XElement screenXml = screensXml.Element(SCREEN);
        //            XElement responseGroupsXml = screenXml.Element(RESPONSE_GROUPS);
        //            //retval.ServiceTimesheets = ServiceDay.createDays(responseGroupsXml);
        //            serviceSheetReturn.ServiceDays= createDays(responseGroupsXml);
        //        }
        //        else if (sectionName.Equals(JOB_SIGNOFF))
        //        {
        //            XElement screensXml = sectionXml.Element(SCREENS);
        //            XElement screenXml = screensXml.Element(SCREEN);
        //            XElement responsesXml = screenXml.Element(RESPONSES);

        //            serviceSheetReturn.JobTotalTimeOnsite = Convert.ToDouble(xmlResult(JOB_TOTAL_TIME_ONSITE, responsesXml));
        //            serviceSheetReturn.JobTotalTravelTime = Convert.ToDouble(xmlResult(TOTAL_TRAVEL_TIME, responsesXml));

        //            //RT 13/10/16 - Mileage, daily/overnight and BP are int, although canvas returns a string like a double.  There will never be a decimal point in them
        //            double mileageRead = Convert.ToDouble(xmlResult(TOTAL_MILEAGE, responsesXml));
        //            serviceSheetReturn.JobTotalMileage = (int)mileageRead;
        //            double totalDaRead = Convert.ToDouble(xmlResult(TOTAL_DAILY_ALLOWANCES, responsesXml));
        //            serviceSheetReturn.TotalDailyAllowances = (int)totalDaRead;
        //            double totalOaRead = Convert.ToDouble(xmlResult(TOTAL_OVERNIGHT_ALLOWANCES, responsesXml));
        //            serviceSheetReturn.TotalOvernightAllowances = (int)totalOaRead;
        //            double totalBpRead = Convert.ToDouble(xmlResult(TOTAL_BARRIER_PAYMENTS, responsesXml));
        //            serviceSheetReturn.TotalBarrierPayments = (int)totalBpRead;
        //            serviceSheetReturn.JobStatus = xmlResult(JOB_STATUS, responsesXml);
        //            serviceSheetReturn.FinalJobReport = xmlResult(FINAL_JOB_REPORT, responsesXml);
        //            serviceSheetReturn.AdditionalFaults = xmlResult(ADDITIONAL_FAULTS_FOUND, responsesXml);
        //            serviceSheetReturn.QuoteRequired = Convert.ToBoolean(xmlResult(QUOTE_REQUIRED, responsesXml));
        //            serviceSheetReturn.FollowUpPartsRequired = xmlResult(FOLLOWUP_PARTS, responsesXml);
        //            serviceSheetReturn.Image1Url = xmlResult(IMAGE_1_URL, responsesXml);
        //            serviceSheetReturn.Image2Url = xmlResult(IMAGE_2_URL, responsesXml);
        //            serviceSheetReturn.Image3Url = xmlResult(IMAGE_3_URL, responsesXml);
        //            serviceSheetReturn.Image4Url = xmlResult(IMAGE_4_URL, responsesXml);
        //            serviceSheetReturn.Image5Url = xmlResult(IMAGE_5_URL, responsesXml);
        //            serviceSheetReturn.CustomerSignatureUrl = xmlResult(CUSTOMER_SIGNATURE, responsesXml);
        //            serviceSheetReturn.CustomerName = xmlResult(CUSTOMER_NAME, responsesXml);
        //            serviceSheetReturn.DtSigned = Convert.ToDateTime(xmlResult(DATE_SIGNED, responsesXml));
        //            serviceSheetReturn.MttEngSignatureUrl = xmlResult(MTT_ENG_SIGNATURE, responsesXml);
        //        }
        //        else
        //        {
        //            new Exception("Unknown Canvas Data Section");
        //        }

        //    }
        //    retval.ServiceSubmission = serviceSheetReturn;
        //    return retval;
        //}

        //RT 29/11/16 - Rewiting to use new MVVM methods
        public static List<ServiceDayViewModel> createDays(XElement allDays)
        {
            List<ServiceDayViewModel> retval = new List<ServiceDayViewModel>();

            foreach (XElement responseGroupXml in allDays.Elements())
            {
                string dtServiceStr = xmlResult(DATE, responseGroupXml);
                DateTime dtReport = Convert.ToDateTime(dtServiceStr);
                XElement sectionXml = responseGroupXml.Element(SECTION);
                XElement screensXml = sectionXml.Element(SCREENS);
                XElement screenXml = screensXml.Element(SCREEN);
                XElement responsesXml = screenXml.Element(RESPONSES);

                string travelStartStr = xmlResult(TRAVEL_START, responsesXml);
                DateTime TravelStartTime = Convert.ToDateTime(dtServiceStr + " " + travelStartStr);
                string arrivalOnsiteStr = xmlResult(ARRIVE_ONSITE, responsesXml);
                DateTime ArrivalOnsiteTime = Convert.ToDateTime(dtServiceStr + " " + arrivalOnsiteStr);
                string departSiteStr = xmlResult(DEPART_SITE, responsesXml);
                DateTime DepartureSiteTime = Convert.ToDateTime(dtServiceStr + " " + departSiteStr);
                string travelEndStr = xmlResult(TRAVEL_END, responsesXml);
                DateTime TravelEndTime = Convert.ToDateTime(dtServiceStr + " " + travelEndStr);
                int Mileage = Convert.ToInt32(xmlResult(MILEAGE, responsesXml));
                string currentXmlResult = xmlResult(DAILY_ALLOWANCE, responsesXml);
                int DailyAllowance = Convert.ToInt32(currentXmlResult);
                bool da;
                if (DailyAllowance == 1)
                {
                    da = true;
                }
                else
                {
                    da = false;
                }
                currentXmlResult = xmlResult(OVERNIGHT_ALLOWANCE, responsesXml);
                int OvernightAllowance = Convert.ToInt32(currentXmlResult);
                bool oN;
                if (OvernightAllowance == 1)
                {
                    oN = true;
                }
                else
                {
                    oN = false;
                }
                currentXmlResult = xmlResult(BARRIER_PAYMENT, responsesXml);
                int BarrierPayment = Convert.ToInt32(currentXmlResult);
                bool bP;
                if (BarrierPayment == 1)
                {
                    bP = true;
                }
                else
                {
                    bP = false;
                }
                double TravelToSiteTime = Convert.ToDouble(xmlResult(TRAVEL_TO_SITE, responsesXml));
                double TravelFromSiteTime = Convert.ToDouble(xmlResult(TRAVEL_FROM_SITE, responsesXml));
                double TotalTravelTime = Convert.ToDouble(xmlResult(TOTAL_TRAVEL, responsesXml));
                double TotalOnsiteTime = Convert.ToDouble(xmlResult(TOTAL_TIME_ONSITE, responsesXml));
                string DailyReport = xmlResult(DAILY_REPORT, responsesXml);
                string PartsSuppliedToday = xmlResult(PARTS_SUPPLIED, responsesXml);

                ServiceDayViewModel sd = new ServiceDayViewModel(TravelStartTime, ArrivalOnsiteTime, DepartureSiteTime, TravelEndTime, Mileage, da, oN,
                    bP, TravelToSiteTime, TravelFromSiteTime, TotalTravelTime, TotalOnsiteTime, DailyReport, PartsSuppliedToday, dtReport, null);

                retval.Add(sd);
            }

            //Need to sort the observable collection
            List<ServiceDayViewModel> sortedDays = new List<ServiceDayViewModel>(retval.OrderBy(a => a.DtReport));
            retval = sortedDays;

            return retval;
        }
        //public static ICollection<ServiceDay> createDays(XElement allDays)
        //{
        //    int totalDays;
        //    totalDays = allDays.Descendants(RESPONSE_GROUP).Count();
        //    List<ServiceDay> retval = new List<ServiceDay>();

        //    foreach (XElement responseGroupXml in allDays.Elements())
        //    {
        //        ServiceDay dayOfService = new ServiceDay();
        //        string dtServiceStr = xmlResult(DATE, responseGroupXml);
        //        dayOfService.DtReport = Convert.ToDateTime(dtServiceStr);
        //        XElement sectionXml = responseGroupXml.Element(SECTION);
        //        XElement screensXml = sectionXml.Element(SCREENS);
        //        XElement screenXml = screensXml.Element(SCREEN);
        //        XElement responsesXml = screenXml.Element(RESPONSES);

        //        string travelStartStr = xmlResult(TRAVEL_START, responsesXml);
        //        dayOfService.TravelStartTime = Convert.ToDateTime(dtServiceStr + " " + travelStartStr);
        //        string arrivalOnsiteStr = xmlResult(ARRIVE_ONSITE, responsesXml);
        //        dayOfService.ArrivalOnsiteTime = Convert.ToDateTime(dtServiceStr + " " + arrivalOnsiteStr);
        //        string departSiteStr = xmlResult(DEPART_SITE, responsesXml);
        //        dayOfService.DepartureSiteTime = Convert.ToDateTime(dtServiceStr + " " + departSiteStr);
        //        string travelEndStr = xmlResult(TRAVEL_END, responsesXml);
        //        dayOfService.TravelEndTime = Convert.ToDateTime(dtServiceStr + " " + travelEndStr);
        //        dayOfService.Mileage = Convert.ToInt32(xmlResult(MILEAGE, responsesXml));
        //        string currentXmlResult = xmlResult(DAILY_ALLOWANCE, responsesXml);
        //        dayOfService.DailyAllowance = Convert.ToInt32(currentXmlResult);
        //        currentXmlResult = xmlResult(OVERNIGHT_ALLOWANCE, responsesXml);
        //        dayOfService.OvernightAllowance = Convert.ToInt32(currentXmlResult);
        //        currentXmlResult = xmlResult(BARRIER_PAYMENT, responsesXml);
        //        dayOfService.BarrierPayment = Convert.ToInt32(currentXmlResult);
        //        dayOfService.TravelToSiteTime = Convert.ToDouble(xmlResult(TRAVEL_TO_SITE, responsesXml));
        //        dayOfService.TravelFromSiteTime = Convert.ToDouble(xmlResult(TRAVEL_FROM_SITE, responsesXml));
        //        dayOfService.TotalTravelTime = Convert.ToDouble(xmlResult(TOTAL_TRAVEL, responsesXml));
        //        dayOfService.TotalOnsiteTime = Convert.ToDouble(xmlResult(TOTAL_TIME_ONSITE, responsesXml));
        //        dayOfService.DailyReport = xmlResult(DAILY_REPORT, responsesXml);
        //        dayOfService.PartsSuppliedToday = xmlResult(PARTS_SUPPLIED, responsesXml);

        //        //Adding a reference to the current submission
        //        //dayOfService.CurrentServiceSubmission = currentSubmission;
        //        retval.Add(dayOfService);
        //    }

        //    //Need to sort the observable collection

        //    List<ServiceDay> retvalSorted = new List<ServiceDay>(retval.OrderBy(a => a.DtReport));

        //    return retvalSorted;
        //}

        //private static bool convertIntToBoolean(string currentXmlResult)
        //{
        //    if (currentXmlResult.Equals("0"))
        //    {
        //        return false;
        //    }
        //    else if(currentXmlResult.Equals("1"))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        new Exception("Unhandled data type");
        //        return false;
        //    }
        //}

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
