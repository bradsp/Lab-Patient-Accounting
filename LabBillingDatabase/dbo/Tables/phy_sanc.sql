CREATE TABLE [dbo].[phy_sanc] (
    [lastname]   VARCHAR (20) NULL,
    [firstname]  VARCHAR (15) NULL,
    [midname]    VARCHAR (15) NULL,
    [busname]    VARCHAR (30) NULL,
    [general]    VARCHAR (20) NULL,
    [specialty]  VARCHAR (20) NULL,
    [upin]       VARCHAR (6)  NULL,
    [npi]        VARCHAR (10) NULL,
    [dob]        VARCHAR (8)  NULL,
    [address]    VARCHAR (30) NULL,
    [city]       VARCHAR (20) NULL,
    [state]      VARCHAR (2)  NULL,
    [zip]        VARCHAR (5)  NULL,
    [sanctype]   VARCHAR (9)  NULL,
    [sancdate]   VARCHAR (8)  NULL,
    [reindate]   VARCHAR (8)  NULL,
    [waiverdate] VARCHAR (8)  NULL,
    [wvrstate]   VARCHAR (50) NULL,
    [uri]        INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_phy_sanc] PRIMARY KEY CLUSTERED ([uri] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_phy_sanc_name]
    ON [dbo].[phy_sanc]([lastname] ASC, [firstname] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_phy_sanc_upin]
    ON [dbo].[phy_sanc]([upin] ASC) WITH (FILLFACTOR = 90);

