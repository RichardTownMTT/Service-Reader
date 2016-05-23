using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Service_Reader
{
    //Model for the service submission
    public class ServiceSubmissionModel : ObservableObject
    {
        //Class variables
        private int m_submissionVersion = 0;
        private string m_username = "";
        private string m_userFirstName = "";
        private string m_userSurname = "";
        private int m_submissionNo = 0;
        private string m_customer = "";
        private string m_address1 = "";
        private string m_address2 = "";
        private string m_townCity = "";
        private string m_postcode = "";
        private string m_customerContact = "";
        private string m_customerPhone = "";
        private string m_machineMakeModel = "";
        private string m_machineSerial = "";
        private string m_machineController = "";
        private DateTime m_jobStart;
        private string m_customerOrderNo = "";
        private string m_mttJobNumber = "";
        private string m_jobDescription = "";
        private double m_totalTimeOnsite = 0;
        private double m_totalTravelTime = 0;
        private double m_totalMileage = 0;
        private double m_totalDailyAllowances = 0;
        private double m_totalOvernightAllowances = 0;
        private double m_totalBarrierPayments = 0;
        private string m_jobStatus = "";
        private string m_finalJobReport = "";
        private string m_additionalFaultsFound = "";
        private Boolean m_quoteRequired = false;
        private string m_partsForFollowup = "";
        private string m_image1Url = "";
        private string m_image2Url = "";
        private string m_image3Url = "";
        private string m_image4Url = "";
        private string m_image5Url = "";
        private string m_customerSignatureUrl = "";
        private string m_customerSignName = "";
        private DateTime m_dtSigned;
        private string m_mttEngSignatureUrl = "";
        private ObservableCollection<ServiceDayModel> m_serviceTimesheets;

        private Boolean m_approved = false;
        
        public int SubmissionVersion
        {
            get
            {
                return m_submissionVersion;
            }

            set
            {
                if (value != this.m_submissionVersion)
                {
                    this.m_submissionVersion = value;
                    onPropertyChanged("SubmissionVersion");
                }
            }
        }

        public void updateTimes()
        {
            if (ServiceTimesheets != null)
            {
                double totalTimeToSiteUpdate = 0;
                double totalTimeFromSiteUpdate = 0;
                double totalTimeOnsiteUpdate = 0;
                foreach (ServiceDayModel currentDay in ServiceTimesheets)
                {
                    totalTimeToSiteUpdate += currentDay.TravelTimeToSite;
                    totalTimeFromSiteUpdate += currentDay.TravelTimeFromSite;
                    totalTimeOnsiteUpdate += currentDay.TotalTimeOnsite;
                }

                double totalTravelUpdate = totalTimeToSiteUpdate + totalTimeFromSiteUpdate;
                TotalTravelTime = totalTravelUpdate;
                TotalTimeOnsite = totalTimeOnsiteUpdate;
            }
        }

        public string Username
        {
            get
            {
                return m_username;
            }

            set
            {
                if (value != this.m_username)
                {
                    this.m_username = value;
                    onPropertyChanged("Username");
                }
            }
        }

        public string UserFirstName
        {
            get
            {
                return m_userFirstName;
            }

            set
            {
                if (value != this.m_userFirstName)
                {
                    this.m_userFirstName = value;
                    onPropertyChanged("UserFirstName");
                }
            }
        }

        public string UserSurname
        {
            get
            {
                return m_userSurname;
            }

            set
            {
                if (value != this.m_userSurname)
                {
                    this.m_userSurname = value;
                    onPropertyChanged("UserSurname");
                }
            }
        }

        public string EngineerFullName
        {
            get
            {
                string retval = UserFirstName + " " + UserSurname;
                return retval;
            }
        }

        public int SubmissionNo
        {
            get
            {
                return m_submissionNo;
            }

            set
            {
                if (value != this.m_submissionNo)
                {
                    this.m_submissionNo = value;
                    onPropertyChanged("SubmissionNo");
                }
            }
        }

        public string Customer
        {
            get
            {
                return m_customer;
            }

            set
            {
                if (value != this.m_customer)
                {
                    this.m_customer = value;
                    onPropertyChanged("Customer");
                }
            }
        }

        public string Address1
        {
            get
            {
                return m_address1;
            }

            set
            {
                if (value != this.m_address1)
                {
                    this.m_address1 = value;
                    onPropertyChanged("Address1");
                }
            }
        }

        public string Address2
        {
            get
            {
                return m_address2;
            }

            set
            {
                if (value != this.m_address2)
                {
                    this.m_address2 = value;
                    onPropertyChanged("Address2");
                }
            }
        }

        public string TownCity
        {
            get
            {
                return m_townCity;
            }

            set
            {
                if (value != this.m_townCity)
                {
                    this.m_townCity = value;
                    onPropertyChanged("TownCity");
                }
            }
        }

        public string Postcode
        {
            get
            {
                return m_postcode;
            }

            set
            {
                if (value != this.m_postcode)
                {
                    this.m_postcode = value;
                    onPropertyChanged("Postcode");
                }
            }
        }

        public string CustomerContact
        {
            get
            {
                return m_customerContact;
            }

            set
            {
                if (value != this.m_customerContact)
                {
                    this.m_customerContact = value;
                    onPropertyChanged("CustomerContact");
                }
            }
        }

        public string CustomerPhone
        {
            get
            {
                return m_customerPhone;
            }

            set
            {
                if (value != this.m_customerPhone)
                {
                    this.m_customerPhone = value;
                    onPropertyChanged("CustomerPhone");
                }
            }
        }

        public string MachineMakeModel
        {
            get
            {
                return m_machineMakeModel;
            }

            set
            {
                if (value != this.m_machineMakeModel)
                {
                    this.m_machineMakeModel = value;
                    onPropertyChanged("MachineMakeModel");
                }
            }
        }

        public string MachineSerial
        {
            get
            {
                return m_machineSerial;
            }

            set
            {
                if (value != this.m_machineSerial)
                {
                    this.m_machineSerial = value;
                    onPropertyChanged("MachineSerial");
                }
            }
        }

        public string MachineController
        {
            get
            {
                return m_machineController;
            }

            set
            {
                if (value != this.m_machineController)
                {
                    this.m_machineController= value;
                    onPropertyChanged("MachineController");
                }
            }
        }

        public DateTime JobStart
        {
            get
            {
                return m_jobStart;
            }

            set
            {
                if (value != this.m_jobStart)
                {
                    this.m_jobStart = value;
                    onPropertyChanged("JobStart");
                }
            }
        }

        public string CustomerOrderNo
        {
            get
            {
                return m_customerOrderNo;
            }

            set
            {
                if (value != this.m_customerOrderNo)
                {
                    this.m_customerOrderNo = value;
                    onPropertyChanged("CustomerOrderNo");
                }
            }
        }

        public string MttJobNumber
        {
            get
            {
                return m_mttJobNumber;
            }

            set
            {
                if (value != this.m_mttJobNumber)
                {
                    this.m_mttJobNumber = value;
                    onPropertyChanged("MttJobNumber");
                }
            }
        }

        public string JobDescription
        {
            get
            {
                return m_jobDescription;
            }

            set
            {
                if (value != this.m_jobDescription)
                {
                    this.m_jobDescription = value;
                    onPropertyChanged("JobDescription");
                }
            }
        }

        public double TotalTimeOnsite
        {
            get
            {
                return m_totalTimeOnsite;
            }

            set
            {
                if (value != this.m_totalTimeOnsite)
                {
                    this.m_totalTimeOnsite = value;
                    onPropertyChanged("TotalTimeOnsite");
                }
            }
        }

        public double TotalTravelTime
        {
            get
            {
                return m_totalTravelTime;
            }

            set
            {
                if (value != this.m_totalTravelTime)
                {
                    this.m_totalTravelTime = value;
                    onPropertyChanged("TotalTravelTime");
                }
            }
        }

        public double TotalMileage
        {
            get
            {
                return m_totalMileage;
            }

            set
            {
                if (value != this.m_totalMileage)
                {
                    this.m_totalMileage = value;
                    onPropertyChanged("TotalMileage");
                }
            }
        }

        public double TotalDailyAllowances
        {
            get
            {
                return m_totalDailyAllowances;
            }

            set
            {
                if (value != this.m_totalDailyAllowances)
                {
                    this.m_totalDailyAllowances = value;
                    onPropertyChanged("TotalDailyAllowances");
                }
            }
        }

        public double TotalOvernightAllowances
        {
            get
            {
                return m_totalOvernightAllowances;
            }

            set
            {
                if (value != this.m_totalOvernightAllowances)
                {
                    this.m_totalOvernightAllowances = value;
                    onPropertyChanged("TotalOvernightAllowances");
                }
            }
        }

        public double TotalBarrierPayments
        {
            get
            {
                return m_totalBarrierPayments;
            }

            set
            {
                if (value != this.m_totalBarrierPayments)
                {
                    this.m_totalBarrierPayments = value;
                    onPropertyChanged("TotalBarrierPayments");
                }
            }
        }

        public string JobStatus
        {
            get
            {
                return m_jobStatus;
            }

            set
            {
                if (value != this.m_jobStatus)
                {
                    this.m_jobStatus = value;
                    onPropertyChanged("JobStatus");
                }
            }
        }

        public string FinalJobReport
        {
            get
            {
                return m_finalJobReport;
            }

            set
            {
                if (value != this.m_finalJobReport)
                {
                    this.m_finalJobReport = value;
                    onPropertyChanged("FinalJobReport");
                }
            }
        }

        public string AdditionalFaultsFound
        {
            get
            {
                return m_additionalFaultsFound;
            }

            set
            {
                if (value != this.m_additionalFaultsFound)
                {
                    this.m_additionalFaultsFound = value;
                    onPropertyChanged("AdditionalFaultsFound");
                }
            }
        }

        public bool QuoteRequired
        {
            get
            {
                return m_quoteRequired;
            }

            set
            {
                if (value != this.m_quoteRequired)
                {
                    this.m_quoteRequired = value;
                    onPropertyChanged("QuoteRequired");
                }
            }
        }

        public string PartsForFollowup
        {
            get
            {
                return m_partsForFollowup;
            }

            set
            {
                if (value != this.m_partsForFollowup)
                {
                    this.m_partsForFollowup = value;
                    onPropertyChanged("PartsForFollowup");
                }
            }
        }

        public string Image1Url
        {
            get
            {
                return m_image1Url;
            }

            set
            {
                if (value != this.m_image1Url)
                {
                    this.m_image1Url = value;
                    onPropertyChanged("Image1Url");
                }
            }
        }

        public string Image2Url
        {
            get
            {
                return m_image2Url;
            }

            set
            {
                if (value != this.m_image2Url)
                {
                    this.m_image2Url = value;
                    onPropertyChanged("Image2Url");
                }
            }
        }

        public string Image3Url
        {
            get
            {
                return m_image3Url;
            }

            set
            {
                if (value != this.m_image3Url)
                {
                    this.m_image3Url = value;
                    onPropertyChanged("Image3Url");
                }
            }
        }

        public string Image4Url
        {
            get
            {
                return m_image4Url;
            }

            set
            {
                if (value != this.m_image4Url)
                {
                    this.m_image4Url = value;
                    onPropertyChanged("Image4Url");
                }
            }
        }

        public string Image5Url
        {
            get
            {
                return m_image5Url;
            }

            set
            {
                if (value != this.m_image5Url)
                {
                    this.m_image5Url = value;
                    onPropertyChanged("Image5Url");
                }
            }
        }

        public string CustomerSignatureUrl
        {
            get
            {
                return m_customerSignatureUrl;
            }

            set
            {
                if (value != this.m_customerSignatureUrl)
                {
                    this.m_customerSignatureUrl = value;
                    onPropertyChanged("CustomerSignatureUrl");
                }
            }
        }

        public string CustomerSignName
        {
            get
            {
                return m_customerSignName;
            }

            set
            {
                if (value != this.m_customerSignName)
                {
                    this.m_customerSignName = value;
                    onPropertyChanged("CustomerSignName");
                }
            }
        }

        public DateTime DtSigned
        {
            get
            {
                return m_dtSigned;
            }

            set
            {
                if (value != this.m_dtSigned)
                {
                    this.m_dtSigned = value;
                    onPropertyChanged("DtSigned");
                }
            }
        }

        public string MttEngSignatureUrl
        {
            get
            {
                return m_mttEngSignatureUrl;
            }

            set
            {
                if (value != this.m_mttEngSignatureUrl)
                {
                    this.m_mttEngSignatureUrl = value;
                    onPropertyChanged("MttEngSignatureUrl");
                }
            }
        }

        public ObservableCollection<ServiceDayModel> ServiceTimesheets
        {
            get
            {
                return m_serviceTimesheets;
            }

            set
            {
                if (value != this.m_serviceTimesheets)
                {
                    this.m_serviceTimesheets = value;
                    onPropertyChanged("ServiceTimesheets");
                }
            }
        }

        public bool Approved
        {
            get
            {
                return m_approved;
            }

            set
            {
                if (m_approved != value)
                {
                    m_approved = value;
                    onPropertyChanged("Approved");
                }
            }
        }
    }
}
