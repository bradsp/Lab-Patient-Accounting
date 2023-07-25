using System;
//--- added for the DBAcess class
using System.Data;		
using System.Data.SqlClient;
using System.Windows.Forms;
//------------------------------


namespace RFClassLibrary
{
	/// <summary>
	/// Include database functionallity
	/// </summary>
	public class DBAccess:RFCObject
	{
		// class field list
		private string m_strDataSrc;
		private string m_strDB;
		private string m_strTable;
		// end of class field list

        /// <summary>
        ///  WARNING: no functions can be called using this constructor
        /// </summary>
        public DBAccess()
        {
            m_bValid = false;           
        }

        /// <summary>
        /// Use this constructor to enable calling member methods.
        /// </summary>
        /// <param name="strDataSrc"></param>
        /// <param name="strDB"></param>
        /// <param name="strTable"></param>
		/// <param name="appName"></param>
		public DBAccess(string strDataSrc, string strDB, string strTable)
		{
			// 
			// TODO: Add constructor logic here
			//
            if (strDataSrc.StartsWith("/"))
            {
                strDataSrc = strDataSrc.Remove(0, 1);
            }
			propDataSrc = strDataSrc;
            if (strDB.StartsWith("/"))
            {
                strDB = strDB.Remove(0, 1);
            }
			propDB = strDB;
            if (strTable.StartsWith("/"))
            {
                strTable = strTable.Remove(0, 1);
            }
            propTable = strTable;
            m_strTable = strTable;
            
            m_bValid = true;
		}

		/// <summary>
		/// The data source for the connection (server/DSN)
		/// </summary>
		public string propDataSrc
		{
			get
			{
				return m_strDataSrc;
			}
			set
			{
				m_strDataSrc = value;
			}
		}

		/// <summary>
		/// Database property
		/// </summary>
		public string propDB
		{
			get
			{
				return m_strDB;
			}
			set
			{
				m_strDB = value;
			}
		}

		/// <summary>
		/// Table name 
		/// </summary>
        public string propTable
		{
			get
			{
				return m_strTable;	
			}
			set	
			{	
				m_strTable = value;	
			}
		}

		/// <summary>
		/// strField is the field to be updated
		/// strFilter is the filter to be used to select records (the sql where clause)
		/// 20100420 rgc/wdk if the filter is for a ROWGUID be sure to use the single quote around the
		/// rowguid. ie '9b7ba1c9-04ae-4ceb-a791-000df4a85cb6'
		/// returns: The number of records updated
		/// calling example:
		///		m_strWork = string.Format("rowguid = '{0}'",m_strWPatRowGUID);
		///		db.propTable = "wpat";
		///		if(db.UpdateField("mri", m_strMT_Resp, m_strWork, out m_strMsg) != 1)
		///		{
		///			// error
		///			lf.WriteLogFile("*** ERROR UPDATING Medical record in wpat ***");
		///			lf.WriteLogFile(m_strMsg);
		///			
		///		}
		/// </summary>
		public int UpdateField(string strField, string strNewValue, string strFilter, out string strMsg)
		{
            if (!m_bValid)
            {
                ThrowException(string.Format("{0} {1}", new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, "called while class DBAccess is invalid"));
            }
			string strSQL;
            // wdk 20111031 added code to set the mod_prg when updating the records one field at at time
            // like is done in ViewerDemoTransfer. this will add that application not just ".NetSqlServiceProvider"
			strSQL = string.Format("UPDATE {0} SET {1} = '{2}', mod_prg = '{3}' WHERE {4}",
				m_strTable, /* a property field in this class */
				strField,
				strNewValue,
                Application.ProductName + Application.ProductVersion, // wdk 20111031 added to add actual prog to the file.
				strFilter);
					
		
					
			return SQLExec(strSQL, out strMsg);
		}
	

