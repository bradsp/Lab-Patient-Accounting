CREATE TABLE [dbo].[client_supply_cnb] (
    [line_no]    VARCHAR (50)  NULL,
    [descrip]    VARCHAR (MAX) NULL,
    [unit_qty]   VARCHAR (50)  NULL,
    [unit_price] MONEY         NULL,
    [each_price] MONEY         NULL,
    [mod_date]   DATETIME      NULL,
    [mod_user]   VARCHAR (50)  NULL
);

