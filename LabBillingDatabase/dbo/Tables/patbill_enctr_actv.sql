CREATE TABLE [dbo].[patbill_enctr_actv] (
    [statement_number]          FLOAT (53)      NOT NULL,
    [record_type]               VARCHAR (4)     NOT NULL,
    [record_cnt]                BIGINT          NOT NULL,
    [enctr_nbr]                 VARCHAR (15)    NOT NULL,
    [activity_id]               VARCHAR (50)    NOT NULL,
    [activity_type]             VARCHAR (1)     NOT NULL,
    [activity_date]             VARCHAR (10)    NULL,
    [activity_description]      VARCHAR (9)     NOT NULL,
    [activity_code]             VARCHAR (1)     NOT NULL,
    [activity_amount]           VARCHAR (1)     NOT NULL,
    [units]                     NUMERIC (4)     NULL,
    [cpt_code]                  VARCHAR (7)     NULL,
    [cpt_description]           VARCHAR (50)    NULL,
    [drg_code]                  VARCHAR (1)     NOT NULL,
    [revenue_code]              VARCHAR (5)     NULL,
    [revenue_code_description]  VARCHAR (1)     NOT NULL,
    [hcpcs_code]                VARCHAR (1)     NOT NULL,
    [hcpcs_description]         VARCHAR (1)     NOT NULL,
    [order_mgmt_activity_type]  VARCHAR (1)     NOT NULL,
    [activity_amount_due]       MONEY           NULL,
    [activity_date_of_service]  VARCHAR (10)    NULL,
    [activity_patient_bal]      NUMERIC (2, 2)  NOT NULL,
    [activity_ins_discount]     NUMERIC (2, 2)  NOT NULL,
    [activity_trans_type]       VARCHAR (6)     NOT NULL,
    [activity_trans_sub_type]   VARCHAR (1)     NOT NULL,
    [activity_trans_amount]     NUMERIC (24, 4) NULL,
    [activity_health_plan_name] VARCHAR (1)     NOT NULL,
    [activity_ins_pending]      NUMERIC (2, 2)  NOT NULL,
    [activity_dr_cr_flag]       INT             NOT NULL,
    [parent_activity_id]        BIGINT          NOT NULL,
    [batch_id]                  VARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_patbill_enctr_actv] PRIMARY KEY CLUSTERED ([statement_number] ASC, [record_type] ASC, [record_cnt] ASC, [enctr_nbr] ASC, [activity_id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_statement_batch]
    ON [dbo].[patbill_enctr_actv]([statement_number] ASC, [batch_id] ASC) WITH (FILLFACTOR = 90);

