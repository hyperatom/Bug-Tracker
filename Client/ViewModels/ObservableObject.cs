using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Client.ViewModels
{
    /// <summary>
    /// This class allows an object to become observable by inheriting
    /// notification methods which can be invoked when properties change.
    /// 
    /// This class has been taken from the quoted website:
    /// http://justinmchase.com/2010/08/26/observableobject-for-wpf-mvvm/
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Sets the value of the field and notifies the view of the property that changed.
        /// </summary>
        /// <typeparam name="T">The type of the field which has changed.</typeparam>
        /// <param name="field">The field to flag changed.</param>
        /// <param name="value">The value which has been changed.</param>
        /// <param name="property">The property which has been changed.</param>
        public virtual void SetAndNotify<T>(ref T field, T value, Expression<Func<T>> property)
        {
            if (!object.ReferenceEquals(field, value))
            {
                field = value;
                this.OnPropertyChanged(property);
            }
        }


        /// <summary>
        /// Raises the property changed event on the specified property.
        /// </summary>
        /// <typeparam name="T">The type of property which has changed.</typeparam>
        /// <param name="changedProperty">The property which has changed.</param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> changedProperty)
        {
            if (PropertyChanged != null)
            {
                string name = ((MemberExpression)changedProperty.Body).Member.Name;
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
