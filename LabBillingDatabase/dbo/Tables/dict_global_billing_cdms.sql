CREATE TABLE [dbo].[dict_global_billing_cdms] (
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF__dict_glob__rowgu__703483B9] DEFAULT (newid()) NOT NULL,
    [cdm]             VARCHAR (7)      NOT NULL,
    [comment]         VARCHAR (255)    CONSTRAINT [DF_dict_global_billing_cdms_comment] DEFAULT ('Original contract') NOT NULL,
    [mod_date]        DATETIME         CONSTRAINT [DF__dict_glob__mod_d__7128A7F2] DEFAULT (getdate()) NOT NULL,
    [mod_user]        VARCHAR (50)     CONSTRAINT [DF__dict_glob__mod_u__721CCC2B] DEFAULT (suser_sname()) NOT NULL,
    [mod_prg]         VARCHAR (50)     CONSTRAINT [DF__dict_glob__mod_p__7310F064] DEFAULT (app_name()) NOT NULL,
    [mod_host]        VARCHAR (50)     CONSTRAINT [DF__dict_glob__mod_h__7405149D] DEFAULT (host_name()) NOT NULL,
    [effective_date]  DATETIME         DEFAULT (getdate()) NULL,
    [expiration_date] DATETIME         DEFAULT (NULL) NULL,
    CONSTRAINT [PK__dict_global_bill__6F405F80] PRIMARY KEY CLUSTERED ([rowguid] ASC) WITH (FILLFACTOR = 90)
);

