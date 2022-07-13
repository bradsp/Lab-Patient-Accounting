CREATE TABLE [dbo].[dict_series_pmt_and_adjustment_codes] (
    [p_m_text] VARCHAR (256) NOT NULL,
    [p_m_code] VARCHAR (7)   NULL,
    [p_m_desc] VARCHAR (256) NULL,
    [p_m_type] VARCHAR (50)  NULL,
    [mod_date] DATETIME      CONSTRAINT [DF_dict_series_pmt_and_adjustment_codes_mod_date] DEFAULT (getdate()) NOT NULL
);

