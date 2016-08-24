using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class JobCostSheetViewModel : ObservableObject, IPageViewModel
    {
        public string Name
        { get { return "Job Cost Sheet"; } }

        private List<ServiceSubmissionModel> allServiceSubmissions;
        private ServiceSubmissionModel selectedSubmission;
        private IList selectedSubmissions;
        private ICommand loadCsvCommand;
        private ICommand createCostSheetCommand;

        public ICommand createCostSheet
        {
            get
            {
                if (createCostSheetCommand == null)
                {
                    createCostSheetCommand = new RelayCommand(param => this.costSheetCreator());
                }
                return createCostSheetCommand;
            }
        }

        private void costSheetCreator()
        {
            CreateCostSheet costSheetExporter = new CreateCostSheet();
            //RT 20/8/16 - Changing creator to deal with multiple sheets
            //bool success = costSheetExporter.exportDataToCostSheet(selectedSubmission);
            bool success = costSheetExporter.exportDataToCostSheet(selectedSubmissions);
            MessageBox.Show("Need to check for success");
        }

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

        public List<ServiceSubmissionModel> AllServiceSubmissions
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

        public ServiceSubmissionModel SelectedSubmission
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

        public IList SelectedSubmissions
        {
            get { return selectedSubmissions; }
            set
            {
                if (value != selectedSubmissions)
                {
                    selectedSubmissions = value;
                    onPropertyChanged("SelectedSubmissions");
                }
            }
        }

        public List<ServiceSubmissionModel> getAllSubmissions
        {
            get { return allServiceSubmissions; }
            set
            {
                if (value != allServiceSubmissions)
                {
                    allServiceSubmissions = value;
                    onPropertyChanged("getAllSubmissions");
                }
            }
        }

    }
}
