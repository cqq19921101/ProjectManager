using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;

namespace LiteOn.GDS.Utility
{
    public class SettingParser
    {
        const string settingPath = @"C:\\Projects\\GoodsMovement\\Settings\\";
        private ArrayList _DetailFields;
        private ArrayList _HeadFields;
        private string _Plant
        {
            get;
            set;
        }
        private string _AppID
        {
            get;
            set;
        }

        private string _SettingFilename
        {
            get
            {
                return settingPath + this._Plant + "-" + this._AppID + ".xml";
            }
        }

        // PUBLIC
        public string Title
        {
            get;
            set;
        }

        // PUBLIC
        public string Subject
        {
            get;
            set;
        }

        public string ApproveXML
        {
            get;
            set;
        }

        public ArrayList DetailFields
        {
            get { return _DetailFields; }
        }

        public ArrayList HeadFields
        {
            get { return _HeadFields; }
        }

        public SettingParser(string Plant, string AppID)
        {
            this._Plant = Plant;
            this._AppID = AppID;
            // IF SETTING NOT EXIST, THROW EXCEPTION
            if (!File.Exists(_SettingFilename))
            {
                // FILE NOT EXITS
                throw new Exception("Setting file not exist: " + _SettingFilename);
            }
            Title = string.Empty;
            Subject = string.Empty;
            SetTitle();
            SetSubject();
            SetDetailFields();
            SetApproveXml();
        }

        private void SetApproveXml()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_SettingFilename);

