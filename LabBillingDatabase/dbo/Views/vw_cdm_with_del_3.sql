/****** Object:  View dbo.vw_cdm_with_del_3    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_cdm_with_del_3]
AS
SELECT     dbo.cdm.cdm, cpt4.cpt4, dbo.cdm.descript AS cdm_desc, cpt4.descript AS cpt4_desc, dbo.cdm.mtype, dbo.cdm.ctype, dbo.cdm.ztype, 
                      cpt4.mprice, cpt4.cprice, cpt4.zprice, dbo.cdm.m_pa_amt, dbo.cdm.c_pa_amt, dbo.cdm.z_pa_amt, cpt4.type, cpt4.link, 
                      cpt4.modi, cpt4.rev_code
FROM         dbo.cdm INNER JOIN
                      dbo.cpt4_3 as cpt4 ON dbo.cdm.cdm = cpt4.cdm
