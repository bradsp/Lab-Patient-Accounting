CREATE proc [dbo].[uspCSharpe_Make_Table_Class] 
@tablein varchar(50)

AS
--from Lambert Antonio, 26-jun-2008 
--converted to C# David Kelly, 06/27/2008
Set NoCount ON

declare @field table (id int identity primary key clustered, 
							fieldname varchar(50), 
							fieldtype varchar(50),
							fieldlength int, --varchar(5),
							fieldisNullable bit, 
							fieldcomment varchar(512))
declare @table_name varchar(50)
set @table_name = @tablein  -- this is the table name passed as a paramater

insert into @field
/* under threat of being sullen don't remove or modify this block*/
 Select sc.name
  , case st.name
			when 'tinyint' then 'byte'		-- int 0 to 255 byte 0 to 255
			when 'smallint' then 'int16'	-- -32,768 to 32,767
            when 'int' then 'int32'			-- -2,147,483,648 to 2,147,483,647
			when 'bigint' then 'int64'		-- -9,223,372,036,854,775,808 through positive 9,223,372,036,854,775,807.
			when 'money' then 'decimal'		--  positive 79,228,162,514,264,337,593,543,950,335 to negative 79,228,162,514,264,337,593,543,950,335
			when 'float' then 'double'		-- –1.79E +308 through 1.79E+308
			when 'real' then 'single'		-- -3.402823e38 to 3.402823e38          
			when 'varchar' then 'string'
            when 'bit' then 'string'				-- bit An integer data type that can take a value of 1, 0, or NULL
            when 'datetime' then 'datetime' -- 1/1/1753 to 12/31/9999 | 1/1/0001 12:00:00 to 12/31/9999 11:59:59 P.M.
			when 'smalldatetime' then 'datetime'	-- 1/1/1900 to 6/6/2079 | 1/1/0001 12:00:00 to 12/31/9999 11:59:59 P.M.
			when 'uniqueidentifier' then 'guid'
            else 'string' 
        end As [type],
	sc.length,
	sc.isnullable,
	coalesce(convert(varchar(500),ep.value),'No Comment Available')+ char(13)+char(9)+char(9)+ '/// Field Type - '+ convert(varchar(50),st.name)
	+char(13)+char(9)+char(9)+'/// length - '+ convert(varchar(50),sc.length) + char(13)+char(9)+char(9)+'/// IsNullable - '+ convert(varchar(1), sc.isnullable)
  from syscolumns sc
	inner join systypes st on st.xtype = sc.xtype
	left outer join sys.extended_properties ep on ep.major_id = sc.id and sc.colid = ep.minor_id
	where id = (select id from sysobjects where name = @table_name)
 
-- ep.major_id is the table identifier and ep.minor_id is the column identifier
/* End threat of being sullen don't remove this block*/
/* SOLVED THE MISTERY OF THE SHORT LINE
on the Menu under Tools:Options
	=>QueryResults
		=>SQL Server
			=> Results to Text tree branch
				right above Reset to default button there is a numeric updown that starts out at 
				256 which limits the number of characters the usp can display change to 1024 and works alright.
*/
declare @CSharpe table (id int identity primary key clustered, Line varchar(512)) 

-- exec uspCSharpe_Make_Table_Class amt
Insert into @CSharpe(Line) Select 'using System;'
Insert into @CSharpe(Line) Select 'using System.Collections.Generic;'
Insert into @CSharpe(Line) Select 'using System.Text;'
Insert into @CSharpe(Line) Select '//Programmer added usings'
Insert into @CSharpe(Line) Select 'using RFClassLibrary;'
Insert into @CSharpe(Line) Select 'using System.IO;'
Insert into @CSharpe(Line) Select 'using System.Data;  // for use with DataRow'
Insert into @CSharpe(Line) Select '' -- blank line

