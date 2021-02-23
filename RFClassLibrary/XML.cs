using System;
using System.Collections.Generic;
using System.Text;
//added
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Collections.Specialized;
using System.Xml.Xsl;
using System.Collections;
using System.Xml.Linq;
using System.Linq;

namespace RFClassLibrary
{
    /// <summary>
    /// XML
    /// </summary>
    public class XML : RFCObject
    {
        string m_strSendingApp = string.Empty;
        #region X12 Variables

        static int m_iStartISA; // Interchange control Header
        static int m_iStartIEA; // Interchange control Trailer
        static int m_iStartGS; // functional group header
        static int m_iStartGE; // functional group trailer
        static int m_iStartST; // Transaction Set Header
        static int m_iStartSE; // Transaction Set trailer
        static int m_iStartBHT; // Beginning of heirarchical transaction
        static int m_iStartDTP; // Date or time or period
        static int m_iStartREF; // Reference Identification
        static int m_iStartNM1; // Individual or Organization Name
        static int m_iStartPER; // Administrative Communications Contact
        static int m_iStartHL; // Billing/Pay-To Provider Hierarchical Level
        static int m_iStartPRV; // Billing/Pay-To Provider Specialty Information
        static int m_iStartN2; // Additional Responsible Party Name Information
        static int m_iStartN3; // Responsible Party Address
        static int m_iStartN4; // Responsible Party City/State/Zip Code
        static int m_iStartSBR; // Subscriber Info
        static int m_iStartDMG; // Subscriber Demographics Info
        static int m_iStartCLM; // Claim Information
        static int m_iStartHI; // Health Care Diagnosis Code
        static int m_iStartLX; // Service Line
        static int m_iStartSV1; // Professional Service
        static int m_iStartPAT; // Patient Information
        static int m_iStartN1;
        static int m_iStartSVC; // Service Payment Information
        static int m_iStartSVC01; // Service Payment Information field 1 definition
        static int m_iStartSVC06; // Service Payment Information field 6 definition
        static int m_iStartQTY; // Claim Supplemental Information Quantity
        static int m_iStartAMT; // Claim Supplemental Information
        static int m_iStartCAS; // Claim Adjustment
        static int m_iStartCLP; // Claim Payment Information
        static int m_iStartTS3; // Provider Summary Information
        static int m_iStartDTM; // Date (See the first field for the type of date. i.e. Claim, processing, service etc)
        static int m_iStartTRN; // Reassociation Trace Number
        static int m_iStartBPR; // Financial Information
        static int m_iStartPLB; // Provider Adjustment
        static int m_iStartLQ; //Health Care Remark Codes



        //      static bool m_bNonStandardX12; // not one of the defined types X12 Identifiers.
        static int m_iStartXXX; // default non standard segment
        #endregion X12

        #region HL7 Variables
        static bool m_bNonStandardXML;// non standard header type. not MSH etc..
        static int m_iStartZZZ; // default non standard segment
        static int m_iStartMSH;
        static int m_iStartPID;
        static int m_iStartOBX;
        static int m_iStartORC;
        static int m_iStartNTE;
        static int m_iStartMSA;
        static int m_iStartACK;
        static int m_iStartIN1;
        static int m_iStartPV1;
        static int m_iStartGT1;
        static int m_iStartOBR;
        static int m_iStartZPS;
        static int m_iStartDG1;
        static int m_iStartIN2; // wdk 20091021 added

        #endregion HL7
        string m_strFile;
        XmlDocument m_xmlDoc;

        /// <summary>
        /// 
        /// </summary>
        public XML()
        {
        }


        /// <summary>
        /// Loads the file into this instance of the XML class as a private XmlDocument.
        /// </summary>
        /// <param name="strFile">this is the file to load.</param>
        public XML(string strFile)
        {
            if (strFile == null || strFile.Length == 0)
            {
                throw new System.NotImplementedException();
            }
            m_strFile = strFile;
            try
            {
                m_xmlDoc = new XmlDocument();
                m_xmlDoc.Load(m_strFile);
            }
            catch (Exception err)
            {
                propErrMsg = err.Message;
                return;
            }

        }

        /// <summary>
        /// Not finished 
        /// </summary>
        public void CovertXmlDocToHTML()
        {
            XsltSettings settingXslt = new XsltSettings(true, true);
            XslCompiledTransform tran = new XslCompiledTransform();

            tran.Load(@"C:\source\RFClassLibrary\XSLTFile.xslt", settingXslt, new XmlUrlResolver());
            XmlReaderSettings settingReader = new XmlReaderSettings();
            settingReader.ConformanceLevel = ConformanceLevel.Fragment;
            settingReader.IgnoreComments = true;
            settingReader.IgnoreWhitespace = true;

            XmlReader xrd = XmlReader.Create(m_strFile, settingReader);

            XmlWriterSettings settingsWriter = new XmlWriterSettings();

            settingsWriter.ConformanceLevel = ConformanceLevel.Fragment;

            XmlWriter xwr = XmlWriter.Create(@"C:\temp\david.htm", settingsWriter);
            //XmlWriter xwr = XmlWriter.Create(@"C:\Program Files\Medical Center Lab\MCL Services Monitor\data_HL7_msgtxtXmlFile090608015826.Txt");
            tran.Transform(xrd, xwr);

        }
        /// <summary>
        /// overloaded to not pass last two paramaters
        /// </summary>
        /// <param name="strFile"></param>
        /// <param name="strTagName"></param>
        /// <param name="strSearchElementName"></param>
        /// <param name="strSearchValue"></param>
        /// <param name="strReturnValueFromElementName"></param>
        /// <returns></returns>
        static public string GetAnElementsText(string strFile, string strTagName, string strSearchElementName, string strSearchValue, string strReturnValueFromElementName)
        {
            return GetAnElementsText(strFile, strTagName, strSearchElementName, strSearchValue, strReturnValueFromElementName, "", "");
        }

