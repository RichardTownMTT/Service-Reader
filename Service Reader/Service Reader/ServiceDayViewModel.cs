using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class ServiceDayViewModel : ObservableObject
    {
        private List<ServiceDayModel> allServiceDays;

        public List<ServiceDayModel> getAllServiceDays
        {
            get { return allServiceDays; }
            set
            {
                if (allServiceDays != value)
                {
                    allServiceDays = value;
                    onPropertyChanged("getAllServiceDays");
                }
            }
        }
    }
}
