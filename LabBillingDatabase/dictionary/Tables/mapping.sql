CREATE TABLE [dictionary].[mapping] (
    [return_value]      VARCHAR (MAX) NOT NULL,
    [return_value_type] VARCHAR (50)  NOT NULL,
    [sending_system]    VARCHAR (50)  NULL,
    [sending_value]     VARCHAR (50)  NOT NULL,
    [uid]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [mod_date]          DATETIME      CONSTRAINT [DF_mapping_mod_date] DEFAULT (getdate()) NOT NULL,
    [mod_prg]           VARCHAR (50)  CONSTRAINT [DF_mapping_mod_prg] DEFAULT (right(coalesce(object_name(@@procid),app_name()),(50))) NULL,
    [mod_user]          VARCHAR (50)  CONSTRAINT [DF_mapping_mod_user] DEFAULT (right(suser_sname(),(50))) NULL,
    [mod_host]          VARCHAR (50)  CONSTRAINT [DF_mapping_mod_host] DEFAULT (right(host_name(),(50))) NULL,
    CONSTRAINT [PK_mapping] PRIMARY KEY CLUSTERED ([uid] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_mapping_lookup]
    ON [dictionary].[mapping]([return_value_type] ASC, [sending_system] ASC, [sending_value] ASC) WITH (FILLFACTOR = 90);

