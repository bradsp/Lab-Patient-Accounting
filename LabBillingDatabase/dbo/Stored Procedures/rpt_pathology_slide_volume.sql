-- =============================================
-- Author:		Bradley Powers
-- Create date: 2/2/2022
-- Description:	Pathology Slide Report
-- =============================================
CREATE PROCEDURE rpt_pathology_slide_volume
	-- Add the parameters for the stored procedure here
	@startDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @slideCDM TABLE
	(QTY INT,
	cdm VARCHAR(50),
	descript VARCHAR(50),
	cpt4  VARCHAR(50),
	time VARCHAR(50))

	DECLARE @slideCDM2 TABLE
	(QTY INT,
	cdm VARCHAR(50),
	descript VARCHAR(50),
	cpt4  VARCHAR(50))


	DECLARE @command1 varchar(MAX)
	DECLARE @command2 varchar(MAX)
	DECLARE @command3 varchar(MAX)
	set @command1 = (select [value] from dbo.system where key_name = 'slides_ihc_stains')
	set @command2 = (select [value] from dbo.system where key_name = 'slides_slides')
	set @command3 = (select [value] from dbo.system where key_name = 'slides_spec_stains')

	INSERT INTO @slideCDM
	execute(@command1)

	INSERT INTO @slideCDM
	execute(@command3)

	INSERT INTO @slideCDM2
	execute(@command2)

	INSERT INTO @slideCDM (cdm, descript, cpt4)
	select cdm, descript, cpt4 from @slideCDM2

	select 
		 acc.cl_mnem,
		 chrg.cdm,
		 cdm.descript,
		 amt.cpt4,
		 SUM(chrg.qty) AS 'Volume',
		 cast(MONTH(chrg.mod_date) as varchar) + '/' + cast(YEAR(chrg.mod_date) as varchar) as 'Period'
	from chrg join acc on chrg.account = acc.account
	join amt on chrg.chrg_num = amt.chrg_num
	left outer join cdm on chrg.cdm = cdm.cdm
	where 
		chrg.mod_date between @startDate and @endDate
		--and chrg.mod_prg like 'ViewerSlides%'
		and chrg.account like 'D%'
		and chrg.cdm in (select DISTINCT cdm from @slideCDM)
	group by acc.cl_mnem, chrg.cdm, cdm.descript, amt.cpt4, 
	cast(MONTH(chrg.mod_date) as varchar) + '/' + cast(YEAR(chrg.mod_date) as varchar)
	order by 
	cast(MONTH(chrg.mod_date) as varchar) + '/' + cast(YEAR(chrg.mod_date) as varchar),
	chrg.cdm
END
