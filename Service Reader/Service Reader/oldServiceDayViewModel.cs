using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class oldServiceDayViewModel : ObservableObject
    {
        private List<oldServiceDayModel> allServiceDays;

        public List<oldServiceDayModel> getAllServiceDays
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