        /// <summary>
        /// Gets the value for a particular element in the XML file 
        /// used primary for configuration file for the Ford and HMS services for start up paramaters
        /// returns a empty string with failure also sets the RFCObject propErrMsg.
        /// NOTE: Search is case sensitive.
        /// </summary>
        /// <param name="strFile"></param>
        /// <param name="strTagName"></param>
        /// <param name="strSearchElementName"></param>
        /// <param name="strSearchValue"></param>
        /// <param name="strReturnValueFromElementName"></param>
        /// <param name="strSecondSearchElementName"></param>
        /// <param name="strSecondSearchValue"></param>
        /// <returns>Empty string if no value is found or an error occurs. See propErrMsg for text of error</returns>
        static public string GetAnElementsText(string strFile, string strTagName, string strSearchElementName, string strSearchValue, string strReturnValueFromElementName, string strSecondSearchElementName, string strSecondSearchValue)
        {
            string strRetVal = "";
            string strSearch = "";

            if (strFile.Length == 0 || strTagName.Length == 0 || strSearchElementName.Length == 0 || strSearchValue.Length == 0 || strReturnValueFromElementName.Length == 0)
            {
                m_strErrMsg = "Invalid length on a paramater passed to GetAttribute()";
                throw new System.IndexOutOfRangeException();
            }

            if (strSecondSearchElementName.Length == 0 && strSecondSearchValue.Length == 0)
            {
                strSearch = string.Format("descendant::{0}[{1}='{2}']", strTagName, strSearchElementName, strSearchValue);
            }
            if (strSecondSearchElementName.Length > 0 && strSecondSearchValue.Length > 0)
            {
                strSearch = string.Format("descendant::{0}[{1}='{2}' and {3}='{4}']", strTagName, strSearchElementName, strSearchValue, strSecondSearchElementName, strSecondSearchValue);
            }
            else
            {
                m_strErrMsg = string.Format("Missing secondary paramater. Passed strSecondSearchElement = {0} and strSecondSearchValue = {1}", strSecondSearchElementName, strSecondSearchValue);
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(strFile);
            }
            catch (Exception err)
            {
                m_strErrMsg = err.Message;
            }

            XmlNode app;
            XmlNode root = doc.DocumentElement;

            app = root.SelectSingleNode(strSearch);


            if (app != null)
            {
                strRetVal = app[strReturnValueFromElementName].InnerText;
            }
            else
            {
                m_strErrMsg = string.Format("Could not find requested info for XML Path {0}", strSearch);
            }


            return strRetVal;
        }
        /// <summary>
        /// Converts XDocument (XmlNode) to a TreeViewNode. Requires a pointer to a TreeViewControl to
        /// convert the document.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="treeNodes"></param>
        static public void ConvertXmlNodeToTreeNode(XmlNode xmlNode, TreeNodeCollection treeNodes)
        {

            // add a treeNode node that represents this xmlNode
            TreeNode newTreeNode = treeNodes.Add(xmlNode.Name);

            // customise the tree node text based on the xmlNode
            // type and convert
            switch (xmlNode.NodeType)
            {
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.XmlDeclaration:
                    {
                        //newTreeNode.Text = "<?" + xmlNode.Name + " " + xmlNode.Value + "?>";
                        newTreeNode.Text = "<?" + xmlNode.Name + " " + xmlNode.Value + "?>";
                        break;
                    }
                case XmlNodeType.Element:
                    {
                        newTreeNode.Text = "<" + xmlNode.Name + ">";
                        break;
                    }
                case XmlNodeType.Attribute:
                    {
                        newTreeNode.Text = "ATTRIBUTE: " + xmlNode.Name;
                        break;
                    }
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                    {
                        newTreeNode.Text = "<!--" + xmlNode.Value + "-->";
                        break;
                    }

            }
            // call this routine recursively for each attribute.
            // (XmlAttribute is a subclass of XmlNode)
            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    ConvertXmlNodeToTreeNode(attribute, newTreeNode.Nodes);
                }
            }
            // call this routine recursively for each child node
            // typically, this child node represents a nested element, or element content
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                ConvertXmlNodeToTreeNode(childNode, newTreeNode.Nodes);
            }

        }

        /// <summary>
        /// overloaded to not require last two paramaters
        /// </summary>
        /// <param name="strTagName"></param>
        /// <param name="strSearchElementName"></param>
        /// <param name="strSearchValue"></param>
        /// <param name="strReturnValueFromElementName"></param>
        /// 
        /// <returns></returns>
        public string GetAnElementsText(string strTagName, string strSearchElementName, string strSearchValue, string strReturnValueFromElementName)
        {
            return GetAnElementsText(strTagName, strSearchElementName, strSearchValue, strReturnValueFromElementName, "", "");
        }

        /// <summary>
        /// Gets the value for a particular element in the XML file 
        /// used primary for configuration file for the Ford and HMS services for start up paramaters
        /// returns a empty string with failure also sets the RFCObject propErrMsg.
        /// NOTE: Search is case sensitive.
        /// </summary>
        /// <param name="strTagName"></param>
        /// <param name="strSearchElementName"></param>
        /// <param name="strReturnValueFromElementName"></param>
        /// <param name="strSearchValue"></param>
        /// <param name="strSecondSearchElementName"></param>
        /// <param name="strSecondSearchValue"></param>
        /// <returns>Empty string if no value is found or an error occurs. See propErrMsg for text of error</returns>
        public string GetAnElementsText(string strTagName, string strSearchElementName, string strSearchValue, string strReturnValueFromElementName, string strSecondSearchElementName, string strSecondSearchValue)
        {
            string strRetVal = "";
            string strSearch = "";

            if (m_strFile.Length == 0 || strTagName.Length == 0 || strSearchElementName.Length == 0 || strSearchValue.Length == 0 || strReturnValueFromElementName.Length == 0)
            {
                m_strErrMsg = "Invalid length on a paramater passed to GetAttribute()";
                throw new System.IndexOutOfRangeException();
            }

            if (strSecondSearchElementName.Length == 0 && strSecondSearchValue.Length == 0)
            {
                strSearch = string.Format("descendant::{0}[{1}='{2}']", strTagName, strSearchElementName, strSearchValue);
            }
            if (strSecondSearchElementName.Length > 0 && strSecondSearchValue.Length > 0)
            {
                strSearch = string.Format("descendant::{0}[{1}='{2}' and {3}='{4}']", strTagName, strSearchElementName, strSearchValue, strSecondSearchElementName, strSecondSearchValue);
            }
            else
            {
                m_strErrMsg = string.Format("Missing secondary paramater. Passed strSecondSearchElement = {0} and strSecondSearchValue = {1}", strSecondSearchElementName, strSecondSearchValue);
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(m_strFile);
            }
            catch (Exception err)
            {
                m_strErrMsg = err.Message;
            }

            XmlNode app;
            XmlNode root = doc.DocumentElement;
            //       book = root.SelectSingleNode("descendant::book[author/last-name='Crone']");

            app = root.SelectSingleNode(strSearch);


            if (app != null)
            {
                strRetVal = app[strReturnValueFromElementName].InnerText;
            }
            else
            {
                m_strErrMsg = string.Format("Could not find requested info for XML Path {0}", strSearch);
            }


            return strRetVal;
        }
        /// <summary>
        /// Converts XDocument (XmlNode) to a TreeViewNode. Requires a pointer to a TreeViewControl to
        /// convert the document.
        /// </summary>
        /// <param name="treeNodes"></param>
        public void ConvertXmlNodeToTreeNode(TreeNodeCollection treeNodes)
        {
            XmlNode xmlNode = m_xmlDoc;
            // add a treeNode node that represents this xmlNode
            TreeNode newTreeNode = treeNodes.Add(xmlNode.Name);

            // customise the tree node text based on the xmlNode
            // type and convert
            switch (xmlNode.NodeType)
            {
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.XmlDeclaration:
                    {
                        //newTreeNode.Text = "<?" + xmlNode.Name + " " + xmlNode.Value + "?>";
                        newTreeNode.Text = "<?" + xmlNode.Name + " " + xmlNode.Value + "?>";
                        break;
                    }
                case XmlNodeType.Element:
                    {
                        newTreeNode.Text = "<" + xmlNode.Name + ">";
                        break;
                    }
                case XmlNodeType.Attribute:
                    {
                        newTreeNode.Text = "ATTRIBUTE: " + xmlNode.Name;
                        break;
                    }
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                    {
                        newTreeNode.Text = "<!--" + xmlNode.Value + "-->";
                        break;
                    }

            }
            // call this routine recursively for each attribute.
            // (XmlAttribute is a subclass of XmlNode)
            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    ConvertXmlNodeToTreeNode(attribute, newTreeNode.Nodes);
                }
            }
            // call this routine recursively for each child node
            // typically, this child node represents a nested element, or element content
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                ConvertXmlNodeToTreeNode(childNode, newTreeNode.Nodes);
            }


        }

        /// <summary>
        /// Converts the string to a valid hl7 document. 
        /// returns xmdDocument? xmlNode? bool? needs work or all three
        /// </summary>
        /// <param name="strHL7">Unformatted string array</param>
        /// <param name="strFileName"></param>
        static public void ConvertHL7toXML(string[] strHL7, string strFileName)
        {
            int nItem = 0;
            XmlWriter writer = null;
            //       XmlReaderSettings xrsSettings = new XmlReaderSettings();
            //       xrsSettings.IgnoreComments = true;
            //       xrsSettings.IgnoreProcessingInstructions = true;
            //       xrsSettings.IgnoreWhitespace = true;
            //       xrsSettings.ValidationType = ValidationType.Schema;

            ////       XmlReader xr = XmlReader.Create(@"C:\HMS\HL7Schema.xsd" );
            ////       XmlSerializer serializer = new XmlSerializer(typeof(Schema));
            // //      Schema MsgHdrs = (Schema)serializer.Deserialize(xr);
            // //      string strAtt = xr.GetAttribute(0);
            //       XmlDocument reader = new XmlDocument();
            //       reader.Load(@"C:\HMS\HL7Schema.xsd");
            //   //    XmlNodeList xl = reader.GetElementsByTagName("msh");//strHL7_segment_id);
            //   //    XmlNode xn = reader.GetElementById("msh");

            //       //foreach (XmlNode nodeSchema in xl)
            //       //{
            //       //    string name = nodeSchema.Name;
            //       //    string xit = nodeSchema.InnerText;
            //       //    string xv = nodeSchema.Value;
            //       //    string xx = nodeSchema.InnerXml;

            //       //}
            //       string strDisplay = "";
            //       while (reader.Read())
            //       {
            //           if (reader.IsStartElement())
            //           {
            //               if (reader.IsEmptyElement)
            //                   strDisplay = reader.Name;
            //               else
            //               {
            //                   strDisplay = reader.Name;
            //                   reader.Read(); // Read the start tag.
            //                   if (reader.IsStartElement())  // Handle nested elements.
            //                   {
            //                       reader.ReadToDescendant("PID");
            //                       strDisplay = reader.Name;
            //                   }
            //                   strDisplay = reader.ReadString();  //Read the text content of the element.
            //               }
            //           }
            //       }


            //       using (XmlReader reader = XmlReader.Create(@"C:\HMS\HL7Schema.xsd", xrsSettings))
            //       {

            //           // Parse the XML document.  ReadString is used to 
            //           // read the text content of the elements.
            //           reader.Read();
            //           reader.ReadStartElement("element");

            //           string str = reader.ReadString();
            //           reader.ReadEndElement();

            //       }



            try
            {

                // Create an XmlWriterSettings object with the correct options. 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;

                // Create the XmlWriter object and write some content.
                writer = XmlWriter.Create(strFileName, settings);
                writer.WriteStartElement("element");
                // writer.WriteWhitespace("\t");

                foreach (string s in strHL7)
                {
                    writer.WriteStartElement("sub-element");
                    string[] arrSplit = s.Split(new char[] { '|' });
                    bool bIsMsh = false;
                    int nArrCount = -1;
                    nItem = 0;
                    foreach (string s1 in arrSplit)
                    {
                        nArrCount++;
                        if (arrSplit[0].IndexOf("msh") > -1)
                        {
                            bIsMsh = true;
                        }
                        if (bIsMsh && (nArrCount == 1))
                        {
                            writer.WriteCData(s);
                            bIsMsh = false;
                        }
                        else
                        {

                            writer.WriteElementString(string.Format("item{0}", nItem), s1);
                            nItem++;
                        }

                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();

            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

            XmlSerializer xs = new XmlSerializer(typeof(string[]));

            XmlDocument xdSchema = new XmlDocument();
            xdSchema.Load(@"C:\HMS\HL7Schema.xsd");

            XmlNodeList xNodeList = xdSchema.GetElementsByTagName("xs:element");//strHL7_segment_id);

            foreach (XmlNode nodeSchema in xNodeList)
            {
                string name = nodeSchema.OuterXml;
                string[] arrOuter = name.Split(new string[] { "<xs:element name=" }, StringSplitOptions.None);
                foreach (string s in arrOuter)
                {
                    int nStart = s.IndexOf('"');
                    if (nStart == -1) // no quote
                    {
                        continue;
                    }
                    if (s.IndexOf('"', (nStart + 1)) == -1) // no closing quote 
                    {
                        continue;
                    }

                    name = s.Substring(1, (s.IndexOf('"', 1) - 1));

                }



            }

        }

        /// <summary>
        /// Both switches uses different cases of the values because the file are case sensitive.
        /// </summary>
        /// <param name="strHL7Msg"></param>
        /// <param name="strFileName"></param>
        static public void ConvertHL7MsgToXMLFile(string strHL7Msg, string strFileName)
        {
            XmlDocument xdSchema = new XmlDocument(); // configure schema for usage
                                                      //try
                                                      //{
                                                      //    xdSchema.Load(string.Format(@"C:\HMS\HL7Schema.xsd", Application.StartupPath));
                                                      //   // xdSchema.Load(string.Format("{0}\\HL7Schema.xsd", Application.StartupPath));
                                                      //}
                                                      //catch (FileNotFoundException fnfe)
                                                      //{
                                                      //    m_strErrMsg = fnfe.Message;
            xdSchema.Load(@"C:\HMS\HL7Schema.xsd");

            //return;
            // }

            XmlNodeList xNodeList = xdSchema.GetElementsByTagName("xs:element");//strHL7_segment_id);
            StringCollection arrSchema = new StringCollection();

            foreach (XmlNode nodeSchema in xNodeList)
            {
                if (!nodeSchema.HasChildNodes)
                {
                    continue;
                }
                string name = nodeSchema.OuterXml;
                string[] arrOuter = name.Split(new string[] { "<xs:element name=" }, StringSplitOptions.None);
                string str = arrOuter[1].Substring(1, 3);
                switch (arrOuter[1].Substring(1, 3).ToLower())
                {
                    case "msh":
                        {
                            m_iStartMSH = arrSchema.Count + 1;
                            break;
                        }
                    case "pid":
                        {
                            m_iStartPID = arrSchema.Count + 1;
                            break;
                        }
                    case "obx":
                        {
                            m_iStartOBX = arrSchema.Count + 1;
                            break;
                        }
                    case "obr":
                        {
                            m_iStartOBR = arrSchema.Count + 1;
                            break;
                        }
                    case "orc":
                        {
                            m_iStartORC = arrSchema.Count + 1;
                            break;
                        }
                    case "nte":
                        {
                            m_iStartNTE = arrSchema.Count + 1;
                            break;
                        }
                    case "msa":
                        {
                            m_iStartMSA = arrSchema.Count + 1;
                            break;
                        }
                    case "ack":
                        {
                            m_iStartACK = arrSchema.Count + 1;
                            break;
                        }
                    case "in1":
                        {
                            m_iStartIN1 = arrSchema.Count + 1;
                            break;
                        }
                    case "in2": //wdk 20091021 added
                        {
                            m_iStartIN2 = arrSchema.Count + 1;
                            break;
                        }
                    case "pv1":
                        {
                            m_iStartPV1 = arrSchema.Count + 1;
                            break;
                        }
                    case "gt1":
                        {
                            m_iStartGT1 = arrSchema.Count + 1;
                            break;
                        }
                    case "zps":
                        {
                            m_iStartZPS = arrSchema.Count + 1;
                            break;
                        }
                    // rgc/wdk 20090312 added DG1
                    case "dg1":
                        {
                            m_iStartDG1 = arrSchema.Count + 1;
                            break;
                        }

                    default:
                        {
                            m_iStartZZZ = arrSchema.Count + 1;
                            break;
                        }
                }
                arrSchema.AddRange(arrOuter);
            }

            int nCount = arrSchema.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (arrSchema[i].Length == 0)
                {
                    arrSchema[i] = "";
                    continue;
                }

                arrSchema[i] = arrSchema[i].Substring(1, (arrSchema[i].IndexOf('"', 1) - 1));
            }


            // configure string for usage
            strHL7Msg = strHL7Msg.Replace(HL7.VT.ToString(), "");
            strHL7Msg = strHL7Msg.Replace(HL7.FS.ToString(), "");
            string[] arrHL7 = strHL7Msg.Split(new char[] { HL7.CR });
            int nItem = 0;
            XmlWriter writer = null;
            XmlSerializer xs = new XmlSerializer(typeof(string[]));
            try
            {

                // Create an XmlWriterSettings object with the correct options. 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;
                settings.CloseOutput = true;

                int nStartIndex = 1;
                string strSegType = "";
                // Create the XmlWriter object and write some content.
                writer = XmlWriter.Create(strFileName, settings);
                //<?xml-stylesheet type = "text/xsl" href="C:\source\bones\bones\StyleSheet_HL7.xslt"?>
                writer.WriteProcessingInstruction("xml-stylesheet", "type = \"text/xsl\" href=\"C:\\source\\bones\\bones\\StyleSheet_HL7.xslt\"");
                writer.WriteProcessingInstruction("xml-stylesheet", "type = \"text/css\" href=\"C:\\source\\bones\\bones\\StyleSheet1.css\"");

                // <?link rel=stylesheet type = "text/css" href="C:\source\bones\bones\StyleSheet1.css"?>
                writer.WriteStartElement("HL7Message"); //  entire HL7 message 
                foreach (string s in arrHL7)
                {
                    if (s.Length == 0) // blank line in message and handle last to carrage returns in message string. DO NOT PROCESS
                    {
                        continue;
                    }
                    m_bNonStandardXML = false;
                    switch (s.Substring(0, 3).ToUpper())
                    {
                        case "MSH":
                            {
                                nStartIndex = m_iStartMSH;
                                strSegType = "MSH";
                                break;
                            }
                        case "PID":
                            {
                                nStartIndex = m_iStartPID;
                                strSegType = "PID";
                                break;
                            }
                        case "ZPS":
                            {
                                nStartIndex = m_iStartZPS;
                                strSegType = "ZPS";
                                break;
                            }
                        case "OBX":
                            {
                                nStartIndex = m_iStartOBX;
                                strSegType = "OBX";
                                break;
                            }
                        case "OBR":
                            {
                                nStartIndex = m_iStartOBR;
                                strSegType = "OBR";
                                break;
                            }
                        case "ORC":
                            {
                                nStartIndex = m_iStartORC;
                                strSegType = "ORC";
                                break;
                            }
                        case "NTE":
                            {
                                nStartIndex = m_iStartNTE;
                                strSegType = "NTE";
                                break;
                            }
                        case "MSA":
                            {
                                nStartIndex = m_iStartMSA;
                                strSegType = "MSA";
                                break;
                            }
                        case "ACK":
                            {
                                nStartIndex = m_iStartACK;
                                strSegType = "ACK";
                                break;
                            }
                        case "IN1":
                            {
                                nStartIndex = m_iStartIN1;
                                strSegType = "IN1";
                                break;
                            }
                        case "IN2":
                            {
                                nStartIndex = m_iStartIN2;
                                strSegType = "IN2";
                                break;
                            }
                        case "PV1":
                            {
                                nStartIndex = m_iStartPV1;
                                strSegType = "PV1";
                                break;
                            }
                        case "GT1":
                            {
                                nStartIndex = m_iStartGT1;
                                strSegType = "GT1";
                                break;
                            }
                        // rgc/wdk 20090312 added DG1
                        case "DG1":
                            {
                                nStartIndex = m_iStartDG1;
                                strSegType = "DG1";
                                break;
                            }
                        default:
                            {
                                nStartIndex = m_iStartZZZ;
                                strSegType = "ZZZ"; // non standard segment
                                m_bNonStandardXML = true;
                                break;
                            }
                    }

                    writer.WriteStartElement(string.Format("{0}segment", strSegType));
                    string[] arrSplit = s.Split(new char[] { '|' });
                    bool bIsMsh = false;
                    int nArrCount = -1;
                    nItem = 0;


                    foreach (string s1 in arrSplit)
                    {
                        nArrCount++;
                        if (arrSplit[0].IndexOf("MSH") > -1)
                        {
                            bIsMsh = true;
                        }
                        if (bIsMsh && (nArrCount == 1))
                        {
                            writer.WriteElementString(string.Format("{0}", arrSchema[(nItem + nStartIndex)]), "|");
                            nItem++;
                            writer.WriteElementString(string.Format("{0}", arrSchema[(nItem + nStartIndex)]), s1);
                            bIsMsh = false;
                            nItem++;
                        }
                        else
                        {
                            string strLabel = "";
                            //string strType = "";
                            int nSchemaCount = arrSchema.Count;
                            if ((nItem + nStartIndex) < nSchemaCount)
                            {
                                strLabel = arrSchema[(nItem + nStartIndex)];

                            }

                            if (strLabel.Length == 0)
                            {
                                strLabel = "NONSTANDARD";
                                m_bNonStandardXML = true;
                            }

                            if (m_bNonStandardXML)
                            {
                                strLabel = "NONSTANDARD";
                            }

                            writer.WriteElementString(string.Format("{0}", strLabel), s1);
                            nItem++;
                        }

                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();


            }
            catch (Exception writerEx)
            {
                m_strErrMsg = writerEx.Message;
                if (writer != null)
                    writer.Close();
                return;
            }

            writer.Close();

        }


        /// <summary>
        /// Both switches uses different cases of the values because the file are case sensitive.
        /// </summary>
        /// <param name="strHL7Msg"></param>
        /// <param name="strFileName"></param>
        public void InstConvertHL7MsgToXMLFile(string strHL7Msg, string strFileName)
        {
            XmlDocument xdSchema = new XmlDocument(); // configure schema for usage
            //try
            //{
            //    xdSchema.Load(string.Format(@"C:\HMS\HL7Schema.xsd", Application.StartupPath));
            //   // xdSchema.Load(string.Format("{0}\\HL7Schema.xsd", Application.StartupPath));
            //}
            //catch (FileNotFoundException fnfe)
            //{
            //    m_strErrMsg = fnfe.Message;
            xdSchema.Load(@"C:\HMS\HL7Schema.xsd");

            //return;
            // }

            XmlNodeList xNodeList = xdSchema.GetElementsByTagName("xs:element");//strHL7_segment_id);
            StringCollection arrSchema = new StringCollection();

            foreach (XmlNode nodeSchema in xNodeList)
            {
                if (!nodeSchema.HasChildNodes)
                {
                    continue;
                }
                string name = nodeSchema.OuterXml;
                string[] arrOuter = name.Split(new string[] { "<xs:element name=" }, StringSplitOptions.None);
                string str = arrOuter[1].Substring(1, 3);
                switch (arrOuter[1].Substring(1, 3).ToLower())
                {
                    case "msh":
                        {
                            m_iStartMSH = arrSchema.Count + 1;
                            break;
                        }
                    case "pid":
                        {
                            m_iStartPID = arrSchema.Count + 1;
                            break;
                        }
                    case "obx":
                        {
                            m_iStartOBX = arrSchema.Count + 1;
                            break;
                        }
                    case "obr":
                        {
                            m_iStartOBR = arrSchema.Count + 1;
                            break;
                        }
                    case "orc":
                        {
                            m_iStartORC = arrSchema.Count + 1;
                            break;
                        }
                    case "nte":
                        {
                            m_iStartNTE = arrSchema.Count + 1;
                            break;
                        }
                    case "msa":
                        {
                            m_iStartMSA = arrSchema.Count + 1;
                            break;
                        }
                    case "ack":
                        {
                            m_iStartACK = arrSchema.Count + 1;
                            break;
                        }
                    case "in1":
                        {
                            m_iStartIN1 = arrSchema.Count + 1;
                            break;
                        }
                    case "pv1":
                        {
                            m_iStartPV1 = arrSchema.Count + 1;
                            break;
                        }
                    case "gt1":
                        {
                            m_iStartGT1 = arrSchema.Count + 1;
                            break;
                        }
                    case "zps":
                        {
                            m_iStartZPS = arrSchema.Count + 1;
                            break;
                        }
                    case "dg1":
                        {
                            m_iStartDG1 = arrSchema.Count + 1;
                            break;
                        }
                    default:
                        {
                            m_iStartZZZ = arrSchema.Count + 1;
                            break;
                        }
                }
                arrSchema.AddRange(arrOuter);
            }

            int nCount = arrSchema.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (arrSchema[i].Length == 0)
                {
                    arrSchema[i] = "";
                    continue;
                }

                arrSchema[i] = arrSchema[i].Substring(1, (arrSchema[i].IndexOf('"', 1) - 1));
            }


            // configure string for usage
            strHL7Msg = strHL7Msg.Replace(HL7.VT.ToString(), "");
            strHL7Msg = strHL7Msg.Replace(HL7.FS.ToString(), "");
            string[] arrHL7 = strHL7Msg.Split(new char[] { HL7.CR });
            int nItem = 0;
            XmlWriter writer = null;
            XmlSerializer xs = new XmlSerializer(typeof(string[]));
            try
            {

                // Create an XmlWriterSettings object with the correct options. 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;
                settings.CloseOutput = true;

                int nStartIndex = 1;
                string strSegType = "";
                // Create the XmlWriter object and write some content.
                writer = XmlWriter.Create(strFileName, settings);
                //<?xml-stylesheet type = "text/xsl" href="C:\source\bones\bones\StyleSheet_HL7.xslt"?>
                //   writer.WriteProcessingInstruction("xml-stylesheet", "type = \"text/xsl\" href=\"C:\\source\\bones\\bones\\StyleSheet_HL7.xslt\"");
                // writer.WriteProcessingInstruction("xml-stylesheet", "type = \"text/css\" href=\"C:\\source\\bones\\bones\\StyleSheet1.css\"");

                // <?link rel=stylesheet type = "text/css" href="C:\source\bones\bones\StyleSheet1.css"?>
                writer.WriteStartElement("HL7Message"); //  entire HL7 message 
                foreach (string s in arrHL7)
                {
                    if (s.Length == 0) // blank line in message and handle last to carrage returns in message string. DO NOT PROCESS
                    {
                        continue;
                    }
                    m_bNonStandardXML = false;
                    switch (s.Substring(0, 3).ToUpper())
                    {
                        case "MSH":
                            {
                                nStartIndex = m_iStartMSH;
                                strSegType = "MSH";
                                break;
                            }
                        case "PID":
                            {
                                nStartIndex = m_iStartPID;
                                strSegType = "PID";
                                break;
                            }
                        case "ZPS":
                            {
                                nStartIndex = m_iStartZPS;
                                strSegType = "ZPS";
                                break;
                            }
                        case "OBX":
                            {
                                nStartIndex = m_iStartOBX;
                                strSegType = "OBX";
                                break;
                            }
                        case "OBR":
                            {
                                nStartIndex = m_iStartOBR;
                                strSegType = "OBR";
                                break;
                            }
                        case "ORC":
                            {
                                nStartIndex = m_iStartORC;
                                strSegType = "ORC";
                                break;
                            }
                        case "NTE":
                            {
                                nStartIndex = m_iStartNTE;
                                strSegType = "NTE";
                                break;
                            }
                        case "MSA":
                            {
                                nStartIndex = m_iStartMSA;
                                strSegType = "MSA";
                                break;
                            }
                        case "ACK":
                            {
                                nStartIndex = m_iStartACK;
                                strSegType = "ACK";
                                break;
                            }
                        case "IN1":
                            {
                                nStartIndex = m_iStartIN1;
                                strSegType = "IN1";
                                break;
                            }
                        case "PV1":
                            {
                                nStartIndex = m_iStartPV1;
                                strSegType = "PV1";
                                break;
                            }
                        case "GT1":
                            {
                                nStartIndex = m_iStartGT1;
                                strSegType = "GT1";
                                break;
                            }
                        case "DG1":
                            {
                                nStartIndex = m_iStartDG1;
                                strSegType = "DG1";
                                break;
                            }
                        default:
                            {
                                nStartIndex = m_iStartZZZ;
                                strSegType = "ZZZ"; // non standard segment
                                m_bNonStandardXML = true;
                                break;
                            }
                    }

                    writer.WriteStartElement(string.Format("{0}segment", strSegType));
                    string[] arrSplit = s.Split(new char[] { '|' });
                    bool bIsMsh = false;
                    int nArrCount = -1;
                    nItem = 0;


                    foreach (string s1 in arrSplit)
                    {
                        nArrCount++;
                        if (arrSplit[0].IndexOf("MSH") > -1)
                        {
                            bIsMsh = true;
                        }
                        if (bIsMsh && (nArrCount == 1))
                        {
                            writer.WriteElementString(string.Format("{0}", arrSchema[(nItem + nStartIndex)]), "|");
                            nItem++;
                            writer.WriteElementString(string.Format("{0}", arrSchema[(nItem + nStartIndex)]), s1);
                            bIsMsh = false;
                            nItem++;
                        }
                        else
                        {
                            string strLabel = "";
                            //string strType = "";
                            int nSchemaCount = arrSchema.Count;
                            if ((nItem + nStartIndex) < nSchemaCount)
                            {
                                strLabel = arrSchema[(nItem + nStartIndex)];

                            }

                            if (strLabel.Length == 0)
                            {
                                strLabel = "NONSTANDARD";
                                m_bNonStandardXML = true;
                            }

                            if (m_bNonStandardXML)
                            {
                                strLabel = "NONSTANDARD";
                            }

                            writer.WriteElementString(string.Format("{0}", strLabel), s1);
                            nItem++;
                        }

                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();


            }
            catch (Exception writerEx)
            {
                m_strErrMsg = writerEx.Message;
                if (writer != null)
                    writer.Close();
                return;
            }

            writer.Close();

        }

        /// <summary>
        /// 07/27/2007 Convert an X12 file to an XML file for viewing in the XML viewer.
        /// The X12Schema should be in the Application's Directory
        /// </summary>
        /// <param name="strX12File">X12File with directory info</param>
        /// <param name="strXMLFile">Output file in XML format</param>
        static public void ConvertX12FileToXMLFile(string strX12File, string strXMLFile)
        {
            ClearX12Variables();
            XmlDocument xdSchema = new XmlDocument();
            try
            {
                //   xdSchema.Load(string.Format("{0}\\XMLSchemaX12", Application.StartupPath));
                // for testing
                xdSchema.Load(@"C:\source\Util\ViewerX12\ViewerX12\XMLSchemaX12.xsd");
            }
            catch (FileNotFoundException fnfe) // can't find the schema in the directory
            {
                m_strErrMsg = fnfe.Message;
                return;
            }
            XmlNodeList xNodeList = xdSchema.GetElementsByTagName("xs:element");
            StringCollection arrSchema = new StringCollection();
            foreach (XmlNode nodeSchema in xNodeList)
            {
                if (!nodeSchema.HasChildNodes)
                {
                    continue;
                }
                string name = nodeSchema.OuterXml;
                string[] arrOuter = name.Split(new string[] { "<xs:element name=" }, StringSplitOptions.None);
                string str = arrOuter[1].Substring(1, 3);
                str = str.Trim('"');
                //switch (arrOuter[1].Substring(1, 3))
                switch (str)
                {
                    case "ISA":
                        {
                            m_iStartISA = arrSchema.Count + 1;
                            break;
                        }
                    case "IEA":
                        {
                            m_iStartIEA = arrSchema.Count + 1;
                            break;
                        }
                    case "GS":
                        {
                            m_iStartGS = arrSchema.Count + 1;
                            break;
                        }
                    case "GE":
                        {
                            m_iStartGE = arrSchema.Count + 1;
                            break;
                        }
                    case "ST":
                        {
                            m_iStartST = arrSchema.Count + 1;
                            break;
                        }
                    case "SE":
                        {
                            m_iStartSE = arrSchema.Count + 1;
                            break;
                        }
                    case "BHT":
                        {
                            m_iStartBHT = arrSchema.Count + 1;
                            break;
                        }
                    case "DTP":
                        {
                            m_iStartDTP = arrSchema.Count + 1;
                            break;
                        }
                    case "REF":
                        {
                            m_iStartREF = arrSchema.Count + 1;
                            break;
                        }
                    case "NM1":
                        {
                            m_iStartNM1 = arrSchema.Count + 1;
                            break;
                        }
                    case "PER":
                        {
                            m_iStartPER = arrSchema.Count + 1;
                            break;
                        }
                    case "HL":
                        {
                            m_iStartHL = arrSchema.Count + 1;
                            break;
                        }
                    case "PRV":
                        {
                            m_iStartPRV = arrSchema.Count + 1;
                            break;
                        }
                    case "N1":
                        {
                            m_iStartN1 = arrSchema.Count + 1;
                            break;
                        }
                    case "N2":
                        {
                            m_iStartN2 = arrSchema.Count + 1;
                            break;
                        }
                    case "N3":
                        {
                            m_iStartN3 = arrSchema.Count + 1;
                            break;
                        }
                    case "N4":
                        {
                            m_iStartN4 = arrSchema.Count + 1;
                            break;
                        }
                    case "SBR":
                        {
                            m_iStartSBR = arrSchema.Count + 1;
                            break;
                        }
                    case "DMG":
                        {
                            m_iStartDMG = arrSchema.Count + 1;
                            break;
                        }
                    case "CLM":
                        {
                            m_iStartCLM = arrSchema.Count + 1;
                            break;
                        }
                    case "HI":
                        {
                            m_iStartHI = arrSchema.Count + 1;
                            break;
                        }
                    case "LX":
                        {
                            m_iStartLX = arrSchema.Count + 1;
                            break;
                        }
                    case "SV1":
                        {
                            m_iStartSV1 = arrSchema.Count + 1;
                            break;
                        }
                    case "PAT":
                        {
                            m_iStartPAT = arrSchema.Count + 1;
                            break;
                        }
                    case "SVC06":
                        {
                            m_iStartSVC06 = arrSchema.Count + 1;
                            break;
                        }
                    case "SVC01":
                        {
                            m_iStartSVC01 = arrSchema.Count + 1;
                            break;
                        }
                    case "SVC":
                        {
                            m_iStartSVC = arrSchema.Count + 1;
                            break;
                        }
                    case "QTY":
                        {
                            m_iStartQTY = arrSchema.Count + 1;
                            break;
                        }
                    case "AMT":
                        {
                            m_iStartAMT = arrSchema.Count + 1;
                            break;
                        }
                    case "CAS":
                        {
                            m_iStartCAS = arrSchema.Count + 1;
                            break;
                        }
                    case "CLP":
                        {
                            m_iStartCLP = arrSchema.Count + 1;
                            break;
                        }
                    case "TS3":
                        {
                            m_iStartTS3 = arrSchema.Count + 1;
                            break;
                        }
                    case "DTM":
                        {
                            m_iStartDTM = arrSchema.Count + 1;
                            break;
                        }
                    case "TRN":
                        {
                            m_iStartTRN = arrSchema.Count + 1;
                            break;
                        }
                    case "BPR":
                        {
                            m_iStartBPR = arrSchema.Count + 1;
                            break;
                        }
                    case "PLB":
                        {
                            m_iStartPLB = arrSchema.Count + 1;
                            break;
                        }
                    case "LQ":
                        {
                            m_iStartLQ = arrSchema.Count + 1;
                            break;
                        }

                    default:
                        {
                            m_iStartXXX = arrSchema.Count + 1;
                            break;
                        }
                }
                arrSchema.AddRange(arrOuter);
            }
            int nCount = arrSchema.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (arrSchema[i].Length == 0)
                {
                    arrSchema[i] = "";
                    continue;
                }

                arrSchema[i] = arrSchema[i].Substring(1, (arrSchema[i].IndexOf('"', 1) - 1));
            }
            //////////////////
            // configure file for usage
            StreamReader srX12;
            try
            {

                srX12 = new StreamReader(strX12File);
            }
            catch (FileNotFoundException fnfe)
            {
                m_strErrMsg = fnfe.Message;
                return;
            }
            string strX12 = srX12.ReadToEnd();
            srX12.Close();
            string[] arrX12 = strX12.Split(new char[] { '~' });
            int nItem = 0;
            XmlWriter writer = null;
            XmlSerializer xs = new XmlSerializer(typeof(string[]));
            try
            {

                // Create an XmlWriterSettings object with the correct options. 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;
                settings.CloseOutput = true;

                int nStartIndex = 1;
                string strSegType = "";
                // Create the XmlWriter object and write some content.
                writer = XmlWriter.Create(strXMLFile, settings);
                writer.WriteStartElement("X12_FILE"); //  entire X12 file
                foreach (string s in arrX12)
                {
                    string str = s.Trim(new char[] { '\r', '\n', ' ' });
                    // str = str.TrimStart(new char[] { ' ' });
                    if (str.Length == 0) // blank line in message and handle last to carrage returns in message string. DO NOT PROCESS
                    {
                        continue;
                    }
                    m_bNonStandardXML = false;
                    switch (str.Substring(0, s.IndexOf('*')))
                    {
                        case "ISA": // interchange control headers
                            {
                                nStartIndex = m_iStartISA;
                                strSegType = "ISA-header";
                                break;
                            }
                        case "IEA": // interchange control trailer
                            {
                                nStartIndex = m_iStartIEA;
                                strSegType = "IEA";
                                break;
                            }
                        case "GS": //functional group header
                            {
                                nStartIndex = m_iStartGS;
                                strSegType = "GS";
                                break;
                            }
                        case "GE": // functional group trailer
                            {
                                nStartIndex = m_iStartGE;
                                strSegType = "GE";
                                break;
                            }
                        case "ST":
                            {
                                nStartIndex = m_iStartST;
                                strSegType = "ST";
                                break;
                            }
                        case "SE":
                            {
                                nStartIndex = m_iStartSE;
                                strSegType = "SE";
                                break;
                            }
                        case "BHT":
                            {
                                nStartIndex = m_iStartBHT;
                                strSegType = "BHT";
                                break;
                            }
                        case "DTP":
                            {
                                nStartIndex = m_iStartDTP;
                                strSegType = "DTP";
                                break;
                            }
                        case "REF":
                            {
                                nStartIndex = m_iStartREF;
                                strSegType = "REF";
                                break;
                            }
                        case "NM1":
                            {
                                nStartIndex = m_iStartNM1;
                                strSegType = "NM1";
                                break;
                            }
                        case "PER":
                            {
                                nStartIndex = m_iStartPER;
                                strSegType = "PER";
                                break;
                            }
                        case "HL":
                            {
                                nStartIndex = m_iStartHL;
                                strSegType = "HL";
                                break;
                            }
                        case "PRV":
                            {
                                nStartIndex = m_iStartPRV;
                                strSegType = "PRV";
                                break;
                            }
                        case "N1":
                            {
                                nStartIndex = m_iStartN1;
                                strSegType = "N1";
                                break;
                            }
                        case "N2":
                            {
                                nStartIndex = m_iStartN2;
                                strSegType = "N2";
                                break;
                            }
                        case "N3":
                            {
                                nStartIndex = m_iStartN3;
                                strSegType = "N3";
                                break;
                            }
                        case "N4":
                            {
                                nStartIndex = m_iStartN4;
                                strSegType = "N4";
                                break;
                            }
                        case "SBR":
                            {
                                nStartIndex = m_iStartSBR;
                                strSegType = "SBR";
                                break;
                            }
                        case "DMG":
                            {
                                nStartIndex = m_iStartDMG;
                                strSegType = "DMG";
                                break;
                            }
                        case "CLM":
                            {
                                nStartIndex = m_iStartCLM;
                                strSegType = "CLM";
                                break;
                            }
                        case "HI":
                            {
                                nStartIndex = m_iStartHI;
                                strSegType = "HI";
                                break;
                            }
                        case "LX":
                            {
                                nStartIndex = m_iStartLX;
                                strSegType = "LX";
                                break;
                            }

                        case "SV1":
                            {
                                nStartIndex = m_iStartSV1;
                                strSegType = "SV1";
                                break;
                            }

                        case "PAT":
                            {
                                nStartIndex = m_iStartPAT;
                                strSegType = "PAT";
                                break;
                            }
                        case "SVC06":
                            {
                                nStartIndex = m_iStartSVC06;
                                strSegType = "SVC06";
                                break;
                            }
                        case "SVC01":
                            {
                                nStartIndex = m_iStartSVC01;
                                strSegType = "SVC01";
                                break;
                            }
                        case "SVC":
                            {
                                nStartIndex = m_iStartSVC;
                                strSegType = "SVC";
                                break;
                            }
                        case "QTY":
                            {
                                nStartIndex = m_iStartQTY;
                                strSegType = "QTY";
                                break;
                            }
                        case "AMT":
                            {
                                nStartIndex = m_iStartAMT;
                                strSegType = "AMT";
                                break;
                            }
                        case "CAS":
                            {
                                nStartIndex = m_iStartCAS;
                                strSegType = "CAS";
                                break;
                            }
                        case "CLP":
                            {
                                nStartIndex = m_iStartCLP;
                                strSegType = "CLP";
                                break;
                            }
                        case "TS3":
                            {
                                nStartIndex = m_iStartTS3;
                                strSegType = "TS3";
                                break;
                            }
                        case "DTM":
                            {
                                nStartIndex = m_iStartDTM;
                                strSegType = "DTM";
                                break;
                            }
                        case "TRN":
                            {
                                nStartIndex = m_iStartTRN;
                                strSegType = "TRN";
                                break;
                            }
                        case "BPR":
                            {
                                nStartIndex = m_iStartBPR;
                                strSegType = "BPR";
                                break;
                            }

                        case "PLB":
                            {
                                nStartIndex = m_iStartPLB;
                                strSegType = "PLB";
                                break;
                            }
                        case "LQ":
                            {
                                nStartIndex = m_iStartLQ;
                                strSegType = "LQ";
                                break;
                            }

                        default:
                            {
                                nStartIndex = m_iStartXXX;
                                strSegType = "UNKNOWN"; // non standard segment
                                // m_bNonStandardX12 = true;
                                break;
                            }
                    }

                    writer.WriteStartElement(string.Format("{0}", strSegType));
                    string[] arrSplit = str.Split(new char[] { '*' });
                    int nArrCount = -1;
                    nItem = 0;

                    foreach (string s1 in arrSplit)
                    {
                        nArrCount++;

                        string strLabel = "";
                        int nSchemaCount = arrSchema.Count;
                        if ((nItem + nStartIndex) < nSchemaCount)
                        {
                            strLabel = arrSchema[(nItem + nStartIndex)];
                        }
                        if (strLabel.Length == 0)
                        {
                            strLabel = "NON_STANDARD";
                            m_bNonStandardXML = true;
                        }
                        if (m_bNonStandardXML)
                        {
                            strLabel = "NON_STANDARD";
                        }
                        writer.WriteElementString(string.Format("{0}", strLabel), s1);
                        nItem++;

                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();


            }
            catch (Exception writerEx)
            {
                m_strErrMsg = writerEx.Message;
                if (writer != null)
                    writer.Close();
                return;
            }

            writer.Close();


            //////////////////
        }

        /// <summary>
        /// 07/27/2007 Convert an X12 file to an XML file for viewing in the XML viewer.
        /// The X12Schema should be in the Application's Directory
        /// </summary>
        /// <param name="strX12">X12 string info</param>
        /// <param name="strXMLFile">Output file in XML format</param>
        static public void ConvertX12ToXMLFile(string strX12, string strXMLFile)
        {
            ClearX12Variables();
            XmlDocument xdSchema = new XmlDocument();
            try
            {
                //   xdSchema.Load(string.Format("{0}\\XMLSchemaX12", Application.StartupPath));
                // for testing
                xdSchema.Load(@"C:\source\Util\ViewerX12\ViewerX12\XMLSchemaX12.xsd");
            }
            catch (FileNotFoundException fnfe) // can't find the schema in the directory
            {
                m_strErrMsg = fnfe.Message;
                return;
            }
            XmlNodeList xNodeList = xdSchema.GetElementsByTagName("xs:element");
            StringCollection arrSchema = new StringCollection();
            foreach (XmlNode nodeSchema in xNodeList)
            {
                if (!nodeSchema.HasChildNodes)
                {
                    continue;
                }
                string name = nodeSchema.OuterXml;
                string[] arrOuter = name.Split(new string[] { "<xs:element name=" }, StringSplitOptions.None);
                string str = arrOuter[1].Substring(1, 3);
                str = str.Trim('"');
                //switch (arrOuter[1].Substring(1, 3))
                switch (str)
                {
                    case "ISA":
                        {
                            m_iStartISA = arrSchema.Count + 1;
                            break;
                        }
                    case "IEA":
                        {
                            m_iStartIEA = arrSchema.Count + 1;
                            break;
                        }
                    case "GS":
                        {
                            m_iStartGS = arrSchema.Count + 1;
                            break;
                        }
                    case "GE":
                        {
                            m_iStartGE = arrSchema.Count + 1;
                            break;
                        }
                    case "ST":
                        {
                            m_iStartST = arrSchema.Count + 1;
                            break;
                        }
                    case "SE":
                        {
                            m_iStartSE = arrSchema.Count + 1;
                            break;
                        }
                    case "BHT":
                        {
                            m_iStartBHT = arrSchema.Count + 1;
                            break;
                        }
                    case "DTP":
                        {
                            m_iStartDTP = arrSchema.Count + 1;
                            break;
                        }
                    case "REF":
                        {
                            m_iStartREF = arrSchema.Count + 1;
                            break;
                        }
                    case "NM1":
                        {
                            m_iStartNM1 = arrSchema.Count + 1;
                            break;
                        }
                    case "PER":
                        {
                            m_iStartPER = arrSchema.Count + 1;
                            break;
                        }
                    case "HL":
                        {
                            m_iStartHL = arrSchema.Count + 1;
                            break;
                        }
                    case "PRV":
                        {
                            m_iStartPRV = arrSchema.Count + 1;
                            break;
                        }
                    case "N1":
                        {
                            m_iStartN1 = arrSchema.Count + 1;
                            break;
                        }
                    case "N2":
                        {
                            m_iStartN2 = arrSchema.Count + 1;
                            break;
                        }
                    case "N3":
                        {
                            m_iStartN3 = arrSchema.Count + 1;
                            break;
                        }
                    case "N4":
                        {
                            m_iStartN4 = arrSchema.Count + 1;
                            break;
                        }
                    case "SBR":
                        {
                            m_iStartSBR = arrSchema.Count + 1;
                            break;
                        }
                    case "DMG":
                        {
                            m_iStartDMG = arrSchema.Count + 1;
                            break;
                        }
                    case "CLM":
                        {
                            m_iStartCLM = arrSchema.Count + 1;
                            break;
                        }
                    case "HI":
                        {
                            m_iStartHI = arrSchema.Count + 1;
                            break;
                        }
                    case "LX":
                        {
                            m_iStartLX = arrSchema.Count + 1;
                            break;
                        }
                    case "SV1":
                        {
                            m_iStartSV1 = arrSchema.Count + 1;
                            break;
                        }
                    case "PAT":
                        {
                            m_iStartPAT = arrSchema.Count + 1;
                            break;
                        }
                    case "SVC06":
                        {
                            m_iStartSVC06 = arrSchema.Count + 1;
                            break;
                        }
                    case "SVC01":
                        {
                            m_iStartSVC01 = arrSchema.Count + 1;
                            break;
                        }
                    case "SVC":
                        {
                            m_iStartSVC = arrSchema.Count + 1;
                            break;
                        }
                    case "QTY":
                        {
                            m_iStartQTY = arrSchema.Count + 1;
                            break;
                        }
                    case "AMT":
                        {
                            m_iStartAMT = arrSchema.Count + 1;
                            break;
                        }
                    case "CAS":
                        {
                            m_iStartCAS = arrSchema.Count + 1;
                            break;
                        }
                    case "CLP":
                        {
                            m_iStartCLP = arrSchema.Count + 1;
                            break;
                        }
                    case "TS3":
                        {
                            m_iStartTS3 = arrSchema.Count + 1;
                            break;
                        }
                    case "DTM":
                        {
                            m_iStartDTM = arrSchema.Count + 1;
                            break;
                        }
                    case "TRN":
                        {
                            m_iStartTRN = arrSchema.Count + 1;
                            break;
                        }
                    case "BPR":
                        {
                            m_iStartBPR = arrSchema.Count + 1;
                            break;
                        }


                    default:
                        {
                            m_iStartXXX = arrSchema.Count + 1;
                            break;
                        }
                }
                arrSchema.AddRange(arrOuter);
            }
            int nCount = arrSchema.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (arrSchema[i].Length == 0)
                {
                    arrSchema[i] = "";
                    continue;
                }

                arrSchema[i] = arrSchema[i].Substring(1, (arrSchema[i].IndexOf('"', 1) - 1));
            }
            //////////////////
            // configure file for usage
            //StreamReader srX12;
            //try
            //{

            //    srX12 = new StreamReader(strX12File);
            //}
            //catch (FileNotFoundException fnfe)
            //{
            //    m_strErrMsg = fnfe.Message;
            //    return;
            //}

            //string strX12 = srX12.ReadToEnd();

            //srX12.Close();
            string[] arrX12 = strX12.Split(new char[] { '~' });
            int nItem = 0;
            XmlWriter writer = null;
            XmlSerializer xs = new XmlSerializer(typeof(string[]));
            try
            {

                // Create an XmlWriterSettings object with the correct options. 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;
                settings.CloseOutput = true;

                int nStartIndex = 1;
                string strSegType = "";
                // Create the XmlWriter object and write some content.
                writer = XmlWriter.Create(strXMLFile, settings);
                writer.WriteStartElement("X12_FILE"); //  entire X12 file
                foreach (string s in arrX12)
                {
                    string str = s.Trim(new char[] { '\r', '\n', ' ' });
                    //str = str.TrimStart(new char[] { ' ' });

                    if (str.Length == 0) // blank line in message and handle last to carrage returns in message string. DO NOT PROCESS
                    {
                        continue;
                    }
                    m_bNonStandardXML = false;
                    switch (str.Substring(0, str.IndexOf('*')))
                    {
                        case "ISA": // interchange control headers
                            {
                                nStartIndex = m_iStartISA;
                                strSegType = "ISA";
                                break;
                            }
                        case "IEA": // interchange control trailer
                            {
                                nStartIndex = m_iStartIEA;
                                strSegType = "IEA";
                                break;
                            }
                        case "GS": //functional group header
                            {
                                nStartIndex = m_iStartGS;
                                strSegType = "GS";
                                break;
                            }
                        case "GE": // functional group trailer
                            {
                                nStartIndex = m_iStartGE;
                                strSegType = "GE";
                                break;
                            }
                        case "ST":
                            {
                                nStartIndex = m_iStartST;
                                strSegType = "ST";
                                break;
                            }
                        case "SE":
                            {
                                nStartIndex = m_iStartSE;
                                strSegType = "SE";
                                break;
                            }
                        case "BHT":
                            {
                                nStartIndex = m_iStartBHT;
                                strSegType = "BHT";
                                break;
                            }
                        case "DTP":
                            {
                                nStartIndex = m_iStartDTP;
                                strSegType = "DTP";
                                break;
                            }
                        case "REF":
                            {
                                nStartIndex = m_iStartREF;
                                strSegType = "REF";
                                break;
                            }
                        case "NM1":
                            {
                                nStartIndex = m_iStartNM1;
                                strSegType = "NM1";
                                break;
                            }
                        case "PER":
                            {
                                nStartIndex = m_iStartPER;
                                strSegType = "PER";
                                break;
                            }
                        case "HL":
                            {
                                nStartIndex = m_iStartHL;
                                strSegType = "HL";
                                break;
                            }
                        case "PRV":
                            {
                                nStartIndex = m_iStartPRV;
                                strSegType = "PRV";
                                break;
                            }
                        case "N2":
                            {
                                nStartIndex = m_iStartN2;
                                strSegType = "N2";
                                break;
                            }
                        case "N3":
                            {
                                nStartIndex = m_iStartN3;
                                strSegType = "N3";
                                break;
                            }
                        case "N4":
                            {
                                nStartIndex = m_iStartN4;
                                strSegType = "N4";
                                break;
                            }
                        case "SBR":
                            {
                                nStartIndex = m_iStartSBR;
                                strSegType = "SBR";
                                break;
                            }
                        case "DMG":
                            {
                                nStartIndex = m_iStartDMG;
                                strSegType = "DMG";
                                break;
                            }
                        case "CLM":
                            {
                                nStartIndex = m_iStartCLM;
                                //strSegType = "CLM";
                                strSegType = "Claim Payment Information";
                                break;
                            }
                        case "HI":
                            {
                                nStartIndex = m_iStartHI;
                                strSegType = "HI";
                                break;
                            }
                        case "LX":
                            {
                                nStartIndex = m_iStartLX;
                                strSegType = "LX";
                                break;
                            }
                        case "SV1":
                            {
                                nStartIndex = m_iStartSV1;
                                strSegType = "SV1";
                                break;
                            }
                        case "PAT":
                            {
                                nStartIndex = m_iStartPAT;
                                strSegType = "PAT";
                                break;
                            }
                        case "SVC06":
                            {
                                nStartIndex = m_iStartSVC06;
                                strSegType = "SVC06";
                                break;
                            }
                        case "SVC01":
                            {
                                nStartIndex = m_iStartSVC01;
                                strSegType = "SVC01";
                                break;
                            }
                        case "SVC":
                            {
                                nStartIndex = m_iStartSVC;
                                strSegType = "SVC";
                                break;
                            }
                        case "QTY":
                            {
                                nStartIndex = m_iStartQTY;
                                strSegType = "QTY";
                                break;
                            }
                        case "AMT":
                            {
                                nStartIndex = m_iStartAMT;
                                strSegType = "AMT";
                                break;
                            }
                        case "CAS":
                            {
                                nStartIndex = m_iStartCAS;
                                strSegType = "CAS";
                                break;
                            }
                        case "CLP":
                            {
                                nStartIndex = m_iStartCLP;
                                strSegType = "CLP";
                                break;
                            }
                        case "TS3":
                            {
                                nStartIndex = m_iStartTS3;
                                strSegType = "TS3";
                                break;
                            }
                        case "DTM":
                            {
                                nStartIndex = m_iStartDTM;
                                strSegType = "DTM";
                                break;
                            }
                        case "TRN":
                            {
                                nStartIndex = m_iStartTRN;
                                strSegType = "TRN";
                                break;
                            }
                        case "BPR":
                            {
                                nStartIndex = m_iStartBPR;
                                strSegType = "BPR";
                                break;
                            }


                        default:
                            {
                                nStartIndex = m_iStartXXX;
                                strSegType = "XXX"; // non standard segment
                                // m_bNonStandardX12 = true;
                                break;
                            }
                    }

                    writer.WriteStartElement(string.Format("{0}", strSegType));
                    string[] arrSplit = str.Split(new char[] { '*' });
                    int nArrCount = -1;
                    nItem = 0;

                    foreach (string s1 in arrSplit)
                    {
                        nArrCount++;

                        string strLabel = "";
                        int nSchemaCount = arrSchema.Count;
                        if ((nItem + nStartIndex) < nSchemaCount)
                        {
                            strLabel = arrSchema[(nItem + nStartIndex)];
                        }
                        if (strLabel.Length == 0)
                        {
                            strLabel = "NON_STANDARD";
                            m_bNonStandardXML = true;
                        }
                        if (m_bNonStandardXML)
                        {
                            strLabel = "NON_STANDARD";
                        }
                        writer.WriteElementString(string.Format("{0}", strLabel), s1);
                        nItem++;

                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();


            }
            catch (Exception writerEx)
            {
                m_strErrMsg = writerEx.Message;
                if (writer != null)
                    writer.Close();
                return;
            }

            writer.Close();


            //////////////////
        }

        static private void ClearX12Variables()
        {
            m_iStartISA = 0; // Interchange control Header
            m_iStartIEA = 0; // Interchange control Trailer
            m_iStartGS = 0; // functional group header
            m_iStartGE = 0; // functional group trailer
            m_iStartST = 0; // Transaction Set Header
            m_iStartSE = 0; // Transaction Set trailer
            m_iStartBHT = 0; // Beginning of heirarchical transaction
            m_iStartDTP = 0; // Date or time or period
            m_iStartREF = 0; // Reference Identification
            m_iStartNM1 = 0; // Individual or Organization Name
            m_iStartPER = 0; // Administrative Communications Contact
            m_iStartHL = 0; // Billing/Pay-To Provider Hierarchical Level
            m_iStartPRV = 0; // Billing/Pay-To Provider Specialty Information
            m_iStartN2 = 0; // Additional Responsible Party Name Information
            m_iStartN3 = 0; // Responsible Party Address
            m_iStartN4 = 0; // Responsible Party City/State/Zip Code
            m_iStartSBR = 0; // Subscriber Info
            m_iStartDMG = 0; // Subscriber Demographics Info
            m_iStartCLM = 0; // Claim Information
            m_iStartHI = 0; // Health Care Diagnosis Code
            m_iStartLX = 0; // Service Line
            m_iStartSV1 = 0; // Professional Service
            m_iStartPAT = 0; // Patient Information
            m_iStartN1 = 0;
            m_iStartSVC = 0; // Service Payment Information
            m_iStartSVC01 = 0; // Service Payment Information field 1 definition
            m_iStartSVC06 = 0; // Service Payment Information field 6 definition
            m_iStartQTY = 0; // Claim Supplemental Information Quantity
            m_iStartAMT = 0; // Claim Supplemental Information
            m_iStartCAS = 0; // Claim Adjustment
            m_iStartCLP = 0; // Claim Payment Information
            m_iStartTS3 = 0; // Provider Summary Information
            m_iStartDTM = 0; // Date (See the first field for the type of date. i.e. Claim, processing, service etc)
            m_iStartTRN = 0; // Reassociation Trace Number
            m_iStartBPR = 0; // Financial Information
            m_iStartPLB = 0;
            m_iStartLQ = 0;


        }


        /// <summary>
        /// Both switches uses different cases of the values because the file are case sensitive.
        /// </summary>
        /// <param name="strHL7Msg"></param>
        /// <param name="strFileName"></param>
        static public void ConvertHL7MsgToXMLFileByNode(string strHL7Msg, string strFileName)
        {
            XmlDocument xdSchema = new XmlDocument(); // configure schema for usage
            try
            {
                xdSchema.Load(string.Format("{0}\\HL7Schema.xsd", Application.StartupPath));
            }
            catch (FileNotFoundException fnfe)
            {
                m_strErrMsg = fnfe.Message;
                xdSchema.Load(@"C:\HMS\HL7Schema.xsd");

                //return;
            }
            // Major Elements are MSH, PID, OBR etc
            XmlNodeList xNodeList = xdSchema.GetElementsByTagName("xs:element");//strHL7_segment_id);
            StringCollection arrSchema = new StringCollection();

            foreach (XmlNode nodeSchema in xNodeList)
            {
                if (!nodeSchema.HasChildNodes)
                {
                    continue;
                }
                string name = nodeSchema.OuterXml;
                string[] arrOuter = name.Split(new string[] { "<xs:element name=" }, StringSplitOptions.None);
                string str = arrOuter[1].Substring(1, 3);
                switch (arrOuter[1].Substring(1, 3).ToLower())
                {
                    case "msh":
                        {
                            m_iStartMSH = arrSchema.Count + 1;
                            break;
                        }
                    case "pid":
                        {
                            m_iStartPID = arrSchema.Count + 1;
                            break;
                        }
                    case "obx":
                        {
                            m_iStartOBX = arrSchema.Count + 1;
                            break;
                        }
                    case "obr":
                        {
                            m_iStartOBR = arrSchema.Count + 1;
                            break;
                        }
                    case "orc":
                        {
                            m_iStartORC = arrSchema.Count + 1;
                            break;
                        }
                    case "nte":
                        {
                            m_iStartNTE = arrSchema.Count + 1;
                            break;
                        }
                    case "msa":
                        {
                            m_iStartMSA = arrSchema.Count + 1;
                            break;
                        }
                    case "ack":
                        {
                            m_iStartACK = arrSchema.Count + 1;
                            break;
                        }
                    case "in1":
                        {
                            m_iStartIN1 = arrSchema.Count + 1;
                            break;
                        }
                    case "pv1":
                        {
                            m_iStartPV1 = arrSchema.Count + 1;
                            break;
                        }
                    case "gt1":
                        {
                            m_iStartGT1 = arrSchema.Count + 1;
                            break;
                        }
                    case "zps":
                        {
                            m_iStartZPS = arrSchema.Count + 1;
                            break;
                        }
                    case "dg1":
                        {
                            m_iStartDG1 = arrSchema.Count + 1;
                            break;
                        }
                    default:
                        {
                            m_iStartZZZ = arrSchema.Count + 1;
                            break;
                        }
                }
                arrSchema.AddRange(arrOuter);
            }

            int nCount = arrSchema.Count;
            for (int i = 0; i < nCount; i++)
            {
                if (arrSchema[i].Length == 0)
                {
                    arrSchema[i] = "";
                    continue;
                }

                arrSchema[i] = arrSchema[i].Substring(1, (arrSchema[i].IndexOf('"', 1) - 1));
            }


            // configure string for usage
            strHL7Msg = strHL7Msg.Replace(HL7.VT.ToString(), "");
            strHL7Msg = strHL7Msg.Replace(HL7.FS.ToString(), "");
            string[] arrHL7 = strHL7Msg.Split(new char[] { HL7.CR });
            int nItem = 0;
            XmlWriter writer = null;
            XmlSerializer xs = new XmlSerializer(typeof(string[]));
            try
            {

                // Create an XmlWriterSettings object with the correct options. 
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("\t");
                settings.OmitXmlDeclaration = false;
                settings.CloseOutput = true;

                int nStartIndex = 1;
                string strSegType = "";
                // Create the XmlWriter object and write some content.
                writer = XmlWriter.Create(strFileName, settings);
                //<?xml-stylesheet type = "text/xsl" href="C:\source\bones\bones\StyleSheet_HL7.xslt"?>
                writer.WriteProcessingInstruction("xml-stylesheet", "type = \"text/xsl\" href=\"C:\\source\\bones\\bones\\StyleSheet_HL7.xslt\"");
                writer.WriteProcessingInstruction("xml-stylesheet", "type = \"text/css\" href=\"C:\\source\\bones\\bones\\StyleSheet1.css\"");

                // <?link rel=stylesheet type = "text/css" href="C:\source\bones\bones\StyleSheet1.css"?>
                writer.WriteStartElement("HL7Message"); //  entire HL7 message 
                foreach (string s in arrHL7)
                {
                    if (s.Length == 0) // blank line in message and handle last to carrage returns in message string. DO NOT PROCESS
                    {
                        continue;
                    }
                    m_bNonStandardXML = false;
                    switch (s.Substring(0, 3).ToUpper())
                    {
                        case "MSH":
                            {
                                nStartIndex = m_iStartMSH;
                                strSegType = "MSH";
                                break;
                            }
                        case "PID":
                            {
                                nStartIndex = m_iStartPID;
                                strSegType = "PID";
                                break;
                            }
                        case "ZPS":
                            {
                                nStartIndex = m_iStartZPS;
                                strSegType = "ZPS";
                                break;
                            }
                        case "OBX":
                            {
                                nStartIndex = m_iStartOBX;
                                strSegType = "OBX";
                                break;
                            }
                        case "OBR":
                            {
                                nStartIndex = m_iStartOBR;
                                strSegType = "OBR";
                                break;
                            }
                        case "ORC":
                            {
                                nStartIndex = m_iStartORC;
                                strSegType = "ORC";
                                break;
                            }
                        case "NTE":
                            {
                                nStartIndex = m_iStartNTE;
                                strSegType = "NTE";
                                break;
                            }
                        case "MSA":
                            {
                                nStartIndex = m_iStartMSA;
                                strSegType = "MSA";
                                break;
                            }
                        case "ACK":
                            {
                                nStartIndex = m_iStartACK;
                                strSegType = "ACK";
                                break;
                            }
                        case "IN1":
                            {
                                nStartIndex = m_iStartIN1;
                                strSegType = "IN1";
                                break;
                            }
                        case "PV1":
                            {
                                nStartIndex = m_iStartPV1;
                                strSegType = "PV1";
                                break;
                            }
                        case "GT1":
                            {
                                nStartIndex = m_iStartGT1;
                                strSegType = "GT1";
                                break;
                            }
                        case "DG1":
                            {
                                nStartIndex = m_iStartDG1;
                                strSegType = "DG1";
                                break;
                            }
                        default:
                            {
                                nStartIndex = m_iStartZZZ;
                                strSegType = "ZZZ"; // non standard segment
                                m_bNonStandardXML = true;
                                break;
                            }
                    }

                    writer.WriteStartElement(string.Format("{0}segment", strSegType));
                    string[] arrSplit = s.Split(new char[] { '|' });
                    bool bIsMsh = false;
                    int nArrCount = -1;
                    nItem = 0;


                    foreach (string s1 in arrSplit)
                    {
                        nArrCount++;
                        if (arrSplit[0].IndexOf("MSH") > -1)
                        {
                            bIsMsh = true;
                        }
                        if (bIsMsh && (nArrCount == 1))
                        {
                            writer.WriteElementString(string.Format("{0}", arrSchema[(nItem + nStartIndex)]), "|");
                            nItem++;
                            writer.WriteElementString(string.Format("{0}", arrSchema[(nItem + nStartIndex)]), s1);
                            bIsMsh = false;
                            nItem++;
                        }
                        else
                        {
                            string strLabel = "";
                            //string strType = "";
                            int nSchemaCount = arrSchema.Count;
                            if ((nItem + nStartIndex) < nSchemaCount)
                            {
                                strLabel = arrSchema[(nItem + nStartIndex)];

                            }

                            if (strLabel.Length == 0)
                            {
                                strLabel = "NONSTANDARD";
                                m_bNonStandardXML = true;
                            }

                            if (m_bNonStandardXML)
                            {
                                strLabel = "NONSTANDARD";
                            }

                            writer.WriteElementString(string.Format("{0}", strLabel), s1);
                            nItem++;
                        }

                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.Flush();


            }
            catch (Exception writerEx)
            {
                m_strErrMsg = writerEx.Message;
                if (writer != null)
                    writer.Close();
                return;
            }

            writer.Close();

        }

        /// <summary>
        /// If the HL7 is a valid order or result or hl7 segment return a document to be displayed in 
        /// a web browser.
        /// </summary>
        /// <param name="strHL7"></param>
        /// <param name="wb">the web browser to display</param>
        /// <returns></returns>
        public HtmlDocument DisplayHL7asHTML(string strHL7, ref WebBrowser wb)
        {
            // remove the vertical tab and file seperators from the string if they exist.
            strHL7 = strHL7.Replace(HL7.VT.ToString(), "").Replace(HL7.FS.ToString(), "").Trim();

            StringBuilder sb = new StringBuilder();
            wb = new WebBrowser();
            wb.Navigate(new Uri("about:blank"));

            HtmlDocument docRetVal = wb.Document;
            docRetVal.OpenNew(false);
            docRetVal.Write("<BODY>");
            // NTE font and color styles
            // NTE font and color style
            sb.Append("<STYLE type=text/css>");
            sb.Append("#NteRow {margin:0;padding:0;border:0}");
            sb.Append("</STYLE>");
            sb.Append("<STYLE type=text/css>");
            sb.Append("#NteFont {FONT SIZE = 9; COLOR=RED}");
            sb.Append("</STYLE>");
            // end of NTE
            // SHADOW style   
            sb.Append("<style type=text/css>");
            sb.Append(".Shadow {");
            sb.Append("border: solid 1px #336699;");
            sb.Append("border-collapse: collapse;");
            sb.Append("background-color: White;");
            sb.Append("margin-bottom: 2px; ");
            sb.Append("filter: progid:DXImageTransform.Microsoft.Shadow(color=#141414,Direction=135, Strength=8);}");
            sb.Append("</style>");
            // end of shadow

            HtmlElement logo = docRetVal.CreateElement("IMG");
            logo.SetAttribute("SRC", "file://C:/mcloe/mcllogo.bmp");
            docRetVal.Body.AppendChild(logo);


            docRetVal.Write("<TABLE PADDING 3><TR><TD WIDTH = 52></TD><TD><ADDRESS><FONT SIZE = 1>Medical Center Laboratory<BR>PO Box 3099<BR>Jackson, Tennessee 38303<BR>731.541.7990 / 800.642.1730<BR>Laboratory Director: Ed Hughes</FONT></ADDRESS></TD></TR></TABLE>");
            int nType = -1; // message fragment
            //docRetVal.Write("<H3 ALIGN = CENTER>MESSAGE PARTS</H3>");
            docRetVal.Title = "MESSAGE FRAGMENT"; // wdk 20100413 default to fragment if not order or result
            if (strHL7.Contains("ORM^O01"))
            {
                nType = 0;
                docRetVal.Title = "ORDER";
                docRetVal.Write("<H3 ALIGN = CENTER>ORDER REPORT</H3>");
            }
            else
            {
                if (strHL7.Contains("ORU^R01"))
                {
                    nType = 1;
                    docRetVal.Title = "RESULT";
                    docRetVal.Write("<H3 ALIGN = CENTER>LABORATORY SERVICE REPORT (RESULTS)</H3>");
                }
            }
            sb.Append("<TABLE ALIGN = 'CENTER' BORDER BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 2 >");

            ArrayList alMsgParts = new ArrayList(strHL7.Split(new char[] { HL7.CR }, StringSplitOptions.RemoveEmptyEntries));
            int nDex = -1;
            string strClient = string.Empty;
            try
            {
                var queryMSH = from string strPrefix in alMsgParts
                               where strPrefix.Substring(0, 3) == "MSH"
                               select strPrefix;
                nDex = alMsgParts.IndexOf(queryMSH.ToArray()[0]);
                string[] strMsh = alMsgParts[nDex].ToString().Split(new char[] { '|' });
                m_strSendingApp = strMsh[2];
                // remember that the MSH counts the first '|' as a field but the split doesn't MSH[6] is strMSH[5]
                strClient = strMsh[3]; // if this is null or empty when we get to OBR[16] and OBR[16.1] is not null or empty use it.

                sb.AppendFormat("<H3>{0}</H3>", m_strSendingApp);

                // have everything we need from this msh remove it from the array 
                while (alMsgParts.Contains(queryMSH.ToArray()[0]))
                {
                    alMsgParts.Remove(queryMSH.ToArray()[0]); // remove the MSH from the array
                }
            }
            catch (IndexOutOfRangeException)
            {
                // contains throw an index out of bounds we don't care the contains should return a bool but it doesn't
            }
            ArrayList alPV1 = null;
            if (nType == 0) // Results will not contain IN1,IN2, GT1, or DG1's so strip them out if they exist.
            {


                // PV1 
                var queryPV1 = from string strPrefix in alMsgParts
                               where strPrefix.Substring(0, 3) == "PV1"
                               select strPrefix;
                alPV1 = new ArrayList(); //Arraylist used becase a size does not have to be defined here.
                if (queryPV1.Count() > 0)
                {
                    nDex = alMsgParts.IndexOf(queryPV1.ToArray()[0]);
                    alPV1 = new ArrayList(alMsgParts[nDex].ToString().Split(new char[] { '|' })); // would cause a crash if queryPV1 were null
                }
                try
                {
                    while (alMsgParts.Contains(queryPV1.ToArray()[0]))
                    {
                        alMsgParts.Remove(queryPV1.ToArray()[0]);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // we don't care
                }
                // add the table header row
                sb.Append("<TABLE ALIGN = 'CENTER' BORDER BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 2>");
                sb.Append("<TR>");
                sb.AppendFormat("<TD WIDTH = 334><B><FONT SIZE = 2>ATTENDING DOCTOR NAME</FONT></B></TD>");
                sb.AppendFormat("<TD WIDTH = 333><B><FONT SIZE = 2>ATTENDING DOCTOR NPI</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 333><B><FONT SIZE = 2>DATE OF SERVICE</B></FONT></TD>");
                sb.Append("</TR>");
                sb.Append("</TR>");
                // add the data
                sb.Append("<TR>");
                string[] strPhyscian = alPV1[7].ToString().Split('^');
                sb.AppendFormat("<TD>{0} {1} {2} {3}</TD>",
                                    strPhyscian[5], // DR if provided
                                        strPhyscian[2], // first name
                                            string.IsNullOrEmpty(strPhyscian[3]) ? strPhyscian[1] : strPhyscian[3],
                                                string.IsNullOrEmpty(strPhyscian[3]) ? "" : strPhyscian[1],
                                                string.IsNullOrEmpty(strPhyscian[3]) ? strPhyscian[1] : "");
                sb.AppendFormat("<TD>{0}</TD>", strPhyscian[0]);
                sb.AppendFormat("<TD>{0}</TD>", HL7.ConvertHL7DateToSqlDate(alPV1[44].ToString()));
                sb.Append("</TR>");
                sb.Append("</TABLE>");

                sb.Append("<HR STYLE= color:#0000ff></HR>"); // horizontal line between tables
                ArrayList alIN1 = null;
                try
                {
                    var queryIN1 = from string strPrefix in alMsgParts
                                   where strPrefix.Substring(0, 3) == "IN1"
                                   select strPrefix;
                    nDex = alMsgParts.IndexOf(queryIN1.ToArray()[0]);
                    alIN1 = new ArrayList(alMsgParts[nDex].ToString().Split(new char[] { '|' })); // would cause a crash if queryPV1 were null
                    while (alMsgParts.Contains(queryIN1.ToArray()[0]))
                    {
                        alMsgParts.Remove(queryIN1.ToArray()[0]); // remove the IN1 if it exists
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // we don't care
                }

                sb.Append("<TABLE ALIGN = 'CENTER' BORDER BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 2>");
                sb.Append("<TR>");
                sb.AppendFormat("<TD WIDTH = 50><B><FONT SIZE = 2>PLAN ID</FONT></B></TD>");
                sb.AppendFormat("<TD WIDTH = 50><B><FONT SIZE = 2>COMPANY ID</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 250><B><FONT SIZE = 2>COMPANY NAME</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>NAME OF INSURED</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 50><B><FONT SIZE = 2>RELATION TO PATIENT</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 50><B><FONT SIZE = 2>INSURED's DOB</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 250><B><FONT SIZE = 2>INSURED's ADDRESS</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 50><B><FONT SIZE = 2>POLICY NO.</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 50><B><FONT SIZE = 2>INSURED's SEX</B></FONT></TD>");
                sb.Append("</TR>");
                sb.Append("</TR>");
                // add the data
                sb.Append("<TR>");
                sb.AppendFormat("<TD>{0}</TD>", alIN1[2].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alIN1[3].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alIN1[4].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alIN1[16].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alIN1[17].ToString());
                sb.AppendFormat("<TD>{0}</TD>", HL7.ConvertHL7DateToSqlDate(alIN1[18].ToString()));
                sb.AppendFormat("<TD>{0}</TD>", alIN1[19].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alIN1[36].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alIN1[43].ToString());

                sb.Append("</TR>");
                sb.Append("</TABLE>");

                sb.Append("<HR STYLE= color:#0000ff></HR>"); // horizontal line between tables


                //    try
                //    {
                //        var queryIN2 = from string strPrefix in alMsgParts
                //                       where strPrefix.Substring(0, 3) == "IN2"
                //                       select strPrefix;
                //        nDex = alMsgParts.IndexOf(queryIN2.ToArray()[0]);

                //        while (alMsgParts.Contains(queryIN2.ToArray()[0]))
                //        {
                //            alMsgParts.Remove(queryIN2.ToArray()[0]); // remove the IN2 if it exists
                //        }
                //    }
                //    catch (IndexOutOfRangeException)
                //    {
                //        // we don't care
                //    }
                ArrayList alDG1 = new ArrayList();
                try
                {
                    var queryDG1 = from string strPrefix in alMsgParts
                                   where strPrefix.Substring(0, 3) == "DG1"
                                   select strPrefix;
                    nDex = alMsgParts.IndexOf(queryDG1.ToArray()[0]);
                    alDG1 = new ArrayList(alMsgParts[nDex].ToString().Split(new char[] { '|' })); // would cause a crash if queryPV1 were null
                    while (alMsgParts.Contains(queryDG1.ToArray()[0]))
                    {
                        alMsgParts.Remove(queryDG1.ToArray()[0]); // remove the DG1 if it exists
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // we don't care
                }
                sb.Append("<TABLE ALIGN = 'CENTER' BORDER BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 2>");
                sb.Append("<TR>");
                sb.AppendFormat("<TD WIDTH = 100><B><FONT SIZE = 2>CODING METHOD</FONT></B></TD>");
                sb.AppendFormat("<TD WIDTH = 100><B><FONT SIZE = 2>CODE</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 800><B><FONT SIZE = 2>DESCRIPTION</B></FONT></TD>");
                sb.Append("</TR>");
                sb.Append("</TR>");
                // add the data
                sb.Append("<TR>");
                sb.AppendFormat("<TD>{0}</TD>", alDG1[2].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alDG1[3].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alDG1[4].ToString());

                sb.Append("</TR>");
                sb.Append("</TABLE>");

                sb.Append("<HR STYLE= color:#0000ff></HR>"); // horizontal line between tables
                ArrayList alGT1 = new ArrayList();
                try
                {
                    var queryGT1 = from string strPrefix in alMsgParts
                                   where strPrefix.Substring(0, 3) == "GT1"
                                   select strPrefix;
                    nDex = alMsgParts.IndexOf(queryGT1.ToArray()[0]);
                    alGT1 = new ArrayList(alMsgParts[nDex].ToString().Split(new char[] { '|' })); // would cause a crash if queryPV1 were null
                    while (alMsgParts.Contains(queryGT1.ToArray()[0]))
                    {
                        alMsgParts.Remove(queryGT1.ToArray()[0]); // remove the GT1 if it exists
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // we don't care
                }
                sb.Append("<TABLE ALIGN = 'CENTER' BORDER BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 2>");
                sb.Append("<TR>");
                sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>GUARANTOR NAME</FONT></B></TD>");
                sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>GUARANTOR ADDRESS</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>GUARANTOR DOB</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>GUARANTOR SEX</B></FONT></TD>");
                sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>GUARANTOR RELATIONSHIP</B></FONT></TD>");
                sb.Append("</TR>");
                sb.Append("</TR>");
                // add the data
                sb.Append("<TR>");
                sb.AppendFormat("<TD>{0}</TD>", alGT1[3].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alGT1[5].ToString());
                sb.AppendFormat("<TD>{0}</TD>", HL7.ConvertHL7DateToSqlDate(alGT1[8].ToString()));
                sb.AppendFormat("<TD>{0}</TD>", alGT1[9].ToString());
                sb.AppendFormat("<TD>{0}</TD>", alGT1[11].ToString());

                sb.Append("</TR>");
                sb.Append("</TABLE>");

                sb.Append("<HR STYLE= color:#0000ff></HR>"); // horizontal line between tables

            }

            if (string.IsNullOrEmpty(strClient))
            {
                var queryOBR = from string strPrefix in alMsgParts
                               where strPrefix.Substring(0, 3) == "OBR"
                               select strPrefix;
                nDex = alMsgParts.IndexOf(queryOBR.ToArray()[0]);
                string[] strOBR = alMsgParts[nDex].ToString().Split(new char[] { '|' });
                if (strOBR[16].Contains('^'))
                {
                    strClient = strOBR[16].Split(new char[] { '^' })[0];
                    if (strClient[0] == 'z')
                    {
                        strClient = strClient.Remove(0, 1);
                    }
                }
            }
            //PID store our information for the HTML part in strPid
            var queryPID = from string strPrefix in alMsgParts
                           where strPrefix.Substring(0, 3) == "PID"
                           select strPrefix;
            nDex = alMsgParts.IndexOf(queryPID.ToArray()[0]);
            string[] strPid = alMsgParts[nDex].ToString().Split(new char[] { '|' });
            try
            {

                while (alMsgParts.Contains(queryPID.ToArray()[0]))
                {
                    alMsgParts.Remove(queryPID.ToArray()[0]); // remove the pid if it exists
                }
            }
            catch (IndexOutOfRangeException)
            {
                // we don't care
            }



            // ORC Should be only one but EHS sends one for each OBR if there is a difference  
            // leave the ORC that is different in to array for debug purposes remove only the queryORC[0] element  
            var queryORC = from string strPrefix in alMsgParts
                           where strPrefix.Substring(0, 3) == "ORC"
                           select strPrefix;
            nDex = alMsgParts.IndexOf(queryORC.ToArray()[0]);
            string[] strORC = alMsgParts[nDex].ToString().Split(new char[] { '|' });
            string strResultStatus = strORC[2];
            try
            {
                if (queryORC.Count() > 1)
                {
                    for (int i = 0; i < queryORC.Count(); i++)
                    {
                        // m_Err.m_Logfile.WriteLogFile("MSG HAS MULTIPLE ORC's");
                        //m_Err.m_Logfile.WriteLogFile(queryORC.ToArray()[i].ToString());
                        //m_Err.m_Email.Send("RESULTS@WEB.ORC", "david.kelly@wth.org", "MULTIPLE ORCs FOUND",
                        //        m_rsHL7Msg.m_strMsg);
                        try
                        {
                            while (alMsgParts.Contains(queryORC.ToArray()[i].ToString()))
                            {
                                alMsgParts.Remove(queryORC.ToArray()[i]);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }
                while (alMsgParts.Contains(queryORC.ToArray()[0].ToString()))
                {
                    alMsgParts.Remove(queryORC.ToArray()[0]);
                }
            }
            catch (IndexOutOfRangeException)
            {
                // we don't care
            }
            // parse the obr for the value we need before the actual results are formatted
            var queryOBRSpecTable = from string strPrefix in alMsgParts
                                    where strPrefix.Substring(0, 3) == "OBR"
                                    select strPrefix;
            nDex = alMsgParts.IndexOf(queryOBRSpecTable.ToArray()[0]);
            string[] strOBRSpecTable = alMsgParts[nDex].ToString().Split(new char[] { '|' });
            sb.Append("<TABLE ALIGN = 'CENTER' BORDER BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 2>");

            // add the table header row
            sb.Append("<TR>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>PATIENT NAME</FONT></B></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>ACCOUNT</B></FONT></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>AGE/SEX</B></FONT></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>CLIENT</B></FONT></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>ORDER NUMBER</FONT></B></TD>");
            sb.Append("</TR>");

            sb.Append("<TR>");
            // patient name
            string[] strName = new string[7];
            strPid[5].Split(new char[] { '^' }).CopyTo(strName, 0);
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.Format("{0},{1}{2}", strName[0], strName[1],
                string.IsNullOrEmpty(strName[2]) ? " " : string.Format(" {0}", strName[2])));
            // account
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(strPid[18].ToString()) ? " " : strPid[18].ToString());
            // age/sex
            string[] strAge = Time.GetAge(HL7.ConvertHL7DateToSqlDate(strPid[7]),
                HL7.ConvertHL7DateToSqlDate(strORC[9].Substring(0, 8)));
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(string.Format("{0}{1}/{2}", strAge[0], strAge[1], strPid[8])) ? " " : string.Format("{0}{1}/{2}", strAge[0], strAge[1], strPid[8]));
            // client 
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(strClient) ? "NA" : strClient);
            // ORDER NUMBER
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(strORC[2]) ? "NA" : strORC[2]);
            sb.Append("</TR>");

            // add a second table header row
            sb.Append("<TR>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>CLIENTS PATIENT ID</FONT></B></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>MT ID</FONT></B></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>HNE NUMBER</FONT></B></TD>");
            sb.AppendFormat("<TD COLSPAN = 2 WIDTH = 200><B><FONT SIZE = 2>REFERRING PHYSICIAN</FONT></B></TD>");
            sb.Append("</TR>");

            // add the data for the second table header
            sb.Append("<TR>");
            // CLients patient id
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(strPid[2].ToString()) ? "NA" : strPid[2].ToString());
            // MRI 
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(strPid[3]) ? "NA" : strPid[3]);
            // HNE
            sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(strPid[4]) ? "NA" : strPid[4]);
            // referring phy name
            string[] strPhyName = new string[10];
            try
            {
                if (!string.IsNullOrEmpty(alPV1[8].ToString()))
                {
                    alPV1[8].ToString().Split(new char[] { '^' }).CopyTo(strPhyName, 0);
                    sb.AppendFormat("<TD COLSPAN = 2><FONT SIZE = 2>{0}</FONT></TD>", string.Format("{0},{1}{2}", strPhyName[0], strPhyName[1],
                    string.IsNullOrEmpty(strPhyName[2]) ? "" : string.Format(" {0}", strPhyName[2])));
                }
                else
                {
                    //   GOMCL.R_registration reg = new GOMCL.R_registration("MCLOE", "FORDTEST", ref m_Err);
                    //    if (reg.GetRecords(string.Format("ov_order_id = '{0}'", strORC[2])) > 0)
                    //   {
                    //       sb.AppendFormat("<TD COLSPAN = 2><FONT SIZE = 2>{0}</FONT></TD>", reg.propRefdoc);
                    //   }
                    //   else
                    //   {
                    sb.Append("<TD COLSPAN = 2><FONT SIZE = 2>NOT PROVIDED</FONT></TD>");
                    //   }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // if the PV1 is not present add the blank for the phy
                sb.Append("<TD COLSPAN = 2><FONT SIZE = 2>NOT PROVIDED</FONT></TD>"); //  no PV1
            }

            sb.Append("</TR>");
            sb.Append("</TABLE>");


            sb.Append("<HR STYLE= color:#0000ff></HR>"); // horizontal line between first two tables
            // Now add the table for the Order information and  Specimen information
            sb.Append("<TABLE ALIGN = 'CENTER' BORDER BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 0 >");
            sb.Append("<TR>");
            // add the table header row
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>SPECIMEN #</FONT></B></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>COLLECTION DATE/TIME</FONT></B></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>RECEIVED IN LAB</FONT></B></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>PRIORITY</FONT></B></TD>");
            sb.AppendFormat("<TD WIDTH = 200><B><FONT SIZE = 2>STATUS</FONT></B></TD>");
            sb.Append("</TR>");

            // add the data row for the specimen   
            sb.Append("<TR>");
            try
            {

                // specimen no
                sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", string.IsNullOrEmpty(strORC[4]) ? "NA" : strORC[4]);
                // Collection Date Time
                sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", HL7.ConvertHL7DateToSqlDate(strORC[9]));
                // these three should be in the resutl table not in this table
                // Received in Lab
                sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", HL7.ConvertHL7DateToSqlDate(strOBRSpecTable[14]));
                // Priority
                try
                {
                    sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", strOBRSpecTable[26]);
                }
                catch (Exception)
                {
                    sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", "&nbsp");
                }

                // Status -- RE or OC etc
                sb.AppendFormat("<TD><FONT SIZE = 2>{0}</FONT></TD>", strORC[1]);

            }
            catch (Exception)
            {
                // don't care
                sb.Append("<TD>&nbsp</TD>");
            }
            finally
            {
                sb.Append("</TR>");
            }

            string strTests = string.Empty;
            foreach (string strPart in alMsgParts)
            {
                string[] strPartSplit = strPart.Split(new char[] { '|' });
                if (strPartSplit[0] == "OBR")
                {
                    try
                    {
                        strTests += string.Format("{0}, ", strPartSplit[4].Substring(0, strPartSplit[4].IndexOf('^')));
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // the OBR wasn't as long as we expected
                    }
                }
            }
            strTests = strTests.Substring(0, strTests.LastIndexOf(','));

            sb.Append("<TR>");
            // tests ordered
            sb.AppendFormat("<TD COLSPAN = 5><B><FONT SIZE = 2>ORDERED TEST{0}</FONT></B></TD>", strTests.Contains(',') ? "S" : "");
            sb.Append("</TR>");
            sb.Append("<TR>");
            sb.AppendFormat("<TD COLSPAN = 5><FONT SIZE = 2>{0}</FONT></TD>", strTests.Trim());
            sb.Append("</TR>");
            // comments
            sb.Append("<TR>");
            sb.Append("<TD COLSPAN = 5><B><FONT SIZE = 2>COMMENT</FONT></B></TD>");
            sb.Append("</TR>");
            sb.Append("<TR>");
            sb.AppendFormat("<TD COLSPAN = 5><FONT SIZE = 2>{0}</FONT></TD>", "&nbsp");
            sb.Append("</TR>");
            sb.Append("</TABLE>");

            sb.Append("<HR STYLE= color:#0000ff></HR>"); // horizontal line between the second and third tables

            // if it is a result create the result table
            if (nType == 1)
            {
                // create the table for the results.
                sb.Append("<TABLE RULES=GROUPS ALIGN = 'CENTER'  BORDER = 1 BORDERCOLOR = #8888FF CELLSPACING = 0 CELLPADDING = 2 >");
                sb.Append("<THEAD FRAME=BELOW >");
                sb.Append("<TR>");
                // add the table header row
                //if (m_strResultType == "PTH")
                //{
                //    sb.Append("<TD COLSPAN = 5 WIDTH = 50><B><FONT SIZE = 2>TEST</FONT></B>"); // blank so the components will be indented
                //    sb.AppendFormat("<TD WIDTH = 150>{0}</TD>", "&nbsp");
                //    sb.Append("<TD COLSPAN = 4 BORDERCOLOR = #FFFFFF WIDTH = 200><B><FONT SIZE = 2>RESULT</FONT></B></TD>");
                //    sb.Append("<TD BORDERCOLOR = #FFFFFF WIDTH = 200><B><FONT SIZE = 0></FONT></B></TD>");
                //    sb.Append("<TD BORDERCOLOR = #FFFFFF WIDTH = 200><B><FONT SIZE = 0></FONT></B></TD>");
                //    sb.Append("<TD BORDERCOLOR = #FFFFFF WIDTH = 200><B><FONT SIZE = 0></FONT></B></TD>");

                //}
                //else
                {
                    sb.Append("<TD FRAME=RHS COLSPAN = 2 WIDTH = 75><B><FONT SIZE = 2>TEST</FONT></B></TD>"); // blank so the components will be indented
                    sb.AppendFormat("<TD WIDTH = 150>{0}</TD>", "&nbsp");
                    sb.Append("<TD WIDTH = 400><B><FONT SIZE = 2>RESULT</FONT></B></TD>");
                    sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>FLAG</FONT></B></TD>");
                    sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>REFERENCE</FONT></B></TD>");
                    sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>REC IN LAB</FONT></B></TD>");
                    sb.Append("<TD WIDTH = 50 ><B><FONT SIZE = 2>PRI</FONT></B></TD>");
                    sb.Append("<TD WIDTH = 50 ><B><FONT SIZE = 2>STATUS</FONT></B></TD>");
                    sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>PEFORM SITE</FONT></B></TD>");
                }
                sb.Append("</TR>");
                sb.Append("</THEAD>");

                sb.Append("<TBODY>");
                // set up loop to process OBR/OBX/NTE
                foreach (string strPart in alMsgParts)
                {
                    string[] strResultParts = strPart.Split(new char[] { '|' });
                    if (strResultParts[0] == "OBR")
                    {
                        AddOBR(ref sb, strResultParts);
                        continue;
                    }
                    if (strResultParts[0] == "OBX")
                    {
                        if (m_strSendingApp == "PTH") // strResultType if this is a result strSendingApp will be the Meditech module that sent the result.
                        {
                            AddPthOBX(ref sb, strResultParts);
                            continue;
                        }
                        AddOBX(ref sb, strResultParts);
                        continue;
                    }
                    if (strResultParts[0] == "NTE")
                    {
                        AddNTE(ref sb, strResultParts);
                        continue;
                    }
                    //if (strResultParts[0] == "ZPS")
                    //{
                    //    AddZPS(ref sb, strResultParts);
                    //    continue;
                    //}
                    //AddUnknowFormat(ref sb, strResultParts);       
                }
                sb.Append("</TBODY>");
                sb.Append("<TFOOT HEIGHT=24 FRAME=BOX BORDER = 1>");
                var queryZPS = from string strPrefix in alMsgParts
                               where strPrefix.Substring(0, 3) == "ZPS"
                               select strPrefix;
                //      alMsgParts.Add(queryZPS.ToArray()[0].ToString());
                for (int z = 0; z <= queryZPS.ToArray().GetUpperBound(0); z++)
                {
                    AddZPS(ref sb, queryZPS.ToArray()[z].Split(new char[] { '|' }));
                }
                sb.Append("</TFOOT>");
                sb.Append("</TABLE>");


                sb.Append("<h5 ALIGN = CENTER>***End of Report****");
                sb.Append("<BR/>");

            }


            docRetVal.Write(sb.ToString());
            return docRetVal;
        }

        /// helper functions for DisplayHL7asHTML

        private void AddPthOBX(ref StringBuilder sb, string[] strResultParts)
        {
            try
            {
                sb.Append("<TR>");
                sb.Append("<TD BORDERCOLOR =#FFFFFF></TD>"); // leave this one blank and the components will be indented.
                sb.Append("<TD BORDERCOLOR =#FFFFFF></TD>"); // leave this one blank and the components will be indented.
                sb.AppendFormat("<TD COLSPAN = 4 BORDERCOLOR =#FFFFFF><FONT SIZE = 2>{0}</FONT></TD>", strResultParts[5]); //Result
                sb.Append("</TR>");
            }
            catch (IndexOutOfRangeException)
            {
                sb.Append("<TR><TD COLSPAN = 6></TD></TR>");
            }
            finally
            {
                sb.Append("</TR>");
            }
        }

        private void AddUnknowFormat(ref StringBuilder sb, string[] strResultParts)
        {
            try
            {
                sb.Append("<TR>");
                sb.AppendFormat("<TD COLSPAN = 5><FONT SIZE = 2>{0}</FONT></TD>", strResultParts.ToArray().ToString());
                sb.Append("</TR>");
            }
            catch (IndexOutOfRangeException)
            {
                sb.Append("<TR><TD COLSPAN = 5></TD></TR>");
            }
            finally
            {
                sb.Append("</TR>");
            }
        }

        private void AddZPS(ref StringBuilder sb, string[] strResultParts)
        {
            try
            {
                string[] strAddress = new string[5];
                strResultParts[4].Split(new char[] { '^' }).CopyTo(strAddress, 0);

                sb.Append("<TR>"); //BORDERCOLOR =#FFFFFF
                sb.AppendFormat("<TD>{0}</TD>", "&nbsp");
                sb.AppendFormat("<TD COLSPAN = 9><FONT SIZE = 2>{0}</FONT></TD>", strResultParts[3]);
                sb.Append("</TR>");

                sb.Append("<TR>");
                sb.AppendFormat("<TD><FONT SIZE = 2><B>{0}</B></FONT></TD>", strResultParts[2]);
                sb.AppendFormat("<TD COLSPAN = 9><FONT SIZE = 2>{0}</FONT></TD>", strAddress[0]);
                sb.Append("</TR>");

                sb.Append("<TR>");
                sb.AppendFormat("<TD>{0}</TD>", "&nbsp");
                sb.AppendFormat("<TD COLSPAN = 9><FONT SIZE = 2>{0},{1} {2}</FONT></TD>", strAddress[2], strAddress[3], strAddress[4]);
                sb.Append("</TR>");
            }
            catch (IndexOutOfRangeException)
            {
                sb.Append("<TR><TD COLSPAN = 9></TD></TR>");
            }
            finally
            {
                sb.Append("</TR>");
            }


        }

        private void AddNTE(ref StringBuilder sb, string[] strResultParts)
        {
            try
            {
                sb.Append("<DIV HEIGHT=1>");
                sb.Append("<TR>");
                sb.AppendFormat("<TD id=NteRow><FONT id = NteFont>{0}</FONT></TD>", "&nbsp");
                sb.AppendFormat("<TD id=NteRow><FONT id = NteFont>{0}</FONT></TD>", "&nbsp");
                sb.AppendFormat("<TD id=NteRow><FONT id = NteFont>{0}</FONT></TD>", "&nbsp");
                sb.AppendFormat("<TD id=NteRow COLSPAN = 4 ><FONT id = NteFont>{0}</FONT></TD>", strResultParts[3]);
                sb.Append("</TR>");
                sb.Append("</DIV>");
            }
            catch (IndexOutOfRangeException)
            {
                //sb.Append("<TR><TD COLSPAN = 5></TD></TR>");
            }
            finally
            {
                sb.Append("</TR>");
            }
        }

        private void AddOBX(ref StringBuilder sb, string[] strResultParts)
        {
            try
            {
                if (!string.IsNullOrEmpty(strResultParts[8]))
                {
                    if (strResultParts[8].Contains('*'))
                    {
                        sb.Append("<TR BORDER = 1 STYLE= COLOR:#FF0000>");
                    }
                    else
                    {
                        sb.Append("<TR BORDER = 1 STYLE= COLOR:#8E2323>");
                    }
                }
                else
                {
                    sb.Append("<TR BORDER = 1>");
                }
                /*
           *  sb.Append("<TD COLSPAN = 2 WIDTH = 75><B><FONT SIZE = 2>TEST</FONT></B></TD>"); // blank so the components will be indented
              sb.AppendFormat("<TD WIDTH = 150>{0}</TD>", "&nbsp");
              sb.Append("<TD WIDTH = 400><B><FONT SIZE = 2>RESULT</FONT></B></TD>");
              sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>FLAG</FONT></B></TD>");
              sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>REFERENCE</FONT></B></TD>");
              sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>REC IN LAB</FONT></B></TD>");
              sb.Append("<TD WIDTH = 50 ><B><FONT SIZE = 2>PRI</FONT></B></TD>");
              sb.Append("<TD WIDTH = 50 ><B><FONT SIZE = 2>STATUS</FONT></B></TD>");
              sb.Append("<TD WIDTH = 100><B><FONT SIZE = 2>PEFORM SITE</FONT></B></TD>");*/

                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF>{0}</TD>", "&nbsp"); // leave this one blank and the components will be indented.
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF><FONT SIZE = 2>{0}</FONT></TD>", strResultParts[3].Substring(0, strResultParts[3].IndexOf('^')));
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF>{0}</TD>", "&nbsp"); // leave this one blank and the components will be indented.
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF><FONT SIZE = 2>{0}</FONT></TD>", strResultParts[5]); //Result
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF><FONT SIZE = 2>{0}</FONT></TD>", strResultParts[8]); // Flag
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF><FONT SIZE = 2>{0} {1}</FONT></TD>", strResultParts[7], strResultParts[6]);// reference
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF>{0}</TD>", "&nbsp"); // leave this one blank and the components will be indented.
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF>{0}</TD>", "&nbsp"); // leave this one blank and the components will be indented.
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF>{0}</TD>", "&nbsp"); // leave this one blank and the components will be indented.
                sb.AppendFormat("<TD BORDERCOLOR =#FFFFFF><FONT SIZE = 2>{0}</FONT></TD>", strResultParts[15].Substring(0, strResultParts[15].IndexOf('^'))); //performing site
                sb.Append("</TR>");
            }
            catch (IndexOutOfRangeException)
            {
                sb.Append("<TR><TD COLSPAN = 6></TD></TR>");
            }
            finally
            {
                sb.Append("</TR>");
            }
        }


        private void AddOBR(ref StringBuilder sb, string[] strResultParts)
        {
            try
            {
                sb.Append("<TR>");
                // TEST
                sb.AppendFormat("<TD COLSPAN = 2 BORDERCOLOR = #FFFFFF >{0}</TD>", strResultParts[4].Substring(0, strResultParts[4].IndexOf('^')));
                // Blank
                sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", "&nbsp");
                // Result
                sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", strResultParts[4].Substring(strResultParts[4].IndexOf('^') + 1, (strResultParts[4].LastIndexOf('^') - strResultParts[4].IndexOf('^') - 1)));
                // Flag
                sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", "&nbsp");
                // Reference
                sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", "&nbsp");
                // Rec in Lab
                string strTime = string.Empty;
                try
                {
                    strTime = strResultParts[14].Insert(10, ":").Insert(8, " ").Insert(6, "-").Insert(4, "-");
                }
                catch (ArgumentOutOfRangeException)
                {
                }

                sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", Time.ConvertObjectToDateTimeString(strTime, true));
                // Pri
                sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", strResultParts[strResultParts.GetUpperBound(0)].Replace("^", " ").Trim());
                // Status
                try
                {
                    sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", strResultParts[25]);
                }
                catch
                {
                    sb.AppendFormat("<TD >{0}</TD>", "&nbsp");
                }

                // Perform Site
                sb.AppendFormat("<TD BORDERCOLOR = #FFFFFF>{0}</TD>", "&nbsp");

            }
            catch (IndexOutOfRangeException)
            {
                // sb.Append("<TR><TD COLSPAN = 9></TD></TR>");
            }
            finally
            {
                sb.Append("</TR>");
            }
        }

        /// <summary>
        /// All XML elements be properly nested, 
        /// All attribute values be quoted, and 
        /// All instances of less than, greater than, ampersand,
        /// quote, and aposterphy be replaced with &lt;, &gt;, &amp;, &quot; and &apos;, respectively. 
        /// Furthermore, XML files are case-sensitive, meaning that the opening and closing tags 
        /// for an XML element must match in case as well as in spelling.
        /// 
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns>true if valid, false if not</returns>
        public bool ValidateXML(string strXml)
        {
            bool bRetVal = false;
            if (string.IsNullOrEmpty(strXml))
            {
                return bRetVal;
            }
            strXml = strXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;");

            return bRetVal;
        }




    } // don't go below this line
}
