
-- Create a stored procedure that will cause an 
-- object resolution error.
CREATE PROCEDURE usp_ExampleProc
AS
    SELECT * FROM NonexistentTable;
