using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using Client.ServiceReference;

namespace Client.Helpers
{

    /// <summary>
    /// Abstracts the property changed logic from view models.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Notifies the view of changes to data which has been
        /// binded to.
        /// </summary>
        /// <param name="name">Name of the field which has changed.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
