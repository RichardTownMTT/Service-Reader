using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class ApplicationViewModel : ObservableObject
    {
        private ICommand changePageCommand;
        private IPageViewModel currentPageViewModel;
        private List<IPageViewModel> allPageViewModels;

        public ApplicationViewModel()
        {
            AllPageViewModels.Add(new SubmissionViewModel());
            allPageViewModels.Add(new IssueServiceReportViewModel());
            allPageViewModels.Add(new HistoryTrackerViewModel());
            allPageViewModels.Add(new JobCostSheetViewModel());
            currentPageViewModel = AllPageViewModels[0];
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (changePageCommand == null)
                {
                    changePageCommand = new RelayCommand(p => ChangeViewModel((IPageViewModel)p), p => p is IPageViewModel);
                }
                return changePageCommand;
            }
        }

        public List<IPageViewModel> AllPageViewModels
        {
            get
            {
                if (allPageViewModels == null)
                {
                    allPageViewModels = new List<IPageViewModel>();
                }
                return allPageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return currentPageViewModel;
            }
            set
            {
                if (allPageViewModels != value)
                {
                    currentPageViewModel = value;
                    onPropertyChanged("CurrentPageViewModel");
                }
            }
        }
        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!AllPageViewModels.Contains(viewModel))
                AllPageViewModels.Add(viewModel);

            CurrentPageViewModel = AllPageViewModels.FirstOrDefault(vm => vm == viewModel);
        }
    }
}
