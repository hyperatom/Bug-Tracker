﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.ServiceRegistration {
    using System.Runtime.Serialization;
    using System;
    
    
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceRegistration.IRegistrationService")]
    public interface IRegistrationService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRegistrationService/Register", ReplyAction="http://tempuri.org/IRegistrationService/RegisterResponse")]
        void Register(Client.ServiceRegistration.User user);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRegistrationService/Register", ReplyAction="http://tempuri.org/IRegistrationService/RegisterResponse")]
        System.IAsyncResult BeginRegister(Client.ServiceRegistration.User user, System.AsyncCallback callback, object asyncState);
        
        void EndRegister(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRegistrationService/UserExists", ReplyAction="http://tempuri.org/IRegistrationService/UserExistsResponse")]
        bool UserExists(string username);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRegistrationService/UserExists", ReplyAction="http://tempuri.org/IRegistrationService/UserExistsResponse")]
        System.IAsyncResult BeginUserExists(string username, System.AsyncCallback callback, object asyncState);
        
        bool EndUserExists(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRegistrationServiceChannel : Client.ServiceRegistration.IRegistrationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UserExistsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public UserExistsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegistrationServiceClient : System.ServiceModel.ClientBase<Client.ServiceRegistration.IRegistrationService>, Client.ServiceRegistration.IRegistrationService {
        
        private BeginOperationDelegate onBeginRegisterDelegate;
        
        private EndOperationDelegate onEndRegisterDelegate;
        
        private System.Threading.SendOrPostCallback onRegisterCompletedDelegate;
        
        private BeginOperationDelegate onBeginUserExistsDelegate;
        
        private EndOperationDelegate onEndUserExistsDelegate;
        
        private System.Threading.SendOrPostCallback onUserExistsCompletedDelegate;
        
        public RegistrationServiceClient() {
        }
        
        public RegistrationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RegistrationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegistrationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegistrationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> RegisterCompleted;
        
        public event System.EventHandler<UserExistsCompletedEventArgs> UserExistsCompleted;
        
        public void Register(Client.ServiceRegistration.User user) {
            base.Channel.Register(user);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginRegister(Client.ServiceRegistration.User user, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRegister(user, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public void EndRegister(System.IAsyncResult result) {
            base.Channel.EndRegister(result);
        }
        
        private System.IAsyncResult OnBeginRegister(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Client.ServiceRegistration.User user = ((Client.ServiceRegistration.User)(inValues[0]));
            return this.BeginRegister(user, callback, asyncState);
        }
        
        private object[] OnEndRegister(System.IAsyncResult result) {
            this.EndRegister(result);
            return null;
        }
        
        private void OnRegisterCompleted(object state) {
            if ((this.RegisterCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RegisterCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RegisterAsync(Client.ServiceRegistration.User user) {
            this.RegisterAsync(user, null);
        }
        
        public void RegisterAsync(Client.ServiceRegistration.User user, object userState) {
            if ((this.onBeginRegisterDelegate == null)) {
                this.onBeginRegisterDelegate = new BeginOperationDelegate(this.OnBeginRegister);
            }
            if ((this.onEndRegisterDelegate == null)) {
                this.onEndRegisterDelegate = new EndOperationDelegate(this.OnEndRegister);
            }
            if ((this.onRegisterCompletedDelegate == null)) {
                this.onRegisterCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRegisterCompleted);
            }
            base.InvokeAsync(this.onBeginRegisterDelegate, new object[] {
                        user}, this.onEndRegisterDelegate, this.onRegisterCompletedDelegate, userState);
        }
        
        public bool UserExists(string username) {
            return base.Channel.UserExists(username);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginUserExists(string username, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginUserExists(username, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public bool EndUserExists(System.IAsyncResult result) {
            return base.Channel.EndUserExists(result);
        }
        
        private System.IAsyncResult OnBeginUserExists(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string username = ((string)(inValues[0]));
            return this.BeginUserExists(username, callback, asyncState);
        }
        
        private object[] OnEndUserExists(System.IAsyncResult result) {
            bool retVal = this.EndUserExists(result);
            return new object[] {
                    retVal};
        }
        
        private void OnUserExistsCompleted(object state) {
            if ((this.UserExistsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.UserExistsCompleted(this, new UserExistsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void UserExistsAsync(string username) {
            this.UserExistsAsync(username, null);
        }
        
        public void UserExistsAsync(string username, object userState) {
            if ((this.onBeginUserExistsDelegate == null)) {
                this.onBeginUserExistsDelegate = new BeginOperationDelegate(this.OnBeginUserExists);
            }
            if ((this.onEndUserExistsDelegate == null)) {
                this.onEndUserExistsDelegate = new EndOperationDelegate(this.OnEndUserExists);
            }
            if ((this.onUserExistsCompletedDelegate == null)) {
                this.onUserExistsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnUserExistsCompleted);
            }
            base.InvokeAsync(this.onBeginUserExistsDelegate, new object[] {
                        username}, this.onEndUserExistsDelegate, this.onUserExistsCompletedDelegate, userState);
        }
    }
}
