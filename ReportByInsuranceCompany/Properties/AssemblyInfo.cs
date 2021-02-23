using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ReportByInsuranceCompany")]
[assembly: AssemblyDescription("This application creates a list of accounts with out standing balances (not PAID_OUT or CLOSED).  "+
    "The user can select the records by date range, financial class, and all insurances or just insurance A, B, or C. The report will be in Plan Name order with current balances.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Medical Center Lab")]
[assembly: AssemblyProduct("ReportByInsuranceCompany")]
[assembly: AssemblyCopyright("Copyright © Medical Center Lab 2007-2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("66b2c6da-f773-4d47-adf2-d48cb8b244bc")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//

[assembly: AssemblyVersion("1.0.0.16")]
[assembly: AssemblyFileVersion("1.0.0.16")]


          
          
/// <Modifications>
///        <Mod>
///		<Version>1.0.0.16</Version>
///		<Date>20120726</Date>
///		<Programmers>David</Programmers>
///		<Changes>
///		Added a status bar with a progress control and status label
///		</Changes>
///	</Mod>
///        <Mod>
///		<Version>1.0.0.15</Version>
///		<Date>20120726</Date>
///		<Programmers>David</Programmers>
///		<Changes>
///		Added the account balance to the grid, per Carols Request
///		</changes>
///	</Mod>
///     <Mod>
///		<Version>1.0.0.14</Version>
///		<Date>20120522</Date>
///		<Programmers> Rick and David </Programmers>
///		<Changes>
///		Added Launch Acc to the RowHeaderDoubleClick handler
///		</changes>
///	</Mod>
///         <Mod>
///		<Version>1.0.0.13</Version>
///		<Date>20120521</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		Removed unwanted fields and added filtering via the RFClassLibaray.
///		Also removed the "NO INS" BUTTON
///		</changes>
///	</Mod>
/// <Mod>
///		<Version>1.0.0.12</Version>
///		<Date>20120305</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		    Made modifications to the MCL recordset rvw_prg_report_by_plan_name added the ins_code from the insurance
///		    records.
///		    Made the modifications to display the new field in the creation of the table and the addrecord(), 
///		    to incorporate the changes.
///		</changes>
///	</Mod>
///     <Mod>
///		<Version>1.0.0.11</Version>
///		<Date>20120215</Date>
///		<Programmers> Rick and David </Programmers>
///		<Changes>
///      rgc/wdk 20120214 removed the filter element for "not (fin_code in ('client','W','X','Y','Z')" as the fin_code in already eliminates them.
///		</changes>
///	</Mod>
///     <Version> 1.0.0.10
///         <Modifications>
///              <Item>Added Policy Number to the SQL view the data is derived from</Item>
///         </Modifications>    
///         <Release Date>20110829 </Release Date> by <Mod Programmers>David </ModProgrammers>
///     </Version>
///     <Version> 1.0.0.9
///         <Modifications>
///              <Item>rebuilt to pick up changed in RFClassLibrary's DataGridViewPrinter's DrawFooter</Item>
///         </Modifications>    
///         <Release Date>20101005 </Release Date> by <Mod Programmers>David </ModProgrammers>
///     </Version>
///     <Version> 1.0.0.8
///         <Modifications>
///              <Item>Set the From/Thru dates to -45 and -15, also added chrg table to view to remove 
///              accounts with no charges</Item>
///         </Modifications>    
///         <Release Date>20100511 </Release Date> by <Mod Programmers>David </ModProgrammers>
///     </Version>
///     <Version> 1.0.0.7
///         <Modifications>
///              <Item>Moved the construction of the CACC to before the for loop because of time issues. so that it would not
///              be recreated for each record</Item>
///         </Modifications>    
///         <Release Date>20100218 </Release Date> by <Mod Programmers>David </ModProgrammers>
///     </Version>

///     <Version> 1.0.0.6
///         <Modifications>
///              <Item>added the args to the constructor of m_ERR = new ERR(new string[] {"/LIVE", args[0],args[1]});</Item>
///         </Modifications>    
///         <Release Date>20100217 </Release Date> by <Mod Programmers>David </ModProgrammers>
///     </Version>
///     <Version> 1.0.0.5
///         <Modifications>
///              <Item>Added new button to display accounts with out insurance</Item>
///              <Item>Added sorting for balance and date columns</Item>
///         </Modifications>    
///         <Release Date>20090120 </Release Date> by <Mod Programmers>David </ModProgrammers>
///     </Version>
/// <Version>1.0.0.4 </Version>
///     <Modifications>
///         <Item> Upgraded to VS2008 copied from source safe.</Item>
///         <Item> Rebuild incorporated changes in MCL record sets to pass application's ERR Instance </Item>
///     <Release Date>06/12/2008</Release Date> 
///     by <Mod Programmers>David and Rick</Mod Programmers>
/// </Modifications>
/// <Version>1.0.0.3 </Version>
///     <Modifications>
///         <Item> Removed the Creation of the PrintDocument and report Generator from the form_load. Was being
///                 handled on the print button</Item>
///         <Item> Added columns for Data Mailer, UB Date, EBill Batch date, H1500 date, Ebill Batch 1500.</Item>
///         <Item> Set the default print  to Landscape</Item>
///     <Release Date>09/21/2007 
///     </Release Date> by <Mod Programmers>David</Mod Programmers>
/// </Modifications>
