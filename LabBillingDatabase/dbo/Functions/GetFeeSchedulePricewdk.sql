-- =============================================
-- Author:		David
-- Create date: 06/13/2013
-- Description:	Gets the fee schedule info based on client and order code
--=============================================
CREATE FUNCTION [dbo].[GetFeeSchedulePricewdk] 
(
	-- Add the parameters for the function here
	@client varchar(10),
	@cdm varchar(7)
	
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colClient varchar(10),
	colCdm varchar(7),
	colCdmDescription varchar(50),
	colLinks int,
	colMPrice numeric(18,2),
	colCPrice numeric(18,2),
	colZPrice NUMERIC(18,2),
	colCDMDiscount numeric(18,2),
	colClientPrice numeric(18,2),
	colClientDiscount numeric(18,2),
	colError varchar(1024)	 
)
AS
BEGIN
--	DECLARE @client VARCHAR(10)
--	SET @client = 'k'
--	DECLARE @cdm VARCHAR(7)
--	SET @cdm = '5382522'

	-- Fill the table variable with the rows for your result set
	declare @feeSchedule varchar(2)
	
	select @client = upper(@client)
	if (NOT EXISTS (select cli_mnem from client 
			where cli_mnem = @client and deleted = 0))
	BEGIN
		insert into @Table_var(colClient, colCdm, colError)
		values (@client, @cdm, 'Client does not exist in the client table')
		return
--		SELECT 'not'
--		return
	END
	
		
	select @feeSchedule = (select fee_schedule from client 
		where cli_mnem = @client and deleted = 0)
	--	SELECT @feeSchedule
	if (NOT EXISTS (select fee_schedule from client 
			where cli_mnem = @client and deleted = 0))
	BEGIN
		insert into @Table_var(colClient, colError)
		values (@client, 'Client fee schedule does not exist in the client table')
		return
--		SELECT 'no'
--		return
	END
	
	DECLARE @clientDiscount numeric(18,2)
	SELECT  @clientDiscount = (select per_disc from client 
									where cli_mnem = @client and deleted = 0)

	--SELECT @clientDiscount
	;with cteFeeSchedule
	as
	(
	select convert(varchar(1),'1') as 'fee_schedule'
		,cdm.cdm, cdm.descript as [CDM DESCRIPTION] 
		, SUM(cpt4.mprice) OVER (PARTITION BY cpt4.cdm ) AS [mprice]
		, SUM(cpt4.cprice) OVER (PARTITION BY cpt4.cdm ) AS [cprice]
		, SUM(cpt4.zprice) OVER (PARTITION BY cpt4.cdm ) AS [zprice]
		, COUNT(cpt4.link) OVER (PARTITION BY cpt4.cdm ) AS [links]
		--, type
	from cdm
	inner join cpt4 on cpt4.cdm = cdm.cdm and cpt4.deleted = 0
	where cdm.deleted = 0 --and cpt4.deleted = 0 
			AND cdm.cdm = @cdm	and type <> 'PC'                                                  

	UNION ALL
	
	select convert(varchar(1),'2') as 'fee_schedule'
		,cdm.cdm, cdm.descript as [CDM DESCRIPTION] 
		, SUM(cpt4.mprice) OVER (PARTITION BY cpt4.cdm ) AS [mprice]
		, SUM(cpt4.cprice) OVER (PARTITION BY cpt4.cdm ) AS [cprice]
		, SUM(cpt4.zprice) OVER (PARTITION BY cpt4.cdm ) AS [zprice]
		, COUNT(cpt4.link) OVER (PARTITION BY cpt4.cdm ) AS [links]
		--, type
	from cdm
	inner join cpt4_2 AS [cpt4] on cpt4.cdm = cdm.cdm and cpt4.deleted = 0
	where cdm.deleted = 0 --and cpt4.deleted = 0 
			AND cdm.cdm = @cdm	and type <> 'PC'                                                  


	UNION ALL
	
	select convert(varchar(1),'3') as 'fee_schedule'
		,cdm.cdm, cdm.descript as [CDM DESCRIPTION] 
		, SUM(cpt4.mprice) OVER (PARTITION BY cpt4.cdm ) AS [mprice]
		, SUM(cpt4.cprice) OVER (PARTITION BY cpt4.cdm ) AS [cprice]
		, SUM(cpt4.zprice) OVER (PARTITION BY cpt4.cdm ) AS [zprice]
		, COUNT(cpt4.link) OVER (PARTITION BY cpt4.cdm ) AS [links]
		--, type
	from cdm
	inner join cpt4_3 AS [cpt4] on cpt4.cdm = cdm.cdm and cpt4.deleted = 0
	where cdm.deleted = 0 --and cpt4.deleted = 0 
			AND cdm.cdm = @cdm	and type <> 'PC'                                                  


	UNION ALL
	
	select convert(varchar(1),'4') as 'fee_schedule'
		,cdm.cdm, cdm.descript as [CDM DESCRIPTION] 
		, SUM(cpt4.mprice) OVER (PARTITION BY cpt4.cdm ) AS [mprice]
		, SUM(cpt4.cprice) OVER (PARTITION BY cpt4.cdm ) AS [cprice]
		, SUM(cpt4.zprice) OVER (PARTITION BY cpt4.cdm ) AS [zprice]
		, COUNT(cpt4.link) OVER (PARTITION BY cpt4.cdm ) AS [links]
		--, type
	from cdm
	inner join cpt4_4 AS [cpt4] on cpt4.cdm = cdm.cdm and cpt4.deleted = 0
	where cdm.deleted = 0 --and cpt4.deleted = 0 
			AND cdm.cdm = @cdm	


	)-- SELECT * FROM cteFeeSchedule WHERE fee_schedule = @feeSchedule
	, ctePrice
	as
	(
		select @client as [client]
			,COALESCE(cdm,@cdm) AS [CDM] , [cdm description]
			,links
			,mprice, cprice, zprice
			--,type
			, cli_dis.percent_ds AS [cdm discount]
			,ROUND(
				COALESCE(
					CASE WHEN percent_ds = 0.00 AND price <> 0.00 THEN price
					ELSE null END,(((100-COALESCE(cli_dis.percent_ds,@clientDiscount))*cprice)/100))
					,2
						
				  ) AS price
				
		,@clientDiscount as [client discount]
		from cteFeeSchedule
		left outer join cli_dis 
			on cteFeeSchedule.cdm between cli_dis.start_cdm and cli_dis.end_cdm	
				AND dbo.cli_dis.cli_mnem = @client AND cli_dis.deleted = 0
				
		where cdm = @cdm  and cteFeeSchedule.fee_schedule = @feeSchedule	
	) --SELECT * FROM ctePrice

insert into @Table_Var 
	(
	colClient, colCdm, colCdmDescription
	, colLinks, colMPrice, colCPrice, colZPrice,
	colCDMDiscount, colClientPrice, colClientDiscount--,colError	 
	)	
	
	select TOP(1)
	@client,
	COALESCE(ctePrice.cdm,@cdm), [cdm description]
	, links ,mprice , cprice, zprice
	,[cdm discount],price AS price,[client discount]
	from ctePrice


	RETURN 
END
