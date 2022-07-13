CREATE TABLE [dbo].[patbill_acc] (
    [statement_number]               FLOAT (53)      NULL,
    [record_cnt_acct]                VARCHAR (30)    NULL,
    [patient_account_number]         VARCHAR (15)    NOT NULL,
    [account_id]                     VARCHAR (50)    NOT NULL,
    [pat_name]                       VARCHAR (40)    NULL,
    [account_subtotal]               VARCHAR (8)     NOT NULL,
    [total_account_subtotal]         NUMERIC (18, 2) NULL,
    [acct_amt_due]                   NUMERIC (18, 2) NULL,
    [acct_ins_pending]               NUMERIC (2, 2)  NOT NULL,
    [acct_dates_of_service]          VARCHAR (30)    NULL,
    [acct_unpaid_bal]                NUMERIC (18, 2) NULL,
    [acct_patient_bal]               NUMERIC (18, 2) NULL,
    [acct_paid_since_last_stmt]      NUMERIC (18, 2) NULL,
    [acct_ins_discount]              NUMERIC (18, 2) NULL,
    [acct_date_due]                  VARCHAR (30)    NULL,
    [acct_health_plan_name]          VARCHAR (45)    NULL,
    [patient_date_of_birth]          VARCHAR (30)    NULL,
    [patient_date_of_death]          VARCHAR (1)     NOT NULL,
    [patient_sex]                    VARCHAR (6)     NULL,
    [patient_vip]                    VARCHAR (1)     NOT NULL,
    [includes_est_pat_lib]           INT             NOT NULL,
    [total_charge_amt]               NUMERIC (18, 2) NULL,
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
    [acc_msg]                        VARCHAR (9)     NOT NULL,
    [mailer]                         VARCHAR (1)     NULL,
    [first_data_mailer]              DATETIME        NULL,
    [last_data_mailer]               DATETIME        NULL,
    [mailer_count]                   INT             NULL,
    [processed_date]                 DATETIME        NULL,
    [date_sent]                      DATETIME        NULL,
    [aging_bucket_current]           MONEY           NULL,
    [aging_bucket_30]                MONEY           NULL,
    [aging_bucket_60]                MONEY           NULL,
    [aging_bucket_90]                MONEY           NULL,
    [batch_id]                       VARCHAR (50)    NOT NULL,
    [uid]                            BIGINT          IDENTITY (1, 1) NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_STATEMENT]
    ON [dbo].[patbill_acc]([batch_id] ASC, [statement_number] ASC, [record_cnt_acct] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [IX_batch_id_datesent]
    ON [dbo].[patbill_acc]([batch_id] ASC, [date_sent] ASC, [account_id] ASC)
    INCLUDE([statement_number]) WITH (FILLFACTOR = 90);

