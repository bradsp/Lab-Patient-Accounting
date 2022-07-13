CREATE TABLE [dbo].[dict_cdm_conversion] (
    [deleted]     BIT          CONSTRAINT [DF_dict_cdm_conversion_deleted] DEFAULT ((0)) NOT NULL,
    [order_code]  VARCHAR (7)  NOT NULL,
    [menm]        VARCHAR (50) NULL,
    [order_cpt]   VARCHAR (5)  NULL,
    [order_link]  INT          NOT NULL,
    [bill_cdm]    VARCHAR (7)  NOT NULL,
    [ins_code]    VARCHAR (50) NULL,
    [ins_name]    VARCHAR (50) NULL,
    [description] VARCHAR (50) NULL,
    [uid]         BIGINT       IDENTITY (1, 1) NOT NULL
);

