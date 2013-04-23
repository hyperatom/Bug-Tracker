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
using System.Windows.Input;
using Client.Properties;

namespace Client.ViewModels
{
    /// <summary>
    /// This class controls operations concerned with the bug table.
    /// </summary>
    public class BugTableViewModel : ObservableObject, IBugTableViewModel
    {

        private int _PageSize;

        private static SortDescription _DefaultTableSortOrder;

        private ObservableRangeCollection<BugViewModel> _BugList = new ObservableRangeCollection<BugViewModel>();
        private CollectionViewSource                    _BugListViewSource = new CollectionViewSource();

        private String _ProjectTitle;
        
        private BugPanelViewModel _SouthViewPanel;
        private ProjectViewModel  _ActiveProject;

        private BugViewModel       _SelectedBug;
        private List<BugViewModel> _SelectedBugs;
        private IList<String>      _FilterList;

        private String _SelectedFilter;
        private String _SearchText;

        // Dependencies
        private IMessenger      _Messenger;
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
            _ControlFactory = ctrlfactory;

            if (activeProj != null && activeProj.Id != 0)
            {
                _ActiveProject = activeProj;
                ProjectTitle = activeProj.Name;
            }

            _SearchText = "";

            _PageSize = GetPageSizeFromSettings();

            _DefaultTableSortOrder = new SortDescription("Id", ListSortDirection.Descending);
            _BugListViewSource.Source = _BugList;

            InitSortingAndPaging();

            if (_ActiveProject != null)
                UpdateTableData();

            ListenForMessages();
        }


        public String SelectedFilter
        {
            get 
            {
                if (_SelectedFilter == null)
                    _SelectedFilter = FilterList[0];

                return _SelectedFilter;
            }
            set 
            { 
                _SelectedFilter = value;

                UpdateTableData(); 
                OnPropertyChanged("SelectedFilter");
            }
        }


        public IList<String> FilterList
        {
            get 
            {
                if (_FilterList == null)
                    _FilterList = new List<String>() { "All", "Assigned To Me", "Open", "In Progress", "Closed" };

                return _FilterList;
            }
            set { _FilterList = value; OnPropertyChanged("FilterList"); }
        }


        public String PageSizeTextBox
        {
            get { return _PageSize.ToString(); }
            set 
            {
                try
                {
                    Pager.PageSize = Int32.Parse(value);
                    Settings.Default["PageCount"] = Pager.PageSize;
                }
                catch (FormatException)
                {
                    Pager.PageSize = 20;
                }
            }
        }


        public PagingController Pager { get; private set; }


