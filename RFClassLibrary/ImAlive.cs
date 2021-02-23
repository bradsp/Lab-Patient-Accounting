/*
 * ImAlive b4 06/10/2003 Rick Crone
 * 
 * This class allows the FORD services FordReq and 
 * FordReg to have a heartbeat (hoof beat?) for the 
 * Ford Monitor.
 * 
 * This function is currently in my RFClassLibrary,
 * but is really specifically designed to be used in
 * my FORD system by the FordReq and FordReg services.
 * 
 * At this time (06/10/2003) the ImAlive class is a one
 * trick pony - the Update() member function does all the 
 * work - it updates the ImAlive table in the FORD database.
 * 
 * History:
 *	11/12/2003 Rick Crone
 *		Added try & catch code around FORD connection 
 *		open. Also update other try & catch code to return
 *		false without falling through any other code.
 * 
 *	06/10/2003 Rick Crone
 *		Added some code to include machine name.
 *		And some minor clean up of the code.
 */

using System;
// added 
using System.Data;
using System.Data.SqlClient; // SQL 7.0
using System.Collections; // for machine name






namespace RFClassLibrary
{
    /// <summary>
    /// 05/08/2003 Rick Crone - updates the FORD ImAlive table
    /// </summary>
    public class ImAlive
    {
        private static string strMsg;
        private static string m_strConnection;
        /// <summary>
        /// Returns or sets the ImAlive tables connnection string.
        /// </summary>
        public string propConnectionString
        {
            get { return m_strConnection; }
            set { m_strConnection = value; }
        }

        /// <summary>
        /// Empty constructor. This class allows for updates to the ImAlive table.
        /// Written for the FORD project. Now (04/11/2007) also use with HIS and HMS
        /// servies.
        /// </summary>
        [Obsolete("use the constructor with the connection to the database")]
        public ImAlive()
        {

        }
        /// <summary>
        /// Overloaded to maintain backwards compatability during transistion to this constructor.
        /// </summary>
        /// <param name="strConnection"></param>
        public ImAlive(string strConnection)
        {
            m_strConnection = strConnection;
        }


