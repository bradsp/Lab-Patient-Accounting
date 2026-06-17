--
-- PostgreSQL database dump
--

\restrict lHVt40WNexIsxfKp0Htf0hAaRtaIueOksbgKbXR86If3VTuWW0KTJIKNlvKSxfm

-- Dumped from database version 17.10 (Debian 17.10-1.pgdg13+1)
-- Dumped by pg_dump version 17.10 (Debian 17.10-1.pgdg13+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: audit; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA audit;


--
-- Name: dbo; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA dbo;


--
-- Name: dict; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA dict;


--
-- Name: dictionary; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA dictionary;


--
-- Name: infce; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA infce;


--
-- Name: tst; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA tst;


--
-- Name: zzz; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA zzz;


--
-- Name: uuid-ossp; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA public;


--
-- Name: EXTENSION "uuid-ossp"; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION "uuid-ossp" IS 'generate universally unique identifiers (UUIDs)';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: audit_acc; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_acc (
    acc_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    cl_mnem character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    cbill_date timestamp without time zone,
    status character varying(8) DEFAULT NULL::character varying,
    ssn character varying(11) DEFAULT NULL::character varying,
    num_comments integer,
    meditech_account character varying(15) DEFAULT NULL::character varying,
    original_fincode character varying(1) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    oereqno character varying(15) DEFAULT NULL::character varying,
    mri character varying(25) DEFAULT NULL::character varying,
    mod_host character varying(50) NOT NULL,
    uid bigint NOT NULL,
    post_date timestamp without time zone,
    ov_order_id character varying(50) DEFAULT NULL::character varying,
    ov_pat_id character varying(50) DEFAULT NULL::character varying,
    "guarantorID" character varying(50) DEFAULT NULL::character varying,
    "HNE_NUMBER" character varying(50) DEFAULT NULL::character varying,
    tdate_update boolean
);


--
-- Name: audit_aging_history; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_aging_history (
    account character varying(15) NOT NULL,
    datestamp timestamp without time zone,
    balance numeric(19,4) DEFAULT NULL::numeric,
    uid bigint NOT NULL,
    mod_indicator character varying(10) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    ins_code character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying
);


--
-- Name: audit_amt; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_amt (
    audit_chrg_num numeric NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    type character varying(6) DEFAULT NULL::character varying,
    amount numeric(19,4) DEFAULT NULL::numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mod_indicator character varying(50) DEFAULT NULL::character varying,
    deleted boolean DEFAULT true NOT NULL,
    amt_uri numeric,
    modi character varying(5) DEFAULT NULL::character varying,
    revcode character varying(5) DEFAULT NULL::character varying,
    modi2 character varying(5) DEFAULT NULL::character varying,
    uri numeric NOT NULL,
    diagnosis_code_ptr character varying(50) DEFAULT NULL::character varying,
    pointer_set boolean,
    account character varying(15) DEFAULT NULL::character varying,
    bill_method character varying(50) DEFAULT NULL::character varying
);


--
-- Name: audit_bad_debt; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_bad_debt (
    baddebt_rowguid uuid,
    deleted boolean,
    debtor_last_name character varying(20) DEFAULT NULL::character varying,
    debtor_first_name character varying(15) DEFAULT NULL::character varying,
    st_addr_1 character varying(25) DEFAULT NULL::character varying,
    st_addr_2 character varying(25) DEFAULT NULL::character varying,
    city character varying(18) DEFAULT NULL::character varying,
    state_zip character varying(15) DEFAULT NULL::character varying,
    spouse character varying(15) DEFAULT NULL::character varying,
    phone character varying(12) DEFAULT NULL::character varying,
    soc_security character varying(10) DEFAULT NULL::character varying,
    license_number character varying(20) DEFAULT NULL::character varying,
    employment character varying(35) DEFAULT NULL::character varying,
    remarks character varying(35) DEFAULT NULL::character varying,
    account_no character varying(25) NOT NULL,
    patient_name character varying(20) DEFAULT NULL::character varying,
    remarks2 character varying(35) DEFAULT NULL::character varying,
    misc character varying(29) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    payment_date timestamp without time zone,
    balance numeric(19,4) DEFAULT NULL::numeric,
    date_entered timestamp without time zone,
    date_sent timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL
);


--
-- Name: audit_cdm; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_cdm (
    deleted boolean,
    cdm character varying(7) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mtype character varying(10) DEFAULT NULL::character varying,
    m_pa_amt numeric(19,4) DEFAULT NULL::numeric,
    ctype character varying(10) DEFAULT NULL::character varying,
    c_pa_amt numeric(19,4) DEFAULT NULL::numeric,
    ztype character varying(10) DEFAULT NULL::character varying,
    z_pa_amt numeric(19,4) DEFAULT NULL::numeric,
    orderable integer,
    cbill_detail integer,
    comments character varying,
    mnem character varying(15) DEFAULT NULL::character varying,
    cost numeric,
    ref_lab_id character varying(50) DEFAULT NULL::character varying,
    ref_lab_bill_code character varying(50) DEFAULT NULL::character varying,
    ref_lab_payment numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    mod_indicator character varying(50) NOT NULL
);


--
-- Name: audit_chk; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_chk (
    chk_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    pay_no numeric NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    chk_date timestamp without time zone,
    date_rec timestamp without time zone,
    chk_no character varying(25) DEFAULT NULL::character varying,
    amt_paid numeric(19,4) DEFAULT NULL::numeric,
    write_off numeric(19,4) DEFAULT NULL::numeric,
    contractual numeric(19,4) DEFAULT NULL::numeric,
    status character varying(15) DEFAULT NULL::character varying,
    source character varying(50) DEFAULT NULL::character varying,
    w_off_date timestamp without time zone,
    invoice character varying(15) DEFAULT NULL::character varying,
    batch numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    bad_debt boolean NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    mod_date_audit timestamp without time zone,
    uid bigint NOT NULL,
    "cpt4Code" character varying(50) DEFAULT NULL::character varying,
    post_file character varying(256) DEFAULT NULL::character varying,
    chrg_rowguid uuid,
    write_off_code character varying(4) DEFAULT NULL::character varying,
    eft_date timestamp without time zone,
    eft_number character varying(50) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying
);


--
-- Name: audit_chrg; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_chrg (
    chrg_rowguid uuid,
    credited boolean,
    chrg_num numeric,
    account character varying(15) DEFAULT NULL::character varying,
    status character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    hist_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    retail numeric(19,4) DEFAULT NULL::numeric,
    inp_price numeric(19,4) DEFAULT NULL::numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    invoice character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    net_amt numeric(19,4) DEFAULT NULL::numeric,
    fin_type character varying(1) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    post_date timestamp without time zone,
    fin_code character varying(10) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying
);


--
-- Name: audit_chrg_err; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_chrg_err (
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    cl_mnem character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    cdm character varying(7) DEFAULT NULL::character varying,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    amount numeric(19,4) DEFAULT NULL::numeric,
    trans_date timestamp without time zone,
    service_date timestamp without time zone,
    qty integer,
    type character varying(6) DEFAULT NULL::character varying,
    error character varying(100) DEFAULT NULL::character varying,
    uri numeric NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    mt_reqno character varying(8) DEFAULT NULL::character varying,
    location character varying(15) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: audit_cli_dis; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_cli_dis (
    audit_rowguid uuid,
    deleted boolean,
    cli_mnem character varying(10) DEFAULT NULL::character varying,
    start_cdm character varying(7) DEFAULT NULL::character varying,
    end_cdm character varying(7) DEFAULT NULL::character varying,
    percent_ds real,
    price numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uri numeric NOT NULL
);


--
-- Name: audit_client; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_client (
    deleted boolean,
    audit_cli_mnem character varying(10) DEFAULT NULL::character varying,
    cli_nme character varying(40) DEFAULT NULL::character varying,
    addr_1 character varying(40) DEFAULT NULL::character varying,
    addr_2 character varying(40) DEFAULT NULL::character varying,
    city character varying(30) DEFAULT NULL::character varying,
    st character varying(2) DEFAULT NULL::character varying,
    zip character varying(10) DEFAULT NULL::character varying,
    phone character varying(40) DEFAULT NULL::character varying,
    fax character varying(15) DEFAULT NULL::character varying,
    contact character varying,
    comment character varying,
    prn_cpt4 boolean,
    per_disc real,
    date_ord boolean,
    print_cc boolean,
    bill_at_disc boolean,
    do_not_bill boolean,
    type integer,
    last_invoice character varying(15) DEFAULT NULL::character varying,
    last_invoice_date timestamp without time zone,
    last_discount character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mro_name character varying(40) DEFAULT NULL::character varying,
    mro_addr1 character varying(40) DEFAULT NULL::character varying,
    mro_addr2 character varying(40) DEFAULT NULL::character varying,
    mro_city character varying(30) DEFAULT NULL::character varying,
    mro_st character varying(2) DEFAULT NULL::character varying,
    mro_zip character varying(10) DEFAULT NULL::character varying,
    prn_loc character varying(1) DEFAULT NULL::character varying,
    route character varying(10) DEFAULT NULL::character varying,
    county character varying(30) DEFAULT NULL::character varying,
    email character varying(40) DEFAULT NULL::character varying,
    late_notice character varying(1) DEFAULT NULL::character varying,
    late_notice_date timestamp without time zone,
    "statsFacility" character varying(15) DEFAULT NULL::character varying,
    fee_schedule smallint,
    commission boolean,
    client_class character varying(5) DEFAULT NULL::character varying,
    client_maint_rep character varying(30) DEFAULT NULL::character varying,
    client_sales_rep character varying(30) DEFAULT NULL::character varying,
    mod_indicator character varying(10) DEFAULT NULL::character varying,
    bill_pc_charges character varying(4) DEFAULT NULL::character varying
);


--
-- Name: audit_cpt4; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_cpt4 (
    audit_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    cost numeric
);


--
-- Name: audit_cpt4_2; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_cpt4_2 (
    audit_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    cost numeric
);


--
-- Name: audit_cpt4_3; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_cpt4_3 (
    audit_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    cost numeric
);


--
-- Name: audit_cpt4_4; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_cpt4_4 (
    audit_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    cost numeric
);


--
-- Name: audit_cpt4_5; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_cpt4_5 (
    audit_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    cost numeric
);


--
-- Name: audit_dbill; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_dbill (
    dbill_rowguid uuid,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone,
    printed boolean NOT NULL,
    run_user character varying(50) DEFAULT NULL::character varying,
    batch numeric,
    text character varying(8000) DEFAULT NULL::character varying,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_date timestamp without time zone,
    mod_host character varying(50) NOT NULL,
    uid bigint NOT NULL
);


--
-- Name: audit_h1500; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_h1500 (
    h1500_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(50) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone,
    printed boolean NOT NULL,
    run_user character varying(20) NOT NULL,
    batch numeric NOT NULL,
    date_sent timestamp without time zone,
    sent_user character varying(20) DEFAULT NULL::character varying,
    ebill_status character varying(5) DEFAULT NULL::character varying,
    ebill_batch numeric,
    text character varying,
    cold_feed timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: audit_ins; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_ins (
    ins_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ins_a_b_c character varying(1) NOT NULL,
    holder_nme character varying(40) DEFAULT NULL::character varying,
    holder_dob timestamp without time zone,
    plan_nme character varying(45) DEFAULT NULL::character varying,
    plan_addr1 character varying(40) DEFAULT NULL::character varying,
    plan_addr2 character varying(40) DEFAULT NULL::character varying,
    p_city_st character varying(40) DEFAULT NULL::character varying,
    policy_num character varying(50) DEFAULT NULL::character varying,
    cert_ssn character varying(15) DEFAULT NULL::character varying,
    grp_nme character varying(50) DEFAULT NULL::character varying,
    grp_num character varying(15) DEFAULT NULL::character varying,
    holder_sex character varying(1) DEFAULT NULL::character varying,
    employer character varying(25) DEFAULT NULL::character varying,
    e_city_st character varying(35) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    plan_effective_date timestamp without time zone,
    plan_expiration_date timestamp without time zone
);


--
-- Name: audit_insc; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_insc (
    audit_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    code character varying(10) NOT NULL,
    name character varying(45) DEFAULT NULL::character varying,
    addr1 character varying(30) DEFAULT NULL::character varying,
    addr2 character varying(30) DEFAULT NULL::character varying,
    citystzip character varying(30) DEFAULT NULL::character varying,
    provider_no character varying(20) DEFAULT NULL::character varying,
    payer_no character varying(50) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(10) DEFAULT NULL::character varying,
    bill_form character varying(5) DEFAULT NULL::character varying,
    num_labels integer,
    fin_code character varying(10) DEFAULT NULL::character varying,
    fin_class character varying(10) DEFAULT NULL::character varying,
    comment character varying(250) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL
);


--
-- Name: audit_lmrp; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_lmrp (
    cpt4 character varying(5) DEFAULT NULL::character varying,
    beg_icd9 character varying(7) DEFAULT NULL::character varying,
    end_icd9 character varying(7) DEFAULT NULL::character varying,
    payor character varying(30) DEFAULT NULL::character varying,
    fincode character varying(10) DEFAULT NULL::character varying,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(20) DEFAULT NULL::character varying,
    rb_date timestamp without time zone,
    lmrp character varying(25) DEFAULT NULL::character varying,
    lmrp2 character varying(25) DEFAULT NULL::character varying,
    rb_date2 timestamp without time zone,
    chk_for_bad integer,
    ama_year character varying(6) DEFAULT NULL::character varying,
    uid numeric,
    expiration_date timestamp without time zone,
    audit_uid numeric NOT NULL
);


--
-- Name: audit_pat; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_pat (
    pat_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ssn character varying(11) DEFAULT NULL::character varying,
    pat_addr1 character varying(40) DEFAULT NULL::character varying,
    pat_addr2 character varying(40) DEFAULT NULL::character varying,
    city_st_zip character varying(40) DEFAULT NULL::character varying,
    dob_yyyy timestamp without time zone,
    sex character varying(1) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    guarantor character varying(40) DEFAULT NULL::character varying,
    guar_addr character varying(40) DEFAULT NULL::character varying,
    g_city_st character varying(50) DEFAULT NULL::character varying,
    pat_marital character varying(1) DEFAULT NULL::character varying,
    icd9_1 character varying(7) DEFAULT NULL::character varying,
    icd9_2 character varying(7) DEFAULT NULL::character varying,
    icd9_3 character varying(7) DEFAULT NULL::character varying,
    icd9_4 character varying(7) DEFAULT NULL::character varying,
    icd9_5 character varying(7) DEFAULT NULL::character varying,
    icd9_6 character varying(7) DEFAULT NULL::character varying,
    icd9_7 character varying(7) DEFAULT NULL::character varying,
    icd9_8 character varying(7) DEFAULT NULL::character varying,
    icd9_9 character varying(7) DEFAULT NULL::character varying,
    pc_code integer,
    mailer character varying(1) DEFAULT NULL::character varying,
    first_dm timestamp without time zone,
    last_dm timestamp without time zone,
    min_amt numeric(19,4) DEFAULT NULL::numeric,
    phy_id character varying(15) DEFAULT NULL::character varying,
    dbill_date timestamp without time zone,
    ub_date timestamp without time zone,
    h1500_date timestamp without time zone,
    colltr_date timestamp without time zone,
    baddebt_date timestamp without time zone,
    batch_date timestamp without time zone,
    guar_phone character varying(13) DEFAULT NULL::character varying,
    bd_list_date timestamp without time zone,
    ebill_batch_date timestamp without time zone,
    ebill_batch_1500 timestamp without time zone,
    e_ub_demand boolean DEFAULT true NOT NULL,
    e_ub_demand_date timestamp without time zone,
    claimsnet_1500_batch_date timestamp without time zone,
    claimsnet_ub_batch_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    hne_epi_number character varying(50) DEFAULT NULL::character varying,
    ssi_batch character varying(50) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    pat_email character varying(256) DEFAULT NULL::character varying
);


--
-- Name: audit_pat_statements_cerner; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_pat_statements_cerner (
    statement_text character varying NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: audit_patdx; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_patdx (
    account character varying(15) NOT NULL,
    dx_number integer NOT NULL,
    diagnosis character varying(50) NOT NULL,
    entry_date timestamp without time zone,
    mod_date timestamp without time zone,
    uid bigint NOT NULL
);


--
-- Name: audit_phy; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_phy (
    rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    upin character varying(6) DEFAULT NULL::character varying,
    ub92_upin character varying(6) DEFAULT NULL::character varying,
    tnh_num character varying(15) DEFAULT NULL::character varying,
    billing_npi character varying(15) DEFAULT NULL::character varying,
    pc_code character varying(2) DEFAULT NULL::character varying,
    cl_mnem character varying(15) DEFAULT NULL::character varying,
    last_name character varying(30) DEFAULT NULL::character varying,
    first_name character varying(30) DEFAULT NULL::character varying,
    mid_init character varying(30) DEFAULT NULL::character varying,
    group1 character varying(35) DEFAULT NULL::character varying,
    addr_1 character varying(40) DEFAULT NULL::character varying,
    addr_2 character varying(40) DEFAULT NULL::character varying,
    city character varying(30) DEFAULT NULL::character varying,
    state character varying(2) DEFAULT NULL::character varying,
    zip character varying(10) DEFAULT NULL::character varying,
    phone character varying(40) DEFAULT NULL::character varying,
    reserved character varying(1) DEFAULT NULL::character varying,
    num_labels integer,
    mod_date timestamp without time zone,
    mod_user character varying(40) DEFAULT NULL::character varying,
    mod_prg character varying(40) DEFAULT NULL::character varying,
    uri numeric,
    mt_mnem character varying(15) DEFAULT NULL::character varying,
    credentials character varying(50) DEFAULT NULL::character varying,
    ov_code character varying(50) DEFAULT NULL::character varying,
    docnbr character varying(5) DEFAULT NULL::character varying,
    audit_uri numeric NOT NULL
);


--
-- Name: audit_ub; Type: TABLE; Schema: audit; Owner: -
--

CREATE TABLE audit.audit_ub (
    ub_rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    run_date timestamp without time zone,
    printed boolean NOT NULL,
    run_user character varying(30) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    pat_name character varying(40) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(50) DEFAULT NULL::character varying,
    ebill_status character varying(5) DEFAULT NULL::character varying,
    batch numeric,
    text character varying(8000) DEFAULT NULL::character varying,
    edited_ub character varying(8000) DEFAULT NULL::character varying,
    cold_feed timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: ACC_LMRP; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."ACC_LMRP" (
    account character varying(15) NOT NULL,
    dos timestamp without time zone,
    fin_code character varying(10) NOT NULL,
    cl_mnem character varying(10) NOT NULL,
    erorr character varying(1024) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    uri bigint NOT NULL
);


--
-- Name: ACC_LMRP_uri_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."ACC_LMRP_uri_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: ACC_LMRP_uri_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."ACC_LMRP_uri_seq" OWNED BY dbo."ACC_LMRP".uri;


--
-- Name: AuditLog; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."AuditLog" (
    "ID" bigint NOT NULL,
    "Command" character varying(1000) DEFAULT NULL::character varying,
    "PostTime" character varying(24) DEFAULT NULL::character varying,
    "HostName" character varying(100) DEFAULT NULL::character varying,
    "LoginName" character varying(100) DEFAULT NULL::character varying
);


--
-- Name: AuditLog_ID_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."AuditLog_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: AuditLog_ID_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."AuditLog_ID_seq" OWNED BY dbo."AuditLog"."ID";


--
-- Name: ClientTotals_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."ClientTotals_del" (
    client character varying(10) DEFAULT NULL::character varying,
    "ClientAmt" numeric(19,4) DEFAULT NULL::numeric,
    "MedicareAmt" numeric(19,4) DEFAULT NULL::numeric,
    "OtherAmt" numeric(19,4) DEFAULT NULL::numeric
);


--
-- Name: ExportTable; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."ExportTable" (
    id integer,
    line character varying(100) DEFAULT NULL::character varying
);


--
-- Name: ExportTableData; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."ExportTableData" (
    id integer,
    "dateTrans" timestamp without time zone,
    pat_name character varying(100) DEFAULT NULL::character varying,
    cli_name character varying(40) DEFAULT NULL::character varying,
    account character varying(15) DEFAULT NULL::character varying,
    "balPrev" numeric,
    address1 character varying(40) DEFAULT NULL::character varying,
    csz character varying(40) DEFAULT NULL::character varying,
    guarantor character varying(40) DEFAULT NULL::character varying,
    mailer character varying(1) DEFAULT NULL::character varying,
    last_dm timestamp without time zone,
    total_charges numeric,
    last_billed_date timestamp without time zone,
    pay_before_ldm numeric,
    pay_after_ldm numeric,
    "balCurrent" numeric,
    file_date timestamp without time zone
);


--
-- Name: GlobalBillingCharges; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."GlobalBillingCharges" (
    "colClient" character varying(10) DEFAULT NULL::character varying,
    "colAcc" character varying(15) DEFAULT NULL::character varying,
    "colChrgNum" numeric,
    "colCDM" character varying(7) DEFAULT NULL::character varying,
    "colCPT" character varying(5) DEFAULT NULL::character varying,
    "colQty" integer,
    "colChrgAmt" numeric,
    "colDOS" timestamp without time zone,
    "colDateEntered" timestamp without time zone,
    "colFinCode" character varying(10) DEFAULT NULL::character varying,
    "colClType" integer,
    "colError" character varying(50) DEFAULT NULL::character varying
);


--
-- Name: Ins_temp_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Ins_temp_cnb" (
    "Account" character varying(15) NOT NULL,
    "Sex" character varying(1) DEFAULT NULL::character varying,
    "Relation" character varying(2) DEFAULT NULL::character varying,
    "InsCode1" character varying(10) NOT NULL,
    "InsName1" character varying(50) DEFAULT NULL::character varying,
    "InsPol1" character varying(25) DEFAULT NULL::character varying,
    "InsGrp1" character varying(25) DEFAULT NULL::character varying,
    "InsCode2" character varying(10) DEFAULT NULL::character varying,
    "InsName2" character varying(50) DEFAULT NULL::character varying,
    "InsPol2" character varying(25) DEFAULT NULL::character varying,
    "InsGrp2" character varying(25) DEFAULT NULL::character varying,
    "InsCode3" character varying(50) DEFAULT NULL::character varying,
    "InsName3" character varying(50) DEFAULT NULL::character varying,
    "InsPol3" character varying(25) DEFAULT NULL::character varying,
    "InsGrp3" character varying(25) DEFAULT NULL::character varying,
    mod_date timestamp without time zone
);


--
-- Name: Lab_Bill_Rev_CNB; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Lab_Bill_Rev_CNB" (
    "ForDate" character varying(50) DEFAULT NULL::character varying,
    "Print_No" character varying(50) DEFAULT NULL::character varying,
    "Test_Mnem" character varying(50) DEFAULT NULL::character varying,
    "Test_cdm" character varying(50) DEFAULT NULL::character varying,
    test_module character varying(50) DEFAULT NULL::character varying,
    "In_qty" integer,
    "Out_qty" integer,
    "Ref_qty" integer,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    "Test_Dept" character varying(50) DEFAULT NULL::character varying,
    "Test_Abbr" character varying(50) DEFAULT NULL::character varying,
    "Perform_Site" character varying(50) DEFAULT NULL::character varying,
    rep_date timestamp without time zone
);


--
-- Name: MCB_REPORT_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."MCB_REPORT_del" (
    account character varying(50) DEFAULT NULL::character varying,
    ov_id character varying(50) DEFAULT NULL::character varying,
    guar_name character varying(50) DEFAULT NULL::character varying,
    date_orig_reported timestamp without time zone,
    date_last_payment timestamp without time zone,
    acct_rec numeric,
    balance numeric
);


--
-- Name: Medicare_Accounts_Date_Time; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Medicare_Accounts_Date_Time" (
    account character varying(15) NOT NULL,
    trans_date timestamp without time zone,
    paid timestamp without time zone,
    payment numeric,
    mod_date timestamp without time zone
);


--
-- Name: Medicare_Accounts_Date_Time_2011; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Medicare_Accounts_Date_Time_2011" (
    account character varying(15) NOT NULL,
    trans_date timestamp without time zone,
    paid timestamp without time zone,
    payment numeric,
    mod_date timestamp without time zone
);


--
-- Name: Medicare_Comm; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Medicare_Comm" (
    ins_name character varying(25) NOT NULL,
    fin_code character varying(1) NOT NULL,
    is_medicare boolean DEFAULT true,
    mod_date timestamp without time zone
);


--
-- Name: Monthly_Reports; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Monthly_Reports" (
    mi_name character varying(50) NOT NULL,
    sql_code character varying(8000) NOT NULL,
    report_title character varying(255) DEFAULT NULL::character varying,
    comments character varying(255) DEFAULT NULL::character varying,
    button character varying(50) NOT NULL,
    child_button boolean DEFAULT true
);


--
-- Name: Numbers; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Numbers" (
    n integer
);


--
-- Name: ReChrg; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."ReChrg" (
    host_name character varying(80) DEFAULT NULL::character varying,
    cdm character varying(10) DEFAULT NULL::character varying,
    qty numeric,
    amount numeric(19,4) DEFAULT NULL::numeric,
    urn numeric(15,0) NOT NULL,
    account character varying(15) DEFAULT NULL::character varying
);


--
-- Name: Temp_GCodeBilling; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Temp_GCodeBilling" (
    "colClient" character varying(10) DEFAULT NULL::character varying,
    "colAcc" character varying(15) DEFAULT NULL::character varying,
    "colOrigChrgNum" numeric,
    "colCDM" character varying(7) DEFAULT NULL::character varying,
    "colCPT" character varying(5) DEFAULT NULL::character varying,
    "colQty" integer,
    "colChrgAmt" numeric,
    "colDOS" timestamp without time zone,
    "colDateEntered" timestamp without time zone,
    "colPrice" numeric,
    "colError" character varying(50) DEFAULT NULL::character varying,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "colNewChrgNum" numeric
);


--
-- Name: Temp_GlobalBilling; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Temp_GlobalBilling" (
    "colClient" character varying(10) DEFAULT NULL::character varying,
    "colAcc" character varying(15) DEFAULT NULL::character varying,
    "colOrigChrgNum" numeric,
    "colCDM" character varying(7) DEFAULT NULL::character varying,
    "colCPT" character varying(5) DEFAULT NULL::character varying,
    "colQty" integer,
    "colChrgAmt" numeric,
    "colDOS" timestamp without time zone,
    "colDateEntered" timestamp without time zone,
    "colPrice" numeric,
    "colError" character varying(50) DEFAULT NULL::character varying,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    "colNewChrgNum" numeric,
    "colSite" character varying(50) DEFAULT NULL::character varying
);


--
-- Name: Totals_TPG; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."Totals_TPG" (
    "Status(F)" character varying(255) DEFAULT NULL::character varying,
    "Processed Date" timestamp without time zone,
    "Message" character varying(255) DEFAULT NULL::character varying,
    "Account#" character varying(255) DEFAULT NULL::character varying,
    "Status" character varying(255) DEFAULT NULL::character varying,
    "Trans_Date" timestamp without time zone,
    "Mod_Date" timestamp without time zone,
    pay_no double precision,
    date_rec timestamp without time zone,
    amt_paid double precision,
    source character varying(255) DEFAULT NULL::character varying,
    uid bigint NOT NULL
);


--
-- Name: TransactionDetail; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."TransactionDetail" (
    "TransactionDetailID" bigint NOT NULL,
    "Date" timestamp without time zone,
    "AccountID" character varying(15) DEFAULT NULL::character varying,
    "Charges" numeric(19,4) DEFAULT NULL::numeric,
    "Payments" numeric(19,4) DEFAULT NULL::numeric,
    "AccountRunningTotal" numeric(19,4) DEFAULT NULL::numeric,
    "AccountRunningCount" integer,
    "NCID" integer
);


--
-- Name: TransactionDetail_TransactionDetailID_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."TransactionDetail_TransactionDetailID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: TransactionDetail_TransactionDetailID_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."TransactionDetail_TransactionDetailID_seq" OWNED BY dbo."TransactionDetail"."TransactionDetailID";


--
-- Name: UserProfile; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."UserProfile" (
    "Id" bigint NOT NULL,
    "UserName" character varying(50) NOT NULL,
    "Parameter" character varying(50) NOT NULL,
    "ParameterData" character varying(255) DEFAULT NULL::character varying,
    "ModDate" timestamp without time zone,
    "ModUser" character varying(50) DEFAULT NULL::character varying,
    "ModPrg" character varying(50) DEFAULT NULL::character varying,
    "ModHost" character varying(50) DEFAULT NULL::character varying
);


--
-- Name: UserProfile_Id_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."UserProfile_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: UserProfile_Id_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."UserProfile_Id_seq" OWNED BY dbo."UserProfile"."Id";


--
-- Name: XmlSourceTable; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."XmlSourceTable" (
    doc_data character varying NOT NULL,
    import_file character varying(50) NOT NULL,
    processed boolean DEFAULT true NOT NULL,
    accounts smallint,
    charges smallint NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    uid bigint NOT NULL
);


--
-- Name: XmlSourceTable_uid_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."XmlSourceTable_uid_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: XmlSourceTable_uid_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."XmlSourceTable_uid_seq" OWNED BY dbo."XmlSourceTable".uid;


--
-- Name: abn; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.abn (
    account character varying(15) NOT NULL,
    cdm character varying(7) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    lmrp integer DEFAULT 0
);


--
-- Name: acc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc (
    rowguid uuid DEFAULT public.uuid_generate_v4(),
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    cl_mnem character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    cbill_date timestamp without time zone,
    status character varying(8) DEFAULT NULL::character varying,
    ssn character varying(11) DEFAULT NULL::character varying,
    num_comments integer DEFAULT 0,
    meditech_account character varying(15) DEFAULT NULL::character varying,
    original_fincode character varying(1) DEFAULT NULL::character varying,
    oereqno character varying(15) DEFAULT NULL::character varying,
    mri character varying(25) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    ov_order_id character varying(50) DEFAULT NULL::character varying,
    ov_pat_id character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    bill_priority integer DEFAULT 0 NOT NULL,
    "guarantorID" character varying(50) DEFAULT NULL::character varying,
    "HNE_NUMBER" character varying(50) DEFAULT NULL::character varying,
    trans_date_time timestamp without time zone,
    tdate_update boolean DEFAULT true
);


--
-- Name: acc_alert; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_alert (
    account character varying(15) NOT NULL,
    alert boolean NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(30) DEFAULT NULL::character varying,
    mod_prg character varying(30) DEFAULT NULL::character varying,
    mod_host character varying(30) DEFAULT NULL::character varying
);


