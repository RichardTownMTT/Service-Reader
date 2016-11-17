using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader 
{
    public class ServiceSheetViewModel : ObservableObject
    {
        private ServiceSheet m_serviceSubmission;

        public ServiceSheet ServiceSubmission
        {
            get
            {
                return m_serviceSubmission;
            }

            set
            {
                m_serviceSubmission = value;
                onPropertyChanged("ServiceSubmission");
            }
        }
        
    }
}
