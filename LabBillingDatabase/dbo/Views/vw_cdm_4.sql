/****** Object:  View dbo.vw_cdm_4    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_cdm_4]
AS
SELECT     dbo.cdm.cdm, cpt4.cpt4, cdm.descript AS cdm_desc, cpt4.descript AS cpt4_desc, cdm.mtype, cdm.ctype, cdm.ztype, 
                      cpt4.mprice, cpt4.cprice, cpt4.zprice, cdm.m_pa_amt, cdm.c_pa_amt, cdm.z_pa_amt, cpt4.type, cpt4.link, 
                      cpt4.modi, cpt4.rev_code
FROM         dbo.cdm INNER JOIN
                      cpt4_4 as cpt4 ON cdm.cdm = cpt4.cdm
WHERE     (cdm.deleted = 0) AND (cpt4.deleted = 0)
