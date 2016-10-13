using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class HistoryTrackerViewModel : ObservableObject, IPageViewModel
    {
        public String Name
        { get { return "History Tracker"; } }

        private List<oldServiceSubmissionModel> allServiceSubmissions;
        private oldServiceSubmissionModel selectedSubmission;
        private ICommand loadCsvCommand;

        public ICommand loadCsv
        {
            get
            {
                if (loadCsvCommand == null)
                {
                    loadCsvCommand = new RelayCommand(param => this.loadCsvData());
                }
                return loadCsvCommand;
            }
        }

        private void loadCsvData()
        {
            CsvServiceImport csvImporter = new CsvServiceImport();
            Boolean successful = csvImporter.importCsvData();
            MessageBox.Show("Need to check for success!");
            AllServiceSubmissions = csvImporter.AllServiceSubmissions;
        }

        public List<oldServiceSubmissionModel> AllServiceSubmissions
        {
            get
            {
                return allServiceSubmissions;
            }
            set
            {
                if (value != allServiceSubmissions)
                {
                    allServiceSubmissions = value;
                    onPropertyChanged("AllServiceSubmissions");
                }
            }
        }

        public oldServiceSubmissionModel SelectedSubmission
        {
            get { return selectedSubmission; }
            set
            {
                if (value != selectedSubmission)
                {
                    selectedSubmission = value;
                    onPropertyChanged("SelectedSubmission");
                }
            }
        }
    }
}
