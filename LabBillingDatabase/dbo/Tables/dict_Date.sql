CREATE TABLE [dbo].[dict_Date] (
    [ID]           INT          IDENTITY (1, 1) NOT NULL,
    [Date]         DATETIME     NOT NULL,
    [Day]          CHAR (2)     NOT NULL,
    [DaySuffix]    VARCHAR (4)  NOT NULL,
    [DayOfWeek]    VARCHAR (9)  NOT NULL,
    [Month]        CHAR (2)     NOT NULL,
    [MonthName]    VARCHAR (9)  NOT NULL,
    [Quarter]      TINYINT      NOT NULL,
    [QuarterName]  VARCHAR (6)  NOT NULL,
    [Year]         CHAR (4)     NOT NULL,
    [StandardDate] VARCHAR (10) NULL,
    [HolidayText]  VARCHAR (50) NULL,
    [Julian]       INT          NULL,
    CONSTRAINT [PK_dict_Date] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

