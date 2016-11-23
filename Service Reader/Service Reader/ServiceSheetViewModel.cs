using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Service_Reader 
{
    public class ServiceSheetViewModel : ObservableObject
    {
        private ServiceSheet m_serviceSubmission;
        //This isn't saved, just used to check that the submission has been read.
        private bool m_officeApproval = false;
        //These are the images, which are cached on the VM.  Downloaded from the Canvas Site
        private ImageSource m_mttEngineerSignature;
        private ImageSource m_customerSignature;
        private ImageSource m_image1;
        private ImageSource m_image2;
        private ImageSource m_image3;
        private ImageSource m_image4;
        private ImageSource m_image5;

        public ServiceSheet ServiceSubmission
        {
            get
            {
                return m_serviceSubmission;
            }

            set
            {
                m_serviceSubmission = value;
                onPropertyChanged("ServiceSubmission");
            }
        }

        //RT 18/11/16 - This is only used for display purposes - it will not be changed
        public string EngineerFullName
        {
            get
            {
                return ServiceSubmission.UserFirstName + " " + ServiceSubmission.UserSurname;
            }
        }
        

        public int SubmissionNumber
        {
            get
            {
                return ServiceSubmission.SubmissionNumber;
            }
        }

        public string Customer
        {
            get
            {
                return ServiceSubmission.Customer;
            }
            set
            {
                ServiceSubmission.Customer = value;
                onPropertyChanged("Customer");
            }
        }

        public string AddressLine1
        {
            get
            {
                return ServiceSubmission.AddressLine1;
            }
            set
            {
                ServiceSubmission.AddressLine1 = value;
                onPropertyChanged("AddressLine1");
            }
        }

        internal void recalculateTravelTime()
        {
            double travelTime = 0;
            foreach(ServiceDayViewModel day in AllServiceDayVMs)
            {
                travelTime += day.TotalTravelTime;
            }
            TotalTravelTime = travelTime;
        }

        public string AddressLine2
        {
            get
            {
                return ServiceSubmission.AddressLine2;
            }
            set
            {
                ServiceSubmission.AddressLine2 = value;
                onPropertyChanged("AddressLine2");
            }
        }

        public string TownCity
        {
            get
            {
                return ServiceSubmission.TownCity;
            }
            set
            {
                ServiceSubmission.TownCity = value;
                onPropertyChanged("TownCity");
            }
        }

        internal void recalulateTimeOnsite()
        {
            double timeOnsite = 0;
            foreach (ServiceDayViewModel serviceDay in AllServiceDayVMs)
            {
                timeOnsite += serviceDay.TotalTimeOnsite;
            }
            TotalTimeOnsite = timeOnsite;
        }

        public string Postcode
        {
            get
            {
                return ServiceSubmission.Postcode;
            }
            set
            {
                ServiceSubmission.Postcode = value;
                onPropertyChanged("Postcode");
            }
        }

        public string CustomerContact
        {
            get
            {
                return ServiceSubmission.CustomerContact;
            }
            set
            {
                ServiceSubmission.CustomerContact = value;
                onPropertyChanged("CustomerContact");
            }
        }

        public string CustomerPhone
        {
            get
            {
                return ServiceSubmission.CustomerPhoneNo;
            }
            set
            {
                ServiceSubmission.CustomerPhoneNo = value;
                onPropertyChanged("CustomerPhone");
            }
        }

        internal void recalculateMileage()
        {
            int updatedMileage = 0;
            foreach(ServiceDayViewModel day in AllServiceDayVMs)
            {
                updatedMileage += day.Mileage;
            }
            TotalMileage = updatedMileage;
        }

        public DateTime JobStartDate
        {
            get
            {
                return ServiceSubmission.DtJobStart;
            }
            set
            {
                ServiceSubmission.DtJobStart = value;
                onPropertyChanged("JobStartDate");
            }
        }

        internal void recalculateDailyAllowances()
        {
            int updatedDA = 0;
            foreach (ServiceDayViewModel day in AllServiceDayVMs)
            {
                if (day.DailyAllowance)
                {
                    updatedDA++;
                }
            }
            TotalDailyAllowances = updatedDA;
        }

        public string MachineMakeModel
        {
            get
            {
                return ServiceSubmission.MachineMakeModel;
            }
            set
            {
                ServiceSubmission.MachineMakeModel = value;
                onPropertyChanged("MachineMakeModel");
            }
        }

        internal void recalculateOvernightAllowances()
        {
            int updatedOA = 0;
            foreach(ServiceDayViewModel day in AllServiceDayVMs)
            {
                if (day.OvernightAllowance)
                {
                    updatedOA++;
                }
            }
            TotalOvernightAllowances = updatedOA;
        }

        public string MachineSerialNo
        {
            get
            {
                return ServiceSubmission.MachineSerial;
            }
            set
            {
                ServiceSubmission.MachineSerial = value;
                onPropertyChanged("MachineSerialNo");
            }
        }

        internal void recalculateBarrierPayments()
        {
            int updatedBP = 0;
            foreach(ServiceDayViewModel day in AllServiceDayVMs)
            {
                if(day.BarrierPayment)
                {
                    updatedBP++;
                }
            }
            TotalBarrierPayments = updatedBP;
        }

        public string MachineController
        {
            get
            {
                return ServiceSubmission.CncControl;
            }
            set
            {
                ServiceSubmission.CncControl = value;
                onPropertyChanged("MachineController");
            }
        }

        public string MttJobNo
        {
            get
            {
                return ServiceSubmission.MttJobNumber;
            }
            set
            {
                ServiceSubmission.MttJobNumber = value;
                onPropertyChanged("MttJobNo");
            }
        }

        public string CustomerOrderNo
        {
            get
            {
                return ServiceSubmission.CustomerOrderNo;
            }
            set
            {
                ServiceSubmission.CustomerOrderNo = value;
                onPropertyChanged("CustomerOrderNo");
            }
        }

        public string JobDescription
        {
            get
            {
                return ServiceSubmission.JobDescription;
            }
            set
            {
                ServiceSubmission.JobDescription = value;
                onPropertyChanged("JobDescription");
            }
        }

        public ObservableCollection<ServiceDayViewModel> AllServiceDayVMs
        {
            get
            {
                ObservableCollection<ServiceDayViewModel> retval = new ObservableCollection<ServiceDayViewModel>();
                foreach (ServiceDay sd in ServiceSubmission.ServiceDays)
                {
                    ServiceDayViewModel serviceDay = new ServiceDayViewModel(sd, this);
                    retval.Add(serviceDay);
                }

                return retval;
            }
        }
        
        public string JobStatus
        {
            get
            {
                return ServiceSubmission.JobStatus;
            }
            set
            {
                ServiceSubmission.JobStatus = value;
                onPropertyChanged("JobStatus");
            }
        }

        public string FinalJobReport
        {
            get
            {
                return ServiceSubmission.FinalJobReport;
            }
            set
            {
                ServiceSubmission.FinalJobReport = value;
                onPropertyChanged("FinalJobReport");
            }
        }

        public string AdditionalFaultsFound
        {
            get
            {
                return ServiceSubmission.AdditionalFaults;
            }
            set
            {
                ServiceSubmission.AdditionalFaults = value;
                onPropertyChanged("AdditionalFaultsFound");
            }
        }

        public bool QuoteRequired
        {
            get
            {
                return ServiceSubmission.QuoteRequired;
            }
            set
            {
                ServiceSubmission.QuoteRequired = value;
                onPropertyChanged("QuoteRequired");
            }
        }

        public string PartsForFollowup
        {
            get
            {
                return ServiceSubmission.FollowUpPartsRequired;
            }
            set
            {
                ServiceSubmission.FollowUpPartsRequired = value;
                onPropertyChanged("PartsForFollowup");
            }
        }

        public double TotalTimeOnsite
        {
            get
            {
                return ServiceSubmission.JobTotalTimeOnsite;
            }
            set
            {
                ServiceSubmission.JobTotalTimeOnsite = value;
                onPropertyChanged("TotalTimeOnsite");
            }
        }

        public double TotalTravelTime
        {
            get
            {
                return ServiceSubmission.JobTotalTravelTime;
            }
            set
            {
                ServiceSubmission.JobTotalTravelTime = value;
                onPropertyChanged("TotalTravelTime");
            }
        }

        public int TotalMileage
        {
            get
            {
                return ServiceSubmission.JobTotalMileage;
            }
            set
            {
                ServiceSubmission.JobTotalMileage = value;
                onPropertyChanged("TotalMileage");
            }
        }

        public int TotalDailyAllowances
        {
            get
            {
                return ServiceSubmission.TotalDailyAllowances;
            }
            set
            {
                ServiceSubmission.TotalDailyAllowances = value;
                onPropertyChanged("TotalDailyAllowances");
            }
        }

        public int TotalOvernightAllowances
        {
            get
            {
                return ServiceSubmission.TotalOvernightAllowances;
            }
            set
            {
                ServiceSubmission.TotalOvernightAllowances = value;
                onPropertyChanged("TotalOvernightAllowances");
            }
        }

        public int TotalBarrierPayments
        {
            get
            {
                return ServiceSubmission.TotalBarrierPayments;
            }
            set
            {
                ServiceSubmission.TotalBarrierPayments = value;
                onPropertyChanged("TotalBarrierPayments");
            }
        }

        public string CustomerSignedName
        {
            get
            {
                return ServiceSubmission.CustomerName;
            }
            set
            {
                ServiceSubmission.CustomerName = value;
                onPropertyChanged("CustomerSignedName");
            }
        }

        public DateTime DtSigned
        {
            get
            {
                return ServiceSubmission.DtSigned;
            }
            set
            {
                ServiceSubmission.DtSigned = value;
                onPropertyChanged("DtSigned");
            }
        }

        //RT - This isn't saved, just used to check that all submissions have been read and checked in office.
        public bool OfficeApproval
        {
            get
            {
                return m_officeApproval;
            }

            set
            {
                m_officeApproval = value;
                onPropertyChanged("OfficeApproval");
            }
        }

        public string MttEngSignatureUrl
        {
            get
            {
                return ServiceSubmission.MttEngSignatureUrl;
            }
        }

        public string CustomerSignatureUrl
        {
            get
            {
                return ServiceSubmission.CustomerSignatureUrl;
            }
        }

        public string Image1Url
        {
            get
            {
                return ServiceSubmission.Image1Url;
            }
        }

        public string Image2Url
        {
            get
            {
                return ServiceSubmission.Image2Url;
            }
        }

        public string Image3Url
        {
            get
            {
                return ServiceSubmission.Image3Url;
            }
        }

        public string Image4Url
        {
            get
            {
                return ServiceSubmission.Image4Url;
            }
        }

        public string Image5Url
        {
            get
            {
                return ServiceSubmission.Image5Url;
            }
        }

        public ImageSource MttEngineerSignature
        {
            get
            {
                return m_mttEngineerSignature;
            }

            set
            {
                m_mttEngineerSignature = value;
                onPropertyChanged("MttEngineerSignature");
            }
        }

        public ImageSource CustomerSignature
        {
            get
            {
                return m_customerSignature;
            }

            set
            {
                m_customerSignature = value;
                onPropertyChanged("CustomerSignature");
            }
        }

        public ImageSource Image1
        {
            get
            {
                return m_image1;
            }

            set
            {
                m_image1 = value;
                onPropertyChanged("Image1");
            }
        }

        public ImageSource Image2
        {
            get
            {
                return m_image2;
            }

            set
            {
                m_image2 = value;
                onPropertyChanged("Image2");
            }
        }

        public ImageSource Image3
        {
            get
            {
                return m_image3;
            }

            set
            {
                m_image3 = value;
                onPropertyChanged("Image3");
            }
        }

        public ImageSource Image4
        {
            get
            {
                return m_image4;
            }

            set
            {
                m_image4 = value;
                onPropertyChanged("Image4");
            }
        }

        public ImageSource Image5
        {
            get
            {
                return m_image5;
            }

            set
            {
                m_image5 = value;
                onPropertyChanged("Image5");
            }
        }
    }
}
