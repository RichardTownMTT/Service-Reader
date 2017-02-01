using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Service_Reader
{
    public class HolidayAbsenceViewModel : ObservableObject
    {
        private ObservableCollection<ServiceSheetViewModel> m_allAbsenceHolidays;
        private ICommand m_loadHolidayAbsenceCommand;

        public ObservableCollection<ServiceSheetViewModel> AllAbsenceHolidays
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

        public ICommand LoadHolidayAbsenceCommand
        {
            get
            {
                if (m_loadHolidayAbsenceCommand == null)
                {
                    m_loadHolidayAbsenceCommand = new RelayCommand(param => loadHolidayAbsences());
                }
                return m_loadHolidayAbsenceCommand;
            }

            set
            {
                m_loadHolidayAbsenceCommand = value;
            }
        }

        private void loadHolidayAbsences()
        {
            List<ServiceSheetViewModel> loadedSheets = DbServiceSheet.loadHolidayAbsenceSheets();
            AllAbsenceHolidays = new ObservableCollection<ServiceSheetViewModel>(loadedSheets);
        }
    }
}
