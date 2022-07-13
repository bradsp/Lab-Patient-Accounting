CREATE Proc dbo.Indexes_Unused_Select
  @Database sysname = 'ALL'
  ,@Index sysname = 'ALL'
  ,@Min_Row_Count int = 5000
  ,@Table sysname = 'ALL'
  ,@User_scans int = 5
  ,@User_Seeks int = 5

as
Set nocount on
Select @Database = Replace(@Database,'''','''''')
  ,@Index = Replace(@Index,'''','''''')
  ,@Table = Replace(@Table,'''','''''')

Declare @SQL Varchar(Max)
Declare @Unused_Indexes Table (
  [DB_Name] sysname
  ,Table_name sysname
  ,[Index_Name] sysname null
  ,User_Seeks int
  ,User_Scans int
  ,User_Lookups int
  ,Row_Count int
  ,Used_Page_Count int
  ,Space_Reserved Varchar(50)
  ,[Type] sysname
)

DECLARE cr_db CURSOR static FOR  
SELECT  [name]
FROM  Master.sys.Databases SD  
WHERE SD.User_Access = 0 --MultiUser  
 AND SD.[State] = 0 --Online  
 And ([Name] = @Database or @Database = 'ALL')

open cr_db

While 0=0
Begin
  Fetch Next
  From cr_db
  into @Database
  If @@Fetch_Status <> 0
    Break
  begin Try    
    Set @SQL = 'Use ' + @Database + ' 
      Select Db_Name() [DB_Name]
        ,Object_name(ius.Object_id) [Name]
        ,si.name [Index_Name]
        ,ius.User_Seeks
        ,ius.User_Scans
        ,ius.User_Lookups
        ,ps.row_count
        ,ps.used_page_count
        ,Case   
          When in_row_reserved_page_count * 8 < 2000 Then Cast(in_row_reserved_page_count * 8 as Varchar(20)) + ''KB''  
          When in_row_reserved_page_count * 8 / 512 < 5000 Then Cast(in_row_reserved_page_count * 8 / 1024 as Varchar(20)) + ''MB''  
          Else Cast(in_row_reserved_page_count * 8 / 1024 /1024 as Varchar(20)) + ''GB''  
          End As Space_Reserved  
        ,si.type_desc [Type]
      From sys.dm_db_index_usage_stats ius
      Join sys.indexes si
        on ius.Object_id = si.Object_id
        and ius.index_id = si.Index_id
      join sys.dm_db_partition_stats ps
        on si.object_id = ps.object_id
        and si.index_id = ps.index_id
      Where ius.Database_ID = db_id()
        and ( Object_Name(ius.Object_ID) = ''' + @Table + ''' or ''' + @Table + ''' = ''ALL'' )
        and ( si.Name = ''' + @Index + ''' or ''' + @Index + ''' = ''ALL'' )
        and row_count > ' + Cast(@Min_Row_count as Varchar(20)) + '
        --and object_name(ius.object_id) = 
        and (User_scans < ' + Cast(@User_Scans as Varchar(10)) + '
        and user_seeks < ' + Cast(@User_Seeks as Varchar(10)) + ')
      order by DB_Name
        ,[Name]
        ,[Index_Name]'
  Insert @Unused_Indexes
  Exec (@SQL)
  End Try
  Begin Catch
    Print Error_Message()
    Print @Database
  End Catch
End
