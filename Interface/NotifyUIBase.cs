using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RT
{
    public class NotifyUIBase : INotifyPropertyChanged
    {
        // Very minimal implementation of INotifyPropertyChanged matching msdn
        // Note that this is dependent on .net 4.5+ because of CallerMemberName
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnPropertyChanged(string prop)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(prop);
                handler(this, e);
            }
        }

    }
}
