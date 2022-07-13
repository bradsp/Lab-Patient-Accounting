-- =============================================
-- Author:		Bradley Powers
-- Create date: 08/29/2015
-- Description:	Process MFN interface messages 
--              and update the phy dictionary table
-- =============================================
CREATE PROCEDURE [infce].[usp_mfn_hl7_processor] 
	@systemMsgID NUMERIC = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    BEGIN TRY
		BEGIN TRANSACTION
		;WITH cteMessages
		AS
		(
			SELECT sourceMsgId, systemMsgId, CONVERT(XML,msgContent) AS msgData
			FROM infce.messages_inbound
			WHERE msgType='MFN-M02' AND processFlag = 'N' AND systemMsgId = @systemMsgID
		)
		SELECT 
			M1.msg.value('data(MSH/MSH.7/MSH.7.1/text())[1]','VARCHAR(8)') AS 'MsgDateTime',
			M1.msg.value('data(STF/STF.2[1]/STF.2.1/text())[1]','VARCHAR(50)') AS 'doctor_number',
			M1.msg.value('data(STF/STF.2[2]/STF.2.1/text())[1]','VARCHAR(50)') AS 'NPI number',
			M1.msg.query('//PRA.6[./PRA.6.2/. = "NPI"]').value('data(//PRA.6.1/text())[1]','VARCHAR(50)') AS 'npi',
			M1.msg.query('//PRA.6[./PRA.6.2/. = "UPIN"]').value('data(//PRA.6.1/text())[1]','VARCHAR(50)') AS 'upin',
			M1.msg.query('//PRA.6[./PRA.6.2/. = "STLIC"]').value('data(//PRA.6.1/text())[1]','VARCHAR(50)') AS 'stlic',
			M1.msg.query('//PRA.6[./PRA.6.2/. = "FDRUG"]').value('data(//PRA.6.1/text())[1]','VARCHAR(50)') AS 'fdrug',
			M1.msg.query('//PRA.6[./PRA.6.2/. = "FEDLIC"]').value('data(//PRA.6.1/text())[1]','VARCHAR(50)') AS 'fedlic',
			REPLACE(M1.msg.value('data(STF/STF.3/STF.3.1/text())[1]','VARCHAR(50)'),'.','') AS 'last_name',
			REPLACE(M1.msg.value('data(STF/STF.3/STF.3.2/text())[1]','VARCHAR(50)'),'.','') AS 'first_name',
			REPLACE(M1.msg.value('data(STF/STF.3/STF.3.3/text())[1]','VARCHAR(50)'),'.','') AS 'mid_name',
			REPLACE(M1.msg.value('data(STF/STF.3/STF.3.5/text())[1]','VARCHAR(50)'),'.','') AS 'suffix',
			M1.msg.value('data(STF/STF.10/STF.10.1/text())[1]','VARCHAR(50)') AS 'phone',
			M1.msg.value('data(STF/STF.10/STF.10.1/text())[2]','VARCHAR(50)') AS 'cell',
			M1.msg.value('data(STF/STF.11/STF.11.1/text())[1]','VARCHAR(50)') AS 'address',
			M1.msg.value('data(STF/STF.11/STF.11.3/text())[1]','VARCHAR(50)') AS 'city',
			M1.msg.value('data(STF/STF.11/STF.11.4/text())[1]','VARCHAR(50)') AS 'state',
			M1.msg.value('data(STF/STF.11/STF.11.5/text())[1]','VARCHAR(50)') AS 'zip',
			cteMessages.sourceMsgId,
			cteMessages.systemMsgId
			INTO #parsed
		FROM cteMessages
		CROSS APPLY msgData.nodes('/HL7Message') AS M1(msg)

		SELECT * INTO #update
		FROM #parsed WHERE #parsed.[NPI number] IN (SELECT tnh_num FROM phy) 

		SELECT * INTO #insert 
		FROM #parsed WHERE #parsed.[NPI number] NOT IN (SELECT tnh_num FROM phy)

		IF EXISTS (SELECT [NPI Number] FROM #insert)
		BEGIN
			INSERT phy
					( upin ,
					  tnh_num ,
					  billing_npi ,
					  last_name ,
					  first_name ,
					  mid_init ,
					  addr_1 ,
					  city ,
					  state ,
					  zip ,
					  phone ,
					  docnbr,
					  deleted
					)
			SELECT upin, [NPI number],[NPI number], REPLACE(last_name,'UTFM - ',''), first_name, mid_name, [address], city, [state], zip, phone, doctor_number,0  
			FROM #insert

			--ALSO MAKE SURE doctor number is mapped to NPI in dictionary.mapping
			INSERT INTO dictionary.mapping
					( return_value ,
					  return_value_type ,
					  sending_system ,
					  sending_value
					)
			SELECT [NPI number], 'PHY_ID','CERNER',[NPI number] FROM #insert

			INSERT INTO dictionary.mapping
					( return_value ,
					  return_value_type ,
					  sending_system ,
					  sending_value
					)
			SELECT [NPI number], 'PHY_ID','CERNER',doctor_number FROM #insert
		END

		IF EXISTS (SELECT [NPI Number] FROM #update)
		BEGIN
			UPDATE phy 
			SET deleted = 0, 
				last_name = REPLACE(#update.last_name,'UTFM - ',''), 
				first_name = #update.first_name, 
				mid_init = #update.mid_name,
				addr_1 = #update.[address], 
				city = #update.city, 
				[state] = #update.[state], 
				zip = #update.zip, 
				phone = #update.phone, 
				docnbr = #update.doctor_number,
				upin = #update.upin
			FROM phy JOIN #update ON phy.tnh_num = #update.[NPI number] 
		END

		IF EXISTS (SELECT systemMsgId FROM #parsed)
		BEGIN
			UPDATE infce.messages_inbound SET processFlag = 'F'
			--SELECT * FROM infce.messages_inbound
			WHERE systemMsgId = @systemMsgID
		END

		COMMIT
		
		DROP TABLE #insert
		DROP TABLE #update
		DROP TABLE #parsed

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK
		DECLARE @ErrMsg NVARCHAR(4000), @ErrSeverity INT
		SELECT @ErrMsg = ERROR_MESSAGE(), @ErrSeverity = ERROR_SEVERITY()
		UPDATE infce.messages_inbound SET processFlag = 'E', processStatusMsg = @ErrMsg
		WHERE systemMsgId = @systemMsgID
		--RAISERROR(@ErrMsg,@ErrSeverity, 1)
	END CATCH
END
