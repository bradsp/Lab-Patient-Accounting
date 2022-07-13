/****** Object:  View dbo.vw_cdm_with_del    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_cdm_with_del]
AS
SELECT     dbo.cdm.cdm, dbo.cpt4.cpt4, dbo.cdm.descript AS cdm_desc, dbo.cpt4.descript AS cpt4_desc, dbo.cdm.mtype, dbo.cdm.ctype, dbo.cdm.ztype, 
                      dbo.cpt4.mprice, dbo.cpt4.cprice, dbo.cpt4.zprice, dbo.cdm.m_pa_amt, dbo.cdm.c_pa_amt, dbo.cdm.z_pa_amt, dbo.cpt4.type, dbo.cpt4.link, 
                      dbo.cpt4.modi, dbo.cpt4.rev_code
FROM         dbo.cdm INNER JOIN
                      dbo.cpt4 ON dbo.cdm.cdm = dbo.cpt4.cdm
