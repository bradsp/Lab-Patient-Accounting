using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using RFClassLibrary;
using MCL;
using System.Collections;
using System.Data;
using EdiTools;
using LabBilling.Core.DataAccess;
using System.Data.Common;

namespace LabBilling.Core
{
    public class Billing837
    {
        private SystemParametersRepository parametersdb;

        SqlConnection m_sqlConnection = null;
        string m_strInterchageControlNumber = null;
        string m_strProductionEnvironment;
        string m_strServer = null;
        string m_strDatabase = null;
        string m_strType = null;
        ArrayList m_alNameSuffix = new ArrayList() { "JR", "SR", "I", "II", "III", "IV", "V", "VI", "VII" };
        private string m_strTransSetControlNumber;
        private Dictionary<string, string> m_dicClaimFilingIndicatorCode;
        private Dictionary<string, string> m_dicRefs;
        private DataTable m_dtInsuranceInfo;
        private DataTable m_dtCpt4Desc;
        private ArrayList m_alCpt4DescriptionRequired;
        private ReportGenerator m_rgReport;
        R_acc m_rAccUpdate = null;
        R_pat m_rPat = null;
        R_number m_rNumber = null;
        ERR m_Err = null;
        private string m_strFilter = null;
        DataSet m_dsBilling = null;
        int m_nHLCounter = 1;
        int m_nHLParent = 0;
        int m_nST = 1;
        int m_nSTSegments = 2; // includes the ST and SE without counting
        string m_strSubmitterId = null;
        int m_nFunctionalGroups = 0;
        string m_strBHT = null;
        string m_strST = null;
        string m_strSE = null;
        string m_strGS = null;
        string m_strGE = null;
        string m_strISA = null;
        string m_strIEA = null;
        ArrayList m_alFile = null;
        SqlDataAdapter m_daAcc = null;
        SqlDataAdapter m_daPat = null;
        SqlDataAdapter m_daIns = null;
        SqlDataAdapter m_daChrg = null;
        SqlDataAdapter m_daAmt = null;

        bool m_bHas1500Error = false;
        StringBuilder m_sbUBHeader = new StringBuilder();
        StringBuilder m_sbUB = new StringBuilder();
        StringBuilder m_sb1500Header = new StringBuilder();
        StringBuilder m_sb1500 = new StringBuilder();
        public string propProductionEnvironment { get; set; }
        private string _connectionString;

        public Billing837(string connectionString)
        {
            _connectionString = connectionString;

            DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
            dbConnectionStringBuilder.ConnectionString = connectionString;

            m_strServer = (string)dbConnectionStringBuilder["Server"];
            m_strDatabase = (string)dbConnectionStringBuilder["Database"];

            m_sqlConnection =
                new SqlConnection(connectionString);

            parametersdb = new SystemParametersRepository(_connectionString);

            propProductionEnvironment = m_strDatabase.Contains("LIVE") ? "P" : "T";
            m_strSubmitterId = parametersdb.GetByKey("fed_tax_id") ?? "626010402";
            m_alFile = new ArrayList();
            string[] strArgs = new string[3];
            strArgs[0] = m_strDatabase.Contains("LIVE") ? "/LIVE" : "/TEST";
            strArgs[1] = m_strServer;
            strArgs[2] = m_strDatabase;

            m_Err = new ERR(strArgs);
            m_rAccUpdate = new R_acc(m_strServer, m_strDatabase, ref m_Err);
            m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_Err);
            m_rNumber = new R_number(m_strServer, m_strDatabase, ref m_Err);

        }

        public EdiDocument Document;

        /// <summary>
        /// Generates a batch of 837 claims
        /// </summary>
        /// <param name="accounts"></param>
        public void Generate837ClaimBatch(List<string> accounts)
        {
            var ediDocument = new EdiDocument();

            var isa = new EdiSegment("ISA");


            //m_strISA = string.Format("ISA*00*          *00*          *ZZ*{0}*ZZ*{1}*{2}*{3}*U*00401*{4}*1*{5}*:~",
            //    m_strSubmitterId.PadLeft(15),
            //    string.Format("ZMIXED").PadLeft(15), // 8
            //    DateTime.Now.ToString("yyMMdd").ToString(), // 9
            //    DateTime.Now.ToString("HHmm").ToString(), // 10
            //    m_strInterchageControlNumber,
            //    propProductionEnvironment);

            isa[01] = "00";
            isa[02] = "".PadRight(10);
            isa[03] = "00";
            isa[04] = "".PadRight(10);
            isa[05] = "ZZ";
            isa[06] = "SENDER".PadRight(15);
            isa[07] = "ZZ";
            isa[08] = "ZMIXED".PadRight(15);
            isa[09] = EdiValue.Date(6, DateTime.Now);
            isa[10] = EdiValue.Time(4, DateTime.Now);
            isa[11] = "U";
            isa[12] = "00401";
            isa[13] = m_strInterchageControlNumber; // 1.ToString("d9");
            isa[14] = "1";
            isa[15] = propProductionEnvironment; //  "P";
            isa[16] = ":";
            ediDocument.Segments.Add(isa);


            //m_strGS = string.Format("GS*HC*{0}*ZMIXED*{1}*{2}*{3}*X*004010X098A1~",
            //    m_strSubmitterId.PadRight(10),
            //    DateTime.Now.ToString("yyyyMMdd"),
            //    DateTime.Now.ToString("HHmm"),
            //    m_strInterchageControlNumber);

            var gs = new EdiSegment("GS");
            gs[01] = "HC";
            gs[02] = m_strSubmitterId.PadRight(10); // "SENDER";
            gs[03] = "ZMIXED";
            gs[04] = EdiValue.Date(8, DateTime.Now);
            gs[05] = EdiValue.Time(4, DateTime.Now);
            gs[06] = m_strInterchageControlNumber; // EdiValue.Numeric(0, 1);
            gs[07] = "X";
            gs[08] = "004010X098A1";
            ediDocument.Segments.Add(gs);

            //loop through accounts - generate loops for each claim
            foreach (string account in accounts)
            {


                // more segments...
                // ST
                // BHT
                // NM1
                // PER
                // NM1

                // Loop 2000A
                // HL1
                // Loop 2010A
                // Loop 2010AA
                // --NM1
                // --N3
                // --N4
                // --REF
                // --PER
                // Loop 2010AB
                // --NM1
                // --N3
                // --N4

                // Loop 2000B
                // --HL
                // --SBR
                // Loop 2010B
                // Loop 2010BA
                // --NM1
                // --N3
                // --N4
                // --DMG
                // Loop 2010BB
                // -- NM1
                // -- N3
                // -- N4

                // Loop 2300
                // --CLM
                // --DTP
                // --REF
                // --HI
                // Loop 2310
                // Loop 2310A
                // - NM1
                // Loop 2310B
                // - NM1
                // - PRV
                // Loop 2310C
                // - NM1
                // - N3
                // - N4

                // Loop 2400
                // - LX
                // - SV1
                // - DTP
                // - REF
                // - SE
            }


            // Footer
            // GE
            // IEA

            ediDocument.Options.SegmentTerminator = '~';
            ediDocument.Options.ElementSeparator = '*';
            ediDocument.Save("c:\\temp\\MCL-837.txt");
        }

        /// <summary>
        /// Generates single 837 claim
        /// </summary>
        /// <param name="account"></param>
        public void Generate837cliam(string account)
        {

        }


    }
}
