using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using System.Collections.ObjectModel;
using System.Windows;
using Client.Helpers;
using AutoMapper;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is a client representation of a project data structure.
    /// It is an observable object and notifies its view of changes.
    /// </summary>
    public class ProjectViewModel : ObservableObject
    {

        private Project _Project;


        /// <summary>
        /// A project object is required to initialise an object of the view
        /// model. Attributes are mapped from the project object to view model.
        /// </summary>
        /// <param name="bug">Bug object data structure.</param>
        public ProjectViewModel(Project proj)
        {
            _Project = proj;
        }


        /// <summary>
        /// Converts the current view model object back into
        /// a data transfer object.
        /// </summary>
        /// <returns>A project object resulting from the mapping.</returns>
        public Project ToProjectModel()
        {
            Mapper.CreateMap<ProjectViewModel, Project>();

            return Mapper.Map<ProjectViewModel, Project>(this);
        }


        public Int32 Id
        {
            get { return _Project.Id; }
            set { _Project.Id = value; OnPropertyChanged("Id"); }
        }

        public String Name
        {
            get { return _Project.Name; }
            set { _Project.Name = value; OnPropertyChanged("Name"); }
        }

        public String Description
        {
            get { return _Project.Description; }
            set { _Project.Description = value; OnPropertyChanged("Description"); }
        }

    }
}
