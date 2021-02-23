/*
 * 20100420 rgc/wdk moved collection of the collection date time from PV1[44] to ORC[9].
 * 
 * 07/27/2006 Rick Crone
 *  This class is to build and decode HL7 messages.
 *  It was designed to be used on the Hardin County LAB Order / Results project.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using System.Collections;
using System.Data;

namespace RFClassLibrary
{
    /// <summary>
    ///  This class is to build and decode HL7 messages.
    ///  It was designed to be used on the Hardin County LAB Order / Results project.
    /// 07/27/2006 Rick Crone
    /// 
    /// wdk 20090521 This class was hijacked for use with any hl7 message and has been converted for both 
    /// orders and results. any use for Hardin County will have to conform to the HL7 structures as documented
    /// by the standards committee.
    /// </summary>
    public class HL7 : RFCObject
    {
        private string m_strFinalsOnly = "false";
        /// <summary>
        /// Does the client only want final results.
        /// </summary>
        public string propFinalsOnly
        {
            get { return m_strFinalsOnly; }
        }


        private string m_strAMAYear;
        /// <summary>
        /// AMAYear is set when the collection date is set.
        /// </summary>
        public string propAMAYear
        {
            get { return m_strAMAYear; }
        }

        private string m_strOrdering_provider;
        /// <summary>
        /// this is the client for Meditech
        /// </summary>
        public string propOrderingProvider
        {
            get { return m_strOrdering_provider; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    m_strOrdering_provider = value;
                }
            }

        }

        /// <summary>
        /// an array of message segments
        /// </summary>
        public string[] m_arrMessageSegments;
        private string m_strPerformingSite;
        /// <summary>
        /// This is read only and will be set using DBAccess to the dbo.wperformsite table in GOMCLLIVE
        /// based on the client in the ORC[12] ordering provider section of the HL7 message.
        /// </summary>
        public string propPerformingSite
        {
            get { return m_strPerformingSite; }
        }

        /// <summary>
        ///  rgc/wdk 20090917 added for HL7 support of EHS.
        /// </summary>
        public string m_strGuarSex = string.Empty;

        private LinkedList<string> m_llValidOrderMsgParts = new LinkedList<string>();

        /// <summary>
        /// This contains the Specimen Group for getting the Ov_order_id
        /// rgc/wdk 20090708 added for results to EHS.
        /// </summary>
        public string m_strPlacersGroupNumber = string.Empty;
        /// <summary>
        /// This is the last segment that has been parsed correctly. Not needed when creating a message.
        /// If orders contain comments and the last segment is OBR or ORC then the WReq's comment should contain
        /// the values that are in this. All values except NTE should set this, that way all NTE's will get commented
        /// on the same WReq. NTE will add NTE to the end of the variable.
        /// </summary>
        private string m_strLastSegment = string.Empty;
        static int m_staticIN1SetID = 1; // rgc/wdk 20090519 static set id if none is in the IN1
        static int m_staticDG1SetID = 1;
        static int m_staticGT1SetID = 1;
        static int m_staticOBRSetID = 1;
        static int m_staticNTESetID = 1;

        /// <summary>
        /// This should be set when reading a message in the ParseMSH, as it defines the character to split the remainder of the
        /// message.
        /// </summary>
        public char m_cMsgSplitChar = '|';
        /// <summary>
        /// Contains the maximum number of attributes for each message segment by its name 
        /// eMSH is for the Message Header Segment etc.
        /// </summary>
        public enum HL7SegmentMax
        {
            ///<summary>For the MSH the first character after the MSH is the Primary delimiter for all parts of this 
            ///message. This makes counting MSH = 0, | equal 1, every thing until the next "|" = 2 (subfield serperators
            ///in order of use ^ = first field, ~ = subfield of the first subfield etc. So splitting the MSH 
            ///presents a problem that no other message segment has using the string.split function because
            ///the string.splits characters are removed from the returned string.</summary>
            eMSH = 21,
            /// <summary>Patient Identification Segment</summary>
            ePID = 31,
            /// <summary> Observation Request Segment</summary>
            eOBR = 44,
            /// <summary>Observation/Result Segment</summary>
            eOBX = 28, //18, wdk 20101102 changed to 28 as the actual results we receive from meditech have 28 segments not 18 per the manual
            /// <summary> Message Acknowledgement Segment</summary>
            eMSA = 7,
            /// <summary>Patient Visit Segment</summary>
            ePV1 = 53,
            ///<summary>Event Type Segment (not used)</summary>
            eEVN = 7,
            ///<summary>Insurance Segment</summary>
            eIN1 = 50,
            ///<summary>Diagnosis Segment</summary>
            eDG1 = 20,
            ///<summary>Guarantor Segmnet</summary>
            eGT1 = 56,
            ///<summary>Common Order Segment</summary>
            eORC = 20,
            ///<summary>Notes and Comments Segment</summary>
            eNTE = 4
        };
        //static private string m_strServer;
        //static private string m_strDB;

        /// 
        /// <!--> <SB> Start Block character (1 byte) – ASCII <VT>, i.e. 0x0B in hexadecimal.-->
        static public char VT = '\x0B';
        /// <!-- <EB> End Block character (1 byte) – ASCII <FS>, i.e. 0x1C in hexadecimal.-->
        static public char FS = '\x1C';
        /// <!--<CR> Carriage Return (1 byte) – The ASCII carriage return character, i.e.-->
        static public char CR = (char)13;

        // rgc/wdk 20090312 add DG1 parts
        // note the EHS project has these fields in this order however they DG1[2] and DG1[4]
        // are provided for backwards compatabliity. Should be in DG1[3] with each as a subfield.
        /// <summary>
        /// HL7 - DG1[2]  wdk 20090316 established by a programmer not to be named and Mike Garner for the EHS(DGM) interface for Meditech
        /// or
        /// DG1[3][subfield] [2] 
        ///  ie. ICD9, ICD10 
        ///  
        /// wdk 20090316 Forward compatability. The MCLOE system uses the ICD9_text for packing slip information. 
        /// As these orders will not be touched by MCLOE I am captureing the Diagnosis Coding Method here, in 
        /// preparation for the ICD10's       
        /// </summary>
        public string[] m_strDiagCodingMethod = new string[9];

        /// <summary>
        /// DG1[3] or DG1[3][subfield]
        /// ie. 786.2, 294.11 etc DG1[3] 
        /// </summary>
        public string[] m_strDiagCode = new string[9];
        /// <summary>
        /// DG1[4] or DG1[3][subfield 0]
        ///  ie. Cough, fever etc. DG1[4]
        ///  wdk 20090316 not used but collected.
        /// </summary>
        public string[] m_strDiagDesc = new string[9];
        // end of DG1 parts

        /// <summary>
        ///  number of NTE segments
        /// </summary>
        public int iNTECounter = 0;
        ///<!--
        /// m_strHL7Msg holds the complete message. It is cleared when CreateMSH() is called.
        /// -->
        public string m_strHL7Msg;
        //string m_strErrMsg; // now in RFCObject

        // MSH
        /// <summary>
        /// set to P/T/D Processing / Training / Debug
        /// </summary>
        public string m_strProcessingId;
        /// <summary>
        /// inbound meditech's MODULE (live/test) or HC's ORDENT
        /// at this time 11/14/2006 outbound it will be RESSEND
        /// </summary>
        public string m_strSendingApplication;
        /// <summary>
        /// inbound  meditech's MODULE or HC's 001
        /// at this time 11/16/2006 outbound it will be MCL
        /// </summary>
        /// <!--m_strSendingFacility -->
        public string m_strSendingFacility;
        /// <summary>
        /// Order is ORM, Result is ORU
        /// </summary>
        public string m_strMsgType;
        /// <summary>
        /// m_strReceivingFaciltiy
        /// </summary>
        public string m_strReceivingFaciltiy;
        /// <summary>
        /// m_strReceivingApplication
        /// </summary>
        public string m_strReceivingApplication;
        /// <summary>
        /// This is the HL7 Message Control ID -- MSH[10] (zero base)
        /// the C# split removes the first '|' from our MSH so the first field should be the 
        /// field seperator but is in actualallity the Encoding Characters.
        /// </summary>
        public string m_strOVControlId;

        /// <summary>
        /// This is the HL7 Messgae Expected sequence Number -- MSH[13] 
        /// the C# split removes the first '|' from our MSH so the first field should be the 
        /// field seperator but is in actualallity the Encoding Characters.
        /// 
        /// rgc/wdk 20090610 added for message validation at time of parsing.
        /// </summary>
        public string m_strSequenceNr;
        /// <summary>
        /// rgc/wdk 20090610 added for validation of messages.
        /// </summary>
        public string m_strVersionNr;

        // PID
        /// <summary>
        ///  from pid-2
        /// </summary>
        public string m_strIndustrialAcct;
        /// <summary>
        /// For RESULTS this is PID[3] for EHS move to PID[4]
        /// </summary>
        public string m_strMTMRI;
        /// <summary>
        /// rgc/wdk 20090604 added for RESULTS this will be READ from PID[3] for returning an EHS order move to
        /// PID[4]
        /// For RESULTS this is the PID[18] 
        /// for ORDERS from EHS it is the patient id which is the same for PID[2][3][4][18] Not used in our order parsing.
        /// per Mike 20090604 the pid[2] will soon become the HNE number from the hospital
        /// </summary>
        public string m_strMTAccount;
        /// <summary>
        /// per Mike on 20090604 this should be PID[2] for EHS once they get under way in live.
        /// </summary>
        public string m_strHNENumber;

        /// <summary>
        /// Other Vender Patient ID from PID[3]
        /// </summary>
        public string m_strOVPatId;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatLName;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatSuffix;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatFName;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatMidInit;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatDOB; //ccyymmdd
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatSex;
        /// <summary>
        /// PID[18] for ORDERS only 
        /// </summary>
        public string m_strOVPatAccount;


        /// <summary>
        /// 
        /// </summary>
        public string m_strPatSSN;
        /// <summary>
        /// 
        /// </summary>
        public string m_strMaritalStatus;

        //OBR
        /// <summary>
        /// 
        /// </summary>
        public string m_strCliMnem;
        /// <summary>
        /// Other Vender Order ID from OCR[2]
        /// </summary>
        public string m_strOVOrderId;
        /// <summary>
        /// 
        /// </summary>
        public string m_strOurOrderId;

        /// <summary>
        /// wdk 20101001 added so we have a list of tests that are on an order message
        /// </summary>
        public ArrayList m_alTestsOrdered;
        /// <summary>
        /// also used in OBX
        /// rgc/wdk 20090319 changed to an array to handle multiple order (OBRs) on one requisition (ORC)
        /// rgc/wdk 20090407 use in CreateOBR() for results.
        /// </summary>
        public string m_strTestMnem;
        /// <summary>
        /// rgc/wdk 20090408
        /// Linked list used for storing Orders information for use in the application. Fields are completed
        /// as below
        /// 1 OBR setid -- Not used at this time 
        /// 2 test mnen -- Their test mnem we will need to check before use
        /// 3 description -- Their description. If the test of 2 above is valid and these don't agree ????
        /// 4 microsource -- Use this to call the micro source map translator, if this is not blank.
        /// 5 OV specimen id -- 
        ///     a. If they provide a specimen number in OBR[18] it will be used.
        ///     b. if OBR[18] is not provided by the client then we make it the OrderID with the OBR's set id (OBR[1])
        /// 6 ABN -- if any of the NTE's for an OBR contain "ABN form signed and on file." this will be "T" otherwise "F"
        /// </summary>
        public LinkedList<string> m_llOrders = new LinkedList<string>();

        /// <summary>
        /// wdk 20091026 added to overcome the ABN limitations of the linked list and AddOrdersToDataSet()
        /// </summary>
        public DataSet m_dsOBR = null;

        /// <summary>
        /// Array of test menmonics visible to using applications. Escpecially used in GOMCL's ComplianceChecker()
        /// </summary>
        public Dictionary<string, string> m_dicOrderedTestMnems = new Dictionary<string, string>();
        /// <summary>
        /// Not used but stored anyway
        /// </summary>
        public string m_strTestDesc; // also used in OBX
        /// <summary>
        /// also in 11/14/2006  OBX
        /// rgc/wdk 20090319 changed to an array to handle multiple order (OBRs) on one requisition (ORC)
        /// </summary>
        public string m_strMicroSource;
        /// <summary>
        /// 
        /// </summary>
        public string m_strOrderPriority;
        /// <summary>
        /// rgc/wdk 20091110 format is ccyymmddhhmm
        /// </summary>
        public string m_strCollDateTime;
        /// <summary>
        /// 
        /// </summary>
        public string m_strResultDateTime; //ccyymmddhhmm
        /// <summary>
        /// 
        /// </summary>
        public string m_strMeditechReceivedDateTime; //ccyymmddhhmm

        /// <summary>
        /// rgc/wdk 20090407 this is the value for the RESULT from Meditech Used in CreateOBR()
        /// All other references have been changed to use Dictionary &lt; int, string> m_dicOVSpecimenId
        /// rgc/wdk 20090319 changed to an array to handle multiple order (OBRs) on one requisition (ORC)
        /// rgc/wdk 20090402 modified to use set id and the OVOrderID ie.
        /// Variables set from OBR[1] and ORC[2]
        /// m_strOVSpecimenId[nSetId - 1] = string.Format("{0}-{1}", m_strOVOrderId , nSetId.ToString());
        /// </summary>
        public string m_strOVSpecimenId;

        /// <summary>
        /// rgc/wdk 20090407 this is used for Orders to allow multiple ORB segments to be sent from the 
        /// HL7 Ordering system.
        /// </summary>
        public Dictionary<int, string> m_dicOVSpecimenId = new Dictionary<int, string>();

        /// <summary>
        /// 
        /// </summary>
        public string m_strResultsStatus;

        //ORC
        /// <summary>
        /// 
        /// </summary>
        public string m_strPhyUpin;

        //OBX
        //public string m_strTestMnem; // also used in OBR
        //public string m_strTestDesc; // also used in OBR

        /// <summary>
        /// wdk 20101027
        /// this is the value from OBX[3]. As this is not an array it has limited useage 
        /// and is identified with the last OBR that is parsed. NEED LOTS OF WORK>
        /// </summary>
        public string m_strResultedTestMnem;
        /// <summary>
        /// 
        /// </summary>
        public string m_strObservationValue;
        /// <summary>
        /// 
        /// </summary>
        public string m_strReferenceRange;
        /// <summary>
        /// 
        /// </summary>
        public string m_strAbnormalFlags;
        /// <summary>
        /// 
        /// </summary>
        public string m_strObservationResultsStatus;

        //NTE
        /// <summary>
        /// 
        /// </summary>
        public string m_strComment;
        /// <summary>
        /// 
        /// </summary>
        public string m_strSourceOfComment;

        // rgc/wdk 20090312 added variable for IN1 from EHS
        /// <summary>
        /// set_id or 1 = A, 2 = B, 3 = C  Add 64 and cast to char for our system
        /// </summary>
        public string[] m_strInsAbc = new string[3];
        /// <summary>
        ///  InsPlanId from IN1[2]
        ///  Use GOMCL's R_dict_winsAS400 to translate the 5 digit number to our ins_code.
        /// </summary>
        public string[] m_strInsPlanId = new string[3];

        /// <summary>
        /// Set in ConvertPlanIdToBillTo() from the insurance detailed in IN1[2], this is how the account 
        /// should be billed. NOTE: not a string array 
        /// </summary>
        public string m_strBillTo;
        /// <summary>
        /// Set in ConvertPlanIdToBillTo() from the insurance detailed in IN1[2] Should be only for the primary insurance
        /// </summary>
        public string[] m_strInsFinCode = new string[3];

        /// <summary>
        ///  IN1[3]
        /// </summary>
        public string[] m_strInsCompanyId = new string[3];
        /// <summary>
        ///  hl7[4]
        /// </summary>
        public string[] m_strInsCompanyName = new string[3];
        /// <summary>
        ///  hl7[5]
        /// </summary>
        public string[] m_strInsCompanyAddr = new string[3];
        /// <summary>
        ///  hl7[8]
        /// </summary>
        public string[] m_strInsGroupNo = new string[3];
        /// <summary>
        /// hl7[9]
        /// </summary>
        public string[] m_strInsGroupName = new string[3];
        /// <summary>
        /// hl7[10]
        /// </summary>
        public string[] m_strInsInsuredGroupEmpId = new string[3];
        /// <summary>
        /// hl7[11]
        /// </summary>
        public string[] m_strInsInsuredGroupEmpName = new string[3];

        //public string m_strInsPlanType; // hl7[15] don't think we need at this time 20090312
        /// <summary>
        ///  hl7[16]
        /// </summary>
        public string[] m_strInsInsuredName = new string[3];
        /// <summary>
        ///  hl7[17] will need to convert from "SELF" etc to "01" etc
        /// </summary>
        public string[] m_strInsInsuredRelation = new string[3];
        /// <summary>
        ///  hl7[18]
        /// </summary>
        public string[] m_strInsInsuredDateOfBirth = new string[3];
        /// <summary>
        ///  hl7[19]
        /// </summary>
        public string[] m_strInsInsuredAddress = new string[3];
        /// <summary>
        ///  hl7[35] EHS has in [36] we feel incorrectly
        /// </summary>
        public string[] m_strInsPolicyNumber = new string[3];
        /// <summary>
        /// hl7[43] ehs has in [44] again we feel incorrectly
        /// </summary>
        public string[] m_strInsSex = new string[3];

        // rgc/wdk 20090312 GT1 fields added
        /// <summary>
        /// GT1[setid 1][3] in WReq and WPat GuarName is the primary insurance (ins_a_b_c 'A' insurance)
        /// while subsequent GT1[setid's 1,2 and 3][3] are the WIns Holders name.
        /// </summary>
        public string m_strGuarName;
        /// <summary>
        /// GT1[Setid 1][5] WReq and WPat Guar_addr1 is the primary insurance (ins_a_b_c 'A' insurance)
        /// this makes the WReq and WPat's address the zero element of the m_strGuarAddress[].
        /// while subsequent GT1[setid's 1,2, and 3][5] are the WIns holder's address (NON PARSED) 
        /// </summary>
        public string m_strGuarAddress;
        /// <summary>
        /// GT1[setid 1][3] in WReq and WPat GuarName is the primary insurance (ins_a_b_c 'A' insurance)
        /// while subsequent GT1[setid's 1,2 and 3][3] are the WIns Holders name.
        /// </summary>
        public string[] m_strHolderName = new string[3];
        /// <summary>
        /// GT1[Setid 1][5] WReq and WPat Guar_addr1 is the primary insurance (ins_a_b_c 'A' insurance)
        /// this makes the WReq and WPat's address the zero element of the m_strGuarAddress[].
        /// while subsequent GT1[setid's 1,2, and 3][5] are the WIns holder's address (NON PARSED) 
        /// </summary>
        public string[] m_strHolderAddress = new string[3];

        // added for PV1
        /// <summary>
        /// hl7[20]
        /// </summary>
        public string m_strFinClass;


        // wdk 20090316 added for paring PID for EHS

        /// <summary>
        /// HL7 - PID[11 subfield [0]]
        /// </summary>
        public string m_strPatAddr1;
        /// <summary>
        /// HL7 - PID[11 subfield [1]]
        /// </summary>
        public string m_strPatAddr2;
        /// <summary>
        /// HL7 - PID[11 subfield [2]]
        /// </summary>
        public string m_strPatCity;
        /// <summary>
        /// HL7 - PID[11 subfield [3]]
        /// </summary>
        public string m_strPatSt;
        /// <summary>
        /// HL7 - PID[11 subfield [4]]
        /// </summary>
        public string m_strPatZip;
        /// <summary>
        /// HL7 - PID[13]
        /// </summary>
        public string m_strPatPhone;

        /// <summary>
        /// HL7 - DG1[1] 
        /// Used to set the diagnosis into the WPAT icd9_X where m_strDiagIndicator is X which is the DG1 set_id
        /// This can be used as the number of Diagnosis provided, as it is set each time. while the values are 
        /// set into arrays 
        /// </summary>
        public int m_nDiagIndicator;

        /// <summary>
        /// HL7 - DG1[15] 
        /// wdk 20090316 Forward compatability. Not used at this time but useful if provided as we are reworking billing to gather this info.
        /// </summary>
        public string m_strDiagPriority;

        // end of 20090316

        /// <summary>
        ///  rgc/wdk 20090318 Added for tracking EHS billto type of insurance
        ///  When parsing the MSH if the sending facility is EHS then set this to INSURANCE.
        /// </summary>
        public string m_strAccBillTo;

        // rgc/wdk 20090409 added to track message parts
        /// <summary>
        /// Contains the string passed into the ParseMSG() function
        /// </summary>
        public string m_strMSH;
        /// <summary>
        /// Contains the string passed into the ParsePID() function
        /// </summary>
        public string m_strPID;
        /// <summary>
        /// Contains the string passed into the ParseORC() function
        /// </summary>
        public string m_strORC;
        /// <summary>
        /// Contains the string passed into the ParseOBR() function
        /// </summary>
        public string m_strOBR;
        /// <summary>
        /// Contains the string passed into the ParseDG1() function
        /// </summary>
        public string m_strDG1;
        /// <summary>
        /// Contains the string passed into the ParseIN1() function
        /// </summary>
        public string m_strIN1;
        /// <summary>
        /// Contains the string passed into the ParseGT1() function
        /// </summary>
        public string m_strGT1;
        /// <summary>
        /// Contains the string passed into the ParsePV1() function
        /// </summary>
        public string m_strPV1;
        /// <summary>
        /// Contains the string passed into the ParseOBX() function
        /// </summary>
        public string m_strOBX;
        /// <summary>
        /// Contains the string passed into the ParseNTE() function
        /// </summary>
        public string m_strNTE;
        /// <summary>
        /// Constructor that sets the HL7's m_ERR to the m_Err file passed by the construction class.
        /// </summary>
        /// <param name="m_Err"></param>
        public HL7(ref ERR m_Err)
        {
            m_ERR = m_Err;
        }

        /// <summary>
        /// Constructor that sets the HL7's m_ERR to the m_Err file passed by the construction class.
        /// Also allows the user to set the server and database for the orders that are passed in.
        /// </summary>
        /// <param name="m_Err"></param>
        /// <param name="strDatabase"></param>
        /// <param name="strServer"></param>
        public HL7(string strServer, string strDatabase, ref ERR m_Err)
        {
            m_ERR = m_Err;
            // CreateObrDataSet(strServer, strDatabase);
        }
        private void CreateObrDataSet(string strServer, string strDatabase)
        {
            m_dsOBR = new DataSet("WORDERS");
            m_dsOBR.Tables.Add("DEFAULTS");
            m_dsOBR.Tables["DEFAULTS"].Columns.Add("SERVER");
            m_dsOBR.Tables["DEFAULTS"].Columns.Add("DATABASE");
            m_dsOBR.Tables["DEFAULTS"].Rows.Add(new string[] { strServer, strDatabase });

            m_dsOBR.Tables.Add("TESTS");
            m_dsOBR.Tables["TESTS"].Columns.Add("TEST_MNEM");
            m_dsOBR.Tables["TESTS"].Columns.Add("CDM");
            m_dsOBR.Tables["TESTS"].Columns.Add("TEST_NAME");
            m_dsOBR.Tables["TESTS"].Columns.Add("MICRO_SOURCE");
            m_dsOBR.Tables["TESTS"].Columns.Add("OV_SPECIMEN_ID");
            m_dsOBR.Tables["TESTS"].Columns.Add("ABN"); // wdk 20091026 using the NTE we can find this and set to Y.
            m_dsOBR.Tables["TESTS"].Columns["ABN"].DefaultValue = "F";
            // create the primary key for finding records to not double add them.
            DataColumn[] pKeyWOrders = new DataColumn[] { m_dsOBR.Tables["TESTS"].Columns["TEST_MNEM"] };
            m_dsOBR.Tables["TESTS"].PrimaryKey = pKeyWOrders;

            // components
            m_dsOBR.Tables.Add("COMP");
            m_dsOBR.Tables["COMP"].Columns.Add("CDM");
            m_dsOBR.Tables["COMP"].Columns.Add("CPT4");
            m_dsOBR.Tables["COMP"].Columns.Add("LINK");
            m_dsOBR.Tables["COMP"].PrimaryKey = new DataColumn[] {
                        m_dsOBR.Tables["COMP"].Columns["cdm"],
                            m_dsOBR.Tables["COMP"].Columns["LINK"]};

            DataRelation drTestsComp =
                new DataRelation("TestsComp", m_dsOBR.Tables["TESTS"].Columns["CDM"],
                        m_dsOBR.Tables["COMP"].Columns["CDM"]);
            drTestsComp.Nested = true;
            m_dsOBR.Relations.Add(drTestsComp);

            m_dsOBR.Tables.Add("PREVIOUS_ORDERS");
            m_dsOBR.Tables["PREVIOUS_ORDERS"].Columns.Add("TEST_MNEM");
            m_dsOBR.Tables["PREVIOUS_ORDERS"].Columns.Add("STATUS");
            m_dsOBR.Tables["PREVIOUS_ORDERS"].Columns.Add("ABN");

            m_dsOBR.Tables["TESTS"].RowChanged += new DataRowChangeEventHandler(HL7_RowChanged);
            //    m_dsOBR.Tables["COMP"].RowChanged += new DataRowChangeEventHandler(COMP_HL7_RowChanged);
        }

        void COMP_HL7_RowChanged()
        {
            if (m_dsOBR == null)
            {
                return;
            }
            // build the connection
            System.Data.SqlClient.SqlConnectionStringBuilder builder =
                  new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder["Data Source"] = m_dsOBR.Tables["DEFAULTS"].Rows[0]["SERVER"];
            builder["integrated Security"] = true;
            builder["Initial Catalog"] = m_dsOBR.Tables["DEFAULTS"].Rows[0]["DATABASE"];
            // create the connection
            System.Data.SqlClient.SqlConnection sqlConnect =
                new System.Data.SqlClient.SqlConnection(builder.ConnectionString);
            sqlConnect.Open();
            string queryString =
            string.Format("select d.test_mnem, d.status, d.abn from wreq inner join worders d on d.wreq_rowguid = wreq.rowguid where wreq.hl7_ov_patid = '{0}' and convert(varchar(10), wreq.coll_date,101) = '{1}' and bill_to = '{2}' and wreq.cli_mnem = '{3}'",
                            m_strOVPatId, HL7.ConvertHL7DateToSqlDate(m_strCollDateTime.Substring(0, 8)), m_strBillTo, m_strCliMnem);

            System.Data.SqlClient.SqlCommand sqlCommandWTests =
                new System.Data.SqlClient.SqlCommand(queryString, sqlConnect);

            System.Data.SqlClient.SqlDataReader sdrTests =
                sqlCommandWTests.ExecuteReader();//CommandBehavior.SingleResult);

            while (sdrTests.Read())
            {
                m_dsOBR.Tables["PREVIOUS_ORDERS"].Rows.Add(new string[] {
                    sdrTests.GetString(0), sdrTests.GetString(1), sdrTests[2].ToString() == "1" ? "T":"F" });

            }
            sdrTests.Close();
            sqlConnect.Close();

        }

        void HL7_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add)
            {
                // build the connection
                System.Data.SqlClient.SqlConnectionStringBuilder builder =
                      new System.Data.SqlClient.SqlConnectionStringBuilder();
                builder["Data Source"] = m_dsOBR.Tables["DEFAULTS"].Rows[0]["SERVER"];
                builder["integrated Security"] = true;
                builder["Initial Catalog"] = m_dsOBR.Tables["DEFAULTS"].Rows[0]["DATABASE"];
                // create the connection
                System.Data.SqlClient.SqlConnection sqlConnect =
                    new System.Data.SqlClient.SqlConnection(builder.ConnectionString);
                sqlConnect.Open();
                string queryString =
                string.Format("select w.test_mnem, w.cdm, w.test_name, c.cpt4, convert(varchar(3),c.link) from wtest w inner join wcpt4 c on w.cdm = c.cdm where test_mnem = '{0}'",
                                e.Row["TEST_MNEM"]);


                System.Data.SqlClient.SqlCommand sqlCommandWTests =
                    new System.Data.SqlClient.SqlCommand(queryString, sqlConnect);

                System.Data.SqlClient.SqlDataReader sdrTests =
                    sqlCommandWTests.ExecuteReader();//CommandBehavior.SingleResult);

                //string[] strVal = new string[sdrTests.FieldCount];
                while (sdrTests.Read())
                {
                    e.Row["CDM"] = sdrTests.GetString(1);
                    m_dsOBR.Tables["COMP"].Rows.Add(new string[] {
                        sdrTests.GetString(1), sdrTests.GetString(3), sdrTests.GetString(4) });

                }
                sdrTests.Close();
                sqlConnect.Close();
            }
        }



        /// <summary>
        /// contructor - set some default values
        /// 07/27/2006 Rick Crone
        /// </summary>
        [Obsolete("Use the constructor with the reference to the ERR class instead.")]
        public HL7()
        {
            //throw new System.NotImplementedException();
            // set some default values
            //MSH
            m_strProcessingId = string.Format("P");// production

            ////PID - defaults from HMS example
            //m_strOVPatId = string.Format("Q000249119"); // Other vendors patient ID
            //m_strPatLName = string.Format("SAMPLE");
            //m_strPatFName = string.Format("PATIENT"); ;
            //m_strPatMidInit = string.Format("");
            //m_strPatDOB = string.Format("19740310"); //ccyymmdd
            //m_strPatSex = "F";
            //m_strOVPatAccount = "Q00006158390";
            //m_strPatSSN = "529430025";

            ////OBR - defaults from HMS example
            //m_strOVOrderId = "L745360";
            //m_strOurOrderId = "45113190";
            //m_strTestMnem = "1380";
            //m_strTestDesc = "CHLAM/GC";
            //m_strOrderPriority = "R";
            //m_strCollDateTime = "2000111171144"; //ccyymmddhhmm
            //m_strResultDateTime = "200111171144"; //ccyymmddhhmm
            //m_strMeditechReceivedDateTime = "200111181924"; //ccyymmddhhmm
            //m_strOVSpecimenId = "1117:CBM:S00003S";
            //m_ResultsStatus = "F";

            ////OBX - defaults from HMS example 
            ////m_strTestMnem; // also used in OBR
            ////m_strTestDesc; // also used in OBR
            //m_strObservationValue = "NEGATIVE";
            //m_strReferenceRange = "NEGATIVE";
            //m_strAbnormalFlags = "N";
            //m_strObservationResultsStatus = "F";

        }
        /// <summary>
        /// 10/27/2006 David Kelly removed all code and call CreateHMS_MSH_ACK as it works fine for Meditech
        /// </summary>
        /// <returns></returns>
        public string CreateMED_MSH_ACK(string strMSHReceived)
        {
            return CreateHMS_MSH_ACK(strMSHReceived);
        }

        /// <summary>
        /// 11/9/2006 David Kelly
        /// TODO - Add Error handling inside this function 
        /// Returns the ack message for HMS HL7
        /// this function parses the whole message header MSH 
        /// The first part of this function breaks apart the strMSHReceived to get the data to identify
        /// the ACK on the sending server.
        /// The second part puts together the ACK and returns the string to the app to send to the sender.
        /// </summary>
        /// <returns></returns>
        public string CreateHMS_MSH_ACK(string strMSHReceived)
        {
            // Unlike the other message segments I've called the MSA part of the acknowledgement
            // from the MSH header for the Acknowledgement, instead of from the application.
            // but it could be done in the app by just calling 
            /*
            SEQ 	LEN 	DT 	OPT 	RP/# 	TBL# 	ITEM # 	ELEMENT NAME	        Values Received
            1 	    1 	    ST 	R 		                00001 	Field Separator	            |
            2 	    4 	    ST 	R 			            00002 	Encoding Characters	        ^~\&
            3 	    180 	HD 	O 			            00003 	Sending Application	        ORDENT
            4 	    180 	HD 	O 	            		00004 	Sending Facility	        001
            5 	    180 	HD 	O 			            00005 	Receiving Application	    MCL
            6 	    180 	HD 	O 			            00006 	Receiving Facility	        001
            7 	    26 	    TS 	O 			            00007 	Date/Time Of Message	    20061025104813
            8 	    40 	    ST 	O 			            00008 	Security	
            9 	    7 	    CM 	R 			            00009 	Message Type	            ORM^O01
            10 	    20 	    ST 	R 			            00010 	Message Control ID	        17
            11 	    3 	    PT 	R 			            00011 	Processing ID	            P
            12 	    8 	    ID 	R 		        0104 	00012 	Version ID	                2.3
            13 	    15 	    NM 	O 			            00013 	Sequence Number	            17
            14 	    180 	ST 	O 			            00014 	Continuation Pointer	
            15 	    2 	    ID 	O 		        0155 	00015 	Accept Acknowledgment Type	
            16 	    2 	    ID 	O 		        0155 	00016 	App Acknowledgment Type	
            17 	    2 	    ID 	O 			            00017 	Country Code	            US
            18 	    6 	    ID 	O 	Y/3 	    0211 	00692 	Character Set	
            19 	    60 	    CE 	O 			            00693 	Principal Language Of Message	

            */

            //  int iStart = 0;
            //  int iEnd = 0;
            //  int iCount = 0;
            // rgc/wdk 20090610 using ParseMSG() to fill out all the errors with the message before 
            // sending the ACK msg
            /*string strMsgSendingApp = "";
            string strMsgSendingFacility = "";
            string strMsgReceivingApp = "";
            string strMsgReceivingFacility = "";
            string strMsgDatetime = "";
            string strMsgType = "";
            string strMsgControlID = "";
            string strMsgProcessingID = "";
            string strMsgVersionID = "";
            string strMsgSequenceNr = "";
            string[] arrString = strMSHReceived.Split(new char[] {HL7.CR});
            arrString = arrString[0].Split(new char[] { '|' }); // index now accounts for the "|" character after "MSH" as a complete field
            int nStrings = arrString.GetUpperBound(0);
            
            strMsgSendingApp = arrString[2];
            strMsgSendingFacility = arrString[3];
            strMsgReceivingApp = arrString[4];
            strMsgReceivingFacility = arrString[5];
            strMsgDatetime = arrString[6];
            strMsgType = arrString[8];
            strMsgControlID = arrString[9];
            m_strOVControlId = strMsgControlID; // eventually parse the string to load all the member level variables.

             */
            string strAckCode = "AA"; //AA = Application Accept set to other code below if errors occur.
            string strAckErrorMsg = string.Empty; // if strAckCode not AA set error message when changing the strAckCode
            ParseMsg(strMSHReceived);  // rgc/wdk 20090618 per meeting with mike. all messages get AA valid or not.
                                       //if (!ParseMsg(strMSHReceived))
                                       //{
                                       //strAckCode = "AE";
                                       //strAckErrorMsg = m_ERR.DumpDataSetErrorsByColumn("ERR_MSG", "\t");

            //}

            //strMsgProcessingID = arrString[10];


            //if (arrString.GetUpperBound(0) > 10)
            //{
            //    strMsgVersionID = arrString[11];
            //}

            //if (arrString.GetUpperBound(0) > 11)
            //{
            //    strMsgSequenceNr = arrString[12];
            //    if (strMsgSequenceNr.Length == 0) // optional field not really an error
            //    {
            //        strMsgSequenceNr = strMsgControlID;
            //    }
            //}                   

            string strMSH;
            //    string strMSA = CreateMSA(strMsgControlID, strMsgProcessingID, "D");// "D" equals Delay type D is for Message Received, stored for later processing, F is for Acknowledgement after processing
            // the fields are as follows 0 = MSH, 1 = "|" (BANG or Field Separator to use in the message).
            //                           0 1| 2 | 3 |4  | 5 | 6 | 7 |8| 9|10 | 11| 12| 13|    0 | 1 | 2  | 3  | 4  |
            strMSH = string.Format(@"{0}MSH|^~\&|MCL|001|{1}|{2}|{3}||ACK|{4}|{5}|{6}|{7}|{8}MSA|{9}|{10}|{11}|{12}|{13}{14}{15}{16}",
                              VT,  // {0}
                              m_strSendingApplication, // {1}  who sent it to us should receive the ACK
                                m_strSendingFacility, //{2} who sent it to us should receive the ACK
                                    Time.HL7TimeStamp(), // {3} date/time of our MSH creation
                                        m_strOVControlId, // {4} this is in field 10 of the message we are receiving
                                            m_strProcessingId, // {5} doesn't seem to work unless in both the MSH and MSA
                                                m_strVersionNr,//{6}
                                                    m_strSequenceNr,//{7} // returning the same sequence number here that was sent to us????
                                                        CR, //{8}
                                                            strAckCode, //{9}
                                                                m_strOVControlId, // {10}
                                                                    strAckErrorMsg, // {11} should only display text if the strAckCode is not "AA" so an error occured.
                                                                        m_strSequenceNr, // {12}
                                                                            "D", // {13} delayed acknowledgement type D = Message received stored for later processing F = Acknowledge after processing
                                                                                CR, // {14} end the MSA segment
                                                                                    FS, // {15} File seperator for MSH
                                                                                        CR); // {16} end the MSH segment

            return (strMSH);
        }

        /// <summary>
        /// 11/9/2006 David Kelly
        /// TODO - Add Error handling inside this function 
        /// Returns the ack message for HMS HL7
        /// this function parses the whole message header MSH 
        /// The first part of this function breaks apart the strMSHReceived to get the data to identify
        /// the ACK on the sending server.
        /// The second part puts together the ACK and returns the string to the app to send to the sender.
        /// </summary>
        /// <returns></returns>
        public string CreateHMS_MSH_NAK(string strMSHReceived)
        {
            // Unlike the other message segments I've called the MSA part of the acknowledgement
            // from the MSH header for the Acknowledgement, instead of from the application.
            // but it could be done in the app by just calling 
            /*
            SEQ 	LEN 	DT 	OPT 	RP/# 	TBL# 	ITEM # 	ELEMENT NAME	        Values Received
            1 	    1 	    ST 	R 		                00001 	Field Separator	            |
            2 	    4 	    ST 	R 			            00002 	Encoding Characters	        ^~\&
            3 	    180 	HD 	O 			            00003 	Sending Application	        ORDENT
            4 	    180 	HD 	O 	            		00004 	Sending Facility	        001
            5 	    180 	HD 	O 			            00005 	Receiving Application	    MCL
            6 	    180 	HD 	O 			            00006 	Receiving Facility	        001
            7 	    26 	    TS 	O 			            00007 	Date/Time Of Message	    20061025104813
            8 	    40 	    ST 	O 			            00008 	Security	
            9 	    7 	    CM 	R 			            00009 	Message Type	            ORM^O01
            10 	    20 	    ST 	R 			            00010 	Message Control ID	        17
            11 	    3 	    PT 	R 			            00011 	Processing ID	            P
            12 	    8 	    ID 	R 		        0104 	00012 	Version ID	                2.3
            13 	    15 	    NM 	O 			            00013 	Sequence Number	            17
            14 	    180 	ST 	O 			            00014 	Continuation Pointer	
            15 	    2 	    ID 	O 		        0155 	00015 	Accept Acknowledgment Type	
            16 	    2 	    ID 	O 		        0155 	00016 	App Acknowledgment Type	
            17 	    2 	    ID 	O 			            00017 	Country Code	            US
            18 	    6 	    ID 	O 	Y/3 	    0211 	00692 	Character Set	
            19 	    60 	    CE 	O 			            00693 	Principal Language Of Message	

            */

            //  int iStart = 0;
            //  int iEnd = 0;
            //  int iCount = 0;
            // rgc/wdk 20090610 using ParseMSG() to fill out all the errors with the message before 
            // sending the ACK msg
            /*string strMsgSendingApp = "";
            string strMsgSendingFacility = "";
            string strMsgReceivingApp = "";
            string strMsgReceivingFacility = "";
            string strMsgDatetime = "";
            string strMsgType = "";
            string strMsgControlID = "";
            string strMsgProcessingID = "";
            string strMsgVersionID = "";
            string strMsgSequenceNr = "";
            string[] arrString = strMSHReceived.Split(new char[] {HL7.CR});
            arrString = arrString[0].Split(new char[] { '|' }); // index now accounts for the "|" character after "MSH" as a complete field
            int nStrings = arrString.GetUpperBound(0);
            
            strMsgSendingApp = arrString[2];
            strMsgSendingFacility = arrString[3];
            strMsgReceivingApp = arrString[4];
            strMsgReceivingFacility = arrString[5];
            strMsgDatetime = arrString[6];
            strMsgType = arrString[8];
            strMsgControlID = arrString[9];
            m_strOVControlId = strMsgControlID; // eventually parse the string to load all the member level variables.

             */
            string strAckCode = "AE"; //= Application Error set to other code below if errors occur.
            string strAckErrorMsg = string.Empty; // if strAckCode not AA set error message when changing the strAckCode
            ParseMsg(strMSHReceived);  // rgc/wdk 20090618 per meeting with mike. all messages get AA valid or not.
            //if (!ParseMsg(strMSHReceived))
            //{
            //strAckCode = "AE";
            //strAckErrorMsg = m_ERR.DumpDataSetErrorsByColumn("ERR_MSG", "\t");

            //}

            //strMsgProcessingID = arrString[10];


            //if (arrString.GetUpperBound(0) > 10)
            //{
            //    strMsgVersionID = arrString[11];
            //}

            //if (arrString.GetUpperBound(0) > 11)
            //{
            //    strMsgSequenceNr = arrString[12];
            //    if (strMsgSequenceNr.Length == 0) // optional field not really an error
            //    {
            //        strMsgSequenceNr = strMsgControlID;
            //    }
            //}                   

            string strMSH;
            //    string strMSA = CreateMSA(strMsgControlID, strMsgProcessingID, "D");// "D" equals Delay type D is for Message Received, stored for later processing, F is for Acknowledgement after processing
            // the fields are as follows 0 = MSH, 1 = "|" (BANG or Field Separator to use in the message).
            //                           0 1| 2 | 3 |4  | 5 | 6 | 7 |8| 9|10 | 11| 12| 13|    0 | 1 | 2  | 3  | 4  |
            strMSH = string.Format(@"{0}MSH|^~\&|MCL|001|{1}|{2}|{3}||ACK|{4}|{5}|{6}|{7}|{8}MSA|{9}|{10}|{11}|{12}|{13}{14}{15}{16}",
                              VT,  // {0}
                              m_strSendingApplication, // {1}  who sent it to us should receive the ACK
                                m_strSendingFacility, //{2} who sent it to us should receive the ACK
                                    Time.HL7TimeStamp(), // {3} date/time of our MSH creation
                                        m_strOVControlId, // {4} this is in field 10 of the message we are receiving
                                            m_strProcessingId, // {5} doesn't seem to work unless in both the MSH and MSA
                                                m_strVersionNr,//{6}
                                                    m_strSequenceNr,//{7} // returning the same sequence number here that was sent to us????
                                                        CR, //{8}
                                                            strAckCode, //{9}
                                                                m_strOVControlId, // {10}
                                                                    strAckErrorMsg, // {11} should only display text if the strAckCode is not "AA" so an error occured.
                                                                        m_strSequenceNr, // {12}
                                                                            "D", // {13} delayed acknowledgement type D = Message received stored for later processing F = Acknowledge after processing
                                                                                CR, // {14} end the MSA segment
                                                                                    FS, // {15} File seperator for MSH
                                                                                        CR); // {16} end the MSH segment

            return (strMSH);
        }


        /// <summary>
        /// create the ACK message
        /// 10/17/2006 Rick Crone
        /// </summary>
        /// <returns>string with the ACK message</returns>
        public string CreateMSH_ACK()
        {
            // 4 sending facility - 192???
            // 7 is hl7 time stamp on NOW
            //10 looks like HL7 timestamp with a 1 appened to the end
            //* 11 = processing ID P/T/D
            //*      A value that defines whether the message is part of a production, training, or debugging system.
            //*      Refer to HL7 table 0103 - Processing ID for valid values. 
            // 16 Application acknowledment type
            //      AL - always
            //      NE - never
            //      ER - error / reject conditions only
            //      SU - successful completion only
            string strMSH;
            // 0  1 2  3        4 5 6    7          8 9   10             11 12   15 16    19    21
            //MSH|^~\&|PATACCT|192|||20060918095626||ACK|200609180956261|P|2.3|||AL|NE||| MSA||4512050|||| 
            //                           4     7        10  11
            strMSH = string.Format("{0}MSH|^~\\&|PATACCT|001|||{1}||ACK|{2}1|{3}|2.3|||AL|NE||| MSA||4512050||||{4}{5}{6}",
                                        VT,
                                        Time.HL7TimeStamp(),
                                         Time.HL7TimeStamp(),
                                          m_strProcessingId,
                                          CR,
                                          FS,
                                          CR);
            return (strMSH);
        }
        /// <summary>
        /// create MSH message header 
        /// note: m_strProcessingId default is D for debug - set member variable to P for Production
        /// 11/17/2006 WDK This CreateMSH() will only be good for HMS because of the version number
        /// 2.3 which is hard coded into the MSH string at Field number 10. will need to be updated when versions change
        /// or if we get another client using this system.
        /// 07/27/2006 Rick Crone
        /// </summary>
        /// <param> name="strControlID" </param>
        /// <returns>
        /// string with complete hl7 MSH segment
        /// </returns>
        public string CreateMSH(string strControlID)
        {
            //throw new System.NotImplementedException();
            iNTECounter = 1;
            string strMSH;
            /*
             * 3 = sending application
             * 4 = sending facility
             * 5 = receiving application
             * 6 = receiving facility
             * 7 = date/time of message HL7 datetime stamp format Format: YYYY[MM[DD[HHMM]
             * 9 = message type ORU Observ result/unsolicited ??? may need ORM Order message
             * 10= message control ID ??? 45113190 in HMS example - this is a sequence number from us
             * 11 = processing ID P/T/D
             *      A value that defines whether the message is part of a production, training, or debugging system.
             *      Refer to HL7 table 0103 - Processing ID for valid values. 
             * 12 = HL7 version


            
            */

            // strMSH = string.Format("MSH|^~\\&|LAB|CLIENT|LAB|10000|{0}||ORU|{1}{2}|{3}|2.1", // Ricks original
            // from eddie
            // MSH|^~\\&|MCL|001|ORDENT|001|200610100725| |ORU^R01|0688|P|2.2 
            // Field numbers 0|1 2  |3  |4  |5     |6  |7           |8|9      |10 |11 |12
            strMSH = string.Format("{0}MSH|^~\\&|{1}|{2}|{3}|{4}|{5}||{6}|{7}|{8}|2.3{9}",
                                    VT, //{0}
                                                m_strSendingApplication, //{1}
                                                    m_strSendingFacility, //{2}
                                                        m_strReceivingApplication, //{3}
                                                            m_strReceivingFaciltiy, // {4}
                                                               Time.HL7TimeStamp(),// {5}
                                                                m_strMsgType, //{6}
                                                                 strControlID,// {7}
                                                                        m_strProcessingId,//{8}
                                                                            HL7.CR); //{9}
            m_strHL7Msg = strMSH;
            m_strMSH = strMSH;
            return (strMSH);
        }
        ///<summary>
        /// create PID Patient ID segment
        ///
        /// set the following member variables before calling this function
        /// m_strOVPatId,
        /// m_strPatLName,m_strPatFName,m_strPatMidInit
        /// m_strPatDOB, ccyymmdd
        /// m_strPatSex,
        /// m_strOVPatAccount,
        /// m_strPatSSN - no dashes
        /// 
        /// 08/01/2006 RIck Crone
        ///</summary>
        ///<returns>
        /// string with complete hl7 PID segment
        ///</returns>
        public string CreatePID()
        {
            /*
             * 1 = Set ID - identifies repetitions 
       wdk      * 2 = patient id external - not used in HMS example // could be our account number
             * 3 = patient id internal ??? OV patient ID ??? m_strOVPatId
       wdk      * 4 = Alternate patient id (optional) // could be our meditech number
             * 5 = patient name family name^given name^middle initial or name
                    string m_strPatLName;
                    string m_strPatFName;
                    string m_strPatMidInit;
             * 7 = patient DOB ccyymmdd
             * 8 = patient sex
             * 18 = patient account number ??? OV account number - for charges ???
             * 19 = pat SSN 
            */
            // format the patient name
            if (string.IsNullOrEmpty(m_strPatMidInit))
            {
                m_strPatMidInit = "";
            }
            string strPatName;

            strPatName = string.Format("{0}^{1}^{2}^^^^L", m_strPatLName.Trim(),
                    m_strPatFName.Trim(), m_strPatMidInit.Trim());

            if (!string.IsNullOrEmpty(m_strPatDOB))
            {
                if (m_strPatDOB.Contains(@"/") || m_strPatDOB.Contains(@":"))
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                    string[] expectedFormats = { "G", "g", "D", "d" };
                    DateTime dt = new DateTime();
                    try
                    {
                        dt = DateTime.ParseExact(m_strPatDOB, expectedFormats, culture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (ArgumentNullException ane)
                    {
                        propErrMsg = ane.Message;
                        m_strPatDOB = string.Empty; // wdk 20091016 empty the string rather than report a bad date.
                    }
                    catch (FormatException fe)
                    {
                        propErrMsg = fe.Message;
                        m_strPatDOB = string.Empty; // wdk 20091016 empty the string rather than report a bad date.
                    }
                    catch (ArgumentException ae)
                    {
                        propErrMsg = ae.Message;
                        m_strPatDOB = string.Empty; // wdk 20091016 empty the string rather than report a bad date.

                    }
                    if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
                    {
                        m_strPatDOB = string.Empty;
                    }
                    else
                    {
                        m_strPatDOB = dt.ToString("yyyyMMddHHmm");
                    }
                }
            }
            // end of - format the patient name
            // eddies format        PID|1||28189100750|5069712|PATIENT^NAME||19851212|F  |||||(931)685-5433|||||5069712|123456789 
            //   |1||3          |       |5           ||7       |8  |||||13           |||||18     |19 
            m_strPID = string.Format("PID|1|{0}|{1}|{2}|{3}||{4}|{5}|||||{6}|||||{7}|{8}{9}",
                                        m_strIndustrialAcct,//m_strOVPatId,   //1 eddie wants this to be blank 12062006 m_strIndustrialAcct
                                        m_strOVPatId,
                                        m_strMTMRI, //2
                                         strPatName, //3
                                          m_strPatDOB,  //4
                                           m_strPatSex, //5
                                             "",   // provided by eddies format phone but who's ? 11/20/2006 according to the HL7 code this is the patients phone number and it is optional //6
                                            m_strMTAccount,// rgc/wdk 20090416 changed to Meditech Account  m_strOVPatAccount,  //7
                                             m_strPatSSN,
                                             CR);

            m_strHL7Msg += m_strPID;
            // m_alCreatedMessageSegments.Add(m_strPID);
            return (m_strPID);
        }

        /// <summary>
        /// converts string datetimes like 1/2/2001, or 1/2/2001 10:40, or 1/2/2001 1040 or 
        /// 1/2/2001 10:40 PM to either an 8 digit date or a 12 digit date time depending on the value passed in.
        /// if the final result is not 8 or 12 digits long returns an empty string.
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        private static string ConvertDateTimeToHL7DateString(string strDate)
        {
            string strRetVal = string.Empty;
            if (string.IsNullOrEmpty(strDate))
            {
                return string.Empty;
            }
            strDate.Replace(@"/", "");
            switch (strDate.Length)
            {
                case 8:
                    {
                        strRetVal = string.Format("{0}{1}", strDate.Substring(4, 4), strDate.Substring(0, 4));
                        break;
                    }
            }
            return strDate;
        }

        ///<summary>
        /// create the OBR order segment -  every results needs an order segment
        /// these member varibles must be loaded before calling CreateOBR()
        /// m_strOVOrderId;
        /// m_strOurOrderId;
        /// m_strTestMnem;
        /// m_strTestDesc;
        /// m_strOrderPriority;
        /// m_strCollDateTime; //ccyymmddhhmm
        /// m_strResultDateTime; //ccyymmddhhmm
        /// m_strMeditechReceivedDateTime; //ccyymmddhhmm
        /// m_strOVSpecimenId;
        /// m_ResultsStatus;
        /// 08/01/2006 Rick Crone
        ///</summary>
        ///<returns>
        /// string with complete OBR segment
        ///</returns>
        public string CreateOBR()
        {
            /*
             * 2 = OV order ID
             * 3 = Our order ID
             * 4 = universal service id (Meditech mnemonic ???) ^description
             * 5 = priority (R/S/A) routine/stat/asap
             * 6 = order time (collection time)
             * 7 = result time
             * 13 = specimen received date time
             * 16 = Ordering Provider rgc/wdk 20090820 cancelled messages need this from the original ORDER ORC[12] we are using m_strOrdering_provider
             * 18 = placers field#1 ??? OV specimen ID
             * 25 = results status
             *          O = order received
             *          A = some results
             *          P = preliminary
             *          C = corrected
             *          F = final*/
            // wdk original format          strOBR = string.Format("OBR|1|{0}        |{1}            |{2}^{3}     |{4}|{5}|{6}         ||||   |||{7}         ||||{8}    ||       ||            ||  |{9}",

            //                       0 |1|2          | 3             | 4          |   |   | 7          |||||||14          ||||18     || 20    ||22          || 24| 25          
            // eddies format        OBR|1|0716576^LAB|28189100750^LAB|006072^RPR^L|   |   |200610080504|||||||200610090001||||0000400||0716576||200610100725||KL  |F| 
            //                       0 |1|2  | 3 | 4       ||| 7 |||||||14 |   |||18 || 20||22 || 24 |  25         
            m_strOBR = string.Format("OBR|1|{0}|{1}|{2}^{3}^L|||{4}|||||||{5}|{6}|{10}||{7}||{8}||{9}|||{11}|{12}",
                                      m_strOVOrderId,//per eddie//"0716576^LAB", //0
                                        m_strOurOrderId,// //per eddie //"28189100750^LAB" //1
                                      m_strTestMnem,//"006072",// 2
                                       m_strTestDesc,// "RPR^L",//3
                                            m_strCollDateTime, //4
                                         m_strMeditechReceivedDateTime, ////5
                                            m_strMicroSource, // holder for the micro source 6
                                          m_strOVSpecimenId,//7 // placer field one
                                           m_strOurOrderId,// our specimen number//8
                                            m_strResultDateTime, //m_strOVSpecimenId, //"0716576",//9
                                            m_strOrdering_provider,//10 // rgc/wdk 20090820 selected this from ORDERS orc[12]
                                             m_strResultsStatus, //11
                                                CR);//12

            m_strHL7Msg += m_strOBR;
            // m_alCreatedMessageSegments.Add(m_strOBR);
            return (m_strOBR);
        }

        ///<summary>
        ///Creates the MSA segment
        ///10/27/2006 David Kelly
        ///</summary>
        ///<returns>
        /// string with complete MSA segment
        /// added to return the MSA as part of the MSH_ACK()
        ///</returns>
        public string CreateMSA(string strMsgControlID, string strSequenceNum, string strDelayType)
        {
            /*   SEQ LEN DT  OPT RP/#    TBL#    ITEM #  ELEMENT NAME
             *   1   2   ID  R           0008    00018   Acknowledgment Code
             *   2   20  ST  R                   00010   Message Control ID
             *   3   80  ST  O                   00020   Text Message
             *   4   15  NM  O                   00021   Expected Sequence Number
             *   5   1   ID  B           0102    00022   Delayed Acknowledgment Type
             *   6   100 CE  O                   00023   Error Condition

             * /* TBL# 0008
             * Value     Description
             * AA         Original mode: Application Accept 
             *            Enhanced mode: Application acknowledgment: Accept
             * 
             * AE         Original mode: Application Error 
             *            Enhanced mode: Application acknowledgment: Error
             * 
             * AR         Original mode: Application Reject 
             *            Enhanced mode: Application acknowledgment: Reject
             * 
             * CA         Enhanced mode: Accept acknowledgment: Commit Accept
             * 
             * CE         Enhanced mode: Accept acknowledgment: Commit Error
             * 
             * CR         Enhanced mode: Accept acknowledgment: Commit Reject
             */
            int nSeqNum = Convert.ToInt32(strSequenceNum);
            nSeqNum++;
            string strMSA;
            //                |1  |2  |3        |4  |5  |6|7|8|9           |10|11|12                 |13 |14
            strMSA = string.Format("MSA|{0}|{1}|MSG ACK|{2}{3}",
                            "AA",// message Acknowledgement code
                            strMsgControlID,//message Control Id of the message sent byte the sending system
                            nSeqNum.ToString(), // expected sequence number
                            CR);
            return strMSA;
        }

        /////<summary>

        /// <summary>
        /// Creates the ORC segment
        /// rgc/wdk 20090408 as of this date we DO NOT repeat DO NOT expect this to be used 
        /// to send ORDERS to anyone like (Mayo or their replacement).
        /// THIS IS FOR RESULTS ONLY!!!!
        /// 
        /// In the HMS_ORD_PROCESS expect to send CreateOCR(OC,CM) for orders that we cannot place into our
        /// WOrders table if the order does not exist in our wtest table, or we cannot map the microsource to 
        /// our MicrosourceMap table.
        /// 
        /// rgc/wdk 20090922 if the m_strCollDateTime is empty we are creating an ORC for an ORDER so use
        /// a new timestamp.
        /// otherwise use the original collection date time converted to HL7
        /// </summary>
        /// <param name="strOBR_1_ControlId">RE - Observations to follow
        ///                                  CR - Cancelled as requested
        ///                                  OC - Order Cancelled</param>
        /// <param name="strOBR_5_Status">CA - Order was cancelled \r\n
        ///                               CM - Order is complete\r\n
        ///                               A - Some, but not all results available
        ///                               DC - Order was discontinued
        ///                               IP - In Process</param>
        /// <returns>string with complete ORC segment</returns>
        public string CreateORC(string strOBR_1_ControlId, string strOBR_5_Status)
        {
            /*
             * 1 = Order Control --     ORDERS                  Results
             *                          NW - New Order          RE Observations to follow
             *                          OK - Order Accepted     CR Cancelled as requested
             *                                                  OC Order cancelled
             * 2 = Placers Order #
             * 3 = Fillers Order #
             * 4 = Placer Group #
             * 5 = Order Status                                 CA Order was cancelled
             *                                                  CM Order is complete
             *                                                  
             * 6 = Response Flag
             * 9 = Date/Time of transaction - I think this is time NOW rgc
             * 12 = Use Phymast/Upin contents NOT REQUIRED
             * 13 = Enters Location
             * 14 = Call back Phone number
             */

            if (!string.IsNullOrEmpty(m_strCollDateTime))
            {
                if (m_strCollDateTime.Contains(@"/") || m_strCollDateTime.Contains(@":"))
                {
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                    string[] expectedFormats = { "G", "g", "D", "d" };
                    DateTime dt = new DateTime();
                    try
                    {
                        dt = DateTime.ParseExact(m_strCollDateTime, expectedFormats, culture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                    }
                    catch (ArgumentNullException ane)
                    {
                        propErrMsg = ane.Message;
                        m_strCollDateTime = string.Empty; // wdk 20091016 empty the string rather than report a bad date.
                    }
                    catch (FormatException fe)
                    {
                        propErrMsg = fe.Message;
                        m_strCollDateTime = string.Empty; // wdk 20091016 empty the string rather than report a bad date.
                    }
                    catch (ArgumentException ae)
                    {
                        propErrMsg = ae.Message;
                        m_strCollDateTime = string.Empty; // wdk 20091016 empty the string rather than report a bad date.

                    }
                    if (dt == DateTime.MinValue || dt == DateTime.MaxValue)
                    {
                        m_strCollDateTime = string.Empty;
                    }
                    else
                    {
                        m_strCollDateTime = dt.ToString("yyyyMMddHHmm");
                    }
                }
                else
                {
                    if (m_strCollDateTime.Length > 12) // milliseconds were provided and this should be ccyyMMddHHmm
                    {
                        m_strCollDateTime.Substring(0, 12);
                    }
                }

            }

            // eddies format           ORC|RE |0716576^LAB|28189100750^LAB| | | | | |200610090001|  |  |DOCTOR,NAME^^^^^^^L
            //                           0|1  |2          |3              |4|5|6|7|8|9           |10|11|12                 |13 |14
            m_strORC = string.Format("ORC|{0}|{1}|{2}||{7}||||{3}|||{4}||{5}{6}",
                                         strOBR_1_ControlId, //0
                                         m_strOVOrderId,//1
                                         m_strOurOrderId,//2
                                         m_strCollDateTime,
                                         m_strOrdering_provider,//5 // rgc/wdk 20090820 selected this from ORDERS orc[12],//5
                                         "",// s/b phone number
                                         CR,
                                         strOBR_5_Status); // althought the place holder is {7} it is the 5th element.

            m_strHL7Msg += m_strORC;
            // m_alCreatedMessageSegments.Add(m_strORC);
            return (m_strORC);
        }



        ///<summary>
        /// create the OBX result message segment
        /// member variable that must be set before calling CreateOBX()
        /// m_strTestMnem; // also used in OBR
        /// m_strTestDesc; // also used in OBR
        /// m_strObservationValue,
        /// m_strReferenceRange,
        /// m_strAbnormalFlags,
        /// m_strObservationResultsStatus);
        /// 08/01/2006 Rick Crone
        /// 
        /// rgc/wdk 20090407 note to selves the set id in the obx below is hard coded to 1
        /// will need to be updated to Meditechs OBX counter as each obx is processed from meditech results.
        ///</summary>
        ///<returns>
        /// returns a string with a complete OBX message
        ///</returns>
        ///
        public string CreateOBX()
        {
            /*
             * 2 = ST string data
             * 3 = test mnemonic ^ description
             * 5 = Observation value
             * 7 = reference range - normal value
             * 8 = abnormal flags - N normal A abnormal micro S/R/I/MS/VS
             * 11 = observation results status
             *          C = correction
             *          F = final
             *          P = preliminary
             *          S = partial
             *          W = wrong - wrong patient
            */

            // per eddie            OBX|1|ST|006072^RPR^L||Non Reactive|   |Non Reactive|||N  |F  |||200610100307|KL 
            //  ||2 |3            ||5           |7  |8           |||11
            m_strOBX = string.Format("OBX|1|ST|{0}^{1}||{2}||{3}|||{4}|{5}|||{6}{7}",
                                        m_strTestMnem,//1
                                         m_strTestDesc,//2
                                          m_strObservationValue,//3
                                           m_strReferenceRange,//4
                                            m_strAbnormalFlags,//5
                                            m_strObservationResultsStatus,//6
                                             "MCL^Medical Center Lab^^L",
                                                CR);//

            m_strHL7Msg += m_strOBX;
            // m_alCreatedMessageSegments.Add(m_strOBX);
            return (m_strOBX);
        }
        ///<summary>
        ///creates a NTE message segment
        ///08/01/2006 Rick Crone 
        ///</summary>
        ///<param name='strResultText'> 
        /// results text to be included in the NTE message segment    
        ///</param>
        ///<returns>
        /// a string with a complete NTE message segment
        ///</returns>
        public string CreateNTE(string strResultText)
        {
            /*
             * 2.24.15.2 Source of comment (ID) 00097
            Definition: This field is used when source of comment must be identified. This table may be extended locally during implementation. 
            Table 0105 - Source of comment 

            Value              Description
              L              Ancillary (filler) department is source of comment
              P              Orderer (placer) is source of comment
              O              Other system is source of comment
            
            */
            /* per eddie
            NTE|1|L|Performed At: KL 
            NTE|2|L|LabCorp Louisville
            NTE|3|L|4500 Conaem Drive 
            NTE|4|L|Louisville, KY 402131961 */


            m_strNTE = string.Format("NTE|{0}|L|{1}{2}",
                                    iNTECounter++,
                                       strResultText,
                                        CR);
            //strNTE = string.Format("NTE|||{0}",  strResultText); // wdk original 
            m_strHL7Msg += m_strNTE;
            // m_alCreatedMessageSegments.Add(m_strNTE);
            return (m_strNTE);
        }

        /// <summary>
        /// Parse Name into first last mid
        /// 08/02/2006 Rick Crone
        /// </summary>
        public bool ParseName(string strName)
        {
            //throw new System.NotImplementedException();
            bool bRetVal = true; // success
            //Str MyString = new Str(); // Str is a class in this RFClassLibrary
            bRetVal = Str.ParseName(strName, out m_strPatLName, out m_strPatFName, out m_strPatMidInit, out m_strPatSuffix);
            if (!bRetVal)
            {
                //store error message at this class level
                //m_strErrMsg = MyString.m_strErrMsg;
            }
            return (bRetVal);
        }

        /// <summary>
        /// undocumented
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public bool ParseHL7Name(string strName)
        {
            bool bRetVal = false; // failed
            //Str MyString = new Str(); // Str is a class in this RFClassLibrary
            int iCommaPos = -1;

            // fix name for parser change first ^ to ,
            iCommaPos = strName.IndexOf('^');
            if (iCommaPos < 0)
            {
                propErrMsg = "ERROR ParseHL7Name()failed. Error no ^ in string";
                return (bRetVal);
            }
            strName = strName.Remove(iCommaPos, 1);
            strName = strName.Insert(iCommaPos, ",");
            // change other ^s to ' ' space
            strName = strName.Replace('^', ' ');

            bRetVal = Str.ParseName(strName, out m_strPatLName, out m_strPatFName, out m_strPatMidInit, out m_strPatSuffix);
            if (!bRetVal)
            {
                //propErrMsg = MyString.m_strErrMsg;
            }
            return (bRetVal);
        }
        /// <summary>
        /// Clears this classes DATA member variables.
        /// Note: does not clear the m_strProcessingId;// set to P/T/D Processing / Training / Debug
        /// or the m_strErrMsg.
        /// 08/10/2006 Rick Crone
        /// </summary>
        public void ClearMemberVariables()
        {

            m_strLastSegment = string.Empty;

            m_staticIN1SetID = 1; // rgc/wdk 20090519 static set id if none is in the IN1
            m_staticDG1SetID = 1;
            m_staticGT1SetID = 1;
            m_staticOBRSetID = 1;
            m_staticNTESetID = 1;


            m_dicOrderedTestMnems.Clear();
            //throw new System.NotImplementedException();
            m_strSendingApplication = "";
            m_strOVPatId = "";
            m_strPatLName = "";
            m_strPatFName = "";
            m_strPatMidInit = "";
            m_strPatSuffix = "";
            m_strPatDOB = ""; //ccyymmdd
            m_strPatSex = "";
            m_strOVPatAccount = "";
            m_strPatSSN = "";
            //OBR

            m_llOrders.Clear();
            m_strOVOrderId = "";
            m_strOurOrderId = "";
            m_strTestMnem = ""; // also used in OBX
            m_strTestDesc = ""; // also used in OBX
            m_strMicroSource = "";
            m_strOrderPriority = "";
            m_strCollDateTime = ""; //ccyymmddhhmm
            m_strResultDateTime = ""; //ccyymmddhhmm
            m_strMeditechReceivedDateTime = ""; //ccyymmddhhmm
            m_strOVSpecimenId = "";
            m_dicOVSpecimenId.Clear();
            m_strResultsStatus = "";
            //OBX
            //public string m_strTestMnem; // also used in OBR
            //public string m_strTestDesc; // also used in OBR
            m_strObservationValue = "";
            m_strReferenceRange = "";
            m_strAbnormalFlags = "";
            m_strObservationResultsStatus = "";

            //DG1/////////
            m_strDiagCodingMethod = new string[9];
            m_strDiagCode = new string[9];
            m_strDiagDesc = new string[9];

            iNTECounter = 0;
            m_strHL7Msg = "";
            m_strProcessingId = "";
            m_strSendingFacility = "";
            m_strMsgType = "";
            m_strReceivingFaciltiy = "";
            m_strReceivingApplication = "";
            m_strOVControlId = "";

            // PID
            m_strIndustrialAcct = "";
            m_strMTMRI = "";
            m_strMTAccount = "";
            m_strMaritalStatus = "";

            //OBR
            m_strCliMnem = "";
            m_strPhyUpin = "";

            //OBX
            m_strResultedTestMnem = "";

            //NTE
            m_strComment = "";
            m_strSourceOfComment = "";

            // rgc/wdk 20090312 added variable for IN1 from EHS
            m_strInsAbc = new string[3];
            m_strInsPlanId = new string[3];
            m_strInsCompanyId = new string[3];
            m_strInsCompanyName = new string[3];
            m_strInsCompanyAddr = new string[3];
            m_strInsGroupNo = new string[3];
            m_strInsGroupName = new string[3];
            m_strInsInsuredGroupEmpId = new string[3];
            m_strInsInsuredGroupEmpName = new string[3];

            m_strInsInsuredName = new string[3];
            m_strInsInsuredRelation = new string[3];
            m_strInsInsuredDateOfBirth = new string[3];
            m_strInsInsuredAddress = new string[3];
            m_strInsPolicyNumber = new string[3];
            m_strInsSex = new string[3];

            // rgc/wdk 20090312 GT1 fields added
            m_strGuarName = "";
            m_strGuarAddress = "";
            m_strFinClass = "";


            // wdk 20090316 added for parsing PID for EHS

            m_strPatAddr1 = "";
            m_strPatAddr2 = "";
            m_strPatCity = "";
            m_strPatSt = "";
            m_strPatZip = "";
            m_strPatPhone = "";
            m_nDiagIndicator = 0;
            m_strDiagPriority = "";
            m_strAccBillTo = "";
            // rgc/wdk 20090409 added for additional funacility
            m_strMSH = "";
            m_strPID = "";
            m_strORC = "";
            m_strOBR = "";
            m_strDG1 = "";
            m_strIN1 = "";
            m_strGT1 = "";
            m_strPV1 = "";
            m_strOBX = "";
            m_strNTE = "";

            // rgc/wdk 20100427

            // wdk 20100716 added 
            m_strPlacersGroupNumber = "";
            //m_arrMessageSegments = new string[0];

            m_alTestsOrdered = new ArrayList();

            // wdk 20101027 added
            m_strFinalsOnly = "";

        }

        /// <summary>
        ///  wdk 20101223 added for results only
        /// </summary>
        /// <param name="strHL7Msg"></param>
        /// <returns></returns>
        public bool ParseResult(string strHL7Msg)
        {
            //m_strmyHL7Msg = strHL7Msg; // wdk 20090804 added for use with hl7's CreateUpdatedResultMsg()
            m_strHL7Msg = strHL7Msg; // rgc/wdk 20101202 added for use with Finals only
            bool bRetVal = true;

            m_ERR.m_Logfile.WriteLogFile("Entered ParseMsg()");
            if (string.IsNullOrEmpty(strHL7Msg))
            {
                propErrMsg = "Message was null or empty.";
                m_ERR.ErrorHandler((int)ERR.ErrLevel.eINFO);
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                return false;
            }
            if (strHL7Msg[0] == HL7.VT)
            {
                strHL7Msg = strHL7Msg.Remove(0, 1);
            }
            //strHL7Msg = strHL7Msg.Replace(HL7.FS, (char)0);
            try
            {
                strHL7Msg = strHL7Msg.Remove(strHL7Msg.IndexOf(HL7.FS), 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                // do nothing the HL7.FS was on a cancelled order message we created.
                propErrMsg = "HL7.FS was not in range";

            }
            m_arrMessageSegments = strHL7Msg.Split(new Char[] { HL7.CR }, StringSplitOptions.RemoveEmptyEntries);
            //if (!ValidateHL7Msg(arrSegment))
            //{

            //    return false;
            //}
            string strSegType = "";
            foreach (string strSegment in m_arrMessageSegments)
            {
                if (strSegment.Length < 3)
                {
                    //bRetVal = false;
                    continue; // nothing we can identify.
                }

                strSegType = strSegment.Substring(0, 3);
                switch (strSegType)
                {
                    case "MSH":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered MSH");
                            if (!ParseMSH(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "MSH";
                            m_llValidOrderMsgParts.Remove("MSH");

                            //m_ERR.m_Logfile.WriteLogFile("Leaving MSH");
                            continue;
                        }

                    case "PID":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered PID");

                            if (!ParsePID(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "PID";
                            m_llValidOrderMsgParts.Remove("PID");

                            //m_ERR.m_Logfile.WriteLogFile("Leaving PID ");
                            continue;
                        }

                    case "PV1":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered PV1");

                            if (!ParsePV1(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "PV1";
                            m_llValidOrderMsgParts.Remove("PV1");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving PV1");
                            continue;
                        }

                    case "IN1":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered IN1");

                            if (!ParseIN1(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "IN1";
                            m_llValidOrderMsgParts.Remove("IN1");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving IN1");
                            continue;
                        }

                    case "GT1":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered GT1");

                            if (!ParseGT1(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "GT1";
                            m_llValidOrderMsgParts.Remove("GT1");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving GT1");
                            continue;
                        }

                    case "ORC":
                        {
                            // m_ERR.m_Logfile.WriteLogFile("Entered ORC");

                            if (!ParseORC(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "ORC";
                            m_llValidOrderMsgParts.Remove("ORC");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving ORC");
                            continue;
                        }

                    case "OBR":
                        {
                            //  m_ERR.m_Logfile.WriteLogFile("Entered OBR");

                            if (!ParseOBR(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "OBR";
                            m_llValidOrderMsgParts.Remove("OBR");

                            //  m_ERR.m_Logfile.WriteLogFile("Leaving OBR");
                            continue;
                        }

                    case "NTE":
                        {
                            // m_ERR.m_Logfile.WriteLogFile("Entered NTE");

                            if (!ParseNTE(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment += " NTE";

                            // m_ERR.m_Logfile.WriteLogFile("Leaving NTE");
                            continue;
                        }
                    case "OBX":
                        {
                            // m_ERR.m_Logfile.WriteLogFile("Entered OBX");

                            if (!ParseOBX(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "OBX";

                            //  m_ERR.m_Logfile.WriteLogFile("Leaving OBX");
                            continue;
                        }
                    case "DG1":
                        {
                            //  m_ERR.m_Logfile.WriteLogFile("Entered DG1");

                            if (!ParseDG1(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "DG1";
                            m_llValidOrderMsgParts.Remove("DG1");

                            //  m_ERR.m_Logfile.WriteLogFile("Leaving DG1");
                            continue;
                        }
                    case "ZPS": // only on results and we don't need to know the performing site.
                        {
                            break;
                        }
                    default:
                        {
                            //error message not what we were expecting we didn't trap for it.
                            // bRetVal = false;
                            // m_strErrMsg = string.Format("Unexpected segment in message.\r\nodeSchema [{0}]", strSegment);
                            // break;
                            //  m_ERR.m_Logfile.WriteLogFile("Entered default");

                            propErrMsg = string.Format("{0} is not a valid segment type.\r\n", strSegType);
                            m_strLastSegment = string.Empty;

                            // m_ERR.m_Logfile.WriteLogFile("Leaving default");
                            continue; // 12/06/2006 at this time we don't care

                        }
                } // end of switch
            } // end of Foreach

            m_ERR.m_Logfile.WriteLogFile(string.Format("Leaving ParseResult() with retval [{0}]", bRetVal.ToString()));
            return bRetVal;
        }


        /// <summary>
        /// Parses a complete HL7 message loading this class's member variables
        /// Adds any errors found in the HL7 message to the m_dsErrors dataset with the [1] ERRORS [2] Format [3] Message
        /// </summary>
        /// <param name="strHL7Msg"></param>
        /// <returns></returns>
        public bool ParseMsg(string strHL7Msg)
        {
            //strHL7Msg = strHL7Msg.Replace("'",""); // wdk 20110228 remove the apostraphes
            m_strHL7Msg = strHL7Msg; // rgc/wdk 20101202 added for use with Finals only
            bool bRetVal = true;
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("MSH"));
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("PID"));
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("PV1"));
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("IN1"));
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("GT1"));
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("DG1"));
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("ORC"));
            m_llValidOrderMsgParts.AddLast(new LinkedListNode<string>("OBR"));
            m_ERR.m_Logfile.WriteLogFile("Entered ParseMsg()");
            if (string.IsNullOrEmpty(strHL7Msg))
            {
                propErrMsg = "Message was null or empty.";
                m_ERR.ErrorHandler((int)ERR.ErrLevel.eINFO);
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                return false;
            }
            if (strHL7Msg[0] == HL7.VT)
            {
                strHL7Msg = strHL7Msg.Remove(0, 1);
            }
            //strHL7Msg = strHL7Msg.Replace(HL7.FS, (char)0);
            try
            {
                strHL7Msg = strHL7Msg.Remove(strHL7Msg.IndexOf(HL7.FS), 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                // do nothing the HL7.FS was on a cancelled order message we created.
                propErrMsg = "HL7.FS was not in range";

            }
            m_arrMessageSegments = strHL7Msg.Split(new Char[] { HL7.CR }, StringSplitOptions.RemoveEmptyEntries);
            //if (!ValidateHL7Msg(arrSegment))
            //{

            //    return false;
            //}
            string strSegType = "";
            foreach (string strSegment in m_arrMessageSegments)
            {
                if (strSegment.Length < 3)
                {
                    //bRetVal = false;
                    continue; // nothing we can identify.
                }

                strSegType = strSegment.Substring(0, 3);
                switch (strSegType)
                {
                    case "MSH":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered MSH");
                            if (!ParseMSH(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "MSH";
                            m_llValidOrderMsgParts.Remove("MSH");

                            //m_ERR.m_Logfile.WriteLogFile("Leaving MSH");
                            continue;
                        }

                    case "PID":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered PID");

                            if (!ParsePID(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "PID";
                            m_llValidOrderMsgParts.Remove("PID");

                            //m_ERR.m_Logfile.WriteLogFile("Leaving PID ");
                            continue;
                        }

                    case "PV1":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered PV1");
                            string strReplaced = strSegment.Replace("'", "");
                            if (!ParsePV1(strReplaced))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "PV1";
                            m_llValidOrderMsgParts.Remove("PV1");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving PV1");
                            continue;
                        }

                    case "IN1":
                        {
                            //m_ERR.m_Logfile.WriteLogFile("Entered IN1");
                            string strReplaced = strSegment.Replace("'", "");
                            if (!ParseIN1(strReplaced))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "IN1";
                            m_llValidOrderMsgParts.Remove("IN1");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving IN1");
                            continue;
                        }
                    case "IN2":
                        {
                            break; // wdk 20101223 EHS has started sending this message segment we don't need.
                        }

                    case "GT1":
                        {
                            string strReplaced = strSegment.Replace("'", "");
                            if (!ParseGT1(strReplaced))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "GT1";
                            m_llValidOrderMsgParts.Remove("GT1");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving GT1");
                            continue;
                        }

                    case "ORC":
                        {
                            // m_ERR.m_Logfile.WriteLogFile("Entered ORC");

                            if (!ParseORC(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "ORC";
                            m_llValidOrderMsgParts.Remove("ORC");

                            // m_ERR.m_Logfile.WriteLogFile("Leaving ORC");
                            continue;
                        }

                    case "OBR":
                        {
                            //  m_ERR.m_Logfile.WriteLogFile("Entered OBR");

                            if (!ParseOBR(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "OBR";
                            m_llValidOrderMsgParts.Remove("OBR");

                            //  m_ERR.m_Logfile.WriteLogFile("Leaving OBR");
                            continue;
                        }

                    case "NTE":
                        {
                            // m_ERR.m_Logfile.WriteLogFile("Entered NTE");

                            if (!ParseNTE(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment += " NTE";

                            // m_ERR.m_Logfile.WriteLogFile("Leaving NTE");
                            continue;
                        }
                    case "OBX":
                        {
                            // m_ERR.m_Logfile.WriteLogFile("Entered OBX");

                            if (!ParseOBX(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "OBX";

                            //  m_ERR.m_Logfile.WriteLogFile("Leaving OBX");
                            continue;
                        }
                    case "DG1":
                        {
                            //  m_ERR.m_Logfile.WriteLogFile("Entered DG1");

                            if (!ParseDG1(strSegment))
                            {
                                bRetVal = false;
                                // break; wdk 20091001 allow the rest of the parts to validate.
                            }
                            m_strLastSegment = "DG1";
                            m_llValidOrderMsgParts.Remove("DG1");

                            //  m_ERR.m_Logfile.WriteLogFile("Leaving DG1");
                            continue;
                        }
                    case "ZPS": // only on results and we don't need to know the performing site.
                        {
                            break;
                        }
                    default:
                        {
                            //error message not what we were expecting we didn't trap for it.
                            // bRetVal = false;
                            // m_strErrMsg = string.Format("Unexpected segment in message.\r\nodeSchema [{0}]", strSegment);
                            // break;
                            //  m_ERR.m_Logfile.WriteLogFile("Entered default");

                            propErrMsg = string.Format("{0} is not a valid segment type.\r\n", strSegType);
                            m_strLastSegment = string.Empty;

                            // m_ERR.m_Logfile.WriteLogFile("Leaving default");
                            continue; // 12/06/2006 at this time we don't care

                        }
                } // end of switch
            } // end of Foreach


            // wdk 20101027 added "&& m_strMsgType.Substring(0,3) == "ORM"" so we don't do this on results.
            if (m_llValidOrderMsgParts.First != null && m_strMsgType.Substring(0, 3) == "ORM")
            {
                bRetVal = false;
                while (m_llValidOrderMsgParts.First != null)
                {
                    propErrMsg = string.Format("{0} not provided in Order message.\r\n", m_llValidOrderMsgParts.First.Value.ToString());
                    m_ERR.ErrorHandler((int)ERR.ErrLevel.eINFO);
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                    m_llValidOrderMsgParts.RemoveFirst();
                }
            }

            // use sql to add the previouly ordered tests for this patient at this client, for this insurance
            // and this collection date to the orders dataset.
            if (m_dsOBR != null)
            {
                m_ERR.m_Logfile.WriteLogFile("Entered if (m_dsOBR != null)");

                COMP_HL7_RowChanged();
                // now checkit
                DataTableReader dt = m_dsOBR.Tables["PREVIOUS_ORDERS"].CreateDataReader();
                while (dt.Read())
                {
                    DataTableReader dtOrders = m_dsOBR.Tables["TESTS"].CreateDataReader();
                    while (dtOrders.Read())
                    {
                        if (dtOrders["TEST_MNEM"].ToString().ToUpper() == dt["TEST_MNEM"].ToString().ToUpper())
                        {
                            // previously ordered
                            if (dt["STATUS"].ToString() != "CAN")
                            {
                                m_ERR.AddErrorToDataSet("OBR`",
                                        dtOrders["TEST_MNEM"].ToString(),
                                            string.Format("TEST {0} previously ordered for {1}", dt["TEST_MNEM"], m_strCollDateTime));
                            }
                        }
                    }
                }
                m_ERR.m_Logfile.WriteLogFile("Leaving if (m_dsOBR != null)");
            }
            m_ERR.m_Logfile.WriteLogFile(string.Format("Leaving ParseMsg() with retval [{0}]", bRetVal.ToString()));
            return bRetVal;
        }

        /// <summary>
        /// Only expect to validate orders
        /// 1. MSH must have 
        /// </summary>
        /// <param name="arrSegment"></param>
        /// <returns></returns>
        private bool ValidateHL7Msg(string[] arrSegment)
        {
            return false;
            //LinkedList<string> llParts = new LinkedList<string>();
            //llParts.AddLast(new LinkedListNode<string>("MSH"));
            //llParts.AddLast(new LinkedListNode<string>("PID"));
            //llParts.AddLast(new LinkedListNode<string>("PV1"));
            //llParts.AddLast(new LinkedListNode<string>("IN1"));
            //llParts.AddLast(new LinkedListNode<string>("GT1"));
            //llParts.AddLast(new LinkedListNode<string>("DG1"));
            //llParts.AddLast(new LinkedListNode<string>("ORC"));
            //llParts.AddLast(new LinkedListNode<string>("OBR"));

            //foreach (string segment in arrSegment)
            //{
            //    string[] segmentPart = segment.Split(new char[] { '|' });
            //    if (llParts.Find(segment[0]) != null)
            //    {
            //        llParts.Remove(segment[0]);
            //    }
            //}

            //propErrMsg = strErr;
        }

        private bool ParseDG1(string strLine)
        {
            // wdk 20090316 using the set id as the icd9_X identifier. Will parse up to nine 
            #region notes
            /*
        <xs:element name="_1_set-id" type="xs:string" />
        <xs:element name="_2_diagnosis-coding-method" type="xs:string" />
        <xs:element name="_3_diagnosis-code" type="xs:string" />
        <xs:element name="_4_diagnosis-description" type="xs:string" />
        <xs:element name="_5_diagnosis-date-time" type="xs:string" />
        <xs:element name="_6_diagnosis-type" type="xs:string" />
        <xs:element name="_7_major-diagnostic-category" type="xs:string" />
        <xs:element name="_8_diagnostic-related-group" type="xs:string" />
        <xs:element name="_9_drg-approval-indicator" type="xs:string" />
        <xs:element name="_10_drg-grouper-review-code" type="xs:string" />
        <xs:element name="_11_outlier-type" type="xs:string" />
        <xs:element name="_12_outlier-days" type="xs:string" />
        <xs:element name="_13_grouper-version-and-type" type="xs:string" />
        <xs:element name="_14_diagnosis-priority" type="xs:string" />
        <xs:element name="_15_diagnosis-code" type="xs:string" />
        <xs:element name="_16_diagnosing-clinician" type="xs:string" />
        <xs:element name="_17_diagnosis-classification" type="xs:string" />
        <xs:element name="_18_confidential-indicator" type="xs:string" />
        <xs:element name="_19_attestation-date-time" type="xs:string" />
             * */
            #endregion notes

            bool bRetVal = true;
            m_strDG1 = strLine;
            // throw new NotImplementedException();
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.

            string[] segment = new string[(int)HL7SegmentMax.eDG1];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < (int)HL7SegmentMax.eDG1; i++)
            {
                segment[i] = "";
            }
            string[] strMax =
            strLine.Split(new Char[] { m_cMsgSplitChar });
            strMax.CopyTo(segment, 0);

            if (!string.IsNullOrEmpty(segment[1]))
            {
                if (!int.TryParse(segment[1], out m_nDiagIndicator))
                {
                    m_nDiagIndicator = m_staticDG1SetID;
                }
            }
            else
            {
                m_nDiagIndicator = m_staticDG1SetID;

            }
            m_staticDG1SetID++;
            //    m_nDiagIndicator--;

            if (segment.GetUpperBound(0) > 16)
            {
                m_strDiagPriority = segment[15]; // wdk 20090316 not provided but will be useful if it is included.
            }


            // wdk 20090316 per the DG1 manual (version 2.3) 
            // rgc/wdk 20090514 this is for the Meditech EHS interface
            if (string.IsNullOrEmpty(segment[2]))//.Length == 0)
            {
                // rgc/wdk 20090514 the HL7 messages will contain DG1 that are modeled per the (version 2.3 syntax)
                //DG1|1     ||I9^034.0^STREPTOCOCCAL SORE THROAT||
                //   |icd9_1|
                //DG1|2     ||I9^787.01^NAUSEA WITH VOMITING|| 
                //   |icd9_2| 
                if (segment[3].Contains("^"))
                {
                    //string[] strNoFail = new string[3];
                    string[] strNoFail = new string[] { "", "", "", "", "", "", "" }; //rgc/wdk 20100429 initialize the array to empty not null
                    string[] strSubPart = segment[3].Split(new char[] { '^' });
                    strSubPart.CopyTo(strNoFail, 0);
                    m_strDiagCodingMethod[m_nDiagIndicator - 1] = strNoFail[0]; // ICD9, ICD10 DG1[2]
                    m_strDiagCode[m_nDiagIndicator - 1] = strNoFail[1]; // 786.2, 294.11 etc DG1[3] 
                    m_strDiagDesc[m_nDiagIndicator - 1] = strNoFail[2]; // Cough, fever etc. DG1[4]

                }

            }
            else
            {  // as initially established by Bradley and Mike Garner for Dyersburg EHS meditech interface.
                //DG1|1|I9|034.0|STREPTOCOCCAL SORE THROAT 
                //DG1|2|I9|787.01|NAUSEA WITH VOMITING 
                m_strDiagCodingMethod[m_nDiagIndicator - 1] = segment[2]; // ICD9, ICD10 DG1[2]
                m_strDiagCode[m_nDiagIndicator - 1] = segment[3]; // 786.2, 294.11 etc DG1[3] 
                m_strDiagDesc[m_nDiagIndicator - 1] = segment[4]; // Cough, fever etc. DG1[4]
            }
            if (string.IsNullOrEmpty(m_strDiagCode[m_nDiagIndicator - 1]))
            {
                bRetVal = false;
                // wdk 20091109 updated to add the DG1 number that is empty or null
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("DG1[3] DIAGNOSIS CODE IS NULL or EMPTY for DG1[{0}]", m_nDiagIndicator));
            }
            return bRetVal;
        }

        /// <summary>
        /// MSH message header.
        /// 
        /// Nothing in the MSH of interest today.
        /// It is the start of a new message so clear the member variables.
        /// Must remove the FS and VT characters before getting here. rgc/wdk 20090507
        /// 
        /// 
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public bool ParseMSH(string strLine)
        {
            ClearMemberVariables();
            bool bRetVal = true;

            m_strMSH = strLine.Trim();
            m_cMsgSplitChar = strLine[3];
            string[] strSplitMax = strLine.Split(new char[] { m_cMsgSplitChar });
            string[] arrStrings = new string[(int)HL7.HL7SegmentMax.eMSH];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < (int)HL7SegmentMax.eMSH; i++)
            {
                arrStrings[i] = "";
            }
            strSplitMax.CopyTo(arrStrings, 0);

            try
            {
                // rgc/wdk 20100203 EHS is now sending lab where we expect EHS. If we get HL7 must be
                // billed to insurance so don't bother checking just set it an forget it.
                // m_strAccBillTo = m_strSendingApplication.ToUpper() == "EHS" ? "INSURANCE" : ""; // for WREQ
                m_strAccBillTo = "INSURANCE";
                m_strFinClass = "M"; // rgc/wdk 20090616 "M" Ins Charge. Truly the FinClass not the FinCode
                // Sending application 
                if (!string.IsNullOrEmpty(arrStrings[2]))
                {
                    m_strSendingApplication = arrStrings[2];
                }
                else
                {
                    propErrMsg = "MSH[3] -- Sending Application is empty.";
                    m_ERR.AddErrorToDataSet("INFO", "MSH[3] -- Sending Application is empty.");
                }
                // sending facility
                if (!string.IsNullOrEmpty(arrStrings[3]))
                {
                    // wdk 20100517 added label printer to the table so when looking this up to find the 
                    // client split on the pipe bar for the first element
                    m_strSendingFacility = arrStrings[3];
                }
                else
                {
                    propErrMsg = "MSH[4] -- Sending Facility is empty";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                    bRetVal = false;
                }

                // Message type ORM is for ORDERS, ORU is for RESULTS.
                // rgc/wdk 20111019 for LIFEPOINT / EHS HL7 this could be ADT or or something else
                if (!string.IsNullOrEmpty(arrStrings[8]))
                {
                    m_strMsgType = arrStrings[8];
                }
                else
                {
                    propErrMsg = "MSH[9] -- Message Type is empty.";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                    bRetVal = false;
                }
                // Other vendoer Control ID
                if (!string.IsNullOrEmpty(arrStrings[9]))
                {
                    m_strOVControlId = arrStrings[9]; // in the MSH the first '|' should be counted as the 1 element. but our split removes the '|' so we query this out of 9
                }
                else
                {
                    propErrMsg = "MSH[10] -- Message control id is empty.";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                    bRetVal = false;
                }
                // processing id D,T,P 
                if (!string.IsNullOrEmpty(arrStrings[10]))
                {
                    m_strProcessingId = arrStrings[10];
                }
                else
                {
                    propErrMsg = "MSH[11] -- Processing id is empty.";
                }
                if (m_strMsgType.Substring(0, 3) == "ORM")
                {
                    // version number 
                    if (!string.IsNullOrEmpty(arrStrings[11]))
                    {
                        m_strVersionNr = arrStrings[11];
                    }
                    else
                    {
                        propErrMsg = "MSH[12] -- Version number is empty.";
                    }
                    // Expected sequence number
                    if (!string.IsNullOrEmpty(arrStrings[12]))
                    {
                        m_strSequenceNr = arrStrings[12];
                    }
                    else
                    {
                        propErrMsg = "MSH[13] -- Expected sequence number is empty.";
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                propErrMsg = string.Format("MSH only contains {0} segments. We expect message sequence number in segment 13",
                                        arrStrings.Length);

                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);

                bRetVal = false;
            }
            return (bRetVal);
        }
        /// <summary>
        /// Nothing in the IN1 of interest today. So do nothing.
        /// IN1 insurance.
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public bool ParseIN1(string strLine)
        {
            #region notes
            /*
        <xs:element name="_1_set-id" type="xs:string" />
        <xs:element name="_2_insurance-plan-id" type="xs:string" />
        <xs:element name="_3_insurance-company-id" type="xs:string" />
        <xs:element name="_4_insurance-company-name" type="xs:string" />
        <xs:element name="_5_insurance-company-address" type="xs:string" />
        <xs:element name="_6_insurance-company-contact-person" type="xs:string" />
        <xs:element name="_7_insurance-company-phone-number" type="xs:string" />
        <xs:element name="_8_group-number" type="xs:string" />
        <xs:element name="_9_group-name" type="xs:string" />
        <xs:element name="_10_insureds-group-emp-id" type="xs:string" />
        <xs:element name="_11_insureds-group-emp-name" type="xs:string" />
        <xs:element name="_12_date-plan-effective" type="xs:string" />
        <xs:element name="_13_date-plan-expiration" type="xs:string" />
        <xs:element name="_14_authorization-information" type="xs:string" />
        <xs:element name="_15_plan-type" type="xs:string" />
        <xs:element name="_16_name-of-insured" type="xs:string" />
        <xs:element name="_17_insureds-relation-to-patient" type="xs:string" />
        <xs:element name="_18_date-of-birth-insureds" type="xs:string" />
        <xs:element name="_19_insureds-address" type="xs:string" />
        <xs:element name="_20_assignment-of-benefits" type="xs:string" />
        <xs:element name="_21_coord-of-ben-priority" type="xs:string" />
        <xs:element name="_22_notice-of-admission-flag" type="xs:string" />
        <xs:element name="_23_notice-of-admission-date" type="xs:string" />
        <xs:element name="_24_report-of-eligibility-flag" type="xs:string" />
        <xs:element name="_25_date-report-of-eligibility" type="xs:string" />
        <xs:element name="_26_release-information-code" type="xs:string" />
        <xs:element name="_27_pre-admit-cert" type="xs:string" />
        <xs:element name="_28_datetime-verification" type="xs:string" />
        <xs:element name="_29_verification-by" type="xs:string" />
        <xs:element name="_30_type-of-agreement-code" type="xs:string" />
        <xs:element name="_31_billing-status" type="xs:string" />
        <xs:element name="_32_lifetime-reserve-days" type="xs:string" />
        <xs:element name="_33_delay-before-lr-day" type="xs:string" />
        <xs:element name="_34_company-plan-code" type="xs:string" />
        <xs:element name="_35_policy-number" type="xs:string" />
        <xs:element name="_36_policy-deductible" type="xs:string" />
        <xs:element name="_37_policy-limit-amount" type="xs:string" />
        <xs:element name="_38_policy-limit-days" type="xs:string" />
        <xs:element name="_39_room-rate-semi-private" type="xs:string" />
        <xs:element name="_40_room-rate-private" type="xs:string" />
        <xs:element name="_41_insureds-employment-status" type="xs:string" />
        <xs:element name="_42_insureds-sex" type="xs:string" />
        <xs:element name="_43_insureds-employer-address" type="xs:string" />
        <xs:element name="_44_verification-status" type="xs:string" />
        <xs:element name="_45_prior-insurance-plan-id" type="xs:string" />
        <xs:element name="_46_coverage-type" type="xs:string" />
        <xs:element name="_47_handicap" type="xs:string" />
        <xs:element name="_48_insureds-id-number" type="xs:string" />
             * */
            #endregion notes

            bool bRetVal = true;
            m_strIN1 = strLine;
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            string[] strMax = strLine.Split(new Char[] { m_cMsgSplitChar });
            // wdk 20100713 added to remove 4th and greater insurance calls that are crashing the service.
            if (int.Parse(strMax[1].ToString()) > 3)
            {
                // fake saving and allow the message to be put into the data_hl7_msg table.
                return true;
            }
            // rgc/wdk 20090520 modified the array creation to the larger of the nte documentation and the actual array sent to us.
            int nArraySize = (int)HL7SegmentMax.eIN1;
            // rgc/wdk 20090520 if the NTE definition is less than the actual number of elements submitted use the 
            // actual number of elements sent to us.
            if ((int)HL7SegmentMax.eIN1 < strMax.GetLength(0))
            {
                nArraySize = strMax.GetLength(0);
            }
            string[] segment = new string[nArraySize]; //new string[(int)HL7SegmentMax.eNTE];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < nArraySize; i++)
            {
                segment[i] = "";
            }
            strMax.CopyTo(segment, 0);
            // rgc/wdk 20090519 

            if (string.IsNullOrEmpty(segment[1].Trim()))
            {
                segment[1] = m_staticIN1SetID.ToString();
            }

            m_strInsAbc[m_staticIN1SetID - 1] = ((char)(64 + m_staticIN1SetID)).ToString(); //  set_id or 1 = A, 2 = B, 3 = C  Add 64 and cast to char for our system
            // InsPlanId from hl7[2] in Mikes Ins table will be Company_id plus Plan_ID 
            m_strInsPlanId[m_staticIN1SetID - 1] = segment[2];
            if (string.IsNullOrEmpty(segment[3]))
            {
                m_strErrMsg = "IN1[3] is blank";
                //m_ERR.AddErrorToDataSet("ERROR", "IN1", strLine);
                StringBuilder sb = new StringBuilder();
                foreach (string str in m_arrMessageSegments)
                {
                    sb.AppendFormat("{0}\r\n", str);
                }
                m_ERR.m_Email.Send("ORDERS.PROCESS@WTH.ORG", "CAROL.SELLARS@WTH.ORG;DAVID.KELLY@WTH.ORG", m_strErrMsg, sb.ToString());
                //bRetVal = false; //rgc/wdk 20100304
            }
            m_strInsCompanyId[m_staticIN1SetID - 1] = segment[3]; // hl7[3] 
            if (m_staticIN1SetID == 1) // wdk 20091030 added to keep from overwriting the primary insurance billing.
            {
                // account bill to is the primary insurance. 
                if (!ConvertCompanyIdToBillTo(m_staticIN1SetID - 1))
                {
                    m_strErrMsg = "Could not convert companyID to billto";
                    //m_ERR.AddErrorToDataSet("ERROR", "IN1", strLine);
                    //bRetVal = false; //rgc/wdk 20100304
                }
            }
            m_strInsCompanyName[m_staticIN1SetID - 1] = segment[4]; // hl7[4]
            m_strInsCompanyAddr[m_staticIN1SetID - 1] = segment[5]; // hl7[5]
            m_strInsGroupNo[m_staticIN1SetID - 1] = segment[8]; // hl7[8]
            m_strInsGroupName[m_staticIN1SetID - 1] = segment[9]; // hl7[9]
            m_strInsInsuredGroupEmpId[m_staticIN1SetID - 1] = segment[10]; // hl7[10]
            m_strInsInsuredGroupEmpName[m_staticIN1SetID - 1] = segment[11]; // hl7[11]
            m_strInsInsuredName[m_staticIN1SetID - 1] = segment[16]; // hl7[16]
            m_strInsInsuredRelation[m_staticIN1SetID - 1] = segment[17]; // hl7[17] will need to convert from "SELF" etc to "01" etc
            m_strInsInsuredDateOfBirth[m_staticIN1SetID - 1] = segment[18]; // hl7[18] 
            m_strInsInsuredAddress[m_staticIN1SetID - 1] = segment[19]; // hl7[19]
            m_strInsPolicyNumber[m_staticIN1SetID - 1] = segment[35];
            m_strInsSex[m_staticIN1SetID - 1] = segment[42];

            m_staticIN1SetID++;
            return (bRetVal);
        }
        /// <summary>
        /// This function is not used but is provided for future reference if needed
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        private bool ParseIN2(string strLine)
        {
            return false;

        }
        /// <summary>
        /// This uses the IN1[3] field. 
        /// wdk 20100219 
        /// </summary>
        /// <param name="nSetId"></param>
        private bool ConvertCompanyIdToBillTo(int nSetId)
        {
            bool bRetVal = false;
            m_strBillTo = "COMM.H";
            // throw new NotImplementedException();
            if (string.IsNullOrEmpty(m_strInsCompanyId[nSetId])) // rgc/wdk 20090526 HL7 message 1789592 from EHS  m_strInsPlanId[nSetId])) 
            {
                //m_strErrMsg = string.Format("IN1|{0}|[3] has no code assigned.", nSetId);
                //m_ERR.AddErrorToDataSet("ERROR", "IN1", m_strErrMsg);
                return bRetVal;
            }
            if (!m_strInsCompanyId[nSetId].ToUpper().Contains("SELF PAY"))
            {
                if (m_strInsCompanyId[nSetId].Length != 7) // rgc/wdk 20090526 see note above.
                {
                    if (m_strInsCompanyId[nSetId].Length != 5)
                    {
                        //m_strErrMsg = string.Format("Insurance company code {0} has an invalid length of {1}.", m_strInsCompanyId[nSetId], m_strInsCompanyId[nSetId].Length);
                        //m_ERR.AddErrorToDataSet("ERROR", "FORMAT", m_strErrMsg);
                        return bRetVal;
                    }
                }
            }

            DBAccess dbIns = new DBAccess("MCLOE", "GOMCLLIVE", "dict_ehs_insurance");
            // rgc/wdk 20090526 the m_strBillTo in the wreq is used by Meditech to 
            string strRetrieve = string.Format("plan_code = '{0}'", m_strInsCompanyId[0].ToString());
            m_strBillTo = //m_strInsFinCode[nSetId] =  
                dbIns.GetField("dict_ehs_insurance", "billing_ins_code",
                    strRetrieve,// rgc/wdk 20100127 changed nSetId] to 0 (primary insurance only) ), // rgc/wdk 20090526 m_strInsPlanId[nSetId]),
                           out m_strErrMsg);

            m_ERR.m_Logfile.WriteLogFile(string.Format("HL7.m_strBillTo equals [{0}]", m_strBillTo));
            m_ERR.m_Logfile.WriteLogFile(string.Format("INSURANCE's errorl message [{0}]", dbIns.propErrMsg));
            bRetVal = true;
            // rgc/wdk 20090526 if NA is returned we don't have the plan coded. Possible reasons could be
            // billing medicare on a 1500 code passed was 00100 should be UB00100
            // this should set the wreq status as "CAN" we can't order any thing and get paid on this insurance.
            // wdk 20100129 added isNullOrEmpty()check 
            // in the meeting on 20100310 Carol said we should not cancel the orders but set the bill to to a 
            // default of H. 
            if (m_strBillTo == "NA" || string.IsNullOrEmpty(m_strBillTo))
            {
                // m_strErrMsg = string.Format("Insurance company code {0} is invalid. Table query returned NA.", m_strInsCompanyId[nSetId]);
                // m_ERR.AddErrorToDataSet("ERROR", "FORMAT", m_strErrMsg);
                bRetVal = false;
                m_strBillTo = "COMM.H";
            }

            return bRetVal;
        }


        /// <summary>
        /// Get the guarantor info from the HL7 message for EHS project.
        /// GT1 guarantor
        /// 08/29/2006 Rick Crone
        ///
        /// GT1[Setid 1][5] WReq and WPat GuarName and Guar_addr1 is the primary insurance (ins_a_b_c 'A' insurance)
        /// this makes the WReq and WPat's address the zero element of the m_strGuarAddress[].
        /// while subsequent GT1[setid's 1,2, and 3][5] are the WIns holder's address (NON PARSED) 
        ///
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns>true when able to identify the guarantor and false when we can't</returns>
        public bool ParseGT1(string strLine)
        {
            #region notes
            /*
                    <xs:element name="_1_set-id" type="xs:string" />
        <xs:element name="_2_guarantor-number" type="xs:string" />
        <xs:element name="_3_guarantor-name" type="xs:string" />
        <xs:element name="_4_guarantor-spouse-name" type="xs:string" />
        <xs:element name="_5_guarantor-address" type="xs:string" />
        <xs:element name="_6_guarantor-phone-num-home" type="xs:string" />
        <xs:element name="_7_guarantor-phone-num-business" type="xs:string" />
        <xs:element name="_8_datetime-guarantor-birth" type="xs:string" />
        <xs:element name="_9_guarantor-sex" type="xs:string" />
        <xs:element name="_10_guarantor-type" type="xs:string" />
        <xs:element name="_11_guarantor-relationship" type="xs:string" />
        <xs:element name="_12_guarantor-ssn" type="xs:string" />
        <xs:element name="_13_date-guarantor-begin" type="xs:string" />
        <xs:element name="_14_date-guarantor-end" type="xs:string" />
        <xs:element name="_15_guarantor-priority" type="xs:string" />
        <xs:element name="_16_guarantor-employer-name" type="xs:string" />
        <xs:element name="_17_guarantor-employer-address" type="xs:string" />
        <xs:element name="_18_guarantor-employer-phone-number" type="xs:string" />
        <xs:element name="_19_guarantor-employee-id-number" type="xs:string" />
        <xs:element name="_20_guarantor-employment-status" type="xs:string" />
        <xs:element name="_21_guarantor-organization-name" type="xs:string" />
        <xs:element name="_22_guarantor-billing-hold-flag" type="xs:string" />
        <xs:element name="_23_guarantor-credit-rating-code" type="xs:string" />
        <xs:element name="_24_datetime-guarantor-death" type="xs:string" />
        <xs:element name="_25_guarantor-death-flag" type="xs:string" />
        <xs:element name="_26_guarantor-charge-adjustment-code" type="xs:string" />
        <xs:element name="_27_guarantor-household-annual-income" type="xs:string" />
        <xs:element name="_28_guarantor-household-size" type="xs:string" />
        <xs:element name="_29_guarantor-employer-id-number" type="xs:string" />
        <xs:element name="_30_guarantor-marital-status-code" type="xs:string" />
        <xs:element name="_31_date-guarantor-hire-effective" type="xs:string" />
        <xs:element name="_32_date-employment-stop" type="xs:string" />
        <xs:element name="_33_living-dependency" type="xs:string" />
        <xs:element name="_34_ambulatory-status" type="xs:string" />
        <xs:element name="_35_citizenship" type="xs:string" />
        <xs:element name="_36_primary-language" type="xs:string" />
        <xs:element name="_37_living-arrangement" type="xs:string" />
        <xs:element name="_38_publicity-indicator" type="xs:string" />
        <xs:element name="_39_protection-indicator" type="xs:string" />
        <xs:element name="_40_student-identicator" type="xs:string" />
        <xs:element name="_41_religion" type="xs:string" />
        <xs:element name="_42_mothers-maiden-name" type="xs:string" />
        <xs:element name="_43_nationality" type="xs:string" />
        <xs:element name="_44_ethnic-group" type="xs:string" />
        <xs:element name="_45_contact-person-name" type="xs:string" />
        <xs:element name="_46_contact-persons-telephone-number" type="xs:string" />
        <xs:element name="_47_contact-reason" type="xs:string" />
        <xs:element name="_48_contact-relationship" type="xs:string" />
        <xs:element name="_49_job-title" type="xs:string" />
        <xs:element name="_50_job-code-class" type="xs:string" />
        <xs:element name="_51_guarantor-employers-organization-name" type="xs:string" />
        <xs:element name="_52_handicap" type="xs:string" />
        <xs:element name="_53_job-status" type="xs:string" />
        <xs:element name="_54_guarantor-financial-class" type="xs:string" />
        <xs:element name="_55_guarantor-race" type="xs:string" /> 
             */
            #endregion notes
            bool bRetVal = true;
            m_strGT1 = strLine;
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            string[] strMax = strLine.Split(new Char[] { m_cMsgSplitChar });
            // rgc/wdk 20090520 if the GT1 definition is less than the actual number of elements submitted use the 
            int nArraySize = (int)HL7SegmentMax.eGT1;
            // actual number of elements sent to us.
            if (nArraySize < strMax.GetLength(0))
            {
                nArraySize = strMax.GetLength(0);
            }
            try
            {
                string[] segment = new string[nArraySize];
                // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
                for (int i = 0; i < nArraySize; i++)
                {
                    segment[i] = "";
                }
                strMax.CopyTo(segment, 0);

                // rgc/wdk 20090520 segment[3] is the Guarantor's Name
                if (m_staticGT1SetID > 1)
                {
                    return false; // we have more than one GT1 and we only have place to store the first one.
                }
                if (!string.IsNullOrEmpty(segment[3].Trim()))
                {
                    //string[] strName = 
                    m_strGuarName = FormatParsedHL7Name(segment[3]);//.Split(new char[] { '^' }));

                    if (!string.IsNullOrEmpty(m_ERR.propErrMsg))
                    {
                        if (m_ERR.propErrMsg.StartsWith("ERROR NAME:"))
                        {
                            m_ERR.m_Logfile.WriteLogFile(m_ERR.propErrMsg);
                            m_ERR.AddErrorToDataSet("ERROR", "FORMAT", m_ERR.propErrMsg);
                            return false;
                        }
                    }
                    // this means we have name that has at least 3 parts
                    //if (strName.GetUpperBound(0) > 1) // rgc/wdk if the Upper bound is greater than 1 we have at least 3 elements [0][1] and [2]
                    //{
                    //    m_strGuarName = string.Format("{0},{1} {2}", strName[0], strName[1], strName[2]).Trim(); // hl7[3] data is formatted like this ->FAMILY^GIVEN^MI (or MIDDLE NAME)^SUFFIX^PREFIX^DEGREE^NAME TYPE CODE
                    //}
                    //else
                    //{ // we do not have a name with at least 3 parts
                    //    m_strGuarName = string.Format("{0}{1}{2}",
                    //            strName[0], strName[1].Length == 0 ? "" : ",", strName[1].Length == 0 ? "" : strName[1]).Trim();
                    //}
                    m_strGuarSex = segment[9];


                }
                else
                {
                    // rgc/wdk 20090520 is the guarantor's name is empty the business name should be in segment[21] if provided
                    if (m_staticGT1SetID == 1)
                    {
                        if (!string.IsNullOrEmpty(segment[21]))
                        {
                            m_strGuarName = segment[21].Trim();

                        }
                        else
                        {
                            bRetVal = false; // neither the guarantors name nor the guarantor-organization-name has a value so we don't know who the guarantor is
                            m_ERR.AddErrorToDataSet("ERROR", "FORMAT", "IN1[3] and IN1[21] are both empty. NO GUARANTOR");
                        }
                    }
                }
                m_strGuarAddress = segment[5]; // GT1[5] being split in the application for the parts CSZ.
            }
            catch (IndexOutOfRangeException iore)
            {
                // the GT1 is not long enough
                m_ERR.m_Logfile.WriteLogFile(string.Format("GT1 parse of [{0}] failed for [{1}].", strLine, iore.Message));
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("GT1 parse of [{0}] failed for [{1}].", strLine, iore.Message));
                bRetVal = false;
            }
            return (bRetVal);
        }

        /// <summary>
        /// Parse the PID segment loading this class's member variables.
        /// PID patient ID
        /// loads the following member variables from the string passed in
        /// m_strOVPatId,
        /// m_strPatLName,m_strPatFName,m_strPatMidInit
        /// m_strPatDOB, ccyymmdd
        /// m_strPatSex,
        /// m_strOVPatAccount,
        /// m_strPatSSN - no dashes
        ///
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <param name="strLine"> a HL7 PID messsage</param>
        /// <returns>true = success
        ///          false = failed see m_strErrMsg for reason</returns>
        public bool ParsePID(string strLine)
        {
            bool bRetVal = true; //success
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            m_strPID = strLine;
            string[] segment = new string[(int)HL7SegmentMax.ePID];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < (int)HL7SegmentMax.ePID; i++)
            {
                segment[i] = "";
            }
            string[] strMax =
                strLine.Split(new char[] { m_cMsgSplitChar });
            strMax.CopyTo(segment, 0);
            #region notes
            /*
     <xs:element name="_1_set-id-patientid" type="xs:string" />
        <xs:element name="_2_patientid-externalid" type="xs:string" />
        <xs:element name="_3_patientid-internalid" type="xs:string" />
        <xs:element name="_4_alternate-patient-id-pid" type="xs:string" />
        <xs:element name="_5_patient-name" type="xs:string" />
        <xs:element name="_6_mothers-maiden-name" type ="xs:string" />
        <xs:element name="_7_datetime-of-birth" type="xs:date" />
        <xs:element name="_8_sex" type="xs:string" />
        <xs:element name="_9_patient-alias" type="xs:string" />
        <xs:element name="_10_race" type="xs:string" />
        <xs:element name="_11_patient-address" type="xs:string" />
        <xs:element name="_12_county-code" type="xs:string" />
        <xs:element name="_13_phone-number-home" type="xs:string" />
        <xs:element name="_14_phone-number-business" type="xs:string" />
        <xs:element name="_15_primary-language" type="xs:string" />
        <xs:element name="_16_marital-status" type="xs:string" />
        <xs:element name="_17_religion" type="xs:string" />
        <xs:element name="_18_patient-account-number" type="xs:string" />
        <xs:element name="_19_ssn-number-patient" type="xs:string" />
        <xs:element name="_20_drivers-license-number-patient" type="xs:string" />
        <xs:element name="_21_mothers-identifier" type="xs:string" />
        <xs:element name="_22_ethnic-group" type="xs:string" />
        <xs:element name="_23_birth-place" type="xs:string" />
        <xs:element name="_24_multiple-birth-indicator" type="xs:string" />
        <xs:element name="_25_birth-order" type="xs:string" />
        <xs:element name="_26_citizenship" type="xs:string" />
        <xs:element name="_27_veterans-military-status" type="xs:string" />
        <xs:element name="_28_nationality" type="xs:string" />
        <xs:element name="_29_datetime-patient-death" type="xs:string" />
        <xs:element name="_30_patient-death-indicator" type="xs:string" />
            */
            #endregion notes
            /*
            * 0 = PID
            * 1 = Set ID - identifies repetitions 
            * 2 = patient id external - not used in HMS example / for EHS this should be the HNE number per Mike 20090604
            * 3 = patient id internal ??? OV patient ID ??? m_strOVPatId
            * 5 = patient name family name^given name^middle initial or name
                   string m_strPatLName;
                   string m_strPatFName;
                   string m_strPatMidInit;
            * 7 = patient DOB ccyymmdd
            * 8 = patient sex
            * 18 = patient account number ??? OV account number - for charges ???
            * 19 = pat SSN 
            */
            if (m_strMsgType.Substring(0, 3) == "ORM")// only check the first three characters
            {
                // order
                if (segment[2].Equals("NOHNE"))
                {
                    segment[2] = string.Empty;
                }
                m_strHNENumber = segment[2]; // rgc/wdk 20090625 added HNE number
                m_strIndustrialAcct = segment[2]; // rgc/wdk 20090625 added HNE number
                if (!string.IsNullOrEmpty(segment[3]))
                {
                    m_strOVPatId = segment[3];  // ORU segment 3
                }
                else
                {
                    bRetVal = false;
                    propErrMsg = "PID[3] -- Order's OV Patient ID is empty.";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);

                }

                m_strMTMRI = "";
                if (!string.IsNullOrEmpty(segment[18]))
                {
                    m_strOVPatAccount = segment[18]; // order
                }
                else
                {
                    bRetVal = false;
                    propErrMsg = "PID[18] -- Order's OV Patient Account is empty.";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                }
                m_strMTAccount = ""; // not result
            }
            else
            {
                // wdk 20100716 removed the failures from the meditech messages just leave the fields blank.
                // result
                m_strOVPatId = "";
                if (!string.IsNullOrEmpty(segment[3]))
                {
                    m_strMTMRI = segment[3];
                }
                else
                {
                    // wdk 20100716
                    m_strMTMRI = "";
                    //if (!string.IsNullOrEmpty(segment[18]))
                    //{
                    //    if (!segment[18].Equals("CANCELLED"))
                    //    {
                    //        bRetVal = false;
                    //        propErrMsg = "PID[3] -- Result's Meditech Patient MRI is empty.";
                    //    }
                    //}
                }
                // wdk 20101026 added code to capture patient ID that is originally submitted via Screen 2
                // in REQ. This is now being placed into the results message.
                if (!string.IsNullOrEmpty(segment[4]))
                {
                    m_strOVPatId = segment[4];
                }
                else
                {
                    m_strOVPatId = "";
                }
                // end of wdk 20101026 changes
                m_strOVPatAccount = ""; // not order
                if (!string.IsNullOrEmpty(segment[18]))
                {
                    m_strMTAccount = segment[18]; // result
                }
                //else // wdk 20100716 removed
                //{
                //    bRetVal = false;
                //    propErrMsg = "PID[18] -- Results's Meditech Patient Account is empty.";
                //}

            }
            ///////////////////////////////////////////////////
            /* rgc/wdk according to mike all EHS orders will initally be blank or "NOHNE" so don't error for these reasons.
            if (!string.IsNullOrEmpty(segment[2]))
            {
                if (segment[2] == "NOHNE")
                {
                    segment[2] = string.Empty;
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", "PID[2] -- HNE identifier for patient is [NOHNE]");
                }
                m_strIndustrialAcct = segment[2];
                m_strHNENumber = segment[2]; // forward thinking???
            }
            else
            {
                propErrMsg = "PID[2] -- HNE identifier for the patient is empty."; // should be HNE number from hospital
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
            }
            */
            // PID[5] patient name
            if (!string.IsNullOrEmpty(segment[5]))
            {
                // rgc/wdk 20100427 changed to splitting the name
                string[] strNoFail = new string[] { "", "", "", "", "", "", "" }; //rgc/wdk 20100429 initialize the array to empty not null
                if (segment[5].Contains("^"))
                {
                    segment[5].Split(new char[] { '^' }).CopyTo(strNoFail, 0);
                    m_strPatLName = strNoFail[0].ToString().ToUpper();
                    m_strPatFName = strNoFail[1].ToString().ToUpper();
                    m_strPatMidInit = strNoFail[2].ToString().ToUpper();
                }
                //bRetVal = ParseHL7Name(segment[5]);
                bRetVal = true;
            }
            else
            {
                m_strPatLName = "";
                m_strPatFName = "";
                m_strPatMidInit = "";
                //bRetVal = false;
                //propErrMsg = "PID[5] -- Patient's name is empty.";
                //m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);

            }
            // PID[7] Date time of birth
            if (!string.IsNullOrEmpty(segment[7]))
            {
                // wdk 20090323 convert to SQL dob
                m_strPatDOB = ConvertHL7DateToSqlDate(segment[7]);
                //if (string.IsNullOrEmpty(m_strPatDOB))
                //{
                //    bRetVal = false;
                //    propErrMsg = "PID[7] -- Patient's date of birth cannot be converted to a valid date.";
                //    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                //}
            }
            else
            {
                m_strPatDOB = "";
                //bRetVal = false; 
                //propErrMsg = "PID[7] -- Patient's date of birth is empty.";
                //m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
            }

            // PID[8] Pat Sex
            if (!string.IsNullOrEmpty(segment[8]))
            {
                m_strPatSex = segment[8];
                if (m_strPatSex.IndexOfAny(new char[] { 'M', 'F' }) == -1)
                {
                    m_strPatSex = "U";
                    propErrMsg = "PID[8] -- Patient's Gender not equal [F or M]. Processing with Unknown so result ranges will not be calculated properly.";
                }
            }
            else
            {
                m_strPatSex = "U";
                // propErrMsg = "PID[8] -- Patient's Gender is empty. Processing with Unknown so result ranges will not be calculated properly.";
            }

            // split the address out and leave the blanks so the array will not be shortened by empty values.
            if (!string.IsNullOrEmpty(segment[11]))
            {
                string[] strNoFail = new string[] { "", "", "", "", "" };
                if (segment[11].Contains("^"))
                {
                    // wdk 20101206 retrofit to handle no zip code in message
                    //string[] strArrayAddress = segment[11].Split(new string[] { "^" },StringSplitOptions.None);
                    segment[11].Split(new string[] { "^" }, StringSplitOptions.None).CopyTo(strNoFail, 0);
                    m_strPatAddr1 = strNoFail[0];
                    m_strPatAddr2 = strNoFail[1];
                    m_strPatCity = strNoFail[2];
                    m_strPatSt = strNoFail[3];
                    m_strPatZip = strNoFail[4];
                }
                else
                {
                    //bRetVal = false;
                    //propErrMsg = "PID[11] -- Patient's Address contains no subparts.";
                    m_strPatAddr1 = "";
                    m_strPatAddr2 = "";
                    m_strPatCity = "";
                    m_strPatSt = "";
                    m_strPatZip = "";
                }
            }
            else
            {
                m_strPatAddr1 = "";
                m_strPatAddr2 = "";
                m_strPatCity = "";
                m_strPatSt = "";
                m_strPatZip = "";
                //    bRetVal = false;
                //    propErrMsg = "PID[11] -- Patient's Address is empty.";
            }


            if (!string.IsNullOrEmpty(segment[13]))
            {
                m_strPatPhone = segment[13];
            }

            // rgc/wdk 20090610 not used in MCLOE at this time.
            //if (!string.IsNullOrEmpty(segment[16]))
            //{
            //    m_strPatMaritalStatus = segment[16];
            //}
            if (!string.IsNullOrEmpty(segment[19]))
            {
                m_strPatSSN = segment[19];
                m_strPatSSN = segment[19].Replace("-", ""); //- no dashes
            }


            return (bRetVal);
        }

        /// <summary>
        /// PV1 visit
        /// nothing - unless we find we need something from this message
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public bool ParsePV1(string strLine)
        {
            #region notes
            /*
             <xs:element name="PV1">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="_1_set-id" type="xs:string" />
                    <xs:element name="_2_patient-class" type="xs:string" />
                    <xs:element name="_3_assigned-patient-location" type="xs:string" />
                    <xs:element name="_4_admission-type" type="xs:string" />
                    <xs:element name="_5_preadmit-number" type="xs:string" />
                    <xs:element name="_6_prior-patient-location" type="xs:string" />
                    <xs:element name="_7_attending-doctor" type="xs:string" />
                    <xs:element name="_8_referring-doctor" type="xs:string" />
                    <xs:element name="_9_consulting-doctor" type="xs:string" />
                    <xs:element name="_10_hospital-service" type="xs:string" />
                    <xs:element name="_11_temporary-location" type="xs:string" />
                    <xs:element name="_12_preadmit-test-indicator" type="xs:string" />
                    <xs:element name="_13_readmission-indicator" type="xs:string" />
                    <xs:element name="_14_admit-source" type="xs:string" />
                    <xs:element name="_15_ambulatory-status" type="xs:string" />
                    <xs:element name="_16_vip-indicator" type="xs:string" />
                    <xs:element name="_17_admitting-doctor" type="xs:string" />
                    <xs:element name="_18_patient-type" type="xs:string" />
                    <xs:element name="_19_visit-number" type="xs:string" />
                    <xs:element name="_20_financial-class" type="xs:string" />
                    <xs:element name="_21_charge-price-indicator" type="xs:string" />
                    <xs:element name="_22_curtesy-code" type="xs:string" />
                    <xs:element name="_23_credit-rating" type="xs:string" />
                    <xs:element name="_24_contract-code" type="xs:string" />
                    <xs:element name="_25_contract-effective-date" type="xs:string" />
                    <xs:element name="_26_contract-amount" type="xs:string" />
                    <xs:element name="_27_contract-period" type="xs:string" />
                    <xs:element name="_28_interest-code" type="xs:string" />
                    <xs:element name="_29_transfer-to-bad-debt-code" type="xs:string" />
                    <xs:element name="_30_transfer-to-bad-debt-date" type="xs:string" />
                    <xs:element name="_31_bad-debt-agency-code" type="xs:string" />
                    <xs:element name="_32_bad-debt-transfer-amount" type="xs:string" />
                    <xs:element name="_33_bad-debt-recovery-amount" type="xs:string" />
                    <xs:element name="_34_delete-account-indicator" type="xs:string" />
                    <xs:element name="_35_delete-account-date" type="xs:string" />
                    <xs:element name="_36_discharge-disposition" type="xs:string" />
                    <xs:element name="_37_discharged-to-location" type="xs:string" />
                    <xs:element name="_38_diet-type" type="xs:string" />
                    <xs:element name="_39_servicing-facility" type="xs:string" />
                    <xs:element name="_40_bed-status" type="xs:string" />
                    <xs:element name="_41_account-status" type="xs:string" />
                    <xs:element name="_42_pending-location" type="xs:string" />
                    <xs:element name="_43_prior-tempory-location" type="xs:string" />
                    <xs:element name="_44_admit-datetime" type="xs:string" />
                    <xs:element name="_45_discharge-datetime" type="xs:string" />
                    <xs:element name="_46_current-patient-balance" type="xs:string" />
                    <xs:element name="_47_total-charges" type="xs:string" />
                    <xs:element name="_48_total-adjustments" type="xs:string" />
                    <xs:element name="_48_total-payments" type="xs:string" />
                    <xs:element name="_50_alternate-visit-id" type="xs:string" />
                    <xs:element name="_51_visit-indicator" type="xs:string" />
                    <xs:element name="_52_other-healthcare-provider" type="xs:string" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
      */
            #endregion notes
            bool bRetVal = true;
            bool bResult = m_strMsgType.Contains("ORU");// wdk 20100716 added 

            m_strPV1 = strLine;
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            string[] segment = new string[(int)HL7SegmentMax.ePV1];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < (int)HL7SegmentMax.ePV1; i++)
            {
                segment[i] = "";
            }
            string[] strMax =
                strLine.Split(new Char[] { m_cMsgSplitChar });
            strMax.CopyTo(segment, 0);
            string strErr = string.Empty;
            if (bResult)
            {
                try
                {
                    if (!string.IsNullOrEmpty(segment[7]))
                    {
                        m_strCliMnem = segment[7];
                        if (segment[7].Contains("^"))
                        {
                            m_strCliMnem = segment[7].Substring(1, segment[7].IndexOf("^") - 1);
                        }

                        if (m_strCliMnem[0].CompareTo('z') == 0)
                        {
                            m_strCliMnem = m_strCliMnem.Remove(0, 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    m_ERR.m_Logfile.WriteLogFile(ex.Message);
                    return false;
                }
                return !string.IsNullOrEmpty(m_strCliMnem);
            }
            try
            {
                // Physician info
                if (!string.IsNullOrEmpty(segment[7]))
                {
                    if (segment[7].Contains("^"))
                    {
                        string[] strParts = segment[7].Split(new char[] { '^' });
                        if (!string.IsNullOrEmpty(strParts[0]))
                        {

                            m_strPhyUpin = strParts[0];
                            DBAccess dPhy = new DBAccess("MCLOE", m_ERR.propIsLive ? "GOMCLLIVE" : "GOMCLTEST", "wphy");
                            if (string.IsNullOrEmpty(dPhy.GetField("wphy", "tnh_num",
                                    string.Format("deleted = 0 AND tnh_num = '{0}'", m_strPhyUpin),
                                        out strErr)))
                            {
                                bRetVal = false;

                                strErr = string.Format("PV1[7][0] -- NPI not in wphy table lookup returned [{0}] error.", strErr);
                                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", strErr);
                            }
                            DBAccess dPhySanc = new DBAccess("MCLOE", m_ERR.propIsLive ? "GOMCLLIVE" : "GOMCLTEST", "wphy_sanc");
                            string strReinDate = null;
                            strReinDate = dPhySanc.GetField("wphy_sanc", "reindate",
                                string.Format("lastname = '{0}' and firstname = '{1}' and (midname like '{2}%' or midname is null)",
                                    strParts[1], strParts[2], strParts[3]), out strErr);
                            if (!string.IsNullOrEmpty(strReinDate)) // the phy is in the sanctioned phy table.
                            {
                                if (strReinDate == "00000000")
                                {
                                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("PHY is on the sanctioned phy list and not reinstated."));
                                }
                                else
                                {
                                    DateTime dtReinDate = DateTime.MinValue;
                                    if (DateTime.TryParse(strReinDate, out dtReinDate))
                                    {
                                        if (dtReinDate > DateTime.Today)
                                        {
                                            m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("PHY is on the sanctioned phy list and not reinstated."));
                                        }
                                    }
                                    else
                                    {
                                        m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("PHY is on the sanctioned phy list and reinstated date is not valid."));
                                    }
                                }
                            }

                        }
                        else
                        {
                            bRetVal = false;
                            strErr = "PV1[7][0] -- Attending doctors NPI is empty.";
                            m_ERR.AddErrorToDataSet("ERROR", "FORMAT", strErr);

                        }
                    }
                    else
                    {
                        bRetVal = false;
                        strErr = "PV1[7] -- Attending doctor is not formatted properly. Cannot determine NPI.";
                        m_ERR.AddErrorToDataSet("ERROR", "FORMAT", strErr);
                    }
                }
                else
                {
                    bRetVal = false;
                    strErr = "PV1[7] -- Attending doctor is empty.";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", strErr);
                }
                // rgc/wdk 20100420 use the ORC's segment[9] for the collection date time. this is 
                // the patients present at the office time???
                //if (!string.IsNullOrEmpty(segment[44]))
                //{
                //    m_strCollDateTime = segment[44];
                //    GetAmaYear();
                //}
                //else
                //{
                //    bRetVal = false;
                //    strErr = "PV1[44] -- Collection date time is empty.";
                //    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", strErr);
                //}
                if (!string.IsNullOrEmpty(segment[39]))
                {
                    DBAccess dPerformSite = new DBAccess("MCLOE", m_ERR.propIsLive ? "GOMCLLIVE" : "GOMCLTEST", "wperformsite");
                    m_strPerformingSite = dPerformSite.GetField("wperformsite",
                        "site_code",
                            string.Format("cli_mnem = '{0}'", segment[39]),
                                out strErr);
                }
            }
            catch (IndexOutOfRangeException)
            {
                bRetVal = false;
                strErr = string.Format("PV1 contains {0} segments. We expect Collection date and time from segment 44.", strMax.Length);
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", strErr);
            }
            return (bRetVal);
        }

        private void GetAmaYear()
        {
            int nYear = int.Parse(m_strCollDateTime.Substring(0, 4));
            int nMo = int.Parse(m_strCollDateTime.Substring(4, 2));
            if (nMo >= 10)
            {
                nYear++;
            }
            m_strAMAYear = nYear.ToString();
        }

        /// <summary>
        /// ORC Orders
        /// gets phy upin
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public bool ParseORC(string strLine)
        {
            #region notes
            /*
              <xs:element name="ORC">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="_1_order-control" type="xs:string" />
                                            NW = new order
                                            CA = cancel order request      
                    <xs:element name="_2_placer-order-number" type="xs:string" />
                    <xs:element name="_3_filler-order-number" type="xs:string" />
                    <xs:element name="_4_placer-group-number" type="xs:string" />
                    <xs:element name="_5_order-status" type="xs:string" />
                    <xs:element name="_6_response-flag" type="xs:string" />
                    <xs:element name="_7_quantity-timing" type="xs:string" />
                    <xs:element name="_8_parent" type="xs:string" />
                    <xs:element name="_9_datetime-of-transaction" type="xs:string" />
                    <xs:element name="_10_entered-by" type="xs:string" />
                    <xs:element name="_11_verified-by" type="xs:string" />
                    <xs:element name="_12_ordering-provider" type="xs:string" />
                    <xs:element name="_13_enterers-location" type="xs:string" />
                    <xs:element name="_14_call-back-phone-number" type="xs:string" />
                    <xs:element name="_15_order-effective-datetime" type="xs:string" />
                    <xs:element name="_16_order-control-code-reason" type="xs:string" />
                    <xs:element name="_17_entering-organization" type="xs:string" />
                    <xs:element name="_18_entering-device" type="xs:string" />
                    <xs:element name="_19_action-by" type="xs:string" />
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
      */
            #endregion notes
            bool bRetVal = true;
            m_strORC = strLine;
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            // EHS orders don't seem to have a carrage return behind the ORC 
            string[] strOBRORC = strLine.Split(new string[] { "OBR|" }, StringSplitOptions.RemoveEmptyEntries);
            if (strOBRORC.GetUpperBound(0) == 1)
            {
                strLine = strOBRORC[0];
            }
            string[] segment = new string[(int)HL7SegmentMax.eORC];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < (int)HL7SegmentMax.eORC; i++)
            {
                segment[i] = "";
            }
            string[] strMax =
                strLine.Split(new Char[] { m_cMsgSplitChar });
            strMax.CopyTo(segment, 0);
            //= strLine.Split(new Char[] { '|' });
            //ORC
            try
            {
                if (m_strMsgType.Substring(0, 3) == "ORM") // only check the first three characters
                {
                    if (!string.IsNullOrEmpty(segment[2]))
                    {
                        m_strOVOrderId = segment[2]; // rgc/wdk 20090312 added (Placers order number)
                        //string[] segPart = segment[12].Split(new char[] { '^' });
                        //m_strCliMnem = segPart[0];  // wdk 20090316 this is wrong wrong wrong but that's where its at. After discussing with Mike Garner and Chris we are leaveing it here for compatability with EHS feed into meditech that was developed when a programmer not to be named worked here. 
                        //if (m_strCliMnem.StartsWith("z"))
                        //{
                        //    m_strCliMnem = m_strCliMnem.Remove(0, 1);
                        //}
                    }
                    else
                    {
                        m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("ORC|{0} Segment[2] is empty, No Order Number", segment[1]));
                        bRetVal = false;
                    }
                    // rgc/wdk 20090416 we don't know or have the order priority so set it to R - Routine.
                    // rgc/wdk 20091015 now set a "FORMAT" error and reject the message.
                    if (!string.IsNullOrEmpty(segment[7]))
                    {
                        m_strOrderPriority = segment[7][segment[7].Length - 1].ToString(); // rgc/wdk 20090312 now placed in ORC which is the common element for all OBR's
                    }
                    else
                    {
                        // m_strOrderPriority = "R";
                        m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("ORC|{0} Segment[7] Priority is empty.", segment[7]));
                        bRetVal = false;
                    }
                    // rgc/wdk 20100420 use the ORC date and time for the collection date and time
                    if (!string.IsNullOrEmpty(segment[9]))
                    {
                        m_strCollDateTime = segment[9];
                        GetAmaYear();
                    }
                    else
                    {
                        m_ERR.AddErrorToDataSet("ERROR", "FORMAT", "ORC Segment[9] Collection date/time is empty.");
                        bRetVal = false;
                    }
                    if (!string.IsNullOrEmpty(segment[12]))
                    {
                        string[] segPart = segment[12].Split(new char[] { '^' });
                        m_strCliMnem = segPart[0];  // wdk 20090316 this is wrong wrong wrong but that's where its at. After discussing with Mike Garner and Chris we are leaveing it here for compatability with EHS feed into meditech that was developed when a programmer not to be named worked here. 
                        if (string.IsNullOrEmpty(m_strCliMnem))
                        {
                            m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("ORC|{0} Segment[12][0] is empty, No Client", segment[12]));
                            bRetVal = false;
                        }
                        else
                        {
                            m_strOrdering_provider = segment[12];
                            if (!string.IsNullOrEmpty(m_strOrdering_provider))
                            {
                                if (m_strCliMnem.StartsWith("z"))
                                {
                                    m_strCliMnem = m_strCliMnem.Remove(0, 1);
                                }
                                string strErr = string.Empty;
                                if (string.IsNullOrEmpty(m_strPerformingSite))
                                {
                                    DBAccess dPerformSite = new DBAccess("MCLOE", m_ERR.propIsLive ? "GOMCLLIVE" : "GOMCLTEST", "wperformsite");
                                    m_strPerformingSite = dPerformSite.GetField("wperformsite",
                                        "site_code",
                                            string.Format("cli_mnem = '{0}'", m_strCliMnem),
                                                out strErr);

                                }
                            }
                        }
                    }
                    else
                    {
                        //m_ERR.AddErrorToDataSet(string.Format("ERROR^{0}", Application.ProductName), string.Format("ORC|{0} Segment[12][0] is empty, No Client", segment[1]));
                        m_ERR.AddErrorToDataSet("ERROR", "FORMAT", string.Format("ORC|{0} Segment[12][0] is empty, No Client", segment[1]));
                        bRetVal = false;
                    }

                }
                else
                {
                    // RESULT
                    m_strPlacersGroupNumber = segment[4];

                    if (!string.IsNullOrEmpty(m_strCliMnem))
                    {
                        string strErr = string.Empty;
                        DBAccess dClient = new DBAccess("MCLOE", m_ERR.propIsLive ? "GOMCLLIVE" : "GOMCLTEST", "dict_cli_results_to_his");

                        m_strFinalsOnly = dClient.GetField("dict_cli_results_to_his",
                                "finals_only",
                                    string.Format("cli_mnem = '{0}'", m_strCliMnem),
                                    out strErr);
                        if (string.IsNullOrEmpty(m_strFinalsOnly))
                        {
                            m_ERR.m_Logfile.WriteLogFile("NULL in dict_cli_results_to_his for client");
                            m_ERR.m_Logfile.WriteLogFile(strErr);
                            m_strFinalsOnly = "False";
                        }
                    }
                }


            }
            catch (IndexOutOfRangeException)
            {
                propErrMsg = string.Format("ORC contains {0} segments. We expect Ordering provider in segment 12.", strMax.Length);
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                bRetVal = false;

            }
            return (bRetVal);
        }

        /// <summary>
        /// OBR Test ID
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public bool ParseOBR(string strLine)
        {
            #region notes
            /*
             *   <xs:element name="OBR">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="_1_set-id" type="xs:string" />
        <xs:element name="_2_placer-order-number" type="xs:string" />
        <xs:element name="_3_filler-order-number" type="xs:string" />
        <xs:element name="_4_universal-service-id" type="xs:string" />
        <xs:element name="_5_priority" type="xs:string" />
        <xs:element name="_6_requested-datetime" type="xs:string" />
        <xs:element name="_7_observation-start-datetime" type="xs:string" />
        <xs:element name="_8_observation-end-datetime" type="xs:string" />
        <xs:element name="_9_collection-volumne" type="xs:string" />
        <xs:element name="_10_collector-identifier" type="xs:string" />
        <xs:element name="_11_specimen-action-code" type="xs:string" />
        <xs:element name="_12_danger-code" type="xs:string" />
        <xs:element name="_13_relevant-clinical-info" type="xs:string" />
        <xs:element name="_14_specimen-received-datetime" type="xs:string" />
        <xs:element name="_15_specimen-source" type="xs:string" />
        <xs:element name="_16_ordering-provider" type="xs:string" />
        <xs:element name="_17_order-callback-phone-number" type="xs:string" />
        <xs:element name="_18_placer-field-1" type="xs:string" />
        <xs:element name="_19_placer-field-2" type="xs:string" />
        <xs:element name="_20_filler-field-1" type="xs:string" />
        <xs:element name="_21_filler-field-2" type="xs:string" />
        <xs:element name="_22_results-rpt-status-chng-datetime" type="xs:string" />
        <xs:element name="_23_change-to-practice" type="xs:string" />
        <xs:element name="_24_diagnostic-serv-sect-id" type="xs:string" />
        <xs:element name="_25_result-status" type="xs:string" />
        <xs:element name="_26_parent-result" type="xs:string" />
        <xs:element name="_27_quality-timing" type="xs:string" />
        <xs:element name="_28_result-copies-to" type="xs:string" />
        <xs:element name="_29_parent" type="xs:string" />
        <xs:element name="_30_transportation-mode" type="xs:string" />
        <xs:element name="_31_reason-for-study" type="xs:string" />
        <xs:element name="_32_principal-result-interpreter" type="xs:string" />
        <xs:element name="_33_assistant-result-interpreter" type="xs:string" />
        <xs:element name="_34_technician" type="xs:string" />
        <xs:element name="_35_transcriptionist" type="xs:string" />
        <xs:element name="_36_scheduled-datetime" type="xs:string" />
        <xs:element name="_37_number-of-sample-containers" type="xs:string" />
        <xs:element name="_38_transport-logistics-of-collected-sample" type="xs:string" />
        <xs:element name="_39_collectors-comments" type="xs:string" />
        <xs:element name="_40_transport-arrangement-responsibility" type="xs:string" />
        <xs:element name="_41_transport-arranged" type="xs:string" />
        <xs:element name="_42_escort-required" type="xs:string" />
        <xs:element name="_43_planned-patient-transport-comment" type="xs:string" />
      </xs:sequence>
    </xs:complexType>
  </xs:element>
             */
            #endregion notes
            bool bRetVal = false;
            //throw new System.NotImplementedException();
            /*
             * /*
             * 2 = OV order ID
             * 3 = Our order ID
             * 4 = universal service id (Meditech mnemonic ???) ^description
             * 5 = priority (R/S/A) routine/stat/asap
             * 7 = order time (collection time)
             * ??7 = result time
             * 13 = specimen received date time
             * 15 = Micro Source/specimen source
             * 18 = placers field#1 ??? OV specimen ID
             * 22 = Result rpt/status change - date time // wdk/rgc 03/28/2007
             * 25 = results status
             *          O = order received
             *          A = some results
             *          P = preliminary
             *          C = corrected
             *          F = final
            */
            m_strOBR = strLine;
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            int nArrSize = (int)HL7SegmentMax.eOBR;
            string[] strMax = strLine.Split(new Char[] { m_cMsgSplitChar });

            if ((int)HL7SegmentMax.eOBR < strMax.GetLength(0))
            {
                nArrSize = strMax.GetLength(0);
            }
            string[] segment = new string[nArrSize];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < nArrSize; i++)
            {
                segment[i] = "";
            }
            strMax.CopyTo(segment, 0);
            try
            {
                // OBR 1 SetID
                if (string.IsNullOrEmpty(segment[1].Trim()))
                {
                    segment[1] = m_staticOBRSetID.ToString();
                    propErrMsg = "OBR[1] SetId was empty. Set value to current set id.";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                    bRetVal = false;
                }
                m_llOrders.AddLast(m_staticOBRSetID.ToString()); // 1 OBR setid
                m_strOVSpecimenId = string.Format("{0}-{1}", m_strOVOrderId, m_staticOBRSetID);

                m_staticOBRSetID++;
                string strTestMnem = string.Empty;
                // OBR 4 Universal Service ID ie orderable test
                if (!string.IsNullOrEmpty(segment[4]))
                {
                    if (segment[4].Contains("^"))
                    {
                        string[] field = segment[4].Split(new char[] { '^' });
                        if (!string.IsNullOrEmpty(field[0]))
                        {
                            bRetVal = true;
                            m_llOrders.AddLast(field[0]); //2 test mnen
                            m_alTestsOrdered.Add(field[0]); // wdk 20101001 added to change results without specimen number.
                            strTestMnem = field[0];
                            try
                            {
                                m_dicOrderedTestMnems.Add(field[0], field[1]);
                                // wdk 20091027 added
                                if (m_dsOBR != null)
                                {
                                    if (m_dsOBR.Tables["TESTS"].Rows.Find(field[0]) == null)
                                    {
                                        m_dsOBR.Tables["TESTS"].Rows.Add(new string[]
                                        {
                                            field[0], // test_mnem
                                            "", // cdm
                                            field[1],  // test desc
                                           "", // micro source
                                           "", // ov_specimen_id
                                           "N" // abn
                                        }
                                        );
                                    }
                                    else
                                    {
                                        // report
                                    }
                                }

                            }
                            catch (ArgumentException)
                            {
                                propErrMsg = string.Format("Order contains duplicated test -- {0}", field[0]);
                                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                                bRetVal = false;
                                // trying to add a duplicate key so just keep on trucking.
                            }
                            m_llOrders.AddLast(field[1]); // 3 description
                        }
                        #region ORM check
                        if (m_strMsgType.Substring(0, 3) == "ORM") // only check the first three characters
                        {
                            //ORM is an order
                            m_strOurOrderId = "";
                            m_strResultDateTime = ""; // not resulted (yet)ccyymmddhhmm 
                            m_strResultsStatus = ""; // not a result
                            // need to look the test up and see if it is a MICRO if so this cannot be blank
                            // and the source must be valid for this test.
                            //if (!string.IsNullOrEmpty(segment[15]))
                            //{
                            m_llOrders.AddLast(segment[15]); //4 microsource



                            m_llOrders.AddLast(m_strOVSpecimenId); //5  OV specimen id  
                            if (m_dsOBR != null)
                            {
                                m_dsOBR.Tables["TESTS"].Rows.Find(strTestMnem)["MICRO_SOURCE"] = segment[15];
                                m_dsOBR.Tables["TESTS"].Rows.Find(strTestMnem)["OV_SPECIMEN_ID"] = m_strOVSpecimenId;
                            }
                        }
                        else
                        {
                            // ORU is a result rgc/wdk the results will need to handle multiple orders 
                            m_strTestDesc = field[1];  // also used in OBX = field[1];  // also used in OBX
                            m_strOurOrderId = segment[3]; // our specimen
                            m_strOrderPriority = "";
                            m_strCollDateTime = segment[7]; //ccyymmddhhmm // rgc/wdk 20090611 this works??? might should convert to mm/dd/ccyy format???
                            GetAmaYear();
                            m_strMeditechReceivedDateTime = segment[14]; //ccyymmddhhmm

                            m_strMicroSource = segment[15];
                            if (string.IsNullOrEmpty(m_strCliMnem)) // rgc/wdk 20101130 added if before adding obr[16] as the client mnemonic.
                            {
                                string[] field1;
                                field1 = segment[16].Split(new char[] { '^' });
                                if (field1.GetUpperBound(0) > -1)
                                {
                                    if (!string.IsNullOrEmpty(field1[0]))
                                    {
                                        m_strCliMnem = field1[0];
                                        if (field1[0].Substring(0, 1) == "z")
                                        {
                                            m_strCliMnem = field1[0].Remove(0, 1);
                                        }
                                    }
                                }
                                else
                                {
                                    m_strErrMsg = string.Format("Can NOT parse OBR. segment[16] = {0}", segment[16]);
                                    return false;
                                }
                            }
                            // wdk/rgc 03/28/2007 if 22 is blank use current date time.
                            if (segment.GetUpperBound(0) > 21)
                            {
                                m_strResultDateTime = string.IsNullOrEmpty(segment[22]) ? Time.HL7TimeStampNoSeconds() : segment[22]; // ccyymmddhhmm
                            }
                            else
                            {
                                m_strResultDateTime = Time.HL7TimeStampNoSeconds();
                            }
                            if (segment.GetUpperBound(0) > 24)
                            {
                                m_strResultsStatus = segment[25];
                            }
                            // meditech does not have these OV values
                            m_strOVOrderId = ""; // rgc/wdk 20090312 look up from origional wreq to tie back to the order.

                        }
                        #endregion ORM check
                    }
                    else
                    {
                        propErrMsg = "OBR[4] Universal Service ID is null or empty";
                        m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                        bRetVal = false;

                    }
                }
                else
                {
                    propErrMsg = "OBR[4] Universal Service ID is empty";
                    m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
                }


                m_llOrders.AddLast(new LinkedListNode<string>("F")); // abn 6
            }
            catch (IndexOutOfRangeException)
            {
                propErrMsg = string.Format("OBR only contains {0} segments. We expect ordering provider in 16.", nArrSize.ToString());
                m_ERR.AddErrorToDataSet("ERROR", "FORMAT", propErrMsg);
            }
            ///////////////////////




            return (bRetVal);
        }

        /// <summary>
        /// NTE - comment
        /// * HMS sends us a non standard NTE by adding additional fields, we don't process the non standard fields as of 12/26/2006.
        /// If NTE sequence number is greater than 1 we add to m_strComment. 
        /// 08/29/2006 Rick Crone
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public bool ParseNTE(string strLine)
        {
            bool bRetVal = false;
            /*
            * HMS sends us a non standard NTE by adding additional fields 
            * 2.24.15.2 Source of comment (ID) 00097
           Definition: This field is used when source of comment must be identified. This table may be extended locally during implementation. 
           Table 0105 - Source of comment 

           Value              Description
             L              Ancillary (filler) department is source of comment
             P              Orderer (placer) is source of comment
             O              Other system is source of comment          
           */
            m_strNTE = strLine;

            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            // rgc/wdk 20090520 modified the array creation to the larger of the nte documentation and the actual array sent to us.
            string[] strMax =
                strLine.Split(new Char[] { m_cMsgSplitChar });
            // rgc/wdk 20090520 if the NTE definition is less than the actual number of elements submitted use the 
            int nArraySize = (int)HL7SegmentMax.eNTE;
            // actual number of elements sent to us.
            if (nArraySize < strMax.GetLength(0))
            {
                nArraySize = strMax.GetLength(0);
            }
            string[] segment = new string[nArraySize]; //new string[(int)HL7SegmentMax.eNTE];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < nArraySize; i++)
            {
                segment[i] = "";
            }
            strMax.CopyTo(segment, 0);

            //  if the previous segment was either an obr or orc keep adding this to the comment.

            m_strSourceOfComment = segment[2];
            if (m_strMsgType.Substring(0, 3) == "ORM") // orders
            {
                // wdk 20091026 added || to the below if.
                if (segment[3].ToLower() == "abn form signed and on file." || segment[3].ToLower() == "abn signed")
                {
                    m_llOrders.Last.Value = "T";
                    if (m_dsOBR != null)
                    {
                        m_dsOBR.Tables["TESTS"].Rows[m_dsOBR.Tables["TESTS"].Rows.Count - 1]["ABN"] = "T";
                    }
                }
                if (!string.IsNullOrEmpty(segment[1]))
                {
                    if (int.TryParse(segment[1], out m_staticNTESetID)) // handle multiple NTE's in the same order
                    {
                        m_strComment += " ";
                        m_strComment += segment[3];
                    }
                    else
                    {
                        m_strComment = segment[3];
                    }
                    bRetVal = true;
                }
            }
            else
            {
                // results 

                m_strComment = segment[3];
                bRetVal = true;
            }


            return (bRetVal);
        }
        /// <summary>
        /// OBX Micro
        /// 11/14/2006 Rick Crone/david
        /// </summary>
        /// <param name="strLine"></param>
        /// <returns></returns>
        public bool ParseOBX(string strLine)
        {
            //throw new System.NotImplementedException();

            bool bRetVal = false;
            m_strOBX = strLine;
            // rgc/wdk 20090507 replaced the array creation with the size of the segment from the HL7 documentation.
            string[] segment = new string[(int)HL7SegmentMax.eOBX];
            // rgc/wdk 20100429 retrofit to ensure that each element is "" not null
            for (int i = 0; i < (int)HL7SegmentMax.eOBX; i++)
            {
                segment[i] = "";
            }
            string[] strMax =
                strLine.Split(new Char[] { m_cMsgSplitChar });
            strMax.CopyTo(segment, 0);
            //= strLine.Split(new Char[] { '|' });
            //OBX // test data OBX|1|ST|||SPE|||||||||||||

            if (segment[0] == "OBX")
            {
                bRetVal = true;
                string[] field = segment[3].Split(new char[] { '^' });
                m_strResultedTestMnem = field[0];
            }
            else
            {
                m_strErrMsg = string.Format("Can NOT parse OBX. segment[5] = {0}", segment[5]);
            }
            return (bRetVal);
        }

        /// <summary>
        /// Date/Time from HL7 Specifications
        ///  TYPE     LEN   DATA NAME   Reference   Notes/Format
        ///  DT        8       Date        2.8.13      YYYY[MM[DD]] 
        ///  TM       18       Time        2.8.39      HH[MM[SS[.S[S[S[S]]]]]][+/-ZZZZ] (not used in our HL7 as of this date)
        ///  TS       26        Time stamp  2.8.42      YYYY[MM[DD[HHMM[SS[.S[S[S[S]]]]]]]][+/-ZZZZ] ^ [degree of precision]
        /// wdk 20090320 
        /// Returns string representation in Sql Date acceptable format
        /// 
        /// for TS date >= 14 returns MM/DD/CCYY HHMM SS
        /// for TS date between 9 and 13 returns MM/DD/CCYY
        /// for TS date or DT date equal 8 returns MM/DD/CCYY
        /// 
        /// Any date that is less than 8 blank is returned.
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertHL7DateToSqlDate(string str)
        {
            /*
             Date/Time from HL7 Specifications
             TYPE        DATA NAME   Reference   Notes/Format
             DT          Date        2.8.13      YYYY[MM[DD]] 
             TM          Time        2.8.39      HH[MM[SS[.S[S[S[S]]]]]][+/-ZZZZ]
             TS          Time stamp  2.8.42      YYYY[MM[DD[HHMM[SS[.S[S[S[S]]]]]]]][+/-ZZZZ] ^ <degree of precision>
             */

            string strRetVal = string.Empty;

            switch (str.Length)
            {
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                    {
                        strRetVal = string.Format("{0}/{1}/{2}",
                            str.Substring(4, 2), str.Substring(6, 2), str.Substring(0, 4)); // MM/DD/CCYY for 8

                        break;
                    }

                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:  // hl7 timestamp
                    {
                        strRetVal = string.Format("{0}/{1}/{2} {3}{4} {5}",
                            str.Substring(4, 2), str.Substring(6, 2), str.Substring(0, 4), // MM/DD/CCYY for 8
                            str.Substring(8, 2), str.Substring(10, 2), str.Substring(12, 2) // HHMM SS 6
                            );
                        break;
                    }
                default:
                    {
                        strRetVal = string.Empty;
                        break;
                    }
            }
            return strRetVal;

        }



        /// <summary>
        /// not finished
        /// </summary>
        /// <returns></returns>
        public bool CreateResultsHtmlDoc()
        {
            //            StringBuilder output = new StringBuilder();
            //            string xmlString = @"<?xml version='1.0'?>
            //                                <!-- This is a sample XML document -->
            //                                <Items>
            //                                <Item>test with a child element <more/> stuff</Item>
            //                                </Items>";
            return false;
        }
        /// <summary>
        /// Updates the ORC[2] with the Other vendors order id. 
        /// </summary>
        /// <param name="strOVOrderId"></param>
        /// <returns></returns>
        public string UpdateORCOrderNumber(string strOVOrderId)
        {
            string strRetVal = string.Empty;
            string[] strORCParts = m_strORC.Split(new char[] { '|' });
            strORCParts[2] = strOVOrderId;
            for (int i = 0; i <= strORCParts.GetUpperBound(0); i++)
            {
                strRetVal += string.Format("{0}|", strORCParts[i]);
            }
            for (int i = 0; i <= m_arrMessageSegments.GetUpperBound(0); i++)
            {
                if (string.IsNullOrEmpty(m_arrMessageSegments[i].ToString()) || m_arrMessageSegments[i].ToString().Length < 4)
                {
                    continue;
                }
                if (m_arrMessageSegments[i].ToString().Substring(0, 4) != "ORC|")
                {
                    continue;
                }
                m_arrMessageSegments[i] = strRetVal;


            }

            return strRetVal;
        }
        /// <summary>
        /// Updates the PID[2] with the Other vendors Pat id, and PID[4] with the Hospitals HNE number.
        /// 
        /// </summary>
        /// <returns>Updated PID segment</returns>
        /// <param name="strOVPatId">Pat id from order</param>
        /// <param name="strHNENumber">HNE Number from the Wpat record</param>
        public string UpdatePIDPatId(string strOVPatId, string strHNENumber)
        {
            string strRetVal = string.Empty;
            string[] strPIDParts = m_strPID.Split(new char[] { '|' });
            if (!string.IsNullOrEmpty(strHNENumber) && strHNENumber != strPIDParts[2].ToString())
            {
                m_ERR.m_Email.Send("HL7@UpdatePID.SRV", "david.kelly@wth.org", "HNE Numbers don't match",
                    string.Format("{PatRecord {0}. ResultMessage {1}", strHNENumber, strPIDParts[2].ToString()));

            }
            strHNENumber = strPIDParts[2].ToString();
            strPIDParts[2] = strOVPatId;
            strPIDParts[4] = strHNENumber;
            for (int i = 0; i <= strPIDParts.GetUpperBound(0); i++)
            {
                strRetVal += string.Format("{0}|", strPIDParts[i]);
            }
            for (int i = 0; i <= m_arrMessageSegments.GetUpperBound(0); i++)
            {
                if (string.IsNullOrEmpty(m_arrMessageSegments[i].ToString()) || m_arrMessageSegments[i].ToString().Length < 4)
                {
                    continue;
                }
                if (m_arrMessageSegments[i].ToString().Substring(0, 4) != "PID|")
                {
                    continue;
                }
                m_arrMessageSegments[i] = strRetVal;
                break;

            }
            return strRetVal;
        }



        /// <summary>
        /// Intended for use with Results. Update the Pid and ORC using UpdatePIDPatId(string strPatid) and
        /// UpdateORCOrderNumber(string strOrderNumber) before calling this function
        ///
        /// Takes the instances array of the Result and recreates the parts back into a sendable HL7 message
        /// string with vertical tabs and file seperators as required.
        /// </summary>
        /// <returns>Updated Result Message with Other Vendor Pat and Order info</returns>       
        public string CreateUpdatedResultMsg()
        {
            // [Obsolete("Should NOT be used. Use the update for the Service calling this function.")]
            if (!m_ERR.propIsLive)
            {
                System.Diagnostics.Debugger.Launch();
            }
            string strRetVal = string.Empty;
            // if (!m_strHL7Msg.Contains(HL7.VT.ToString()))
            // {
            //     strRetVal = HL7.VT.ToString();
            // }
            //strRetVal += m_strHL7Msg;
            //for (int i = 0; i <= m_arrMessageSegments.GetUpperBound(0); i++)
            //{
            //    strRetVal += string.Format("{0}{1}", m_arrMessageSegments[i], HL7.CR);
            //}
            if (!m_strMSH.Contains(HL7.VT.ToString()))
            {
                m_strMSH = m_strMSH.Insert(0, HL7.VT.ToString());
            }
            if (!m_strMSH.Contains(HL7.CR.ToString()))
            {
                strRetVal += string.Format("{0}{1}", m_strMSH, HL7.CR);
            }
            else
            {
                strRetVal += m_strMSH;
            }

            if (!m_strPID.Contains(HL7.CR.ToString()))
            {
                strRetVal += string.Format("{0}{1}", m_strPID, HL7.CR);
            }
            else
            {
                strRetVal += m_strPID;
            }
            if (!m_strORC.Contains(HL7.CR.ToString()))
            {
                strRetVal += string.Format("{0}{1}", m_strORC, HL7.CR);
            }
            else
            {
                strRetVal += m_strORC;
            }
            if (!m_strOBR.Contains(HL7.CR.ToString()))
            {
                strRetVal += string.Format("{0}{1}", m_strOBR, HL7.CR);
            }
            else
            {
                strRetVal += m_strOBR;
            }
            if (!m_strNTE.Contains(HL7.CR.ToString()))
            {
                strRetVal += string.Format("{0}{1}", m_strNTE, HL7.CR);
            }
            else
            {
                strRetVal += m_strNTE;
            }

            strRetVal += string.Format("{0}{1}", HL7.FS, HL7.CR);

            return strRetVal;
        }

        /// <summary>
        /// Intended for use with Results. Upate the Pid and ORC using UpdatePIDPatId(string strPatid) and
        /// UpdateORCOrderNumber(string strOrderNumber) before calling this function
        ///
        /// Takes the instances array of the Result and recreates the parts back into a sendable HL7 message
        /// string with vertical tabs and file seperators as required.
        /// </summary>
        /// <returns>Updated Result Message with Other Vendor Pat and Order info</returns>
        public string CreateUpdatedResultMsgForResultsParse()
        {
            string strRetVal = string.Empty;
            if (!m_strHL7Msg.Contains(HL7.VT.ToString()))
            {
                strRetVal = HL7.VT.ToString();
            }
            //strRetVal += m_strHL7Msg;
            for (int i = 0; i <= m_arrMessageSegments.GetUpperBound(0); i++)
            {
                strRetVal += string.Format("{0}{1}", m_arrMessageSegments[i], HL7.CR);
            }
            if (!m_strHL7Msg.Contains(HL7.FS.ToString()))
            {
                strRetVal += string.Format("{0}{1}", HL7.FS, HL7.CR);
            }

            return strRetVal;
        }

        /// <summary>
        /// Returns an array list with the data for the order to be cancelled. The segments will have to be updated 
        /// by the application.
        /// </summary>
        /// <param name="strTestMnem"></param>
        /// <returns></returns>
        public ArrayList UpdateSegmentForCancelledOrder(string strTestMnem)
        {
            ArrayList alRetVal = new ArrayList();//m_arrMessageSegments);
            bool bHaveOrc = false;

            for (int i = 0; i < m_arrMessageSegments.GetUpperBound(0); i++)
            {
                if (m_arrMessageSegments[i].Substring(0, 4) == "ORC|" && !bHaveOrc)
                {
                    alRetVal.Add(m_arrMessageSegments[i]);
                    bHaveOrc = true;
                    continue;
                }

                if (m_arrMessageSegments[i].Substring(0, 4) == "OBR|")
                {
                    string[] strOBRParts = m_arrMessageSegments[i].Split(new char[] { '|' }, StringSplitOptions.None);
                    if (strOBRParts[4].Contains("^"))
                    {
                        string[] strTestParts = strOBRParts[4].Split(new char[] { '^' }, StringSplitOptions.None);
                        if (strTestParts[0].ToString() == strTestMnem)
                        {
                            alRetVal.Add(m_arrMessageSegments[i]);

                            break;

                        }
                    }
                }
                if (m_arrMessageSegments[i].Substring(0, 4) == "IN1|")
                {
                    continue;
                }
                if (m_arrMessageSegments[i].Substring(0, 4) == "GT1|")
                {
                    continue;
                }
                if (m_arrMessageSegments[i].Substring(0, 4) == "DG1|")
                {
                    continue;
                }

                alRetVal.Add(m_arrMessageSegments[i]);
            }
            return alRetVal;
        }

        /// <summary>
        /// Returns a name string in the following format
        /// LastName,FirstName MiddleName(or Initial if passed) if all three exist in the passed string. or 
        /// LastName,FirstName if only those exist.
        /// if the name cannot be parsed the m_ERR class will have it's propErrMsg set starting with "ERROR NAME:"
        /// and the returned value will be null or empty.
        /// </summary>
        /// <param name="strHL7Name"></param>
        /// <returns></returns>
        public string FormatParsedHL7Name(string strHL7Name)
        {
            string strRetVal = string.Empty;
            if (!strHL7Name.Contains("^"))
            {
                m_ERR.propErrMsg = string.Format("ERROR NAME: [{0}] Can not be parsed.", strHL7Name);
                return strRetVal;
            }
            string[] strSplitName = strHL7Name.Split(new char[] { '^' });
            if (strSplitName.Length != 7)
            {
                // not a valid hl7 formatted name.
            }
            try
            {
                if (string.IsNullOrEmpty(strSplitName[0].Trim()))
                {
                    m_ERR.propErrMsg = "ERROR NAME: Last Name is blank";
                }
                if (string.IsNullOrEmpty(strSplitName[1].Trim()))
                {
                    m_ERR.propErrMsg = "ERROR NAME: First Name is blank";
                }
                switch (strSplitName.Length)
                {
                    case 0:
                    case 1:
                        {
                            break;
                        }
                    case 2:
                        {
                            strRetVal = string.Format("{0}{1}{2}", strSplitName[0], strSplitName[1].Length < 1 ? " " : ", ", strSplitName[1]).Trim();
                            break;
                        }
                    case 3:
                        {
                            strRetVal = string.Format("{0},{1} {2}", strSplitName[0], strSplitName[1], strSplitName[2]).Trim();
                            break;
                        }
                    default:
                        {
                            strRetVal = string.Format("{0}{3},{1} {2}",
                                    strSplitName[0].Trim(),
                                        strSplitName[1].Trim(),
                                            strSplitName[2].Trim(),
                                            string.IsNullOrEmpty(strSplitName[3].Trim()) ? "" : string.Format(" {0}", strSplitName[3].Trim()));
                            break;
                        }
                }
            }
            catch (IndexOutOfRangeException iore)
            {
                strRetVal = string.Empty;
                m_ERR.propErrMsg = string.Format("ERROR NAME: {0}", iore.Message);
            }
            return strRetVal;
        }


    }
}
