CREATE VIEW dbo.dict_ViewerAccSql
as

select v1.type_check, v2.fin_code, v2.ins_code, v2.bill_form, v1.valid, v1.strSql, v2.effective_date, v2.expire_date, v1.error, v2.mod_date, v2.mod_prg, v2.mod_user, v2.mod_host, v2.uid
from dict_acc_validation v1 join dict_acc_validation_criteria v2
	on v1.rule_id = v2.rule_id;

