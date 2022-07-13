CREATE TABLE [dbo].[dict_cpt4_reimbursements] (
    [fin_code]         VARCHAR (50)    NOT NULL,
    [cpt4]             VARCHAR (5)     NOT NULL,
    [max_payment]      DECIMAL (18, 2) NOT NULL,
    [max_payment_date] DATETIME        NOT NULL,
    [min_payment]      DECIMAL (18, 2) NOT NULL,
    [min_payment_date] DATETIME        NOT NULL,
    [number]           INT             NOT NULL,
    [total_payments]   DECIMAL (18, 2) NOT NULL
);

