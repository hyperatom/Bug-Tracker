using System.Collections.ObjectModel;
using System.ComponentModel;
using System;

namespace Client.Views.Controls.Notifications
{
    public class Notification : INotifyPropertyChanged
    {

        public static String ICON_NOTIFICATION = "pack://application:,,,/Resources/notification-icon.png";
        public static String ICON_EDIT = "pack://application:,,,/Resources/edit_bug.png";
        public static String ICON_ADD = "pack://application:,,,/Resources/add_bug.png";
        public static String ICON_DELETE = "pack://application:,,,/Resources/delete_bug.png";
        public static String ICON_TICK = "pack://application:,,,/Resources/tick.png";


        private string message;
        public string Message
        {
            get { return message; }

            set
            {
                if (message == value) return;
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private int id;
        public int Id
        {
            get { return id; }

            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }

            set
            {
                if (imageUrl == value) return;
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }

            set
            {
                if (title == value) return;
                title = value;
                OnPropertyChanged("Title");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Notifications : ObservableCollection<Notification> { }
}