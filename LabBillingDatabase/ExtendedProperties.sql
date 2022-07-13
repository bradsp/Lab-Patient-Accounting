EXECUTE sp_addextendedproperty @name = N'AutoDeployed', @value = N'yes', @level0type = N'Assembly', @level0name = N'AccSqlServer';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyProjectRoot', @value = N'C:\source\MCLVS2008\AccSqlServer\AccSqlServer\AccSqlServer', @level0type = N'Assembly', @level0name = N'AccSqlServer';


GO
EXECUTE sp_addextendedproperty @name = N'AutoDeployed', @value = N'yes', @level0type = N'Assembly', @level0name = N'RDS_PROJECT';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyProjectRoot', @value = N'C:\source\MCLVS2008\RDS_PROJECT\RDS_PROJECT', @level0type = N'Assembly', @level0name = N'RDS_PROJECT';


GO
EXECUTE sp_addextendedproperty @name = N'AutoDeployed', @value = N'yes', @level0type = N'Assembly', @level0name = N'SqlServerProject1';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyProjectRoot', @value = N'C:\Documents and Settings\wkelly\Local Settings\Application Data\Temporary Projects\SqlServerProject1', @level0type = N'Assembly', @level0name = N'SqlServerProject1';