		/// <summary>
		/// overload of UpdateField to include the table name
		/// *note: reloads the existing table name when done
		/// </summary>
		public int UpdateField(string strField, string strNewValue, string strFilter, out string strMsg, string strTable)
		{
            if (!m_bValid)
            {
                ThrowException(string.Format("{0} {1}", new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, "called while class DBAccess is invalid"));
            }
			int RetVal = -1;
			string strCurrentTable = propTable;
			propTable = strTable;
			RetVal = UpdateField(strField,strNewValue,strFilter,out strMsg);
			propTable = strCurrentTable;
			return(RetVal);
		}
		
		
		/// <summary>
		/// Executes sql UPDATE, INSERT, DELETE statements. See RecCount() for SELECT.
		/// example call to store the meditech medical record index in the wpat table mri field
		///  str = string.Format("UPDATE wpat SET mri = '{0}' WHERE rowguid = '{1}' ",m_strMT_Resp, m_strWPatRowGUID);
		///  
		/// returns number of records update (-1 indicate error - see strMsg for more info) 	
		/// caller may consider 0 to be an error condition - no records updated
		/// </summary>
		public int SQLExec(string strSQL, out string strMsg)
		{
            // 09/12/2008 wdk
            /* SqlClean here causes the UPDATE (non O'HERN) (updating the number table) to fail.
             * Probably need to SqlClean before this call in all calling apps/services.
             * Removed below to allow UPDATE to number table from Post835Remittance
             * Will add SqlClean to the recordsets when setting the Name fields.
             */
            //09/10/2008
            //strSQL = SqlClean(strSQL); //fix for names like O'HERN
            // end of 09/10/2008
            // end of 09/12/2008 wdk

            if (!m_bValid)
            {
                ThrowException(string.Format("{0} {1}", new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, "called while class DBAccess is invalid"));
            }
            if (strSQL.IndexOf("SELECT", 0, 6) > -1) //12/22/2008 wdk changed count in indexof from 5 to 6
            {
                ThrowException("Executes sql UPDATE, INSERT, DELETE statements. See RecCount() for SELECT.");
            }
			int result = -1; // number of records updated

			using (SqlConnection con = new SqlConnection())
			{
				con.ConnectionString = string.Format("Data Source = {0};"+ // Server
					"Database = {1};"+ 
					"Integrated Security=SSPI",m_strDataSrc, m_strDB);

                //SqlTransaction st = null;
				// open the connection
				try
				{
					con.Open();
				}
				catch (Exception excpt)
				{
                    m_ERR.m_Logfile.WriteLogFile(strSQL);
					strMsg = string.Format("***ERROR in DBAccess::SQLExec()'s con.Open()*** {0} Exception caught. {1}", excpt, Environment.StackTrace);
					//WriteLogFile(strMsg);
					//Environment.Exit(13);
                    m_ERR.m_Logfile.WriteLogFile(strMsg); // rgc/wdk 20100323 added
                    m_ERR.AddErrorToDataSet("IMALIVE", OS.GetAppName(), strMsg); //rgc/wdk 20100323 shift in thinking. Let the Services monitor handle restarting dead services
					return(-1); // error can NOT continue
				}

				// Display info about the connection
				if (con.State == ConnectionState.Open)
				{
					
					IDbCommand icom = con.CreateCommand();
                    
					icom.CommandType = CommandType.Text;
                    
					
					icom.CommandText = strSQL;
					try
					{
                      // st = con.BeginTransaction();
    					result = icom.ExecuteNonQuery();
					}
					catch (Exception excpt)
					{
                      //  st.Rollback();
                        m_ERR.m_Logfile.WriteLogFile(strSQL);
						strMsg = string.Format("***ERROR in DBAccess::SQLExec()*** {0} Exception caught. {1}", excpt, Environment.StackTrace);
						return(-1); // error can NOT continue
					}
                  //  st.Commit();
					// success message
					strMsg = string.Format("{0}",result > 1 ? "Records" :"Record");
					//("{0} contains {1} {2} with the status of 'READY'", "wreq",recToDo,recToDo > 1 ? "records" :"record");
					strMsg = string.Format("{0} record(s) updated.",result);
					
				}
				else
				{
					strMsg = "DBAcess::SQLExec() unable to open SQL connection";
				}
			}
			return result;
		}
        
        
        /// <summary>
        ///   Implement IDisposable.
        ///   Do not make this method virtual.
        ///   A derived class should not be able to override this method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.

            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Use interop to call the method necessary
        /// to clean up the unmanaged resource.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        private bool disposed = false;
        private IntPtr handle;
        private System.ComponentModel.Component component = new System.ComponentModel.Component(); 
        /// <summary>
        ///  Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    component.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;

