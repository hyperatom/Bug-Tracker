using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Helpers
{
    /// <summary>
    /// Collection of messages which objects can use to communicate. Objects wishing
    /// to communicate data must use the same message object as an identifier.
    /// </summary>
    public static class Messages
    {
        public static readonly string AddPanelSavedBug = "Add Panel Saved Bug";
        public static readonly string WebServiceReferenceUpdated = "Web Service Refernce Updated";
        public static readonly string ActiveProjectChanged = "Active Project Changed";
        public static readonly string ShowBugViewPanel = "Show Bug View Panel";
        public static readonly string ShowBugAddPanel = "Show Bug Add Panel";
        public static readonly string SelectedBugsChanged = "Selected Bugs Change";
        public static readonly string SelectedBugChanged = "Selected Bug Change";
        public static readonly string SelectedBugDeleted = "Selected Bug Deleted";
        public static readonly string SelectedBugSaved = "Selected Bug Saved";
        public static readonly string RequestSelectedBugs = "Request Selected Bugs";
        public static readonly string SavedProject = "Saved Project";
        public static readonly string AddedProject = "Added Project";
        public static readonly string RequestDeleteProject = "Request Delete Project";
        public static readonly string DeletedProject = "Deleted Project";
        public static readonly string CloseDeleteProjectDialog = "Close Delete Dialog";
        public static readonly string ShowProjectViewPanel = "Show Project View Panel";
        public static readonly string ShowProjectAddPanel = "Show Project Add Panel";
        public static readonly string ShowProjectDeleteDialog = "Show Project Delete Dialog";
        public static readonly string ProjectSelected = "Project Selected";
        public static readonly string ManagedProjectSelected = "Managed Project Selected";
        public static readonly string AssignedProjectSelected = "Assigned Project Selected";
        public static readonly string BugTableDisplaying = "Bug Table Displaying";
        public static readonly string ShowAssignedBugs = "Show Assigned Bugs";
        public static readonly string DeleteSelectedBugs = "Delete Selected Bugs";
    }
}
