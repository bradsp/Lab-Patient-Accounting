CREATE TABLE [dbo].[Monthly_Reports] (
    [mi_name]      VARCHAR (50)   NOT NULL,
    [sql_code]     VARCHAR (8000) NOT NULL,
    [report_title] VARCHAR (255)  NULL,
    [comments]     VARCHAR (255)  NULL,
    [button]       VARCHAR (50)   CONSTRAINT [DF_Monthly_Reports_button] DEFAULT ('MAIN') NOT NULL,
    [child_button] BIT            CONSTRAINT [DF_Monthly_Reports_child_button] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Monthly_Reports] PRIMARY KEY CLUSTERED ([mi_name] ASC) WITH (FILLFACTOR = 90)
);

