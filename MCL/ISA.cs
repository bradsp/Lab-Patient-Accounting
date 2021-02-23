/*
 * This class creates the ISA/IEA envelope for the new ALL electronic billing using X12 for Claimsnet.
 * Created for use by Claim837 Class which is used by the application Claim837App.exe.
 * 07/03/2008 wdk/rgc
 */
using System;
// programmer added usings
using RFClassLibrary;

namespace MCL
{
    class X12_ISA
    {
        /// <summary>
        /// Authorization Information Qualifier
        /// 
        /// 00 by default
        ///     00 - No authorization information present (no meaningful info in ISA02
        ///         ADVISED UNLESS SECURITY REQUIREMENTS MANDAUE USE OF ADDITIONAL IDENTIFICATION
        ///         INFORMATION.
        ///     03 - Additional Data Identification
        /// </summary>
        string m_strISA01;

        /// <summary>
        /// Authorization Information
        /// 
        /// EMPTY by default
        ///     Information used for additional identification of authorization of the interchange sender
        ///     or the data in the interchange; the type of information is seet by the Authorization Information 
        ///     Qualifier ISA01
        /// </summary>
        string m_strISA02;

        /// <summary>
        /// Security Information Qualifier
        /// 
        /// 00 by default
        ///     00 - No security information present (no meaningful info in ISA04
        ///         ADVISED UNLESS SECURITY REQUIREMENTS MANDAUE USE OF PASSWORD DATA
        ///     03 - Password
        /// </summary>
        string m_strISA03;

        /// <summary>
        /// Security Information
        /// 
        /// EMPTY by default
        ///     Used for identifying the security Information about the interchange sender or the data in the 
        ///     interchange; the type of informaiton is set by the Security Information Qualifier ISA03
        /// </summary>
        string m_strISA04;

        /// <summary>
        /// Interchange ID Qualifier -- Sender
        /// 
        /// ZZ by default (CLAIMSNET MUTUALLY DEFINED in Implementation Guide)
        ///     Other values are available see B.4 of the 837 implementation guide.
        /// </summary>
        string m_strISA05;

        /// <summary>
        /// Interchange Sender ID - Identifies the sender
        ///  
        /// Federal Tax ID by default (CLAIMSNET MUTUALLY DEFINED in Implementation Guide)
        /// </summary>
        string m_strISA06;

        /// <summary>
        /// Interchange ID Qualifier -- Receiver
        /// 
        /// ZZ by default (CLAIMSNET MUTUALLY DEFINED in Implementation Guide)
        ///     Other values are available see B.4 and B.5 of the 837 implementation guide.
        /// </summary>
        string m_strISA07;

        /// <summary>
        /// Interchange Receiver ID -- Identifies the receiver
        /// 
        /// 752380212 by default (CLAIMSNET MUTUALLY DEFINED in Implementation Guide)
        /// </summary>
        string m_strISA08;

        /// <summary>
        /// Interchange Date
        /// 
        /// YYMMDD format
        /// </summary>
        string m_strISA09;

        /// <summary>
        /// Interchange Time
        /// 
        /// HHMM format
        /// </summary>
        string m_strISA10;

        /// <summary>
        /// Interchange Control Standards Identifier
        /// 
        /// U by default (U.S. EDI Community of ASC X12, TDCC, and UCS) Code to identify the agency responsible
        ///     for the control standard used by the message that is enclosed by the interchange header and trailer.
        /// </summary>
        string m_strISA11;

        /// <summary>
        /// Interchange Control Version Number
        /// 
        /// 00401 by default
        /// </summary>
        string m_strISA12;

        /// <summary>
        /// Interchange Control Number
        /// 
        /// Attributes 
        ///     (M)anditory
        ///     N0 - means (Numeric) 0 Decimals or Integer value
        ///     9/9 - means must fill nine digits.
        /// </summary>
        string m_strISA13;

        /// <summary>
        /// Acknowledgment Requested
        /// 
        ///     0 - No Acknowledgment Requested
        ///     1 - Interchange Acknowledgment Requested
        ///     
        /// 0 by default (CLAIMSNET DESIGNATED  in Implementation Guide)
        /// </summary>
        string m_strISA14;

