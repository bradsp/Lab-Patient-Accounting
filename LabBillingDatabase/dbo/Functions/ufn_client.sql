-- =============================================
-- Author:		David
-- Create date: 03/09/2014
-- Description:	allow cross apply
-- =============================================
CREATE FUNCTION [dbo].[ufn_client] 
(
	-- Add the parameters for the function here
	@Client varchar(10) = NULL
	--, 	@cdm varchar(7)
	
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	cli_mnem varchar(10), 
	cli_name varchar(40),
	discountUsed VARCHAR(15),--REAL,
	cdm_mnem VARCHAR(15),
	cdm VARCHAR(7),
	cdmDescription VARCHAR(50),
	cost NUMERIC(18,2),
	priceCli NUMERIC(18,2),
	priceIns NUMERIC(18,2),
--	price NUMERIC(18,2),
	feeSchedule VARCHAR(1),
	cliType VARCHAR(2),--INT,
	SortOrder BIGINT	
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
;WITH cteCpt
AS
(
	select convert(varchar(1),'1') as 'fee_schedule'
		, ROW_NUMBER() OVER (ORDER BY cdm.cdm) AS [SortOrder]
		,cdm.cdm
		, SUM(ISNULL(mprice,0.00)) AS [mprice], SUM(ISNULL(cprice,0.00)) AS [cprice]
		FROM cdm
		LEFT OUTER JOIN cpt4 ON cpt4.cdm = cdm.cdm AND cpt4.deleted = 0 AND cpt4.type <> 'PC'
	where cdm.deleted = 0 AND cdm.orderable = 1 
	GROUP BY cdm.cdm 
		 
	union all
	select convert(varchar(1),'2') as 'fee_schedule'
		, ROW_NUMBER() OVER (ORDER BY cdm.cdm) AS [SortOrder]
		,cdm.cdm
		, SUM(ISNULL(mprice,0.00)) AS [mprice], SUM(ISNULL(cprice,0.00)) AS [cprice]
		FROM cdm
		LEFT OUTER JOIN cpt4_2 cpt4 ON cpt4.cdm = cdm.cdm AND cpt4.deleted = 0 AND cpt4.type <> 'PC'
	where cdm.deleted = 0 AND cdm.orderable = 1 
	GROUP BY cdm.cdm 

	union all
	select convert(varchar(1),'3') as 'fee_schedule'
		, ROW_NUMBER() OVER (ORDER BY cdm.cdm) AS [SortOrder]
		,cdm.cdm
		, SUM(ISNULL(mprice,0.00)) AS [mprice], SUM(ISNULL(cprice,0.00)) AS [cprice]
		FROM cdm
		LEFT OUTER JOIN cpt4_3 cpt4 ON cpt4.cdm = cdm.cdm AND cpt4.deleted = 0 AND cpt4.type <> 'PC'
	where cdm.deleted = 0 AND cdm.orderable = 1 
	GROUP BY cdm.cdm 

	union all
	select convert(varchar(1),'4') as 'fee_schedule'
		, ROW_NUMBER() OVER (ORDER BY cdm.cdm) AS [SortOrder]
		,cdm.cdm
		, SUM(ISNULL(mprice,0.00)) AS [mprice], SUM(ISNULL(cprice,0.00)) AS [cprice]
		FROM cdm
		LEFT OUTER JOIN cpt4_4 cpt4 ON cpt4.cdm = cdm.cdm AND cpt4.deleted = 0 AND cpt4.type <> 'PC'
	where cdm.deleted = 0 AND cdm.orderable = 1 
	GROUP BY cdm.cdm 
	
	union all
	select convert(varchar(1),'5') as 'fee_schedule'
		, ROW_NUMBER() OVER (ORDER BY cdm.cdm) AS [SortOrder]
		,cdm.cdm
		, SUM(ISNULL(mprice,0.00)) AS [mprice], SUM(ISNULL(cprice,0.00)) AS [cprice]
		FROM cdm
		LEFT OUTER JOIN cpt4_5 cpt4 ON cpt4.cdm = cdm.cdm AND cpt4.deleted = 0 AND cpt4.type <> 'PC'
	where cdm.deleted = 0 AND cdm.orderable = 1 
	GROUP BY cdm.cdm 
	
) --SELECT * FROM cteCpt
,cteCdm
AS
(
SELECT  cteCpt.SortOrder,
		cdm.cdm ,
        cdm.descript ,
        mnem ,
        cdm.cost,
	        ISNULL(cteCpt.mprice,0.00)  AS [priceIns],
	        ISNULL(cteCpt.cprice,0.00)  AS [priceCli],
	        fee_schedule
        from dbo.cdm
        LEFT OUTER JOIN cteCpt ON cteCpt.cdm = dbo.cdm.cdm
        
) --SELECT * FROM cteCdm
,cteAllClients
AS
(
SELECT  cli_mnem ,
	        cli_nme ,
	        per_disc AS [Clients Discount] ,
	        type ,
	         cteCdm.SortOrder ,
	                cteCdm.cdm ,
	                cteCdm.descript ,
	                cteCdm.mnem ,
	                cteCdm.cost ,
	                cteCdm.priceIns ,
	                cteCdm.priceCli ,
	                ISNULL(cteCdm.fee_schedule,client.fee_schedule) AS [fee_schedule]
	        FROM dbo.client
	        CROSS JOIN cteCdm
	        WHERE client.deleted = 0 
	        AND (cli_mnem like ISNULL(@Client,cli_mnem))
	        AND client.fee_schedule = cteCdm.fee_schedule
	        
) --SELECT * FROM cteAllClients
, cteDiscount
AS
(
	SELECT  cteClient.cli_mnem ,
	        cteClient.cli_nme ,
	        cteClient.type  AS [client type],
	        cteClient.fee_schedule ,
	        cteClient.cost ,
	        cteClient.mnem ,
	        cteClient.cdm ,
	        cteClient.descript ,
	        cteClient.SortOrder,
	        --cli_dis.price ,
	        CASE WHEN cli_dis.price = 0.00
				THEN COALESCE(cli_dis.percent_ds,cteClient.[Clients Discount]) 
				ELSE CASE WHEN cli_dis.percent_ds IS NULL
					THEN cteClient.[Clients Discount]
					ELSE 
				0 END 
				END AS [DISCOUNT USED],
	        
	        CASE WHEN price <> 0.00
				THEN price
				ELSE CONVERT(NUMERIC(18,2),((100-COALESCE(cli_dis.percent_ds,cteClient.[Clients Discount]))*cteClient.priceCli/100))
				END AS [priceCli],
	        cteClient.priceIns 
	        
	         FROM  cteAllClients AS cteClient
	LEFT OUTER JOIN cli_dis ON cli_dis.cli_mnem = cteClient.cli_mnem 
	AND  (cteClient.cdm BETWEEN cli_dis.start_cdm AND cli_dis.end_cdm AND cli_dis.deleted = 0)--) -- 2300


)

INSERT INTO @Table_var 
(cli_mnem, cli_name, feeSchedule, discountUsed
, cliType
,cdm_mnem, cdm, cdmDescription
, cost
, priceIns, priceCli--, price
, SortOrder
)

select  UPPER(cteDiscount.cli_mnem),UPPER(cli_nme),fee_schedule
,CONVERT(VARCHAR(15),CONVERT(NUMERIC(18,2),[DISCOUNT USED]))
,[client type]
	,UPPER(mnem) ,upper(cdm) ,  upper(descript) 
	,COALESCE(cost,0.00)
    ,COALESCE(priceIns,0.00)
    ,COALESCE(priceCli,0.00)
 --   , price
    ,SortOrder 		
        from cteDiscount
ORDER BY UPPER(cteDiscount.cli_mnem),upper(cdm) ,  upper(descript) 	
	RETURN 
END
