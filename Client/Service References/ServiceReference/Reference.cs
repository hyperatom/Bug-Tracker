﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.ServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Bug", Namespace="http://schemas.datacontract.org/2004/07/DataEntities.Entity")]
    [System.SerializableAttribute()]
    public partial class Bug : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Client.ServiceReference.User AssignedUserField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Client.ServiceReference.User CreatedByField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DateFoundField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool FixedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime LastModifiedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PriorityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Client.ServiceReference.Project ProjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Client.ServiceReference.User AssignedUser {
            get {
                return this.AssignedUserField;
            }
            set {
                if ((object.ReferenceEquals(this.AssignedUserField, value) != true)) {
                    this.AssignedUserField = value;
                    this.RaisePropertyChanged("AssignedUser");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Client.ServiceReference.User CreatedBy {
            get {
                return this.CreatedByField;
            }
            set {
                if ((object.ReferenceEquals(this.CreatedByField, value) != true)) {
                    this.CreatedByField = value;
                    this.RaisePropertyChanged("CreatedBy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DateFound {
            get {
                return this.DateFoundField;
            }
            set {
                if ((this.DateFoundField.Equals(value) != true)) {
                    this.DateFoundField = value;
                    this.RaisePropertyChanged("DateFound");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Fixed {
            get {
                return this.FixedField;
            }
            set {
                if ((this.FixedField.Equals(value) != true)) {
                    this.FixedField = value;
                    this.RaisePropertyChanged("Fixed");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime LastModified {
            get {
                return this.LastModifiedField;
            }
            set {
                if ((this.LastModifiedField.Equals(value) != true)) {
                    this.LastModifiedField = value;
                    this.RaisePropertyChanged("LastModified");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Priority {
            get {
                return this.PriorityField;
            }
            set {
                if ((object.ReferenceEquals(this.PriorityField, value) != true)) {
                    this.PriorityField = value;
                    this.RaisePropertyChanged("Priority");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Client.ServiceReference.Project Project {
            get {
                return this.ProjectField;
            }
            set {
                if ((object.ReferenceEquals(this.ProjectField, value) != true)) {
                    this.ProjectField = value;
                    this.RaisePropertyChanged("Project");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="User", Namespace="http://schemas.datacontract.org/2004/07/DataEntities.Entity")]
    [System.SerializableAttribute()]
    public partial class User : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SurnameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Surname {
            get {
                return this.SurnameField;
            }
            set {
                if ((object.ReferenceEquals(this.SurnameField, value) != true)) {
                    this.SurnameField = value;
                    this.RaisePropertyChanged("Surname");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username {
            get {
                return this.UsernameField;
            }
            set {
                if ((object.ReferenceEquals(this.UsernameField, value) != true)) {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Project", Namespace="http://schemas.datacontract.org/2004/07/DataEntities.Entity")]
    [System.SerializableAttribute()]
    public partial class Project : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Code {
            get {
                return this.CodeField;
            }
            set {
                if ((object.ReferenceEquals(this.CodeField, value) != true)) {
                    this.CodeField = value;
                    this.RaisePropertyChanged("Code");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Role", Namespace="http://schemas.datacontract.org/2004/07/DataEntities.Entity")]
    [System.SerializableAttribute()]
    public partial class Role : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RoleNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RoleName {
            get {
                return this.RoleNameField;
            }
            set {
                if ((object.ReferenceEquals(this.RoleNameField, value) != true)) {
                    this.RoleNameField = value;
                    this.RaisePropertyChanged("RoleName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.ITrackerService")]
    public interface ITrackerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetAllBugs", ReplyAction="http://tempuri.org/ITrackerService/GetAllBugsResponse")]
        System.Collections.Generic.List<Client.ServiceReference.Bug> GetAllBugs();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/DeleteBugs", ReplyAction="http://tempuri.org/ITrackerService/DeleteBugsResponse")]
        bool DeleteBugs(System.Collections.Generic.List<Client.ServiceReference.Bug> bugList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/DeleteBug", ReplyAction="http://tempuri.org/ITrackerService/DeleteBugResponse")]
        void DeleteBug(Client.ServiceReference.Bug bug);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetProjectsAssignedTo", ReplyAction="http://tempuri.org/ITrackerService/GetProjectsAssignedToResponse")]
        System.Collections.Generic.List<Client.ServiceReference.Project> GetProjectsAssignedTo(Client.ServiceReference.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetAllProjectsByUser", ReplyAction="http://tempuri.org/ITrackerService/GetAllProjectsByUserResponse")]
        System.Collections.Generic.List<Client.ServiceReference.Project> GetAllProjectsByUser(Client.ServiceReference.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetBugsByProject", ReplyAction="http://tempuri.org/ITrackerService/GetBugsByProjectResponse")]
        System.Collections.Generic.List<Client.ServiceReference.Bug> GetBugsByProject(Client.ServiceReference.Project project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/SaveBug", ReplyAction="http://tempuri.org/ITrackerService/SaveBugResponse")]
        Client.ServiceReference.Bug SaveBug(Client.ServiceReference.Bug bug);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/AddBug", ReplyAction="http://tempuri.org/ITrackerService/AddBugResponse")]
        Client.ServiceReference.Bug AddBug(Client.ServiceReference.Bug bug);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetMyUser", ReplyAction="http://tempuri.org/ITrackerService/GetMyUserResponse")]
        Client.ServiceReference.User GetMyUser();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetBugPriorityList", ReplyAction="http://tempuri.org/ITrackerService/GetBugPriorityListResponse")]
        System.Collections.Generic.List<string> GetBugPriorityList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetUsersByProject", ReplyAction="http://tempuri.org/ITrackerService/GetUsersByProjectResponse")]
        System.Collections.Generic.List<Client.ServiceReference.User> GetUsersByProject(Client.ServiceReference.Project proj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetAssignedUsersByProject", ReplyAction="http://tempuri.org/ITrackerService/GetAssignedUsersByProjectResponse")]
        System.Collections.Generic.List<Client.ServiceReference.User> GetAssignedUsersByProject(Client.ServiceReference.Project proj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetManagerUsersByProject", ReplyAction="http://tempuri.org/ITrackerService/GetManagerUsersByProjectResponse")]
        System.Collections.Generic.List<Client.ServiceReference.User> GetManagerUsersByProject(Client.ServiceReference.Project proj);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetBugStatusList", ReplyAction="http://tempuri.org/ITrackerService/GetBugStatusListResponse")]
        System.Collections.Generic.List<string> GetBugStatusList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetProjectsManagedBy", ReplyAction="http://tempuri.org/ITrackerService/GetProjectsManagedByResponse")]
        System.Collections.Generic.List<Client.ServiceReference.Project> GetProjectsManagedBy(Client.ServiceReference.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/AddProject", ReplyAction="http://tempuri.org/ITrackerService/AddProjectResponse")]
        Client.ServiceReference.Project AddProject(Client.ServiceReference.Project project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/SaveProject", ReplyAction="http://tempuri.org/ITrackerService/SaveProjectResponse")]
        Client.ServiceReference.Project SaveProject(Client.ServiceReference.Project project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/DeleteProject", ReplyAction="http://tempuri.org/ITrackerService/DeleteProjectResponse")]
        void DeleteProject(Client.ServiceReference.Project project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/LeaveProject", ReplyAction="http://tempuri.org/ITrackerService/LeaveProjectResponse")]
        void LeaveProject(Client.ServiceReference.Project project, Client.ServiceReference.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/RequestProjectAssignment", ReplyAction="http://tempuri.org/ITrackerService/RequestProjectAssignmentResponse")]
        void RequestProjectAssignment(string code, Client.ServiceReference.User user, Client.ServiceReference.Role role);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetAllRoles", ReplyAction="http://tempuri.org/ITrackerService/GetAllRolesResponse")]
        System.Collections.Generic.List<Client.ServiceReference.Role> GetAllRoles();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetProjectByCode", ReplyAction="http://tempuri.org/ITrackerService/GetProjectByCodeResponse")]
        Client.ServiceReference.Project GetProjectByCode(string projectCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/GetUsersPendingProjectJoin", ReplyAction="http://tempuri.org/ITrackerService/GetUsersPendingProjectJoinResponse")]
        System.Collections.Generic.List<Client.ServiceReference.User> GetUsersPendingProjectJoin(Client.ServiceReference.Project project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/AcceptUserOnProject", ReplyAction="http://tempuri.org/ITrackerService/AcceptUserOnProjectResponse")]
        void AcceptUserOnProject(Client.ServiceReference.User user, Client.ServiceReference.Project project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/RejectUserFromProject", ReplyAction="http://tempuri.org/ITrackerService/RejectUserFromProjectResponse")]
        void RejectUserFromProject(Client.ServiceReference.User user, Client.ServiceReference.Project project);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITrackerService/IsValidProjectCode", ReplyAction="http://tempuri.org/ITrackerService/IsValidProjectCodeResponse")]
        bool IsValidProjectCode(string code);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ITrackerServiceChannel : Client.ServiceReference.ITrackerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TrackerServiceClient : System.ServiceModel.ClientBase<Client.ServiceReference.ITrackerService>, Client.ServiceReference.ITrackerService {
        
        public TrackerServiceClient() {
        }
        
        public TrackerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TrackerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TrackerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TrackerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.Bug> GetAllBugs() {
            return base.Channel.GetAllBugs();
        }
        
        public bool DeleteBugs(System.Collections.Generic.List<Client.ServiceReference.Bug> bugList) {
            return base.Channel.DeleteBugs(bugList);
        }
        
        public void DeleteBug(Client.ServiceReference.Bug bug) {
            base.Channel.DeleteBug(bug);
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.Project> GetProjectsAssignedTo(Client.ServiceReference.User user) {
            return base.Channel.GetProjectsAssignedTo(user);
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.Project> GetAllProjectsByUser(Client.ServiceReference.User user) {
            return base.Channel.GetAllProjectsByUser(user);
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.Bug> GetBugsByProject(Client.ServiceReference.Project project) {
            return base.Channel.GetBugsByProject(project);
        }
        
        public Client.ServiceReference.Bug SaveBug(Client.ServiceReference.Bug bug) {
            return base.Channel.SaveBug(bug);
        }
        
        public Client.ServiceReference.Bug AddBug(Client.ServiceReference.Bug bug) {
            return base.Channel.AddBug(bug);
        }
        
        public Client.ServiceReference.User GetMyUser() {
            return base.Channel.GetMyUser();
        }
        
        public System.Collections.Generic.List<string> GetBugPriorityList() {
            return base.Channel.GetBugPriorityList();
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.User> GetUsersByProject(Client.ServiceReference.Project proj) {
            return base.Channel.GetUsersByProject(proj);
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.User> GetAssignedUsersByProject(Client.ServiceReference.Project proj) {
            return base.Channel.GetAssignedUsersByProject(proj);
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.User> GetManagerUsersByProject(Client.ServiceReference.Project proj) {
            return base.Channel.GetManagerUsersByProject(proj);
        }
        
        public System.Collections.Generic.List<string> GetBugStatusList() {
            return base.Channel.GetBugStatusList();
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.Project> GetProjectsManagedBy(Client.ServiceReference.User user) {
            return base.Channel.GetProjectsManagedBy(user);
        }
        
        public Client.ServiceReference.Project AddProject(Client.ServiceReference.Project project) {
            return base.Channel.AddProject(project);
        }
        
        public Client.ServiceReference.Project SaveProject(Client.ServiceReference.Project project) {
            return base.Channel.SaveProject(project);
        }
        
        public void DeleteProject(Client.ServiceReference.Project project) {
            base.Channel.DeleteProject(project);
        }
        
        public void LeaveProject(Client.ServiceReference.Project project, Client.ServiceReference.User user) {
            base.Channel.LeaveProject(project, user);
        }
        
        public void RequestProjectAssignment(string code, Client.ServiceReference.User user, Client.ServiceReference.Role role) {
            base.Channel.RequestProjectAssignment(code, user, role);
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.Role> GetAllRoles() {
            return base.Channel.GetAllRoles();
        }
        
        public Client.ServiceReference.Project GetProjectByCode(string projectCode) {
            return base.Channel.GetProjectByCode(projectCode);
        }
        
        public System.Collections.Generic.List<Client.ServiceReference.User> GetUsersPendingProjectJoin(Client.ServiceReference.Project project) {
            return base.Channel.GetUsersPendingProjectJoin(project);
        }
        
        public void AcceptUserOnProject(Client.ServiceReference.User user, Client.ServiceReference.Project project) {
            base.Channel.AcceptUserOnProject(user, project);
        }
        
        public void RejectUserFromProject(Client.ServiceReference.User user, Client.ServiceReference.Project project) {
            base.Channel.RejectUserFromProject(user, project);
        }
        
        public bool IsValidProjectCode(string code) {
            return base.Channel.IsValidProjectCode(code);
        }
    }
}
