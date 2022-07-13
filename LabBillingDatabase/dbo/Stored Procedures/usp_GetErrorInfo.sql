
-- Create procedure to retrieve error information.
CREATE PROCEDURE usp_GetErrorInfo
AS
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() as ErrorState,
        ERROR_LINE () as ErrorLine,
        ERROR_PROCEDURE() as ErrorProcedure,
        ERROR_MESSAGE() as ErrorMessage;
