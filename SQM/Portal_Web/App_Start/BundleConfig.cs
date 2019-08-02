using System.Web;
using System.Web.Optimization;

namespace Portal_Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //=========================
            //Original by Trevor: 03/09/2015
            //This project: 02/22/2016
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));
            //=========================

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryjqgrid").Include(
                //"~/Scripts/jquery.jqGrid.js",
                "~/Scripts/jquery.jqGrid.min.js",
                "~/Scripts/i18n/grid.locale-en.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            "~/Scripts/jquery.inputmask/inputmask.js",
            "~/Scripts/jquery.inputmask/jquery.inputmask.js",
            "~/Scripts/jquery.inputmask/inputmask.extensions.js",
            "~/Scripts/jquery.inputmask/inputmask.date.extensions.js",
            //and other extensions you want to include
            "~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jquerycookie").Include(
            //    "~/Scripts/jquery-cookie.js",
            //"~/Scripts/jquery-cookie-{version}.js"));

            /* ============================
            //Removed by Trevor: 03/09/2015
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            ============================ */

            //=========================
            //Add by Trevor: 03/09/2015
            //This project: 02/22/2016
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/jquery.jqGrid/ui.jqgrid.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/*.css"));

            bundles.Add(new ScriptBundle("~/Scripts/MegaMenu/js").Include(
                "~/Scripts/MegaMenu/jquery.hoverIntent.minified.js",
                "~/Scripts/MegaMenu/jquery.dcmegamenu.1.3.3.(min).js",
                "~/Scripts/MegaMenu/mminitjs.js"));

            //Uploadify
            //=========================
            //Add by Trevor: 03/09/2015
            //This project: 02/22/2016
            // temporary remarked (can un-remarked when required this component)
            bundles.Add(new ScriptBundle("~/bundles/UploadifyF32/js").Include(
                "~/Scripts/UploadifyF32/UploadFiles_UploadifyF32.js"));
            bundles.Add(new StyleBundle("~/bundles/UploadifyF32/css").Include(
                "~/Scripts/UploadifyF32/uploadify.css"));
            //=========================

            //_layout
            bundles.Add(new ScriptBundle("~/bundles/Global/Commonjs").Include(
                "~/Scripts/_Global/*.js",
                "~/Scripts/json2.js"));
            bundles.Add(new StyleBundle("~/bundles/Global/Commoncss").Include(
                "~/Scripts/_Global/CommonUIStyle.css"));

            bundles.Add(new StyleBundle("~/bundles/SQMBasic/Basicinfo/css").Include(
                "~/Scripts/SQMBasic/Basicinfo/BasicInfo.css"));

            //Account/Login
            bundles.Add(new ScriptBundle("~/bundles/Account/Login/js").Include(
                "~/Scripts/Account/Login/*.js"));
            //"~/Scripts/Account/Login/LoginPage.js"));
            bundles.Add(new StyleBundle("~/bundles/Account/Login/css").Include(
                "~/Scripts/Account/Login/LoginPage.css"));

            //Account/Activate
            bundles.Add(new ScriptBundle("~/bundles/Account/Activate").Include(
                "~/Scripts/Account/Activate/*.js"));

            //Account/ForgotPassword
            bundles.Add(new ScriptBundle("~/bundles/Account/ForgotPassword").Include(
                "~/Scripts/Account/ForgotPassword/*.js"));

            //SystemMgmt/MemberManagement
            bundles.Add(new ScriptBundle("~/bundles/SystemMgmt/MemberManagement").Include(
                "~/Scripts/SystemMgmt/MemberManagement/*.js"));

            //SubMemberManagement
            bundles.Add(new ScriptBundle("~/bundles/SubMemberManagement").Include(
                "~/Scripts/SubMemberManagement/*.js"));

            //SystemMgmt/MenuStructureMgmt
            bundles.Add(new ScriptBundle("~/bundles/SystemMgmt/MenuStructureMgmt").Include(
                "~/Scripts/SystemMgmt/MenuStructureMgmt/*.js"));

            //SystemMgmt/RoleManagement
            bundles.Add(new ScriptBundle("~/bundles/SystemMgmt/RoleManagement").Include(
                "~/Scripts/SystemMgmt/RoleManagement/*.js"));

            //SystemMgmt/RoleMemberAndDelegation
            bundles.Add(new ScriptBundle("~/bundles/SystemMgmt/RoleMemberAndDelegation").Include(
                "~/Scripts/SystemMgmt/RoleMemberAndDelegation/*.js"));

            //SystemMgmt/RunAs
            bundles.Add(new ScriptBundle("~/bundles/SystemMgmt/RunAs").Include(
                "~/Scripts/SystemMgmt/RunAs/*.js"));

            //SystemMgmt/RunAs
            bundles.Add(new ScriptBundle("~/bundles/SystemMgmt/RunAs_VMI").Include(
                "~/Scripts/SystemMgmt/RunAs_VMI/*.js"));
            //SQMRegister/SQMRegister
            bundles.Add(new ScriptBundle("~/bundles/SQMMgmt/VendorList").Include(
                "~/Scripts/SQMMgmt/VendorList/*.js"));
            //SystemMgmt/RoleMemberMgmt
            bundles.Add(new ScriptBundle("~/bundles/SystemMgmt/RoleMemberMgmt").Include(
                "~/Scripts/SystemMgmt/RoleMemberMgmt/*.js",
                "~/Scripts/SystemMgmt/RoleMemberAndDelegation/Shared.js",
                "~/Scripts/SystemMgmt/RoleMemberAndDelegation/AssignRoleMembers.js"));
            //=========================

            //Add by Edward Huang For VMI V2
            //Start date from 2016/03
            #region VMIShare
            bundles.Add(new ScriptBundle("~/bundles/VMIShare").Include(
                "~/Scripts/VMIShare/*.js"
                ));
            #endregion

            #region VMIConfigration/ASNVendorProfile
            bundles.Add(new ScriptBundle("~/bundles/VMIConfigration/ASNVendorProfile").Include(
                "~/Scripts/VMIConfigration/ASNVendorProfile/*.js"
                ));

            #endregion

            #region VMIProcess/ToDoASN
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/ToDoASN").Include(
                "~/Scripts/VMIProcess/ToDoASN/*.js"
                ));
            #endregion

            #region VMIProcess/ToDoVDS
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/ToDoVDS").Include(
                "~/Scripts/VMIProcess/ToDoVDS/*.js"));
            #endregion

            #region VMIQuery/QueryVDS
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/QueryVDS").Include(
                "~/Scripts/VMIQuery/QueryVDS/*.js"));
            #endregion

            #region VMIProcess/UploadVDS
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/UploadVDS").Include(
                "~/Scripts/VMIProcess/UploadVDS/*.js"));
            #endregion

            #region VMIProcess/ASNSpecialCancel
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/ASNSpecialCancel").Include(
                "~/Scripts/VMIProcess/ASNSpecialCancel/*.js"));
            #endregion

            #region VMIProcess/UploadASN
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/UploadASN").Include(
                "~/Scripts/VMIProcess/UploadASN/*.js"));
            #endregion

            #region VMIQuery/QueryDailyInventory
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/QueryDailyInventory").Include(
                "~/Scripts/VMIQuery/QueryDailyInventory/*.js"));
            #endregion

            #region VMIQuery/APSearch
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/APSearch").Include(
                "~/Scripts/VMIQuery/APSearch/*.js"));
            #endregion

            #region VMIQuery/GRReport
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/GRReport").Include(
                "~/Scripts/VMIQuery/GRReport/*.js"));
            #endregion
            #region VMIQuery/QueryASN
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/QueryASN").Include(
                "~/Scripts/VMIQuery/QueryASN/*.js"));
            #endregion

            #region VMIQuery/POAPCheckStatement
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/POAPCheckStatement").Include(
                "~/Scripts/VMIQuery/POAPCheckStatement/*.js"));
            #endregion

            #region VMIQuery/APCheckStatement
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/APCheckStatement").Include(
                "~/Scripts/VMIQuery/APCheckStatement/*.js"));
            #endregion

            #region VMIBulletion/RefAndDownload
            bundles.Add(new ScriptBundle("~/bundles/VMIBulletin/RefAndDownload").Include(
                "~/Scripts/VMIBulletin/RefAndDownload/*.js"));
            #endregion
            #region VMIQuery/ASNReport
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/ASNReport").Include(
                "~/Scripts/VMIQuery/ASNReport/*.js"));
            #endregion
            //=========================

            bundles.Add(new StyleBundle("~/Content/jQuery-File-Upload").Include(
                    "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                   "~/Content/jQuery.FileUpload/css/jquery.fileupload-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/jQuery-File-Upload").Include(
                //<!-- The Templates plugin is included to render the upload/download listings -->
                "~/Scripts/jQuery.FileUpload/vendor/jquery.ui.widget.js",
                "~/Scripts/jQuery.FileUpload/tmpl.min.js",
                //<!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                //<!-- The basic File Upload plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                //<!-- The File Upload processing plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
                //<!-- The File Upload validation plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
                //<!-- The File Upload user interface plugin -->
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js",
                //<!-- Self Defined File Upload -->
                "~/Scripts/jQuery.FileUpload/fileupload-common.js"));

            bundles.Add(new StyleBundle("~/bundles/Telerik/css").Include(
                "~/ReportViewer/styles/kendo.common.min.css",
                "~/ReportViewer/styles/kendo.blueopal.min.css"));

            #region VMIProcess/ApplicationVendorAccount
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/ApplicationVendorAccount").Include(
                "~/Scripts/VMIProcess/ApplicationVendorAccount/*.js"));
            #endregion

            #region VMIProcess/ToDoPOAck
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/ToDoPOAck").Include(
                "~/Scripts/VMIProcess/ToDoPOAck/*.js"));
            #endregion

            #region VMIQuery/QueryPOAck
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/QueryPOAck").Include(
                "~/Scripts/VMIQuery/QueryPOAck/*.js"));
            #endregion

            #region VMIProcess/VendorEmailQueryAndChange
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/VendorEmailQueryAndChange").Include(
                "~/Scripts/VMIProcess/VendorEmailQueryAndChange/*.js"));
            #endregion

            #region VMIQuery/PerInvRptConsign
            bundles.Add(new ScriptBundle("~/bundles/VMIQuery/PerInvRptConsign").Include(
                "~/Scripts/VMIQuery/PerInvRptConsign/*.js"));
            #endregion

            #region VMIProcess/OpenPOConfirmation
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/OpenPOConfirmation").Include(
                "~/Scripts/VMIProcess/OpenPOConfirmation/*.js"));
            #endregion

            #region VMIProcess/UploadOpenPOConfirmation
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/UploadOpenPOConfirmation").Include(
                "~/Scripts/VMIProcess/UploadOpenPOConfirmation/*.js"));
            #endregion

            #region VMIProcess/ToDoImportList
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/ToDoImportList").Include(
                "~/Scripts/VMIProcess/ToDoImportList/*.js"));
            #endregion

            #region VMIConfigration/PlantForwarderInfo
            bundles.Add(new ScriptBundle("~/bundles/VMIConfigration/PlantForwarderInfo").Include(
                "~/Scripts/VMIConfigration/PlantForwarderInfo/*.js"));
            #endregion

            #region VMIConfigration/PlantReceiveInfo
            bundles.Add(new ScriptBundle("~/bundles/VMIConfigration/PlantReceiveInfo").Include(
                "~/Scripts/VMIConfigration/PlantReceiveInfo/*.js"));
            #endregion

            #region VMIConfigration/PlantCustomsBrokerInfo
            bundles.Add(new ScriptBundle("~/bundles/VMIConfigration/PlantCustomsBrokerInfo").Include(
                "~/Scripts/VMIConfigration/PlantCustomsBrokerInfo/*.js"));
            #endregion

            #region SQM
            //SQMMgmt/VendorAccount
            bundles.Add(new ScriptBundle("~/bundles/SQMMgmt/VendorAccount").Include(
                "~/Scripts/SQMMgmt/VendorAccount/*.js"));
            //chart
            bundles.Add(new ScriptBundle("~/bundles/Chart").Include(
                "~/Scripts/Chart/*.js"));
            //SQMMgmt/SubFuncManagement
            bundles.Add(new ScriptBundle("~/bundles/SQMMgmt/SubFuncManagement").Include(
                "~/Scripts/SQMMgmt/SubFuncManagement/*.js"));
            //SQMMgmt/MenuSubFuncMgmt
            bundles.Add(new ScriptBundle("~/bundles/SQMMgmt/MenuSubFuncMgmt").Include(
                "~/Scripts/SQMMgmt/MenuSubFuncMgmt/*.js"));

            //SQMBasic/BasicInfo
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/BasicInfo").Include(
                "~/Scripts/SQMBasic/BasicInfo/*.js"));

            //SQMPRO/SQMProduct1
            bundles.Add(new ScriptBundle("~/bundles/SQMPRO/SQMProduct1").Include(
                "~/Scripts/SQMPRO/SQMProduct1/*.js"));

            //SQMProcess/ProcessInfo
            bundles.Add(new ScriptBundle("~/bundles/SQMProcess/ProcessInfo").Include(
                "~/Scripts/SQMProcess/ProcessInfo/*.js"));

            //SQMCertification/CertificationInfo
            bundles.Add(new ScriptBundle("~/bundles/SQMCertification/CertificationInfo").Include(
                "~/Scripts/SQMCertification/CertificationInfo/*.js"));

            //SQMHR/HRInfo
            bundles.Add(new ScriptBundle("~/bundles/SQMHR/HRInfo").Include(
                "~/Scripts/SQMHR/HRInfo/*.js"));

            //SQMBasic/SQMPVL/
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMPVL").Include(
                "~/Scripts/SQMBasic/SQMPVL/*.js"));

            //SQMBasic/SQMPVL/
            bundles.Add(new ScriptBundle("~/bundles/SQMMaterialMgmt/SQMScarMgmt").Include(
                "~/Scripts/SQMMaterialMgmt/SQMScarMgmt/*.js"));
            //SQMBasic/SQMPVL/
            bundles.Add(new ScriptBundle("~/bundles/SQEMaterialMgmt/SQEScarMgmt").Include(
                "~/Scripts/SQEMaterialMgmt/SQEScarMgmt/*.js"));
            #endregion

            #region SQMBasic/KeyInfoIntro
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/KeyInfoIntro").Include(
                "~/Scripts/SQMBasic/KeyInfoIntro/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMAgents/KeyInfoIntro").Include(
                "~/Scripts/SQMBasic/SQMAgents/KeyInfoIntro/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMTraders/KeyInfoIntro").Include(
                "~/Scripts/SQMBasic/SQMTraders/KeyInfoIntro/*.js"));

            //SQMProduct
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMProduct/SQMBasic").Include(
                "~/Scripts/SQMBasic/SQMProduct/SQMBasic/*.js"));
            //SQMBasic/SQMAgents
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMAgents/SQMBasic").Include(
                "~/Scripts/SQMBasic/SQMAgents/SQMBasic/*.js"));
            //SQMTraders
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMTraders/SQMBasic").Include(
                "~/Scripts/SQMBasic/SQMTraders/SQMBasic/*.js"));
            //SQMBasic/SQMContact
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMContact").Include(
                "~/Scripts/SQMBasic/SQMContact/*.js"));
            //SQMBasic/SQMEquipment
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMEquipment").Include(
                "~/Scripts/SQMBasic/SQMEquipment/*.js"));
            //SQMBasic/Criticism
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/Criticism").Include(
                "~/Scripts/SQMBasic/Criticism/*.js"));
            //SQMBasic/Common
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/Common").Include(
                "~/Scripts/SQMBasic/Common/*.js"));
            //SQMPlant/SQMPlant
            bundles.Add(new ScriptBundle("~/bundles/SQMPlant/SQMPlant").Include(
                "~/Scripts/SQMPlant/*.js"));
            //SQMSQEPUR/SQMSQEPUR
            bundles.Add(new ScriptBundle("~/bundles/SQMSQEPUR/SQMSQEPUR").Include(
    "~/Scripts/SQMSQEPUR/*.js"));
            //SQMMailR/SQMMailR
            bundles.Add(new ScriptBundle("~/bundles/SQMMailR/SQMMailR").Include(
    "~/Scripts/SQMMailR/*.js"));




            #endregion
            #region SQMBasic/SQEKeyInfoIntro
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEKeyInfoIntro").Include(
                "~/Scripts/SQMBasic/SQEKeyInfoIntro/*.js"));
            #endregion
            #region SQMCertification/KeyInfoIntro
            bundles.Add(new ScriptBundle("~/bundles/SQMCertification/KeyInfoIntro").Include(
                "~/Scripts/SQMCertification/KeyInfoIntro/*.js"));
            #endregion

            #region SQMBasic/SQMCustomers2
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMCustomers").Include(
                "~/Scripts/SQMBasic/SQMCustomers/*.js"));
            #endregion

            #region SQMRegist/Regist

            bundles.Add(new ScriptBundle("~/bundles/Regist/js").Include(
            "~/Scripts/Regist/*.js"));

            bundles.Add(new StyleBundle("~/bundles/Regist/css").Include(
                "~/Scripts/Regist/Regist.css"));
            #endregion

            #region SQM/Approve

            bundles.Add(new ScriptBundle("~/bundles/SQMApprove/js").Include(
            "~/Scripts/SQMApprove/*.js"));

            bundles.Add(new StyleBundle("~/bundles/SQMApprove/css").Include(
                "~/Scripts/SQMApprove/Approve.css"));
            #endregion

            #region SQM/ChangeApprove

            bundles.Add(new ScriptBundle("~/bundles/ChangeApprove/js").Include(
            "~/Scripts/ChangeApprove/*.js"));

            bundles.Add(new StyleBundle("~/bundles/ChangeApprove/css").Include(
                "~/Scripts/ChangeApprove/ChangeApprove.css"));
            #endregion
            #region SQM/SQMReliabilityApprove

            bundles.Add(new ScriptBundle("~/bundles/ReliabilityApprove/js").Include(
            "~/Scripts/SQMReliabilityApprove/*.js"));

            bundles.Add(new StyleBundle("~/bundles/ReliabilityApprove/css").Include(
                "~/Scripts/SQMReliabilityApprove/ReliabilityApprove.css"));
            #endregion
            #region SQM/SQMReliabilityFileApprove

            bundles.Add(new ScriptBundle("~/bundles/ReliabilityFileApprove/js").Include(
            "~/Scripts/SQMReliabilityFileApprove/*.js"));

            bundles.Add(new StyleBundle("~/bundles/ReliabilityFileApprove/css").Include(
                "~/Scripts/SQMReliabilityFileApprove/ReliabilityFileApprove.css"));
            #endregion
            #region SQM/SQMReliability

            bundles.Add(new ScriptBundle("~/bundles/SQMReliability").Include(
            "~/Scripts/SQMReliability/SQMReliability/*.js"));
            #endregion
            bundles.Add(new ScriptBundle("~/bundles/CPK/js").Include(
            "~/Scripts/CPK/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/CPK/css").Include(
            "~/Scripts/CPK/*.css"));
         
            #region SQM/SQMBar

            bundles.Add(new ScriptBundle("~/bundles/SQMBar").Include(
            "~/Scripts/SQMReliability/SQMBar/*.js"));

            #endregion
            #region  SQM/SQMMRB
            bundles.Add(new ScriptBundle("~/bundles/SQMMRB").Include(
         "~/Scripts/SQMMRB/*.js"));
            #endregion
            #region  SQM/MRBAppove
            bundles.Add(new ScriptBundle("~/bundles/MRBAppove").Include(
         "~/Scripts/MRBAppove/*.js"));
            #endregion
            #region SQM/Hold

            bundles.Add(new ScriptBundle("~/bundles/Hold").Include(
            "~/Scripts/Hold/*.js"));

            #endregion
            #region SQM/SQEHold

            bundles.Add(new ScriptBundle("~/bundles/SQEHold").Include(
            "~/Scripts/SQEHold/*.js"));

            #endregion

            #region  SQM/VenderAccount
            bundles.Add(new ScriptBundle("~/bundles/VenderAccount").Include(
         "~/Scripts/VenderAccount/*.js"));
            #endregion
            #region  SQM/AnnualObjectives
            bundles.Add(new ScriptBundle("~/bundles/AnnualObjectives").Include(
         "~/Scripts/AnnualObjectives/*.js"));
            #endregion
            #region  SQM/SQMGrade
            bundles.Add(new ScriptBundle("~/bundles/SQMGrade").Include(
         "~/Scripts/SQMGrade/*.js"));
            #endregion
            #region  SQM/SQMBenefit
            bundles.Add(new ScriptBundle("~/bundles/SQMBenefit").Include(
         "~/Scripts/SQMBenefit/*.js"));
            #endregion
            #region SQM/SQMQuality

            #region SQMBasic/SQEInsp
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEInsp").Include(
               "~/Scripts/SQMBasic/SQEInsp/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEInsp/InspDesc").Include(
            "~/Scripts/SQMBasic/SQEInsp/InspDesc/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEInsp/InspDescVar").Include(
            "~/Scripts/SQMBasic/SQEInsp/InspDescVar/*.js"));

            #endregion
            #region SQMQualityInsp
            bundles.Add(new ScriptBundle("~/bundles/SQMQualityInsp").Include(
               "~/Scripts/SQMReliability/SQMQualityInsp/*.js"));
            #endregion
            #region SQMQualityFile
            bundles.Add(new ScriptBundle("~/bundles/SQMQualityFile").Include(
               "~/Scripts/SQMReliability/SQMQualityFile/*.js"));
            #endregion
            #region SQMCPK
            bundles.Add(new ScriptBundle("~/bundles/SQMCPK").Include(
               "~/Scripts/SQMReliability/SQMCPK/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/SQMCPK/CPKSub").Include(
            "~/Scripts/SQMReliability/SQMCPK/CPKSub/*.js"));
            bundles.Add(new ScriptBundle("~/bundles/SQMCPK/CPKData").Include(
            "~/Scripts/SQMReliability/SQMCPK/CPKData/*.js"));
            #endregion

            bundles.Add(new ScriptBundle("~/bundles/SQMQuality").Include(
            "~/Scripts/SQMReliability/SQMQuality/*.js"));

            #endregion
            #region SQM/SQMBasic/SQEReliability

            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEReliability").Include(
            "~/Scripts/SQMBasic/SQEReliability/*.js"));

            #endregion
            #region SQM/SQMBasic/SQEContact

            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEContact").Include(
            "~/Scripts/SQMBasic/SQEContact/*.js"));

            #endregion
            #region SQM/SQMBasic/SQEContact/Info

            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEContact/Info").Include(
            "~/Scripts/SQMBasic/SQEContact/Info/*.js"));

            #endregion
            #region"SQM/SQMBasic/SQECriticism
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQECriticism").Include(
            "~/Scripts/SQMBasic/SQECriticism/*.js"));
            #endregion
            #region"SQM/SQMBasic/SQECriticism/Criticism
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQECriticism/Criticism").Include(
            "~/Scripts/SQMBasic/SQECriticism/Criticism/*.js"));
            #endregion
            #region"SQM/SQMBasic/SQEQuality
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEQuality").Include(
            "~/Scripts/SQMBasic/SQEQuality/*.js"));
            #endregion
            #region"SQM/SQMBasic/SQEQuality/Insp
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEQuality/Insp").Include(
            "~/Scripts/SQMBasic/SQEQuality/Insp/*.js"));
            #endregion
            #region"SQM/SQMBasic/SQEQuality/File
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEQuality/File").Include(
            "~/Scripts/SQMBasic/SQEQuality/File/*.js"));
            #endregion
            #region"SQM/SQMReliability/SQMECN
            bundles.Add(new ScriptBundle("~/bundles/SQMReliability/SQMECN").Include(
            "~/Scripts/SQMReliability/SQMECN/*.js"));
            #endregion
            #region"SQM/SQECPK/SQECPK
            bundles.Add(new ScriptBundle("~/bundles/SQECPK/SQECPK").Include(
            "~/Scripts/SQECPK/*.js"));
            #endregion
            #region SQMBasic/SQEAQLPlant/AQLPlant
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEAQLPlant/AQLPlant").Include(
            "~/Scripts/SQMBasic/SQEAQLPlant/AQLPlant/*.js"));
            #endregion
            #region SQMBasic/SQEAQLPlant/AQLPlantMap
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEAQLPlant/AQLPlantMap").Include(
            "~/Scripts/SQMBasic/SQEAQLPlant/AQLPlantMap/*.js"));
            #endregion
            #region SQMBasic/SQEAQLPlant/AQLPlantRule
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEAQLPlant/AQLPlantRule").Include(
            "~/Scripts/SQMBasic/SQEAQLPlant/AQLPlantRule/*.js"));
            #endregion
            #region SQMBasic/SQEUDMaintain
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQEUDMaintain").Include(
            "~/Scripts/SQMBasic/SQEUDMaintain/*.js"));
            #endregion
            #region SQMBasic/SQELitNO_Plant
            bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQELitNO_Plant").Include(
            "~/Scripts/SQMBasic/SQELitNO_Plant/*.js"));
            #endregion
            //    #region SQMBasic/SQMCustomers2
            //    bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMCustomers2").Include(
            //        "~/Scripts/SQMBasic/SQMCustomers2/*.js"));
            //    #endregion
            //    #region SQMBasic/SQMCustomers3
            //    bundles.Add(new ScriptBundle("~/bundles/SQMBasic/SQMCustomers3").Include(
            //        "~/Scripts/SQMBasic/SQMCustomers3/*.js"));
            //    #endregion
            //SystemMgmt/MemberManagement
            bundles.Add(new ScriptBundle("~/bundles/VMIConfigration/GroupMgmt").Include(
                "~/Scripts/VMIConfigration/GroupMgmt/*.js"));

            #region VMIProcess/ApplicationBuyerAccount
            bundles.Add(new ScriptBundle("~/bundles/VMIProcess/ApplicationBuyerAccount").Include(
                "~/Scripts/VMIProcess/ApplicationBuyerAccount/*.js"));
            #endregion

            #region VMIConfigration/BuyerGroup
            bundles.Add(new ScriptBundle("~/bundles/VMIConfigration/BuyerGroup").Include(
                "~/Scripts/VMIConfigration/BuyerGroup/*.js"));
            #endregion
        }
    }
}