        //============================
        /// <summary>
        /// 04/16/03 Rick Crone - to update the FORD ImAlive table
        /// </summary>
        [Obsolete("Use the constructor without strFordConnection as it should be set at instanstiation")]
        public bool Update(string strService, string strStatus, string strComment, string strFORDConnection)
        {
            // return value
            bool ret_val = true;
            SqlConnection FORDConnection;

            // get the current date and time
            DateTime now = DateTime.Now;
            string strTime = now.ToString("MM/dd/yy HH:mm:ss:ffff");

            //----add the time stamp to the comment------------
            //string 
            if (strComment.Length != 0)
            {
                strMsg = string.Format("{0} {1}",
                    strTime,
                    strComment);

                strComment = strMsg;

            }
            //---------------

            //-------- get the machine name -------
            string strMachineName;
            strMachineName = string.Format("{0}", Environment.MachineName);
            //    Console.WriteLine("MachineName: {0}", Environment.MachineName);


            //------------------------------------
            // FORD ImAlive table
            if (strFORDConnection.Length == 0)
            {
                // default test system
                //FORDConnection = new SqlConnection(@"Data Source=MCL003;Integrated Security=SSPI; Initial Catalog=FORDTEST");
                // changes server from MCL003 to MCL03 08/16/2006 rgc
                FORDConnection = new SqlConnection(@"Data Source=MCL03;Integrated Security=SSPI; Initial Catalog=FORDTEST");
                //strComment = "Defaulting to  test system";
            }
            else
            {
                FORDConnection = new SqlConnection(strFORDConnection);
                //strComment = strFORDConnection;
            }

            try
            {
                FORDConnection.Open();
            }
#pragma warning disable 168
            catch (Exception excpt)
            {
                // can not open connection to FORD sql server
                return false;
            }
#pragma warning restore 168

            //--- now a data adapter to allow read/write ---

            // ImAlive adapter
            string strSelect;
            //strSelect = string.Format("SELECT * from ImAlive WHERE Service = 'TESTING'");
            strSelect = string.Format("SELECT * from ImAlive WHERE Service = '{0}' AND MachineName = '{1}'",
                strService,
                strMachineName);

            SqlDataAdapter ImAliveAdapter = new SqlDataAdapter(strSelect, FORDConnection);

            //---------------------------------------------

            //--- now CommandBuilder ---------
            // ImAlive Builder
            SqlCommandBuilder ImAliveBuilder = new SqlCommandBuilder(ImAliveAdapter);

            //---------------------------------------------

            //--- now DataSet - to hold the data----
            //ImAlive
            DataSet ImAliveDataSet = new DataSet();

            //------------------------------------

            // fill the ImAliveDataSet - from the ImAlive table
            try
            {
                ImAliveAdapter.Fill(ImAliveDataSet, "ImAlive");
            }
#pragma warning disable 168
            catch (Exception excpt)
            {
                // can not fill dataset
                return false;
            }
#pragma warning restore 168
            //-----------------------------------------


            // now get the record count to see if we are adding a new record or updating an existing record
            double ImAliveRecCount = ImAliveDataSet.Tables["ImAlive"].Rows.Count;
            if (ImAliveRecCount < 1)
            {
                // add a new record
                //----------------
                DataRow NewImAlive = ImAliveDataSet.Tables["ImAlive"].NewRow();

                NewImAlive["Service"] = strService;
                NewImAlive["Time"] = now;
                NewImAlive["strTime"] = strTime;
                NewImAlive["Status"] = "NEW";
                //--- only update comment if we have a new comment ---
                if (strComment.Length > 0)
                {
                    NewImAlive["Comment"] = strComment;
                }
                //-------------------------------------
                NewImAlive["MachineName"] = strMachineName;

                ImAliveDataSet.Tables["ImAlive"].Rows.Add(NewImAlive);

                try
                {
                    ImAliveAdapter.Update(ImAliveDataSet, "ImAlive");
                }
#pragma warning disable 168
                catch (Exception excpt)
                {

                    return false;
                    //strMsg = string.Format("{0} Exception caught.", excpt);
                    //WriteLogFile(strMsg);
                }
#pragma warning restore 168

            }
            else
            {
                // update an existing record

                ImAliveDataSet.Tables["ImAlive"].Rows[0]["Service"] = strService;
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["Time"] = now;
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["strTime"] = strTime;
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["Status"] = strStatus;
                // --- only update comment if we have a new comment ---
                if (strComment.Length > 0)
                {
                    ImAliveDataSet.Tables["ImAlive"].Rows[0]["Comment"] = strComment;
                }
                //---------------------------------------------------
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["MachineName"] = strMachineName;

#pragma warning disable
                //Warning	13	The variable 'excpt' is declared but never used	C:\Source\RFClassLibrary\RFClassLibrary\ImAlive.cs	204	5	RFClassLibrary

                try
                {
                    ImAliveAdapter.Update(ImAliveDataSet, "ImAlive");
                }
                catch (Exception excpt)
                {
                    // ignore error at this time (07/06/06) rgc
                    return (false);
                    //strMsg = string.Format("{0} Exception caught.", excpt);
                    //WriteLogFile(strMsg);
                    //ret_val = false;
                }
#pragma warning restore
            }

            // close the connection		
            FORDConnection.Close();

            return ret_val;
        }