        public ICollectionView MyBugList
        {
            get { return this._BugListViewSource.View; }
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

                foreach (BugViewModel bug in _BugList)
                {
                    if (bug.IsSelected)
                        _SelectedBugs.Add(bug);
                }

                return _SelectedBugs;
            }
        }


        public String SearchText
        {
            get { return _SearchText; }
            set 
            {
                _SearchText = value;

                UpdateTableData();
                OnPropertyChanged("SearchText");
            }
        }


        /// <summary>
        /// Subscribes to incoming messages concerned with
        /// data stored in this object.
        /// </summary>
        private void ListenForMessages()
        {            
            _Messenger.Register<ProjectViewModel>(Messages.ActiveProjectChanged, ActiveProjectChanged);
            _Messenger.Register<BugViewModel>(Messages.AddPanelSavedBug, p => UpdateTableData());
            _Messenger.Register<BugViewModel>(Messages.SelectedBugDeleted, p => UpdateTableData());
            _Messenger.Register<BugViewModel>(Messages.SelectedBugSaved, p => UpdateTableData());

            _Messenger.Register(Messages.ShowBugViewPanel, ShowBugViewPanel);
            _Messenger.Register(Messages.ShowBugAddPanel, ShowBugAddPanel);
            _Messenger.Register(Messages.DeleteSelectedBugs, DeleteSelectedBugs);
            _Messenger.Register(Messages.ShowAssignedBugs, ShowAssignedBugs);
        }


        private void ShowAssignedBugs()
        {

            UpdateTableData();
        }


        private void InitSortingAndPaging()
        {
            var sortDescriptions = (INotifyCollectionChanged)_BugListViewSource.View.SortDescriptions;
            sortDescriptions.CollectionChanged += OnSortOrderChanged;

            if (_ActiveProject == null || _ActiveProject.Id == 0)
            {
                Pager = new PagingController(0, _PageSize);
            }
            else
            {
                Pager = new PagingController(_Service.CountBugsInProject(_ActiveProject.ToProjectModel()), _PageSize);
            }
            
            Pager.CurrentPageChanged += (s, e) => UpdateTableData();
        }


        private void UpdateTableData()
        {
            if (SouthViewPanel != null)
                SouthViewPanel.IsVisible = false;
            
            var currentSort = _BugListViewSource.View.SortDescriptions.DefaultIfEmpty(_DefaultTableSortOrder).ToArray();

            IList<BugViewModel> bugList = GetProjectBugs();

            bugList = OrderListBySortDescription(currentSort, bugList);

            Pager.ItemCount = bugList.Count;

            this._BugList.Clear();

            if (ListNotEmpty(bugList))
            {
                if (Pager.ItemCount - Pager.CurrentPageStartIndex < Pager.PageSize)
                {
                    _BugList.AddRange(bugList.ToList().GetRange(Pager.CurrentPageStartIndex, Pager.ItemCount - Pager.CurrentPageStartIndex));
                }
                else if (BugsLessThanPageSize(bugList))
                {
                    _BugList.AddRange(bugList.ToList().GetRange(0, bugList.Count()));
                }
                else if (Pager.CurrentPageStartIndex < 0)
                {
                    _BugList.AddRange(bugList.ToList().GetRange(0, Pager.PageSize));
                }
                else
                {
                    _BugList.AddRange(bugList.ToList().GetRange(Pager.CurrentPageStartIndex, Pager.PageSize));
                }
            }
        }


        private static bool ListNotEmpty(IList<BugViewModel> bugList)
        {
            return bugList.Count() > 0;
        }


        private bool BugsLessThanPageSize(IList<BugViewModel> bugList)
        {
            return bugList.Count() < Pager.PageSize;
        }


        private int GetPageSizeFromSettings()
        {
            return (int) Settings.Default["PageCount"];
        }


        private IList<BugViewModel> GetProjectBugs()
        {
            var data = new List<Bug>();
            int myId = _Service.GetMyUser().Id;

            data = _Service.SearchAllProjectBugsAttributes(_ActiveProject.ToProjectModel(), SearchText);
            
            switch (SelectedFilter)
            {
                case "Assigned To Me":

                    if (data.Count > 0)
                        data = data.Where(p => p.AssignedUser != null && p.AssignedUser.Id == myId).ToList();

                    break;

                case "Open":

                    if (data.Count > 0)
                        data = data.Where(p => p.Status == "Open").ToList();

                    break;

                case "Closed":
                    
                    if (data.Count > 0)
                        data = data.Where(p => p.Status == "Closed").ToList();

                    break;
            }
            
           
            IList<BugViewModel> vmList = new List<BugViewModel>();

            data.ForEach(p => vmList.Add(new BugViewModel(p)));

            return vmList;
        }


        private static IList<BugViewModel> OrderListBySortDescription(SortDescription[] currentSort, IList<BugViewModel> vmList)
        {
            foreach (SortDescription sortDescription in currentSort)
            {
                PropertyInfo propertyInfo = typeof(BugViewModel).GetProperty(sortDescription.PropertyName);
                Func<BugViewModel, object> keySelector = item => GetPropValue<object>(item, sortDescription.PropertyName);

                switch (sortDescription.Direction)
                {
                    case ListSortDirection.Ascending:
                        vmList = vmList.OrderBy(keySelector).ToList();
                        break;
                    case ListSortDirection.Descending:
                        vmList = vmList.OrderByDescending(keySelector).ToList();
                        break;
                    default:
                        continue;
                }
            }
            return vmList;
        }


        private void OnSortOrderChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                UpdateTableData();
            }
        }


        /// <summary>
        /// Removes a bug object from the table and closes the
        /// south view panel if it is open.
        /// </summary>
        /// <param name="bug">The bug to remove from the table.</param>
        private void RemoveBugFromTable(BugViewModel bug)
        {
            if (SouthViewPanel != null && SouthViewPanel.IsVisible)
                SouthViewPanel.IsVisible = false;

            UpdateTableData();
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
            ProjectTitle = proj.Name;

            UpdateTableData();
        }


        /// <summary>
        /// Deletes the user selected bugs from the web service and bug table.
        /// </summary>
        private void DeleteSelectedBugs()
        {
            // For each selected bug, remove it from the service and view
            foreach (BugViewModel bug in SelectedBugs)
            {
                // Delete using web service
                _Service.DeleteBug(bug.ToBugModel());
                // Notify listeners of delete operation
                _Messenger.NotifyColleagues(Messages.SelectedBugDeleted, bug);
            }
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

            return (T)retval;
        }
        
    }
}