                XmlElement el = doc.DocumentElement;
                XmlNamespaceManager xMgr = new XmlNamespaceManager(doc.NameTable);
                XmlNode xn = el.SelectSingleNode("//GoodsMovementSetting/DOA", xMgr);
                ApproveXML = "<ROOT>" + xn.InnerXml + "</ROOT>";
            }
            catch
            {
            }
        }

        private void SetTitle()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_SettingFilename);

                XmlElement el = doc.DocumentElement;
                XmlNamespaceManager xMgr = new XmlNamespaceManager(doc.NameTable);
                XmlNodeList xList = el.SelectNodes("//GoodsMovementSetting/Title", xMgr);

                foreach (XmlNode node in xList)
                {
                    try
                    {
                        this.Title = node.Attributes.GetNamedItem("Text").Value;
                    }
                    catch { /* USE DEFAULT */}
                }
            }
            catch
            {
            }
        }

        private void SetSubject()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_SettingFilename);

                XmlElement el = doc.DocumentElement;
                XmlNamespaceManager xMgr = new XmlNamespaceManager(doc.NameTable);
                XmlNodeList xList = el.SelectNodes("//GoodsMovementSetting/Subject", xMgr);


                foreach (XmlNode node in xList)
                {
                    try
                    {
                        this.Subject = node.Attributes.GetNamedItem("Text").Value;
                    }
                    catch
                    {
                        /* USE DEFAULT */
                        Subject = "Goods Movement [{MBLNR_A}]-{WERKS}";
                    }
                }
                if (xList.Count == 0)
                {
                    /* USE DEFAULT */
                    Subject = "Goods Movement [{MBLNR_A}]-{WERKS}";
                }
            }
            catch
            {
                /* USE DEFAULT */
                Subject = "Goods Movement [{MBLNR_A}]-{WERKS}";
            }
        }

        public void SetDetailFields()
        {
            SetDetailFields("DetailFields");
            SetDetailFields("HeadFields");

        }

        public void SetDetailFields(string type)
        {
            ArrayList alFields = new ArrayList();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_SettingFilename);

                XmlElement el = doc.DocumentElement;
                XmlNamespaceManager xMgr = new XmlNamespaceManager(doc.NameTable);
                XmlNodeList xList = el.SelectNodes("//GoodsMovementSetting/" + type + "/Field", xMgr);

                foreach (XmlNode node in xList)
                {
                    FieldData fd = new FieldData();
                    try
                    {
                        fd.Name = node.Attributes.GetNamedItem("Name").Value;
                    }
                    catch { }
                    try
                    {
                        fd.DisplayName = fd.Name;
                    }
                    catch { }
                    try
                    {
                        fd.DisplayName = node.Attributes.GetNamedItem("DisplayName").Value;
                    }
                    catch { }
                    try
                    {
                        fd.Type = node.Attributes.GetNamedItem("Type").Value.ToLower();
                    }
                    catch
                    {
                        fd.Type = "string"; // DEFAULT = "string" 
                    }

                    alFields.Add(fd);
                }
                if (type == "DetailFields")
                {
                    this._DetailFields = alFields;
                }
                else
                {
                    this._HeadFields = alFields;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public string GetApproveLine()
        {
            return "";
        }

        public class FieldData
        {
            public string Name = "";
            public string DisplayName = "";
            public string Type = "string";
        }

    }


    // --------------------------------------------------------------------
    public class SPMAppLine
    {
        public enum FlowParseType
        {
            None = 0,
            CurrentApprover = 100,
            NextApprover = 200,
            CurrentStep = 600,
            NextStep = 700
        }

        public SPMAppLine()
        {
        }

        /* ********** Approve Line XML Format Sample **********
    strApproverXML = string.Format(@"
    [[STEPS]]
     [[STEP ID='A1' TITLE='Sales Head' TYPE='Users' GETBY='Role+Product' GETVALUE='Sales Head+{0}' /]]
     [[STEP ID='A2' TITLE='PM' TYPE='Line' GETBY='Role+Product' GETVALUE='PM+{0}' /]]
     [[STEP ID='A3' TITLE='PM Manager' TYPE='Users' GETBY='Role+Site' GETVALUE='PM Manager+{1}'/]]
     [[STEP ID='A4' TITLE='SM Head' TYPE='Users' GETBY='Role+Product+Site' GETVALUE='SM Head+{0}+{1}' /]]
     [[STEP ID='A5' TITLE='PD Head' TYPE='Users' GETBY='Role+Product' GETVALUE='PD Head+{0}' /]]
     [[STEP ID='A6' TITLE='PD' TYPE='Line' GETBY='Role+Product' GETVALUE='PD+{0}' /]]
    [[/STEPS]]", strProductCode, strIssuerSite);
         */
        // --------------------------------------------------------------------
        static private string FlowXMLParse(FlowParseType eType, string ApproveLineXML)
        {
            return FlowXMLParse(eType, ApproveLineXML, false);
        }

        // --------------------------------------------------------------------
        static private string FlowXMLParse(FlowParseType eType, string ApproveLineXML, bool bGenNext)
        {
            string _xml = ApproveLineXML.Replace("[[", "<").Replace("]]", ">");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_xml);
            XmlElement el = doc.DocumentElement;
            XmlNamespaceManager xMgr = new XmlNamespaceManager(doc.NameTable);
            XmlNodeList nl = el.SelectNodes("//APP", xMgr);

            foreach (XmlNode no in nl)
            {
                string ID = "";
                if (no.Attributes["ID"] != null)
                    ID = no.Attributes["ID"].Value.ToString();

                if (no.Attributes["PASS"] == null)
                {
                    if (bGenNext)
                    {
                        // ADD PASS TO ATTRIBUTE 
                        XmlAttribute newAttr = doc.CreateAttribute("PASS");
                        newAttr.Value = "True";
                        no.Attributes.Append(newAttr);
                        return doc.InnerXml.Replace("<", "[[").Replace(">", "]]"); ;
                    }
                    if (eType == FlowParseType.CurrentApprover) return ID; // ** CurrentApprover 
                    if (eType == FlowParseType.CurrentStep)
                    {
                        XmlNode noStep = no.ParentNode;

                        //string TITLE = noStep.Attributes["Title"].Value.ToString();
                        string TITLE = no.Attributes["Title"].Value.ToString();
                        return TITLE; // ** CurrentStep 
                    }

                    if (eType == FlowParseType.NextApprover)
                    {
                        eType = FlowParseType.CurrentApprover; // ** NextApprover
                    }
                    else if (eType == FlowParseType.NextStep)
                    {
                        eType = FlowParseType.CurrentStep;  // ** NextStep
                    }
                }
            }

            if (bGenNext)
            {
                return doc.InnerXml.Replace("<", "[[").Replace(">", "]]");
            }
            else
            {
                return "*"; // END
            }
        }

        // --------------------------------------------------------------------
        static public string GetJumpXML(string ApproveLineXML, string RejectToStep)
        {
            string _xml = ApproveLineXML.Replace("[[", "<").Replace("]]", ">");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_xml);
            XmlElement el = doc.DocumentElement;
            XmlNamespaceManager xMgr = new XmlNamespaceManager(doc.NameTable);
            XmlNodeList nl = el.SelectNodes("//APP", xMgr);

            bool bClean = false;

            foreach (XmlNode no in nl)
            {
                XmlNode noStep = no.ParentNode;

                string TITLE = noStep.Attributes["TITLE"].Value.ToString();
                if (TITLE == RejectToStep)
                    bClean = true;

                if ((bClean) && (no.Attributes["PASS"] != null))
                {
                    // REMOVE PASS FROM ATTRIBUTE 
                    no.Attributes.RemoveNamedItem("PASS");
                }
                else
                {
                    if ((!bClean) && (no.Attributes["PASS"] == null))
                    {
                        // ADD PASS TO ATTRIBUTE 
                        XmlAttribute newAttr = doc.CreateAttribute("PASS");
                        newAttr.Value = "True";
                        no.Attributes.Append(newAttr);
                    }
                }
            }
            return doc.InnerXml.Replace("<", "[[").Replace(">", "]]");
        }

        // --------------------------------------------------------------------
        static public string GetResetXML(string ApproveLineXML)
        {
            string RejectToStep = "";
            string _xml = ApproveLineXML.Replace("[[", "<").Replace("]]", ">");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(_xml);
            XmlElement el = doc.DocumentElement;
            XmlNamespaceManager xMgr = new XmlNamespaceManager(doc.NameTable);
            XmlNodeList nl = el.SelectNodes("//STEP", xMgr);

            foreach (XmlNode no in nl)
            {
                RejectToStep = no.Attributes["TITLE"].Value.ToString();
                return GetJumpXML(ApproveLineXML, RejectToStep);
            }
            throw new Exception("ERR - [SPMAppLine][GetResetXML]");
        }

        // --------------------------------------------------------------------
        static public string GetCurrentApprover(string ApproveLineXML)
        {
            return FlowXMLParse(FlowParseType.CurrentApprover, ApproveLineXML);
        }

        // --------------------------------------------------------------------
        static public string GetCurrentStep(string ApproveLineXML)
        {
            return FlowXMLParse(FlowParseType.CurrentStep, ApproveLineXML);
        }

        // --------------------------------------------------------------------
        static public string GetNextApprover(string ApproveLineXML)
        {
            return FlowXMLParse(FlowParseType.NextApprover, ApproveLineXML);
        }

        // --------------------------------------------------------------------
        static public string GetNextStep(string ApproveLineXML)
        {
            return FlowXMLParse(FlowParseType.NextStep, ApproveLineXML);
        }

        // --------------------------------------------------------------------
        static public string GetApproveXML(string ApproveLineXML)
        {
            return FlowXMLParse(FlowParseType.CurrentApprover, ApproveLineXML, true);
        }
    }

}
