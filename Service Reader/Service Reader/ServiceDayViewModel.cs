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
                onPropertyChanged("ServiceDate");
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
                onPropertyChanged("TravelStartTime");
                //If the travel start is altered, then the travel to site needs recalculating
                recalculateTravelTimeToSite();
            }
        }

        private void recalculateTravelTimeToSite()
        {
           TimeSpan travelToSite = ArrivalOnsiteTime - TravelStartTime;
           TravelTimeToSite = travelToSite.TotalHours;
           recalculateTotalTravelTime();
        }

        private void recalculateTotalTravelTime()
        {
            TotalTravelTime = TravelTimeToSite + TravelTimeFromSite;
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
                onPropertyChanged("ArrivalOnsiteTime");
                recalculateTravelTimeToSite();
                recalculateTimeOnsite();
            }
        }

        private void recalculateTimeOnsite()
        {
            TimeSpan timeOnsite = DepartSiteTime - ArrivalOnsiteTime;
            TotalTimeOnsite = timeOnsite.TotalHours;
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
                onPropertyChanged("DepartSiteTime");
                recalculateTimeOnsite();
                recalculateTravelFromSite();
            }
        }

        private void recalculateTravelFromSite()
        {
            TimeSpan travelFrom = TravelEndTime - DepartSiteTime;
            TravelTimeFromSite = travelFrom.TotalHours;
            recalculateTotalTravelTime();
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
                onPropertyChanged("TravelEndTime");
                recalculateTravelFromSite();
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
                onPropertyChanged("Mileage");
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
                onPropertyChanged("DailyAllowance");
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
                onPropertyChanged("OvernightAllowance");
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
                onPropertyChanged("BarrierPayment");
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
                onPropertyChanged("TravelTimeToSite");
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
                onPropertyChanged("TravelTimeFromSite");
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
                onPropertyChanged("TotalTravelTime");
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
                onPropertyChanged("TotalTimeOnsite");
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
                onPropertyChanged("DailyReport");
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
                onPropertyChanged("PartsSupplied");
            }
        }
    }
}
