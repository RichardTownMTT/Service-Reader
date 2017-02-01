using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class HolidayAbsenceCreatorViewModel : ObservableObject
    {
        private ServiceSheetViewModel m_holidayAbsence;
        private ObservableCollection<DbEmployee> m_employees;
        private ObservableCollection<EngineerActivity> m_engineerActivities;
        private ICommand m_saveActivityCommand;
        private DbEmployee m_selectedUser;
        private EngineerActivity m_selectedActivity;
        private DateTime m_startDate = DateTime.Now;
        private DateTime m_endDate = DateTime.Now;

        public HolidayAbsenceCreatorViewModel()
         {
            HolidayAbsence = new ServiceSheetViewModel();
            //Setup the data required from the database
            setupUsernameCombobox();
            setupActivityCombobox();
        }

        private void setupActivityCombobox()
        {
            List<EngineerActivity> allActivities = EngineerActivity.getAllActivities();
            EngineerActivities = new ObservableCollection<EngineerActivity>(allActivities);
        }

        private void setupUsernameCombobox()
        {
            #if DEBUG
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            #endif

            //Loads the usernames from the Service Sheet table
            List<DbEmployee> usernames = DbServiceSheet.getAllUsers();
            if (usernames == null)
            {
                MessageBox.Show("Error loading usernames");
                Employees = null;
                return;
            }
            Employees = new ObservableCollection<DbEmployee>(usernames);
        }

        public ServiceSheetViewModel HolidayAbsence
        {
            get
            {
                return m_holidayAbsence;
            }

            set
            {
                m_holidayAbsence = value;
            }
        }

        public ObservableCollection<DbEmployee> Employees
        {
            get
            {
                return m_employees;
            }

            set
            {
                m_employees = value;
                onPropertyChanged("Employees");
            }
        }

        public ObservableCollection<EngineerActivity> EngineerActivities
        {
            get
            {
                return m_engineerActivities;
            }

            set
            {
                m_engineerActivities = value;
                onPropertyChanged("EngineerActivities");
            }
        }

        public ICommand SaveActivityCommand
        {
            get
            {
                if (m_saveActivityCommand == null)
                {
                    m_saveActivityCommand = new RelayCommand(param => saveActivity());
                }
                return m_saveActivityCommand;
            }

            set
            {
                m_saveActivityCommand = value;
            }
        }

        public DbEmployee SelectedUser
        {
            get
            {
                return m_selectedUser;
            }

            set
            {
                m_selectedUser = value;
                onPropertyChanged("SelectedUser");
            }
        }

        public EngineerActivity SelectedActivity
        {
            get
            {
                return m_selectedActivity;
            }

            set
            {
                m_selectedActivity = value;
                onPropertyChanged("SelectedActivity");
            }
        }

        public DateTime StartDate
        {
            get
            {
                return m_startDate;
            }

            set
            {
                m_startDate = value;
                onPropertyChanged("StartDate");
            }
        }

        public DateTime EndDate
        {
            get
            {
                return m_endDate;
            }

            set
            {
                m_endDate = value;
                onPropertyChanged("EndDate");
            }
        }

        private void saveActivity()
        {
            bool valid = validateEntry();
            if (!valid)
            {
                return;
            }

            //Need to create the service sheet / day entities and save
            ServiceSheetViewModel serviceSheetCreated = new ServiceSheetViewModel(SelectedUser, SelectedActivity, StartDate, EndDate);
            //Now save
            serviceSheetCreated.SaveToModel();
            bool saveSuccessful = DbServiceSheet.saveSheetsAndDays(serviceSheetCreated);
            if (!saveSuccessful)
            {
                MessageBox.Show("Error saving to database.  Need to show error message!");
                return;
            }
        }

        private bool validateEntry()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("You must select an engineer.");
                return false;
            }
            else if (SelectedActivity == null)
            {
                MessageBox.Show("You must select an activity.");
                return false;
            }
            else if (StartDate == new DateTime())
            {
                MessageBox.Show("You must select a start date");
                return false;
            }
            else if (EndDate == new DateTime())
            {
                MessageBox.Show("You must select an end date");
                return false;
            }
            else if (StartDate > EndDate)
            {
                MessageBox.Show("Start date must be before end date");
                return false;
            }

            return true;
        }
    }
}
