using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Service_Reader
{
    public class JobCostSheetViewModel : ObservableObject
    {
        //RT 12/12/16 - Changing to ViewModel

        private ObservableCollection<ServiceSheetViewModel> m_allServiceSheets;
        private ICommand m_loadCsvCommand;
        private ICommand m_createCostSheetCommand;
        private ObservableCollection<ServiceSheetViewModel> m_selectedSubmissions;

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
                if (m_loadCsvCommand == null)
                {
                    m_loadCsvCommand = new RelayCommand(param => loadHistoricalDataFromCsv());
                }
                return m_loadCsvCommand;
            }

            set
            {
                m_loadCsvCommand = value;
            }
        }

        //public ObservableCollection<ServiceSheetViewModel> SelectedSubmissions
        //{
        //    get
        //    {
        //        return m_selectedSubmissions;
        //    }

        //    set
        //    {
        //        m_selectedSubmissions = value;
        //        onPropertyChanged("SelectedSubmissions");
        //    }
        //}

        public ICommand CreateCostSheetCommand
        {
            get
            {
                if (m_createCostSheetCommand == null)
                {
                    m_createCostSheetCommand = new RelayCommand(param => costSheetCreator());
                }
                return m_createCostSheetCommand;
            }

            set
            {
                m_createCostSheetCommand = value;
            }
        }

        private void loadHistoricalDataFromCsv()
        {
            //RT - This calls the import csv and loads the csv file previously created from the Canvas Submissions screen.
            CsvServiceImport importer = new CsvServiceImport();
            bool result = importer.importCsvData();
            AllServiceSheets = importer.AllServiceSubmissions;
        }

        private void costSheetCreator()
        {
            CreateCostSheet costSheetExporter = new CreateCostSheet();
            bool success = costSheetExporter.exportDataToCostSheet(SelectedSubmissions);
            MessageBox.Show("Need to check for success");
        }

        //RT 13/12/16 - This is used to return all selected rows in the datagrid
        public IEnumerable<ServiceSheetViewModel> SelectedSubmissions
        {
            get
            {
                return AllServiceSheets.Where(o => o.Selected);
            }
        }

        //public string Name
        //{ get { return "Job Cost Sheet"; } }

        //private List<oldServiceSubmissionModel> allServiceSubmissions;
        //private oldServiceSubmissionModel selectedSubmission;
        //private IList selectedSubmissions;
        //private ICommand loadCsvCommand;
        //private ICommand createCostSheetCommand;

        //public ICommand createCostSheet
        //{
        //    get
        //    {
        //        if (createCostSheetCommand == null)
        //        {
        //            createCostSheetCommand = new RelayCommand(param => this.costSheetCreator());
        //        }
        //        return createCostSheetCommand;
        //    }
        //}

        //private void costSheetCreator()
        //{
        //    CreateCostSheet costSheetExporter = new CreateCostSheet();
        //    //RT 20/8/16 - Changing creator to deal with multiple sheets
        //    //bool success = costSheetExporter.exportDataToCostSheet(selectedSubmission);
        //    //bool success = costSheetExporter.exportDataToCostSheet(selectedSubmissions);
        //    MessageBox.Show("Need to check for success");
        //}

        //public ICommand loadCsv
        //{
        //    get
        //    {
        //        if (loadCsvCommand == null)
        //        {
        //            loadCsvCommand = new RelayCommand(param => this.loadCsvData());
        //        }
        //        return loadCsvCommand;
        //    }
        //}

        //private void loadCsvData()
        //{
        //    CsvServiceImport csvImporter = new CsvServiceImport();
        //    Boolean successful = csvImporter.importCsvData();
        //    MessageBox.Show("Need to check for success!");
        //    //AllServiceSubmissions = csvImporter.AllServiceSubmissions;
        //}

        //public List<oldServiceSubmissionModel> AllServiceSubmissions
        //{
        //    get
        //    {
        //        return allServiceSubmissions;
        //    }
        //    set
        //    {
        //        if (value != allServiceSubmissions)
        //        {
        //            allServiceSubmissions = value;
        //            onPropertyChanged("AllServiceSubmissions");
        //        }
        //    }
        //}

        //public oldServiceSubmissionModel SelectedSubmission
        //{
        //    get { return selectedSubmission; }
        //    set
        //    {
        //        if (value != selectedSubmission)
        //        {
        //            selectedSubmission = value;
        //            onPropertyChanged("SelectedSubmission");
        //        }
        //    }
        //}

        //public IList SelectedSubmissions
        //{
        //    get { return selectedSubmissions; }
        //    set
        //    {
        //        if (value != selectedSubmissions)
        //        {
        //            selectedSubmissions = value;
        //            onPropertyChanged("SelectedSubmissions");
        //        }
        //    }
        //}

        //public List<oldServiceSubmissionModel> getAllSubmissions
        //{
        //    get { return allServiceSubmissions; }
        //    set
        //    {
        //        if (value != allServiceSubmissions)
        //        {
        //            allServiceSubmissions = value;
        //            onPropertyChanged("getAllSubmissions");
        //        }
        //    }
        //}

    }
}