                // Note disposing has been done.
                disposed = true;

            }

        }

		/// <summary>
		/// strTable is the table to count the records from
		/// strFilter is the filter to be used to select records (the sql where clause)
		/// returns: The number of records updated
		/// calling example:
		///		m_strWork = string.Format("rowguid = '{0}'",m_strWPatRowGUID);
		///		db.propTable = "wpat";
		///		if(db.RecCount(m_strTable, m_strFilter, out m_strMsg) != 1)
		///		{
		///			// error
		///			lf.WriteLogFile("*** ERROR Counting Medical record in wpat ***");
		///			lf.WriteLogFile(m_strMsg);
		///			
		///		}
		/// </summary>
		public int RecCount(string strTable, string strField, string strOp, string strFilter, out string strMsg)
		{
            if (!m_bValid)
            {
                ThrowException(string.Format("{0} {1}", new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, "called while class DBAccess is invalid"));
            }
			lock(this)
			{
				string strSQL = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} {2} '{3}'",strTable,strField,strOp,strFilter);		
				int result = -1;
				using (SqlConnection con = new SqlConnection())
				{
					con.ConnectionString = string.Format("Data Source = {0};"+ // Server
						"Database = {1};"+ 
						"Integrated Security=SSPI",m_strDataSrc, m_strDB);

					// open the connection
					// this may not work as I expected
					try
					{
						con.Open();
					}
					catch (SqlException se)
					{
				
						strMsg = string.Format("***ERROR in DBAccess::RecCount()'s con.Open()*** {0} Exception caught. {1}", se, Environment.StackTrace);
						//WriteLogFile(strMsg);
						//Environment.Exit(13);
                        m_ERR.m_Logfile.WriteLogFile(strMsg); // rgc/wdk 20100323 added
                        m_ERR.AddErrorToDataSet("IMALIVE", OS.GetAppName(), strMsg); //rgc/wdk 20100323 shift in thinking. Let the Services monitor handle restarting dead services
					
						return(-1); // error can NOT continue
					}

					// Display info about the connection
					if (con.State == ConnectionState.Open)
					{
						IDbCommand icom = con.CreateCommand();
						icom.CommandType = CommandType.Text;
						icom.CommandText = strSQL;
						try
						{
							result = (int) icom.ExecuteScalar();
						}
						catch (Exception excpt)
						{
				
							strMsg = string.Format("***ERROR in DBAccess::RecCount()*** {0} Exception caught. {1}", excpt, Environment.StackTrace);
							return(-1); // error can NOT continue
						}

						// success message
						strMsg = string.Format("{0} {1} in table.",result, result > 1 ? "Records" : "Record");
					
					}
					else
					{
						strMsg = "DBAcess::RecCount() unable to open SQL connection";
					}
				}
				return result;
							
			}// end of lock
		} // end of reccount function
	
	    /// <summary>
	    /// Gets the real record count.
	    /// </summary>
	    /// <param name="strTable"></param>
	    /// <param name="strFilter"></param>
	    /// <param name="strMsg"></param>
	    /// <returns></returns>
		public int RecCount(string strTable, string strFilter, out string strMsg)
		{
            if (!m_bValid)
            {
                ThrowException(string.Format("{0} {1}", new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, "called while class DBAccess is invalid"));
            }
			lock(this)
			{
				string strSQL = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} ", strTable, strFilter);		
				int result = -1;
				using (SqlConnection con = new SqlConnection())
				{
					con.ConnectionString = string.Format("Data Source = {0};"+ // Server
						"Database = {1};"+ 
						"Integrated Security=SSPI",m_strDataSrc, m_strDB);

					// open the connection
					// this may not work as I expected
					try
					{
						con.Open();
					}
					catch (SqlException se)
					{
						strMsg = string.Format("***ERROR in DBAccess::RecCount()'s con.Open()*** {0} Exception caught. {1}", se, Environment.StackTrace);
						//WriteLogFile(strMsg);
						//Environment.Exit(13);
                        m_ERR.m_Logfile.WriteLogFile(strMsg); // rgc/wdk 20100323 added
                        m_ERR.AddErrorToDataSet("IMALIVE", OS.GetAppName(), strMsg); //rgc/wdk 20100323 shift in thinking. Let the Services monitor handle restarting dead services
					
						return(-1); // error can NOT continue
					}

					// Display info about the connection
					if (con.State == ConnectionState.Open)
					{
						IDbCommand icom = con.CreateCommand();
						icom.CommandType = CommandType.Text;
						icom.CommandText = strSQL;
						try
						{
							result = (int) icom.ExecuteScalar();
						}
						catch (Exception excpt)
						{
							strMsg = string.Format("***ERROR in DBAccess::RecCount()*** {0} Exception caught. {1}", excpt, Environment.StackTrace);
							return(-1); // error can NOT continue
						}

						// success message
						strMsg = string.Format("{0} {1} in table.",result, result > 1 ? "Records" : "Record");
					}
					else
					{
						strMsg = "DBAcess::RecCount() unable to open SQL connection";
					}
				}
				return result;
			} // end of lock
		}// end of reccount 2 function

		/// <summary>
		/// GetField() 
        /// Mod Hist: 08/31/2006 Rick Crone
        ///     Was returning error. Error is stored in the out variable strMsg.
        ///     Now returns an empty string on error - see strMsg for error details.
        /// Rick Crone 2006
		/// </summary>
        public string GetField(string strTable, string strField, string strFilter, out string strMsg)
        {
            if (!m_bValid)
            {
                ThrowException(string.Format("{0} {1}", new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, "called while class DBAccess is invalid"));
            }
            lock (this)
            {
                string strSQL = string.Format("SELECT {0} FROM {1} WHERE {2}",
                                                strField,
                                                 strTable,
                                                  strFilter);

                string result = "";
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = string.Format("Data Source = {0};" + // Server
                        "Database = {1};" +
                        "Integrated Security=SSPI", m_strDataSrc, m_strDB);

                    // open the connection
                    // this may not work as I expected
                    try
                    {
                        con.Open();
                    }
                    
                    catch (SqlException se)
                    {

                        strMsg = string.Format("***ERROR in DBAccess::GetField()'s con.Open()*** {0} Exception caught. {1}", se, Environment.StackTrace);
                        //WriteLogFile(strMsg);
                        //Environment.Exit(13);
                        m_ERR.m_Logfile.WriteLogFile(strMsg); // rgc/wdk 20100323 added
                        m_ERR.AddErrorToDataSet("IMALIVE", OS.GetAppName(), strMsg); //rgc/wdk 20100323 shift in thinking. Let the Services monitor handle restarting dead services
					
                        return (result); // error can NOT continue
                    }

                    // Display info about the connection
                    if (con.State == ConnectionState.Open)
                    {
                        IDbCommand icom = con.CreateCommand();
                        icom.CommandType = CommandType.Text;
                        icom.CommandText = strSQL;
                        try
                        {
                            result = icom.ExecuteScalar().ToString();
                        }
                        catch (System.NullReferenceException)
                        {
                            // success message nothing in the result set
                            strMsg = string.Format("{0}", result);
                            return (result);
                        }
                        catch (Exception excpt)
                        {
                            // null reference just means no record found
                            strMsg = string.Format("***ERROR in DBAccess::GetField()*** {0} Exception caught. {1}", excpt, Environment.StackTrace);
                            return (result); // error can NOT continue
                        }

                        // success message
                        strMsg = string.Format("{0}", result);

                    }
                    else
                    {
                        strMsg = "DBAcess::GetField() unable to open SQL connection";
                    }
                }
                return result;

            }
        }// end of lock

        /// <summary>
        /// This function added for applications that need a database connection.
        /// Caller (application)or(RCRecordset) owns the DBConnection and passes it as an out parameter
        /// to this function.
        /// 08/09/2006 Rick Crone
        /// </summary>
        /// <param name="DBConnection"></param>
        /// <returns>
        /// true = success
        /// false = failure
        /// </returns>
        public bool InitalizeDBConnection(out SqlConnection DBConnection)
            {
                if (!m_bValid)
                {
                    ThrowException(string.Format("{0} {1}", new System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name, "called while class DBAccess is invalid"));
                }
                bool bRetVal = true; // success
                string strDBConnection = string.Format(@"Data Source={0};Integrated Security=SSPI;Initial Catalog={1};Application Name={2};Connection Timeout = 120",
                                                            propDataSrc,
                                                             propDB,
															 propAppName);
                    
                //********************************
                DBConnection = new SqlConnection(strDBConnection);
                //********************************

                try
                {
                    DBConnection.Open();
                }
                catch (Exception excpt)
                {
                    propErrMsg = string.Format("Can NOT open DB connection. {0} Exception caught.\r\r DBConnection {1}\r\r", excpt, strDBConnection);
                    string strMsg = string.Format("Can NOT open DB connection. {0} Exception caught.", excpt);
                    //WriteLogFile(strMsg);
                    bRetVal = false;
                }

                

                return (bRetVal);
            }


 
		} 		
	}
