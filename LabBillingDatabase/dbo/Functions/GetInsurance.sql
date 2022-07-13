-- =============================================
-- Author:		David
-- Create date: 07/23/2014
-- Description:	Get existing insurances for account
-- =============================================
CREATE FUNCTION GetInsurance 
(
	-- Add the parameters for the function here
	@acc varchar(15)
	
)
RETURNS 
@Table_Var TABLE 
(
	-- Add the column definitions for the TABLE variable here
	colAccount varchar(15),
	colPriority varchar(1),
	colHolderName varchar(40) ,
	colHolderDob datetime,
	colHolderSex varchar(1) ,
	colHolderAddr varchar(40),
	colHolderCSZ varchar(40) ,
	colPlanName varchar(45) ,
	colPlanAddr1 varchar(40) ,
	colPlanAddr2 varchar(40),
	colPlanCSZ varchar(40) ,
	colPolicyNum varchar(50) ,
	colCertSsn varchar(15),
	colGroupName varchar(50) ,
	colGroupNum varchar(15),
	colEmployer varchar(25),
	colEmployerCSZ varchar(35),
	colFinCode varchar(1),
	colInsCode varchar(10),
	colRelation varchar(2),
	colModDate datetime,
	colModUser varchar(50),
	colModPrg varchar(50),
	colModHost varchar(50) ,
	colHolderLName varchar(40),
	colHolderFNname varchar(40),
	colHolderMName varchar(40)
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	INSERT INTO @Table_Var
			(
				colAccount,
				colPriority,
				colHolderName,
				colHolderDob,
				colHolderSex,
				colHolderAddr,
				colHolderCSZ,
				colPlanName,
				colPlanAddr1,
				colPlanAddr2,
				colPlanCSZ,
				colPolicyNum,
				colCertSsn,
				colGroupName,
				colGroupNum,
				colEmployer,
				colEmployerCSZ,
				colFinCode,
				colInsCode,
				colRelation,
				colModDate,
				colModUser,
				colModPrg,
				colModHost,
				colHolderLName,
				colHolderFNname,
				colHolderMName
			)
	SELECT 	account ,
			ins_a_b_c ,
			holder_nme ,
			holder_dob ,
			holder_sex ,
			holder_addr ,
			holder_city_st_zip ,
			plan_nme ,
			plan_addr1 ,
			plan_addr2 ,
			p_city_st ,
			policy_num ,
			cert_ssn ,
			grp_nme ,
			grp_num ,
			employer ,
			e_city_st ,
			fin_code ,
			ins_code ,
			relation ,
			mod_date ,
			mod_user ,
			mod_prg ,
			mod_host ,
			holder_lname ,
			holder_fname ,
			holder_mname FROM dbo.ins
			WHERE account = @acc AND deleted = 0 
	
	RETURN 
END
