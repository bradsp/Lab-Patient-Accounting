/****** Object:  View dbo.vw_phy_sanc    Script Date: 9/19/2001 10:47:04 AM ******/
CREATE VIEW [dbo].[vw_phy_sanc] AS
/*
	vw_phy_sanc 09/12/00 Rick Crone
	This view selects the records from the phy_sanc table
	that have a upin number.
	The record set RVW_PHY_SANC in our MCL library includes
	a GetRecord() function to allow our applications to 
	easily check to see if a physician is on the list.
	note: The phy_sanc data comes from a dBASE III file that is 
	down loaded from:
	    http://hhs.gov/progorg/oig/cumsan/2000/index.htm
*/
select * from phy_sanc
where upin is NOT NULL
