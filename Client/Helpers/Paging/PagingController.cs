namespace Client.Helpers.Paging
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Utility class that helps coordinate paged access to a data store.
    /// </summary>
    public sealed class PagingController : ObservableObject
    {
        /// <summary>
        /// The count of items to be divided into pages.
        /// </summary>
        private int itemCount;

        /// <summary>
        /// The current page.
        /// </summary>
        private int currentPage;

        /// <summary>
        /// The length (number of items) of each page.
        /// </summary>
        private int pageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingController"/> class.
        /// </summary>
        /// <param name="itemCount">The item count.</param>
        /// <param name="pageSize">The size of each page.</param>
        public PagingController(int itemCount, int pageSize)
        {
            Contract.Requires(itemCount >= 0);
            Contract.Requires(pageSize > 0);

            this.itemCount = itemCount;
            this.pageSize = pageSize;
            this.currentPage = this.itemCount == 0 ? 0 : 1;

            this.GotoFirstPageCommand = new RelayCommand(p => this.CurrentPage = 1, p => this.ItemCount != 0 && this.CurrentPage > 1);
            this.GotoLastPageCommand = new RelayCommand(p => this.CurrentPage = this.PageCount, p => this.ItemCount != 0 && this.CurrentPage < this.PageCount);
            this.GotoNextPageCommand = new RelayCommand(p => ++this.CurrentPage, p => this.ItemCount != 0 && this.CurrentPage < this.PageCount);
            this.GotoPreviousPageCommand = new RelayCommand(p => --this.CurrentPage, p => this.ItemCount != 0 && this.CurrentPage > 1);
        }

        /// <summary>
        /// Occurs when the value of <see cref="CurrentPage"/> changes.
        /// </summary>
        public event EventHandler<CurrentPageChangedEventArgs> CurrentPageChanged;

        /// <summary>
        /// Gets the command that, when executed, sets <see cref="CurrentPage"/> to 1.
        /// </summary>
        /// <value>The command that changes the current page.</value>
        public ICommand GotoFirstPageCommand { get; private set; }

        /// <summary>
        /// Gets the command that, when executed, decrements <see cref="CurrentPage"/> by 1.
        /// </summary>
        /// <value>The command that changes the current page.</value>
        public ICommand GotoPreviousPageCommand { get; private set; }

        /// <summary>
        /// Gets the command that, when executed, increments <see cref="CurrentPage"/> by 1.
        /// </summary>
        /// <value>The command that changes the current page.</value>
        public ICommand GotoNextPageCommand { get; private set; }

        /// <summary>
        /// Gets the command that, when executed, sets <see cref="CurrentPage"/> to <see cref="PageCount"/>.
        /// </summary>
        /// <value>The command that changes the current page.</value>
        public ICommand GotoLastPageCommand { get; private set; }

        /// <summary>
        /// Gets or sets the total number of items to be divided into pages.
        /// </summary>
        /// <value>The item count.</value>
        public int ItemCount
        {
            get
            {
                return this.itemCount;
            }

            set
            {
                Contract.Requires(value >= 0);

                this.itemCount = value;
                OnPropertyChanged("ItemCount");
                OnPropertyChanged("PageCount");
                RaiseCanExecuteChanged(this.GotoLastPageCommand, this.GotoNextPageCommand);

                if (this.CurrentPage > this.PageCount)
                {
                    this.CurrentPage = this.PageCount;
                }
                else if (CurrentPage == 0 && PageCount > 0)
                {
                    this.CurrentPage = 1;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of items that each page contains.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return this.pageSize;
            }

            set
            {
                Contract.Requires(value > 0);

                var oldStartIndex = this.CurrentPageStartIndex;
                this.pageSize = value;
                OnPropertyChanged("PageSize");
                OnPropertyChanged("PageCount");
                OnPropertyChanged("CurrentPageStartIndex");
                RaiseCanExecuteChanged(this.GotoLastPageCommand, this.GotoNextPageCommand);

                if (oldStartIndex >= 0)
                {
                    this.CurrentPage = this.GetPageFromIndex(oldStartIndex);
                }
            }
        }

        /// <summary>
        /// Gets the number of pages required to contain all items.
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() == 0 || this.itemCount > 0);
                Contract.Ensures(Contract.Result<int>() > 0 || this.itemCount == 0);

                if (this.itemCount == 0)
                {
                    return 0;
                }

                var ceil = (int)Math.Ceiling((double)this.itemCount / this.pageSize);

                Contract.Assume(ceil > 0); // Math.Ceiling makes the static checker unable to prove the postcondition without help
                return ceil;
            }
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            get
            {
                return this.currentPage;
            }

            set
            {
                Contract.Requires(value == 0 || this.PageCount != 0);
                Contract.Requires(value > 0 || this.PageCount == 0);
                Contract.Requires(value <= this.PageCount);

                this.currentPage = value;
                OnPropertyChanged("CurrentPage");
                OnPropertyChanged("CurrentPageStartIndex");
                RaiseCanExecuteChanged(this.GotoLastPageCommand, this.GotoNextPageCommand);
                RaiseCanExecuteChanged(this.GotoFirstPageCommand, this.GotoPreviousPageCommand);

                var handler = this.CurrentPageChanged;
                if (handler != null)
                {
                    handler(this, new CurrentPageChangedEventArgs(this.CurrentPageStartIndex, this.PageSize));
                }
            }
        }

        /// <summary>
        /// Gets the index of the first item belonging to the current page.
        /// </summary>
        /// <value>The index of the first item belonging to the current page.</value>
        public int CurrentPageStartIndex
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() == -1 || this.PageCount != 0);
                Contract.Ensures(Contract.Result<int>() >= 0 || this.PageCount == 0);
                Contract.Ensures(Contract.Result<int>() < this.ItemCount);
                return this.PageCount == 0 ? -1 : (this.CurrentPage - 1) * this.PageSize;
            }
        }

        /// <summary>
        /// Calls RaiseCanExecuteChanged on any number of DelegateCommand instances.
        /// </summary>
        /// <param name="commands">The commands.</param>
        [Pure]
        private static void RaiseCanExecuteChanged(params ICommand[] commands)
        {
            Contract.Requires(commands != null);
            foreach (var command in commands.Cast<RelayCommand>())
            {
                command.CanExecute(command) ;
            }
        }

        /// <summary>
        /// Gets the number of the page to which the item with the specified index belongs.
        /// </summary>
        /// <param name="itemIndex">The index of the item in question.</param>
        /// <returns>The number of the page in which the item with the specified index belongs.</returns>
        [Pure]
        private int GetPageFromIndex(int itemIndex)
        {
            Contract.Requires(itemIndex >= 0);
            Contract.Requires(itemIndex < this.itemCount);
            Contract.Ensures(Contract.Result<int>() >= 1);
            Contract.Ensures(Contract.Result<int>() <= this.PageCount);

            var result = (int)Math.Floor((double)itemIndex / this.PageSize) + 1;
            Contract.Assume(result >= 1); // Math.Floor makes the static checker unable to prove the postcondition without help
            Contract.Assume(result <= this.PageCount); // Ditto
            return result;
        }

        /// <summary>
        /// Defines the invariant for object of this class.
        /// </summary>
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.currentPage == 0 || this.PageCount != 0);
            Contract.Invariant(this.currentPage > 0 || this.PageCount == 0);
            Contract.Invariant(this.currentPage <= this.PageCount);
            Contract.Invariant(this.pageSize > 0);
            Contract.Invariant(this.itemCount >= 0);
        }
    }
}