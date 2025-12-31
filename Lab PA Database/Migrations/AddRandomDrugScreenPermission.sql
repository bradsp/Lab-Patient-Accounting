-- Add permission for Random Drug Screen module access
-- This column controls which users can access the Random Drug Screen functionality

ALTER TABLE [dbo].[emp]
ADD [access_random_drug_screen] BIT NOT NULL CONSTRAINT [DF_emp_access_random_drug_screen] DEFAULT ((0));
GO

-- Add extended property for documentation
EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Grants access to the Random Drug Screen module for specimen collection tracking', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'emp', 
    @level2type = N'COLUMN', @level2name = N'access_random_drug_screen';
GO
