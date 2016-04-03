using System;
using System.ComponentModel;

namespace Service_Reader
{
    //Model for the service submission
    public class mServiceSubmission : INotifyPropertyChanged
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
        private int m_totalDailyAllowances = 0;
        private int m_totalOvernightAllowances = 0;
        private int m_totalBarrierPayments = 0;
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
        private ServiceDay[] m_serviceTimesheets;

        public event PropertyChangedEventHandler PropertyChanged;

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
                    notifyPropertyChanged();
                }
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
                }
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
                }
            }
        }

        public int TotalDailyAllowances
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
                    notifyPropertyChanged();
                }
            }
        }

        public int TotalOvernightAllowances
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
                    notifyPropertyChanged();
                }
            }
        }

        public int TotalBarrierPayments
        {
            get
            {
                return m_totalBarrierPayments;
            }

            set
            {
                if (value != this.m_totalBarrierPayments)
                {
                    this.TotalBarrierPayments = value;
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
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
                    notifyPropertyChanged();
                }
            }
        }

        public ServiceDay[] ServiceTimesheets
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
                    notifyPropertyChanged();
                }
            }
        }
        
        //The property changed method is called by the set method handler
        private void notifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
