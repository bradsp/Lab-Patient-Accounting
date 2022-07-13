-- =============================================
-- Author:		David 
-- Create date: 04/23/2014
-- Description:	Gets the correct bill method for the charge
-- =============================================
CREATE FUNCTION [dbo].[GetBillMethod] 
(
	-- Add the parameters for the function here
	@acc varchar(15),		-- existing accounts use this
	@client varchar(10),	-- this is the new client
	@cdm VARCHAR(7),		-- either this or the chrg_num must be provided.
	@chrg_num numeric(38,0), -- existing charges use this otherwise use @cdm
	@fincode varchar(10),
	@inscode varchar(10),
	@policy_no varchar(15),
	@age int
)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @cli_type int
	DECLARE @nCount int
	set @nCount = -1
	DECLARE @Result varchar(50)
	-- special cases
	IF(@client IN ('BCH','CGH','COM'))
	BEGIN
		RETURN UPPER(@client)
	END	
	IF (@inscode = 'CLIENT')
	BEGIN
		RETURN UPPER(@client)
	END
	IF (@fincode in ('E','S','PCP'))
	BEGIN
		RETURN 'PATIENT'
	END
	IF (@fincode IN ('W','X','Y','Z','CLIENT'))
	BEGIN
		RETURN upper(@client)
	END
	-- end of special cases
	
	set @Result = 'ERROR: Cannot determine Bill Method'
	
-- Add the T-SQL statements to compute the return value here		
	SELECT @acc = 
		UPPER(@acc)
	
	SELECT @age = 
		coalesce(@age,(SELECT DATEDIFF(YEAR,dob_yyyy,trans_date) FROM dbo.acc
				INNER JOIN pat ON dbo.pat.account = dbo.acc.account
				WHERE acc.account = @acc))
		
	SELECT @fincode = 
		coalesce(@fincode,(SELECT fin_code FROM acc WHERE acc.account = @acc))
		
	SELECT @client = 
		coalesce(@client,(SELECT cl_mnem FROM acc WHERE acc.account = @acc))
		
	select @cli_type = (select type from client where cli_mnem = @client)
		
	SELECT @inscode = 
		coalesce(@inscode,(SELECT coalesce(ins_code,'<INSURANCE ERROR>No Insurance CODE</INSURANCE ERROR>') FROM ins WHERE ins_a_b_c = 'A' AND account = @acc))
		
	SELECT @policy_no = 
		coalesce(@policy_no,(SELECT policy_num FROM ins WHERE ins_a_b_c = 'A' AND account = @acc))
		
	
	-- set the bill method for the bluecare and special bluecross charges
	IF ((@fincode = 'D') OR (@fincode  = 'B' AND @policy_no like 'ZXK%'))
	BEGIN
		IF (@cdm BETWEEN '5520000' AND '5527417' OR @cdm BETWEEN '5527420' AND '552ZZZZ')
		BEGIN
			SELECT @Result = 'QUESTREF'
			RETURN UPPER(@Result)
		END
		ELSE
		BEGIN
			SELECT @Result = (SELECT 
				CASE WHEN COUNT(1) = 0 THEN 'QUESTR' ELSE @inscode END
					FROM dbo.cpt4
					INNER JOIN dbo.dict_quest_exclusions_final_draft dq 
							ON dq.cpt = cpt4.cpt4 AND cpt4.deleted = 0							
					LEFT outer JOIN dbo.dict_quest_reference_lab_tests dt 
							ON dt.cdm = dbo.cpt4.cdm AND dt.deleted = 0
					where ((dq.age_appropriate = 1 AND @age < 11) OR dq.age_appropriate = 0)
					AND cpt4.cdm = @cdm)			
			RETURN UPPER(@Result)
		END					
	END
	
	-- handle special clients that do their own pathology
	IF (EXISTS(SELECT cdm FROM dict_global_billing_cdms WHERE cdm = @cdm) 
			and (@client in ('LEW','TPG','TPG2','TPG3','BMUC','MGPS')))
	BEGIN
		RETURN UPPER(@client)
	END
	
	IF (EXISTS(SELECT cdm FROM dict_global_billing_cdms WHERE cdm = @cdm))
	BEGIN
		
		SET @Result = 'EXISTS'
	END
	
	-- handle affiliates
	IF (EXISTS
	(
		(SELECT cdm FROM dict_global_billing_cdms WHERE cdm = @cdm)) 
		and (
		(@cli_type in (0,1,2) and @fincode NOT IN ('A','M','X','Y'))
		or (@cli_type NOT in ('0','1','2'))
		))
		begin
			
			RETURN UPPER('JPG')
			
		end
--		else
--		BEGIN
--			SELECT @Result = COALESCE(@inscode,@client)
--		END
		
--		IF (@inscode IS NOT NULL)
--		BEGIN
--			RETURN UPPER(@inscode)
--		END
--	
--	
--	
--		
--	-- Return the result of the function	
	RETURN UPPER(COALESCE(@inscode,
		dbo.GetInsCode(@inscode,NULL,@fincode),@Result))
	
END
