using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class AllServiceDayViewModels : ObservableObject
    {
        private ObservableCollection<ServiceDayViewModel> m_allServiceDays;

        public ObservableCollection<ServiceDayViewModel> AllServiceDayVMs
        {
            get
            {
                if(m_allServiceDays == null)
                {
                    m_allServiceDays = new ObservableCollection<ServiceDayViewModel>();
                }
                return m_allServiceDays;
            }

            set
            {
                m_allServiceDays = value;
                onPropertyChanged("AllServiceDays");
            }
        }

        public void AddServiceDay(ServiceDayViewModel sdToAdd)
        {
            AllServiceDayVMs.Add(sdToAdd);
        }
    }
}