Insert into @CSharpe(Line) Select 'namespace MCL' -- Namespace
Insert into @CSharpe(Line) Select '{' -- Namespace 
Insert into @CSharpe(Line) Select char(9)+'/// <summary>'
Insert into @CSharpe(Line) Select char(9)+'/// '+ UPPER(@table_name)+' Recordset based on RCRecordset'
Insert into @CSharpe(Line) Select char(9)+'/// </summary>' 
Insert into @CSharpe(Line) Select char(9)+'public class R_' + @table_name + ' : RCRecordset' -- Class definition
Insert into @CSharpe(Line) Select char(9)+'{' -- Class
Insert into @CSharpe(Line)
Select char(13)+char(9)+char(9)+'private string' + ' m_str' + 
		upper(substring(fieldname,1,1)) +  
		lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname)))+';'+char(13)+char(9)+char(9)+'///<summary>'+char(13)+char(9)+char(9)+'/// '+ rtrim(ltrim(fieldcomment))+char(13)+char(9)+char(9)+'///</summary>'+char(13)+char(9)+char(9)+'public string prop'+ upper(substring(fieldname,1,1)) +  
		lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname))) + '
		{
			get { return m_str' +upper(substring(fieldname,1,1)) +
		lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname))) + '; } 
			set {'+char(13)+char(9)+char(9)+char(9)+char(9)+char(9)+'m_str' + upper(substring(fieldname,1,1)) +  lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname))) + ' = value.Substring(0,value.ToString().Length >'+convert(varchar(5),fieldlength)+'?'++convert(varchar(5),fieldlength)+':value.Length);'+char(13)+char(9)+char(9)+char(9)+char(9)+'}'+char(13)+char(9)+char(9)+'}'
 From @field
 where fieldname not in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')
 Order By ID 
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+'//Private variables set in constructor to initialize the base class' 
Insert into @CSharpe(Line) Select char(9)+'private string m_strServer;' 
Insert into @CSharpe(Line) Select char(9)+'private string m_strDataBase;'

Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+'/// <summary>'
Insert into @CSharpe(Line) Select char(9)+'/// '+ UPPER(@table_name)+' Recordset based on RCRecordset'
Insert into @CSharpe(Line) Select char(9)+'/// </summary>' 
Insert into @CSharpe(Line) Select char(9)+'public R_'+ @table_name+'(string strServer, string strDataBase, ref ERR errLog)' 
Insert into @CSharpe(Line) Select '              : base(strServer, strDataBase, "'+ @table_name + '", ref errLog)'
Insert into @CSharpe(Line) Select '        {' 
Insert into @CSharpe(Line) Select '             m_strServer = strServer;' 
Insert into @CSharpe(Line) Select '             m_strDataBase = strDataBase;'
Insert into @CSharpe(Line) Select '				// Error log set via  RCRecordset m_ERR base class call' 
Insert into @CSharpe(Line) Select '        }'

-- GetRecords
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+'///<summary>'
Insert into @CSharpe(Line) Select char(9)+'/// Gets all records for the strWhere'
Insert into @CSharpe(Line) Select char(9)+'///</summary>' 
Insert into @CSharpe(Line) Select char(9)+'///<param name = "strWhere">filter for the recordset</param>' 
Insert into @CSharpe(Line) Select char(9)+'///<returns>the number of records for the filter</returns>' 
Insert into @CSharpe(Line) Select char(9)+'public int GetRecords(string strWhere)'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'Querry(strWhere);'
Insert into @CSharpe(Line) Select char(9)+char(9)+'m_strErrMsg = string.Format("{0} Records(s) read", m_CurrentRecordCount);'
Insert into @CSharpe(Line) Select char(9)+char(9)+'if (m_CurrentRecordCount> 0)'
Insert into @CSharpe(Line) Select char(9)+char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'LoadMemberVariablesFromDataSet();'
Insert into @CSharpe(Line) Select char(9)+char(9)+'}'
Insert into @CSharpe(Line) Select char(9)+char(9)+'return (m_CurrentRecordCount);'
Insert into @CSharpe(Line) Select char(9)+'}'

-- GetActiveRecords
--Insert into @CSharpe(Line) Select '             '
--Insert into @CSharpe(Line) Select char(9)+'///<summary>'
--Insert into @CSharpe(Line) Select char(9)+'/// Gets all active records for the strWhere'
--Insert into @CSharpe(Line) Select char(9)+'///</summary>' 
--Insert into @CSharpe(Line) Select char(9)+'///<param name = "strWhere">filter for the recordset</param>' 
--Insert into @CSharpe(Line) Select char(9)+'///<returns>the number of records for the filter</returns>' 
--Insert into @CSharpe(Line) Select char(9)+'public int GetActiveRecords(string strWhere)'
--Insert into @CSharpe(Line) Select char(9)+'{'
--Insert into @CSharpe(Line) Select char(9)+char(9)+'strWhere = string.Format("deleted = 0 AND {0}", strWhere);'
--Insert into @CSharpe(Line) Select char(9)+char(9)+'return (GetRecords(strWhere));'
--Insert into @CSharpe(Line) Select char(9)+'}'

