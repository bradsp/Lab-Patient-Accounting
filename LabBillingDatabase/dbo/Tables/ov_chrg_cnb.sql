CREATE TABLE [dbo].[ov_chrg_cnb] (
    [the_msg_no] NCHAR (15)   NOT NULL,
    [account]    VARCHAR (50) NULL,
    [trans_date] DATETIME     NOT NULL,
    [test]       VARCHAR (50) NOT NULL,
    [qty]        INT          NOT NULL,
    [file_nme]   VARCHAR (50) NOT NULL,
    [mod_date]   DATETIME     NOT NULL,
    [mod_user]   VARCHAR (50) NOT NULL,
    [run_date]   DATETIME     NULL,
    CONSTRAINT [PK_ov_chrg_cnb] PRIMARY KEY CLUSTERED ([the_msg_no] ASC) WITH (FILLFACTOR = 90)
);

