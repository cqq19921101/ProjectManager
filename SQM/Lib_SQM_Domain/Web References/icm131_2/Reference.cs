﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 原始程式碼已由 Microsoft.VSDesigner 自動產生，版本 4.0.30319.42000。
// 
#pragma warning disable 1591

namespace Lib_SQM_Domain.icm131_2 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SQM_AccountServiceSoap", Namespace="http://tempuri.org/")]
    public partial class SQM_AccountService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SQM_CreateAccountOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SQM_AccountService() {
            this.Url = global::Lib_SQM_Domain.Properties.Settings.Default.Lib_SQM_Domain_icm131_2_SQM_AccountService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event SQM_CreateAccountCompletedEventHandler SQM_CreateAccountCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SQM_CreateAccount", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int SQM_CreateAccount(string AuthorizedID, string NewUserID, string NewUserPassword, string RoleID, string MailAddr) {
            object[] results = this.Invoke("SQM_CreateAccount", new object[] {
                        AuthorizedID,
                        NewUserID,
                        NewUserPassword,
                        RoleID,
                        MailAddr});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void SQM_CreateAccountAsync(string AuthorizedID, string NewUserID, string NewUserPassword, string RoleID, string MailAddr) {
            this.SQM_CreateAccountAsync(AuthorizedID, NewUserID, NewUserPassword, RoleID, MailAddr, null);
        }
        
        /// <remarks/>
        public void SQM_CreateAccountAsync(string AuthorizedID, string NewUserID, string NewUserPassword, string RoleID, string MailAddr, object userState) {
            if ((this.SQM_CreateAccountOperationCompleted == null)) {
                this.SQM_CreateAccountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSQM_CreateAccountOperationCompleted);
            }
            this.InvokeAsync("SQM_CreateAccount", new object[] {
                        AuthorizedID,
                        NewUserID,
                        NewUserPassword,
                        RoleID,
                        MailAddr}, this.SQM_CreateAccountOperationCompleted, userState);
        }
        
        private void OnSQM_CreateAccountOperationCompleted(object arg) {
            if ((this.SQM_CreateAccountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SQM_CreateAccountCompleted(this, new SQM_CreateAccountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    public delegate void SQM_CreateAccountCompletedEventHandler(object sender, SQM_CreateAccountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2558.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SQM_CreateAccountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SQM_CreateAccountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591