insert into @CSharpe(Line)
select 
	case fieldname 
		when 'deleted' then
		char(9)+'///<summary>'+char(13)+
		char(9)+'/// Gets all active records for the strWhere'+char(13)+
		char(9)+'///</summary>'+char(13)+ 
		char(9)+'///<param name = "strWhere">filter for the recordset</param>'+char(13)+ 
		char(9)+'///<returns>the number of records for the filter</returns>'+char(13)+ 
		char(9)+'public int GetActiveRecords(string strWhere)'+char(13)+
		char(9)+'{'+char(13)+
		char(9)+char(9)+'strWhere = string.Format("deleted = 0 AND {0}", strWhere);'+char(13)+
		char(9)+char(9)+'return (GetRecords(strWhere));'+char(13)+
		char(9)+'}'
		end as [name]
from @field
where fieldname = 'deleted'


-- GetNext
Insert into @CSharpe(Line) Select '             '
Insert into @CSharpe(Line) Select char(9)+'///<summary>'
Insert into @CSharpe(Line) Select char(9)+'/// Gets next record from the recordset'
Insert into @CSharpe(Line) Select char(9)+'/// Sets the Recordsets m_strErrMsg to "EOF" when there are no more records in the recordset'
Insert into @CSharpe(Line) Select char(9)+'///</summary>' 
Insert into @CSharpe(Line) Select char(9)+'///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>' 
Insert into @CSharpe(Line) Select char(9)+'public bool GetNext()'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'bool bRetVal = false;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'if (m_CurrentRecordIndex < (m_CurrentRecordCount-1))'
Insert into @CSharpe(Line) Select char(9)+char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'m_CurrentRecordIndex++;'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'return(LoadMemberVariablesFromDataSet());'
Insert into @CSharpe(Line) Select char(9)+char(9)+'}'
Insert into @CSharpe(Line) Select char(9)+char(9)+'m_strErrMsg = "EOF";'
Insert into @CSharpe(Line) Select char(9)+char(9)+'return (bRetVal);'
Insert into @CSharpe(Line) Select char(9)+'}'

-- GetPrev
Insert into @CSharpe(Line) Select '             '
Insert into @CSharpe(Line) Select char(9)+'///<summary>'
Insert into @CSharpe(Line) Select char(9)+'/// Gets previous record from the recordset'
Insert into @CSharpe(Line) Select char(9)+'/// Sets the Recordsets m_strErrMsg to "BOF" when there are no more records in the recordset'
Insert into @CSharpe(Line) Select char(9)+'///</summary>' 
Insert into @CSharpe(Line) Select char(9)+'///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>' 
Insert into @CSharpe(Line) Select char(9)+'public bool GetPrev()'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'bool bRetVal = false;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'if (m_CurrentRecordIndex > 1)'
Insert into @CSharpe(Line) Select char(9)+char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'m_CurrentRecordIndex--;'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'return(LoadMemberVariablesFromDataSet());'
Insert into @CSharpe(Line) Select char(9)+char(9)+'}'
Insert into @CSharpe(Line) Select char(9)+char(9)+'m_strErrMsg = "BOF";'
Insert into @CSharpe(Line) Select char(9)+char(9)+'return (bRetVal);'
Insert into @CSharpe(Line) Select char(9)+'}'

-- MoveFirst
Insert into @CSharpe(Line) Select '             '
Insert into @CSharpe(Line) Select char(9)+'///<summary>'
Insert into @CSharpe(Line) Select char(9)+'/// Sets the records index to the first record in the recordset'
Insert into @CSharpe(Line) Select char(9)+'///</summary>' 
Insert into @CSharpe(Line) Select char(9)+'///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>' 
Insert into @CSharpe(Line) Select char(9)+'public bool MoveFirst()'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'m_CurrentRecordIndex = 1;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'return(LoadMemberVariablesFromDataSet());'
Insert into @CSharpe(Line) Select char(9)+'}'