        /// <summary>
        /// Keeps the ImAlive data base updated with current status.
        /// </summary>
        /// <param name="strService">Who</param>
        /// <param name="strStatus">what</param>
        /// <param name="strComment">info</param>
        /// <returns></returns>
        public bool Update(string strService, string strStatus, string strComment)
        {
            // return value
            bool ret_val = true;
            SqlConnection FORDConnection;

            // get the current date and time
            DateTime now = DateTime.Now;
            string strTime = now.ToString("MM/dd/yy HH:mm:ss:ffff");

            //----add the time stamp to the comment------------
            //string 
            if (strComment.Length != 0)
            {
                strMsg = string.Format("{0} {1}",
                    strTime,
                    strComment);

                strComment = strMsg;

            }
            //---------------

            //-------- get the machine name -------
            string strMachineName;
            strMachineName = string.Format("{0}", Environment.MachineName);

            //------------------------------------
            // FORD ImAlive table
            if (string.IsNullOrEmpty(m_strConnection))
            //        if (strFORDConnection.Length == 0)
            {
                // default test system
                //FORDConnection = new SqlConnection(@"Data Source=MCL003;Integrated Security=SSPI; Initial Catalog=FORDTEST");
                // changes server from MCL003 to MCL03 08/16/2006 rgc
                FORDConnection = new SqlConnection(@"Data Source=MCL03;Integrated Security=SSPI; Initial Catalog=FORDTEST");
                //strComment = "Defaulting to  test system";
            }
            else
            {
                FORDConnection = new SqlConnection(m_strConnection);
                //strComment = strFORDConnection;
            }

            try
            {
                FORDConnection.Open();
            }
#pragma warning disable 168
            catch (Exception excpt)
            {
                // can not open connection to FORD sql server
                return false;
            }
#pragma warning restore 168

            //--- now a data adapter to allow read/write ---

            // ImAlive adapter
            string strSelect;
            //strSelect = string.Format("SELECT * from ImAlive WHERE Service = 'TESTING'");
            strSelect = string.Format("SELECT * from ImAlive WHERE Service = '{0}' AND MachineName = '{1}'",
                strService,
                strMachineName);

            SqlDataAdapter ImAliveAdapter = new SqlDataAdapter(strSelect, FORDConnection);

            //---------------------------------------------

            //--- now CommandBuilder ---------
            // ImAlive Builder
            SqlCommandBuilder ImAliveBuilder = new SqlCommandBuilder(ImAliveAdapter);

            //---------------------------------------------

            //--- now DataSet - to hold the data----
            //ImAlive
            DataSet ImAliveDataSet = new DataSet();

            //------------------------------------

            // fill the ImAliveDataSet - from the ImAlive table
            try
            {
                ImAliveAdapter.Fill(ImAliveDataSet, "ImAlive");
            }
#pragma warning disable 168
            catch (Exception excpt)
            {
                // can not fill dataset
                return false;
            }
#pragma warning restore 168
            //-----------------------------------------


            // now get the record count to see if we are adding a new record or updating an existing record
            double ImAliveRecCount = ImAliveDataSet.Tables["ImAlive"].Rows.Count;
            if (ImAliveRecCount < 1)
            {
                // add a new record
                //----------------
                DataRow NewImAlive = ImAliveDataSet.Tables["ImAlive"].NewRow();

                NewImAlive["Service"] = strService;
                NewImAlive["Time"] = now;
                NewImAlive["strTime"] = strTime;
                NewImAlive["Status"] = "NEW";
                //--- only update comment if we have a new comment ---
                if (strComment.Length > 0)
                {
                    NewImAlive["Comment"] = strComment;
                }
                //-------------------------------------
                NewImAlive["MachineName"] = strMachineName;

                ImAliveDataSet.Tables["ImAlive"].Rows.Add(NewImAlive);

                try
                {
                    ImAliveAdapter.Update(ImAliveDataSet, "ImAlive");
                }
#pragma warning disable 168
                catch (Exception excpt)
                {

                    return false;
                    //strMsg = string.Format("{0} Exception caught.", excpt);
                    //WriteLogFile(strMsg);
                }
#pragma warning restore 168

            }
            else
            {
                // update an existing record

                ImAliveDataSet.Tables["ImAlive"].Rows[0]["Service"] = strService;
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["Time"] = now;
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["strTime"] = strTime;
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["Status"] = strStatus;
                // --- only update comment if we have a new comment ---
                if (strComment.Length > 0)
                {
                    ImAliveDataSet.Tables["ImAlive"].Rows[0]["Comment"] = strComment;
                }
                //---------------------------------------------------
                ImAliveDataSet.Tables["ImAlive"].Rows[0]["MachineName"] = strMachineName;

#pragma warning disable
                //Warning	13	The variable 'excpt' is declared but never used	C:\Source\RFClassLibrary\RFClassLibrary\ImAlive.cs	204	5	RFClassLibrary

                try
                {
                    ImAliveAdapter.Update(ImAliveDataSet, "ImAlive");
                }
                catch (Exception excpt)
                {
                    // ignore error at this time (07/06/06) rgc
                    return (false);
                    //strMsg = string.Format("{0} Exception caught.", excpt);
                    //WriteLogFile(strMsg);
                    //ret_val = false;
                }
#pragma warning restore
            }

            // close the connection		
            FORDConnection.Close();

            return ret_val;
        }


    }
}
