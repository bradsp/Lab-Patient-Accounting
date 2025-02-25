﻿CREATE TABLE [dbo].[patbill_stmt] (
    [record_type]                    VARCHAR (4)     NOT NULL,
    [record_cnt]                     BIGINT          NOT NULL,
    [statement_number]               FLOAT (53)      NOT NULL,
    [billing_entity_street]          VARCHAR (8000)  NULL,
    [billing_entity_city]            VARCHAR (8000)  NULL,
    [billing_entity_state]           VARCHAR (8000)  NULL,
    [billing_entity_zip]             VARCHAR (8000)  NULL,
    [billing_entity_federal_tax_id]  VARCHAR (8000)  NULL,
    [billing_entity_name]            VARCHAR (8000)  NULL,
    [billing_entity_phone_number]    VARCHAR (8000)  NULL,
    [billing_entity_fax_number]      VARCHAR (8000)  NULL,
    [remit_to_street]                VARCHAR (8000)  NULL,
    [remit_to_street2]               VARCHAR (8000)  NULL,
    [remit_to_city]                  VARCHAR (8000)  NULL,
    [remit_to_state]                 VARCHAR (8000)  NULL,
    [remit_to_zip]                   VARCHAR (8000)  NULL,
    [remit_to_org_name]              VARCHAR (8000)  NULL,
    [guarantor_street]               VARCHAR (40)    NULL,
    [guarantor_street_2]             VARCHAR (1)     NOT NULL,
    [guarantor_city]                 VARCHAR (50)    NULL,
    [guarantor_state]                VARCHAR (50)    NULL,
    [guarantor_zip]                  VARCHAR (50)    NULL,
    [guarantor_name]                 VARCHAR (128)   NULL,
    [amount_due]                     NUMERIC (38, 2) NULL,
    [date_due]                       VARCHAR (30)    NULL,
    [balance_forward]                NUMERIC (18, 2) NULL,
    [aging_bucket_current]           MONEY           NULL,
    [aging_bucket_30_day]            MONEY           NULL,
    [aging_bucket_60_day]            MONEY           NULL,
    [aging_bucket_90_day]            MONEY           NULL,
    [statement_total_amount]         NUMERIC (38, 2) NULL,
    [insurance_billed_amount]        VARCHAR (1)     NOT NULL,
    [balance_forward_amount]         VARCHAR (1)     NOT NULL,
    [balance_forward_date]           VARCHAR (1)     NOT NULL,
    [primary_health_plan_name]       VARCHAR (1)     NOT NULL,
    [prim_health_plan_policy_number] VARCHAR (1)     NOT NULL,
    [prim_health_plan_group_number]  VARCHAR (1)     NOT NULL,
    [secondary_health_plan_name]     VARCHAR (1)     NOT NULL,
    [sec_health_plan_policy_number]  VARCHAR (1)     NOT NULL,
    [sec_health_plan_group_number]   VARCHAR (1)     NOT NULL,
    [tertiary_health_plan_name]      VARCHAR (1)     NOT NULL,
    [ter_health_plan_policy_number]  VARCHAR (1)     NOT NULL,
    [ter_health_plan_group_number]   VARCHAR (1)     NOT NULL,
    [statement_time]                 VARCHAR (5)     NOT NULL,
    [page_number]                    VARCHAR (1)     NOT NULL,
    [insurance_pending]              NUMERIC (2, 2)  NOT NULL,
    [unpaid_balance]                 NUMERIC (38, 2) NULL,
    [patient_balance]                NUMERIC (18, 2) NULL,
    [totat_paid_since_last_stmt]     NUMERIC (38, 2) NULL,
    [insurance_discount]             NUMERIC (2, 2)  NOT NULL,
    [contact text]                   VARCHAR (1)     NOT NULL,
    [transmission_dt_tm]             VARCHAR (1)     NOT NULL,
    [guarantor_country]              VARCHAR (3)     NOT NULL,
    [guarantor_ssn]                  VARCHAR (1)     NOT NULL,
    [guarantor_phone]                VARCHAR (25)    NULL,
    [statement_submitted_dt_tm]      DATETIME        NULL,
    [includes_est_pat_lib]           INT             NOT NULL,
    [total_charge_amount]            NUMERIC (38, 2) NULL,
    [non_covered_charge_amt]         INT             NOT NULL,
    [ABN_charge_amt]                 INT             NOT NULL,
    [est_contract_allowance_amt_ind] INT             NOT NULL,
    [est_contract_allowance_amt]     INT             NOT NULL,
    [encntr_deductible_rem_amt_ind]  INT             NOT NULL,
    [deductible_applied_amt]         INT             NOT NULL,
    [encntr_copay_amt_ind]           INT             NOT NULL,
    [encntr_copay_amt]               INT             NOT NULL,
    [encntr_coinsurance_pct_ind]     INT             NOT NULL,
    [encntr_coinsurance_pct]         INT             NOT NULL,
    [encntr_coinsurance_amt]         INT             NOT NULL,
    [maximum_out_of_pocket_amt_ind]  INT             NOT NULL,
    [amt_over_max_out_of_pocket]     INT             NOT NULL,
    [est_patient_liab_amt]           INT             NOT NULL,
    [online_billpay_url]             VARCHAR (1)     NOT NULL,
    [guarantor_access_code]          VARCHAR (1)     NOT NULL,
    [batch_id]                       VARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_patbill_stmt_1] PRIMARY KEY CLUSTERED ([record_type] ASC, [record_cnt] ASC, [statement_number] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_statement_number]
    ON [dbo].[patbill_stmt]([statement_number] ASC) WITH (FILLFACTOR = 90);

