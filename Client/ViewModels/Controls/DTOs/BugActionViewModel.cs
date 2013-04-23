using System;
using System.Collections.Generic;
using AutoMapper;
using Client.Helpers;
using Client.ServiceReference;
using Client.ViewModels.Controls.DTOs;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;


namespace Client.ViewModels
{
    /// <summary>
    /// This class is a client representation of a bug action log data structure.
    /// It is an observable object and notifies its container of changes.
    /// 
    /// </summary>
    public class BugActionLogViewModel : ObservableObject
    {

        private BugActionLog _BugActionLog;
        private ImageSource _ActionIcon;

        private const int Create_Action = 1;
        private const int Update_Action = 2;
        private const int Delete_Action = 3;


        /// <summary>
        /// A bug object is required to initialise an object of the view
        /// model. Attributes are mapped from the bug object to view model.
        /// </summary>
        /// <param name="bug">Bug object data structure.</param>
        public BugActionLogViewModel(BugActionLog log)
        {
            _BugActionLog = log;

            _ActionIcon = GetImageData(GetIconFile());
        }


        private byte[] GetIconFile()
        {
            switch (_BugActionLog.Action.Id)
            {
                case Create_Action:
                    return File.ReadAllBytes(@"..\..\..\img\add_icon.png");

                case Update_Action:
                    return File.ReadAllBytes(@"..\..\..\img\edit_icon.png");

                case Delete_Action:
                    return File.ReadAllBytes(@"..\..\..\img\delete_icon.png");
            }


            return File.ReadAllBytes(@"..\..\..\img\edit_icon.png");
        }


        private ImageSource GetImageData(byte[] data)
        {
            var source = new BitmapImage();

            source.BeginInit();
            source.StreamSource = new MemoryStream(data);
            source.EndInit();

            return source;
        }


        public Int32 Id
        {
            get { return this._BugActionLog.Id; }
            set { _BugActionLog.Id = value; OnPropertyChanged("Id"); }
        }


        public BugAction Action
        {
            get 
            {
                if (_BugActionLog.Action == null)
                    return null;

                return this._BugActionLog.Action; 
            }
            set { _BugActionLog.Action = value; OnPropertyChanged("Action"); }
        }
        

        public String BugName
        {
            get 
            {
                return _BugActionLog.BugName;
            }

            set { _BugActionLog.BugName = value; OnPropertyChanged("BugName"); }
        }

        public DateTime Date
        {
            get { return this._BugActionLog.Date; }
            set { _BugActionLog.Date = value; OnPropertyChanged("Date"); }
        }


        public String UserName
        {
            get
            {
                return _BugActionLog.UserName;
            }
            set { _BugActionLog.UserName = value; OnPropertyChanged("UserName"); }
        }


        public ImageSource ActionIcon
        {
            get { return _ActionIcon; }
            set
            {
                _ActionIcon = value;
                OnPropertyChanged("ActionIcon");
            }
        }


        public String ActionMessage
        {
            get
            {
                return UserName + " " + Action.ActionName + " " + BugName + " \n@ " + Date.ToShortDateString();
            }
        }

        
    }
}
