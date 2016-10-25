using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    class CanvasUserViewModel : ObservableObject
    {
        private CanvasUserModel m_canvasUser;

        public CanvasUserModel CanvasUser
        {
            get
            {
                return m_canvasUser;
            }

            set
            {
                m_canvasUser = value;
                onPropertyChanged("CanvasUser");
            }
        }
    }
}
