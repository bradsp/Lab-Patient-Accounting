CREATE TABLE [dbo].[tblPropAccCrossover] (
    [propPK]            INT           NOT NULL,
    [propFincode]       VARCHAR (10)  NULL,
    [propEnctrType]     VARCHAR (20)  NULL,
    [propType]          VARCHAR (10)  NULL,
    [propAcc]           VARCHAR (15)  NOT NULL,
    [propTDate]         DATETIME      NULL,
    [propPatName]       VARCHAR (125) NULL,
    [propHospAcc]       VARCHAR (50)  NULL,
    [propHneNumber]     VARCHAR (50)  NULL,
    [propAdmitDate]     DATETIME      NULL,
    [propDischargeDate] DATETIME      NULL,
    [mod_date]          DATETIME      CONSTRAINT [DF_tblPropAccCrossover_mod_date] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_tblPropAccCrossover_1] PRIMARY KEY CLUSTERED ([propPK] ASC, [propAcc] ASC) WITH (FILLFACTOR = 90)
);

