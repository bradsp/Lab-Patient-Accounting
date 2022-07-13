CREATE TABLE [dbo].[UserProfile] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [UserName]      VARCHAR (50)  NOT NULL,
    [Parameter]     VARCHAR (50)  NOT NULL,
    [ParameterData] VARCHAR (255) NULL,
    [ModDate]       DATETIME      CONSTRAINT [DF_UserProfile_ModDate] DEFAULT (getdate()) NULL,
    [ModUser]       VARCHAR (50)  CONSTRAINT [DF_UserProfile_ModUser] DEFAULT (right(suser_sname(),(50))) NULL,
    [ModPrg]        VARCHAR (50)  CONSTRAINT [DF_UserProfile_ModPrg] DEFAULT (right(app_name(),(50))) NULL,
    [ModHost]       VARCHAR (50)  CONSTRAINT [DF_UserProfile_ModHost] DEFAULT (right(host_name(),(50))) NULL,
    CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED ([Id] ASC)
);

