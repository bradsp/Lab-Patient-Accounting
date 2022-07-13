/****** Object:  Stored Procedure dbo.sp_delacc    Script Date: 9/19/2001 10:47:26 AM ******/
CREATE PROCEDURE [dbo].[sp_delacc_no_longer_used] @account varchar(15) AS
--DELETE acc_comment FROM acc_comment where account = @account // wdk 02/20/2007  removed
DELETE amt FROM chrg,amt
 where amt.chrg_num = chrg.chrg_num and chrg.account = @account
DELETE chrg_rev_trk FROM chrg,chrg_rev_trk crt
 where crt.chrg_num = chrg.chrg_num and chrg.account = @account
DELETE chrg_pa FROM chrg,chrg_pa
 where chrg_pa.chrg_num = chrg.chrg_num and chrg.account = @account
DELETE chrg where chrg.account = @account
DELETE h1500 where account = @account
DELETE ub where account = @account
DELETE dbill where account = @account
DELETE pat where account = @account
DELETE ins where account = @account
DELETE chk where account = @account
DELETE abn where account = @account
DELETE acc where account = @account
