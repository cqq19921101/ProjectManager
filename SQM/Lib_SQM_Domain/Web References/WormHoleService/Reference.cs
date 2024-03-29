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
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Lib_SQM_Domain.WormHoleService
{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "WormHoleWebServiceSoapBinding", Namespace = "http://gms021/axis/services/WormHoleWebService")]
    public partial class WormHoleServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback testOperationCompleted;

        private System.Threading.SendOrPostCallback callOutBoundRscByTimeOperationCompleted;

        private System.Threading.SendOrPostCallback callOutBoundRscByKeyOperationCompleted;

        private System.Threading.SendOrPostCallback callOutBoundSpecByTimeOperationCompleted;

        private System.Threading.SendOrPostCallback callOutBoundSpecByKeyOperationCompleted;

        private System.Threading.SendOrPostCallback callOutBoundFileByKeyOperationCompleted;

        private System.Threading.SendOrPostCallback callOutBoundFileByTimeOperationCompleted;

        private System.Threading.SendOrPostCallback getAccountUrlOperationCompleted;

        private System.Threading.SendOrPostCallback updateAccountEmailOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public WormHoleServiceService()
        {
            this.Url = global::Lib_SQM_Domain.Properties.Settings.Default.Lib_SQM_Domain_WormHoleService_WormHoleServiceService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event testCompletedEventHandler testCompleted;

        /// <remarks/>
        public event callOutBoundRscByTimeCompletedEventHandler callOutBoundRscByTimeCompleted;

        /// <remarks/>
        public event callOutBoundRscByKeyCompletedEventHandler callOutBoundRscByKeyCompleted;

        /// <remarks/>
        public event callOutBoundSpecByTimeCompletedEventHandler callOutBoundSpecByTimeCompleted;

        /// <remarks/>
        public event callOutBoundSpecByKeyCompletedEventHandler callOutBoundSpecByKeyCompleted;

        /// <remarks/>
        public event callOutBoundFileByKeyCompletedEventHandler callOutBoundFileByKeyCompleted;

        /// <remarks/>
        public event callOutBoundFileByTimeCompletedEventHandler callOutBoundFileByTimeCompleted;

        /// <remarks/>
        public event getAccountUrlCompletedEventHandler getAccountUrlCompleted;

        /// <remarks/>
        public event updateAccountEmailCompletedEventHandler updateAccountEmailCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("testReturn")]
        public string test(string startTime, [System.Xml.Serialization.SoapElementAttribute("test")] string[] test1)
        {
            object[] results = this.Invoke("test", new object[] {
                        startTime,
                        test1});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void testAsync(string startTime, string[] test1)
        {
            this.testAsync(startTime, test1, null);
        }

        /// <remarks/>
        public void testAsync(string startTime, string[] test1, object userState)
        {
            if ((this.testOperationCompleted == null))
            {
                this.testOperationCompleted = new System.Threading.SendOrPostCallback(this.OntestOperationCompleted);
            }
            this.InvokeAsync("test", new object[] {
                        startTime,
                        test1}, this.testOperationCompleted, userState);
        }

        private void OntestOperationCompleted(object arg)
        {
            if ((this.testCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.testCompleted(this, new testCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("callOutBoundRscByTimeReturn")]
        public string callOutBoundRscByTime(string startTime, string endTime)
        {
            object[] results = this.Invoke("callOutBoundRscByTime", new object[] {
                        startTime,
                        endTime});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void callOutBoundRscByTimeAsync(string startTime, string endTime)
        {
            this.callOutBoundRscByTimeAsync(startTime, endTime, null);
        }

        /// <remarks/>
        public void callOutBoundRscByTimeAsync(string startTime, string endTime, object userState)
        {
            if ((this.callOutBoundRscByTimeOperationCompleted == null))
            {
                this.callOutBoundRscByTimeOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallOutBoundRscByTimeOperationCompleted);
            }
            this.InvokeAsync("callOutBoundRscByTime", new object[] {
                        startTime,
                        endTime}, this.callOutBoundRscByTimeOperationCompleted, userState);
        }

        private void OncallOutBoundRscByTimeOperationCompleted(object arg)
        {
            if ((this.callOutBoundRscByTimeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callOutBoundRscByTimeCompleted(this, new callOutBoundRscByTimeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("callOutBoundRscByKeyReturn")]
        public string callOutBoundRscByKey(string itemNo)
        {
            object[] results = this.Invoke("callOutBoundRscByKey", new object[] {
                        itemNo});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void callOutBoundRscByKeyAsync(string itemNo)
        {
            this.callOutBoundRscByKeyAsync(itemNo, null);
        }

        /// <remarks/>
        public void callOutBoundRscByKeyAsync(string itemNo, object userState)
        {
            if ((this.callOutBoundRscByKeyOperationCompleted == null))
            {
                this.callOutBoundRscByKeyOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallOutBoundRscByKeyOperationCompleted);
            }
            this.InvokeAsync("callOutBoundRscByKey", new object[] {
                        itemNo}, this.callOutBoundRscByKeyOperationCompleted, userState);
        }

        private void OncallOutBoundRscByKeyOperationCompleted(object arg)
        {
            if ((this.callOutBoundRscByKeyCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callOutBoundRscByKeyCompleted(this, new callOutBoundRscByKeyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("callOutBoundSpecByTimeReturn")]
        public string callOutBoundSpecByTime(string startTime, string endTime)
        {
            object[] results = this.Invoke("callOutBoundSpecByTime", new object[] {
                        startTime,
                        endTime});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void callOutBoundSpecByTimeAsync(string startTime, string endTime)
        {
            this.callOutBoundSpecByTimeAsync(startTime, endTime, null);
        }

        /// <remarks/>
        public void callOutBoundSpecByTimeAsync(string startTime, string endTime, object userState)
        {
            if ((this.callOutBoundSpecByTimeOperationCompleted == null))
            {
                this.callOutBoundSpecByTimeOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallOutBoundSpecByTimeOperationCompleted);
            }
            this.InvokeAsync("callOutBoundSpecByTime", new object[] {
                        startTime,
                        endTime}, this.callOutBoundSpecByTimeOperationCompleted, userState);
        }

        private void OncallOutBoundSpecByTimeOperationCompleted(object arg)
        {
            if ((this.callOutBoundSpecByTimeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callOutBoundSpecByTimeCompleted(this, new callOutBoundSpecByTimeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("callOutBoundSpecByKeyReturn")]
        public string callOutBoundSpecByKey(string itemNo)
        {
            object[] results = this.Invoke("callOutBoundSpecByKey", new object[] {
                        itemNo});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void callOutBoundSpecByKeyAsync(string itemNo)
        {
            this.callOutBoundSpecByKeyAsync(itemNo, null);
        }

        /// <remarks/>
        public void callOutBoundSpecByKeyAsync(string itemNo, object userState)
        {
            if ((this.callOutBoundSpecByKeyOperationCompleted == null))
            {
                this.callOutBoundSpecByKeyOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallOutBoundSpecByKeyOperationCompleted);
            }
            this.InvokeAsync("callOutBoundSpecByKey", new object[] {
                        itemNo}, this.callOutBoundSpecByKeyOperationCompleted, userState);
        }

        private void OncallOutBoundSpecByKeyOperationCompleted(object arg)
        {
            if ((this.callOutBoundSpecByKeyCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callOutBoundSpecByKeyCompleted(this, new callOutBoundSpecByKeyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("callOutBoundFileByKeyReturn")]
        public string callOutBoundFileByKey(string itemNo)
        {
            object[] results = this.Invoke("callOutBoundFileByKey", new object[] {
                        itemNo});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void callOutBoundFileByKeyAsync(string itemNo)
        {
            this.callOutBoundFileByKeyAsync(itemNo, null);
        }

        /// <remarks/>
        public void callOutBoundFileByKeyAsync(string itemNo, object userState)
        {
            if ((this.callOutBoundFileByKeyOperationCompleted == null))
            {
                this.callOutBoundFileByKeyOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallOutBoundFileByKeyOperationCompleted);
            }
            this.InvokeAsync("callOutBoundFileByKey", new object[] {
                        itemNo}, this.callOutBoundFileByKeyOperationCompleted, userState);
        }

        private void OncallOutBoundFileByKeyOperationCompleted(object arg)
        {
            if ((this.callOutBoundFileByKeyCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callOutBoundFileByKeyCompleted(this, new callOutBoundFileByKeyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("callOutBoundFileByTimeReturn")]
        public string callOutBoundFileByTime(string startTime, string endTime)
        {
            object[] results = this.Invoke("callOutBoundFileByTime", new object[] {
                        startTime,
                        endTime});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void callOutBoundFileByTimeAsync(string startTime, string endTime)
        {
            this.callOutBoundFileByTimeAsync(startTime, endTime, null);
        }

        /// <remarks/>
        public void callOutBoundFileByTimeAsync(string startTime, string endTime, object userState)
        {
            if ((this.callOutBoundFileByTimeOperationCompleted == null))
            {
                this.callOutBoundFileByTimeOperationCompleted = new System.Threading.SendOrPostCallback(this.OncallOutBoundFileByTimeOperationCompleted);
            }
            this.InvokeAsync("callOutBoundFileByTime", new object[] {
                        startTime,
                        endTime}, this.callOutBoundFileByTimeOperationCompleted, userState);
        }

        private void OncallOutBoundFileByTimeOperationCompleted(object arg)
        {
            if ((this.callOutBoundFileByTimeCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.callOutBoundFileByTimeCompleted(this, new callOutBoundFileByTimeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("getAccountUrlReturn")]
        public string getAccountUrl(string account)
        {
            object[] results = this.Invoke("getAccountUrl", new object[] {
                        account});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void getAccountUrlAsync(string account)
        {
            this.getAccountUrlAsync(account, null);
        }

        /// <remarks/>
        public void getAccountUrlAsync(string account, object userState)
        {
            if ((this.getAccountUrlOperationCompleted == null))
            {
                this.getAccountUrlOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAccountUrlOperationCompleted);
            }
            this.InvokeAsync("getAccountUrl", new object[] {
                        account}, this.getAccountUrlOperationCompleted, userState);
        }

        private void OngetAccountUrlOperationCompleted(object arg)
        {
            if ((this.getAccountUrlCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAccountUrlCompleted(this, new getAccountUrlCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace = "http://api.whs.com", ResponseNamespace = "http://gms021/axis/services/WormHoleWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("updateAccountEmailReturn")]
        public string updateAccountEmail(string account, string email)
        {
            object[] results = this.Invoke("updateAccountEmail", new object[] {
                        account,
                        email});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void updateAccountEmailAsync(string account, string email)
        {
            this.updateAccountEmailAsync(account, email, null);
        }

        /// <remarks/>
        public void updateAccountEmailAsync(string account, string email, object userState)
        {
            if ((this.updateAccountEmailOperationCompleted == null))
            {
                this.updateAccountEmailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupdateAccountEmailOperationCompleted);
            }
            this.InvokeAsync("updateAccountEmail", new object[] {
                        account,
                        email}, this.updateAccountEmailOperationCompleted, userState);
        }

        private void OnupdateAccountEmailOperationCompleted(object arg)
        {
            if ((this.updateAccountEmailCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.updateAccountEmailCompleted(this, new updateAccountEmailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void testCompletedEventHandler(object sender, testCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class testCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal testCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void callOutBoundRscByTimeCompletedEventHandler(object sender, callOutBoundRscByTimeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callOutBoundRscByTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal callOutBoundRscByTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void callOutBoundRscByKeyCompletedEventHandler(object sender, callOutBoundRscByKeyCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callOutBoundRscByKeyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal callOutBoundRscByKeyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void callOutBoundSpecByTimeCompletedEventHandler(object sender, callOutBoundSpecByTimeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callOutBoundSpecByTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal callOutBoundSpecByTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void callOutBoundSpecByKeyCompletedEventHandler(object sender, callOutBoundSpecByKeyCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callOutBoundSpecByKeyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal callOutBoundSpecByKeyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void callOutBoundFileByKeyCompletedEventHandler(object sender, callOutBoundFileByKeyCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callOutBoundFileByKeyCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal callOutBoundFileByKeyCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void callOutBoundFileByTimeCompletedEventHandler(object sender, callOutBoundFileByTimeCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class callOutBoundFileByTimeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal callOutBoundFileByTimeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void getAccountUrlCompletedEventHandler(object sender, getAccountUrlCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAccountUrlCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getAccountUrlCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void updateAccountEmailCompletedEventHandler(object sender, updateAccountEmailCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class updateAccountEmailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal updateAccountEmailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591