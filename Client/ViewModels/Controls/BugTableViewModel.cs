using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client.Helpers;
using Client.ServiceReference;
using Client.Factories;
using Client.ViewModels.Controls;
using System.ComponentModel;
using System.Windows.Data;
using Client.Helpers.Paging;
using System.Collections.Specialized;
using System.Windows;
using System.Reflection;

namespace Client.ViewModels
{
    /// <summary>
    /// This class controls operations concerned with the bug table.
    /// </summary>
    public class BugTableViewModel : ObservableObject, IBugTableViewModel
    {

        private const int _PageSize = 2;

        private static SortDescription DefaultSortOrder = new SortDescription("Id", ListSortDirection.Ascending);

        private ObservableRangeCollection<BugViewModel> bugs = new ObservableRangeCollection<BugViewModel>();

        private CollectionViewSource bugsViewSource = new CollectionViewSource();


        private String _ProjectTitle;

        private BugViewModel _SelectedBug;
        private List<BugViewModel> _SelectedBugs;

        private ObservableCollection<BugViewModel> _BugList;
        private BugPanelViewModel _SouthViewPanel;

        private ProjectViewModel _ActiveProject;

        // Dependencies
        private IMessenger _Messenger;
        private ITrackerService _Service;
        private IControlFactory _ControlFactory;


        /// <summary>
        /// Stores dependencies and initialises selected bug list. Also
        /// sets up a message listener to recieve incoming messages.
        /// </summary>
        /// <param name="comm">The mediator for communicatin with other view models.</param>
        /// <param name="svc">The bug tracker web service.</param>
        /// <param name="activeProj">The currently active project</param>
        public BugTableViewModel(IMessenger comm, ITrackerService svc, 
                                 IControlFactory ctrlfactory, ProjectViewModel activeProj)
        {
            _Messenger = comm;
            _Service = svc;
            _ActiveProject = activeProj;
            _ControlFactory = ctrlfactory;

            ProjectTitle = activeProj.Name;

            this.bugsViewSource.Source = this.bugs;

            var sortDescriptions = (INotifyCollectionChanged)this.bugsViewSource.View.SortDescriptions;
            sortDescriptions.CollectionChanged += this.OnSortOrderChanged;

            // The 5000 here is the total number of reservations
            this.Pager = new PagingController(4, _PageSize);
            this.Pager.CurrentPageChanged += (s, e) => this.UpdateData();

            this.UpdateData();

            ListenForMessages();
        }


        public PagingController Pager { get; private set; }

        private void UpdateData()
        {
            var currentSort = this.bugsViewSource.View.SortDescriptions.DefaultIfEmpty(DefaultSortOrder).ToArray();

            var data = _Service.GetBugsByProject(_ActiveProject.ToProjectModel());
            IList<BugViewModel> vmList = new List<BugViewModel>();

            IEnumerable<BugViewModel> orderedList = new List<BugViewModel>();

            data.ForEach(p => vmList.Add(new BugViewModel(p)));

            foreach (SortDescription sortDescription in currentSort)
            {
                PropertyInfo propertyInfo = typeof(BugViewModel).GetProperty(sortDescription.PropertyName);
                Func<BugViewModel, object> keySelector = item => GetPropValue<object>(item, sortDescription.PropertyName);

                switch (sortDescription.Direction)
                {
                    case ListSortDirection.Ascending:
                        orderedList = vmList.OrderBy(keySelector);
                        break;
                    case ListSortDirection.Descending:
                        orderedList = vmList.OrderByDescending(keySelector);
                        break;
                    default:
                        continue;
                }
            }

            this.bugs.Clear();

            bugs.AddRange(orderedList.ToList().GetRange(Pager.CurrentPageStartIndex, Pager.PageSize));
        }


        public PropertyInfo GetProp(Type baseType, string propertyName)
        {
            string[] parts = propertyName.Split('.');

            return (parts.Length > 1) ? GetProp(baseType.GetProperty(parts[0]).PropertyType, parts.Skip(1).Aggregate((a, i) => a + "." + i)) : baseType.GetProperty(propertyName);
        }


        public static Object GetPropValue(Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static T GetPropValue<T>(Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }


        public ICollectionView MyBugList
        {
            get { return this.bugsViewSource.View; }
        }

        private void OnSortOrderChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                this.UpdateData();
            }
        }


        /// <summary>
        /// The title of the currently active project.
        /// </summary>
        public String ProjectTitle
        {
            get { return _ProjectTitle; }
            set
            {
                _ProjectTitle = value + " Bugs";
                OnPropertyChanged("ProjectTitle");
            }
        }


