using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class HistoryTrackerViewModel : ObservableObject
    {
        private ObservableCollection<ServiceSheetViewModel> m_allServiceSheets;
        private ICommand loadCsvCommand;
        private ServiceSheetViewModel m_selectedSubmission;
        


        public ObservableCollection<ServiceSheetViewModel> AllServiceSheets
        {
            get
            {
                return m_allServiceSheets;
            }

            set
            {
                m_allServiceSheets = value;
                onPropertyChanged("AllServiceSheets");
            }
        }

        public ICommand LoadCsvCommand
        {
            get
            {
                if (loadCsvCommand == null)
                {
                    loadCsvCommand = new RelayCommand(param => loadHistoricalDataFromCsv());
                }
                return loadCsvCommand;
            }

            set
            {
                loadCsvCommand = value;
            }
        }

        public ServiceSheetViewModel SelectedSubmission
        {
            get
            {
                return m_selectedSubmission;
            }

            set
            {
                m_selectedSubmission = value;
                onPropertyChanged("SelectedSubmission");
            }
        }

        private void loadHistoricalDataFromCsv()
        {
            //RT - This calls the import csv and loads the csv file previously created from the Canvas Submissions screen.
            CsvServiceImport importer = new CsvServiceImport();
            bool result = importer.importCsvData();
            AllServiceSheets = importer.AllServiceSubmissions;
        }
    }
}