--
-- Name: acc_dup_check; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_dup_check (
    master_account character varying(15) DEFAULT NULL::character varying,
    account character varying(15) DEFAULT NULL::character varying,
    client character varying(256) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    fin_code character varying(50) DEFAULT NULL::character varying,
    pat_name character varying(100) DEFAULT NULL::character varying,
    pat_ssn character varying(13) DEFAULT NULL::character varying,
    unitno character varying(50) DEFAULT NULL::character varying,
    pat_dob timestamp without time zone,
    is_duplicate_acc boolean,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid numeric(18,0) NOT NULL
);


--
-- Name: acc_location; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_location (
    account character varying(15) NOT NULL,
    location character varying(15) DEFAULT NULL::character varying,
    pt_type character varying(5) DEFAULT NULL::character varying,
    surveydate timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    ov_acct character varying(30) DEFAULT NULL::character varying,
    ov_mri character varying(30) DEFAULT NULL::character varying
);


--
-- Name: acc_merges; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_merges (
    account character varying(15) NOT NULL,
    dup_acc character varying(15) NOT NULL,
    pat_ssn character varying(11) DEFAULT NULL::character varying,
    pat_mri character varying(50) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    fin_code character varying(10) DEFAULT NULL::character varying,
    xml_info xml,
    xml_file character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: acc_paid_out; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_paid_out (
    account character varying(15) NOT NULL,
    trans_date character varying(10) DEFAULT NULL::character varying,
    "mDate" character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    uid bigint
);


--
-- Name: acc_status_updates; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_status_updates (
    account character varying(15) NOT NULL,
    acc_status character varying(10) NOT NULL,
    trans_date timestamp without time zone,
    chrg_dos timestamp without time zone,
    emailed boolean DEFAULT true NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: acc_track; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_track (
    account character varying(15) NOT NULL,
    track_code character varying(15) NOT NULL,
    event_date timestamp without time zone,
    event_user character varying(50) DEFAULT NULL::character varying,
    event_prg character varying(50) DEFAULT NULL::character varying,
    event_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: acc_validation_status; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.acc_validation_status (
    account character varying(15) NOT NULL,
    validation_text character varying,
    mod_date timestamp without time zone,
    mod_user character varying(100) DEFAULT NULL::character varying,
    mod_prg character varying(100) DEFAULT NULL::character varying,
    mod_host character varying(100) DEFAULT NULL::character varying
);


--
-- Name: aging_history; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.aging_history (
    account character varying(15) NOT NULL,
    datestamp timestamp without time zone NOT NULL,
    balance numeric(19,4) DEFAULT NULL::numeric,
    fin_code character varying(10) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mailer character varying(1) DEFAULT NULL::character varying
);


--
-- Name: amt; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.amt (
    chrg_num numeric NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    type character varying(6) DEFAULT NULL::character varying,
    amount numeric(19,4) DEFAULT NULL::numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    deleted boolean DEFAULT true NOT NULL,
    uri numeric(15,0) NOT NULL,
    modi character varying(5) DEFAULT NULL::character varying,
    revcode character varying(5) DEFAULT NULL::character varying,
    modi2 character varying(5) DEFAULT NULL::character varying,
    diagnosis_code_ptr character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(50) DEFAULT NULL::character varying,
    order_code character varying(7) DEFAULT NULL::character varying,
    bill_type character varying(50) DEFAULT NULL::character varying,
    bill_method character varying(50) DEFAULT NULL::character varying,
    pointer_set boolean DEFAULT true
);


--
-- Name: bad_debt; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.bad_debt (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    debtor_last_name character varying(20) DEFAULT NULL::character varying,
    debtor_first_name character varying(15) DEFAULT NULL::character varying,
    st_addr_1 character varying(25) DEFAULT NULL::character varying,
    st_addr_2 character varying(25) DEFAULT NULL::character varying,
    city character varying(18) DEFAULT NULL::character varying,
    state_zip character varying(15) DEFAULT NULL::character varying,
    spouse character varying(15) DEFAULT NULL::character varying,
    phone character varying(12) DEFAULT NULL::character varying,
    soc_security character varying(10) DEFAULT NULL::character varying,
    license_number character varying(20) DEFAULT NULL::character varying,
    employment character varying(35) DEFAULT NULL::character varying,
    remarks character varying(35) DEFAULT NULL::character varying,
    account_no character varying(25) NOT NULL,
    patient_name character varying(20) DEFAULT NULL::character varying,
    remarks2 character varying(35) DEFAULT NULL::character varying,
    misc character varying(29) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    payment_date timestamp without time zone,
    balance numeric(19,4) DEFAULT NULL::numeric,
    date_entered timestamp without time zone,
    date_sent timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    date_transmitted timestamp without time zone
);


--
-- Name: cbill_hist; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cbill_hist (
    cl_mnem character varying(10) NOT NULL,
    thru_date timestamp without time zone,
    invoice character varying(10) NOT NULL,
    bal_forward numeric(19,4) DEFAULT '0'::numeric,
    total_chrg numeric(19,4) DEFAULT '0'::numeric,
    discount numeric(19,4) DEFAULT '0'::numeric,
    balance_due numeric(19,4) DEFAULT '0'::numeric,
    payments numeric(19,4) DEFAULT NULL::numeric,
    true_balance_due numeric(19,4) DEFAULT NULL::numeric,
    cbill_html character varying,
    cbill_filestream bytea,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cdm; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cdm (
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    descript character varying(50) DEFAULT NULL::character varying,
    mtype character varying(10) DEFAULT NULL::character varying,
    m_pa_amt numeric(19,4) DEFAULT '0'::numeric,
    ctype character varying(10) DEFAULT NULL::character varying,
    c_pa_amt numeric(19,4) DEFAULT '0'::numeric,
    ztype character varying(10) DEFAULT NULL::character varying,
    z_pa_amt numeric(19,4) DEFAULT '0'::numeric,
    orderable integer DEFAULT 1,
    cbill_detail integer DEFAULT 0,
    comments character varying,
    mnem character varying(15) DEFAULT NULL::character varying,
    cost numeric,
    ref_lab_id character varying(50) DEFAULT NULL::character varying,
    ref_lab_bill_code character varying(50) DEFAULT NULL::character varying,
    ref_lab_payment numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cdm_link_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cdm_link_cnb (
    "CDM" character varying(50) DEFAULT NULL::character varying,
    link_cnt character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_date timestamp without time zone
);


--
-- Name: cdm_map; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cdm_map (
    vendor character varying(30) NOT NULL,
    vendor_code character varying(30) NOT NULL,
    cdm character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cdw; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cdw (
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    hosp_mnem character varying(10) NOT NULL,
    descript character varying(35) DEFAULT NULL::character varying,
    type character varying(10) DEFAULT NULL::character varying,
    price numeric(19,4) DEFAULT NULL::numeric,
    retail numeric(19,4) DEFAULT NULL::numeric,
    inp_price numeric(19,4) DEFAULT NULL::numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    meditech_mnem character varying(15) DEFAULT NULL::character varying,
    pa_amount numeric(19,4) DEFAULT NULL::numeric,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chk; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    pay_no numeric(15,0) NOT NULL,
    account character varying(15) NOT NULL,
    chk_date timestamp without time zone,
    date_rec timestamp without time zone,
    chk_no character varying(25) DEFAULT NULL::character varying,
    amt_paid numeric(19,4) DEFAULT '0'::numeric,
    write_off numeric(19,4) DEFAULT '0'::numeric,
    contractual numeric(19,4) DEFAULT '0'::numeric,
    status character varying(15) DEFAULT NULL::character varying,
    source character varying(50) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    w_off_date timestamp without time zone,
    invoice character varying(15) DEFAULT NULL::character varying,
    batch numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    bad_debt boolean DEFAULT true NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    mod_date_audit timestamp without time zone,
    "cpt4Code" character varying(50) DEFAULT NULL::character varying,
    post_file character varying(256) DEFAULT NULL::character varying,
    chrg_rowguid uuid,
    write_off_code character varying(4) DEFAULT NULL::character varying,
    eft_date timestamp without time zone,
    eft_number character varying(50) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    ins_code character varying(10) DEFAULT NULL::character varying,
    claim_adj_code character varying(50) DEFAULT NULL::character varying,
    claim_adj_group_code character varying(50) DEFAULT NULL::character varying,
    facility_code character varying(50) DEFAULT NULL::character varying,
    claim_no character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chk_Tests_CNB; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."chk_Tests_CNB" (
    "Test1_Abbr" character varying(50) NOT NULL,
    "Test2_Abbr" character varying(50) NOT NULL
);


--
-- Name: chk_batch; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_batch (
    "BatchNo" bigint NOT NULL,
    "User" character varying(50) NOT NULL,
    "BatchDate" date NOT NULL,
    "BatchData" xml,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chk_batch_BatchNo_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."chk_batch_BatchNo_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: chk_batch_BatchNo_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."chk_batch_BatchNo_seq" OWNED BY dbo.chk_batch."BatchNo";


--
-- Name: chk_electronic; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_electronic (
    account character varying(15) NOT NULL,
    pay_id bigint NOT NULL,
    claim_status_code character varying(2) NOT NULL,
    claim_facility_code character varying(2) NOT NULL,
    clp_chrg numeric NOT NULL,
    clp_paid numeric NOT NULL,
    clp_pat_resp numeric NOT NULL,
    clp_date timestamp without time zone,
    pay_date timestamp without time zone,
    pay_number character varying(50) NOT NULL,
    payor_id character varying(50) NOT NULL,
    payor_name character varying(50) NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    uid bigint NOT NULL
);


--
-- Name: chk_electronic_cpt_adjustment_codes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_electronic_cpt_adjustment_codes (
    pay_id bigint NOT NULL,
    account character varying(15) NOT NULL,
    cpt character varying(5) NOT NULL,
    cpt_modi character varying(5) DEFAULT NULL::character varying,
    adjustment_group_code character varying(2) NOT NULL,
    adjustment_reason_code character varying(5) NOT NULL,
    adjustment_amount numeric NOT NULL,
    adjustment_qty integer NOT NULL,
    uid bigint NOT NULL
);


--
-- Name: chk_electronic_cpt_detail; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_electronic_cpt_detail (
    account character varying(15) NOT NULL,
    pay_id bigint NOT NULL,
    cpt character varying(5) NOT NULL,
    cpt_modi character varying(5) DEFAULT NULL::character varying,
    cpt_charges numeric NOT NULL,
    cpt_paid numeric NOT NULL,
    cpt_qty integer NOT NULL
);


--
-- Name: chk_held; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_held (
    rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    chk_date timestamp without time zone,
    date_rec timestamp without time zone,
    chk_no character varying(25) DEFAULT NULL::character varying,
    amt_paid numeric(19,4) DEFAULT NULL::numeric,
    write_off numeric(19,4) DEFAULT NULL::numeric,
    contractual numeric(19,4) DEFAULT NULL::numeric,
    status character varying(15) DEFAULT NULL::character varying,
    source character varying(50) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    w_off_date timestamp without time zone,
    invoice character varying(15) DEFAULT NULL::character varying,
    batch numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    bad_debt boolean NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    mod_date_audit timestamp without time zone,
    "cpt4Code" character varying(50) DEFAULT NULL::character varying,
    post_file character varying(256) DEFAULT NULL::character varying,
    chrg_rowguid uuid,
    write_off_code character varying(4) DEFAULT NULL::character varying,
    eft_date timestamp without time zone,
    eft_number character varying(50) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    ins_code character varying(10) DEFAULT NULL::character varying,
    claim_adj_code character varying(50) DEFAULT NULL::character varying,
    claim_adj_group_code character varying(50) DEFAULT NULL::character varying,
    facility_code character varying(50) DEFAULT NULL::character varying,
    claim_no character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chk_tests_cnt_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_tests_cnt_cnb (
    specimen_tot character varying(50) DEFAULT NULL::character varying,
    specimen_hit character varying(50) DEFAULT NULL::character varying,
    rundatetime timestamp without time zone,
    cmp_rp_hit character varying(50) DEFAULT NULL::character varying,
    cmp_hfp_hit character varying(50) DEFAULT NULL::character varying,
    chkdatetime timestamp without time zone,
    runondatetime timestamp without time zone,
    missed_spc_hit character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chk_vp_AFFL_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."chk_vp_AFFL_cnb" (
    acct_cnt integer,
    acct_dup integer,
    acct_no character varying,
    run_date timestamp without time zone,
    run_for character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chk_vp_affl_cnb_2; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_vp_affl_cnb_2 (
    acct_cnt integer,
    acct_dup integer,
    acct_no character varying,
    run_date timestamp without time zone,
    run_for timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chk_vp_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chk_vp_cnb (
    acct_cnt integer,
    acct_dup integer,
    acct_no character varying,
    run_date timestamp without time zone,
    run_for timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chrg; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    credited boolean DEFAULT true NOT NULL,
    chrg_num numeric(15,0) NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    status character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    hist_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    retail numeric(19,4) DEFAULT 0.00,
    inp_price numeric(19,4) DEFAULT 0.00,
    comment character varying(50) DEFAULT NULL::character varying,
    invoice character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    net_amt numeric(19,4) DEFAULT 0.00,
    fin_type character varying(1) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(50) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    fin_code character varying(50) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying,
    bill_method character varying(50) DEFAULT NULL::character varying,
    post_file character varying(50) DEFAULT NULL::character varying,
    lname character varying(100) DEFAULT NULL::character varying,
    fname character varying(100) DEFAULT NULL::character varying,
    mname character varying(100) DEFAULT NULL::character varying,
    name_suffix character varying(100) DEFAULT NULL::character varying,
    name_prefix character varying(100) DEFAULT NULL::character varying,
    pat_name character varying(100) DEFAULT NULL::character varying,
    order_site character varying(100) DEFAULT NULL::character varying,
    pat_ssn character varying(100) DEFAULT NULL::character varying,
    unitno character varying(50) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    responsiblephy character varying(50) DEFAULT NULL::character varying,
    mt_mnem character varying(50) DEFAULT NULL::character varying,
    action character varying(50) DEFAULT NULL::character varying,
    facility character varying(50) DEFAULT NULL::character varying,
    referencereq character varying(50) DEFAULT NULL::character varying,
    pat_dob timestamp without time zone,
    chrg_err character varying(8000) DEFAULT NULL::character varying,
    istemp character varying(50) DEFAULT NULL::character varying,
    age_on_date_of_service integer,
    calc_amt numeric
);


--
-- Name: chrg_deleted; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_deleted (
    rowguid uuid,
    credited boolean,
    chrg_num numeric,
    account character varying(15) DEFAULT NULL::character varying,
    status character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    hist_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    retail numeric(19,4) DEFAULT NULL::numeric,
    inp_price numeric(19,4) DEFAULT NULL::numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    invoice character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    net_amt numeric(19,4) DEFAULT NULL::numeric,
    fin_type character varying(1) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(50) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    fin_code character varying(50) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying,
    bill_method character varying(50) DEFAULT NULL::character varying,
    delete_comment character varying(1024) DEFAULT NULL::character varying
);


--
-- Name: chrg_err; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_err (
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    cl_mnem character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    cdm character varying(7) DEFAULT NULL::character varying,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    amount numeric(19,4) DEFAULT NULL::numeric,
    trans_date timestamp without time zone,
    service_date timestamp without time zone,
    qty integer,
    type character varying(50) DEFAULT NULL::character varying,
    error character varying(100) DEFAULT NULL::character varying,
    uri numeric(15,0) NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    mt_reqno character varying(50) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chrg_nhc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_nhc (
    rowguid uuid NOT NULL,
    credited boolean NOT NULL,
    chrg_num numeric(15,0) NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    status character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    hist_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    retail numeric(19,4) DEFAULT NULL::numeric,
    inp_price numeric(19,4) DEFAULT NULL::numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    invoice character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    net_amt numeric(19,4) DEFAULT NULL::numeric,
    fin_type character varying(1) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(50) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    fin_code character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chrg_orphans; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_orphans (
    rowguid uuid,
    credited boolean,
    chrg_num numeric,
    account character varying(15) DEFAULT NULL::character varying,
    status character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    hist_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    retail numeric(19,4) DEFAULT NULL::numeric,
    inp_price numeric(19,4) DEFAULT NULL::numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    invoice character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    net_amt numeric(19,4) DEFAULT NULL::numeric,
    fin_type character varying(1) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(50) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    fin_code character varying(50) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chrg_pa; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_pa (
    chrg_num numeric NOT NULL,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    pa_amount numeric(19,4) DEFAULT NULL::numeric,
    batch numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(8) DEFAULT NULL::character varying,
    perform_site character varying(10) DEFAULT NULL::character varying
);


--
-- Name: chrg_pa_save; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_pa_save (
    chrg_num numeric NOT NULL,
    pa_amount numeric(19,4) DEFAULT NULL::numeric,
    batch numeric,
    mod_date timestamp without time zone,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_prg character varying(20) DEFAULT NULL::character varying,
    mod_host character varying(20) DEFAULT NULL::character varying,
    mt_req_no character varying(8) DEFAULT NULL::character varying,
    perform_site character varying(10) DEFAULT NULL::character varying,
    rowguid uuid
);


--
-- Name: chrg_pathcharges; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_pathcharges (
    "ACCOUNT" character varying(50) DEFAULT NULL::character varying,
    "PATIENT_NAME" character varying(125) DEFAULT NULL::character varying,
    "REG_DATE" character varying(50) DEFAULT NULL::character varying,
    "ENCOUNTER_TYPE" character varying(50) DEFAULT NULL::character varying,
    "LOCATION" character varying(50) DEFAULT NULL::character varying,
    "ORIG_ORDER_DTTM" character varying(50) DEFAULT NULL::character varying,
    "ORDER_DESC" character varying(50) DEFAULT NULL::character varying,
    "ORDER_STATUS" character varying(50) DEFAULT NULL::character varying,
    "ORDER_ID" numeric,
    "CHG_DESC" character varying(125) DEFAULT NULL::character varying,
    "UPDATE_DTTM" character varying(50) DEFAULT NULL::character varying,
    "QTY" smallint,
    "DBT_CRDT" character varying(6) DEFAULT NULL::character varying,
    "CDM" character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    mod_file_date character varying(50) NOT NULL
);


--
-- Name: chrg_pathorders; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_pathorders (
    "ACCOUNT" character varying(50) DEFAULT NULL::character varying,
    "PATIENT_NAME" character varying(50) DEFAULT NULL::character varying,
    "REG_DATE" character varying(50) DEFAULT NULL::character varying,
    "ENCOUNTER_TYPE" character varying(50) DEFAULT NULL::character varying,
    "LOCATION" character varying(50) DEFAULT NULL::character varying,
    "ORIG_ORDER_DTTM" character varying(50) DEFAULT NULL::character varying,
    "ORDER_DESC" character varying(50) DEFAULT NULL::character varying,
    "ORDER_STATUS" character varying(50) DEFAULT NULL::character varying,
    "ORDER_ID" numeric,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    mod_file_date character varying(50) NOT NULL
);


--
-- Name: chrg_pc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_pc (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    cli_mnem character varying(10) NOT NULL,
    account character varying(15) NOT NULL,
    cdm character varying(7) NOT NULL,
    qty integer NOT NULL,
    service_date timestamp without time zone,
    src_file character varying(50) NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: chrg_postdate; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_postdate (
    account character varying(15) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying,
    "accType" integer,
    cli_mnem character varying(10) DEFAULT NULL::character varying,
    date_of_service timestamp without time zone,
    total_charges numeric,
    chrg_post_date timestamp without time zone,
    amt_paid numeric,
    contractual numeric,
    date_of_chk timestamp without time zone,
    write_off numeric,
    write_off_code character varying(4) DEFAULT NULL::character varying,
    chk_post_date timestamp without time zone
);


--
-- Name: chrg_rev_trk; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_rev_trk (
    chrg_num numeric NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_prg character varying(20) DEFAULT NULL::character varying,
    mod_host character varying(20) DEFAULT NULL::character varying
);


--
-- Name: chrg_test; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_test (
    rowguid uuid,
    credited boolean,
    chrg_num numeric(15,0) NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    status character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    hist_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    retail numeric(19,4) DEFAULT NULL::numeric,
    inp_price numeric(19,4) DEFAULT NULL::numeric,
    comment character varying(50) DEFAULT NULL::character varying,
    invoice character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    net_amt numeric(19,4) DEFAULT NULL::numeric,
    fin_type character varying(1) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(50) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    fin_code character varying(50) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying,
    bill_method character varying(50) DEFAULT NULL::character varying,
    post_file character varying(50) DEFAULT NULL::character varying,
    lname character varying(100) DEFAULT NULL::character varying,
    fname character varying(100) DEFAULT NULL::character varying,
    mname character varying(100) DEFAULT NULL::character varying,
    name_suffix character varying(100) DEFAULT NULL::character varying,
    name_prefix character varying(100) DEFAULT NULL::character varying,
    pat_name character varying(100) DEFAULT NULL::character varying,
    order_site character varying(100) DEFAULT NULL::character varying,
    pat_ssn character varying(100) DEFAULT NULL::character varying,
    unitno character varying(50) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    responsiblephy character varying(50) DEFAULT NULL::character varying,
    mt_mnem character varying(50) DEFAULT NULL::character varying,
    action character varying(50) DEFAULT NULL::character varying,
    facility character varying(50) DEFAULT NULL::character varying,
    referencereq character varying(50) DEFAULT NULL::character varying,
    pat_dob timestamp without time zone,
    chrg_err character varying(8000) DEFAULT NULL::character varying,
    istemp character varying(50) DEFAULT NULL::character varying,
    age_on_date_of_service integer,
    calc_amt numeric
);


--
-- Name: chrg_testing_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_testing_del (
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    cl_mnem character varying(10) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    cdm character varying(7) DEFAULT NULL::character varying,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    amount numeric(19,4) DEFAULT NULL::numeric,
    trans_date timestamp without time zone,
    service_date timestamp without time zone,
    qty integer,
    type character varying(6) DEFAULT NULL::character varying,
    error character varying(100) DEFAULT NULL::character varying,
    uri numeric(15,0) NOT NULL,
    deleted boolean NOT NULL,
    mt_reqno character varying(8) DEFAULT NULL::character varying,
    location character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: chrg_unprocessed; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.chrg_unprocessed (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    status character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mt_req_no character varying(50) DEFAULT NULL::character varying,
    performing_site character varying(50) DEFAULT NULL::character varying,
    post_file character varying(50) DEFAULT NULL::character varying,
    lname character varying(50) DEFAULT NULL::character varying,
    fname character varying(100) DEFAULT NULL::character varying,
    mname character varying(100) DEFAULT NULL::character varying,
    name_suffix character varying(100) DEFAULT NULL::character varying,
    name_prefix character varying(100) DEFAULT NULL::character varying,
    pat_name character varying(100) DEFAULT NULL::character varying,
    order_site character varying(100) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    responsiblephy character varying(50) DEFAULT NULL::character varying,
    mt_mnem character varying(50) DEFAULT NULL::character varying,
    action character varying(50) DEFAULT NULL::character varying,
    facility character varying(50) DEFAULT NULL::character varying,
    pat_dob timestamp without time zone,
    chrg_err character varying(8000) DEFAULT NULL::character varying
);


--
-- Name: city_county_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.city_county_del (
    county character varying(100) NOT NULL,
    city character varying(100) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_date character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: cli_dis; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cli_dis (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cli_mnem character varying(10) NOT NULL,
    start_cdm character varying(7) NOT NULL,
    end_cdm character varying(7) NOT NULL,
    percent_ds real,
    price numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) NOT NULL,
    uri numeric(10,0) NOT NULL
);


--
-- Name: cli_dis_updates; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cli_dis_updates (
    client character varying(10) DEFAULT NULL::character varying,
    cdm character varying(7) DEFAULT NULL::character varying,
    nprice numeric
);


--
-- Name: cli_fac_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cli_fac_cnb (
    cli_mnem character varying(10) NOT NULL,
    facility character varying(50) NOT NULL
);


--
-- Name: client; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.client (
    deleted boolean DEFAULT true NOT NULL,
    cli_mnem character varying(10) NOT NULL,
    cli_nme character varying(40) DEFAULT NULL::character varying,
    addr_1 character varying(40) DEFAULT NULL::character varying,
    addr_2 character varying(40) DEFAULT NULL::character varying,
    city character varying(30) DEFAULT NULL::character varying,
    st character varying(2) DEFAULT NULL::character varying,
    zip character varying(10) DEFAULT NULL::character varying,
    phone character varying(40) DEFAULT NULL::character varying,
    fax character varying(15) DEFAULT NULL::character varying,
    contact character varying,
    comment character varying,
    prn_cpt4 boolean DEFAULT true NOT NULL,
    per_disc real DEFAULT '0'::real,
    date_ord boolean DEFAULT true NOT NULL,
    print_cc boolean DEFAULT true NOT NULL,
    bill_at_disc boolean DEFAULT true NOT NULL,
    do_not_bill boolean DEFAULT true NOT NULL,
    type integer DEFAULT 9,
    last_invoice character varying(15) DEFAULT NULL::character varying,
    last_invoice_date timestamp without time zone,
    last_discount character varying(15) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    mro_name character varying(40) DEFAULT NULL::character varying,
    mro_addr1 character varying(40) DEFAULT NULL::character varying,
    mro_addr2 character varying(40) DEFAULT NULL::character varying,
    mro_city character varying(30) DEFAULT NULL::character varying,
    mro_st character varying(2) DEFAULT NULL::character varying,
    mro_zip character varying(10) DEFAULT NULL::character varying,
    prn_loc character varying(1) DEFAULT NULL::character varying,
    route character varying(10) DEFAULT NULL::character varying,
    county character varying(30) DEFAULT NULL::character varying,
    email character varying(40) DEFAULT NULL::character varying,
    late_notice character varying(1) DEFAULT NULL::character varying,
    late_notice_date timestamp without time zone,
    "statsFacility" character varying(15) DEFAULT NULL::character varying,
    fee_schedule smallint DEFAULT '2'::smallint,
    commission boolean,
    client_class character varying(5) DEFAULT NULL::character varying,
    client_maint_rep character varying(30) DEFAULT NULL::character varying,
    client_sales_rep character varying(30) DEFAULT NULL::character varying,
    outpatient_billing boolean DEFAULT true NOT NULL,
    electronic_billing_type character varying(50) DEFAULT NULL::character varying,
    gl_code character varying(10) NOT NULL,
    "facilityNo" character varying(50) DEFAULT NULL::character varying,
    bill_pc_charges character varying(4) NOT NULL,
    notes character varying,
    old_phone character varying(40) DEFAULT NULL::character varying,
    old_fax character varying(15) DEFAULT NULL::character varying,
    bill_to_client character varying(10) DEFAULT NULL::character varying
);


--
-- Name: client_facility_no; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.client_facility_no (
    cl_mnem character varying(15) NOT NULL,
    facilityno character varying(15) NOT NULL
);


--
-- Name: client_supply_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.client_supply_cnb (
    line_no character varying(50) DEFAULT NULL::character varying,
    descrip character varying,
    unit_qty character varying(50) DEFAULT NULL::character varying,
    unit_price numeric(19,4) DEFAULT NULL::numeric,
    each_price numeric(19,4) DEFAULT NULL::numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying
);


--
-- Name: client_supply_tracking_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.client_supply_tracking_cnb (
    cli_mnem character varying(50) DEFAULT NULL::character varying,
    item_no character varying(50) DEFAULT NULL::character varying,
    unit_rec character varying(50) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying
);


--
-- Name: complianceTotals; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."complianceTotals" (
    client character varying(10) DEFAULT NULL::character varying,
    "ClientAmt" numeric(19,4) DEFAULT NULL::numeric,
    "MedicareAmt" numeric(19,4) DEFAULT NULL::numeric,
    "OtherAmt" numeric(19,4) DEFAULT NULL::numeric
);


--
-- Name: complianceTotalsExpanded; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."complianceTotalsExpanded" (
    client character varying(10) DEFAULT NULL::character varying,
    "ClientAmt" numeric(19,4) DEFAULT NULL::numeric,
    "MedicareAmt" numeric(19,4) DEFAULT NULL::numeric,
    "BlueCareAmt" numeric(19,4) DEFAULT NULL::numeric,
    "AmeriChoiceAmt" numeric(19,4) DEFAULT NULL::numeric,
    "OtherAmt" numeric(19,4) DEFAULT NULL::numeric
);


--
-- Name: cpt4; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4 (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    code_flag character varying(50) NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT '0'::numeric,
    cprice numeric(19,4) DEFAULT '0'::numeric,
    zprice numeric(19,4) DEFAULT '0'::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric
);


--
-- Name: cpt4_2; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_2 (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT '0'::numeric,
    cprice numeric(19,4) DEFAULT '0'::numeric,
    zprice numeric(19,4) DEFAULT '0'::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric,
    code_flag character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cpt4_3; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_3 (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT '0'::numeric,
    cprice numeric(19,4) DEFAULT '0'::numeric,
    zprice numeric(19,4) DEFAULT '0'::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric,
    code_flag character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cpt4_4; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_4 (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT '0'::numeric,
    cprice numeric(19,4) DEFAULT '0'::numeric,
    zprice numeric(19,4) DEFAULT '0'::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric,
    code_flag character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cpt4_5; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_5 (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT '0'::numeric,
    cprice numeric(19,4) DEFAULT '0'::numeric,
    zprice numeric(19,4) DEFAULT '0'::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric,
    code_flag character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cpt4_CLAB2016; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."cpt4_CLAB2016" (
    "HCPCS" character varying(5) DEFAULT NULL::character varying,
    "Modifier" character varying(2) DEFAULT NULL::character varying,
    "TN" numeric(19,4) DEFAULT NULL::numeric,
    "SHORTDESC" character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cpt4_MC; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."cpt4_MC" (
    rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    code_flag character varying(50) NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric
);


--
-- Name: cpt4_ama; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_ama (
    cpt4 character varying(5) NOT NULL,
    short_desc character varying,
    med_description character varying
);


--
-- Name: cpt4_audit; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_audit (
    deleted boolean NOT NULL,
    cdm character varying(7) DEFAULT NULL::character varying,
    link integer,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    audit_date timestamp without time zone,
    audit_user character varying(50) DEFAULT NULL::character varying,
    audit_host character varying(50) DEFAULT NULL::character varying,
    uri numeric(18,0) NOT NULL,
    audit_prg character varying(50) DEFAULT NULL::character varying
);


--
-- Name: cpt4_bak; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_bak (
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying
);


--
-- Name: cpt4_david; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_david (
    rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT NULL::numeric,
    cprice numeric(19,4) DEFAULT NULL::numeric,
    zprice numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric
);


--
-- Name: cpt4_jackson_clinic; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_jackson_clinic (
    "Description" character varying(250) DEFAULT NULL::character varying,
    cpt4 character varying(5) DEFAULT NULL::character varying
);


--
-- Name: cpt4_link_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_link_cnb (
    "CPT4" character varying(50) NOT NULL,
    link_cdm character varying(50) NOT NULL,
    descript character varying,
    mod_user character varying(50) NOT NULL,
    mod_date timestamp without time zone
);


--
-- Name: cpt4_wdk; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.cpt4_wdk (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    descript character varying(50) DEFAULT NULL::character varying,
    mprice numeric(19,4) DEFAULT '0'::numeric,
    cprice numeric(19,4) DEFAULT '0'::numeric,
    zprice numeric(19,4) DEFAULT '0'::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    cost numeric
);


--
-- Name: data_EOB; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."data_EOB" (
    rowguid uuid NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    payor character varying(50) DEFAULT NULL::character varying,
    account character varying(15) NOT NULL,
    "subscriberID" character varying(50) DEFAULT NULL::character varying,
    "subscriberName" character varying(150) DEFAULT NULL::character varying,
    date_of_service timestamp without time zone,
    "ICN" character varying(50) DEFAULT NULL::character varying,
    "PatStat" character varying(50) DEFAULT NULL::character varying,
    claim_status character varying(50) DEFAULT NULL::character varying,
    claim_type character varying(50) DEFAULT NULL::character varying,
    charges_reported numeric(19,4) DEFAULT NULL::numeric,
    charges_noncovered numeric(19,4) DEFAULT NULL::numeric,
    charges_denied numeric(19,4) DEFAULT NULL::numeric,
    pat_lib_coinsurance numeric(19,4) DEFAULT NULL::numeric,
    pat_lib_noncovered numeric(19,4) DEFAULT NULL::numeric,
    pay_data_reimb_rate character varying(5) DEFAULT NULL::character varying,
    pay_data_msp_prim_pay numeric(19,4) DEFAULT NULL::numeric,
    pay_data_hcpcs_amt numeric(19,4) DEFAULT NULL::numeric,
    pay_data_cont_adj_amt numeric(19,4) DEFAULT NULL::numeric,
    pay_data_pat_refund numeric(19,4) DEFAULT NULL::numeric,
    pay_data_per_diem_rate character varying(5) DEFAULT NULL::character varying,
    pay_data_net_reimb_amt numeric(19,4) DEFAULT NULL::numeric,
    claim_forwarded_to character varying(100) DEFAULT NULL::character varying,
    claim_forwarded_id character varying(50) DEFAULT NULL::character varying,
    eft_file character varying(255) NOT NULL,
    eft_number character varying(50) NOT NULL,
    eft_date timestamp without time zone,
    eob_print_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    bill_cycle_date timestamp without time zone,
    check_no character varying(50) NOT NULL,
    provider_id character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL
);


--
-- Name: data_EOB_Detail; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."data_EOB_Detail" (
    rowguid uuid NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    claim_status character varying(50) NOT NULL,
    "ServiceCode" character varying(50) NOT NULL,
    rev_code character varying(50) DEFAULT NULL::character varying,
    units integer,
    apc_nr character varying(50) DEFAULT NULL::character varying,
    allowed_amt numeric(19,4) DEFAULT NULL::numeric,
    stat character varying(50) DEFAULT NULL::character varying,
    wght character varying(5) DEFAULT NULL::character varying,
    date_of_service timestamp without time zone,
    charge_amt numeric(19,4) DEFAULT NULL::numeric,
    paid_amt numeric(19,4) DEFAULT NULL::numeric,
    reason_type character varying(5) DEFAULT NULL::character varying,
    reason_code character varying(5) DEFAULT NULL::character varying,
    adj_amt numeric(19,4) DEFAULT NULL::numeric,
    other_adj_amt numeric(19,4) DEFAULT NULL::numeric,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    uid bigint NOT NULL,
    bill_cycle_date timestamp without time zone,
    check_no character varying(50) NOT NULL
);


--
-- Name: data_ErrLog; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."data_ErrLog" (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    uri numeric(18,0) NOT NULL,
    "App_Name" character varying(50) DEFAULT NULL::character varying,
    "Error_Msg" character varying(1024) DEFAULT NULL::character varying,
    "Error_Level" integer DEFAULT 0,
    "Stack_Trace" character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL
);


--
-- Name: data_HL7_Msg; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."data_HL7_Msg" (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    cli_mnem character varying(15) DEFAULT NULL::character varying,
    cli_printed boolean,
    msg_type character varying(50) DEFAULT NULL::character varying,
    msg character varying NOT NULL,
    msg_status character varying(50) DEFAULT NULL::character varying,
    msg_status_reason character varying(256) DEFAULT NULL::character varying,
    continuation_rowguid uuid NOT NULL,
    service_tx_date timestamp without time zone,
    service_rv_date timestamp without time zone,
    "msg_control_ID" character varying(50) DEFAULT NULL::character varying,
    ov_order_id character varying(50) DEFAULT NULL::character varying,
    ov_pat_id character varying(50) DEFAULT NULL::character varying,
    msg_sequence_nr character varying(50) DEFAULT NULL::character varying,
    resulted_datetime timestamp without time zone,
    wreq_rowguid uuid,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: data_billing_batch; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_billing_batch (
    batch numeric NOT NULL,
    run_date timestamp without time zone,
    run_user character varying(100) NOT NULL,
    x12_text character varying,
    claim_count integer,
    mod_date timestamp without time zone,
    mod_user character varying(100) NOT NULL,
    mod_prg character varying(100) NOT NULL,
    mod_host character varying(100) NOT NULL,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL
);


--
-- Name: data_billing_history; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_billing_history (
    rowguid uuid DEFAULT public.uuid_generate_v4(),
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone NOT NULL,
    printed boolean DEFAULT true NOT NULL,
    run_user character varying(20) NOT NULL,
    batch numeric DEFAULT '-1'::numeric NOT NULL,
    ebill_status character varying(10) DEFAULT NULL::character varying,
    ebill_batch numeric,
    text character varying,
    ins_complete timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: data_catalog_20130305; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_catalog_20130305 (
    "TEST MNEMONIC" character varying(255) DEFAULT NULL::character varying,
    "TEST NAME" character varying(255) DEFAULT NULL::character varying,
    "MESSAGE" character varying(255) DEFAULT NULL::character varying,
    "TEST ALIAS" character varying(255) DEFAULT NULL::character varying,
    "CPT CODES" character varying(255) DEFAULT NULL::character varying,
    "INCLUDES" character varying(255) DEFAULT NULL::character varying,
    "PREF SPECIMEN" character varying(255) DEFAULT NULL::character varying,
    "MIN VOL" character varying(255) DEFAULT NULL::character varying,
    "OTH SPEC" character varying(255) DEFAULT NULL::character varying,
    "INSTRUCTIONS" character varying(255) DEFAULT NULL::character varying,
    "CONTAINER" character varying(255) DEFAULT NULL::character varying,
    "TRANS TEMP" character varying(255) DEFAULT NULL::character varying,
    "SPEC STAB" character varying(255) DEFAULT NULL::character varying,
    "REJ CRIT" character varying(255) DEFAULT NULL::character varying,
    "METHODOLOGY" character varying(255) DEFAULT NULL::character varying,
    "FDA STAT" character varying(255) DEFAULT NULL::character varying,
    "SETUP SCHD" character varying(255) DEFAULT NULL::character varying,
    "REP AVAIL(HRS)" character varying(255) DEFAULT NULL::character varying,
    "LIMITATIONS" character varying(255) DEFAULT NULL::character varying,
    "REF RANGE" character varying(255) DEFAULT NULL::character varying,
    "CLIN SIGNIFICANCE" character varying(255) DEFAULT NULL::character varying
);


--
-- Name: data_cerner_base_client_fee_schedule; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_cerner_base_client_fee_schedule (
    cdm character varying(7) NOT NULL,
    cdm_description character varying(50) DEFAULT NULL::character varying,
    mnemonic character varying(15) DEFAULT NULL::character varying,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    cpt4_description character varying(50) DEFAULT NULL::character varying,
    price numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    date_included timestamp without time zone
);


--
-- Name: data_cerner_medicare_fee_schedule; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_cerner_medicare_fee_schedule (
    cdm character varying(7) NOT NULL,
    cdm_description character varying(50) DEFAULT NULL::character varying,
    mnemonic character varying(15) DEFAULT NULL::character varying,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    cpt4_description character varying(50) DEFAULT NULL::character varying,
    price numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    date_included timestamp without time zone
);


--
-- Name: data_cerner_third_party_fee_schedule; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_cerner_third_party_fee_schedule (
    cdm character varying(7) NOT NULL,
    cdm_description character varying(50) DEFAULT NULL::character varying,
    mnemonic character varying(15) DEFAULT NULL::character varying,
    link integer NOT NULL,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    cpt4_description character varying(50) DEFAULT NULL::character varying,
    price numeric(19,4) DEFAULT NULL::numeric,
    rev_code character varying(4) DEFAULT NULL::character varying,
    type character varying(4) DEFAULT NULL::character varying,
    modi character varying(2) DEFAULT NULL::character varying,
    billcode character varying(7) DEFAULT NULL::character varying,
    date_included timestamp without time zone
);


--
-- Name: data_cost_20140328; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_cost_20140328 (
    "CDM" character varying(7) DEFAULT NULL::character varying,
    "DESCRIPTION" character varying(255) DEFAULT NULL::character varying,
    "CCOSTS" numeric(19,4) DEFAULT NULL::numeric
);


--
-- Name: data_cost_20141224; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_cost_20141224 (
    "CDM" character varying(7) DEFAULT NULL::character varying,
    "CPT" character varying(5) DEFAULT NULL::character varying,
    "DESCRIPTION" character varying(255) DEFAULT NULL::character varying,
    "COSTS" double precision,
    "COST_TYPE" character varying(255) DEFAULT NULL::character varying,
    "2014 COSTS" double precision
);


--
-- Name: data_electronic_status; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_electronic_status (
    account character varying(15) NOT NULL,
    status_type character varying(50) NOT NULL,
    bill_type character varying(4) DEFAULT NULL::character varying,
    provider character varying(50) DEFAULT NULL::character varying,
    subid character varying(50) DEFAULT NULL::character varying,
    tracer_no character varying(50) DEFAULT NULL::character varying,
    amt_on_report numeric(19,4) DEFAULT NULL::numeric,
    status_on_claim character varying(50) DEFAULT NULL::character varying,
    status_text character varying(8000) DEFAULT NULL::character varying,
    status_date timestamp without time zone,
    status_batch character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    uid bigint NOT NULL
);


--
-- Name: data_fincode_d_ssi; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_fincode_d_ssi (
    rowguid uuid,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(50) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone,
    printed boolean NOT NULL,
    run_user character varying(20) NOT NULL,
    batch numeric NOT NULL,
    date_sent timestamp without time zone,
    sent_user character varying(20) DEFAULT NULL::character varying,
    ebill_status character varying(5) DEFAULT NULL::character varying,
    ebill_batch numeric,
    text character varying,
    cold_feed timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: data_h1500_to_SSI; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."data_h1500_to_SSI" (
    rowguid uuid,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(50) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone,
    printed boolean NOT NULL,
    run_user character varying(20) NOT NULL,
    batch numeric NOT NULL,
    date_sent timestamp without time zone,
    sent_user character varying(20) DEFAULT NULL::character varying,
    ebill_status character varying(5) DEFAULT NULL::character varying,
    ebill_batch numeric,
    text character varying,
    cold_feed timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: data_ins_lifepoint; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_ins_lifepoint (
    plan_key integer,
    plan_name character varying(150) DEFAULT NULL::character varying,
    address1 character varying(50) DEFAULT NULL::character varying,
    address2 character varying(50) DEFAULT NULL::character varying,
    city character varying(50) DEFAULT NULL::character varying,
    state character varying(50) DEFAULT NULL::character varying,
    zip character varying(50) DEFAULT NULL::character varying,
    lifepoint_code character varying(50) DEFAULT NULL::character varying
);


--
-- Name: data_jpg_refunds; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_jpg_refunds (
    "Acct" double precision,
    "First Name" character varying(255) DEFAULT NULL::character varying,
    "Last Name" character varying(255) DEFAULT NULL::character varying,
    "DOS" timestamp without time zone,
    "CPT" character varying(255) DEFAULT NULL::character varying,
    "Units" double precision,
    "Accession" character varying(255) DEFAULT NULL::character varying,
    "Insurance" character varying(255) DEFAULT NULL::character varying
);


--
-- Name: data_lab_deletes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_lab_deletes (
    "cdm " character varying(7) DEFAULT NULL::character varying,
    "Description" character varying(255) DEFAULT NULL::character varying,
    " CPT/HCPCS Code" double precision,
    "rev code" double precision,
    "YTD Volume" double precision,
    "BP HCPCS Comments" character varying
);


--
-- Name: data_monitor_360; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_monitor_360 (
    user360 character varying(50) NOT NULL,
    app_date timestamp without time zone
);


--
-- Name: data_monthly_ins_report; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_monthly_ins_report (
    account character varying(15) NOT NULL,
    pat_name character varying(256) NOT NULL,
    pat_sex character varying(1) NOT NULL,
    ins_holder_sex character varying(1) NOT NULL,
    ins_code character varying(50) DEFAULT NULL::character varying,
    ins_plan_name character varying(256) DEFAULT NULL::character varying,
    reported_date timestamp without time zone
);


--
-- Name: data_prior_month_ar; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_prior_month_ar (
    uid bigint NOT NULL,
    prior_month_ar numeric NOT NULL,
    prior_month timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: data_quest_360; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_quest_360 (
    deleted boolean DEFAULT true NOT NULL,
    "Patient" character varying(255) DEFAULT NULL::character varying,
    patid character varying(256) NOT NULL,
    html_doc character varying,
    account character varying(15) NOT NULL,
    date_of_service timestamp without time zone,
    pre360_error boolean DEFAULT true NOT NULL,
    bill_code_error boolean DEFAULT true NOT NULL,
    entered boolean DEFAULT true NOT NULL,
    charges_entered boolean DEFAULT true NOT NULL,
    bill_type character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    entry_date timestamp without time zone,
    uid bigint NOT NULL,
    emailed boolean DEFAULT true,
    transmission_date timestamp without time zone
);


--
-- Name: data_quest_360_uid_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.data_quest_360_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: data_quest_360_uid_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.data_quest_360_uid_seq OWNED BY dbo.data_quest_360.uid;


--
-- Name: data_quest_billing; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_quest_billing (
    deleted boolean DEFAULT true NOT NULL,
    status character varying(50) DEFAULT NULL::character varying,
    req_no character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    "Patient" character varying(255) DEFAULT NULL::character varying,
    collection_date timestamp without time zone,
    "DOB" timestamp without time zone,
    atl_user character varying(50) DEFAULT NULL::character varying,
    cdm character varying(7) DEFAULT NULL::character varying,
    quest_code character varying(50) DEFAULT NULL::character varying,
    quest_desc character varying,
    quest_cpt4 character varying,
    date_entered timestamp without time zone,
    "SSN" character varying(11) DEFAULT NULL::character varying,
    post_date timestamp without time zone,
    invoice character varying(50) DEFAULT NULL::character varying,
    comment character varying(1024) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    mod_file character varying(255) NOT NULL
);


--
-- Name: data_quest_cpt4_d_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_quest_cpt4_d_del (
    "MCL CPT4" character varying(10) DEFAULT NULL::character varying,
    "Quest Test Code" character varying(20) DEFAULT NULL::character varying,
    "TEST NAME" character varying(255) DEFAULT NULL::character varying,
    "SUM OF GAP LIST" double precision,
    "SUM OF EXCLUSION LIST" double precision,
    "SUM OF TOTAL QTY" double precision,
    "DRAFT PROPOSED PRICING" double precision,
    "GAP LIST TOTAL" numeric(19,4) DEFAULT NULL::numeric,
    "EXCLUSIONS LIST TOTAL" numeric(19,4) DEFAULT NULL::numeric,
    "EXTENDED PROPOSED PRICE" numeric(19,4) DEFAULT NULL::numeric,
    "Out reach Reimbursement" double precision,
    "Draft Proposed Pricing (Column G) as a percent of Outreach Reim" double precision,
    "Out Patient Reimbursements" double precision,
    "BLUE CARE Published Reimbursements" double precision,
    "Percent BC Published /Actual" double precision,
    "Actual Reimb# Gap" numeric(19,4) DEFAULT NULL::numeric,
    "Actual Reimb# Exclusion" numeric(19,4) DEFAULT NULL::numeric,
    "TEST COST" double precision
);


--
-- Name: data_quest_global_billing_tracking; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_quest_global_billing_tracking (
    deleted boolean DEFAULT true,
    account character varying(15) NOT NULL,
    pat_name character varying(100) NOT NULL,
    date_of_service timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    credited boolean DEFAULT true,
    credited_date timestamp without time zone,
    amt numeric,
    "Qaccount_invoice" character varying(15) DEFAULT NULL::character varying,
    "Jaccount_invoice" character varying(15) DEFAULT NULL::character varying,
    chrg_num numeric,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL
);


--
-- Name: data_rechrg_outpatient; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_rechrg_outpatient (
    account character varying(15) DEFAULT NULL::character varying,
    chrg_num numeric NOT NULL,
    cdm character varying(7) DEFAULT NULL::character varying,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    net_amt numeric(19,4) DEFAULT NULL::numeric,
    qty numeric,
    amount numeric(19,4) DEFAULT NULL::numeric,
    "new price" numeric(19,4) DEFAULT NULL::numeric,
    diff numeric(19,4) DEFAULT NULL::numeric,
    processed boolean NOT NULL,
    processed_date timestamp without time zone
);


--
-- Name: data_reports; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_reports (
    account character varying(15) DEFAULT NULL::character varying,
    report character varying NOT NULL,
    batch character varying(50) DEFAULT NULL::character varying,
    batch_time character varying(4) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: data_ssi_1500_deleted; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_ssi_1500_deleted (
    rowguid uuid,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(50) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone,
    printed boolean NOT NULL,
    run_user character varying(20) NOT NULL,
    batch numeric NOT NULL,
    date_sent timestamp without time zone,
    sent_user character varying(20) DEFAULT NULL::character varying,
    ebill_status character varying(5) DEFAULT NULL::character varying,
    ebill_batch numeric,
    text character varying,
    cold_feed timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: data_tier_pricing; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.data_tier_pricing (
    "CLIENT" character varying(10) DEFAULT NULL::character varying,
    "NAME" character varying(40) DEFAULT NULL::character varying,
    "CDM" character varying(7) DEFAULT NULL::character varying,
    "CDM DESCRIPTION" character varying(50) DEFAULT NULL::character varying,
    "CLIENT PRICE" numeric,
    "INSURANCE PRICE" numeric
);


--
-- Name: dbill; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dbill (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone,
    printed boolean DEFAULT true NOT NULL,
    run_user character varying(20) DEFAULT NULL::character varying,
    batch numeric DEFAULT '-1'::numeric,
    text character varying(8000) DEFAULT NULL::character varying,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_date character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: dict_A_CPEDIT; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."dict_A_CPEDIT" (
    "ME_1" character varying(50) NOT NULL,
    "ME_2" character varying(50) NOT NULL,
    effective_date character varying(50) DEFAULT NULL::character varying,
    deletion_date character varying(50) DEFAULT NULL::character varying,
    prior_rebundled_code_indicator character varying(50) DEFAULT NULL::character varying,
    standard_policy_statement character varying(1000) DEFAULT NULL::character varying,
    cci_indicator character varying(50) DEFAULT NULL::character varying
);


--
-- Name: dict_BCBST_Laboratory_Fee_Schedule; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."dict_BCBST_Laboratory_Fee_Schedule" (
    "Code" character varying(5) DEFAULT NULL::character varying,
    "Allowed" numeric(19,4) DEFAULT NULL::numeric,
    "Note" character varying(255) DEFAULT NULL::character varying
);


--
-- Name: dict_BlueCare_Reimbursement_2012; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."dict_BlueCare_Reimbursement_2012" (
    cpt character varying(5) DEFAULT NULL::character varying,
    max_payment numeric,
    note character varying(255) DEFAULT NULL::character varying
);


--
-- Name: dict_C_MEEDIT; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."dict_C_MEEDIT" (
    "ME_1" character varying(50) NOT NULL,
    "ME_2" character varying(50) NOT NULL,
    effective_date character varying(50) DEFAULT NULL::character varying,
    deletion_date character varying(50) DEFAULT NULL::character varying,
    prior_rebundled_code_indicator character varying(50) DEFAULT NULL::character varying,
    standard_policy_statement character varying(1000) DEFAULT NULL::character varying,
    cci_indicator character varying(50) DEFAULT NULL::character varying
);


--
-- Name: dict_Date; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."dict_Date" (
    "ID" bigint NOT NULL,
    "Date" timestamp without time zone,
    "Day" character varying(2) NOT NULL,
    "DaySuffix" character varying(4) NOT NULL,
    "DayOfWeek" character varying(9) NOT NULL,
    "Month" character varying(2) NOT NULL,
    "MonthName" character varying(9) NOT NULL,
    "Quarter" smallint NOT NULL,
    "QuarterName" character varying(6) NOT NULL,
    "Year" character varying(4) NOT NULL,
    "StandardDate" character varying(10) DEFAULT NULL::character varying,
    "HolidayText" character varying(50) DEFAULT NULL::character varying,
    "Julian" integer
);


--
-- Name: dict_Date_ID_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."dict_Date_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: dict_Date_ID_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."dict_Date_ID_seq" OWNED BY dbo."dict_Date"."ID";


--
-- Name: dict_Madison_County_Capitated_Contract; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."dict_Madison_County_Capitated_Contract" (
    deleted boolean NOT NULL,
    cdm character varying(7) NOT NULL,
    description character varying(8000) DEFAULT NULL::character varying,
    effective_date timestamp without time zone,
    end_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: dict_acc_validation; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_acc_validation (
    rule_id bigint NOT NULL,
    type_check character varying(50) DEFAULT NULL::character varying,
    valid boolean NOT NULL,
    "strSql" character varying(8000) NOT NULL,
    error character varying(256) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: dict_acc_validation_criteria; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_acc_validation_criteria (
    rule_id integer NOT NULL,
    fin_code character varying(10) DEFAULT NULL::character varying,
    ins_code character varying(50) DEFAULT NULL::character varying,
    bill_form character varying(50) DEFAULT NULL::character varying,
    effective_date timestamp without time zone,
    expire_date timestamp without time zone,
    uid bigint NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: dict_acc_validation_criteria_uid_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.dict_acc_validation_criteria_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: dict_acc_validation_criteria_uid_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.dict_acc_validation_criteria_uid_seq OWNED BY dbo.dict_acc_validation_criteria.uid;


--
-- Name: dict_acc_validation_rule_id_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.dict_acc_validation_rule_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: dict_acc_validation_rule_id_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.dict_acc_validation_rule_id_seq OWNED BY dbo.dict_acc_validation.rule_id;


--
-- Name: dict_bcPayments_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."dict_bcPayments_del" (
    account character varying(10) DEFAULT NULL::character varying,
    cpt4 character varying(5) DEFAULT NULL::character varying,
    out_patient_payments numeric,
    outreach_payments numeric
);


--
-- Name: dict_bluecare_reimb_from_files_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_bluecare_reimb_from_files_del (
    cpt4 character varying(5) DEFAULT NULL::character varying,
    reimbursement numeric
);


--
-- Name: dict_cdm_conversion; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_cdm_conversion (
    deleted boolean DEFAULT true NOT NULL,
    order_code character varying(7) NOT NULL,
    menm character varying(50) DEFAULT NULL::character varying,
    order_cpt character varying(5) DEFAULT NULL::character varying,
    order_link integer NOT NULL,
    bill_cdm character varying(7) NOT NULL,
    ins_code character varying(50) DEFAULT NULL::character varying,
    ins_name character varying(50) DEFAULT NULL::character varying,
    description character varying(50) DEFAULT NULL::character varying,
    uid bigint NOT NULL
);


--
-- Name: dict_claim_adjustment_codes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_claim_adjustment_codes (
    code character varying(100) DEFAULT NULL::character varying,
    description character varying(8000) DEFAULT NULL::character varying,
    start_date timestamp without time zone,
    last_modified_date timestamp without time zone,
    stop_date timestamp without time zone,
    notes character varying(8000) DEFAULT NULL::character varying
);


--
-- Name: dict_claim_validation_rule_criteria; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_claim_validation_rule_criteria (
    "RuleId" integer NOT NULL,
    "LineType" character varying(50) DEFAULT NULL::character varying,
    "GroupId" integer,
    "ParentGroupId" integer,
    "Class" character varying(50) DEFAULT NULL::character varying,
    "MemberName" character varying(50) DEFAULT NULL::character varying,
    "Operator" character varying(50) DEFAULT NULL::character varying,
    "TargetValue" character varying(50) DEFAULT NULL::character varying,
    "RuleCriteriaId" bigint NOT NULL,
    mod_user character varying(100) DEFAULT NULL::character varying,
    mod_prg character varying(100) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_host character varying(100) DEFAULT NULL::character varying
);


--
-- Name: dict_claim_validation_rule_criteria_RuleCriteriaId_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."dict_claim_validation_rule_criteria_RuleCriteriaId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: dict_claim_validation_rule_criteria_RuleCriteriaId_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."dict_claim_validation_rule_criteria_RuleCriteriaId_seq" OWNED BY dbo.dict_claim_validation_rule_criteria."RuleCriteriaId";


--
-- Name: dict_claim_validation_rules; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_claim_validation_rules (
    "RuleId" bigint NOT NULL,
    "RuleName" character varying(80) DEFAULT NULL::character varying,
    "Description" character varying(100) DEFAULT NULL::character varying,
    "ErrorText" character varying(100) DEFAULT NULL::character varying,
    "EffectiveDate" date,
    "EndEffectiveDate" date,
    mod_user character varying(100) DEFAULT NULL::character varying,
    mod_prg character varying(100) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_host character varying(100) DEFAULT NULL::character varying
);


--
-- Name: dict_claim_validation_rules_RuleId_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo."dict_claim_validation_rules_RuleId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: dict_claim_validation_rules_RuleId_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo."dict_claim_validation_rules_RuleId_seq" OWNED BY dbo.dict_claim_validation_rules."RuleId";


--
-- Name: dict_code_835; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_code_835 (
    code_type character varying(50) NOT NULL,
    code character varying(50) NOT NULL,
    code_description character varying(2096) NOT NULL,
    date_start timestamp without time zone,
    date_end timestamp without time zone,
    date_last_modified timestamp without time zone,
    action character varying(50) DEFAULT NULL::character varying
);


--
-- Name: dict_cost_by_cdm; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_cost_by_cdm (
    "CDM" character varying(7) DEFAULT NULL::character varying,
    "CDM_DESC" character varying(255) DEFAULT NULL::character varying,
    "Total cost" numeric(19,4) DEFAULT NULL::numeric
);


--
-- Name: dict_cpt4_mc_percent; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_cpt4_mc_percent (
    cpt4 character varying(50) DEFAULT NULL::character varying,
    mc_percent numeric
);


--
-- Name: dict_cpt4_reimbursements; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_cpt4_reimbursements (
    fin_code character varying(50) NOT NULL,
    cpt4 character varying(5) NOT NULL,
    max_payment numeric NOT NULL,
    max_payment_date timestamp without time zone,
    min_payment numeric NOT NULL,
    min_payment_date timestamp without time zone,
    number integer NOT NULL,
    total_payments numeric NOT NULL
);


--
-- Name: dict_cpt4_warnings; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_cpt4_warnings (
    deleted boolean DEFAULT true NOT NULL,
    cpt4 character varying(5) NOT NULL,
    note character varying(1024) DEFAULT NULL::character varying,
    is_ssi_edit boolean DEFAULT true NOT NULL,
    mod_date timestamp without time zone,
    mod_prog character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL
);


--
-- Name: dict_general_ledger_codes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_general_ledger_codes (
    gl_account_code character varying(3) NOT NULL,
    level_1 character varying(4) NOT NULL,
    level_2 character varying(4) NOT NULL,
    level_3 character varying(4) NOT NULL,
    description character varying(250) NOT NULL
);


--
-- Name: dict_global_billing_cdms; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_global_billing_cdms (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    cdm character varying(7) NOT NULL,
    comment character varying(255) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    effective_date timestamp without time zone,
    expiration_date timestamp without time zone
);


--
-- Name: dict_hospital_outpatient_prices; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_hospital_outpatient_prices (
    "Hosp #" double precision,
    "Dept" double precision,
    "Item" double precision,
    "CDM Desc" character varying(255) DEFAULT NULL::character varying,
    "Active/Inactive" character varying(255) DEFAULT NULL::character varying,
    "Pointer Value" double precision,
    "CPT4 Code" character varying(255) DEFAULT NULL::character varying,
    "Pointer Value1" double precision,
    "Hosp Bill Code" character varying(255) DEFAULT NULL::character varying,
    "Current Price" double precision
);


--
-- Name: dict_job_scheduler; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_job_scheduler (
    stored_procedure character varying(256) NOT NULL,
    run_date_day_of_week integer NOT NULL,
    run_date_last timestamp without time zone,
    run_hour integer,
    run_hour_last integer,
    run_date_datepart character varying(5) NOT NULL,
    paramater_list xml
);


--
-- Name: dict_lab_deletes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_lab_deletes (
    cdm character varying(7) DEFAULT NULL::character varying,
    description character varying(255) DEFAULT NULL::character varying,
    "CPT/HCPCS_Code" character varying,
    rev_code character varying,
    "YTD_Volume" double precision,
    "BP_HCPCS_Comments" character varying
);


--
-- Name: dict_multiple_unit_cpt4s; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_multiple_unit_cpt4s (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    cpt4 character varying(5) NOT NULL,
    mod_date character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: dict_na_cdms; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_na_cdms (
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    effective_date timestamp without time zone,
    end_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: dict_ncd; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_ncd (
    ncd_id character varying(25) NOT NULL,
    "F2" character varying(255) DEFAULT NULL::character varying,
    cpt character varying(15) DEFAULT NULL::character varying,
    cpt_eff_date timestamp without time zone,
    cpt_term_date timestamp without time zone,
    icd10 character varying(25) NOT NULL,
    icd10_eff_date timestamp without time zone,
    icd10_term_date timestamp without time zone,
    resolution_code character varying(1) DEFAULT NULL::character varying
);


--
-- Name: dict_quest_cdm_cost_jerry_to_be_deleted; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_quest_cdm_cost_jerry_to_be_deleted (
    "CDM" character varying(50) DEFAULT NULL::character varying,
    mcl_cost character varying(50) DEFAULT NULL::character varying,
    "KE Test List 10 09 12 update#CODE" character varying(255) DEFAULT NULL::character varying,
    report_name character varying(255) DEFAULT NULL::character varying,
    "Quest CPT Code  *" character varying(255) DEFAULT NULL::character varying,
    "Is this Test on the current VSHP Exclusion List? **" character varying(255) DEFAULT NULL::character varying,
    "Proposed Pricing / What Quest will pay to JMH" numeric(19,4) DEFAULT NULL::numeric,
    "Quest Comments" character varying(255) DEFAULT NULL::character varying,
    "MCL CPT Code ? &  Comments" character varying(255) DEFAULT NULL::character varying,
    "Methodology" character varying(255) DEFAULT NULL::character varying,
    mcl_comments character varying(255) DEFAULT NULL::character varying
);


--
-- Name: dict_quest_cdm_cost_to_be_deleted; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_quest_cdm_cost_to_be_deleted (
    "MCL CDM" character varying(255) DEFAULT NULL::character varying,
    "MCL Cost" character varying(255) DEFAULT NULL::character varying,
    "KE Test List 10 09 12 update#CODE" character varying(255) DEFAULT NULL::character varying,
    "REPORT NAME" character varying(255) DEFAULT NULL::character varying,
    "Quest CPT Code  *" character varying(255) DEFAULT NULL::character varying,
    "Is this Test on the current VSHP Exclusion List? **" character varying(255) DEFAULT NULL::character varying,
    "Proposed Pricing / What Quest will pay to JMH" numeric(19,4) DEFAULT NULL::numeric,
    "Quest Comments" character varying(255) DEFAULT NULL::character varying,
    "MCL CPT Code ? &  Comments" character varying(255) DEFAULT NULL::character varying,
    "Methodology" character varying(255) DEFAULT NULL::character varying,
    "MCL Comments" character varying(255) DEFAULT NULL::character varying,
    "F31" character varying(255) DEFAULT NULL::character varying,
    "F32" character varying(255) DEFAULT NULL::character varying,
    "F33" character varying(255) DEFAULT NULL::character varying,
    "F34" character varying(255) DEFAULT NULL::character varying,
    "F35" character varying(255) DEFAULT NULL::character varying,
    "F36" character varying(255) DEFAULT NULL::character varying,
    "F37" character varying(255) DEFAULT NULL::character varying,
    "F38" character varying(255) DEFAULT NULL::character varying,
    "F39" character varying(255) DEFAULT NULL::character varying,
    "F40" character varying(255) DEFAULT NULL::character varying,
    "F41" character varying(255) DEFAULT NULL::character varying,
    "F42" character varying(255) DEFAULT NULL::character varying,
    "F43" character varying(255) DEFAULT NULL::character varying,
    "F44" character varying(255) DEFAULT NULL::character varying,
    "F45" character varying(255) DEFAULT NULL::character varying,
    "F46" character varying(255) DEFAULT NULL::character varying,
    "F47" character varying(255) DEFAULT NULL::character varying,
    "F48" character varying(255) DEFAULT NULL::character varying,
    "F49" character varying(255) DEFAULT NULL::character varying,
    "F50" character varying(255) DEFAULT NULL::character varying,
    "F51" character varying(255) DEFAULT NULL::character varying,
    "F52" character varying(255) DEFAULT NULL::character varying,
    "F53" character varying(255) DEFAULT NULL::character varying,
    "F54" character varying(255) DEFAULT NULL::character varying,
    "F55" character varying(255) DEFAULT NULL::character varying,
    "F56" character varying(255) DEFAULT NULL::character varying,
    "F57" character varying(255) DEFAULT NULL::character varying,
    "F58" character varying(255) DEFAULT NULL::character varying,
    "F59" character varying(255) DEFAULT NULL::character varying,
    "F60" character varying(255) DEFAULT NULL::character varying,
    "F61" character varying(255) DEFAULT NULL::character varying,
    "F62" character varying(255) DEFAULT NULL::character varying,
    "F63" character varying(255) DEFAULT NULL::character varying,
    "F64" character varying(255) DEFAULT NULL::character varying,
    "F65" character varying(255) DEFAULT NULL::character varying,
    "F66" character varying(255) DEFAULT NULL::character varying,
    "F67" character varying(255) DEFAULT NULL::character varying,
    "F68" character varying(255) DEFAULT NULL::character varying,
    "F69" character varying(255) DEFAULT NULL::character varying,
    "F70" character varying(255) DEFAULT NULL::character varying,
    "F71" character varying(255) DEFAULT NULL::character varying,
    "F72" character varying(255) DEFAULT NULL::character varying,
    "F73" character varying(255) DEFAULT NULL::character varying,
    "F74" character varying(255) DEFAULT NULL::character varying,
    "F75" character varying(255) DEFAULT NULL::character varying,
    "F76" character varying(255) DEFAULT NULL::character varying,
    "F77" character varying(255) DEFAULT NULL::character varying,
    "F78" character varying(255) DEFAULT NULL::character varying,
    "F79" character varying(255) DEFAULT NULL::character varying,
    "F80" character varying(255) DEFAULT NULL::character varying,
    "F81" character varying(255) DEFAULT NULL::character varying,
    "F82" character varying(255) DEFAULT NULL::character varying,
    "F83" character varying(255) DEFAULT NULL::character varying,
    "F84" character varying(255) DEFAULT NULL::character varying,
    "F85" character varying(255) DEFAULT NULL::character varying,
    "F86" character varying(255) DEFAULT NULL::character varying,
    "F87" character varying(255) DEFAULT NULL::character varying,
    "F88" character varying(255) DEFAULT NULL::character varying,
    "F89" character varying(255) DEFAULT NULL::character varying,
    "F90" character varying(255) DEFAULT NULL::character varying,
    "F91" character varying(255) DEFAULT NULL::character varying,
    "F92" character varying(255) DEFAULT NULL::character varying,
    "F93" character varying(255) DEFAULT NULL::character varying,
    "F94" character varying(255) DEFAULT NULL::character varying,
    "F95" character varying(255) DEFAULT NULL::character varying,
    "F96" character varying(255) DEFAULT NULL::character varying,
    "F97" character varying(255) DEFAULT NULL::character varying,
    "F98" character varying(255) DEFAULT NULL::character varying,
    "F99" character varying(255) DEFAULT NULL::character varying,
    "F100" character varying(255) DEFAULT NULL::character varying,
    "F101" character varying(255) DEFAULT NULL::character varying,
    "F102" character varying(255) DEFAULT NULL::character varying,
    "F103" character varying(255) DEFAULT NULL::character varying,
    "F104" character varying(255) DEFAULT NULL::character varying,
    "F105" character varying(255) DEFAULT NULL::character varying,
    "F106" character varying(255) DEFAULT NULL::character varying,
    "F107" character varying(255) DEFAULT NULL::character varying,
    "F108" character varying(255) DEFAULT NULL::character varying,
    "F109" character varying(255) DEFAULT NULL::character varying,
    "F110" character varying(255) DEFAULT NULL::character varying,
    "F111" character varying(255) DEFAULT NULL::character varying,
    "F112" character varying(255) DEFAULT NULL::character varying,
    "F113" character varying(255) DEFAULT NULL::character varying,
    "F114" character varying(255) DEFAULT NULL::character varying,
    "F115" character varying(255) DEFAULT NULL::character varying,
    "F116" character varying(255) DEFAULT NULL::character varying,
    "F117" character varying(255) DEFAULT NULL::character varying,
    "F118" character varying(255) DEFAULT NULL::character varying,
    "F119" character varying(255) DEFAULT NULL::character varying,
    "F120" character varying(255) DEFAULT NULL::character varying,
    "F121" character varying(255) DEFAULT NULL::character varying,
    "F122" character varying(255) DEFAULT NULL::character varying,
    "F123" character varying(255) DEFAULT NULL::character varying,
    "F124" character varying(255) DEFAULT NULL::character varying,
    "F125" character varying(255) DEFAULT NULL::character varying,
    "F126" character varying(255) DEFAULT NULL::character varying,
    "F127" character varying(255) DEFAULT NULL::character varying,
    "F128" character varying(255) DEFAULT NULL::character varying,
    "F129" character varying(255) DEFAULT NULL::character varying,
    "F130" character varying(255) DEFAULT NULL::character varying,
    "F131" character varying(255) DEFAULT NULL::character varying,
    "F132" character varying(255) DEFAULT NULL::character varying,
    "F133" character varying(255) DEFAULT NULL::character varying,
    "F134" character varying(255) DEFAULT NULL::character varying,
    "F135" character varying(255) DEFAULT NULL::character varying,
    "F136" character varying(255) DEFAULT NULL::character varying,
    "F137" character varying(255) DEFAULT NULL::character varying,
    "F138" character varying(255) DEFAULT NULL::character varying,
    "F139" character varying(255) DEFAULT NULL::character varying,
    "F140" character varying(255) DEFAULT NULL::character varying,
    "F141" character varying(255) DEFAULT NULL::character varying,
    "F142" character varying(255) DEFAULT NULL::character varying,
    "F143" character varying(255) DEFAULT NULL::character varying,
    "F144" character varying(255) DEFAULT NULL::character varying,
    "F145" character varying(255) DEFAULT NULL::character varying,
    "F146" character varying(255) DEFAULT NULL::character varying,
    "F147" character varying(255) DEFAULT NULL::character varying,
    "F148" character varying(255) DEFAULT NULL::character varying,
    "F149" character varying(255) DEFAULT NULL::character varying,
    "F150" character varying(255) DEFAULT NULL::character varying,
    "F151" character varying(255) DEFAULT NULL::character varying,
    "F152" character varying(255) DEFAULT NULL::character varying,
    "F153" character varying(255) DEFAULT NULL::character varying,
    "F154" character varying(255) DEFAULT NULL::character varying,
    "F155" character varying(255) DEFAULT NULL::character varying,
    "F156" character varying(255) DEFAULT NULL::character varying,
    "F157" character varying(255) DEFAULT NULL::character varying,
    "F158" character varying(255) DEFAULT NULL::character varying,
    "F159" character varying(255) DEFAULT NULL::character varying,
    "F160" character varying(255) DEFAULT NULL::character varying,
    "F161" character varying(255) DEFAULT NULL::character varying,
    "F162" character varying(255) DEFAULT NULL::character varying,
    "F163" character varying(255) DEFAULT NULL::character varying,
    "F164" character varying(255) DEFAULT NULL::character varying,
    "F165" character varying(255) DEFAULT NULL::character varying,
    "F166" character varying(255) DEFAULT NULL::character varying,
    "F167" character varying(255) DEFAULT NULL::character varying,
    "F168" character varying(255) DEFAULT NULL::character varying,
    "F169" character varying(255) DEFAULT NULL::character varying,
    "F170" character varying(255) DEFAULT NULL::character varying,
    "F171" character varying(255) DEFAULT NULL::character varying,
    "F172" character varying(255) DEFAULT NULL::character varying,
    "F173" character varying(255) DEFAULT NULL::character varying,
    "F174" character varying(255) DEFAULT NULL::character varying,
    "F175" character varying(255) DEFAULT NULL::character varying,
    "F176" character varying(255) DEFAULT NULL::character varying,
    "F177" character varying(255) DEFAULT NULL::character varying,
    "F178" character varying(255) DEFAULT NULL::character varying,
    "F179" character varying(255) DEFAULT NULL::character varying,
    "F180" character varying(255) DEFAULT NULL::character varying,
    "F181" character varying(255) DEFAULT NULL::character varying,
    "F182" character varying(255) DEFAULT NULL::character varying,
    "F183" character varying(255) DEFAULT NULL::character varying,
    "F184" character varying(255) DEFAULT NULL::character varying,
    "F185" character varying(255) DEFAULT NULL::character varying,
    "F186" character varying(255) DEFAULT NULL::character varying,
    "F187" character varying(255) DEFAULT NULL::character varying,
    "F188" character varying(255) DEFAULT NULL::character varying,
    "F189" character varying(255) DEFAULT NULL::character varying,
    "F190" character varying(255) DEFAULT NULL::character varying,
    "F191" character varying(255) DEFAULT NULL::character varying,
    "F192" character varying(255) DEFAULT NULL::character varying,
    "F193" character varying(255) DEFAULT NULL::character varying,
    "F194" character varying(255) DEFAULT NULL::character varying,
    "F195" character varying(255) DEFAULT NULL::character varying,
    "F196" character varying(255) DEFAULT NULL::character varying,
    "F197" character varying(255) DEFAULT NULL::character varying,
    "F198" character varying(255) DEFAULT NULL::character varying,
    "F199" character varying(255) DEFAULT NULL::character varying,
    "F200" character varying(255) DEFAULT NULL::character varying,
    "F201" character varying(255) DEFAULT NULL::character varying,
    "F202" character varying(255) DEFAULT NULL::character varying,
    "F203" character varying(255) DEFAULT NULL::character varying,
    "F204" character varying(255) DEFAULT NULL::character varying,
    "F205" character varying(255) DEFAULT NULL::character varying,
    "F206" character varying(255) DEFAULT NULL::character varying,
    "F207" character varying(255) DEFAULT NULL::character varying,
    "F208" character varying(255) DEFAULT NULL::character varying,
    "F209" character varying(255) DEFAULT NULL::character varying,
    "F210" character varying(255) DEFAULT NULL::character varying,
    "F211" character varying(255) DEFAULT NULL::character varying,
    "F212" character varying(255) DEFAULT NULL::character varying,
    "F213" character varying(255) DEFAULT NULL::character varying,
    "F214" character varying(255) DEFAULT NULL::character varying,
    "F215" character varying(255) DEFAULT NULL::character varying,
    "F216" character varying(255) DEFAULT NULL::character varying,
    "F217" character varying(255) DEFAULT NULL::character varying,
    "F218" character varying(255) DEFAULT NULL::character varying,
    "F219" character varying(255) DEFAULT NULL::character varying,
    "F220" character varying(255) DEFAULT NULL::character varying,
    "F221" character varying(255) DEFAULT NULL::character varying,
    "F222" character varying(255) DEFAULT NULL::character varying,
    "F223" character varying(255) DEFAULT NULL::character varying,
    "F224" character varying(255) DEFAULT NULL::character varying,
    "F225" character varying(255) DEFAULT NULL::character varying,
    "F226" character varying(255) DEFAULT NULL::character varying,
    "F227" character varying(255) DEFAULT NULL::character varying,
    "F228" character varying(255) DEFAULT NULL::character varying,
    "F229" character varying(255) DEFAULT NULL::character varying,
    "F230" character varying(255) DEFAULT NULL::character varying,
    "F231" character varying(255) DEFAULT NULL::character varying,
    "F232" character varying(255) DEFAULT NULL::character varying,
    "F233" character varying(255) DEFAULT NULL::character varying,
    "F234" character varying(255) DEFAULT NULL::character varying,
    "F235" character varying(255) DEFAULT NULL::character varying,
    "F236" character varying(255) DEFAULT NULL::character varying,
    "F237" character varying(255) DEFAULT NULL::character varying,
    "F238" character varying(255) DEFAULT NULL::character varying
);


--
-- Name: dict_quest_client_utilization; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_quest_client_utilization (
    "ReportDate" character varying(255) DEFAULT NULL::character varying,
    "FEE SCHEDULE" character varying(255) DEFAULT NULL::character varying,
    "MASTER CLIENT" double precision,
    "MASTER CLIENT NAME" character varying(255) DEFAULT NULL::character varying,
    "NTC" character varying(255) DEFAULT NULL::character varying,
    "MCL" character varying(50) DEFAULT NULL::character varying,
    "NTCX" character varying(255) DEFAULT NULL::character varying,
    "UNIT CODE" character varying(255) DEFAULT NULL::character varying,
    "TEST NAME" character varying(255) DEFAULT NULL::character varying,
    "PERF LAB" character varying(255) DEFAULT NULL::character varying,
    "TSO Flag" character varying(255) DEFAULT NULL::character varying,
    "CPT" character varying(255) DEFAULT NULL::character varying,
    "MCL MNEUMONIC" character varying(255) DEFAULT NULL::character varying,
    "CDM" character varying(50) DEFAULT NULL::character varying,
    "Patient Cost" numeric(19,4) DEFAULT NULL::numeric,
    "CURR PRICE" numeric(19,4) DEFAULT NULL::numeric,
    "Client Special Flag" character varying(255) DEFAULT NULL::character varying,
    "TOTAL REV" numeric(19,4) DEFAULT NULL::numeric,
    "CDM1" character varying(50) DEFAULT NULL::character varying
);


--
-- Name: dict_quest_exclusions_final_draft; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_quest_exclusions_final_draft (
    "CPT" character varying(5) DEFAULT NULL::character varying,
    "Description" character varying(255) DEFAULT NULL::character varying,
    age_appropriate boolean,
    outpatient_surgery boolean,
    test_cost numeric,
    start_date timestamp without time zone,
    expire_date timestamp without time zone
);


--
-- Name: dict_quest_global_billing; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_quest_global_billing (
    deleted boolean DEFAULT true,
    cdm character varying(7) DEFAULT NULL::character varying,
    cpt character varying(5) DEFAULT NULL::character varying,
    amt numeric,
    effective_date timestamp without time zone,
    end_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: dict_quest_reference_lab_tests; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_quest_reference_lab_tests (
    deleted boolean DEFAULT true NOT NULL,
    start_date timestamp without time zone,
    expire_date timestamp without time zone,
    mt_mnem character varying(50) DEFAULT NULL::character varying,
    has_multiples boolean DEFAULT true NOT NULL,
    cdm character varying(50) NOT NULL,
    cdm_description character varying(255) DEFAULT NULL::character varying,
    cpt4 character varying(5) NOT NULL,
    cpt4_description character varying(255) DEFAULT NULL::character varying,
    link integer NOT NULL,
    quest_code character varying(50) DEFAULT NULL::character varying,
    quest_description character varying(255) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    uid numeric(18,0) NOT NULL,
    amendment integer
);


--
-- Name: dict_reference_business; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_reference_business (
    deleted boolean DEFAULT true NOT NULL,
    cdm character varying(7) NOT NULL,
    description character varying(256) NOT NULL,
    cost_per_test numeric NOT NULL,
    performed_by character varying(50) NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: dict_series_pmt_and_adjustment_codes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_series_pmt_and_adjustment_codes (
    p_m_text character varying(256) NOT NULL,
    p_m_code character varying(7) DEFAULT NULL::character varying,
    p_m_desc character varying(256) DEFAULT NULL::character varying,
    p_m_type character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone
);


--
-- Name: dict_write_off_codes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.dict_write_off_codes (
    write_off_code character varying(4) NOT NULL,
    write_off_description character varying(255) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_date timestamp without time zone,
    mod_host character varying(50) NOT NULL
);


--
-- Name: emp; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.emp (
    name character varying(20) NOT NULL,
    access character varying(20) DEFAULT NULL::character varying,
    mainmenu character varying(15) DEFAULT NULL::character varying,
    access_edit_dictionary boolean DEFAULT true NOT NULL,
    access_bad_debt boolean DEFAULT true NOT NULL,
    access_billing boolean DEFAULT true NOT NULL,
    access_fin_code boolean DEFAULT true NOT NULL,
    add_chrg boolean DEFAULT true NOT NULL,
    add_chk boolean DEFAULT true NOT NULL,
    add_chk_amt boolean DEFAULT true NOT NULL,
    reserve4 boolean DEFAULT true NOT NULL,
    reserve5 boolean DEFAULT true NOT NULL,
    reserve6 boolean DEFAULT true NOT NULL,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    full_name character varying(35) DEFAULT NULL::character varying,
    password character varying(100) DEFAULT NULL::character varying,
    menu_profile_id integer,
    impersonate boolean DEFAULT true NOT NULL
);


--
-- Name: emp_removed; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.emp_removed (
    name character varying(20) NOT NULL,
    access character varying(20) DEFAULT NULL::character varying,
    mainmenu character varying(15) DEFAULT NULL::character varying,
    access_edit_dictionary boolean NOT NULL,
    access_bad_debt boolean NOT NULL,
    access_billing boolean NOT NULL,
    access_fin_code boolean NOT NULL,
    add_chrg boolean NOT NULL,
    add_chk boolean NOT NULL,
    add_chk_amt boolean NOT NULL,
    reserve4 boolean NOT NULL,
    reserve5 boolean NOT NULL,
    reserve6 boolean NOT NULL,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    full_name character varying(35) DEFAULT NULL::character varying
);


--
-- Name: error_prg; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.error_prg (
    "PROCESSED" boolean DEFAULT true,
    error_type character varying(50) DEFAULT NULL::character varying,
    app_name character varying(50) NOT NULL,
    app_module character varying(1024) DEFAULT NULL::character varying,
    error character varying NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    uid bigint NOT NULL
);


--
-- Name: error_prg_uid_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.error_prg_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: error_prg_uid_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.error_prg_uid_seq OWNED BY dbo.error_prg.uid;


--
-- Name: fin; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.fin (
    deleted boolean DEFAULT true NOT NULL,
    fin_code character varying(10) NOT NULL,
    res_party character varying(40) DEFAULT NULL::character varying,
    form_type character varying(30) DEFAULT NULL::character varying,
    chrgsource character varying(20) DEFAULT NULL::character varying,
    type character varying(1) DEFAULT NULL::character varying,
    h1500 character varying(1) DEFAULT NULL::character varying,
    ub92 character varying(1) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL
);


--
-- Name: h1500; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.h1500 (
    rowguid uuid DEFAULT public.uuid_generate_v4(),
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(50) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    run_date timestamp without time zone,
    printed boolean DEFAULT true NOT NULL,
    run_user character varying(20) NOT NULL,
    batch numeric DEFAULT '-1'::numeric NOT NULL,
    date_sent timestamp without time zone,
    sent_user character varying(20) DEFAULT NULL::character varying,
    ebill_status character varying(5) DEFAULT NULL::character varying,
    ebill_batch numeric,
    text character varying,
    cold_feed timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: hp_prices_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.hp_prices_cnb (
    cpt_code character varying(50) DEFAULT NULL::character varying,
    modi character varying(50) DEFAULT NULL::character varying,
    al_price character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone
);


--
-- Name: icd9desc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.icd9desc (
    deleted boolean DEFAULT true NOT NULL,
    version character varying(50) NOT NULL,
    icd9_num character varying(7) NOT NULL,
    icd9_desc character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(40) DEFAULT NULL::character varying,
    mod_prg character varying(40) DEFAULT NULL::character varying,
    "AMA_year" character varying(6) NOT NULL,
    id bigint NOT NULL
);


--
-- Name: icd9desc_wdk; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.icd9desc_wdk (
    deleted boolean,
    icd9_num character varying(7) NOT NULL,
    icd9_desc character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(40) DEFAULT NULL::character varying,
    mod_prg character varying(40) DEFAULT NULL::character varying,
    "AMA_year" character varying(6) NOT NULL
);


--
-- Name: ins; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.ins (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    ins_a_b_c character varying(1) NOT NULL,
    holder_nme character varying(40) DEFAULT NULL::character varying,
    holder_dob timestamp without time zone,
    holder_sex character varying(1) DEFAULT NULL::character varying,
    holder_addr character varying(40) DEFAULT NULL::character varying,
    holder_city_st_zip character varying(40) DEFAULT NULL::character varying,
    plan_nme character varying(45) DEFAULT NULL::character varying,
    plan_addr1 character varying(40) DEFAULT NULL::character varying,
    plan_addr2 character varying(40) DEFAULT NULL::character varying,
    p_city_st character varying(40) DEFAULT NULL::character varying,
    policy_num character varying(50) DEFAULT NULL::character varying,
    cert_ssn character varying(15) DEFAULT NULL::character varying,
    grp_nme character varying(50) DEFAULT NULL::character varying,
    grp_num character varying(15) DEFAULT NULL::character varying,
    employer character varying(25) DEFAULT NULL::character varying,
    e_city_st character varying(35) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    holder_lname character varying(40) DEFAULT NULL::character varying,
    holder_fname character varying(40) DEFAULT NULL::character varying,
    holder_mname character varying(40) DEFAULT NULL::character varying,
    plan_effective_date timestamp without time zone,
    plan_expiration_date timestamp without time zone
);


--
-- Name: ins_backup; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.ins_backup (
    rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ins_a_b_c character varying(1) NOT NULL,
    holder_nme character varying(40) DEFAULT NULL::character varying,
    holder_dob timestamp without time zone,
    holder_sex character varying(1) DEFAULT NULL::character varying,
    holder_addr character varying(40) DEFAULT NULL::character varying,
    holder_city_st_zip character varying(40) DEFAULT NULL::character varying,
    plan_nme character varying(45) DEFAULT NULL::character varying,
    plan_addr1 character varying(40) DEFAULT NULL::character varying,
    plan_addr2 character varying(40) DEFAULT NULL::character varying,
    p_city_st character varying(40) DEFAULT NULL::character varying,
    policy_num character varying(50) DEFAULT NULL::character varying,
    cert_ssn character varying(15) DEFAULT NULL::character varying,
    grp_nme character varying(50) DEFAULT NULL::character varying,
    grp_num character varying(15) DEFAULT NULL::character varying,
    employer character varying(25) DEFAULT NULL::character varying,
    e_city_st character varying(35) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    holder_lname character varying(40) DEFAULT NULL::character varying,
    holder_fname character varying(40) DEFAULT NULL::character varying,
    holder_mname character varying(40) DEFAULT NULL::character varying,
    plan_effective_date timestamp without time zone,
    plan_expiration_date timestamp without time zone
);


--
-- Name: insc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.insc (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    code character varying(10) NOT NULL,
    name character varying(45) DEFAULT NULL::character varying,
    addr1 character varying(30) DEFAULT NULL::character varying,
    addr2 character varying(30) DEFAULT NULL::character varying,
    citystzip character varying(30) DEFAULT NULL::character varying,
    provider_no_qualifier character varying(3) DEFAULT NULL::character varying,
    provider_no character varying(20) DEFAULT NULL::character varying,
    payer_no character varying(50) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(10) DEFAULT NULL::character varying,
    bill_form character varying(5) DEFAULT NULL::character varying,
    num_labels integer DEFAULT 0,
    fin_code character varying(10) DEFAULT NULL::character varying,
    comment character varying(250) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    is_mc_hmo boolean,
    allow_outpatient_billing boolean DEFAULT true NOT NULL,
    payor_code character varying(8000) DEFAULT NULL::character varying,
    fin_class character varying(10) DEFAULT NULL::character varying,
    bill_as_jmcgh boolean
);


--
-- Name: insc_payor; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.insc_payor (
    name character varying(255) NOT NULL,
    insc_code character varying(50) NOT NULL,
    fin_code character varying(50) NOT NULL,
    uid bigint NOT NULL
);


--
-- Name: insc_payor_uid_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.insc_payor_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: insc_payor_uid_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.insc_payor_uid_seq OWNED BY dbo.insc_payor.uid;


--
-- Name: job_track_wdk_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.job_track_wdk_del (
    uid bigint NOT NULL,
    job_name character varying(150) NOT NULL,
    job_count numeric NOT NULL,
    mod_date timestamp without time zone
);


--
-- Name: job_track_wdk_del_uid_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.job_track_wdk_del_uid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: job_track_wdk_del_uid_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.job_track_wdk_del_uid_seq OWNED BY dbo.job_track_wdk_del.uid;


--
-- Name: lcd_lmrp; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.lcd_lmrp (
    lcd_cpt4 character varying(5) NOT NULL,
    lcd_beg_icd9 character varying(7) NOT NULL,
    lcd_end_icd9 character varying(7) NOT NULL,
    lcd_ama_year character varying(6) NOT NULL,
    lcd_chk_for_bad integer,
    lcd_expiration_date timestamp without time zone
);


--
-- Name: lcd_lmrp_old; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.lcd_lmrp_old (
    lcd_cpt4 character varying(5) NOT NULL,
    lcd_beg_icd9 character varying(7) NOT NULL,
    lcd_end_icd9 character varying(7) NOT NULL,
    lcd_ama_year character varying(6) NOT NULL,
    lcd_expiration_date timestamp without time zone
);


--
-- Name: medicare_exclusions; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.medicare_exclusions (
    cpt_code character varying(5) NOT NULL,
    effective_date timestamp without time zone NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: menu; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.menu (
    menuid character varying(15) NOT NULL,
    itemno numeric NOT NULL,
    description character varying(30) DEFAULT NULL::character varying,
    command character varying(15) DEFAULT NULL::character varying,
    argument character varying(100) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: month_end_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.month_end_del (
    account character varying(15) NOT NULL,
    datestamp timestamp without time zone NOT NULL,
    balance numeric(19,4) DEFAULT NULL::numeric,
    fin_code character varying(10) DEFAULT NULL::character varying,
    ins_code character varying(10) DEFAULT NULL::character varying
);


--
-- Name: monthlycharges_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.monthlycharges_del (
    rowid bigint NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    amt numeric(19,4) DEFAULT '0'::numeric,
    "nYear" integer,
    "nMonth" integer,
    fincodetotal numeric(19,4) DEFAULT '0'::numeric,
    "finCode" character varying(10) DEFAULT NULL::character varying,
    "finType" character varying(10) DEFAULT NULL::character varying,
    audit_moddate timestamp without time zone,
    chrg_moddate timestamp without time zone,
    service_date timestamp without time zone,
    huhdate timestamp without time zone
);


--
-- Name: monthlycharges_del_rowid_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.monthlycharges_del_rowid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: monthlycharges_del_rowid_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.monthlycharges_del_rowid_seq OWNED BY dbo.monthlycharges_del.rowid;


--
-- Name: mutually_excl; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.mutually_excl (
    cpt4_1 character varying(5) NOT NULL,
    cpt4_2 character varying(5) NOT NULL,
    version character varying(5) DEFAULT NULL::character varying,
    mod_user character varying(30) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(30) DEFAULT NULL::character varying,
    mod_host character varying(30) DEFAULT NULL::character varying,
    modi_indicator character varying(1) DEFAULT NULL::character varying
);


--
-- Name: my_who2; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.my_who2 (
    spid integer,
    status character varying(255) DEFAULT NULL::character varying,
    login character varying(255) DEFAULT NULL::character varying,
    hostname character varying(255) DEFAULT NULL::character varying,
    blkby character varying(255) DEFAULT NULL::character varying,
    dbname character varying(255) DEFAULT NULL::character varying,
    command character varying(2048) DEFAULT NULL::character varying,
    "CPUTime" integer,
    "DiskIO" integer,
    "LastBatch" character varying(255) DEFAULT NULL::character varying,
    programname character varying(255) DEFAULT NULL::character varying,
    spid2 integer
);


--
-- Name: notes; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.notes (
    account character varying(15) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    comment character varying,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL
);


--
-- Name: number; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.number (
    keyfield character varying(15) NOT NULL,
    cnt numeric
);


--
-- Name: outputCost; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."outputCost" (
    cdm character varying(7) DEFAULT NULL::character varying,
    descript character varying(256) DEFAULT NULL::character varying,
    "oldCost" numeric(19,4) DEFAULT NULL::numeric,
    "newCost" numeric(19,4) DEFAULT NULL::numeric,
    mod_date timestamp without time zone
);


--
-- Name: ov_chrg_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.ov_chrg_cnb (
    the_msg_no character varying(15) NOT NULL,
    account character varying(50) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    test character varying(50) NOT NULL,
    qty integer NOT NULL,
    file_nme character varying(50) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    run_date timestamp without time zone
);


--
-- Name: pat; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.pat (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    ssn character varying(11) DEFAULT NULL::character varying,
    pat_addr1 character varying(40) DEFAULT NULL::character varying,
    pat_addr2 character varying(40) DEFAULT NULL::character varying,
    city_st_zip character varying(40) DEFAULT NULL::character varying,
    dob_yyyy timestamp without time zone,
    sex character varying(1) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    guarantor character varying(40) DEFAULT NULL::character varying,
    guar_addr character varying(40) DEFAULT NULL::character varying,
    g_city_st character varying(50) DEFAULT NULL::character varying,
    pat_marital character varying(1) DEFAULT NULL::character varying,
    icd9_1 character varying(7) DEFAULT NULL::character varying,
    icd9_2 character varying(7) DEFAULT NULL::character varying,
    icd9_3 character varying(7) DEFAULT NULL::character varying,
    icd9_4 character varying(7) DEFAULT NULL::character varying,
    icd9_5 character varying(7) DEFAULT NULL::character varying,
    icd9_6 character varying(7) DEFAULT NULL::character varying,
    icd9_7 character varying(7) DEFAULT NULL::character varying,
    icd9_8 character varying(7) DEFAULT NULL::character varying,
    icd9_9 character varying(7) DEFAULT NULL::character varying,
    icd_indicator character varying(3) DEFAULT NULL::character varying,
    pc_code integer,
    mailer character varying(1) DEFAULT NULL::character varying,
    first_dm timestamp without time zone,
    last_dm timestamp without time zone,
    min_amt numeric(19,4) DEFAULT 0.00,
    phy_id character varying(15) DEFAULT NULL::character varying,
    dbill_date timestamp without time zone,
    ub_date timestamp without time zone,
    h1500_date timestamp without time zone,
    ssi_batch character varying(50) DEFAULT NULL::character varying,
    colltr_date timestamp without time zone,
    baddebt_date timestamp without time zone,
    batch_date timestamp without time zone,
    guar_phone character varying(13) DEFAULT NULL::character varying,
    bd_list_date timestamp without time zone,
    ebill_batch_date timestamp without time zone,
    ebill_batch_1500 timestamp without time zone,
    e_ub_demand boolean DEFAULT true NOT NULL,
    e_ub_demand_date timestamp without time zone,
    claimsnet_1500_batch_date timestamp without time zone,
    claimsnet_ub_batch_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    hne_epi_number character varying(50) DEFAULT NULL::character varying,
    pat_full_name character varying(128) DEFAULT NULL::character varying,
    pat_city character varying(50) DEFAULT NULL::character varying,
    pat_state character varying(2) DEFAULT NULL::character varying,
    pat_zip character varying(10) DEFAULT NULL::character varying,
    guar_city character varying(50) DEFAULT NULL::character varying,
    guar_state character varying(2) DEFAULT NULL::character varying,
    guar_zip character varying(10) DEFAULT NULL::character varying,
    pat_race character varying(5) DEFAULT NULL::character varying,
    pat_phone character varying(25) DEFAULT NULL::character varying,
    phy_comment character varying(128) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    pat_email character varying(256) DEFAULT NULL::character varying,
    dx_update_prg character varying(50) DEFAULT NULL::character varying
);


--
-- Name: pat_backup; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.pat_backup (
    rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ssn character varying(11) DEFAULT NULL::character varying,
    pat_addr1 character varying(40) DEFAULT NULL::character varying,
    pat_addr2 character varying(40) DEFAULT NULL::character varying,
    city_st_zip character varying(40) DEFAULT NULL::character varying,
    dob_yyyy timestamp without time zone,
    sex character varying(1) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    guarantor character varying(40) DEFAULT NULL::character varying,
    guar_addr character varying(40) DEFAULT NULL::character varying,
    g_city_st character varying(50) DEFAULT NULL::character varying,
    pat_marital character varying(1) DEFAULT NULL::character varying,
    icd9_1 character varying(7) DEFAULT NULL::character varying,
    icd9_2 character varying(7) DEFAULT NULL::character varying,
    icd9_3 character varying(7) DEFAULT NULL::character varying,
    icd9_4 character varying(7) DEFAULT NULL::character varying,
    icd9_5 character varying(7) DEFAULT NULL::character varying,
    icd9_6 character varying(7) DEFAULT NULL::character varying,
    icd9_7 character varying(7) DEFAULT NULL::character varying,
    icd9_8 character varying(7) DEFAULT NULL::character varying,
    icd9_9 character varying(7) DEFAULT NULL::character varying,
    icd_indicator character varying(3) DEFAULT NULL::character varying,
    pc_code integer,
    mailer character varying(1) DEFAULT NULL::character varying,
    first_dm timestamp without time zone,
    last_dm timestamp without time zone,
    min_amt numeric(19,4) DEFAULT NULL::numeric,
    phy_id character varying(15) DEFAULT NULL::character varying,
    dbill_date timestamp without time zone,
    ub_date timestamp without time zone,
    h1500_date timestamp without time zone,
    ssi_batch character varying(50) DEFAULT NULL::character varying,
    colltr_date timestamp without time zone,
    baddebt_date timestamp without time zone,
    batch_date timestamp without time zone,
    guar_phone character varying(13) DEFAULT NULL::character varying,
    bd_list_date timestamp without time zone,
    ebill_batch_date timestamp without time zone,
    ebill_batch_1500 timestamp without time zone,
    e_ub_demand boolean NOT NULL,
    e_ub_demand_date timestamp without time zone,
    claimsnet_1500_batch_date timestamp without time zone,
    claimsnet_ub_batch_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    hne_epi_number character varying(50) DEFAULT NULL::character varying,
    pat_full_name character varying(128) DEFAULT NULL::character varying,
    pat_city character varying(50) DEFAULT NULL::character varying,
    pat_state character varying(2) DEFAULT NULL::character varying,
    pat_zip character varying(10) DEFAULT NULL::character varying,
    guar_city character varying(50) DEFAULT NULL::character varying,
    guar_state character varying(2) DEFAULT NULL::character varying,
    guar_zip character varying(10) DEFAULT NULL::character varying,
    pat_race character varying(5) DEFAULT NULL::character varying,
    pat_phone character varying(25) DEFAULT NULL::character varying,
    phy_comment character varying(128) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    pat_email character varying(256) DEFAULT NULL::character varying,
    dx_update_prg character varying(50) DEFAULT NULL::character varying
);


--
-- Name: pat_statements_cerner; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.pat_statements_cerner (
    statement_type character varying(50) DEFAULT NULL::character varying,
    statement_type_id character varying(50) DEFAULT NULL::character varying,
    account character varying(15) DEFAULT NULL::character varying,
    statement_text character varying NOT NULL,
    batch_id character varying(50) NOT NULL
);


--
-- Name: pat_ub_update; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.pat_ub_update (
    rowguid uuid NOT NULL,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ssn character varying(11) DEFAULT NULL::character varying,
    pat_addr1 character varying(40) DEFAULT NULL::character varying,
    pat_addr2 character varying(40) DEFAULT NULL::character varying,
    city_st_zip character varying(40) DEFAULT NULL::character varying,
    dob_yyyy timestamp without time zone,
    sex character varying(1) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    guarantor character varying(40) DEFAULT NULL::character varying,
    guar_addr character varying(40) DEFAULT NULL::character varying,
    g_city_st character varying(40) DEFAULT NULL::character varying,
    pat_marital character varying(1) DEFAULT NULL::character varying,
    icd9_1 character varying(7) DEFAULT NULL::character varying,
    icd9_2 character varying(7) DEFAULT NULL::character varying,
    icd9_3 character varying(7) DEFAULT NULL::character varying,
    icd9_4 character varying(7) DEFAULT NULL::character varying,
    icd9_5 character varying(7) DEFAULT NULL::character varying,
    icd9_6 character varying(7) DEFAULT NULL::character varying,
    icd9_7 character varying(7) DEFAULT NULL::character varying,
    icd9_8 character varying(7) DEFAULT NULL::character varying,
    icd9_9 character varying(7) DEFAULT NULL::character varying,
    pc_code integer,
    mailer character varying(1) DEFAULT NULL::character varying,
    first_dm timestamp without time zone,
    last_dm timestamp without time zone,
    min_amt numeric(19,4) DEFAULT NULL::numeric,
    phy_id character varying(15) DEFAULT NULL::character varying,
    dbill_date timestamp without time zone,
    ub_date timestamp without time zone,
    h1500_date timestamp without time zone,
    colltr_date timestamp without time zone,
    baddebt_date timestamp without time zone,
    batch_date timestamp without time zone,
    guar_phone character varying(13) DEFAULT NULL::character varying,
    bd_list_date timestamp without time zone,
    ebill_batch_date timestamp without time zone,
    ebill_batch_1500 timestamp without time zone,
    e_ub_demand boolean NOT NULL,
    e_ub_demand_date timestamp without time zone,
    claimsnet_1500_batch_date timestamp without time zone,
    claimsnet_ub_batch_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: pat_zip_update; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.pat_zip_update (
    rowguid uuid,
    deleted boolean NOT NULL,
    account character varying(15) NOT NULL,
    ssn character varying(11) DEFAULT NULL::character varying,
    pat_addr1 character varying(40) DEFAULT NULL::character varying,
    pat_addr2 character varying(40) DEFAULT NULL::character varying,
    city_st_zip character varying(40) DEFAULT NULL::character varying,
    dob_yyyy timestamp without time zone,
    sex character varying(1) DEFAULT NULL::character varying,
    relation character varying(2) DEFAULT NULL::character varying,
    guarantor character varying(40) DEFAULT NULL::character varying,
    guar_addr character varying(40) DEFAULT NULL::character varying,
    g_city_st character varying(50) DEFAULT NULL::character varying,
    pat_marital character varying(1) DEFAULT NULL::character varying,
    icd9_1 character varying(7) DEFAULT NULL::character varying,
    icd9_2 character varying(7) DEFAULT NULL::character varying,
    icd9_3 character varying(7) DEFAULT NULL::character varying,
    icd9_4 character varying(7) DEFAULT NULL::character varying,
    icd9_5 character varying(7) DEFAULT NULL::character varying,
    icd9_6 character varying(7) DEFAULT NULL::character varying,
    icd9_7 character varying(7) DEFAULT NULL::character varying,
    icd9_8 character varying(7) DEFAULT NULL::character varying,
    icd9_9 character varying(7) DEFAULT NULL::character varying,
    icd_indicator character varying(3) DEFAULT NULL::character varying,
    pc_code integer,
    mailer character varying(1) DEFAULT NULL::character varying,
    first_dm timestamp without time zone,
    last_dm timestamp without time zone,
    min_amt numeric(19,4) DEFAULT NULL::numeric,
    phy_id character varying(15) DEFAULT NULL::character varying,
    dbill_date timestamp without time zone,
    ub_date timestamp without time zone,
    h1500_date timestamp without time zone,
    ssi_batch character varying(50) DEFAULT NULL::character varying,
    colltr_date timestamp without time zone,
    baddebt_date timestamp without time zone,
    batch_date timestamp without time zone,
    guar_phone character varying(13) DEFAULT NULL::character varying,
    bd_list_date timestamp without time zone,
    ebill_batch_date timestamp without time zone,
    ebill_batch_1500 timestamp without time zone,
    e_ub_demand boolean NOT NULL,
    e_ub_demand_date timestamp without time zone,
    claimsnet_1500_batch_date timestamp without time zone,
    claimsnet_ub_batch_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    hne_epi_number character varying(50) DEFAULT NULL::character varying,
    pat_full_name character varying(128) DEFAULT NULL::character varying,
    pat_city character varying(50) DEFAULT NULL::character varying,
    pat_state character varying(2) DEFAULT NULL::character varying,
    pat_zip character varying(10) DEFAULT NULL::character varying,
    guar_city character varying(50) DEFAULT NULL::character varying,
    guar_state character varying(2) DEFAULT NULL::character varying,
    guar_zip character varying(10) DEFAULT NULL::character varying,
    pat_race character varying(5) DEFAULT NULL::character varying,
    pat_phone character varying(25) DEFAULT NULL::character varying,
    phy_comment character varying(128) DEFAULT NULL::character varying,
    location character varying(50) DEFAULT NULL::character varying,
    pat_email character varying(256) DEFAULT NULL::character varying,
    dx_update_prg character varying(50) DEFAULT NULL::character varying,
    guar_update boolean DEFAULT true
);


--
-- Name: patbill_acc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.patbill_acc (
    statement_number double precision,
    record_cnt_acct character varying(30) DEFAULT NULL::character varying,
    patient_account_number character varying(15) NOT NULL,
    account_id character varying(50) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    account_subtotal character varying(8) NOT NULL,
    total_account_subtotal numeric,
    acct_amt_due numeric,
    acct_ins_pending numeric NOT NULL,
    acct_dates_of_service character varying(30) DEFAULT NULL::character varying,
    acct_unpaid_bal numeric,
    acct_patient_bal numeric,
    acct_paid_since_last_stmt numeric,
    acct_ins_discount numeric,
    acct_date_due character varying(30) DEFAULT NULL::character varying,
    acct_health_plan_name character varying(45) DEFAULT NULL::character varying,
    patient_date_of_birth character varying(30) DEFAULT NULL::character varying,
    patient_date_of_death character varying(1) NOT NULL,
    patient_sex character varying(6) DEFAULT NULL::character varying,
    patient_vip character varying(1) NOT NULL,
    includes_est_pat_lib integer NOT NULL,
    total_charge_amt numeric,
    non_covered_charge_amt integer NOT NULL,
    "ABN_charge_amt" integer NOT NULL,
    est_contract_allowance_amt_ind integer NOT NULL,
    est_contract_allowance_amt integer NOT NULL,
    encntr_deductible_rem_amt_ind integer NOT NULL,
    deductible_applied_amt integer NOT NULL,
    encntr_copay_amt_ind integer NOT NULL,
    encntr_copay_amt integer NOT NULL,
    encntr_coinsurance_pct_ind integer NOT NULL,
    encntr_coinsurance_pct integer NOT NULL,
    encntr_coinsurance_amt integer NOT NULL,
    maximum_out_of_pocket_amt_ind integer NOT NULL,
    amt_over_max_out_of_pocket integer NOT NULL,
    est_patient_liab_amt integer NOT NULL,
    acc_msg character varying(9) NOT NULL,
    mailer character varying(1) DEFAULT NULL::character varying,
    first_data_mailer timestamp without time zone,
    last_data_mailer timestamp without time zone,
    mailer_count integer,
    processed_date timestamp without time zone,
    date_sent timestamp without time zone,
    aging_bucket_current numeric(19,4) DEFAULT NULL::numeric,
    aging_bucket_30 numeric(19,4) DEFAULT NULL::numeric,
    aging_bucket_60 numeric(19,4) DEFAULT NULL::numeric,
    aging_bucket_90 numeric(19,4) DEFAULT NULL::numeric,
    batch_id character varying(50) NOT NULL,
    uid bigint NOT NULL
);


--
-- Name: patbill_enctr; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.patbill_enctr (
    record_type character varying(4) NOT NULL,
    record_cnt bigint,
    statement_number double precision,
    enctr_nbr character varying(15) NOT NULL,
    pft_encntr_id character varying(50) DEFAULT NULL::character varying,
    place_of_service character varying(50) DEFAULT NULL::character varying,
    pft_encntr_dates_of_service character varying(10) DEFAULT NULL::character varying,
    pft_encntr_amt_due character varying(10) DEFAULT NULL::character varying,
    pft_encntr_prov_name character varying(1) NOT NULL,
    pft_encntr_prov_org_name character varying(1) NOT NULL,
    pft_encntr_prov_org_str_addr_ character varying(1) NOT NULL,
    pft_encntr_prov_org_str_addr_2 character varying(1) NOT NULL,
    pft_encntr_prov_org_str_addr_3 character varying(1) NOT NULL,
    pft_encntr_prov_org_str_addr_4 character varying(1) NOT NULL,
    pft_encntr_prov_org_city character varying(1) NOT NULL,
    pft_encntr_prov_org_state character varying(1) NOT NULL,
    pft_encntr_prov_org_zip character varying(1) NOT NULL,
    pft_encntr_prov_org_phone character varying(1) NOT NULL,
    pft_encntr_prov_hrs character varying(1) NOT NULL,
    pft_encntr_unpaid_bal character varying(1) NOT NULL,
    pft_encntr_patient_bal numeric NOT NULL,
    pft_encntr_paid_since_last_stmt numeric NOT NULL,
    pft_encntr_ins_discount character varying(1) NOT NULL,
    pft_encntr_ord_mgmt_act_type character varying(1) NOT NULL,
    pft_encntr_ord_mgmt_cat_type character varying(1) NOT NULL,
    pft_encntr_health_plan_name character varying(1) NOT NULL,
    pft_encntr_in_pending numeric NOT NULL,
    pft_encntr_total character varying(10) DEFAULT NULL::character varying,
    encntr_admit_dt_tm character varying(10) DEFAULT NULL::character varying,
    encntr_discharge_dt_tm character varying(10) DEFAULT NULL::character varying,
    encntr_medical_service character varying(1) NOT NULL,
    encntr_type character varying(20) NOT NULL,
    encntr_financial_class character varying(1) NOT NULL,
    encntr_vip character varying(1) NOT NULL,
    pft_encntr_qualifier character varying(1) NOT NULL,
    pft_encntr_total_charges character varying(10) DEFAULT NULL::character varying,
    total_patient_payments numeric NOT NULL,
    total_patient_adjustments numeric NOT NULL,
    total_insurance_payments character varying(1) NOT NULL,
    total_insurance_adjustments character varying(1) NOT NULL,
    pft_encntr_assigned_agency character varying(1) NOT NULL,
    pft_encntr_pay_plan_flag character varying(1) NOT NULL,
    pft_encntr_pay_plan_status character varying(1) NOT NULL,
    pft_encntr_pay_plan_orig_amt character varying(1) NOT NULL,
    pft_encntr_pay_plan_pay_amt character varying(1) NOT NULL,
    pft_encntr_pay_plan_begin_dttm character varying(1) NOT NULL,
    pft_encntr_pay_plan_delinq_amt character varying(1) NOT NULL,
    pftectr_pri_clm_orig_trans_dttm character varying(1) NOT NULL,
    pftectr_pri_clm_cur_trans_dttm character varying(1) NOT NULL,
    pftectr_sec_clm_orig_trans_dttm character varying(1) NOT NULL,
    pftectr_sec_clm_cur_trans_dttm character varying(1) NOT NULL,
    pftectr_ter_clm_orig_trans_dttm character varying(1) NOT NULL,
    pftectr_ter_clm_cur_trans_dttm character varying(1) NOT NULL,
    pft_ectr_prim_insr_balance character varying(1) NOT NULL,
    pft_ectr_sec_insr_balance character varying(1) NOT NULL,
    pft_ectr_tert_insr_balance character varying(1) NOT NULL,
    pft_ectr_self_pay_balance character varying(1) NOT NULL,
    attending_physician_name character varying(1) NOT NULL,
    includes_est_pat_liab integer NOT NULL,
    total_charge_amount integer NOT NULL,
    non_covered_charge_amt integer NOT NULL,
    "ABN_charge_amt" integer NOT NULL,
    est_contract_allowance_amt_ind integer NOT NULL,
    est_contract_allowance_amt integer NOT NULL,
    encntr_deductible_rem_amt_ind integer NOT NULL,
    encntr_deductible_rem_amt integer NOT NULL,
    deductible_applied_amt integer NOT NULL,
    encntr_copay_amt_ind integer NOT NULL,
    encntr_copay_amt integer NOT NULL,
    encntr_coinsurance_pct_ind integer NOT NULL,
    encntr_coinsurance_pct integer NOT NULL,
    encntr_coinsurance_amt integer NOT NULL,
    maximum_out_of_pocket_amt_ind integer NOT NULL,
    maximum_out_of_pocket_amt integer NOT NULL,
    amt_over_max_out_of_pocket integer NOT NULL,
    est_patient_liab_amt integer NOT NULL,
    batch_id character varying(50) NOT NULL
);


--
-- Name: patbill_enctr_actv; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.patbill_enctr_actv (
    statement_number double precision NOT NULL,
    record_type character varying(4) NOT NULL,
    record_cnt bigint NOT NULL,
    enctr_nbr character varying(15) NOT NULL,
    activity_id character varying(50) NOT NULL,
    activity_type character varying(1) NOT NULL,
    activity_date character varying(10) DEFAULT NULL::character varying,
    activity_description character varying(9) NOT NULL,
    activity_code character varying(1) NOT NULL,
    activity_amount character varying(1) NOT NULL,
    units numeric,
    cpt_code character varying(7) DEFAULT NULL::character varying,
    cpt_description character varying(50) DEFAULT NULL::character varying,
    drg_code character varying(1) NOT NULL,
    revenue_code character varying(5) DEFAULT NULL::character varying,
    revenue_code_description character varying(1) NOT NULL,
    hcpcs_code character varying(1) NOT NULL,
    hcpcs_description character varying(1) NOT NULL,
    order_mgmt_activity_type character varying(1) NOT NULL,
    activity_amount_due numeric(19,4) DEFAULT NULL::numeric,
    activity_date_of_service character varying(10) DEFAULT NULL::character varying,
    activity_patient_bal numeric NOT NULL,
    activity_ins_discount numeric NOT NULL,
    activity_trans_type character varying(6) NOT NULL,
    activity_trans_sub_type character varying(1) NOT NULL,
    activity_trans_amount numeric,
    activity_health_plan_name character varying(1) NOT NULL,
    activity_ins_pending numeric NOT NULL,
    activity_dr_cr_flag integer NOT NULL,
    parent_activity_id bigint NOT NULL,
    batch_id character varying(50) NOT NULL
);


--
-- Name: patbill_stmt; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.patbill_stmt (
    record_type character varying(4) NOT NULL,
    record_cnt bigint NOT NULL,
    statement_number double precision NOT NULL,
    billing_entity_street character varying(8000) DEFAULT NULL::character varying,
    billing_entity_city character varying(8000) DEFAULT NULL::character varying,
    billing_entity_state character varying(8000) DEFAULT NULL::character varying,
    billing_entity_zip character varying(8000) DEFAULT NULL::character varying,
    billing_entity_federal_tax_id character varying(8000) DEFAULT NULL::character varying,
    billing_entity_name character varying(8000) DEFAULT NULL::character varying,
    billing_entity_phone_number character varying(8000) DEFAULT NULL::character varying,
    billing_entity_fax_number character varying(8000) DEFAULT NULL::character varying,
    remit_to_street character varying(8000) DEFAULT NULL::character varying,
    remit_to_street2 character varying(8000) DEFAULT NULL::character varying,
    remit_to_city character varying(8000) DEFAULT NULL::character varying,
    remit_to_state character varying(8000) DEFAULT NULL::character varying,
    remit_to_zip character varying(8000) DEFAULT NULL::character varying,
    remit_to_org_name character varying(8000) DEFAULT NULL::character varying,
    guarantor_street character varying(40) DEFAULT NULL::character varying,
    guarantor_street_2 character varying(1) NOT NULL,
    guarantor_city character varying(50) DEFAULT NULL::character varying,
    guarantor_state character varying(50) DEFAULT NULL::character varying,
    guarantor_zip character varying(50) DEFAULT NULL::character varying,
    guarantor_name character varying(128) DEFAULT NULL::character varying,
    amount_due numeric,
    date_due character varying(30) DEFAULT NULL::character varying,
    balance_forward numeric,
    aging_bucket_current numeric(19,4) DEFAULT NULL::numeric,
    aging_bucket_30_day numeric(19,4) DEFAULT NULL::numeric,
    aging_bucket_60_day numeric(19,4) DEFAULT NULL::numeric,
    aging_bucket_90_day numeric(19,4) DEFAULT NULL::numeric,
    statement_total_amount numeric,
    insurance_billed_amount character varying(1) NOT NULL,
    balance_forward_amount character varying(1) NOT NULL,
    balance_forward_date character varying(1) NOT NULL,
    primary_health_plan_name character varying(1) NOT NULL,
    prim_health_plan_policy_number character varying(1) NOT NULL,
    prim_health_plan_group_number character varying(1) NOT NULL,
    secondary_health_plan_name character varying(1) NOT NULL,
    sec_health_plan_policy_number character varying(1) NOT NULL,
    sec_health_plan_group_number character varying(1) NOT NULL,
    tertiary_health_plan_name character varying(1) NOT NULL,
    ter_health_plan_policy_number character varying(1) NOT NULL,
    ter_health_plan_group_number character varying(1) NOT NULL,
    statement_time character varying(5) NOT NULL,
    page_number character varying(1) NOT NULL,
    insurance_pending numeric NOT NULL,
    unpaid_balance numeric,
    patient_balance numeric,
    totat_paid_since_last_stmt numeric,
    insurance_discount numeric NOT NULL,
    "contact text" character varying(1) NOT NULL,
    transmission_dt_tm character varying(1) NOT NULL,
    guarantor_country character varying(3) NOT NULL,
    guarantor_ssn character varying(1) NOT NULL,
    guarantor_phone character varying(25) DEFAULT NULL::character varying,
    statement_submitted_dt_tm timestamp without time zone,
    includes_est_pat_lib integer NOT NULL,
    total_charge_amount numeric,
    non_covered_charge_amt integer NOT NULL,
    "ABN_charge_amt" integer NOT NULL,
    est_contract_allowance_amt_ind integer NOT NULL,
    est_contract_allowance_amt integer NOT NULL,
    encntr_deductible_rem_amt_ind integer NOT NULL,
    deductible_applied_amt integer NOT NULL,
    encntr_copay_amt_ind integer NOT NULL,
    encntr_copay_amt integer NOT NULL,
    encntr_coinsurance_pct_ind integer NOT NULL,
    encntr_coinsurance_pct integer NOT NULL,
    encntr_coinsurance_amt integer NOT NULL,
    maximum_out_of_pocket_amt_ind integer NOT NULL,
    amt_over_max_out_of_pocket integer NOT NULL,
    est_patient_liab_amt integer NOT NULL,
    online_billpay_url character varying(1) NOT NULL,
    guarantor_access_code character varying(1) NOT NULL,
    batch_id character varying(50) NOT NULL
);


--
-- Name: patdx; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.patdx (
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    dx_number integer NOT NULL,
    diagnosis character varying(7) NOT NULL,
    version character varying(50) NOT NULL,
    code_qualifier character varying(5) DEFAULT NULL::character varying,
    is_error boolean DEFAULT true NOT NULL,
    import_file character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    posted_date timestamp without time zone,
    uid bigint NOT NULL
);


--
-- Name: perform_site; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.perform_site (
    site_code character varying(10) NOT NULL,
    site_name character varying(50) DEFAULT NULL::character varying,
    site_address character varying(30) DEFAULT NULL::character varying,
    site_city character varying(30) DEFAULT NULL::character varying,
    site_st character varying(2) DEFAULT NULL::character varying,
    site_zip character varying(10) DEFAULT NULL::character varying,
    site_phone character varying(20) DEFAULT NULL::character varying,
    site_director character varying(50) DEFAULT NULL::character varying,
    site_clia character varying(20) DEFAULT NULL::character varying,
    mod_date character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    referral_lab boolean
);


--
-- Name: phy; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.phy (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    upin character varying(6) DEFAULT NULL::character varying,
    ub92_upin character varying(6) DEFAULT NULL::character varying,
    tnh_num character varying(15) DEFAULT NULL::character varying,
    billing_npi character varying(15) DEFAULT NULL::character varying,
    pc_code character varying(2) DEFAULT NULL::character varying,
    cl_mnem character varying(15) DEFAULT NULL::character varying,
    last_name character varying(30) DEFAULT NULL::character varying,
    first_name character varying(30) DEFAULT NULL::character varying,
    mid_init character varying(30) DEFAULT NULL::character varying,
    group1 character varying(35) DEFAULT NULL::character varying,
    addr_1 character varying(40) DEFAULT NULL::character varying,
    addr_2 character varying(40) DEFAULT NULL::character varying,
    city character varying(30) DEFAULT NULL::character varying,
    state character varying(2) DEFAULT NULL::character varying,
    zip character varying(10) DEFAULT NULL::character varying,
    phone character varying(40) DEFAULT NULL::character varying,
    reserved character varying(1) DEFAULT NULL::character varying,
    num_labels integer DEFAULT 0,
    mod_date timestamp without time zone,
    mod_user character varying(40) DEFAULT NULL::character varying,
    mod_prg character varying(40) DEFAULT NULL::character varying,
    uri numeric(15,0) NOT NULL,
    mt_mnem character varying(15) DEFAULT NULL::character varying,
    credentials character varying(50) DEFAULT NULL::character varying,
    ov_code character varying(50) DEFAULT NULL::character varying,
    docnbr character varying(5) DEFAULT NULL::character varying
);


--
-- Name: phy_sanc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.phy_sanc (
    lastname character varying(20) DEFAULT NULL::character varying,
    firstname character varying(15) DEFAULT NULL::character varying,
    midname character varying(15) DEFAULT NULL::character varying,
    busname character varying(30) DEFAULT NULL::character varying,
    general character varying(20) DEFAULT NULL::character varying,
    specialty character varying(20) DEFAULT NULL::character varying,
    upin character varying(6) DEFAULT NULL::character varying,
    npi character varying(10) DEFAULT NULL::character varying,
    dob character varying(8) DEFAULT NULL::character varying,
    address character varying(30) DEFAULT NULL::character varying,
    city character varying(20) DEFAULT NULL::character varying,
    state character varying(2) DEFAULT NULL::character varying,
    zip character varying(5) DEFAULT NULL::character varying,
    sanctype character varying(9) DEFAULT NULL::character varying,
    sancdate character varying(8) DEFAULT NULL::character varying,
    reindate character varying(8) DEFAULT NULL::character varying,
    waiverdate character varying(8) DEFAULT NULL::character varying,
    wvrstate character varying(50) DEFAULT NULL::character varying,
    uri bigint NOT NULL
);


--
-- Name: phy_sanc_uri_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.phy_sanc_uri_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: phy_sanc_uri_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.phy_sanc_uri_seq OWNED BY dbo.phy_sanc.uri;


--
-- Name: pth; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.pth (
    deleted boolean DEFAULT true NOT NULL,
    pc_code bigint NOT NULL,
    name character varying(30) DEFAULT NULL::character varying,
    mc_pin character varying(7) DEFAULT NULL::character varying,
    bc_pin character varying(7) DEFAULT NULL::character varying,
    tlc_num character varying(7) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying
);


--
-- Name: pth_pc_code_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.pth_pc_code_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: pth_pc_code_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.pth_pc_code_seq OWNED BY dbo.pth.pc_code;


--
-- Name: pwise_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.pwise_cnb (
    "TheAcct" character varying(15) NOT NULL,
    "TheDate" character varying(20) NOT NULL,
    "Mod_date" character varying(20) NOT NULL
);


--
-- Name: rds; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.rds (
    uri numeric(15,0) NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    name character varying(40) NOT NULL,
    cli_mnem character varying(10) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying,
    shift character varying(10) DEFAULT NULL::character varying,
    test_date timestamp without time zone
);


--
-- Name: revcode; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.revcode (
    code character varying(4) NOT NULL,
    description character varying(25) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_prg character varying(20) DEFAULT NULL::character varying,
    mod_host character varying(20) DEFAULT NULL::character varying
);


--
-- Name: rpt_track; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.rpt_track (
    uri bigint NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(30) DEFAULT NULL::character varying,
    mod_host character varying(30) DEFAULT NULL::character varying,
    mod_app character varying(30) DEFAULT NULL::character varying,
    form_printed character varying(10) DEFAULT NULL::character varying,
    cli_nme character varying(40) DEFAULT NULL::character varying,
    qty_printed integer,
    printer_name character varying(80) DEFAULT NULL::character varying
);


--
-- Name: rpt_track_uri_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.rpt_track_uri_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: rpt_track_uri_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.rpt_track_uri_seq OWNED BY dbo.rpt_track.uri;


--
-- Name: ssi_remittance; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.ssi_remittance (
    file_date timestamp without time zone NOT NULL,
    prov_no character varying(15) DEFAULT NULL::character varying,
    last_name character varying(20) DEFAULT NULL::character varying,
    first_name character varying(10) DEFAULT NULL::character varying,
    mid_init character varying(1) DEFAULT NULL::character varying,
    icn character varying(15) NOT NULL,
    clm character varying(6) DEFAULT NULL::character varying,
    claim_status character varying(11) DEFAULT NULL::character varying,
    pcn character varying(17) DEFAULT NULL::character varying,
    hic character varying(30) DEFAULT NULL::character varying,
    tob character varying(3) DEFAULT NULL::character varying,
    svc_date_from timestamp without time zone,
    svc_date_thru timestamp without time zone,
    reported_charges numeric(19,4) DEFAULT NULL::numeric,
    non_cov_charges numeric(19,4) DEFAULT NULL::numeric,
    denied_charges numeric(19,4) DEFAULT NULL::numeric,
    net_payable_charges numeric(19,4) DEFAULT NULL::numeric,
    deductible numeric(19,4) DEFAULT NULL::numeric,
    co_insurance numeric(19,4) DEFAULT NULL::numeric,
    reim_rate numeric,
    contractual numeric(19,4) DEFAULT NULL::numeric,
    net_reimbursement numeric(19,4) DEFAULT NULL::numeric,
    fiscal_period_end timestamp without time zone,
    carrier character varying(20) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: ssi_remittance_charges; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.ssi_remittance_charges (
    file_date timestamp without time zone NOT NULL,
    icn character varying(15) NOT NULL,
    chrg_line integer NOT NULL,
    cpt_code character varying(5) DEFAULT NULL::character varying,
    rev_code character varying(3) DEFAULT NULL::character varying,
    reported_amt numeric(19,4) DEFAULT NULL::numeric,
    allowed_amt numeric(19,4) DEFAULT '0'::numeric
);


--
-- Name: statement; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.statement (
    statement_number character varying(50) DEFAULT NULL::character varying,
    name character varying(50) DEFAULT NULL::character varying
);


--
-- Name: system; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.system (
    key_name character varying(25) NOT NULL,
    value character varying(8000) DEFAULT NULL::character varying,
    programs character varying(50) DEFAULT NULL::character varying,
    restricted_users character varying(8000) DEFAULT NULL::character varying,
    comment character varying(1024) DEFAULT NULL::character varying,
    update_prg character varying(255) DEFAULT NULL::character varying,
    button character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    category character varying(50) DEFAULT NULL::character varying,
    description character varying(150) DEFAULT NULL::character varying,
    "dataType" character varying(25) DEFAULT NULL::character varying
);


--
-- Name: system_log; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.system_log (
    log_date timestamp without time zone,
    log_text character varying,
    account character varying(15) DEFAULT NULL::character varying,
    log_function character varying(50) DEFAULT NULL::character varying,
    log_program character varying(50) DEFAULT NULL::character varying,
    log_user character varying(50) DEFAULT NULL::character varying,
    log_workstation character varying(50) DEFAULT NULL::character varying,
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL
);


--
-- Name: tblPropAcc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."tblPropAcc" (
    "propPK" integer,
    "propClient" character varying(10) DEFAULT NULL::character varying,
    "propAccount" character varying(15) DEFAULT NULL::character varying,
    "propFinCode" character varying(10) DEFAULT NULL::character varying,
    "propInsCode" character varying(10) DEFAULT NULL::character varying,
    "propInsPolicy" character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: tblPropAccCrossover; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."tblPropAccCrossover" (
    "propPK" integer NOT NULL,
    "propFincode" character varying(10) DEFAULT NULL::character varying,
    "propEnctrType" character varying(20) DEFAULT NULL::character varying,
    "propType" character varying(10) DEFAULT NULL::character varying,
    "propAcc" character varying(15) NOT NULL,
    "propTDate" timestamp without time zone,
    "propPatName" character varying(125) DEFAULT NULL::character varying,
    "propHospAcc" character varying(50) DEFAULT NULL::character varying,
    "propHneNumber" character varying(50) DEFAULT NULL::character varying,
    "propAdmitDate" timestamp without time zone,
    "propDischargeDate" timestamp without time zone,
    mod_date timestamp without time zone
);


--
-- Name: tblTemp; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."tblTemp" (
    "Field1" character varying(5) DEFAULT NULL::character varying,
    "Field2" character varying(100) DEFAULT NULL::character varying
);


--
-- Name: tblTempOutPut; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."tblTempOutPut" (
    "Field1" character varying(5) DEFAULT NULL::character varying,
    "Field2" character varying(100) DEFAULT NULL::character varying
);


--
-- Name: tempChk; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."tempChk" (
    account character varying(15) DEFAULT NULL::character varying,
    source character varying(50) DEFAULT NULL::character varying,
    paid numeric,
    contractual numeric,
    write_off numeric,
    total numeric
);


--
-- Name: tempDaily; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."tempDaily" (
    account character varying(15) NOT NULL,
    "startBal" numeric,
    charges numeric,
    payments numeric,
    "endBal" numeric,
    error numeric
);


--
-- Name: tempReplace; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo."tempReplace" (
    link integer,
    cdm character varying(7) DEFAULT NULL::character varying,
    "DESCRIPT" character varying(50) DEFAULT NULL::character varying
);


--
-- Name: temp_ssi_del; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.temp_ssi_del (
    remit_date character varying(10) DEFAULT NULL::character varying,
    lname character varying(30) DEFAULT NULL::character varying,
    fname character varying(30) DEFAULT NULL::character varying,
    init character varying(5) DEFAULT NULL::character varying,
    covers_beg character varying(10) DEFAULT NULL::character varying,
    account character varying(15) DEFAULT NULL::character varying,
    balance character varying(9) DEFAULT NULL::character varying,
    abn character varying(1) DEFAULT NULL::character varying,
    co_ins character varying(9) DEFAULT NULL::character varying,
    payment character varying(9) DEFAULT NULL::character varying,
    contractual character varying(9) DEFAULT NULL::character varying,
    covered_chrgs character varying(9) DEFAULT NULL::character varying,
    non_cov character varying(9) DEFAULT NULL::character varying,
    third_party character varying(9) DEFAULT NULL::character varying,
    deductable character varying(9) DEFAULT NULL::character varying,
    type character varying(20) DEFAULT NULL::character varying,
    batch numeric,
    err_text character varying(100) DEFAULT NULL::character varying,
    uri numeric(10,0) NOT NULL
);


--
-- Name: temp_track; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.temp_track (
    comment character varying(8000) NOT NULL,
    row_count integer,
    error character varying(8000) DEFAULT NULL::character varying,
    start_time timestamp without time zone,
    mod_date timestamp without time zone
);


--
-- Name: test_acc; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.test_acc (
    account character varying(15) NOT NULL,
    code character varying(5) DEFAULT NULL::character varying
);


--
-- Name: test_charges; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.test_charges (
    charge_number bigint NOT NULL,
    account character varying(15) NOT NULL,
    cdm character varying(10) NOT NULL,
    qty integer NOT NULL
);


--
-- Name: test_charges_charge_number_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.test_charges_charge_number_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: test_charges_charge_number_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.test_charges_charge_number_seq OWNED BY dbo.test_charges.charge_number;


--
-- Name: test_charges_detail; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.test_charges_detail (
    charge_number integer NOT NULL,
    cpt4 character varying(5) NOT NULL,
    amount numeric
);


--
-- Name: test_chk; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.test_chk (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    pay_no numeric(15,0) NOT NULL,
    account character varying(15) NOT NULL,
    chk_date timestamp without time zone,
    date_rec timestamp without time zone,
    chk_no character varying(10) DEFAULT NULL::character varying,
    amt_paid numeric(19,4) DEFAULT '0'::numeric
);


--
-- Name: test_chrg; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.test_chrg (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    chrg_num numeric(15,0) NOT NULL,
    account character varying(15) DEFAULT NULL::character varying,
    service_date timestamp without time zone,
    cdm character varying(7) DEFAULT NULL::character varying,
    qty numeric,
    retail numeric(19,4) DEFAULT 0.00,
    net_amt numeric(19,4) DEFAULT 0.00
);


--
-- Name: test_collect_cnb; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.test_collect_cnb (
    test_mnem character varying(50) DEFAULT NULL::character varying,
    test_collect character varying(50) DEFAULT NULL::character varying,
    test_name character varying(50) DEFAULT NULL::character varying,
    test_cdm character varying(50) DEFAULT NULL::character varying,
    test_type character varying(50) DEFAULT NULL::character varying,
    test_printed boolean,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone
);


--
-- Name: test_payments; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.test_payments (
    account character varying(15) NOT NULL,
    amt_paid numeric NOT NULL,
    contractual numeric NOT NULL,
    write_off numeric NOT NULL,
    write_off_code character varying(10) DEFAULT NULL::character varying
);


--
-- Name: topdeletemctest; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.topdeletemctest (
    account character varying(15) NOT NULL,
    trans_date timestamp without time zone,
    paid timestamp without time zone,
    payment numeric,
    mod_date timestamp without time zone
);


--
-- Name: ub; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.ub (
    rowguid uuid DEFAULT public.uuid_generate_v4() NOT NULL,
    deleted boolean DEFAULT true NOT NULL,
    account character varying(15) NOT NULL,
    ins_abc character varying(1) NOT NULL,
    run_date timestamp without time zone,
    printed boolean DEFAULT true NOT NULL,
    run_user character varying(30) DEFAULT NULL::character varying,
    fin_code character varying(1) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    pat_name character varying(40) DEFAULT NULL::character varying,
    claimsnet_payer_id character varying(50) DEFAULT NULL::character varying,
    ebill_status character varying(5) DEFAULT NULL::character varying,
    batch numeric DEFAULT '-1'::numeric,
    text character varying(8000) DEFAULT NULL::character varying,
    edited_ub character varying(8000) DEFAULT NULL::character varying,
    cold_feed timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(50) NOT NULL,
    mod_prg character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL
);


--
-- Name: unapp_panels; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.unapp_panels (
    deleted boolean DEFAULT true NOT NULL,
    profile_cdm character varying(10) NOT NULL,
    comp_cdm character varying(10) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_prg character varying(20) DEFAULT NULL::character varying
);


--
-- Name: wl_cat; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.wl_cat (
    deleted boolean DEFAULT true NOT NULL,
    uri bigint NOT NULL,
    type_desc character varying(40) DEFAULT NULL::character varying,
    units character varying(20) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_prg character varying(20) DEFAULT NULL::character varying
);


--
-- Name: wl_cat_uri_seq; Type: SEQUENCE; Schema: dbo; Owner: -
--

CREATE SEQUENCE dbo.wl_cat_uri_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: wl_cat_uri_seq; Type: SEQUENCE OWNED BY; Schema: dbo; Owner: -
--

ALTER SEQUENCE dbo.wl_cat_uri_seq OWNED BY dbo.wl_cat.uri;


--
-- Name: zip; Type: TABLE; Schema: dbo; Owner: -
--

CREATE TABLE dbo.zip (
    zip character varying(10) NOT NULL,
    city character varying(30) DEFAULT NULL::character varying,
    st character varying(2) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_prg character varying(20) DEFAULT NULL::character varying
);


--
-- Name: client_cerner; Type: TABLE; Schema: dict; Owner: -
--

CREATE TABLE dict.client_cerner (
    "Client Number" character varying(255) DEFAULT NULL::character varying,
    "Organization Name" character varying(255) DEFAULT NULL::character varying,
    "Specific Location" character varying(255) DEFAULT NULL::character varying,
    "Address" character varying(255) DEFAULT NULL::character varying,
    "City" character varying(255) DEFAULT NULL::character varying,
    "State" character varying(255) DEFAULT NULL::character varying,
    "Zip code" character varying(255) DEFAULT NULL::character varying,
    "Business Phone #" character varying(255) DEFAULT NULL::character varying,
    "Fax #" character varying(255) DEFAULT NULL::character varying,
    "Contact Info/Instructions" character varying(255) DEFAULT NULL::character varying,
    "Outreach Callback" character varying(255) DEFAULT NULL::character varying,
    "Outreach Fax" character varying(255) DEFAULT NULL::character varying,
    "Outreach Callback After Hours" character varying(255) DEFAULT NULL::character varying,
    "Mnem" character varying(255) DEFAULT NULL::character varying,
    "Facility Number" character varying(255) DEFAULT NULL::character varying,
    "Location Alias" character varying(255) DEFAULT NULL::character varying
);


--
-- Name: menuitems; Type: TABLE; Schema: dict; Owner: -
--

CREATE TABLE dict.menuitems (
    item_id integer NOT NULL,
    description character varying(50) DEFAULT NULL::character varying,
    application character varying(100) NOT NULL,
    arguments character varying(100) DEFAULT NULL::character varying,
    apptype character varying(50) DEFAULT NULL::character varying,
    appcategory character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_user character varying(100) NOT NULL,
    mod_prg character varying(100) NOT NULL,
    mod_host character varying(100) NOT NULL
);


--
-- Name: menuprofile; Type: TABLE; Schema: dict; Owner: -
--

CREATE TABLE dict.menuprofile (
    profile_id integer NOT NULL,
    profile_name character varying(100) NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(100) NOT NULL,
    mod_prg character varying(100) NOT NULL,
    mod_host character varying(100) NOT NULL
);


--
-- Name: menuprofile_items; Type: TABLE; Schema: dict; Owner: -
--

CREATE TABLE dict.menuprofile_items (
    profile_id integer NOT NULL,
    item_id integer NOT NULL,
    mod_date timestamp without time zone,
    mod_user character varying(100) NOT NULL,
    mod_prg character varying(100) NOT NULL,
    mod_host character varying(100) NOT NULL
);


--
-- Name: client_utilization; Type: TABLE; Schema: dictionary; Owner: -
--

CREATE TABLE dictionary.client_utilization (
    "ReportDate" character varying(255) DEFAULT NULL::character varying,
    "FEE SCHEDULE" character varying(255) DEFAULT NULL::character varying,
    "MASTER CLIENT" character varying(255) DEFAULT NULL::character varying,
    "MASTER CLIENT NAME" character varying(255) DEFAULT NULL::character varying,
    "NTC" character varying(255) DEFAULT NULL::character varying,
    "Codes listed in MCL" character varying(255) DEFAULT NULL::character varying,
    "NTCX" character varying(255) DEFAULT NULL::character varying,
    "UNIT CODE" character varying(255) DEFAULT NULL::character varying,
    "TEST NAME" character varying(255) DEFAULT NULL::character varying,
    "PERF LAB" character varying(255) DEFAULT NULL::character varying,
    "TSO Flag" character varying(255) DEFAULT NULL::character varying,
    "CPT" character varying(255) DEFAULT NULL::character varying,
    "MCL MNEUMONIC" character varying(255) DEFAULT NULL::character varying,
    "CDM" character varying(7) DEFAULT NULL::character varying,
    "Patient Cost" numeric(19,4) DEFAULT NULL::numeric,
    "CURR PRICE" numeric(19,4) DEFAULT NULL::numeric,
    "Client Special Flag" character varying(255) DEFAULT NULL::character varying,
    "TOTAL REV" numeric(19,4) DEFAULT NULL::numeric,
    "2014 Medicare Reimburs#" bigint,
    "2015 Medicare Reim" double precision,
    "TOTAL VOL" double precision,
    "2014-01 TestRev" numeric(19,4) DEFAULT NULL::numeric,
    "2014-01 TestVol" double precision,
    "2014-02 TestRev" numeric(19,4) DEFAULT NULL::numeric,
    "2014-02 TestVol" double precision,
    "2014-03 TestRev" numeric(19,4) DEFAULT NULL::numeric,
    "2014-03 TestVol" double precision,
    "2014-04 TestRev" numeric(19,4) DEFAULT NULL::numeric,
    "2014-04 TestVol" double precision,
    "2014-05 TestRev" numeric(19,4) DEFAULT NULL::numeric,
    "2014-05 TestVol" double precision,
    "2014-06 TestRev" numeric(19,4) DEFAULT NULL::numeric,
    "2014-06 TestVol" double precision,
    "2015 Pricing" numeric(19,4) DEFAULT NULL::numeric,
    "T# Vol x       Current Price" numeric(19,4) DEFAULT NULL::numeric,
    "T# Vol x       2015 Pricing" numeric(19,4) DEFAULT NULL::numeric,
    "2015 Pricing minus  2015 MCRI" numeric(19,4) DEFAULT NULL::numeric,
    "F38" character varying(255) DEFAULT NULL::character varying
);


--
-- Name: clienttype; Type: TABLE; Schema: dictionary; Owner: -
--

CREATE TABLE dictionary.clienttype (
    type integer NOT NULL,
    description character varying(30) DEFAULT NULL::character varying
);


--
-- Name: facilities; Type: TABLE; Schema: dictionary; Owner: -
--

CREATE TABLE dictionary.facilities (
    "facilityNo" character varying(50) NOT NULL,
    "facilityName" character varying(100) DEFAULT NULL::character varying
);


--
-- Name: icd9gem; Type: TABLE; Schema: dictionary; Owner: -
--

CREATE TABLE dictionary.icd9gem (
    version character varying(2) NOT NULL,
    icd9 character varying(7) NOT NULL,
    icd10 character varying(9) DEFAULT NULL::character varying,
    flags character varying(7) DEFAULT NULL::character varying,
    uid numeric NOT NULL
);


--
-- Name: icd9version; Type: TABLE; Schema: dictionary; Owner: -
--

CREATE TABLE dictionary.icd9version (
    version character varying(10) NOT NULL,
    effective_date timestamp without time zone,
    effective_end_date timestamp without time zone,
    mod_date timestamp without time zone,
    mod_user character varying(100) DEFAULT NULL::character varying,
    mod_prg character varying(100) DEFAULT NULL::character varying,
    mod_host character varying(100) DEFAULT NULL::character varying
);


--
-- Name: lmrp; Type: TABLE; Schema: dictionary; Owner: -
--

CREATE TABLE dictionary.lmrp (
    cpt4 character varying(5) NOT NULL,
    beg_icd9 character varying(7) NOT NULL,
    end_icd9 character varying(7) NOT NULL,
    payor character varying(30) DEFAULT NULL::character varying,
    fincode character varying(10) DEFAULT NULL::character varying,
    mod_user character varying(20) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(20) DEFAULT NULL::character varying,
    rb_date timestamp without time zone,
    lmrp character varying(25) DEFAULT NULL::character varying,
    lmrp2 character varying(25) DEFAULT NULL::character varying,
    rb_date2 timestamp without time zone,
    chk_for_bad integer,
    ama_year character varying(6) DEFAULT NULL::character varying,
    uid numeric NOT NULL,
    expiration_date timestamp without time zone
);


--
-- Name: mapping; Type: TABLE; Schema: dictionary; Owner: -
--

CREATE TABLE dictionary.mapping (
    return_value character varying NOT NULL,
    return_value_type character varying(50) NOT NULL,
    sending_system character varying(50) DEFAULT NULL::character varying,
    sending_value character varying(50) NOT NULL,
    uid bigint NOT NULL,
    mod_date timestamp without time zone,
    mod_prg character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_host character varying(50) DEFAULT NULL::character varying
);


--
-- Name: messages_inbound; Type: TABLE; Schema: infce; Owner: -
--

CREATE TABLE infce.messages_inbound (
    account_cerner character varying(50) DEFAULT NULL::character varying,
    "sourceMsgId" numeric,
    "sourceInfce" character varying(50) DEFAULT NULL::character varying,
    "msgType" character varying(20) DEFAULT NULL::character varying,
    "msgDate" timestamp without time zone,
    "msgContent" character varying,
    "processFlag" character varying(5) DEFAULT NULL::character varying,
    "processStatusMsg" character varying(250) DEFAULT NULL::character varying,
    "systemMsgId" numeric NOT NULL,
    dx_processed boolean,
    mod_date timestamp without time zone,
    order_pat_id character varying(50) DEFAULT NULL::character varying,
    order_visit_id character varying(50) DEFAULT NULL::character varying,
    "DOS" timestamp without time zone,
    dx_processed_method character varying(50) DEFAULT NULL::character varying,
    ins_fin_code character varying(50) DEFAULT NULL::character varying,
    "HL7Message" character varying
);


--
-- Name: messages_inbound_adt; Type: TABLE; Schema: infce; Owner: -
--

CREATE TABLE infce.messages_inbound_adt (
    account_cerner character varying(50) DEFAULT NULL::character varying,
    "sourceMsgId" numeric,
    "sourceInfce" character varying(50) DEFAULT NULL::character varying,
    "msgType" character varying(20) DEFAULT NULL::character varying,
    "msgDate" timestamp without time zone,
    "msgContent" character varying,
    "processFlag" character varying(5) DEFAULT NULL::character varying,
    "processStatusMsg" character varying(250) DEFAULT NULL::character varying,
    "systemMsgId" numeric NOT NULL,
    dx_processed boolean,
    mod_date timestamp without time zone,
    order_pat_id character varying(50) DEFAULT NULL::character varying,
    order_visit_id character varying(50) DEFAULT NULL::character varying,
    "DOS" timestamp without time zone,
    dx_processed_method character varying(50) DEFAULT NULL::character varying
);


--
-- Name: messages_inbound_webconnect; Type: TABLE; Schema: infce; Owner: -
--

CREATE TABLE infce.messages_inbound_webconnect (
    account_cerner character varying(50) DEFAULT NULL::character varying,
    "sourceMsgId" numeric,
    "sourceInfce" character varying(50) DEFAULT NULL::character varying,
    "msgType" character varying(20) DEFAULT NULL::character varying,
    "msgDate" timestamp without time zone,
    "msgContent" character varying,
    "processFlag" character varying(5) DEFAULT NULL::character varying,
    "processStatusMsg" character varying(250) DEFAULT NULL::character varying,
    "systemMsgId" numeric NOT NULL,
    dx_processed boolean,
    mod_date timestamp without time zone,
    order_pat_id character varying(50) DEFAULT NULL::character varying,
    order_visit_id character varying(50) DEFAULT NULL::character varying,
    "DOS" timestamp without time zone
);


--
-- Name: messages_outbound; Type: TABLE; Schema: infce; Owner: -
--

CREATE TABLE infce.messages_outbound (
    account_cerner character varying(50) DEFAULT NULL::character varying,
    "sourceMsgId" numeric,
    "sourceInfce" character varying(50) DEFAULT NULL::character varying,
    "msgType" character varying(20) DEFAULT NULL::character varying,
    "msgDate" timestamp without time zone,
    "msgContent" character varying,
    "processFlag" character varying(5) DEFAULT NULL::character varying,
    "processStatusMsg" character varying(250) DEFAULT NULL::character varying,
    "systemMsgId" numeric NOT NULL,
    dx_processed boolean,
    mod_date timestamp without time zone,
    order_pat_id character varying(50) DEFAULT NULL::character varying,
    order_visit_id character varying(50) DEFAULT NULL::character varying,
    "DOS" timestamp without time zone,
    dx_processed_method character varying(50) DEFAULT NULL::character varying
);


--
-- Name: patient_demographics; Type: TABLE; Schema: infce; Owner: -
--

CREATE TABLE infce.patient_demographics (
    enctr_sys character varying(25) DEFAULT NULL::character varying,
    client character varying,
    account character varying(16) DEFAULT NULL::character varying,
    service_date character varying(8) DEFAULT NULL::character varying,
    acc_outreach character varying(50) DEFAULT NULL::character varying,
    acc_fin_nbr character varying(50) DEFAULT NULL::character varying,
    "systemMsgId" character varying(36) NOT NULL,
    hne character varying(25) DEFAULT NULL::character varying,
    mrn character varying(25) DEFAULT NULL::character varying,
    mrn_outreach character varying(50) DEFAULT NULL::character varying,
    "outreach_personID" character varying(50) DEFAULT NULL::character varying,
    "PATIENT" character varying(133) DEFAULT NULL::character varying,
    pat_dob character varying(8) DEFAULT NULL::character varying,
    visit_id character varying(40) DEFAULT NULL::character varying,
    "xmlContents" xml
);


--
-- Name: data_patBill; Type: TABLE; Schema: tst; Owner: -
--

CREATE TABLE tst."data_patBill" (
    account character varying(15) NOT NULL,
    pat_name character varying(40) DEFAULT NULL::character varying,
    trans_date timestamp without time zone,
    fin_code character varying(10) DEFAULT NULL::character varying,
    cl_mnem character varying(10) DEFAULT NULL::character varying,
    status character varying(8) DEFAULT NULL::character varying,
    dbill_date timestamp without time zone,
    ub_date timestamp without time zone,
    h1500_date timestamp without time zone,
    batch_date timestamp without time zone,
    ebill_batch_date timestamp without time zone,
    mailer character varying(1) DEFAULT NULL::character varying,
    h1500 character varying(1) DEFAULT NULL::character varying,
    ub92 character varying(1) DEFAULT NULL::character varying,
    phy_id character varying(15) DEFAULT NULL::character varying,
    ebill_batch_1500 timestamp without time zone,
    last_dm timestamp without time zone,
    bd_list_date timestamp without time zone,
    claimsnet_1500_batch_date timestamp without time zone
);


--
-- Name: dict_ViewerAccSql; Type: TABLE; Schema: zzz; Owner: -
--

CREATE TABLE zzz."dict_ViewerAccSql" (
    type_check character varying(50) DEFAULT NULL::character varying,
    fin_code character varying(10) DEFAULT NULL::character varying,
    ins_code character varying(50) DEFAULT NULL::character varying,
    bill_form character varying(50) DEFAULT NULL::character varying,
    valid boolean NOT NULL,
    "strSql" character varying(8000) NOT NULL,
    effective_date timestamp without time zone,
    expire_date timestamp without time zone,
    error character varying(256) DEFAULT NULL::character varying,
    mod_date timestamp without time zone,
    mod_prg character varying(50) NOT NULL,
    mod_user character varying(50) NOT NULL,
    mod_host character varying(50) NOT NULL,
    uid integer NOT NULL
);


--
-- Name: pat_bill_20150831; Type: TABLE; Schema: zzz; Owner: -
--

CREATE TABLE zzz.pat_bill_20150831 (
    stmt_number numeric,
    stmt_copy character varying(150) DEFAULT NULL::character varying
);


--
-- Name: phy_deleted; Type: TABLE; Schema: zzz; Owner: -
--

CREATE TABLE zzz.phy_deleted (
    rowguid uuid,
    deleted boolean,
    upin character varying(6) DEFAULT NULL::character varying,
    ub92_upin character varying(6) DEFAULT NULL::character varying,
    tnh_num character varying(15) DEFAULT NULL::character varying,
    billing_npi character varying(15) DEFAULT NULL::character varying,
    pc_code character varying(2) DEFAULT NULL::character varying,
    cl_mnem character varying(15) DEFAULT NULL::character varying,
    last_name character varying(30) DEFAULT NULL::character varying,
    first_name character varying(30) DEFAULT NULL::character varying,
    mid_init character varying(30) DEFAULT NULL::character varying,
    group1 character varying(35) DEFAULT NULL::character varying,
    addr_1 character varying(40) DEFAULT NULL::character varying,
    addr_2 character varying(40) DEFAULT NULL::character varying,
    city character varying(30) DEFAULT NULL::character varying,
    state character varying(2) DEFAULT NULL::character varying,
    zip character varying(10) DEFAULT NULL::character varying,
    phone character varying(40) DEFAULT NULL::character varying,
    reserved character varying(1) DEFAULT NULL::character varying,
    num_labels integer,
    mod_date timestamp without time zone,
    mod_user character varying(40) DEFAULT NULL::character varying,
    mod_prg character varying(40) DEFAULT NULL::character varying,
    uri numeric,
    mt_mnem character varying(15) DEFAULT NULL::character varying,
    credentials character varying(50) DEFAULT NULL::character varying,
    ov_code character varying(50) DEFAULT NULL::character varying
);


--
-- Name: req_clients_cnb; Type: TABLE; Schema: zzz; Owner: -
--

CREATE TABLE zzz.req_clients_cnb (
    cli_mnem character varying(50) DEFAULT NULL::character varying,
    test_mnem character varying(50) DEFAULT NULL::character varying,
    test_name character varying(50) DEFAULT NULL::character varying,
    mod_user character varying(50) DEFAULT NULL::character varying,
    mod_date timestamp without time zone
);


--
-- Name: ACC_LMRP uri; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."ACC_LMRP" ALTER COLUMN uri SET DEFAULT nextval('dbo."ACC_LMRP_uri_seq"'::regclass);


--
-- Name: AuditLog ID; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."AuditLog" ALTER COLUMN "ID" SET DEFAULT nextval('dbo."AuditLog_ID_seq"'::regclass);


--
-- Name: TransactionDetail TransactionDetailID; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."TransactionDetail" ALTER COLUMN "TransactionDetailID" SET DEFAULT nextval('dbo."TransactionDetail_TransactionDetailID_seq"'::regclass);


--
-- Name: UserProfile Id; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."UserProfile" ALTER COLUMN "Id" SET DEFAULT nextval('dbo."UserProfile_Id_seq"'::regclass);


--
-- Name: XmlSourceTable uid; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."XmlSourceTable" ALTER COLUMN uid SET DEFAULT nextval('dbo."XmlSourceTable_uid_seq"'::regclass);


--
-- Name: chk_batch BatchNo; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chk_batch ALTER COLUMN "BatchNo" SET DEFAULT nextval('dbo."chk_batch_BatchNo_seq"'::regclass);


--
-- Name: data_quest_360 uid; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.data_quest_360 ALTER COLUMN uid SET DEFAULT nextval('dbo.data_quest_360_uid_seq'::regclass);


--
-- Name: dict_Date ID; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."dict_Date" ALTER COLUMN "ID" SET DEFAULT nextval('dbo."dict_Date_ID_seq"'::regclass);


--
-- Name: dict_acc_validation rule_id; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_acc_validation ALTER COLUMN rule_id SET DEFAULT nextval('dbo.dict_acc_validation_rule_id_seq'::regclass);


--
-- Name: dict_acc_validation_criteria uid; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_acc_validation_criteria ALTER COLUMN uid SET DEFAULT nextval('dbo.dict_acc_validation_criteria_uid_seq'::regclass);


--
-- Name: dict_claim_validation_rule_criteria RuleCriteriaId; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_claim_validation_rule_criteria ALTER COLUMN "RuleCriteriaId" SET DEFAULT nextval('dbo."dict_claim_validation_rule_criteria_RuleCriteriaId_seq"'::regclass);


--
-- Name: dict_claim_validation_rules RuleId; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_claim_validation_rules ALTER COLUMN "RuleId" SET DEFAULT nextval('dbo."dict_claim_validation_rules_RuleId_seq"'::regclass);


--
-- Name: error_prg uid; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.error_prg ALTER COLUMN uid SET DEFAULT nextval('dbo.error_prg_uid_seq'::regclass);


--
-- Name: insc_payor uid; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.insc_payor ALTER COLUMN uid SET DEFAULT nextval('dbo.insc_payor_uid_seq'::regclass);


--
-- Name: job_track_wdk_del uid; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.job_track_wdk_del ALTER COLUMN uid SET DEFAULT nextval('dbo.job_track_wdk_del_uid_seq'::regclass);


--
-- Name: monthlycharges_del rowid; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.monthlycharges_del ALTER COLUMN rowid SET DEFAULT nextval('dbo.monthlycharges_del_rowid_seq'::regclass);


--
-- Name: phy_sanc uri; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.phy_sanc ALTER COLUMN uri SET DEFAULT nextval('dbo.phy_sanc_uri_seq'::regclass);


--
-- Name: pth pc_code; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.pth ALTER COLUMN pc_code SET DEFAULT nextval('dbo.pth_pc_code_seq'::regclass);


--
-- Name: rpt_track uri; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.rpt_track ALTER COLUMN uri SET DEFAULT nextval('dbo.rpt_track_uri_seq'::regclass);


--
-- Name: test_charges charge_number; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.test_charges ALTER COLUMN charge_number SET DEFAULT nextval('dbo.test_charges_charge_number_seq'::regclass);


--
-- Name: wl_cat uri; Type: DEFAULT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.wl_cat ALTER COLUMN uri SET DEFAULT nextval('dbo.wl_cat_uri_seq'::regclass);


--
-- Name: audit_acc idx_17665_PK_audit_acc; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_acc
    ADD CONSTRAINT "idx_17665_PK_audit_acc" PRIMARY KEY (uid);


--
-- Name: audit_aging_history idx_17683_PK_audit_aging_history; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_aging_history
    ADD CONSTRAINT "idx_17683_PK_audit_aging_history" PRIMARY KEY (uid);


--
-- Name: audit_amt idx_17689_PK_audit_amt; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_amt
    ADD CONSTRAINT "idx_17689_PK_audit_amt" PRIMARY KEY (uri);


--
-- Name: audit_bad_debt idx_17708_PK_audit_bad_debt; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_bad_debt
    ADD CONSTRAINT "idx_17708_PK_audit_bad_debt" PRIMARY KEY (uid);


--
-- Name: audit_cdm idx_17732_PK_audit_cdm; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_cdm
    ADD CONSTRAINT "idx_17732_PK_audit_cdm" PRIMARY KEY (uid);


--
-- Name: audit_chk idx_17751_PK_audit_chk_1; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_chk
    ADD CONSTRAINT "idx_17751_PK_audit_chk_1" PRIMARY KEY (uid);


--
-- Name: audit_chrg idx_17771_PK_audit_chrg_uid; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_chrg
    ADD CONSTRAINT "idx_17771_PK_audit_chrg_uid" PRIMARY KEY (uid);


--
-- Name: audit_cli_dis idx_17810_PK_audit_cli_dis; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_cli_dis
    ADD CONSTRAINT "idx_17810_PK_audit_cli_dis" PRIMARY KEY (uri);


--
-- Name: audit_cpt4 idx_17857_PK_audit_cpt4; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_cpt4
    ADD CONSTRAINT "idx_17857_PK_audit_cpt4" PRIMARY KEY (uid);


--
-- Name: audit_cpt4_2 idx_17874_PK_audit_cpt4_2; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_cpt4_2
    ADD CONSTRAINT "idx_17874_PK_audit_cpt4_2" PRIMARY KEY (uid);


--
-- Name: audit_cpt4_3 idx_17891_PK_audit_cpt4_3; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_cpt4_3
    ADD CONSTRAINT "idx_17891_PK_audit_cpt4_3" PRIMARY KEY (uid);


--
-- Name: audit_cpt4_4 idx_17908_PK_uid; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_cpt4_4
    ADD CONSTRAINT "idx_17908_PK_uid" PRIMARY KEY (uid);


--
-- Name: audit_cpt4_5 idx_17925_PK_audit_cpt4_5; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_cpt4_5
    ADD CONSTRAINT "idx_17925_PK_audit_cpt4_5" PRIMARY KEY (uid);


--
-- Name: audit_dbill idx_17942_PK_audit_dbill; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_dbill
    ADD CONSTRAINT "idx_17942_PK_audit_dbill" PRIMARY KEY (uid);


--
-- Name: audit_insc idx_17981_PK_audit_insc; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_insc
    ADD CONSTRAINT "idx_17981_PK_audit_insc" PRIMARY KEY (uid);


--
-- Name: audit_lmrp idx_18000_PK_audit_lmrp; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_lmrp
    ADD CONSTRAINT "idx_18000_PK_audit_lmrp" PRIMARY KEY (audit_uid);


--
-- Name: audit_pat idx_18015_PK_audit_pat; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_pat
    ADD CONSTRAINT "idx_18015_PK_audit_pat" PRIMARY KEY (uid);


--
-- Name: audit_patdx idx_18056_PK_audit_patdx; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_patdx
    ADD CONSTRAINT "idx_18056_PK_audit_patdx" PRIMARY KEY (uid);


--
-- Name: audit_phy idx_18059_PK_audit_phy; Type: CONSTRAINT; Schema: audit; Owner: -
--

ALTER TABLE ONLY audit.audit_phy
    ADD CONSTRAINT "idx_18059_PK_audit_phy" PRIMARY KEY (audit_uri);


--
-- Name: abn idx_18099_PK___1__20; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.abn
    ADD CONSTRAINT "idx_18099_PK___1__20" PRIMARY KEY (account, cdm);


--
-- Name: acc idx_18105_PK_acc_account; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.acc
    ADD CONSTRAINT "idx_18105_PK_acc_account" PRIMARY KEY (account);


--
-- Name: acc_alert idx_18128_PK_acc_alert; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.acc_alert
    ADD CONSTRAINT "idx_18128_PK_acc_alert" PRIMARY KEY (account);


--
-- Name: acc_dup_check idx_18134_PK_acc_dup_check; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.acc_dup_check
    ADD CONSTRAINT "idx_18134_PK_acc_dup_check" PRIMARY KEY (uid);


--
-- Name: ACC_LMRP idx_18150_PK_ACC_LMRP; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."ACC_LMRP"
    ADD CONSTRAINT "idx_18150_PK_ACC_LMRP" PRIMARY KEY (uri);


--
-- Name: acc_location idx_18156_PK_acc_location; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.acc_location
    ADD CONSTRAINT "idx_18156_PK_acc_location" PRIMARY KEY (account);


--
-- Name: acc_merges idx_18166_PK_acc_merges; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.acc_merges
    ADD CONSTRAINT "idx_18166_PK_acc_merges" PRIMARY KEY (account, dup_acc);


--
-- Name: acc_track idx_18188_PK_acc_track; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.acc_track
    ADD CONSTRAINT "idx_18188_PK_acc_track" PRIMARY KEY (account, track_code);


--
-- Name: acc_validation_status idx_18194_PK__acc_vali__EA162E10900A8494; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.acc_validation_status
    ADD CONSTRAINT "idx_18194_PK__acc_vali__EA162E10900A8494" PRIMARY KEY (account);


--
-- Name: aging_history idx_18202_PK_aging_history; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.aging_history
    ADD CONSTRAINT "idx_18202_PK_aging_history" PRIMARY KEY (account, datestamp);


--
-- Name: amt idx_18210_PK_amt_1__10; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.amt
    ADD CONSTRAINT "idx_18210_PK_amt_1__10" PRIMARY KEY (uri);


--
-- Name: AuditLog idx_18231_PK__AuditLog__3214EC27488093BF; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."AuditLog"
    ADD CONSTRAINT "idx_18231_PK__AuditLog__3214EC27488093BF" PRIMARY KEY ("ID");


--
-- Name: bad_debt idx_18241_PK_bad_debt_1__27; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.bad_debt
    ADD CONSTRAINT "idx_18241_PK_bad_debt_1__27" PRIMARY KEY (account_no);


--
-- Name: cbill_hist idx_18267_PK_cbill_hist_1__19; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cbill_hist
    ADD CONSTRAINT "idx_18267_PK_cbill_hist_1__19" PRIMARY KEY (cl_mnem, invoice);


--
-- Name: cdm idx_18281_PK_cdm_1; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cdm
    ADD CONSTRAINT "idx_18281_PK_cdm_1" PRIMARY KEY (cdm);


--
-- Name: cdm_map idx_18306_PK_cdm_map; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cdm_map
    ADD CONSTRAINT "idx_18306_PK_cdm_map" PRIMARY KEY (vendor, vendor_code);


--
-- Name: cdw idx_18313_PK_cdw_1__12; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cdw
    ADD CONSTRAINT "idx_18313_PK_cdw_1__12" PRIMARY KEY (cdm, hosp_mnem);


--
-- Name: chk idx_18327_PK_chk_pay_no; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chk
    ADD CONSTRAINT "idx_18327_PK_chk_pay_no" PRIMARY KEY (pay_no);


--
-- Name: chk_batch idx_18354_PK_chk_batch; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chk_batch
    ADD CONSTRAINT "idx_18354_PK_chk_batch" PRIMARY KEY ("BatchNo");


--
-- Name: chk_electronic idx_18363_PK_chk_electronic; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chk_electronic
    ADD CONSTRAINT "idx_18363_PK_chk_electronic" PRIMARY KEY (uid);


--
-- Name: chk_electronic_cpt_adjustment_codes idx_18368_PK_chk_electronic_cpt_adjustment_codes; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chk_electronic_cpt_adjustment_codes
    ADD CONSTRAINT "idx_18368_PK_chk_electronic_cpt_adjustment_codes" PRIMARY KEY (uid);


--
-- Name: chrg idx_18433_PK_chrg_1__10; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chrg
    ADD CONSTRAINT "idx_18433_PK_chrg_1__10" PRIMARY KEY (chrg_num);


--
-- Name: chrg_err idx_18496_PK_chrg_err_1__11; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chrg_err
    ADD CONSTRAINT "idx_18496_PK_chrg_err_1__11" PRIMARY KEY (uri);


--
-- Name: chrg_nhc idx_18516_PK_chrg_nhc; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chrg_nhc
    ADD CONSTRAINT "idx_18516_PK_chrg_nhc" PRIMARY KEY (chrg_num);


--
-- Name: chrg_pa idx_18555_PK_chrg_pa_1__13; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chrg_pa
    ADD CONSTRAINT "idx_18555_PK_chrg_pa_1__13" PRIMARY KEY (chrg_num, rowguid);


--
-- Name: chrg_pc idx_18608_PK_chrg_pc; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chrg_pc
    ADD CONSTRAINT "idx_18608_PK_chrg_pc" PRIMARY KEY (rowguid);


--
-- Name: chrg_rev_trk idx_18622_PK_chrg_rev_trk_1__16; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.chrg_rev_trk
    ADD CONSTRAINT "idx_18622_PK_chrg_rev_trk_1__16" PRIMARY KEY (chrg_num);


--
-- Name: cli_dis idx_18716_PK_cli_dis_new_1__23; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cli_dis
    ADD CONSTRAINT "idx_18716_PK_cli_dis_new_1__23" PRIMARY KEY (uri);


--
-- Name: client idx_18735_PK_client_1__13; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.client
    ADD CONSTRAINT "idx_18735_PK_client_1__13" PRIMARY KEY (cli_mnem);


--
-- Name: client_facility_no idx_18783_PK_client_facility_no; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.client_facility_no
    ADD CONSTRAINT "idx_18783_PK_client_facility_no" PRIMARY KEY (cl_mnem, facilityno);


--
-- Name: cpt4 idx_18826_PK_cpt4_1__23; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4
    ADD CONSTRAINT "idx_18826_PK_cpt4_1__23" PRIMARY KEY (cdm, link);


--
-- Name: cpt4_2 idx_18845_PK_cpt4_2_1__23; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_2
    ADD CONSTRAINT "idx_18845_PK_cpt4_2_1__23" PRIMARY KEY (cdm, link);


--
-- Name: cpt4_3 idx_18865_PK_cpt4_3_1__23; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_3
    ADD CONSTRAINT "idx_18865_PK_cpt4_3_1__23" PRIMARY KEY (cdm, link);


--
-- Name: cpt4_4 idx_18885_PK_cpt4_4_1__23; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_4
    ADD CONSTRAINT "idx_18885_PK_cpt4_4_1__23" PRIMARY KEY (cdm, link);


--
-- Name: cpt4_5 idx_18905_PK_cpt4_5_1_23; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_5
    ADD CONSTRAINT "idx_18905_PK_cpt4_5_1_23" PRIMARY KEY (cdm, link);


--
-- Name: cpt4_ama idx_18925_PK_cpt4_ama; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_ama
    ADD CONSTRAINT "idx_18925_PK_cpt4_ama" PRIMARY KEY (cpt4);


--
-- Name: cpt4_audit idx_18930_PK_cpt4_audit; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_audit
    ADD CONSTRAINT "idx_18930_PK_cpt4_audit" PRIMARY KEY (uri);


--
-- Name: cpt4_link_cnb idx_18993_PK_cpt4_link_cnb; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_link_cnb
    ADD CONSTRAINT "idx_18993_PK_cpt4_link_cnb" PRIMARY KEY ("CPT4", link_cdm);


--
-- Name: cpt4_wdk idx_19015_PK_cpt4_wdk_1__23; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.cpt4_wdk
    ADD CONSTRAINT "idx_19015_PK_cpt4_wdk_1__23" PRIMARY KEY (cdm, link);


--
-- Name: data_billing_batch idx_19034_PK_batch; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.data_billing_batch
    ADD CONSTRAINT "idx_19034_PK_batch" PRIMARY KEY (batch);


--
-- Name: data_billing_history idx_19040_PK_data_billing_history; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.data_billing_history
    ADD CONSTRAINT "idx_19040_PK_data_billing_history" PRIMARY KEY (account, run_date);


--
-- Name: data_EOB idx_19143_PK_data_EOB; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."data_EOB"
    ADD CONSTRAINT "idx_19143_PK_data_EOB" PRIMARY KEY (uid);


--
-- Name: data_EOB_Detail idx_19171_PK_data_EOB_Detail; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."data_EOB_Detail"
    ADD CONSTRAINT "idx_19171_PK_data_EOB_Detail" PRIMARY KEY (uid);


--
-- Name: data_ErrLog idx_19188_PK_data_ErrLog; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."data_ErrLog"
    ADD CONSTRAINT "idx_19188_PK_data_ErrLog" PRIMARY KEY (rowguid);


--
-- Name: data_HL7_Msg idx_19218_PK_data_HL7_Msg; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."data_HL7_Msg"
    ADD CONSTRAINT "idx_19218_PK_data_HL7_Msg" PRIMARY KEY (rowguid);


--
-- Name: data_quest_360 idx_19276_PK_data_quest_360; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.data_quest_360
    ADD CONSTRAINT "idx_19276_PK_data_quest_360" PRIMARY KEY (uid);


--
-- Name: data_quest_billing idx_19291_PK_data_quest_billing; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.data_quest_billing
    ADD CONSTRAINT "idx_19291_PK_data_quest_billing" PRIMARY KEY (uid);


--
-- Name: dbill idx_19370_PK_dbill; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dbill
    ADD CONSTRAINT "idx_19370_PK_dbill" PRIMARY KEY (account);


--
-- Name: dict_A_CPEDIT idx_19383_PK_dict_A_CPEDIT; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."dict_A_CPEDIT"
    ADD CONSTRAINT "idx_19383_PK_dict_A_CPEDIT" PRIMARY KEY ("ME_1", "ME_2");


--
-- Name: dict_acc_validation idx_19394_PK__dict_acc__E92A9296BE474D4E; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_acc_validation
    ADD CONSTRAINT "idx_19394_PK__dict_acc__E92A9296BE474D4E" PRIMARY KEY (rule_id);


--
-- Name: dict_acc_validation_criteria idx_19403_PK__dict_acc__DD7012649431FF3D; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_acc_validation_criteria
    ADD CONSTRAINT "idx_19403_PK__dict_acc__DD7012649431FF3D" PRIMARY KEY (uid);


--
-- Name: dict_C_MEEDIT idx_19436_PK_me1_me2; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."dict_C_MEEDIT"
    ADD CONSTRAINT "idx_19436_PK_me1_me2" PRIMARY KEY ("ME_1", "ME_2");


--
-- Name: dict_claim_validation_rule_criteria idx_19464_PK_dict_claim_validation_rule_criteria; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_claim_validation_rule_criteria
    ADD CONSTRAINT "idx_19464_PK_dict_claim_validation_rule_criteria" PRIMARY KEY ("RuleCriteriaId");


--
-- Name: dict_claim_validation_rules idx_19479_PK__dict_cla__110458E266915717; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_claim_validation_rules
    ADD CONSTRAINT "idx_19479_PK__dict_cla__110458E266915717" PRIMARY KEY ("RuleId");


--
-- Name: dict_cpt4_warnings idx_19514_PK_dict_diagnosis_warnings; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_cpt4_warnings
    ADD CONSTRAINT "idx_19514_PK_dict_diagnosis_warnings" PRIMARY KEY (rowguid);


--
-- Name: dict_Date idx_19524_PK_dict_Date; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."dict_Date"
    ADD CONSTRAINT "idx_19524_PK_dict_Date" PRIMARY KEY ("ID");


--
-- Name: dict_global_billing_cdms idx_19533_PK__dict_global_bill__6F405F80; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_global_billing_cdms
    ADD CONSTRAINT "idx_19533_PK__dict_global_bill__6F405F80" PRIMARY KEY (rowguid);


--
-- Name: dict_ncd idx_19572_PK_dict_ncd; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_ncd
    ADD CONSTRAINT "idx_19572_PK_dict_ncd" PRIMARY KEY (ncd_id, icd10);


--
-- Name: dict_quest_reference_lab_tests idx_19859_PK_dict_quest_reference_lab_tests; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_quest_reference_lab_tests
    ADD CONSTRAINT "idx_19859_PK_dict_quest_reference_lab_tests" PRIMARY KEY (uid);


--
-- Name: dict_write_off_codes idx_19885_PK_dict_write_off_codes; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.dict_write_off_codes
    ADD CONSTRAINT "idx_19885_PK_dict_write_off_codes" PRIMARY KEY (write_off_code);


--
-- Name: emp idx_19888_PK_emp_1__12; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.emp
    ADD CONSTRAINT "idx_19888_PK_emp_1__12" PRIMARY KEY (name);


--
-- Name: fin idx_19943_PK_fin_1__13; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.fin
    ADD CONSTRAINT "idx_19943_PK_fin_1__13" PRIMARY KEY (fin_code);


--
-- Name: h1500 idx_19964_PK_h1500; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.h1500
    ADD CONSTRAINT "idx_19964_PK_h1500" PRIMARY KEY (account, ins_abc);


--
-- Name: icd9desc idx_19985_PK_icd9desc; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.icd9desc
    ADD CONSTRAINT "idx_19985_PK_icd9desc" PRIMARY KEY (id);


--
-- Name: ins idx_19998_PK_account_abc; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.ins
    ADD CONSTRAINT "idx_19998_PK_account_abc" PRIMARY KEY (account, ins_a_b_c);


--
-- Name: insc idx_20066_PK_insc_1__12; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.insc
    ADD CONSTRAINT "idx_20066_PK_insc_1__12" PRIMARY KEY (code);


--
-- Name: Medicare_Comm idx_20139_PK_Medicare_Comm_L; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."Medicare_Comm"
    ADD CONSTRAINT "idx_20139_PK_Medicare_Comm_L" PRIMARY KEY (ins_name, fin_code);


--
-- Name: medicare_exclusions idx_20143_PK_medicare_exclusions; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.medicare_exclusions
    ADD CONSTRAINT "idx_20143_PK_medicare_exclusions" PRIMARY KEY (cpt_code, effective_date);


--
-- Name: menu idx_20149_PK_menu; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.menu
    ADD CONSTRAINT "idx_20149_PK_menu" PRIMARY KEY (menuid, itemno);


--
-- Name: month_end_del idx_20160_PK_month_end; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.month_end_del
    ADD CONSTRAINT "idx_20160_PK_month_end" PRIMARY KEY (account, datestamp);


--
-- Name: Monthly_Reports idx_20166_PK_Monthly_Reports; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."Monthly_Reports"
    ADD CONSTRAINT "idx_20166_PK_Monthly_Reports" PRIMARY KEY (mi_name);


--
-- Name: monthlycharges_del idx_20175_PK_MC_ROWID; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.monthlycharges_del
    ADD CONSTRAINT "idx_20175_PK_MC_ROWID" PRIMARY KEY (rowid);


--
-- Name: mutually_excl idx_20184_PK_cpt41_cpt42; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.mutually_excl
    ADD CONSTRAINT "idx_20184_PK_cpt41_cpt42" PRIMARY KEY (cpt4_1, cpt4_2);


--
-- Name: notes idx_20205_PK_rowguid; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.notes
    ADD CONSTRAINT "idx_20205_PK_rowguid" PRIMARY KEY (rowguid);


--
-- Name: number idx_20214_PK_number_1__16; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.number
    ADD CONSTRAINT "idx_20214_PK_number_1__16" PRIMARY KEY (keyfield);


--
-- Name: ov_chrg_cnb idx_20229_PK_ov_chrg_cnb; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.ov_chrg_cnb
    ADD CONSTRAINT "idx_20229_PK_ov_chrg_cnb" PRIMARY KEY (the_msg_no);


--
-- Name: pat idx_20233_PK_pat_3__10; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.pat
    ADD CONSTRAINT "idx_20233_PK_pat_3__10" PRIMARY KEY (account);


--
-- Name: patbill_enctr_actv idx_20435_PK_patbill_enctr_actv; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.patbill_enctr_actv
    ADD CONSTRAINT "idx_20435_PK_patbill_enctr_actv" PRIMARY KEY (statement_number, record_type, record_cnt, enctr_nbr, activity_id);


--
-- Name: patbill_stmt idx_20446_PK_patbill_stmt_1; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.patbill_stmt
    ADD CONSTRAINT "idx_20446_PK_patbill_stmt_1" PRIMARY KEY (record_type, record_cnt, statement_number);


--
-- Name: patdx idx_20476_PK_patdx; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.patdx
    ADD CONSTRAINT "idx_20476_PK_patdx" PRIMARY KEY (uid);


--
-- Name: perform_site idx_20483_PK_perform_site; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.perform_site
    ADD CONSTRAINT "idx_20483_PK_perform_site" PRIMARY KEY (site_code);


--
-- Name: phy idx_20498_PK_phy_5__12; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.phy
    ADD CONSTRAINT "idx_20498_PK_phy_5__12" PRIMARY KEY (uri);


--
-- Name: phy_sanc idx_20530_PK_phy_sanc; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.phy_sanc
    ADD CONSTRAINT "idx_20530_PK_phy_sanc" PRIMARY KEY (uri);


--
-- Name: pth idx_20553_PK_pth_1__12; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.pth
    ADD CONSTRAINT "idx_20553_PK_pth_1__12" PRIMARY KEY (pc_code);


--
-- Name: pwise_cnb idx_20564_PK_pwise_cnb; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.pwise_cnb
    ADD CONSTRAINT "idx_20564_PK_pwise_cnb" PRIMARY KEY ("TheAcct", "TheDate", "Mod_date");


--
-- Name: rds idx_20567_PK_rds_1__24; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.rds
    ADD CONSTRAINT "idx_20567_PK_rds_1__24" PRIMARY KEY (uri);


--
-- Name: ReChrg idx_20575_PK___1__13; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."ReChrg"
    ADD CONSTRAINT "idx_20575_PK___1__13" PRIMARY KEY (urn);


--
-- Name: revcode idx_20584_PK___2__11; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.revcode
    ADD CONSTRAINT "idx_20584_PK___2__11" PRIMARY KEY (code);


--
-- Name: rpt_track idx_20592_PK_rpt_track; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.rpt_track
    ADD CONSTRAINT "idx_20592_PK_rpt_track" PRIMARY KEY (uri);


--
-- Name: ssi_remittance idx_20602_PK_ssi_remittance; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.ssi_remittance
    ADD CONSTRAINT "idx_20602_PK_ssi_remittance" PRIMARY KEY (file_date, icn);


--
-- Name: ssi_remittance_charges idx_20628_PK_ssi_remittance_charges; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.ssi_remittance_charges
    ADD CONSTRAINT "idx_20628_PK_ssi_remittance_charges" PRIMARY KEY (file_date, icn, chrg_line);


--
-- Name: system idx_20640_PK___1__26; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.system
    ADD CONSTRAINT "idx_20640_PK___1__26" PRIMARY KEY (key_name);


--
-- Name: system_log idx_20654_PK_system_log; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.system_log
    ADD CONSTRAINT "idx_20654_PK_system_log" PRIMARY KEY (rowguid);


--
-- Name: tblPropAccCrossover idx_20673_PK_tblPropAccCrossover_1; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."tblPropAccCrossover"
    ADD CONSTRAINT "idx_20673_PK_tblPropAccCrossover_1" PRIMARY KEY ("propPK", "propAcc");


--
-- Name: temp_ssi_del idx_20715_PK___1__24; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.temp_ssi_del
    ADD CONSTRAINT "idx_20715_PK___1__24" PRIMARY KEY (uri);


--
-- Name: tempDaily idx_20750_PK__tempDail__EA162E10896CD402; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."tempDaily"
    ADD CONSTRAINT "idx_20750_PK__tempDail__EA162E10896CD402" PRIMARY KEY (account);


--
-- Name: test_acc idx_20760_PK_test_acc; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.test_acc
    ADD CONSTRAINT "idx_20760_PK_test_acc" PRIMARY KEY (account);


--
-- Name: test_charges idx_20765_PK_test_charges; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.test_charges
    ADD CONSTRAINT "idx_20765_PK_test_charges" PRIMARY KEY (charge_number);


--
-- Name: test_chk idx_20774_PK_chk_1__10_test; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.test_chk
    ADD CONSTRAINT "idx_20774_PK_chk_1__10_test" PRIMARY KEY (pay_no);


--
-- Name: test_chrg idx_20781_PK_chrg_1__10_test; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.test_chrg
    ADD CONSTRAINT "idx_20781_PK_chrg_1__10_test" PRIMARY KEY (chrg_num);


--
-- Name: TransactionDetail idx_20822_PK_TransactionDetail_TransactionDetailID; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."TransactionDetail"
    ADD CONSTRAINT "idx_20822_PK_TransactionDetail_TransactionDetailID" PRIMARY KEY ("TransactionDetailID");


--
-- Name: ub idx_20830_PK_ub; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.ub
    ADD CONSTRAINT "idx_20830_PK_ub" PRIMARY KEY (rowguid);


--
-- Name: unapp_panels idx_20846_PK_unapp_panels; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.unapp_panels
    ADD CONSTRAINT "idx_20846_PK_unapp_panels" PRIMARY KEY (profile_cdm, comp_cdm);


--
-- Name: UserProfile idx_20853_PK_UserProfile; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."UserProfile"
    ADD CONSTRAINT "idx_20853_PK_UserProfile" PRIMARY KEY ("Id");


--
-- Name: wl_cat idx_20864_PK_wl_cat; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.wl_cat
    ADD CONSTRAINT "idx_20864_PK_wl_cat" PRIMARY KEY (uri);


--
-- Name: XmlSourceTable idx_20874_PK__XmlSourceTable__74BA0D0B; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."XmlSourceTable"
    ADD CONSTRAINT "idx_20874_PK__XmlSourceTable__74BA0D0B" PRIMARY KEY (uid);


--
-- Name: zip idx_20881_PK___1__18; Type: CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo.zip
    ADD CONSTRAINT "idx_20881_PK___1__18" PRIMARY KEY (zip);


--
-- Name: menuitems idx_20909_PK__menuitem__52020FDD8AB7D191; Type: CONSTRAINT; Schema: dict; Owner: -
--

ALTER TABLE ONLY dict.menuitems
    ADD CONSTRAINT "idx_20909_PK__menuitem__52020FDD8AB7D191" PRIMARY KEY (item_id);


--
-- Name: menuprofile idx_20918_PK__menuprof__AEBB701F5FA8B0AA; Type: CONSTRAINT; Schema: dict; Owner: -
--

ALTER TABLE ONLY dict.menuprofile
    ADD CONSTRAINT "idx_20918_PK__menuprof__AEBB701F5FA8B0AA" PRIMARY KEY (profile_id);


--
-- Name: menuprofile_items idx_20921_PK_menuprofile_items; Type: CONSTRAINT; Schema: dict; Owner: -
--

ALTER TABLE ONLY dict.menuprofile_items
    ADD CONSTRAINT "idx_20921_PK_menuprofile_items" PRIMARY KEY (profile_id, item_id);


--
-- Name: clienttype idx_20958_PK_clienttype; Type: CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY dictionary.clienttype
    ADD CONSTRAINT "idx_20958_PK_clienttype" PRIMARY KEY (type);


--
-- Name: facilities idx_20962_PK_facilities; Type: CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY dictionary.facilities
    ADD CONSTRAINT "idx_20962_PK_facilities" PRIMARY KEY ("facilityNo");


--
-- Name: lmrp idx_20979_PK_lmrp; Type: CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY dictionary.lmrp
    ADD CONSTRAINT "idx_20979_PK_lmrp" PRIMARY KEY (uid);


--
-- Name: mapping idx_20991_PK_mapping; Type: CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY dictionary.mapping
    ADD CONSTRAINT "idx_20991_PK_mapping" PRIMARY KEY (uid);


--
-- Name: messages_inbound idx_21000_PK_messages_inbound; Type: CONSTRAINT; Schema: infce; Owner: -
--

ALTER TABLE ONLY infce.messages_inbound
    ADD CONSTRAINT "idx_21000_PK_messages_inbound" PRIMARY KEY ("systemMsgId");


--
-- Name: messages_inbound_adt idx_21014_PK_messages_inbound_adt; Type: CONSTRAINT; Schema: infce; Owner: -
--

ALTER TABLE ONLY infce.messages_inbound_adt
    ADD CONSTRAINT "idx_21014_PK_messages_inbound_adt" PRIMARY KEY ("systemMsgId");


--
-- Name: messages_inbound_webconnect idx_21027_PK_messages_inbound_webconnect; Type: CONSTRAINT; Schema: infce; Owner: -
--

ALTER TABLE ONLY infce.messages_inbound_webconnect
    ADD CONSTRAINT "idx_21027_PK_messages_inbound_webconnect" PRIMARY KEY ("systemMsgId");


--
-- Name: messages_outbound idx_21039_PK_messages_outbound; Type: CONSTRAINT; Schema: infce; Owner: -
--

ALTER TABLE ONLY infce.messages_outbound
    ADD CONSTRAINT "idx_21039_PK_messages_outbound" PRIMARY KEY ("systemMsgId");


--
-- Name: patient_demographics idx_21052_PK_patient_demographics; Type: CONSTRAINT; Schema: infce; Owner: -
--

ALTER TABLE ONLY infce.patient_demographics
    ADD CONSTRAINT "idx_21052_PK_patient_demographics" PRIMARY KEY ("systemMsgId");


--
-- Name: idx_17665_IX_account_mod_date; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17665_IX_account_mod_date" ON audit.audit_acc USING btree (account, mod_date);


--
-- Name: idx_17665_IX_user_mod_date; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17665_IX_user_mod_date" ON audit.audit_acc USING btree (mod_user, mod_date);


--
-- Name: idx_17665_missing_index_100_99_audit_acc; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX idx_17665_missing_index_100_99_audit_acc ON audit.audit_acc USING btree (acc_rowguid);


--
-- Name: idx_17689_audit_chrg_num; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX idx_17689_audit_chrg_num ON audit.audit_amt USING btree (audit_chrg_num, mod_date, mod_indicator);


--
-- Name: idx_17751_IX_audit_chk_account; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17751_IX_audit_chk_account" ON audit.audit_chk USING btree (account);


--
-- Name: idx_17751_IX_audit_chk_rowguid; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17751_IX_audit_chk_rowguid" ON audit.audit_chk USING btree (chk_rowguid);


--
-- Name: idx_17771_IX_audit_chrg; Type: INDEX; Schema: audit; Owner: -
--

CREATE UNIQUE INDEX "idx_17771_IX_audit_chrg" ON audit.audit_chrg USING btree (chrg_num, uid);


--
-- Name: idx_17771_idxAcc; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17771_idxAcc" ON audit.audit_chrg USING btree (account, chrg_num, mod_date);


--
-- Name: idx_17790_IX_audit_chrg_err; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17790_IX_audit_chrg_err" ON audit.audit_chrg_err USING btree (uri, mod_date);


--
-- Name: idx_17821_IX_audit_client; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17821_IX_audit_client" ON audit.audit_client USING btree (audit_cli_mnem, mod_date);


--
-- Name: idx_17951_IX_audit_h1500; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17951_IX_audit_h1500" ON audit.audit_h1500 USING btree (account, ins_abc);


--
-- Name: idx_17951_ix_account; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX idx_17951_ix_account ON audit.audit_h1500 USING btree (account);


--
-- Name: idx_17961_IX_account_mod_date; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17961_IX_account_mod_date" ON audit.audit_ins USING btree (account, mod_date);


--
-- Name: idx_17961_IX_audit_ins; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17961_IX_audit_ins" ON audit.audit_ins USING btree (account, ins_a_b_c, mod_date);


--
-- Name: idx_17961_IX_audit_ins_rowguid; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_17961_IX_audit_ins_rowguid" ON audit.audit_ins USING btree (ins_rowguid);


--
-- Name: idx_18015_IX_audit_pat; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_18015_IX_audit_pat" ON audit.audit_pat USING btree (pat_rowguid);


--
-- Name: idx_18015_IX_audit_pat_mod_date; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_18015_IX_audit_pat_mod_date" ON audit.audit_pat USING btree (mod_date, ub_date, h1500_date, mailer, first_dm, last_dm, mod_user);


--
-- Name: idx_18015_IX_audit_pat_user_date; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_18015_IX_audit_pat_user_date" ON audit.audit_pat USING btree (mod_user, mod_date);


--
-- Name: idx_18015_ix_account; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX idx_18015_ix_account ON audit.audit_pat USING btree (account, ub_date, h1500_date);


--
-- Name: idx_18015_ix_pat_rowguid; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX idx_18015_ix_pat_rowguid ON audit.audit_pat USING btree (pat_rowguid);


--
-- Name: idx_18087_IX_audit_ub; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX "idx_18087_IX_audit_ub" ON audit.audit_ub USING btree (account, ins_abc);


--
-- Name: idx_18087_ix_account; Type: INDEX; Schema: audit; Owner: -
--

CREATE INDEX idx_18087_ix_account ON audit.audit_ub USING btree (account);


--
-- Name: idx_18105_ix fin_code, trans_date, status INCLUDE deleted, acco; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18105_ix fin_code, trans_date, status INCLUDE deleted, acco" ON dbo.acc USING btree (fin_code, trans_date, status, deleted, account, pat_name, cl_mnem, cbill_date, ssn, num_comments, meditech_account, original_fincode, mod_date, mod_user, mod_prg, oereqno, mri);


--
-- Name: idx_18105_ix_cli_mnem; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_cli_mnem ON dbo.acc USING btree (cl_mnem, account, fin_code);


--
-- Name: idx_18105_ix_meditech_acct; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_meditech_acct ON dbo.acc USING btree (meditech_account);


--
-- Name: idx_18105_ix_mri; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_mri ON dbo.acc USING btree (mri);


--
-- Name: idx_18105_ix_pat_name; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_pat_name ON dbo.acc USING btree (pat_name);


--
-- Name: idx_18105_ix_ssn; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_ssn ON dbo.acc USING btree (ssn);


--
-- Name: idx_18105_ix_status_acct_fincode_name; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_status_acct_fincode_name ON dbo.acc USING btree (status, account, fin_code, pat_name, cl_mnem, trans_date);


--
-- Name: idx_18105_ix_status_fincode; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_status_fincode ON dbo.acc USING btree (status, fin_code);


--
-- Name: idx_18105_ix_status_transdate_fincode; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_status_transdate_fincode ON dbo.acc USING btree (status, trans_date, fin_code);


--
-- Name: idx_18105_ix_transdate_include_acct; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18105_ix_transdate_include_acct ON dbo.acc USING btree (trans_date, account);


--
-- Name: idx_18150_IX_acc_lmrp_account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18150_IX_acc_lmrp_account" ON dbo."ACC_LMRP" USING btree (account);


--
-- Name: idx_18156_IX_acc_location; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18156_IX_acc_location" ON dbo.acc_location USING btree (location, surveydate);


--
-- Name: idx_18156_IX_acc_location_ov_acct; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18156_IX_acc_location_ov_acct" ON dbo.acc_location USING btree (ov_acct, ov_mri);


--
-- Name: idx_18156_ix_location, surveydate INCLUDE account, pt_type, mod; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18156_ix_location, surveydate INCLUDE account, pt_type, mod" ON dbo.acc_location USING btree (location, surveydate, account, pt_type, mod_date, mod_user, mod_prg, mod_host, ov_acct, ov_mri);


--
-- Name: idx_18181_CDX_account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18181_CDX_account" ON dbo.acc_status_updates USING btree (account, mod_date);


--
-- Name: idx_18181_IX_emailed; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18181_IX_emailed" ON dbo.acc_status_updates USING btree (emailed, account, acc_status, trans_date, mod_date);


--
-- Name: idx_18202_ix_datestamp INCLUDE account, balance; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18202_ix_datestamp INCLUDE account, balance" ON dbo.aging_history USING btree (datestamp, fin_code, account, balance, ins_code, mod_date, mod_user, mailer);


--
-- Name: idx_18210_chrg_num_cdx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18210_chrg_num_cdx ON dbo.amt USING btree (chrg_num);


--
-- Name: idx_18210_ix_cpt4_include_chrg_num; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18210_ix_cpt4_include_chrg_num ON dbo.amt USING btree (cpt4, chrg_num);


--
-- Name: idx_18210_ix_mod_date, type INCLUDE chrg_num, amount; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18210_ix_mod_date, type INCLUDE chrg_num, amount" ON dbo.amt USING btree (mod_date, type, chrg_num, amount, cpt4);


--
-- Name: idx_18241_date_sent_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18241_date_sent_idx ON dbo.bad_debt USING btree (date_sent);


--
-- Name: idx_18267_IX_thru_date; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18267_IX_thru_date" ON dbo.cbill_hist USING btree (thru_date, cl_mnem, invoice, bal_forward, total_chrg, discount, balance_due, payments, true_balance_due, mod_user, mod_date, mod_prg, mod_host);


--
-- Name: idx_18267_invoice_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18267_invoice_idx ON dbo.cbill_hist USING btree (invoice);


--
-- Name: idx_18281_IX_cdm_descript; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18281_IX_cdm_descript" ON dbo.cdm USING btree (descript);


--
-- Name: idx_18281_IX_cdm_mnem; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18281_IX_cdm_mnem" ON dbo.cdm USING btree (mnem);


--
-- Name: idx_18302_IX_cdm_link_cnb; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18302_IX_cdm_link_cnb" ON dbo.cdm_link_cnb USING btree ("CDM", link_cnt);


--
-- Name: idx_18313_IX_cdw_hosp_mnem; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18313_IX_cdw_hosp_mnem" ON dbo.cdw USING btree (hosp_mnem, meditech_mnem);


--
-- Name: idx_18327_IX_account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18327_IX_account" ON dbo.chk USING btree (account);


--
-- Name: idx_18327_IX_chk_mod_date_status; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18327_IX_chk_mod_date_status" ON dbo.chk USING btree (mod_date, status, account, amt_paid, write_off, contractual);


--
-- Name: idx_18327_batch_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18327_batch_idx ON dbo.chk USING btree (batch);


--
-- Name: idx_18327_chk_no_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18327_chk_no_idx ON dbo.chk USING btree (chk_no);


--
-- Name: idx_18327_invoice_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18327_invoice_idx ON dbo.chk USING btree (invoice);


--
-- Name: idx_18327_ix_rowguid INCLUDE account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18327_ix_rowguid INCLUDE account" ON dbo.chk USING btree (rowguid, account);


--
-- Name: idx_18406_IX_chk_tests_cnt_cnb; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18406_IX_chk_tests_cnt_cnb" ON dbo.chk_tests_cnt_cnb USING btree (rundatetime);


--
-- Name: idx_18427_IX_chk_vp_cnb; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18427_IX_chk_vp_cnb" ON dbo.chk_vp_cnb USING btree (run_date);


--
-- Name: idx_18433_IX_chrg_mod_date; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18433_IX_chrg_mod_date" ON dbo.chrg USING btree (mod_date, bill_method);


--
-- Name: idx_18433_IX_postfile; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18433_IX_postfile" ON dbo.chrg USING btree (post_file, chrg_num, account, qty, retail, inp_price, mod_date, net_amt, fin_type);


--
-- Name: idx_18433_account_cdx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18433_account_cdx ON dbo.chrg USING btree (account);


--
-- Name: idx_18433_invoice_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18433_invoice_idx ON dbo.chrg USING btree (invoice);


--
-- Name: idx_18433_ix_credited,cdm,mtreqno; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18433_ix_credited,cdm,mtreqno" ON dbo.chrg USING btree (credited, cdm, mt_req_no);


--
-- Name: idx_18433_ix_rowguid; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18433_ix_rowguid ON dbo.chrg USING btree (rowguid);


--
-- Name: idx_18433_ix_service_date, cdm INCLUDE chrg_num, account, qty; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18433_ix_service_date, cdm INCLUDE chrg_num, account, qty" ON dbo.chrg USING btree (service_date, cdm, chrg_num, account, qty, fin_code);


--
-- Name: idx_18433_ix_status, cdm INCLUDE chrg_num, account, qty, retail; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18433_ix_status, cdm INCLUDE chrg_num, account, qty, retail" ON dbo.chrg USING btree (cdm, status, chrg_num, account, qty, retail, inp_price, mod_date, net_amt, fin_type);


--
-- Name: idx_18496_account_cdx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18496_account_cdx ON dbo.chrg_err USING btree (account);


--
-- Name: idx_18555_IX_chrg_pa_batch; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18555_IX_chrg_pa_batch" ON dbo.chrg_pa USING btree (batch);


--
-- Name: idx_18555_mt_req_no_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_18555_mt_req_no_idx ON dbo.chrg_pa USING btree (mt_req_no);


--
-- Name: idx_18716_IX_cli_dis_1; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18716_IX_cli_dis_1" ON dbo.cli_dis USING btree (cli_mnem, start_cdm, end_cdm);


--
-- Name: idx_18716_IX_cli_dis_deleted_cli; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18716_IX_cli_dis_deleted_cli" ON dbo.cli_dis USING btree (deleted, cli_mnem, start_cdm, end_cdm, percent_ds, price);


--
-- Name: idx_18735_IX_bill_to_client; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18735_IX_bill_to_client" ON dbo.client USING btree (bill_to_client, cli_mnem);


--
-- Name: idx_18735_IX_client_cli_nme; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18735_IX_client_cli_nme" ON dbo.client USING btree (cli_nme);


--
-- Name: idx_18735_ix deleted, cli_nme; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18735_ix deleted, cli_nme" ON dbo.client USING btree (deleted, cli_nme);


--
-- Name: idx_18735_ix_deleted, cli_mnem; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18735_ix_deleted, cli_mnem" ON dbo.client USING btree (deleted, cli_mnem);


--
-- Name: idx_18826_IX_cpt4_billcode; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18826_IX_cpt4_billcode" ON dbo.cpt4 USING btree (billcode);


--
-- Name: idx_18826_IX_cpt4_cpt4; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18826_IX_cpt4_cpt4" ON dbo.cpt4 USING btree (cpt4);


--
-- Name: idx_18826_ix_billcode INCLUDE cdm, link, cpt4; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_18826_ix_billcode INCLUDE cdm, link, cpt4" ON dbo.cpt4 USING btree (billcode, cdm, link, cpt4);


--
-- Name: idx_19130_IX_data_electronic_status; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19130_IX_data_electronic_status" ON dbo.data_electronic_status USING btree (account, mod_date);


--
-- Name: idx_19143_IX_EOB_rowguid; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19143_IX_EOB_rowguid" ON dbo."data_EOB" USING btree (rowguid);


--
-- Name: idx_19171_IX_EOB_detail_rowguid; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19171_IX_EOB_detail_rowguid" ON dbo."data_EOB_Detail" USING btree (rowguid);


--
-- Name: idx_19208_IX_data_h1500_to_SSI; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19208_IX_data_h1500_to_SSI" ON dbo."data_h1500_to_SSI" USING btree (account, ins_abc);


--
-- Name: idx_19253_IX_data_lab_deletes; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19253_IX_data_lab_deletes" ON dbo.data_lab_deletes USING btree ("cdm ", " CPT/HCPCS Code");


--
-- Name: idx_19260_IX_data_monitor_360; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19260_IX_data_monitor_360" ON dbo.data_monitor_360 USING btree (user360);


--
-- Name: idx_19276_IX_data_quest_360_account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19276_IX_data_quest_360_account" ON dbo.data_quest_360 USING btree (account, bill_type, patid);


--
-- Name: idx_19276_IX_data_quest_360_error; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19276_IX_data_quest_360_error" ON dbo.data_quest_360 USING btree (pre360_error, bill_code_error, entered, bill_type, date_of_service);


--
-- Name: idx_19291_IX_data_quest_billing_quest_code; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19291_IX_data_quest_billing_quest_code" ON dbo.data_quest_billing USING btree (account, req_no, quest_code);


--
-- Name: idx_19361_ndx_data_tier_pricing_primary; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19361_ndx_data_tier_pricing_primary ON dbo.data_tier_pricing USING btree ("CLIENT", "CDM");


--
-- Name: idx_19370_IX_dbill_fin_code_name; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19370_IX_dbill_fin_code_name" ON dbo.dbill USING btree (fin_code, pat_name);


--
-- Name: idx_19370_IX_dbill_pat_name; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19370_IX_dbill_pat_name" ON dbo.dbill USING btree (pat_name);


--
-- Name: idx_19370_batch_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19370_batch_idx ON dbo.dbill USING btree (batch, run_date);


--
-- Name: idx_19403_rule_id-20220627-161011; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19403_rule_id-20220627-161011" ON dbo.dict_acc_validation_criteria USING btree (rule_id);


--
-- Name: idx_19514_IX_dict_diagnosis_warnings; Type: INDEX; Schema: dbo; Owner: -
--

CREATE UNIQUE INDEX "idx_19514_IX_dict_diagnosis_warnings" ON dbo.dict_cpt4_warnings USING btree (cpt4);


--
-- Name: idx_19572_IX_ncd_cpt; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19572_IX_ncd_cpt" ON dbo.dict_ncd USING btree (ncd_id, cpt);


--
-- Name: idx_19859_IX_deleted_multiples_startdate_cdm; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19859_IX_deleted_multiples_startdate_cdm" ON dbo.dict_quest_reference_lab_tests USING btree (deleted, has_multiples, cdm, link, start_date, expire_date, quest_code, quest_description);


--
-- Name: idx_19859_IX_dict_quest_ref_lab_code; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19859_IX_dict_quest_ref_lab_code" ON dbo.dict_quest_reference_lab_tests USING btree (quest_code);


--
-- Name: idx_19964_PK_account, ins_abc; Type: INDEX; Schema: dbo; Owner: -
--

CREATE UNIQUE INDEX "idx_19964_PK_account, ins_abc" ON dbo.h1500 USING btree (account, ins_abc);


--
-- Name: idx_19964_ebill_batch; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19964_ebill_batch ON dbo.h1500 USING btree (ebill_batch);


--
-- Name: idx_19964_ebill_status_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19964_ebill_status_idx ON dbo.h1500 USING btree (ebill_status);


--
-- Name: idx_19964_fin_code_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19964_fin_code_idx ON dbo.h1500 USING btree (fin_code);


--
-- Name: idx_19964_ix_rowguid INCLUDE account, ins_abc; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19964_ix_rowguid INCLUDE account, ins_abc" ON dbo.h1500 USING btree (rowguid, account, ins_abc);


--
-- Name: idx_19964_pat_name_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19964_pat_name_idx ON dbo.h1500 USING btree (pat_name);


--
-- Name: idx_19964_run_date_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19964_run_date_idx ON dbo.h1500 USING btree (run_date);


--
-- Name: idx_19985_IX_icd9desc_icd9_desc; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19985_IX_icd9desc_icd9_desc" ON dbo.icd9desc USING btree (icd9_desc, "AMA_year");


--
-- Name: idx_19985_IX_icd9num_amayear; Type: INDEX; Schema: dbo; Owner: -
--

CREATE UNIQUE INDEX "idx_19985_IX_icd9num_amayear" ON dbo.icd9desc USING btree (icd9_num, "AMA_year", version, icd9_desc);


--
-- Name: idx_19998_IX_deleted; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19998_IX_deleted" ON dbo.ins USING btree (deleted, account);


--
-- Name: idx_19998_IX_ins_code; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19998_IX_ins_code" ON dbo.ins USING btree (ins_code, account, ins_a_b_c, policy_num);


--
-- Name: idx_19998_ix_abc Include account, ins_code; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19998_ix_abc Include account, ins_code" ON dbo.ins USING btree (ins_a_b_c, account, ins_code, policy_num);


--
-- Name: idx_19998_ix_rowguid INCLUDE account, ins_a_b_c; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_19998_ix_rowguid INCLUDE account, ins_a_b_c" ON dbo.ins USING btree (rowguid, account, ins_a_b_c);


--
-- Name: idx_19998_policy_num_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_19998_policy_num_idx ON dbo.ins USING btree (policy_num);


--
-- Name: idx_20066_name_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20066_name_idx ON dbo.insc USING btree (name);


--
-- Name: idx_20205_IX_account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20205_IX_account" ON dbo.notes USING btree (account);


--
-- Name: idx_20233_IX_pat_demographics; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20233_IX_pat_demographics" ON dbo.pat USING btree (ssn, dob_yyyy, sex, icd9_1, relation, account, mod_date);


--
-- Name: idx_20233_IX_pat_last_dm; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20233_IX_pat_last_dm" ON dbo.pat USING btree (last_dm);


--
-- Name: idx_20233_batch_date_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_batch_date_idx ON dbo.pat USING btree (batch_date);


--
-- Name: idx_20233_ebill_batch_1500; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_ebill_batch_1500 ON dbo.pat USING btree (ebill_batch_1500);


--
-- Name: idx_20233_ebill_batch_date_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_ebill_batch_date_idx ON dbo.pat USING btree (ebill_batch_date);


--
-- Name: idx_20233_guarantor_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_guarantor_idx ON dbo.pat USING btree (guarantor, account);


--
-- Name: idx_20233_ix_account_include_billtracking; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_ix_account_include_billtracking ON dbo.pat USING btree (account, mailer, phy_id, dbill_date, ub_date, h1500_date, batch_date, ebill_batch_date);


--
-- Name: idx_20233_ix_mailer INCLUDE account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20233_ix_mailer INCLUDE account" ON dbo.pat USING btree (mailer, account);


--
-- Name: idx_20233_ix_rowguid; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_ix_rowguid ON dbo.pat USING btree (rowguid);


--
-- Name: idx_20233_ix_ssi_batch; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_ix_ssi_batch ON dbo.pat USING btree (ssi_batch, account, ub_date, h1500_date);


--
-- Name: idx_20233_phy_id_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20233_phy_id_idx ON dbo.pat USING btree (phy_id, account);


--
-- Name: idx_20405_IX_STATEMENT; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20405_IX_STATEMENT" ON dbo.patbill_acc USING btree (batch_id, statement_number, record_cnt_acct);


--
-- Name: idx_20405_IX_batch_id_datesent; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20405_IX_batch_id_datesent" ON dbo.patbill_acc USING btree (batch_id, date_sent, account_id, statement_number);


--
-- Name: idx_20422_CDX_statement_number; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20422_CDX_statement_number" ON dbo.patbill_enctr USING btree (statement_number, enctr_nbr, pft_encntr_id, place_of_service);


--
-- Name: idx_20435_IX_statement_batch; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20435_IX_statement_batch" ON dbo.patbill_enctr_actv USING btree (statement_number, batch_id);


--
-- Name: idx_20446_IX_statement_number; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20446_IX_statement_number" ON dbo.patbill_stmt USING btree (statement_number);


--
-- Name: idx_20476_IX_patdx_account; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20476_IX_patdx_account" ON dbo.patdx USING btree (account);


--
-- Name: idx_20498_IX_phy_billing_npi; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20498_IX_phy_billing_npi" ON dbo.phy USING btree (billing_npi);


--
-- Name: idx_20498_IX_phy_deleted_tnh_num; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20498_IX_phy_deleted_tnh_num" ON dbo.phy USING btree (tnh_num, deleted, billing_npi, last_name, first_name, mid_init, mt_mnem);


--
-- Name: idx_20498_IX_phy_mt_mnem; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20498_IX_phy_mt_mnem" ON dbo.phy USING btree (mt_mnem);


--
-- Name: idx_20498_IX_phy_upin; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20498_IX_phy_upin" ON dbo.phy USING btree (upin);


--
-- Name: idx_20498_name_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20498_name_idx ON dbo.phy USING btree (last_name, first_name, mid_init);


--
-- Name: idx_20530_IX_phy_sanc_name; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20530_IX_phy_sanc_name" ON dbo.phy_sanc USING btree (lastname, firstname);


--
-- Name: idx_20530_IX_phy_sanc_upin; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20530_IX_phy_sanc_upin" ON dbo.phy_sanc USING btree (upin);


--
-- Name: idx_20553_name_idx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20553_name_idx ON dbo.pth USING btree (name);


--
-- Name: idx_20567_name_cdx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20567_name_cdx ON dbo.rds USING btree (name);


--
-- Name: idx_20602_ix_pcn; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20602_ix_pcn ON dbo.ssi_remittance USING btree (pcn);


--
-- Name: idx_20628_ix_icn INCLUDE cpt_code, rev_code, reported_amt, allo; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20628_ix_icn INCLUDE cpt_code, rev_code, reported_amt, allo" ON dbo.ssi_remittance_charges USING btree (icn, cpt_code, rev_code, reported_amt, allowed_amt);


--
-- Name: idx_20715_name_cdx; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX idx_20715_name_cdx ON dbo.temp_ssi_del USING btree (lname, fname, init);


--
-- Name: idx_20822_IXC_Transaction_AccountID_Date_TransactionDetailID; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20822_IXC_Transaction_AccountID_Date_TransactionDetailID" ON dbo."TransactionDetail" USING btree ("AccountID", "Date", "TransactionDetailID");


--
-- Name: idx_20822_IX_Transaction_NCID; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20822_IX_Transaction_NCID" ON dbo."TransactionDetail" USING btree ("NCID");


--
-- Name: idx_20830_PK_account, ins_abc; Type: INDEX; Schema: dbo; Owner: -
--

CREATE UNIQUE INDEX "idx_20830_PK_account, ins_abc" ON dbo.ub USING btree (account, ins_abc);


--
-- Name: idx_20874_IX_XmlSourceTable_processed; Type: INDEX; Schema: dbo; Owner: -
--

CREATE INDEX "idx_20874_IX_XmlSourceTable_processed" ON dbo."XmlSourceTable" USING btree (processed);


--
-- Name: idx_20979_IX_lmrp_all; Type: INDEX; Schema: dictionary; Owner: -
--

CREATE INDEX "idx_20979_IX_lmrp_all" ON dictionary.lmrp USING btree (ama_year, cpt4, beg_icd9, end_icd9);


--
-- Name: idx_20979_ix_cpt4, ama_year, beg_icd9, end_icd9, rb_date INCLUD; Type: INDEX; Schema: dictionary; Owner: -
--

CREATE INDEX "idx_20979_ix_cpt4, ama_year, beg_icd9, end_icd9, rb_date INCLUD" ON dictionary.lmrp USING btree (cpt4, ama_year, beg_icd9, end_icd9, rb_date, payor, fincode, mod_user, mod_date, mod_prg, lmrp, lmrp2, rb_date2, chk_for_bad, uid, expiration_date);


--
-- Name: idx_20991_IX_mapping_lookup; Type: INDEX; Schema: dictionary; Owner: -
--

CREATE INDEX "idx_20991_IX_mapping_lookup" ON dictionary.mapping USING btree (return_value_type, sending_system, sending_value);


--
-- Name: idx_21000_IX_PROC; Type: INDEX; Schema: infce; Owner: -
--

CREATE INDEX "idx_21000_IX_PROC" ON infce.messages_inbound USING btree ("msgType", "processFlag", "processStatusMsg", "systemMsgId");


--
-- Name: idx_21000_IX_PROC_DFT; Type: INDEX; Schema: infce; Owner: -
--

CREATE INDEX "idx_21000_IX_PROC_DFT" ON infce.messages_inbound USING btree ("msgType", "processFlag");


--
-- Name: idx_21000_IX_account_cerner; Type: INDEX; Schema: infce; Owner: -
--

CREATE INDEX "idx_21000_IX_account_cerner" ON infce.messages_inbound USING btree (account_cerner, "DOS");


--
-- Name: idx_21000_IX_msgtype_date; Type: INDEX; Schema: infce; Owner: -
--

CREATE INDEX "idx_21000_IX_msgtype_date" ON infce.messages_inbound USING btree ("msgType", "msgDate", "processFlag", account_cerner, order_pat_id, order_visit_id);


--
-- Name: idx_21000_IX_sourceMsgId; Type: INDEX; Schema: infce; Owner: -
--

CREATE INDEX "idx_21000_IX_sourceMsgId" ON infce.messages_inbound USING btree ("sourceMsgId", "sourceInfce", "msgType");


--
-- Name: idx_21014_IX_account_cerner; Type: INDEX; Schema: infce; Owner: -
--

CREATE INDEX "idx_21014_IX_account_cerner" ON infce.messages_inbound_adt USING btree (account_cerner, "msgType", "msgDate");


--
-- Name: data_EOB_Detail FK_data_EOB_Detail_data_EOB_Detail; Type: FK CONSTRAINT; Schema: dbo; Owner: -
--

ALTER TABLE ONLY dbo."data_EOB_Detail"
    ADD CONSTRAINT "FK_data_EOB_Detail_data_EOB_Detail" FOREIGN KEY (uid) REFERENCES dbo."data_EOB_Detail"(uid);


--
-- PostgreSQL database dump complete
--

\unrestrict lHVt40WNexIsxfKp0Htf0hAaRtaIueOksbgKbXR86If3VTuWW0KTJIKNlvKSxfm

