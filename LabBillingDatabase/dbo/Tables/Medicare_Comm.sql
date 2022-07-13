CREATE TABLE [dbo].[Medicare_Comm] (
    [ins_name]    VARCHAR (25) NOT NULL,
    [fin_code]    VARCHAR (1)  NOT NULL,
    [is_medicare] BIT          CONSTRAINT [DF__Medicare___is_me__621B6E8C] DEFAULT ((0)) NULL,
    [mod_date]    DATETIME     CONSTRAINT [DF__Medicare___mod_d__630F92C5] DEFAULT (CONVERT([varchar](10),getdate(),(101))) NOT NULL,
    CONSTRAINT [PK_Medicare_Comm_L] PRIMARY KEY CLUSTERED ([ins_name] ASC, [fin_code] ASC) WITH (FILLFACTOR = 90)
);

