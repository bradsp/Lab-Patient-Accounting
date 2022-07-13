CREATE TABLE [dbo].[client_facility_no] (
    [cl_mnem]    VARCHAR (15) NOT NULL,
    [facilityno] VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_client_facility_no] PRIMARY KEY CLUSTERED ([cl_mnem] ASC, [facilityno] ASC) WITH (FILLFACTOR = 90)
);

