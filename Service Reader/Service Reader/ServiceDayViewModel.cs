using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class ServiceDayViewModel : ObservableObject
    {
       private ServiceDay m_serviceDay;
       public ServiceDayViewModel(ServiceDay currentDay)
        {
            ServiceDay = currentDay;
        }

        public ServiceDay ServiceDay
        {
            get
            {
                return m_serviceDay;
            }

            set
            {
                m_serviceDay = value;
                onPropertyChanged("ServiceDay");
            }
        }

        public DateTime ServiceDate
        {
            get
            {
                return ServiceDay.DtReport;
            }
            set
            {
                ServiceDay.DtReport = value;
            }
        }

        public DateTime TravelStartTime
        {
            get
            {
                return ServiceDay.TravelStartTime;
            }
            set
            {
                ServiceDay.TravelStartTime = value;
            }
        }

        public DateTime ArrivalOnsiteTime
        {
            get
            {
                return ServiceDay.ArrivalOnsiteTime;
            }
            set
            {
                ServiceDay.ArrivalOnsiteTime = value;
            }
        }

        public DateTime DepartSiteTime
        {
            get
            {
                return ServiceDay.DepartureSiteTime;
            }
            set
            {
                ServiceDay.DepartureSiteTime = value;
            }
        }

        public DateTime TravelEndTime
        {
            get
            {
                return ServiceDay.TravelEndTime;
            }
            set
            {
                ServiceDay.TravelEndTime = value;
            }
        }

        public int Mileage
        {
            get
            {
                return ServiceDay.Mileage;
            }
            set
            {
                ServiceDay.Mileage = value;
            }
        }

        public bool DailyAllowance
        {
            get
            {
                if (ServiceDay.DailyAllowance == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    ServiceDay.DailyAllowance = 1;
                }
                else
                {
                    ServiceDay.DailyAllowance = 0;
                }
            }
        }

        public bool OvernightAllowance
        {
            get
            {
                if (ServiceDay.OvernightAllowance == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    ServiceDay.OvernightAllowance = 1;
                }
                else
                {
                    ServiceDay.OvernightAllowance = 0;
                }
            }
        }

        public bool BarrierPayment
        {
            get
            {
                if (ServiceDay.BarrierPayment == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    ServiceDay.BarrierPayment = 1;
                }
                else
                {
                    ServiceDay.BarrierPayment = 0;
                }
            }
        }

        public double TravelTimeToSite
        {
            get
            {
                return ServiceDay.TravelToSiteTime;
            }
            set
            {
                ServiceDay.TravelToSiteTime = value;
            }
        }

        public double TravelTimeFromSite
        {
            get
            {
                return ServiceDay.TravelFromSiteTime;
            }
            set
            {
                ServiceDay.TravelFromSiteTime = value;
            }
        }

        public double TotalTravelTime
        {
            get
            {
                return ServiceDay.TotalTravelTime;
            }
            set
            {
                ServiceDay.TotalTravelTime = value;
            }
        }

        public double TotalTimeOnsite
        {
            get
            {
                return ServiceDay.TotalOnsiteTime;
            }
            set
            {
                ServiceDay.TotalOnsiteTime = value;
            }
        }

        public string DailyReport
        {
            get
            {
                return ServiceDay.DailyReport;
            }
            set
            {
                ServiceDay.DailyReport = value;
            }
        }

        public string PartsSupplied
        {
            get
            {
                return ServiceDay.PartsSuppliedToday;
            }
            set
            {
                ServiceDay.PartsSuppliedToday = value;
            }
        }
    }
}
