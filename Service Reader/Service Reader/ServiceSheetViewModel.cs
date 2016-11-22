using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader 
{
    public class ServiceSheetViewModel : ObservableObject
    {
        private ServiceSheet m_serviceSubmission;
        //This isn't saved, just used to check that the submission has been read.
        private bool m_officeApproval = false;
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
            }
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
            }
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
            }
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
            }
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
            }
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
            }
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
            }
        }

        public ObservableCollection<ServiceDayViewModel> AllServiceDayVMs
        {
            get
            {
                ObservableCollection<ServiceDayViewModel> retval = new ObservableCollection<ServiceDayViewModel>();
                foreach (ServiceDay sd in ServiceSubmission.ServiceDays)
                {
                    ServiceDayViewModel serviceDay = new ServiceDayViewModel(sd);
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
            }
        }

        public double TotalTimeOnsite
        {
            get
            {
                return ServiceSubmission.JobTotalTimeOnsite;
            }
        }

        public double TotalTravelTime
        {
            get
            {
                return ServiceSubmission.JobTotalTravelTime;
            }
        }

        public double TotalMileage
        {
            get
            {
                return ServiceSubmission.JobTotalMileage;
            }
        }

        public double TotalDailyAllowances
        {
            get
            {
                return ServiceSubmission.TotalDailyAllowances;
            }
        }

        public double TotalOvernightAllowances
        {
            get
            {
                return ServiceSubmission.TotalOvernightAllowances;
            }
        }

        public double TotalBarrierPayments
        {
            get
            {
                return ServiceSubmission.TotalBarrierPayments;
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

    }
}