-- MoveLast
Insert into @CSharpe(Line) Select '             '
Insert into @CSharpe(Line) Select char(9)+'///<summary>'
Insert into @CSharpe(Line) Select char(9)+'/// Sets the records index to the last record in the recordset'
Insert into @CSharpe(Line) Select char(9)+'/// Sets the Recordsets m_strErrMsg to "EOF"'
Insert into @CSharpe(Line) Select char(9)+'///</summary>' 
Insert into @CSharpe(Line) Select char(9)+'///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>' 
Insert into @CSharpe(Line) Select char(9)+'public bool MoveLast()'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'m_CurrentRecordIndex = m_CurrentRecordCount;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'return(LoadMemberVariablesFromDataSet());'
Insert into @CSharpe(Line) Select char(9)+'}'

-- exec uspCSharpe_Make_table_Class conflict
insert into @cSharpe(line) 
Select 
	case fieldname
		when 'deleted' then
		char(9)+'///<summary>'+char(13)+
		char(9)+'/// Flags the current record as deleted'+char(13)+
		char(9)+'///</summary>'+char(13)+ 
		char(9)+'///<returns>Returns the value from Update()</returns>'+char(13)+ 
		char(9)+'public int FlagCurrentRecordDeleted()'+ char(13)+
		char(9)+'{'+ char(13)+
		char(9)+char(9)+'m_strDeleted = "T";'+ char(13)+
		char(9)+char(9)+'return(Update());'+ char(13)+
		char(9)+'}'+ char(13)+
		char(9)+''
	end As [name]
From @field
where fieldname = 'deleted'

insert into @cSharpe(line) 
Select 
	case fieldname
		when 'deleted' then
		char(9)+'///<summary>'+char(13)+
		char(9)+'/// Flags the current record not deleted'+char(13)+
		char(9)+'///</summary>'+char(13)+ 
		char(9)+'///<returns>Returns the value from Update()</returns>'+char(13)+
		char(9)+'public int FlagCurrentRecordNOTDeleted()'+ char(13)+
		char(9)+'{'+ char(13)+
		char(9)+char(9)+'m_strDeleted = "F";'+ char(13)+
		char(9)+char(9)+'return(Update());'+ char(13)+
		char(9)+'}'+char(13)+
		char(9)+''
	end As [name]
From @field
where fieldname = 'deleted'


-- ClearMemberVariables
Insert into @CSharpe(Line) Select '             '
Insert into @CSharpe(Line) Select char(9)+'///<summary>'
Insert into @CSharpe(Line) Select char(9)+'///Clears the member variables for the current instance'
Insert into @CSharpe(Line) Select char(9)+'///</summary>' 
Insert into @CSharpe(Line) Select char(9)+'public void ClearMemberVariables()'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'// Standard Fields'
insert into @cSharpe(line) 
Select 
	case fieldname
		when 'uid'		then ''
		when 'deleted'	then char(9)+char(9)+'m_strDeleted = "F";'
		when 'rowguid'	then char(9)+char(9)+'m_strRowguid = Guid.NewGuid().ToString();'
		when 'mod_date' then char(9)+char(9)+'m_strModDate = Time.SNows();'
		when 'mod_host' then char(9)+char(9)+'m_strModHost = OS.GetMachineName();'
		when 'mod_prg'	then char(9)+char(9)+'m_strModPrg = OS.GetAppName();'
		when 'mod_user'	then char(9)+char(9)+'m_strModUser = OS.GetUserName();'
	end As [name]
From @field
where fieldname in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')

Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+'// Tables fields'
Insert into @CSharpe(Line)
Select char(9)+char(9)+'m_str' + 
		upper(substring(fieldname,1,1)) +  -- capital first letter
		lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname)))+'= "";'
 From @field
 where fieldname not in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')
 Order By ID 
Insert into @CSharpe(Line) Select char(9)+'}'

-- need to add DBException handling when fields don't match add message to the error log
-- return from the function with a -1 set the error message for the m_ERR class so
-- the app can handle appropriately. Additionally need to add a Fatal Error function
-- in RFCObject to handle logging

