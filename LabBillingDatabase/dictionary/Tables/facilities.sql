CREATE TABLE [dictionary].[facilities] (
    [facilityNo]   VARCHAR (50)  NOT NULL,
    [facilityName] VARCHAR (100) NULL,
    CONSTRAINT [PK_facilities] PRIMARY KEY CLUSTERED ([facilityNo] ASC) WITH (FILLFACTOR = 90)
);

