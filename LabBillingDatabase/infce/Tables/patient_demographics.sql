CREATE TABLE [infce].[patient_demographics] (
    [enctr_sys]         VARCHAR (25)  NULL,
    [client]            VARCHAR (MAX) NULL,
    [account]           VARCHAR (16)  NULL,
    [service_date]      VARCHAR (8)   NULL,
    [acc_outreach]      VARCHAR (50)  NULL,
    [acc_fin_nbr]       VARCHAR (50)  NULL,
    [systemMsgId]       VARCHAR (36)  NOT NULL,
    [hne]               VARCHAR (25)  NULL,
    [mrn]               VARCHAR (25)  NULL,
    [mrn_outreach]      VARCHAR (50)  NULL,
    [outreach_personID] VARCHAR (50)  NULL,
    [PATIENT]           VARCHAR (133) NULL,
    [pat_dob]           VARCHAR (8)   NULL,
    [visit_id]          VARCHAR (40)  NULL,
    [xmlContents]       XML           NULL,
    CONSTRAINT [PK_patient_demographics] PRIMARY KEY CLUSTERED ([systemMsgId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE PRIMARY XML INDEX [idx_xmlContent]
    ON [infce].[patient_demographics]([xmlContents])
    WITH (PAD_INDEX = OFF, FILLFACTOR = 90);

