
CREATE VIEW [app].[person]
as
select DISTINCT HNE_NUMBER, mri, acc.pat_name, acc.ssn, pat.dob_yyyy, pat.sex 
from acc left outer join pat on acc.account = pat.account
where acc.deleted = 0
