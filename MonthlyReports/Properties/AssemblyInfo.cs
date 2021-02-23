using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MonthlyReports")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MonthlyReports")]
[assembly: AssemblyCopyright("Copyright ©  2009-2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8427b86f-3bdb-46a2-893d-caae7fbc0718")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.14")]
[assembly: AssemblyFileVersion("1.0.0.14")]


          
/// <MODIFICATIONS>
///     <Mod>
///		<Version>1.0.0.14</Version>
///		<Date>20170522</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		reworked for sql server change to wthmclbill
///		</changes>
///     <Mod>
///		<Version>1.0.0.13</Version>
///		<Date>20140603</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		reworked the m_dgvReport's binding source to be m_dtRecords
///		</changes>
///	</Mod>
///     <Mod>
///		<Version>1.0.0.12</Version>
///		<Date>20121019</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		Changed the Monthly Reports button to group the reports by predefiend categories that have been added to
///		the monthly report table.
///		</changes>
///	</Mod>
///     <Mod>
///		<Version>1.0.0.11</Version>
///		<Date>20120322</Date>
///		<Programmers> Rick and David </Programmers>
///		<Changes>
///		Removed the messagebox on load telling the user this can only be run in Live, as Rick and Dummy are the only ones that can do that.
///		Added a RightClick handler to the datagridview to allow the user to see the data inside the cell selected in a
///		    form. 
///		</changes>
///	</Mod>
///        <Mod>
///		<Version>1.0.0.10</Version>
///		<Date>20120305</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		Modified several queries to improve thier performance.
///		Added a notification icon to inform the users of longer running reports and added code to retry the long
///		running queries.
///		</changes>
///	</Mod>         
/// 1.0.0.9 rgc/wdk 20111227 modified tssbTableReports_DropDownItemClick to read a new tool strip combo box
///             that is loaded with the clients mnemonic and name at form_load if one is selected to prevent
///             the report from crashing when the users (Jan) runs the report for a specific client. If a client
///             is not selected we still load the form with an internal form FormDataCollection.
/// 1.0.0.8 rgc/wdk 20111222 added code to set the datagridview's data source from a datatable. Removed the
///     AutoSizeColumnsMode to inprove the performance of the data loading. YEAH!
/// 1.0.0.7 rgc/wdk 20110922 added try/catch around PROGRAM.CS's MAIN() starting code to handle errors 
///         that are not caught by other code inside the application. Added an Application error handler in 
///         the Application constructor. Added a FormDataCollection to the assembly instead of creating on
///         on the fly in tssbTableReportsClick(). Finally changed the sql query in MonthlyReports table to
///         remove the need for the "WC CDM totals" to be useable for other clinics. this solved the problem
///         we were having with Jan's computer not being able to run the report with out crashing without notification 
///         of any type.
/// 1.0.0.6 rgc/wdk 20110920 added RFClassLibrary's ERR class to provide error logging to track issue as to 
///         why Jan cannot run the WC report. Logging added to several exception handlers and the 
///         SelectRows() function to track the issue.
/// 1.0.0.5 wdk 20110913 removed 1000 item limit from data
/// 1.0.0.4 wdk 20101004 Rebuilt to pick up changes in RFClassLibrary's DataGridViewPrinter's DrawFooter.
/// 1.0.0.3 wdk 20100916 added new report matrix button to retrieve sql from table in billing. Added Printing landscape or portrait.
/// 1.0.0.2 wdk 20100209 added report to check for two CDM's on the same account.
/// 1.0.0.1 rgc/wdk 20090505 Modified the selects for the Accounts with no Patient or Insurance records to add
///     and acc.fin_code not in ('X','Y') to eliiminate these fincodes from the reports as they don't require insurance or patient records
/// </MODIFICATIONS>