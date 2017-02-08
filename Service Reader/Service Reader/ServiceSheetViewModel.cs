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
        //RT 29/11/16 - Changing this to properly implement MVVM
        private ServiceSheet m_serviceSubmission;
        //private DateTime m_dtStartSubmission;
        //private DateTime m_dtEndSubmission;
        private int m_submissionNumber;
        private string m_appName;
        private string m_username;
        private string m_userFirstName;
        private string m_userSurname;
        private string m_canvasResponseId;
        private DateTime m_dtResponse;
        private DateTime m_dtDevice;
        private string m_submissionFormName;
        private int m_submissionFormVersion;
        private string m_customer;
        private string m_addressLine1;
        private string m_addressLine2;
        private string m_townCity;
        private string m_postcode;
        private string m_customerContact;
        private string m_customerPhoneNo;
        private string m_machineMakeModel;
        private string m_machineSerial;
        private string m_cncControl;
        private DateTime m_dtJobStart;
        private string m_customerOrderNo;
        private string m_mttJobNumber;
        private string m_jobDescription;
        private double m_jobTotalTimeOnsite;
        private double m_jobTotalTravelTime;
        private int m_jobTotalMileage;
        private int m_totalDailyAllowances;
        private int m_totalOvernightAllowances;
        private int m_totalBarrierPayments;
        private string m_jobStatus;
        private string m_finalJobReport;
        private string m_additionalFaults;
        private bool m_quoteRequired;
        private string m_followUpPartsRequired;
        private string m_image1Url;
        private string m_image2Url;
        private string m_image3Url;
        private string m_image4Url;
        private string m_image5Url;
        private string m_customerSignatureUrl;
        private string m_customerName;
        private DateTime m_dtSigned;
        private string m_mttEngSignatureUrl;

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
        //RT 30/1/17 - Adding a flag for Uk/US sheets.  This doesn't save to the model.  Calculated from the job number
        private bool? ukServiceSheet = null;
        //RT 1/2/17 - Adding a date for the last date on the job.  
        private DateTime m_sheetFinishDate;

        private AllServiceDayViewModels m_AllServiceDays;
        //RT 11/12/16 - Adding an edit mode for the submission.
        private bool m_editMode = false;
        //RT 13/12/16 - Following is used to see if multiple sheets have been selected in datagrid
        private bool m_selected = false;



        //Creator for the VM
        public ServiceSheetViewModel(int submissionNoEntered, string appNameEntered, string userfirstNameEntered, string userSurnameEntered, string canvasResponseIdEntered, DateTime dtResponseEntered, 
            DateTime dtDeviceEntered, string submissionFormNameEntered, int submissionVersionEntered, string customerEntered, string address1Entered, string address2Entered, string townCityEntered,
            string postcodeEntered, string contactEntered, string phoneNumberEntered, string makeModelEntered, string serialNumberEntered, string cncControlEntered, DateTime dtJobStartEntered, 
            string customerOrderEntered, string mttJobNumberEntered, string jobDescEntered, double totalTimeOnsiteEntered, double totalTravelTimeEntered, int totalMileageEntered,
            int totalDAEntered, int totalOAEntered, int totalBPEntered, string jobStatusEntered, string finalJobReportEntered, string additionalFaultsEntered, bool quoteEntered, string followupPartsEntered,
            string image1UrlEntered, string image2UrlEntered, string image3UrlEntered, string image4UrlEntered, string image5UrlEntered, string customerSignatureUrlEntered, string custSignedNameEntered,
            DateTime dtSignedEntered, string mttEngSignatureUrlEntered, AllServiceDayViewModels serviceDaysEntered, string usernameEntered)
        {
            //ServiceSheetViewModel retval = new ServiceSheetViewModel();
            this.SubmissionNumber = submissionNoEntered;
            this.AppName = appNameEntered;
            this.UserFirstName = userfirstNameEntered;
            this.UserSurname = userSurnameEntered;
            this.Username = usernameEntered;
            this.CanvasResponseId = canvasResponseIdEntered;
            this.DtResponse = dtResponseEntered;
            this.DtDevice = dtDeviceEntered;
            this.SubmissionFormName = submissionFormNameEntered;
            this.SubmissionFormVersion = submissionVersionEntered;
            this.Customer = customerEntered;
            this.AddressLine1 = address1Entered;
            this.AddressLine2 = address2Entered;
            this.TownCity = townCityEntered;
            this.Postcode = postcodeEntered;
            this.CustomerContact = contactEntered;
            this.CustomerPhoneNo = phoneNumberEntered;
            this.MachineMakeModel = makeModelEntered;
            this.MachineSerial = serialNumberEntered;
            this.CncControl = cncControlEntered;
            this.DtJobStart = dtJobStartEntered;
            this.CustomerOrderNo = customerOrderEntered;
            this.MttJobNumber = mttJobNumberEntered;
            this.JobDescription = jobDescEntered;
            this.JobTotalTimeOnsite = totalTimeOnsiteEntered;
            this.JobTotalTravelTime = totalTravelTimeEntered;
            this.JobTotalMileage = totalMileageEntered;
            this.TotalDailyAllowances = totalDAEntered;
            this.TotalOvernightAllowances = totalOAEntered;
            this.TotalBarrierPayments = totalBPEntered;
            this.JobStatus = jobStatusEntered;
            this.FinalJobReport = finalJobReportEntered;
            this.AdditionalFaults = additionalFaultsEntered;
            this.QuoteRequired = quoteEntered;
            this.FollowUpPartsRequired = followupPartsEntered;
            this.Image1Url = image1UrlEntered;
            this.Image2Url = image2UrlEntered;
            this.Image3Url = image3UrlEntered;
            this.Image4Url = image4UrlEntered;
            this.Image5Url = image5UrlEntered;
            this.CustomerSignatureUrl = customerSignatureUrlEntered;
            this.CustomerName = custSignedNameEntered;
            this.DtSigned = dtSignedEntered;
            this.MttEngSignatureUrl = mttEngSignatureUrlEntered;
            //We need to set the service sheet reference on the day
            //We also need to set the service days on the sheet
            this.ServiceSubmission.ServiceDays = new List<ServiceDay>();

            foreach (ServiceDayViewModel sd in serviceDaysEntered.AllServiceDayVMs)
            {
                this.ServiceSubmission.ServiceDays.Add(sd.ServiceDayModel);
                sd.ParentServiceSheetVM = this;
                //RT 1/2/17 - Need to calculate the date of the last sheet
                checkLastServiceDayDate(sd.DtReport);
            }
            this.AllServiceDays = serviceDaysEntered;
        }

        private void checkLastServiceDayDate(DateTime dtReport)
        {
            //Checks if the service day date is greater than the one currently set
            if (SheetFinishDate == new DateTime())
            {
                SheetFinishDate = dtReport;
            }
            else if (SheetFinishDate < dtReport)
            {
                SheetFinishDate = dtReport;
            }
        }

        private void checkUkJobNumber()
        {
            if(string.IsNullOrEmpty(MttJobNumber))
            {
                UkServiceSheet = null;
                return;
            }
            string jobNumberFirstTwoChars = MttJobNumber.Substring(0, 2);
            jobNumberFirstTwoChars = jobNumberFirstTwoChars.ToUpper();
            if (jobNumberFirstTwoChars.Equals("US"))
            {
                UkServiceSheet = false;
            }
            else
            {
                UkServiceSheet = true;
            }
        }

        public static List<ServiceSheetViewModel> loadFromModel(IQueryable<ServiceSheet> serviceSheets)
        {
            List<ServiceSheetViewModel> retval = new List<ServiceSheetViewModel>();
            foreach (var serviceSheet in serviceSheets)
            {
                ServiceSheetViewModel sheetLoaded = new ServiceSheetViewModel(serviceSheet);
                retval.Add(sheetLoaded);
            }
            return retval;
        }

        //RT 11/12/16 - This loads the values from the ServiceSheet and the ServiceDays
        public void CancelEdit()
        {
            AdditionalFaults = ServiceSubmission.AdditionalFaults;
            AddressLine1 = ServiceSubmission.AddressLine1;
            AddressLine2 = ServiceSubmission.AddressLine2;
            AppName = ServiceSubmission.AppName;
            CanvasResponseId = ServiceSubmission.CanvasResponseId;
            CncControl = ServiceSubmission.CncControl;
            Customer = ServiceSubmission.Customer;
            CustomerContact = ServiceSubmission.CustomerContact;
            CustomerName = ServiceSubmission.CustomerName;
            CustomerOrderNo = ServiceSubmission.CustomerOrderNo;
            CustomerPhoneNo = ServiceSubmission.CustomerPhoneNo;
            CustomerSignatureUrl = ServiceSubmission.CustomerSignatureUrl;
            DtDevice = ServiceSubmission.DtDevice;
            //DtEndSubmission = ServiceSubmission.DtEndSubmission;
            DtJobStart = ServiceSubmission.DtJobStart;
            DtResponse = ServiceSubmission.DtResponse;
            DtSigned = ServiceSubmission.DtSigned;
            //DtStartSubmission = ServiceSubmission.DtStartSubmission;
            FinalJobReport = ServiceSubmission.FinalJobReport;
            FollowUpPartsRequired = ServiceSubmission.FollowUpPartsRequired;
            Image1Url = ServiceSubmission.Image1Url;
            Image2Url = ServiceSubmission.Image2Url;
            Image3Url = ServiceSubmission.Image3Url;
            Image4Url = ServiceSubmission.Image4Url;
            Image5Url = ServiceSubmission.Image5Url;
            JobDescription = ServiceSubmission.JobDescription;
            JobStatus = ServiceSubmission.JobStatus;
            JobTotalMileage = ServiceSubmission.JobTotalMileage;
            JobTotalTimeOnsite = ServiceSubmission.JobTotalTimeOnsite;
            JobTotalTravelTime = ServiceSubmission.JobTotalTravelTime;
            MachineMakeModel = ServiceSubmission.MachineMakeModel;
            MachineSerial = ServiceSubmission.MachineSerial;
            MttEngSignatureUrl = ServiceSubmission.MttEngSignatureUrl;
            MttJobNumber = ServiceSubmission.MttJobNumber;
            Postcode = ServiceSubmission.Postcode;
            QuoteRequired = ServiceSubmission.QuoteRequired;
            SubmissionFormName = ServiceSubmission.SubmissionFormName;
            SubmissionFormVersion  = ServiceSubmission.SubmissionFormVersion;
            SubmissionNumber = ServiceSubmission.SubmissionNumber;
            TotalBarrierPayments = ServiceSubmission.TotalBarrierPayments;
            TotalDailyAllowances = ServiceSubmission.TotalDailyAllowances;
            TotalOvernightAllowances = ServiceSubmission.TotalOvernightAllowances;
            TownCity = ServiceSubmission.TownCity;
            UserFirstName = ServiceSubmission.UserFirstName;
            Username = ServiceSubmission.Username;
            UserSurname = ServiceSubmission.UserSurname;
            foreach (ServiceDayViewModel sd in AllServiceDays.AllServiceDayVMs)
            {
                sd.CancelEdit();
                //RT 1/2/17 - need to reset the sheet finish date
                checkLastServiceDayDate(sd.DtReport);
            }
            //If the job number starts with US, then not a uk job
            checkUkJobNumber();
            EditMode = false;
        }

        public ServiceSheetViewModel()
        {
            ServiceSubmission = new ServiceSheet();
        }

        public ServiceSheetViewModel(ServiceSheet serviceSheet)
        {
            this.SubmissionNumber = serviceSheet.SubmissionNumber;
            this.AppName = serviceSheet.AppName;
            this.UserFirstName = serviceSheet.UserFirstName;
            this.UserSurname = serviceSheet.UserSurname;
            this.Username = serviceSheet.Username;
            this.CanvasResponseId = serviceSheet.CanvasResponseId;
            this.DtResponse = serviceSheet.DtResponse;
            this.DtDevice = serviceSheet.DtDevice;
            this.SubmissionFormName = serviceSheet.SubmissionFormName;
            this.SubmissionFormVersion = serviceSheet.SubmissionFormVersion;
            this.Customer = serviceSheet.Customer;
            this.AddressLine1 = serviceSheet.AddressLine1;
            this.AddressLine2 = serviceSheet.AddressLine2;
            this.TownCity = serviceSheet.TownCity;
            this.Postcode = serviceSheet.Postcode;
            this.CustomerContact = serviceSheet.CustomerContact;
            this.CustomerPhoneNo = serviceSheet.CustomerPhoneNo;
            this.MachineMakeModel = serviceSheet.MachineMakeModel;
            this.MachineSerial = serviceSheet.MachineSerial;
            this.CncControl = serviceSheet.CncControl;
            this.DtJobStart = serviceSheet.DtJobStart;
            this.CustomerOrderNo = serviceSheet.CustomerOrderNo;
            this.MttJobNumber = serviceSheet.MttJobNumber;
            this.JobDescription = serviceSheet.JobDescription;
            this.JobTotalTimeOnsite = serviceSheet.JobTotalTimeOnsite;
            this.JobTotalTravelTime = serviceSheet.JobTotalTravelTime;
            this.JobTotalMileage = serviceSheet.JobTotalMileage;
            this.TotalDailyAllowances = serviceSheet.TotalDailyAllowances;
            this.TotalOvernightAllowances = serviceSheet.TotalOvernightAllowances;
            this.TotalBarrierPayments = serviceSheet.TotalBarrierPayments;
            this.JobStatus = serviceSheet.JobStatus;
            this.FinalJobReport = serviceSheet.FinalJobReport;
            this.AdditionalFaults = serviceSheet.AdditionalFaults;
            this.QuoteRequired = serviceSheet.QuoteRequired;
            this.FollowUpPartsRequired = serviceSheet.FollowUpPartsRequired;
            this.Image1Url = serviceSheet.Image1Url;
            this.Image2Url = serviceSheet.Image2Url;
            this.Image3Url = serviceSheet.Image3Url;
            this.Image4Url = serviceSheet.Image4Url;
            this.Image5Url = serviceSheet.Image5Url;
            this.CustomerSignatureUrl = serviceSheet.CustomerSignatureUrl;
            this.CustomerName = serviceSheet.CustomerName;
            this.DtSigned = serviceSheet.DtSigned;
            this.MttEngSignatureUrl = serviceSheet.MttEngSignatureUrl;
            //We need to set the service sheet reference on the day
            //We also need to set the service days on the sheet
            this.ServiceSubmission.ServiceDays = new List<ServiceDay>();
            this.ServiceSubmission = serviceSheet;

            //If the job number starts with US, then not a uk job
            checkUkJobNumber();

            //Load the service days from the models
            foreach (ServiceDay sd in serviceSheet.ServiceDays)
            {
                ServiceDayViewModel serviceDayVM = new ServiceDayViewModel(sd);
                this.AllServiceDays.AddServiceDay(serviceDayVM);
                serviceDayVM.ParentServiceSheetVM = this;
                //Need to set the last date from the service days
                checkLastServiceDayDate(serviceDayVM.DtReport);
            }
        }

        public ServiceSheetViewModel(DbEmployee selectedUser, EngineerActivity selectedActivity, DateTime startDate, DateTime endDate)
        {
            //RT 31/1/17 - This creates an activity not carried out on customer site. E.g. Holiday, stand by, etc
            this.SubmissionNumber = selectedActivity.ActivityCode;
            this.AppName = "Service Sheet";
            this.Username = selectedUser.Username;
            this.UserFirstName = selectedUser.Firstname;
            this.UserSurname = selectedUser.Surname;
            this.CanvasResponseId = "";
            this.DtResponse = DateTime.Now;
            this.DtDevice = DateTime.Now;
            this.SubmissionFormName = "Service Sheet";
            this.SubmissionFormVersion = 12;
            this.Customer = "MTT";
            this.AddressLine1 = "";
            this.AddressLine2 = "";
            this.TownCity = "";
            this.Postcode = "";
            this.CustomerContact = "";
            this.CustomerPhoneNo = "";
            this.MachineMakeModel = "";
            this.MachineSerial = "";
            this.CncControl = "";
            this.DtJobStart = startDate;
            this.CustomerOrderNo = "";
            this.MttJobNumber = selectedActivity.ActivityDescription;
            this.JobDescription = selectedActivity.ActivityDescription;
            this.JobTotalTimeOnsite = 0;
            this.JobTotalTravelTime = 0;
            this.JobTotalMileage = 0;
            this.TotalDailyAllowances = 0;
            this.TotalOvernightAllowances = 0;
            this.TotalBarrierPayments = 0;
            this.JobStatus = "Job completed";
            this.FinalJobReport = "";
            this.AdditionalFaults = "";
            this.QuoteRequired = false;
            this.FollowUpPartsRequired = "";
            this.Image1Url = "";
            this.Image2Url = "";
            this.Image3Url = "";
            this.Image4Url = "";
            this.Image5Url = "";
            this.CustomerSignatureUrl = "";
            this.CustomerName = "";
            this.DtSigned = new DateTime();
            this.MttEngSignatureUrl = "";

            TimeSpan noOfDaysSpan =  endDate - startDate;
            double noOfDays = noOfDaysSpan.TotalDays + 1;
            
            for (DateTime currentDate = startDate; currentDate <= endDate; currentDate = currentDate.AddDays(1))
            {
                ServiceDayViewModel sdVM = ServiceDayViewModel.createServiceDayForHolidayAbsence(currentDate);
                //If it is a weekend, then the day will be null
                if (sdVM == null)
                {
                    continue;
                }
                sdVM.ParentServiceSheetVM = this;

                AllServiceDays.AddServiceDay(sdVM);

                checkLastServiceDayDate(sdVM.DtReport);
            }
            
        }

        //Method to save the data back to the model
        public void SaveToModel()
        {
            if (ServiceSubmission == null)
            {
                ServiceSubmission = new ServiceSheet();
            }

            ServiceSubmission.AdditionalFaults = m_additionalFaults;
            ServiceSubmission.AddressLine1 = m_addressLine1;
            ServiceSubmission.AddressLine2 = m_addressLine2;
            ServiceSubmission.AppName = m_appName;
            ServiceSubmission.CanvasResponseId = m_canvasResponseId;
            ServiceSubmission.CncControl = m_cncControl;
            ServiceSubmission.Customer = m_customer;
            ServiceSubmission.CustomerContact = m_customerContact;
            ServiceSubmission.CustomerName = m_customerName;
            ServiceSubmission.CustomerOrderNo = m_customerOrderNo;
            ServiceSubmission.CustomerPhoneNo = m_customerPhoneNo;
            ServiceSubmission.CustomerSignatureUrl = m_customerSignatureUrl;
            ServiceSubmission.DtDevice = m_dtDevice;
            //ServiceSubmission.DtEndSubmission = m_dtEndSubmission;
            ServiceSubmission.DtJobStart = m_dtJobStart;
            ServiceSubmission.DtResponse = m_dtResponse;
            ServiceSubmission.DtSigned = m_dtSigned;
            //ServiceSubmission.DtStartSubmission = m_dtStartSubmission;
            ServiceSubmission.FinalJobReport = m_finalJobReport;
            ServiceSubmission.FollowUpPartsRequired = m_followUpPartsRequired;
            ServiceSubmission.Image1Url = m_image1Url;
            ServiceSubmission.Image2Url = m_image2Url;
            ServiceSubmission.Image3Url = m_image3Url;
            ServiceSubmission.Image4Url = m_image4Url;
            ServiceSubmission.Image5Url = m_image5Url;
            ServiceSubmission.JobDescription = m_jobDescription;
            ServiceSubmission.JobStatus = m_jobStatus;
            ServiceSubmission.JobTotalMileage = m_jobTotalMileage;
            ServiceSubmission.JobTotalTimeOnsite = m_jobTotalTimeOnsite;
            ServiceSubmission.JobTotalTravelTime = m_jobTotalTravelTime;
            ServiceSubmission.MachineMakeModel = m_machineMakeModel;
            ServiceSubmission.MachineSerial = m_machineSerial;
            ServiceSubmission.MttEngSignatureUrl = m_mttEngSignatureUrl;
            ServiceSubmission.MttJobNumber = m_mttJobNumber;
            ServiceSubmission.Postcode = m_postcode;
            ServiceSubmission.QuoteRequired = m_quoteRequired;
            ServiceSubmission.SubmissionFormName = m_submissionFormName;
            ServiceSubmission.SubmissionFormVersion = m_submissionFormVersion;
            ServiceSubmission.SubmissionNumber = m_submissionNumber;
            ServiceSubmission.TotalBarrierPayments = m_totalBarrierPayments;
            ServiceSubmission.TotalDailyAllowances = m_totalDailyAllowances;
            ServiceSubmission.TotalOvernightAllowances = m_totalOvernightAllowances;
            ServiceSubmission.TownCity = m_townCity;
            ServiceSubmission.UserFirstName = m_userFirstName;
            ServiceSubmission.Username = m_username;
            ServiceSubmission.UserSurname = m_userSurname;
            //Need to call the save on the service days
            foreach (ServiceDayViewModel sd in AllServiceDays.AllServiceDayVMs)
            {
                sd.Save();
            }
            EditMode = false;
            
        }

        //public ServiceSheet ServiceSubmission
        //{
        //    get
        //    {
        //        return m_serviceSubmission;
        //    }

        //    set
        //    {
        //        m_serviceSubmission = value;
        //        onPropertyChanged("ServiceSubmission");
        //    }
        //}

        //RT 18/11/16 - This is only used for display purposes - it will not be changed
        public string EngineerFullName
        {
            get
            {
                return UserFirstName + " " + UserSurname;
            }
        }


        //public int SubmissionNumber
        //{
        //    get
        //    {
        //        return ServiceSubmission.SubmissionNumber;
        //    }
        //    set
        //    {
        //        ServiceSubmission.SubmissionNumber = value;
        //        onPropertyChanged("SubmissionNumber");
        //    }
        //}

        //public string Customer
        //{
        //    get
        //    {
        //        return ServiceSubmission.Customer;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Customer = value;
        //        onPropertyChanged("Customer");
        //    }
        //}

        //public string AddressLine1
        //{
        //    get
        //    {
        //        return ServiceSubmission.AddressLine1;
        //    }
        //    set
        //    {
        //        ServiceSubmission.AddressLine1 = value;
        //        onPropertyChanged("AddressLine1");
        //    }
        //}

        public void recalculateTravelTime()
        {
            double travelTime = 0;
            foreach(ServiceDayViewModel day in AllServiceDays.AllServiceDayVMs)
            {
                travelTime += day.TotalTravelTime;
            }
            JobTotalTravelTime = travelTime;
        }

        //public string AddressLine2
        //{
        //    get
        //    {
        //        return ServiceSubmission.AddressLine2;
        //    }
        //    set
        //    {
        //        ServiceSubmission.AddressLine2 = value;
        //        onPropertyChanged("AddressLine2");
        //    }
        //}

        //public string TownCity
        //{
        //    get
        //    {
        //        return ServiceSubmission.TownCity;
        //    }
        //    set
        //    {
        //        ServiceSubmission.TownCity = value;
        //        onPropertyChanged("TownCity");
        //    }
        //}

        public void recalulateTimeOnsite()
        {
            double timeOnsite = 0;
            foreach (ServiceDayViewModel serviceDay in AllServiceDays.AllServiceDayVMs)
            {
                timeOnsite += serviceDay.TotalOnsiteTime;
            }
            JobTotalTimeOnsite = timeOnsite;
        }

        public void updateAllTimes()
        {
            //RT 24/11/16 - This is used when importing data
            recalculateBarrierPayments();
            recalculateDailyAllowances();
            recalculateOvernightAllowances();
            recalculateMileage();
            recalculateTravelTime();
            recalulateTimeOnsite();
        }

        //public string Postcode
        //{
        //    get
        //    {
        //        return ServiceSubmission.Postcode;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Postcode = value;
        //        onPropertyChanged("Postcode");
        //    }
        //}

        //public string CustomerContact
        //{
        //    get
        //    {
        //        return ServiceSubmission.CustomerContact;
        //    }
        //    set
        //    {
        //        ServiceSubmission.CustomerContact = value;
        //        onPropertyChanged("CustomerContact");
        //    }
        //}

        //public string CustomerPhone
        //{
        //    get
        //    {
        //        return ServiceSubmission.CustomerPhoneNo;
        //    }
        //    set
        //    {
        //        ServiceSubmission.CustomerPhoneNo = value;
        //        onPropertyChanged("CustomerPhone");
        //    }
        //}

        public void recalculateMileage()
        {
            int updatedMileage = 0;
            foreach(ServiceDayViewModel day in AllServiceDays.AllServiceDayVMs)
            {
                updatedMileage += day.Mileage;
            }
            JobTotalMileage = updatedMileage;
        }

        //public DateTime JobStartDate
        //{
        //    get
        //    {
        //        return ServiceSubmission.DtJobStart;
        //    }
        //    set
        //    {
        //        ServiceSubmission.DtJobStart = value;
        //        onPropertyChanged("JobStartDate");
        //    }
        //}

        public void recalculateDailyAllowances()
        {
            int updatedDA = 0;
            foreach (ServiceDayViewModel day in AllServiceDays.AllServiceDayVMs)
            {
                if (day.DailyAllowance)
                {
                    updatedDA++;
                }
            }
            TotalDailyAllowances = updatedDA;
        }

        //public string MachineMakeModel
        //{
        //    get
        //    {
        //        return ServiceSubmission.MachineMakeModel;
        //    }
        //    set
        //    {
        //        ServiceSubmission.MachineMakeModel = value;
        //        onPropertyChanged("MachineMakeModel");
        //    }
        //}

        public void recalculateOvernightAllowances()
        {
            int updatedOA = 0;
            foreach(ServiceDayViewModel day in AllServiceDays.AllServiceDayVMs)
            {
                if (day.OvernightAllowance)
                {
                    updatedOA++;
                }
            }
            TotalOvernightAllowances = updatedOA;
        }

        //public string MachineSerialNo
        //{
        //    get
        //    {
        //        return ServiceSubmission.MachineSerial;
        //    }
        //    set
        //    {
        //        ServiceSubmission.MachineSerial = value;
        //        onPropertyChanged("MachineSerialNo");
        //    }
        //}

        public void recalculateBarrierPayments()
        {
            int updatedBP = 0;
            foreach(ServiceDayViewModel day in AllServiceDays.AllServiceDayVMs)
            {
                if(day.BarrierPayment)
                {
                    updatedBP++;
                }
            }
            TotalBarrierPayments = updatedBP;
        }

        //public string MachineController
        //{
        //    get
        //    {
        //        return ServiceSubmission.CncControl;
        //    }
        //    set
        //    {
        //        ServiceSubmission.CncControl = value;
        //        onPropertyChanged("MachineController");
        //    }
        //}

        //public string MttJobNo
        //{
        //    get
        //    {
        //        return ServiceSubmission.MttJobNumber;
        //    }
        //    set
        //    {
        //        ServiceSubmission.MttJobNumber = value;
        //        onPropertyChanged("MttJobNo");
        //    }
        //}

        //public string CustomerOrderNo
        //{
        //    get
        //    {
        //        return ServiceSubmission.CustomerOrderNo;
        //    }
        //    set
        //    {
        //        ServiceSubmission.CustomerOrderNo = value;
        //        onPropertyChanged("CustomerOrderNo");
        //    }
        //}

        //public string JobDescription
        //{
        //    get
        //    {
        //        return ServiceSubmission.JobDescription;
        //    }
        //    set
        //    {
        //        ServiceSubmission.JobDescription = value;
        //        onPropertyChanged("JobDescription");
        //    }
        //}

        //public ObservableCollection<ServiceDayViewModel> AllServiceDayVMs
        //{
        //    get
        //    {
        //        ObservableCollection<ServiceDayViewModel> retval = new ObservableCollection<ServiceDayViewModel>();
        //        foreach (ServiceDay sd in ServiceSubmission.ServiceDays)
        //        {
        //            ServiceDayViewModel serviceDay = new ServiceDayViewModel(sd, this);
        //            retval.Add(serviceDay);
        //        }

        //        return retval;
        //    }
        //}
        
        //public string JobStatus
        //{
        //    get
        //    {
        //        return ServiceSubmission.JobStatus;
        //    }
        //    set
        //    {
        //        ServiceSubmission.JobStatus = value;
        //        onPropertyChanged("JobStatus");
        //    }
        //}

        //public string FinalJobReport
        //{
        //    get
        //    {
        //        return ServiceSubmission.FinalJobReport;
        //    }
        //    set
        //    {
        //        ServiceSubmission.FinalJobReport = value;
        //        onPropertyChanged("FinalJobReport");
        //    }
        //}

        //public string AdditionalFaultsFound
        //{
        //    get
        //    {
        //        return ServiceSubmission.AdditionalFaults;
        //    }
        //    set
        //    {
        //        ServiceSubmission.AdditionalFaults = value;
        //        onPropertyChanged("AdditionalFaultsFound");
        //    }
        //}

        //public bool QuoteRequired
        //{
        //    get
        //    {
        //        return ServiceSubmission.QuoteRequired;
        //    }
        //    set
        //    {
        //        ServiceSubmission.QuoteRequired = value;
        //        onPropertyChanged("QuoteRequired");
        //    }
        //}

        //public string PartsForFollowup
        //{
        //    get
        //    {
        //        return ServiceSubmission.FollowUpPartsRequired;
        //    }
        //    set
        //    {
        //        ServiceSubmission.FollowUpPartsRequired = value;
        //        onPropertyChanged("PartsForFollowup");
        //    }
        //}

        //public double TotalTimeOnsite
        //{
        //    get
        //    {
        //        return ServiceSubmission.JobTotalTimeOnsite;
        //    }
        //    set
        //    {
        //        ServiceSubmission.JobTotalTimeOnsite = value;
        //        onPropertyChanged("TotalTimeOnsite");
        //    }
        //}

        //public double TotalTravelTime
        //{
        //    get
        //    {
        //        return ServiceSubmission.JobTotalTravelTime;
        //    }
        //    set
        //    {
        //        ServiceSubmission.JobTotalTravelTime = value;
        //        onPropertyChanged("TotalTravelTime");
        //    }
        //}

        //public int TotalMileage
        //{
        //    get
        //    {
        //        return ServiceSubmission.JobTotalMileage;
        //    }
        //    set
        //    {
        //        ServiceSubmission.JobTotalMileage = value;
        //        onPropertyChanged("TotalMileage");
        //    }
        //}

        //public int TotalDailyAllowances
        //{
        //    get
        //    {
        //        return ServiceSubmission.TotalDailyAllowances;
        //    }
        //    set
        //    {
        //        ServiceSubmission.TotalDailyAllowances = value;
        //        onPropertyChanged("TotalDailyAllowances");
        //    }
        //}

        //public int TotalOvernightAllowances
        //{
        //    get
        //    {
        //        return ServiceSubmission.TotalOvernightAllowances;
        //    }
        //    set
        //    {
        //        ServiceSubmission.TotalOvernightAllowances = value;
        //        onPropertyChanged("TotalOvernightAllowances");
        //    }
        //}

        //public int TotalBarrierPayments
        //{
        //    get
        //    {
        //        return ServiceSubmission.TotalBarrierPayments;
        //    }
        //    set
        //    {
        //        ServiceSubmission.TotalBarrierPayments = value;
        //        onPropertyChanged("TotalBarrierPayments");
        //    }
        //}

        //public string CustomerSignedName
        //{
        //    get
        //    {
        //        return ServiceSubmission.CustomerName;
        //    }
        //    set
        //    {
        //        ServiceSubmission.CustomerName = value;
        //        onPropertyChanged("CustomerSignedName");
        //    }
        //}

        //public DateTime DtSigned
        //{
        //    get
        //    {
        //        return ServiceSubmission.DtSigned;
        //    }
        //    set
        //    {
        //        ServiceSubmission.DtSigned = value;
        //        onPropertyChanged("DtSigned");
        //    }
        //}

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

        //public string MttEngSignatureUrl
        //{
        //    get
        //    {
        //        return ServiceSubmission.MttEngSignatureUrl;
        //    }
        //    set
        //    {
        //        ServiceSubmission.MttEngSignatureUrl = value;
        //        onPropertyChanged("MttEngSignatureUrl");
        //    }
        //}

        //public string CustomerSignatureUrl
        //{
        //    get
        //    {
        //        return ServiceSubmission.CustomerSignatureUrl;
        //    }
        //    set
        //    {
        //        ServiceSubmission.CustomerSignatureUrl = value;
        //        onPropertyChanged("CustomerSignatureUrl");
        //    }
        //}

        //public string Image1Url
        //{
        //    get
        //    {
        //        return ServiceSubmission.Image1Url;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Image1Url = value;
        //        onPropertyChanged("Image1Url");
        //    }
        //}

        //public string Image2Url
        //{
        //    get
        //    {
        //        return ServiceSubmission.Image2Url;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Image2Url = value;
        //        onPropertyChanged("Image2Url");
        //    }
        //}

        //public string Image3Url
        //{
        //    get
        //    {
        //        return ServiceSubmission.Image3Url;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Image3Url = value;
        //        onPropertyChanged("Image3Url");
        //    }
        //}

        //public string Image4Url
        //{
        //    get
        //    {
        //        return ServiceSubmission.Image4Url;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Image4Url = value;
        //        onPropertyChanged("Image4Url");
        //    }
        //}

        //public string Image5Url
        //{
        //    get
        //    {
        //        return ServiceSubmission.Image5Url;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Image5Url = value;
        //        onPropertyChanged("Image5Url");
        //    }
        //}

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

        //public string Username
        //{
        //    get
        //    {
        //        return ServiceSubmission.Username;
        //    }
        //    set
        //    {
        //        ServiceSubmission.Username = value;
        //        onPropertyChanged("Username");
        //    }
        //}

        //public string UserSurname
        //{
        //    get
        //    {
        //        return ServiceSubmission.UserSurname;
        //    }
        //    set
        //    {
        //        ServiceSubmission.UserSurname = value;
        //        onPropertyChanged("UserSurname");
        //    }
        //}

        //public string UserFirstName
        //{
        //    get
        //    {
        //        return ServiceSubmission.UserFirstName;
        //    }
        //    set
        //    {
        //        ServiceSubmission.UserFirstName = value;
        //        onPropertyChanged("UserFirstName");
        //    }
        //}

        //public string ResponseId
        //{
        //    get
        //    {
        //        return ServiceSubmission.CanvasResponseId;
        //    }
        //    set
        //    {
        //        ServiceSubmission.CanvasResponseId = value;
        //        onPropertyChanged("ResponseId");
        //    }
        //}

        //public DateTime DtResponse
        //{
        //    get
        //    {
        //        return ServiceSubmission.DtResponse;
        //    }
        //    set
        //    {
        //        ServiceSubmission.DtResponse = value;
        //        onPropertyChanged("DtResponse");
        //    }
        //}

        //public DateTime DtDevice
        //{
        //    get
        //    {
        //        return ServiceSubmission.DtDevice;
        //    }
        //    set
        //    {
        //        ServiceSubmission.DtDevice = value;
        //        onPropertyChanged("DtDevice");
        //    }
        //}

        //public int SubmissionVersion
        //{
        //    get
        //    {
        //        return ServiceSubmission.SubmissionFormVersion;
        //    }
        //    set
        //    {
        //        ServiceSubmission.SubmissionFormVersion = value;
        //        onPropertyChanged("SubmissionVersion");
        //    }
        //}

        public AllServiceDayViewModels AllServiceDays
        {
            get
            {
                if (m_AllServiceDays == null)
                {
                    m_AllServiceDays = new AllServiceDayViewModels();
                }
                return m_AllServiceDays;
            }
            set
            {
                m_AllServiceDays = value;
                onPropertyChanged("AllServiceDays");
            }
        }

        public ServiceSheet ServiceSubmission
        {
            get
            {
                if (m_serviceSubmission == null)
                {
                    m_serviceSubmission = new ServiceSheet();
                }
                return m_serviceSubmission;
            }

            set
            {
                m_serviceSubmission = value;
                onPropertyChanged("ServiceSubmission");
            }
        }

        //public DateTime DtStartSubmission
        //{
        //    get
        //    {
        //        return m_dtStartSubmission;
        //    }

        //    set
        //    {
        //        m_dtStartSubmission = value;
        //        onPropertyChanged("DtStartSubmission");
        //    }
        //}

        //public DateTime DtEndSubmission
        //{
        //    get
        //    {
        //        return m_dtEndSubmission;
        //    }

        //    set
        //    {
        //        m_dtEndSubmission = value;
        //        onPropertyChanged("DtEndSubmission");
        //    }
        //}

        public int SubmissionNumber
        {
            get
            {
                return m_submissionNumber;
            }

            set
            {
                m_submissionNumber = value;
                onPropertyChanged("SubmissionNumber");
            }
        }

        public string AppName
        {
            get
            {
                return m_appName;
            }

            set
            {
                m_appName = value;
                onPropertyChanged("AppName");
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
                m_username = value;
                onPropertyChanged("Username");
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
                m_userFirstName = value;
                onPropertyChanged("UserFirstName");
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
                m_userSurname = value;
                onPropertyChanged("UserSurname");
            }
        }

        public string CanvasResponseId
        {
            get
            {
                return m_canvasResponseId;
            }

            set
            {
                m_canvasResponseId = value;
                onPropertyChanged("CanvasResponseId");
            }
        }

        public DateTime DtResponse
        {
            get
            {
                return m_dtResponse;
            }

            set
            {
                m_dtResponse = value;
                onPropertyChanged("DtResponse");
            }
        }

        public DateTime DtDevice
        {
            get
            {
                return m_dtDevice;
            }

            set
            {
                m_dtDevice = value;
                onPropertyChanged("DtDevice");
            }
        }

        public string SubmissionFormName
        {
            get
            {
                return m_submissionFormName;
            }

            set
            {
                m_submissionFormName = value;
                onPropertyChanged("SubmissionFormName");
            }
        }

        public int SubmissionFormVersion
        {
            get
            {
                return m_submissionFormVersion;
            }

            set
            {
                m_submissionFormVersion = value;
                onPropertyChanged("SubmissionFormVersion");
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
                m_customer = value;
                onPropertyChanged("Customer");
            }
        }

        public string AddressLine1
        {
            get
            {
                return m_addressLine1;
            }

            set
            {
                m_addressLine1 = value;
                onPropertyChanged("AddressLine1");
            }
        }

        public string AddressLine2
        {
            get
            {
                return m_addressLine2;
            }

            set
            {
                m_addressLine2 = value;
                onPropertyChanged("AddressLine2");
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
                m_townCity = value;
                onPropertyChanged("TownCity");
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
                m_postcode = value;
                onPropertyChanged("Postcode");
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
                m_customerContact = value;
                onPropertyChanged("CustomerContact");
            }
        }

        public string CustomerPhoneNo
        {
            get
            {
                return m_customerPhoneNo;
            }

            set
            {
                m_customerPhoneNo = value;
                onPropertyChanged("CustomerPhoneNo");
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
                m_machineMakeModel = value;
                onPropertyChanged("MachineMakeModel");
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
                m_machineSerial = value;
                onPropertyChanged("MachineSerial");
            }
        }

        public string CncControl
        {
            get
            {
                return m_cncControl;
            }

            set
            {
                m_cncControl = value;
                onPropertyChanged("CncControl");
            }
        }

        public DateTime DtJobStart
        {
            get
            {
                return m_dtJobStart;
            }

            set
            {
                m_dtJobStart = value;
                onPropertyChanged("DtJobStart");
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
                m_customerOrderNo = value;
                onPropertyChanged("CustomerOrderNo");
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
                m_mttJobNumber = value;
                onPropertyChanged("MttJobNumber");
                //If the job number starts with US, then not a uk job
                checkUkJobNumber();
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
                m_jobDescription = value;
                onPropertyChanged("JobDescription");
            }
        }

        public double JobTotalTimeOnsite
        {
            get
            {
                return m_jobTotalTimeOnsite;
            }

            set
            {
                m_jobTotalTimeOnsite = value;
                onPropertyChanged("JobTotalTimeOnsite");
            }
        }

        public double JobTotalTravelTime
        {
            get
            {
                return m_jobTotalTravelTime;
            }

            set
            {
                m_jobTotalTravelTime = value;
                onPropertyChanged("JobTotalTravelTime");
            }
        }

        public int JobTotalMileage
        {
            get
            {
                return m_jobTotalMileage;
            }

            set
            {
                m_jobTotalMileage = value;
                onPropertyChanged("JobTotalMileage");
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
                m_totalDailyAllowances = value;
                onPropertyChanged("TotalDailyAllowances");
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
                m_totalOvernightAllowances = value;
                onPropertyChanged("TotalOvernightAllowances");
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
                m_totalBarrierPayments = value;
                onPropertyChanged("TotalBarrierPayments");
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
                m_jobStatus = value;
                onPropertyChanged("JobStatus");
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
                m_finalJobReport = value;
                onPropertyChanged("FinalJobReport");
            }
        }

        public string AdditionalFaults
        {
            get
            {
                return m_additionalFaults;
            }

            set
            {
                m_additionalFaults = value;
                onPropertyChanged("AdditionalFaults");
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
                m_quoteRequired = value;
                onPropertyChanged("QuoteRequired");
            }
        }

        public string FollowUpPartsRequired
        {
            get
            {
                return m_followUpPartsRequired;
            }

            set
            {
                m_followUpPartsRequired = value;
                onPropertyChanged("FollowUpPartsRequired");
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
                m_image1Url = value;
                onPropertyChanged("Image1Url");
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
                m_image2Url = value;
                onPropertyChanged("Image2Url");
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
                m_image3Url = value;
                onPropertyChanged("Image3Url");
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
                m_image4Url = value;
                onPropertyChanged("Image4Url");
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
                m_image5Url = value;
                onPropertyChanged("Image5Url");
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
                m_customerSignatureUrl = value;
                onPropertyChanged("CustomerSignatureUrl");
            }
        }

        public string CustomerName
        {
            get
            {
                return m_customerName;
            }

            set
            {
                m_customerName = value;
                onPropertyChanged("CustomerName");
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
                m_dtSigned = value;
                onPropertyChanged("DtSigned");
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
                m_mttEngSignatureUrl = value;
                onPropertyChanged("MttEngSignatureUrl");
            }
        }

        public bool EditMode
        {
            get
            {
                return m_editMode;
            }

            set
            {
                m_editMode = value;
                onPropertyChanged("EditMode");
            }
        }

        public bool Selected
        {
            get
            {
                return m_selected;
            }

            set
            {
                m_selected = value;
            }
        }

        public bool? UkServiceSheet
        {
            get
            {
                return ukServiceSheet;
            }

            set
            {
                ukServiceSheet = value;
            }
        }

        public DateTime SheetFinishDate
        {
            get
            {
                return m_sheetFinishDate;
            }

            set
            {
                m_sheetFinishDate = value;
            }
        }

        public void AddServiceDayViewModel(ServiceDayViewModel serviceDayToAdd)
        {
            AllServiceDays.AddServiceDay(serviceDayToAdd);
        }

    }
}
