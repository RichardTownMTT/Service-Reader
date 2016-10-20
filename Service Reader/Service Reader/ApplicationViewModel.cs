using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class ApplicationViewModel : ObservableObject
    {
        //RT 20/10/16 - Creating a command for each view switch
        //private ICommand changeViewCommand;
        private ICommand m_processCanvasDataCommand;
        private ICommand m_issueServiceReportsCommand;
        private ICommand m_historyTrackerCommand;
        private ICommand m_jobCostSheetCommand;
        private object m_currentPageView;
        private List<object> m_allPageViews;

        public ApplicationViewModel()
        {
            AllPageViews.Add(new CanvasSubmissionsView());
            AllPageViews.Add(new IssueServiceReportsView());
            AllPageViews.Add(new HistoryTrackerView());
            AllPageViews.Add(new JobCostSheetView());
            CurrentPageView = AllPageViews[0];
        }

        //public ICommand ChangeViewCommand
        //{
        //    get
        //    {
        //        if (changeViewCommand== null)
        //        {
        //            changeViewCommand = new RelayCommand(p => ChangeViewModel(p), p => p is object);
        //        }
        //        return changeViewCommand;
        //    }
        //}

        public List<object> AllPageViews
        {
            get
            {
                if (m_allPageViews == null)
                {
                    m_allPageViews = new List<object>();
                }
                return m_allPageViews;
            }
        }

        public object CurrentPageView
        {
            get
            {
                return m_currentPageView;
            }
            set
            {
                if (m_currentPageView != value)
                {
                    m_currentPageView = value;
                    onPropertyChanged("CurrentPageView");
                }
            }
        }

        public ICommand ProcessCanvasDataCommand
        {
            get
            {
                if (m_processCanvasDataCommand == null)
                {
                    m_processCanvasDataCommand = new RelayCommand(param => selectCanvasDataView());
                }
                return m_processCanvasDataCommand;
            }

            set
            {
                m_processCanvasDataCommand = value;
            }
        }

        public ICommand IssueServiceReportsCommand
        {
            get
            {
                if (m_issueServiceReportsCommand == null)
                {
                    m_issueServiceReportsCommand = new RelayCommand(param => selectIssueServiceReportsView());
                }
                return m_issueServiceReportsCommand;
            }

            set
            {
                m_issueServiceReportsCommand = value;
            }
        }

        public ICommand HistoryTrackerCommand
        {
            get
            {
                if (m_historyTrackerCommand == null)
                {
                    m_historyTrackerCommand = new RelayCommand(param => selectHistoryTrackerView());
                }
                return m_historyTrackerCommand;
            }

            set
            {
                m_historyTrackerCommand = value;
            }
        }

        public ICommand JobCostSheetCommand
        {
            get
            {
                if (m_jobCostSheetCommand == null)
                {
                    m_jobCostSheetCommand = new RelayCommand(param => selectJobCostSheetView());
                }
                return m_jobCostSheetCommand;
            }

            set
            {
                m_jobCostSheetCommand = value;
            }
        }

        private void selectJobCostSheetView()
        {
            CurrentPageView = AllPageViews[3];
        }

        private void selectHistoryTrackerView()
        {
            CurrentPageView = AllPageViews[2];
        }

        private void selectIssueServiceReportsView()
        {
            CurrentPageView = AllPageViews[1];
        }

        private void selectCanvasDataView()
        {
            CurrentPageView = AllPageViews[0];
        }

        private void ChangeViewModel(object view)
        {
            if (!AllPageViews.Contains(view))
                AllPageViews.Add(view);

            CurrentPageView = AllPageViews.FirstOrDefault(vm => vm == view);
        }
    }
}