        /// <summary>
        /// Usage Indicator
        /// 
        ///     P - Production Data
        ///     T - Test Data
        /// 
        /// P by default (CLAIMSNET has no TEST system all testing must be done via call in to prevent the 
        ///                 file from being processed in the live system.)
        /// </summary>
        string m_strISA15;

        /// <summary>
        /// Component Element Separator
        /// 
        /// : (Colon) by Default (CLAIMSNET Implementation Guide)
        /// </summary>
        string m_strISA16;

        private bool m_bIsValid;
        
        /// <summary>
        /// Constructor which creates the ISA for the main looping structure in Claim837 Class.
        /// Loads Claimsnet defaults in the constructor
        /// </summary>
        /// <param name="strServer">Server Name</param>
        /// <param name="strDatabase">Database Name</param>
        /// <param name="clERR">Reference to the ERR class.</param>
        public X12_ISA(string strServer, string strDatabase, ref ERR clERR)
        {
            m_bIsValid = false;
            try
            {
                R_system lSystem = new R_system(strServer, strDatabase, ref clERR);
                R_number lNumber = new R_number(strServer, strDatabase, ref clERR);
                m_strISA01 = "00";
                m_strISA02 = "";
                m_strISA03 = "00";
                m_strISA04 = "";
                m_strISA05 = "ZZ";
                m_strISA06 = lSystem.GetValue("fed_tax_id");
                m_strISA07 = "ZZ";
                m_strISA08 = "752380212";
                m_strISA09 = DateTime.Today.ToString("yyMMdd");
                m_strISA10 = DateTime.Today.ToString("hhmm");
                m_strISA11 = "U";
                m_strISA12 = "00401";
                m_strISA13 = string.Format("{0:D9}", (int.Parse(lNumber.GetNumber("ISA_control_no"))));
                m_strISA14 = "0";
                m_strISA15 = "P";
                m_strISA16 = ":";
                m_bIsValid = true; 
            }
            catch (Exception ex)
            {
                clERR.propErrMsg = string.Format("ERROR CREATING ISA INSTANCE. \r\n{0}", ex.Message);
              //  clERR.ErrorHandler(ERR.ErrLevel.eINFO); // we expect the application to display/log the message.
            }

                
        }

        /// <summary>
        /// Returns the formatted ISA string.
        /// ISA is the Start of the interchange file
        /// </summary>
        /// <returns></returns>
        public bool GetISA(out string strISA)
        {
            strISA = "***ERROR in GetISA()***";
            if (!m_bIsValid)
            {
                return (m_bIsValid);
            }

            strISA = string.Format("ISA*{0}*{1}*{2}*{3}*{4}*{5}*{6}*{7}*{8}*{9}*{10}*{11}*{12}*{13}*{14}*{15}~{16}",
                                             m_strISA01,
                                                m_strISA02,
                                                    m_strISA03,
                                                    m_strISA04,
                                                    m_strISA05,
                                                    m_strISA06,
                                                    m_strISA07,
                                                    m_strISA08,
                                                    m_strISA09,
                                                    m_strISA10,
                                                    m_strISA11,
                                                    m_strISA12,
                                                    m_strISA13,
                                                    m_strISA14,
                                                    m_strISA15,
                                                    m_strISA16,
                                                        Environment.NewLine);
            
            return (true);

          }

        /// <summary>
        /// End of the interchange file.
        /// 07/03/2008 get the IEA01 from the GS class we think
        /// </summary>
        /// <returns></returns>
        /// <param name="strFunGroups">The number of GS segments in the ISA/IEA envelope</param>
        /// <param name="strIEA">the formatted IEA is available using this variable.</param>
        public bool GetIEA(string strFunGroups, out string strIEA)
        {
            strIEA = "***ERROR in GetIEA()***";
            if (!m_bIsValid)
            {
                return (m_bIsValid);
            }
            
            strIEA = string.Format("IEA*{0}*{1}~{2}",
                                          strFunGroups,
                                             m_strISA13,
                                                Environment.NewLine);
            return (true);
        }

    } // don't type below this line
}
