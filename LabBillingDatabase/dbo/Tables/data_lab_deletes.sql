CREATE TABLE [dbo].[data_lab_deletes] (
    [cdm ]              VARCHAR (7)    NULL,
    [Description]       NVARCHAR (255) NULL,
    [ CPT/HCPCS Code]   FLOAT (53)     NULL,
    [rev code]          FLOAT (53)     NULL,
    [YTD Volume]        FLOAT (53)     NULL,
    [BP HCPCS Comments] NVARCHAR (MAX) NULL
);


GO
CREATE CLUSTERED INDEX [IX_data_lab_deletes]
    ON [dbo].[data_lab_deletes]([cdm ] ASC, [ CPT/HCPCS Code] ASC) WITH (FILLFACTOR = 90);

