using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class AbsenceHolidayViewModel : ObservableObject
    {
        private ObservableCollection<ServiceDayViewModel> m_allAbsenceHolidays;

        public ObservableCollection<ServiceDayViewModel> AllAbsenceHolidays
        {
            get
            {
                return m_allAbsenceHolidays;
            }

            set
            {
                m_allAbsenceHolidays = value;
                onPropertyChanged("AllAbsenceHolidays");
            }
        }
    }
}
