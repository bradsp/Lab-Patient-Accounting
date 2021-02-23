using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Posting835Remittance")]
[assembly: AssemblyDescription("Description: Provide support for posting Medicare payments.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Medical Center Labratory")]
[assembly: AssemblyProduct("Posting835Remittance")]
[assembly: AssemblyCopyright("Copyright © Medical Center Lab 2008-2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("33ae2141-4201-4eeb-9747-4e73ec261e5d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.5")]
[assembly: AssemblyFileVersion("1.0.0.5")]


///        <Mod>
///		<Version>1.0.0.6</Version>
///		<Date>20160629</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		Increaded account length check from 8 to 9 to allow for L1 account numbers.
///		</Changes>
///	</Mod>
                   
          
/// <Modifications>
///     <Mod>
///		<Version>1.0.0.6</Version>
///		<Date>20160629</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		Increaded account length check from 8 to 9 to allow for L1 account numbers.
///		</Changes>
///	</Mod>
///     <Mod>
///		<Version>1.0.0.5</Version>
///		<Date>20160523</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		Added changes to allow using the directory that the billing clerks can modify files in.
///		</Changes>
///	</Mod>
///     <Mod>
///		<Version>1.0.0.4</Version>
///		<Date>20160429</Date>
///		<Programmers> David </Programmers>
///		<Changes>
///		Added code to automatically move the files to the proper locations before processing and posting.
///		</Changes>
///	</Mod>
///     <Mod>
///		<Version>1.0.0.3</Version>
///		<Date>20120425</Date>
///		<Programmers> Rick and David </Programmers>
///		<Changes>
///		Moved all local instances of the recordset's to be created in the Load to prevent spid overrun in the SQL database
///		Moved code to check for the account being paid_out from below the insert statement to the first check for
///		the account being the same and changed the test from "...status != "NEW"" to "...status == "PAID_OUT""
///		to prevent overwrite of SSI status "SSIUB,SSI1500" etc.
///		</changes>
///	</Mod>
///     <Modification>
///         <Item>
///         1.0.0.2
///         </Item>
///         <Date>20100661</Date>
///         <Programmers>David</Programmers>
///     </Modification>
///     <Modification>
///         <Item>
///         Original Release.
///         </Item>
///         <Date>20090605</Date>
///         <Programmers>David</Programmers>
///     </Modification>
/// </Modifications>