        public BugPanelViewModel SouthViewPanel
        {
            get
            {
                return _SouthViewPanel;
            }

            set
            {
                _SouthViewPanel = value;
                OnPropertyChanged("SouthViewPanel");
            }
        }


        /// <summary>
        /// Stores the bug view model which the user has currently selected
        /// and notifies listeners of the change.
        /// </summary>
        public BugViewModel SelectedBug
        {
            get
            {
                return _SelectedBug;
            }

            set
            {
                _SelectedBug = value;

                _Messenger.NotifyColleagues(Messages.SelectedBugChanged, value);

                OnPropertyChanged("SelectedBug");
            }
        }


        /// <summary>
        /// Derives the currently selected bugs by searching for
        /// bugs which have IsSelected property set to true.
        /// </summary>
        public List<BugViewModel> SelectedBugs
        {
            get
            {
                _SelectedBugs = new List<BugViewModel>();

                foreach (BugViewModel bug in BugList)
                {
                    if (bug.IsSelected)
                        _SelectedBugs.Add(bug);
                }

                return _SelectedBugs;
            }
        }


        /// <summary>
        /// Observable collection of bug view models contained
        /// within the bug table.
        /// </summary>
        public ObservableCollection<BugViewModel> BugList
        {
            get
            {
                if (_BugList == null)
                {
                    _BugList = new ObservableCollection<BugViewModel>();
                }

                return _BugList;
            }

            set
            {
                _BugList = value;
                OnPropertyChanged("BugList");
            }
        }


        /// <summary>
        /// Populates the table of bugs according to currently selected project.
        /// </summary>
        public void PopulateBugTable()
        {
            if (_ActiveProject != null)
            {
                BugList.Clear();

                _Service.GetBugsByProject(_ActiveProject.ToProjectModel()).ForEach(p => BugList.Add(new BugViewModel(p)));
            }
        }


        /// <summary>
        /// Subscribes to incoming messages concerned with
        /// data stored in this object.
        /// </summary>
        private void ListenForMessages()
        {            
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, ActiveProjectChanged);
            _Messenger.Register<BugViewModel>(Messages.AddPanelSavedBug, p => _BugList.Add(p));
            _Messenger.Register<BugViewModel>(Messages.SelectedBugDeleted, RemoveBugFromTable);
            _Messenger.Register<BugViewModel>(Messages.SelectedBugSaved, SaveBugToTable);

            _Messenger.Register(Messages.ShowBugViewPanel, ShowBugViewPanel);
            _Messenger.Register(Messages.ShowBugAddPanel, ShowBugAddPanel);
            _Messenger.Register(Messages.RequestSelectedBugs, SendSelectedBugs);
        }


        /// <summary>
        /// Removes a bug object from the table and closes the
        /// south view panel if it is open.
        /// </summary>
        /// <param name="bug">The bug to remove from the table.</param>
        private void RemoveBugFromTable(BugViewModel bug)
        {
            _BugList.Remove(bug);

            if (SouthViewPanel != null && SouthViewPanel.IsVisible)
                SouthViewPanel.IsVisible = false;
        }


        /// <summary>
        /// Notifies listeners when the selected bug list changes.
        /// </summary>
        private void SendSelectedBugs()
        {
            _Messenger.NotifyColleagues(Messages.SelectedBugsChanged, SelectedBugs);
        }


        /// <summary>
        /// Saves a bug object to the table.
        /// </summary>
        /// <param name="bug">The bug object to save.</param>
        private void SaveBugToTable(BugViewModel bug)
        {
            BugViewModel selectedBug = BugList.Where(p => p.Id == bug.Id).SingleOrDefault();

            int index = BugList.IndexOf(selectedBug);

            BugList.Remove(selectedBug);
            BugList.Insert(index, bug);
            SelectedBug = BugList.ElementAt(index);
        }


        /// <summary>
        /// Creates a new bug editing panel and sets it visible.
        /// </summary>
        private void ShowBugViewPanel()
        {
            SouthViewPanel = _ControlFactory.CreateBugViewPanel(_ActiveProject, _SelectedBug);
            SouthViewPanel.IsVisible = true;
        }


        /// <summary>
        /// Creates a new bug add panel and sets it visible.
        /// </summary>
        private void ShowBugAddPanel()
        {
            SouthViewPanel = _ControlFactory.CreateBugAddPanel(_ActiveProject);
            SouthViewPanel.IsVisible = true;
        }


        /// <summary>
        /// Updates object data when the currently active project
        /// is changed.
        /// </summary>
        /// <param name="proj">The new active project.</param>
        private void ActiveProjectChanged(ProjectViewModel proj)
        {
            _ActiveProject = proj;
            PopulateBugTable();
            ProjectTitle = proj.Name;
        }
        
    }
}
