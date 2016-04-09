using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Service_Reader
{
    //This class handles the property change notification
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, eventArgs);
            }
        }
    }
}