-- LoadMemberVariablesFromDataSet
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+'/// <summary>    '
Insert into @CSharpe(Line) Select char(9)+'/// Loads the member variables for use add the additional fields    '
Insert into @CSharpe(Line) Select char(9)+'/// Clears the variables before loading.    '
Insert into @CSharpe(Line) Select char(9)+'/// </summary>'
Insert into @CSharpe(Line) Select char(9)+'/// <returns>Number of records update or -1 for Error. m_strErrMsg has details of error</returns>    '
Insert into @CSharpe(Line) Select char(9)+'public bool LoadMemberVariablesFromDataSet()'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'ClearMemberVariables();'
Insert into @CSharpe(Line) Select char(9)+char(9)+'bool bRetVal = false;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'if (m_CurrentRecordIndex > -1) // do not attempt to load if there are no records'
Insert into @CSharpe(Line) Select char(9)+char(9)+'{'
insert into @cSharpe(Line) 
Select distinct char(9)+char(9)+char(9)+'// ==== "standard" fields =====================' +char(13)
From @field
where fieldname in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')

insert into @cSharpe(Line) 
Select 
	case fieldname
		when 'uid' then ''
		when 'deleted'	then char(9)+char(9)+char(9)+'m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";'
		when 'rowguid'	then char(9)+char(9)+char(9)+'m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString();'
		when 'mod_date' then char(9)+char(9)+char(9)+'if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))'+char(13)+
							 char(9)+char(9)+char(9)+'{'+char(13)+
							 char(9)+char(9)+char(9)+char(9)+'m_strModDate = ((DateTime) m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");'+char(13)+
							 char(9)+char(9)+char(9)+'}'
		when 'mod_host' then char(9)+char(9)+char(9)+'m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();'
		when 'mod_prg'	then char(9)+char(9)+char(9)+'m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); '
		when 'mod_user' then char(9)+char(9)+char(9)+'m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();'
	end As [name]
From @field
where fieldname in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')

insert into @CSharpe(Line)
Select distinct char(9)+char(9)+char(9)+'// ==== End of "standard fields" ============='+char(13)
From @field
where fieldname in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')

Insert into @CSharpe(Line) 
Select char(9)+char(9)+char(9)+'m_str' + 
		upper(substring(fieldname,1,1)) +  -- capital first letter
		lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname)))+' = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["'+fieldname+'"].ToString();'
 from @field
where fieldname not in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')
and fieldtype <> 'datetime'
order by ID

Insert into @CSharpe(Line)
Select char(9)+char(9)+char(9)+'if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["'+fieldname+'"].Equals(System.DBNull.Value))'+char(13)+
	   char(9)+char(9)+char(9)+'{'+char(13)+
	   char(9)+char(9)+char(9)+char(9)+'m_str'+upper(substring(fieldname,1,1)) + lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname)))+' = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["'+fieldname+'"]).ToString("G");'+char(13)+
	   char(9)+char(9)+char(9)+'}'
from @field
where fieldname not in ('deleted','uid','rowguid','mod_date','mod_user','mod_host','mod_prg')
and fieldtype = 'datetime'
order by ID

-- exec uspCSharpe_Make_Table_Class worders

Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'// Return true! we were successful'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'bRetVal = true;'
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+'}'
Insert into @CSharpe(Line) Select char(9)+char(9)+'return (bRetVal);'
Insert into @CSharpe(Line) Select char(9)+'}'


-- AddRecord
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+'/// <summary>     '
Insert into @CSharpe(Line) Select char(9)+'/// Returns the number of records in the DataSet for the strWhere passed.    '
Insert into @CSharpe(Line) Select char(9)+'/// if you expect only one record (the currently added record) you can check    '
Insert into @CSharpe(Line) Select char(9)+'///     the return value to see if only one record exists.  '
Insert into @CSharpe(Line) Select char(9)+'///     i.e. only one insurance record for ins_a_b_c of "A" should exist for any patient. More records means an erorr has happened.    '
Insert into @CSharpe(Line) Select char(9)+'///'
Insert into @CSharpe(Line) Select char(9)+'/// This allows the RecordSet instance to scroll through the records as necessary.    '
Insert into @CSharpe(Line) Select char(9)+'///'
Insert into @CSharpe(Line) Select char(9)+'/// This is the new methodology for AddRecord   '
Insert into @CSharpe(Line) Select char(9)+'/// This function will load the DataSet    '
Insert into @CSharpe(Line) Select char(9)+'/// The first record will be the last added record when the strWhere contains and order by mod_date   '
Insert into @CSharpe(Line) Select char(9)+'/// <param name = "strWhere">Filter for the records required</param>'
Insert into @CSharpe(Line) Select char(9)+'/// </summary>'
Insert into @CSharpe(Line) Select char(9)+'public int AddRecord(string strWhere)'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'int iRetVal = -1;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'string strSQL;'
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+'strSQL = string.Format("INSERT INTO {0} ("+'
Insert into @CSharpe(Line) Select char(9)+char(9)+'"'+fieldname+', "+'
 From @field
 Order By ID 
