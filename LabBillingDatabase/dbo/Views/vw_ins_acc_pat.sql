/****** Object:  View dbo.vw_ins_acc_pat    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_ins_acc_pat] AS
select ins.account,ins.policy_num,acc.trans_date,pat.ub_date,pat.ebill_batch_date,pat.batch_date
from ins JOIN acc on acc.account = ins.account
JOIN pat on ins.account = pat.account
