﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.ServiceReference;
using Client.Commands;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.ViewModels
{
    /// <summary>
    /// This class is the view model which controls the main bug
    /// view window.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase<MainWindow>
    {

        /// <summary>
        /// Inherits constructor from base class.
        /// </summary>
        public MainWindowViewModel() : base() { }


        /// <summary>
        /// Stores reference to window controller as global variable.
        /// </summary>
        /// <param name="controller">Reference to window controller object.</param>
        public MainWindowViewModel(IWindowController controller, TrackerServiceClient service)
            : base(controller, service) { }


        /// <summary>
        /// Stores the bug view model which the user has currently selected.
        /// </summary>
        private BugViewModel _SelectedBug;
        public BugViewModel SelectedBug
        {
            get { return _SelectedBug; }
            set { _SelectedBug = value; }
        }


        /// <summary>
        /// Observable collection of bug view models contained
        /// within the bug table.
        /// </summary>
        private ObservableCollection<BugViewModel> _bugList;
        public ObservableCollection<BugViewModel> BugList
        {
            get
            {
                if (_bugList == null)
                {
                    _bugList = new ObservableCollection<BugViewModel>();
                }

                return _bugList;
            }
            
            set { _bugList = value; }
        }


        /// <summary>
        /// Displays the south view panel containing details of bug attributes.
        /// </summary>
        private RelayCommand _EditBugCommand;
        public ICommand EditBugCommand
        {
            get
            {
                if (_EditBugCommand == null)
                {
                    _EditBugCommand = new RelayCommand(param => this.ToggleBugView(), param => this.IsRowsSelected);
                }

                return _EditBugCommand;
            }
        }


        private RelayCommand _DeBugCommand;
        public ICommand DeBugCommand
        {
            get
            {
                if (_DeBugCommand == null)
                {
                    _DeBugCommand = new RelayCommand(param => this.Debug());
                }

                return _DeBugCommand;
            }
        }


        private RelayCommand _NewBugCommand;
        public ICommand NewBugCommand
        {
            get
            {
                if (_NewBugCommand == null)
                {
                    _NewBugCommand = new RelayCommand(param => this.OpenNewBugView());
                }

                return _NewBugCommand;
            }
        }


        /// <summary>
        /// When add new bug button is clicked, show the south view panel
        /// and reset the bug currently being viewed. Hide the save button
        /// and display the add button.
        /// </summary>
        private void OpenNewBugView()
        {
            if (!IsBugViewVisible()) ToggleBugView();

            _View.buttonSave.Visibility = Visibility.Hidden;
            _View.buttonAdd.Visibility = Visibility.Visible;

            if (SelectedBug == null)
            {
                SelectedBug = new BugViewModel(new Bug());
            }
            else
            {
                ResetTextBoxesContent();
            }
            

            ResetTextBoxBindings();
            User myUser = (User) _Service.GetMyUser();
            SelectedBug.CreatedBy = myUser;
            SelectedBug.CreatedBy.Username = myUser.Username;
            SelectedBug.Priority = "meep";
        }


        /// <summary>
        /// Resets the text box bindings to allow the content to become
        /// rebound with the selected bug.
        /// </summary>
        private void ResetTextBoxBindings()
        {
            Binding nameTextBinding = new Binding("Name");
            Binding descTextBinding = new Binding("Description");
            Binding statusTextBinding = new Binding("Status");
            Binding foundTextBinding = new Binding("DateFound");
            Binding modifiedTextBinding = new Binding("LastModified");
            Binding fixedTextBinding = new Binding("Fixed");
            Binding createdByTextBinding = new Binding("CreatedBy");
            Binding priorityTextBinding = new Binding("Priority");

            nameTextBinding.Source = SelectedBug;
            descTextBinding.Source = SelectedBug;
            statusTextBinding.Source = SelectedBug;
            foundTextBinding.Source = SelectedBug;
            modifiedTextBinding.Source = SelectedBug;
            fixedTextBinding.Source = SelectedBug;
            priorityTextBinding.Source = SelectedBug;

            _View.textBoxName.SetBinding(TextBox.TextProperty, nameTextBinding);
            _View.textBoxDesc.SetBinding(TextBox.TextProperty, descTextBinding);
            _View.textBoxStatus.SetBinding(TextBox.TextProperty, statusTextBinding);
            _View.textBoxFound.SetBinding(TextBox.TextProperty, foundTextBinding);
            _View.textBoxModified.SetBinding(TextBox.TextProperty, modifiedTextBinding);
            _View.checkBoxFixed.SetBinding(TextBox.TextProperty, fixedTextBinding);
            _View.textBoxCreatedBy.SetBinding(TextBox.TextProperty, createdByTextBinding);
            _View.textBoxPriority.SetBinding(TextBox.TextProperty, priorityTextBinding);
        }


        /// <summary>
        /// Resets the content in south view panel text boxes.
        /// </summary>
        private void ResetTextBoxesContent()
        {
            _View.textBoxCreatedBy.Text = "";
            _View.textBoxDesc.Text = "";
            _View.textBoxFound.Text = "";
            _View.textBoxModified.Text = "";
            _View.textBoxName.Text = "";
            _View.textBoxPriority.Text = "";
            _View.textBoxStatus.Text = "";
        }


        private RelayCommand _SaveBugCommand;
        public ICommand SaveBugCommand
        {
            get
            {
                if (_SaveBugCommand == null)
                {
                    _SaveBugCommand = new RelayCommand(param => this.SaveBug(param));
                }

                return _SaveBugCommand;
            }
        }


        /// <summary>
        /// Saves a bug which has been editied.
        /// </summary>
        /// <param name="bug">The object which has been edited.</param>
        private void SaveBug(object bug)
        {
            if (bug is BugViewModel)
            {
                BugViewModel vm = (BugViewModel)bug;
                _Service.SaveBug(vm.ToBugModel());
            }
        }


        private RelayCommand _AddBugCommand;
        public ICommand AddBugCommand
        {
            get
            {
                if (_AddBugCommand == null)
                {
                    _AddBugCommand = new RelayCommand(param => this.AddBug());
                }

                return _AddBugCommand;
            }
        }


        private void AddBug()
        {
            _Service.AddBug(SelectedBug.ToBugModel());
            BugList.Add(SelectedBug);
        }


        private void Debug()
        {
            if (SelectedBug == null)
            {
                MessageBox.Show("Selected bug is null!");
            }
            else
            {
                MessageBox.Show(SelectedBug.Name+SelectedBug.Priority+SelectedBug.Status+SelectedBug.Description);
            }
        }


        /// <summary>
        /// Populates the table of bugs according to currently selected project.
        /// </summary>
        private void PopulateBugTable()
        {
            if (SelectedActiveProject != null)
            {
                BugList.Clear();
                List<Bug> bugList = _Service.GetBugsByProject(SelectedActiveProject.ToProjectModel());

                foreach (Bug bug in bugList)
                {
                    BugList.Add(new BugViewModel(bug));
                }
            }
        }


        /// <summary>
        /// The list of user assigned projects to be displayed in combo box.
        /// </summary>
        private ObservableCollection<ProjectViewModel> _ProjectComboBox;
        public ObservableCollection<ProjectViewModel> ProjectComboBox
        {
            get {
                if (_ProjectComboBox == null)
                {
                    _ProjectComboBox = ProjectModelToViewModel(_Service.GetMyProjects().ToList());
                }

                return _ProjectComboBox;
            }

            set { _ProjectComboBox = value; }
        }


        /// <summary>
        /// Stores the currently selected project from combo box.
        /// </summary>
        private ProjectViewModel _SelectedActiveProject;
        public ProjectViewModel SelectedActiveProject
        {
            get { return _SelectedActiveProject; }
            set 
            {
                _SelectedActiveProject = value;
                PopulateBugTable();
            }
        }


        /// <summary>
        /// Converts a list of project objects to an observable collection of projects.
        /// </summary>
        /// <param name="list">The list of project objects.</param>
        /// <returns>The result of the mappings between project and view model objects.</returns>
        private ObservableCollection<ProjectViewModel> ProjectModelToViewModel(List<Project> list)
        {
            ObservableCollection<ProjectViewModel> obsProjects = new ObservableCollection<ProjectViewModel>();

            foreach (Project proj in list)
            {
                obsProjects.Add(new ProjectViewModel(proj));
            }

            return obsProjects;
        }


        /// <summary>
        /// Checks if the south view panel is currently open or closed.
        /// </summary>
        /// <returns>Returns true if visible.</returns>
        private bool IsBugViewVisible()
        {
            if (_View.BugView.Visibility == System.Windows.Visibility.Hidden)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Toggles the visibility of the south view panel.
        /// </summary>
        private void ToggleBugView()
        {
            if (IsBugViewVisible() == false)
            {
                _View.BugView.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                _View.BugView.Visibility = System.Windows.Visibility.Hidden;
            }
        }


        /// <summary>
        /// Initialises the command to delete the users selected bugs.
        /// </summary>
        private RelayCommand _DeleteSelectedBugsCommand;
        public ICommand DeleteSelectedBugsCommand
        {
            get
            {
                if (_DeleteSelectedBugsCommand == null)
                {
                    _DeleteSelectedBugsCommand = new RelayCommand(param => this.DeleteSelectedBugs(),
                                                                  param => this.IsRowsSelected);
                }

                return _DeleteSelectedBugsCommand;
            }
        }


        /// <summary>
        /// The state of row selection. True if more than one rows are selected.
        /// </summary>
        private bool IsRowsSelected
        {
            get 
            {
                if (_View != null && _View.dataGrid1.SelectedItems.Count == 0)
                {
                    return false;
                }

                return true;
            }

            set { }
        }


        /// <summary>
        /// Deletes the user selected bugs from the web service and bug table.
        /// </summary>
        private void DeleteSelectedBugs()
        {
            // Copy and cast selected bugs into a list of bug view models
            List<BugViewModel> bugVmList = _View.dataGrid1.SelectedItems.Cast<BugViewModel>().ToList();

            // For each selected bug, remove it from the service and view
            foreach (BugViewModel bug in bugVmList)
            {
                // Delete using web service
                _Service.DeleteBug(bug.ToBugModel());
                // Delete from local bug view
                BugList.Remove(bug);
            }

            // If south view panel open then close it
            if (IsBugViewVisible() == true) { ToggleBugView(); }
        }

    }
}