Insert into @CSharpe(Line) Select char(9)+char(9)+'")VALUES("+'
Insert into @CSharpe(Line) Select char(9)+char(9)+'"''{'+convert(varchar(2),ID)+'}'', "+'
 From @field
 Order By ID 
Insert into @CSharpe(Line) Select char(9)+char(9)+'")",'
Insert into @CSharpe(Line) select char(9)+char(9)+'propTable,'
Insert into @CSharpe(Line)
Select char(9)+char(9)+'m_str' + 
		upper(substring(fieldname,1,1)) +  -- capital first letter
		lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname)))+','
 From @field
 Order By ID 
Insert into @CSharpe(Line) Select char(9)+char(9)+');'
Insert into @CSharpe(Line) select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+'iRetVal = SQLExec(strSQL, out m_strErrMsg);'
Insert into @CSharpe(Line) Select char(9)+char(9)+'if (iRetVal > 0)'
Insert into @CSharpe(Line) Select char(9)+char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'//Add new Record to dataset'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'int nRetVal = Querry(strWhere);'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'LoadMemberVariablesFromDataSet();'
Insert into @CSharpe(Line) Select char(9)+char(9)+char(9)+'return nRetVal;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'}'
Insert into @CSharpe(Line) Select char(9)+char(9)+'return iRetVal;'
Insert into @CSharpe(Line) Select char(9)+'}'


-- Update
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+'/// <summary>'
Insert into @CSharpe(Line) Select char(9)+'/// Updates the recordset with the application supplied values. Cannot update the primary key!'
Insert into @CSharpe(Line) Select char(9)+'/// </summary>'
Insert into @CSharpe(Line) Select char(9)+'/// <returns>Count of the records updated</returns>'
Insert into @CSharpe(Line) Select char(9)+'public int Update()'
Insert into @CSharpe(Line) Select char(9)+'{'
Insert into @CSharpe(Line) Select char(9)+char(9)+'string strWhere;'
Insert into @CSharpe(Line) Select char(9)+char(9)+'string strSQL;'
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+'//Set the where clause for the KEY for this table from the DataSet values'
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+'strWhere = string.Format("rowguid = ''' + '{0}'''+ '",'
Insert into @CSharpe(Line) Select char(9)+char(9)+'m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());'
Insert into @CSharpe(Line) Select char(9)+char(9)+'strSQL = string.Format("UPDATE {0} SET " +'
Insert into @CSharpe(Line) Select char(9)+char(9)+'"'+fieldname+'= ''{'+convert(varchar(2),ID)+'}'', "+'
 From @field
 Order By ID 
declare @Where int 
select @Where =  id from @field order by id;
Insert into @CSharpe(Line) Select char(9)+char(9)+'"where {'+convert(varchar(2),(@Where+1))+'}",'

Insert into @CSharpe(Line) select char(9)+char(9)+'propTable,'
Insert into @CSharpe(Line)
Select char(9)+char(9)+'m_str' + 
		upper(substring(fieldname,1,1)) +  -- capital first letter
		lower(substring(fieldname, (charindex('%_',fieldname)+2), len(fieldname)))+','
 From @field
 Order By ID 
Insert into @CSharpe(Line) Select char(9)+char(9)+');'

Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select char(9)+char(9)+'return SQLExec(strSQL, out m_strErrMsg);'
Insert into @CSharpe(Line) Select char(9)+'}'
Insert into @CSharpe(Line) Select ''

Insert into @CSharpe(Line) Select char(9)+'} // do not go below this line'  -- End Class'
Insert into @CSharpe(Line) Select ''
Insert into @CSharpe(Line) Select '}' -- End namespace


Select Line From @CSharpe Order By ID


