-- =============================================
-- Author:		David Kelly
-- Create date: 11/20/2008
-- Description:	Copy billing data to Test for use with applications for testing
-- =============================================
CREATE PROCEDURE [dbo].[usp_CopyBillingToTest] 
	-- Add the parameters for the stored procedure here
	@account varchar(15) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT @account

	insert into MCLTEST.dbo.acc
	(rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, 
						  mod_date, mod_user, mod_prg, oereqno, mri, mod_host)

	select rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, 
						  mod_date, mod_user, mod_prg, oereqno, mri, mod_host 
	from acc where account = @account
	
	insert into MCLTEST.dbo.pat
	( rowguid, deleted, account, ssn, pat_addr1, pat_addr2, city_st_zip, dob_yyyy, sex, relation, guarantor, guar_addr, g_city_st, pat_marital, icd9_1, 
                      icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9, pc_code, mailer, first_dm, last_dm, min_amt, phy_id, dbill_date, ub_date, h1500_date, 
                      colltr_date, baddebt_date, batch_date, guar_phone, bd_list_date, ebill_batch_date, ebill_batch_1500, e_ub_demand, e_ub_demand_date, 
                      claimsnet_1500_batch_date, claimsnet_ub_batch_date, mod_date, mod_user, mod_prg, mod_host)
	select  rowguid, deleted, account, ssn, pat_addr1, pat_addr2, city_st_zip, dob_yyyy, sex, relation, guarantor, guar_addr, g_city_st, pat_marital, icd9_1, 
                      icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9, pc_code, mailer, first_dm, last_dm, min_amt, phy_id, dbill_date, ub_date, h1500_date, 
                      colltr_date, baddebt_date, batch_date, guar_phone, bd_list_date, ebill_batch_date, ebill_batch_1500, e_ub_demand, e_ub_demand_date, 
                      claimsnet_1500_batch_date, claimsnet_ub_batch_date, mod_date, mod_user, mod_prg, mod_host
	from pat where account = @account
	
	insert into MCLTEST.dbo.ins
	(  rowguid, deleted, account, ins_a_b_c, holder_nme, plan_nme, plan_addr1, plan_addr2, p_city_st, policy_num, cert_ssn, grp_nme, grp_num, 
                      holder_sex, employer, e_city_st, fin_code, ins_code, mod_date, mod_user, mod_prg, mod_host)
	select   rowguid, deleted, account, ins_a_b_c, holder_nme, plan_nme, plan_addr1, plan_addr2, p_city_st, policy_num, cert_ssn, grp_nme, grp_num, 
                      holder_sex, employer, e_city_st, fin_code, ins_code, mod_date, mod_user, mod_prg, mod_host
	from ins where account = @account


	insert into MCLTEST.dbo.chrg
	(rowguid, credited, /*chrg_num,is the identity field so can't add*/
	 account, status, service_date, hist_date, cdm, qty, retail, inp_price, comment, invoice, mod_date, mod_user, mod_prg, 
                      net_amt, fin_type, mod_host)
	select rowguid, credited, /*chrg_num,*/ account, status, service_date, hist_date, cdm, qty, retail, inp_price, comment, invoice, mod_date, mod_user, mod_prg, 
                      net_amt, fin_type, mod_host
	from chrg where account = @account; -- must end with the semicolon because of the cte below

-- amt table is handled jussssst a bit different. The charge record has a new chrg_num (key field)
-- so get the new charge number and use it as the link to the new amt records.
-- simple right?
	with cte (chrg_num, account, cdm)
	AS
	(
		select chrg_num, account, cdm
		from MCLTEST.dbo.chrg
		where account = @account
	)
	insert into MCLTEST.dbo.amt
	( chrg_num, cpt4, type, amount, mod_date, mod_user, mod_prg, deleted, /*uri,-- identify field*/
		 modi, revcode, modi2)
	select  cte.chrg_num as [chrg_num], amt.cpt4, amt.type, amt.amount, amt.mod_date, 
		amt.mod_user, amt.mod_prg, amt.deleted, /*uri,-- identify field*/
		 amt.modi, amt.revcode, amt.modi2
	from chrg as c 
	join cte on c.account = cte.account and c.cdm = cte.cdm
	join amt on c.chrg_num = amt.chrg_num


	insert into MCLTEST.dbo.chk
	(rowguid, deleted, /*pay_no, key field*/ account, chk_date, date_rec, chk_no, amt_paid, write_off, contractual, status, source, w_off_date, invoice, batch, comment, 
                      bad_debt, mod_date, mod_user, mod_prg, mod_host, mod_date_audit, cpt4Code, post_file, chrg_rowguid, write_off_code, eft_date, eft_number)
	select rowguid, deleted, /*pay_no,*/ account, chk_date, date_rec, chk_no, amt_paid, write_off, contractual, status, source, w_off_date, invoice, batch, comment, 
                      bad_debt, mod_date, mod_user, mod_prg, mod_host, mod_date_audit, cpt4Code, post_file, chrg_rowguid, write_off_code, eft_date, eft_number
	from chk where account = @account

	insert into MCLTEST.dbo.h1500
	(rowguid, deleted, account, ins_abc, pat_name, fin_code, claimsnet_payer_id, trans_date, run_date, printed, run_user, batch, date_sent, sent_user, 
                      ebill_status, ebill_batch, text, cold_feed, mod_date, mod_user, mod_prg, mod_host)
	select rowguid, deleted, account, ins_abc, pat_name, fin_code, claimsnet_payer_id, trans_date, run_date, printed, run_user, batch, date_sent, sent_user, 
                      ebill_status, ebill_batch, text, cold_feed, mod_date, mod_user, mod_prg, mod_host
	from h1500 where account = @account


	insert into MCLTEST.dbo.ub
	( /*rowguid,key field*/ deleted, account, ins_abc, run_date, printed, run_user, fin_code, trans_date, pat_name, claimsnet_payer_id, ebill_status, batch, text, 
                      edited_ub, cold_feed, mod_date, mod_user, mod_prg, mod_host)
	select  /*rowguid,*/ deleted, account, ins_abc, run_date, printed, run_user, fin_code, trans_date, pat_name, claimsnet_payer_id, ebill_status, batch, text, 
                      edited_ub, cold_feed, mod_date, mod_user, mod_prg, mod_host
	from ub where account = @account


END
