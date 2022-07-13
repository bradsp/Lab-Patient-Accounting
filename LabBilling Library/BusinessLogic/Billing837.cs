using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using RFClassLibrary;
using MCL;
using System.Collections;
using System.Data;
using EdiTools;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using System.Data.Common;
using System.Linq;

namespace LabBilling.Core
{
    public class Billing837
    {

        public Billing837(string connectionString)
        {

        }

        public EdiDocument Document;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims">List of claims to be compiled.</param>
        /// <param name="interchangeControlNumber"></param>
        /// <param name="environment"></param>
        /// <param name="batchSubmitterId"></param>
        /// <param name="segmentTerminator"></param>
        /// <param name="elementTerminator"></param>
        /// <param name="componentSeparator"></param>
        /// <param name="repetitionSeparator"></param>
        public string Generate837pClaimBatch(IEnumerable<ClaimData> claims, string interchangeControlNumber, string environment, string batchSubmitterId, 
            string file_location, char segmentTerminator = '~', char elementTerminator = '*', char componentSeparator = ':', char repetitionSeparator = '^')
        {
            var ediDocument = new EdiDocument();

            ediDocument.Options.SegmentTerminator = segmentTerminator;
            ediDocument.Options.ElementSeparator = elementTerminator;
            ediDocument.Options.ComponentSeparator = componentSeparator;
            ediDocument.Options.RepetitionSeparator = repetitionSeparator;


            ediDocument.Segments.Add(new EdiSegment("ISA")
            {
                [01] = "00", //authorization information qualifier
                [02] = "".PadRight(10), // authorization information
                [03] = "00", // security information qualifier
                [04] = "".PadRight(10), // security information
                [05] = "ZZ", // interchange id qualifier
                [06] = "SENDER".PadRight(15), //sender id
                [07] = "ZZ", //interchnage id qualifier
                [08] = "ZMIXED".PadRight(15), //receiver id
                [09] = EdiValue.Date(6, DateTime.Now), // interchange date
                [10] = EdiValue.Time(4, DateTime.Now), //interchange time
                [11] = ediDocument.Options.RepetitionSeparator.ToString(), // repetition separator
                [12] = "00501", // interchange control version number
                [13] = interchangeControlNumber, // 1.ToString("d9");
                [14] = "1", // acknowledgement requested
                [15] = environment, //  interchange usage indicator "P";
                [16] = ediDocument.Options.ComponentSeparator.ToString(), // component element separator
            });

            ediDocument.Segments.Add(new EdiSegment("GS")
            {
                [01] = "HC", //Functional identifier code
                [02] = batchSubmitterId.PadRight(10), // application sender code "SENDER";
                [03] = "ZMIXED", // application receiver code
                [04] = EdiValue.Date(8, DateTime.Now), // date
                [05] = EdiValue.Time(4, DateTime.Now), //time 
                [06] = interchangeControlNumber, // group control number 
                [07] = "X", //responsible agency code
                [08] = "005010X222" // version / release/ industry identifier code
            });
            //loop through accounts - generate loops for each claim
            int transactionSets = 0;
            foreach (ClaimData claim in claims)
            {
                int hlCount = 1;
                int segmentCount = 0;
                // ST - transaction set header
                ediDocument.Segments.Add(new EdiSegment("ST")
                {
                    [01] = "837",
                    [02] = claim.claimAccount.account, // transaction set control number - must match SE02
                    [03] = "005010X222", //implementation convention reference
                });
                segmentCount++;

                // BHT - beginning of hierarchical level
                ediDocument.Segments.Add(new EdiSegment("BHT")
                {
                    [01] = "0019", //hierarchical structure code
                    [02] = "00", //transaction set purpose 00 - original; 18 - reissue
                    [03] = claim.claimAccount.account, //reference identification
                    [04] = EdiValue.Date(8, DateTime.Today), // date claim file is create - should be today
                    [05] = EdiValue.Time(4, DateTime.Now), //time transaction set is created
                    [06] = claim.TransactionTypeCode //transaction type code - CH = chargable
                });
                segmentCount++;

                //Loop 1000A - Submitter Name
                // NM1 - Submitter Name
                ediDocument.Segments.Add(BuildNM1(EntityIdentifier.Submitter, claim));
                segmentCount++;

                // PER - submitter EDI Contact information
                ediDocument.Segments.Add(new EdiSegment("PER")
                {
                    [01] = "IC",
                    [02] = claim.SubmitterContactName, //free form name
                    [03] = "TE", //contact type (TE - telephone, EM - email, FX - fax)
                    [04] = claim.SubmitterContactPhone, //contact phone
                    [05] = "EM", //2nd contact type
                    [06] = claim.SubmitterContactEmail //contact email
                });
                segmentCount++;

                //Loop 1000B - Receiver Name
                // NM1 - Receiver Name
                ediDocument.Segments.Add(BuildNM1(EntityIdentifier.Receiver, claim));
                segmentCount++;

                // Loop 2000A - Billing Provider Hierarchical level
                // HL1 - Billing Provider Hierarchical Level
                ediDocument.Segments.Add(new EdiSegment("HL")
                {
                    [01] = hlCount++.ToString(),
                    [03] = "20",
                    [04] = "1"
                });
                segmentCount++;
                // PRV - Billing Provider Specialty Information
                ediDocument.Segments.Add(new EdiSegment("PRV")
                {
                    [01] = "BI",
                    [02] = "PXC",
                    [03] = claim.ProviderTaxonomyCode //provider taxonomy code
                });
                segmentCount++;
                // CUR - Foreign Currency Information

                // Loop 2010A 
                // Loop 2010AA - Billing Provider Name
                // --NM1 - Billing Provider Name
                ediDocument.Segments.Add(BuildNM1(EntityIdentifier.BillingProvider, claim));
                segmentCount++;
                // --N3 - Billing Provider Address
                if (!string.IsNullOrEmpty(claim.BillingProviderAddress))
                {
                    ediDocument.Segments.Add(new EdiSegment("N3")
                    {
                        [01] = claim.BillingProviderAddress
                    });
                    segmentCount++;
                }
                // --N4 - Billing Provider City St Zip
                ediDocument.Segments.Add(new EdiSegment("N4")
                {
                    [01] = claim.BillingProviderCity,
                    [02] = claim.BillingProviderState,
                    [03] = claim.BillingProviderZipCode,
                    [04] = claim.BillingProviderCountry
                });
                segmentCount++;
                // --REF - Billing Provider Tax Identification
                ediDocument.Segments.Add(new EdiSegment("REF")
                {
                    [01] = "EI",
                    [02] = claim.SubmitterId
                });
                segmentCount++;
                // --REF - Billing Provider UPIN/License Information
                /* -- not needed
                ediDocument.Segments.Add(new EdiSegment("REF")
                {
                    [01] = "1G",
                    [02] = claim.BillingProviderUPIN
                });
                segmentCount++;
                */
                // --PER - Billing Provider Contact Information
                ediDocument.Segments.Add(new EdiSegment("PER")
                {
                    [01] = "IC",
                    [02] = claim.BillingProviderContactName,
                    [03] = "TE",
                    [04] = claim.BillingProviderContactPhone,
                    [05] = "EM",
                    [06] = claim.BillingProviderContactEmail
                });
                segmentCount++;

                // Loop 2010AB - Pay to Address Name
                // --NM1 - Pay to Address Name
                ediDocument.Segments.Add(BuildNM1(EntityIdentifier.PayToProvider, claim));
                segmentCount++;
                // --N3 - Pay to Address ADDRESS
                if (!string.IsNullOrEmpty(claim.PayToAddress))
                {
                    ediDocument.Segments.Add(new EdiSegment("N3")
                    {
                        [01] = claim.PayToAddress
                    });
                    segmentCount++;
                }
                // --N4 - Pay to Address City State Zip
                ediDocument.Segments.Add(new EdiSegment("N4")
                {
                    [01] = claim.PayToCity,
                    [02] = claim.PayToState,
                    [03] = claim.PayToZipCode,
                    [04] = claim.PayToCountry
                });
                segmentCount++;

# region Loop 2010AC - Pay to Plan Name
                //Required when willing trading partners agree to use this implementation
                //for their subrogation payment requests.
                //1. This loop may only be used when BHT06 = 31.
                if (claim.PayToPlanName != null && claim.PayToPlanName != String.Empty && claim.TransactionTypeCode == "31")
                {
                    // - NM1 - Pay to Plan Name
                    ediDocument.Segments.Add(BuildNM1(EntityIdentifier.Payee, claim));
                    segmentCount++;
                    // - N3 - Pay to Plan address
                    if (!string.IsNullOrEmpty(claim.PayToPlanAddress))
                    {
                        ediDocument.Segments.Add(new EdiSegment("N3")
                        {
                            [01] = claim.PayToPlanAddress
                        });
                        segmentCount++;
                    }
                    // - N4 - Pay to Plan City State Zip
                    ediDocument.Segments.Add(new EdiSegment("N4")
                    {
                        [01] = claim.PayToPlanCity,
                        [02] = claim.PaytoPlanState,
                        [03] = claim.PaytoPlanZipCode,
                        [04] = claim.PayToPlanCountry
                    });
                    segmentCount++;
                    // - REF - Pay to Plan Secondary Information 
                    ediDocument.Segments.Add(new EdiSegment("REF")
                    {
                        [01] = "2U",
                        [02] = claim.PayToPlanSecondaryIdentifier
                    });
                    segmentCount++;
                    // - REF - Pay to Plan Tax Identification Number
                    ediDocument.Segments.Add(new EdiSegment("REF")
                    {
                        [01] = "EI",
                        [02] = claim.PayToPlanTaxId
                    });
                    segmentCount++;
                }
                #endregion Loop2010AC

                #region Loop 2000B Subscriber Hierarchical Level - this loop repeats for every payer
                foreach (ClaimSubscriber subscriber in claim.Subscribers)
                {
                    // --HL - Subscriber Hierarchical Level
                    ediDocument.Segments.Add(new EdiSegment("HL")
                    {
                        [01] = hlCount++.ToString(),
                        [02] = "1",
                        [03] = "22",
                        [04] = "1"
                    });
                    segmentCount++;
                    // --SBR - Subscriber Information
                    ediDocument.Segments.Add(new EdiSegment("SBR")
                    {
                        [01] = subscriber.PayerResponsibilitySequenceCode, //payer responsibility
                        [02] = subscriber.IndividualRelationshipCode, //individual relationship code
                        [03] = subscriber.ReferenceIdentification, //reference identification
                        [04] = subscriber.PlanName, //name
                        [05] = subscriber.InsuranceTypeCode,
                        [06] = subscriber.CoordinationOfBenefitsCode,
                        [07] = subscriber.ConditionResponseCode,
                        [08] = subscriber.EmployementStatusCode,
                        [09] = subscriber.ClaimFilingIndicatorCode
                    });
                    segmentCount++;

                    // - PAT - Patient Information - probably not needed in our case
                    //ediDocument.Segments.Add(new EdiSegment("PAT")
                    //{
                    //    [05] = "", //deceased date time period format qualifier
                    //    [06] = "", //deceased date
                    //    [07] = "", //unit or basis for measurement
                    //    [08] = "", //weight
                    //    [09] = "", //condition or response code
                    //});

                    // Loop 2010B
                    // Loop 2010BA - Subscriber Name
                    // --NM1 - Subscriber Name
                    ediDocument.Segments.Add(BuildNM1(EntityIdentifier.InsuredOrSubscriber, subscriber));
                    segmentCount++;
                    // --N3 - Subscriber Address
                    if (!string.IsNullOrEmpty(subscriber.Address) || !string.IsNullOrEmpty(subscriber.Address2))
                    {
                        ediDocument.Segments.Add(new EdiSegment("N3")
                        {
                            [01] = subscriber.Address,
                            [02] = subscriber.Address2
                        });
                        segmentCount++;
                    }
                    // --N4 - Subscriber City State Zip
                    ediDocument.Segments.Add(new EdiSegment("N4")
                    {
                        [01] = subscriber.City,
                        [02] = subscriber.State,
                        [03] = subscriber.ZipCode,
                        [04] = subscriber.Country
                    }); 
                    segmentCount++;
                    // --DMG - Subscriber Demographic Information
                    string sbsDob;
                    if (subscriber.DateOfBirth != null)
                        sbsDob = EdiValue.Date(8, (DateTime)subscriber.DateOfBirth);
                    else
                        sbsDob = "";
                    //date of birth must be 8 characters long
                    if (sbsDob.Length == 8)
                    {
                        ediDocument.Segments.Add(new EdiSegment("DMG")
                        {
                            [01] = "D8",
                            [02] = sbsDob,
                            [03] = subscriber.Gender
                        });
                        segmentCount++;
                    }
                    // --REF - Subscriber Secondary Identification - only include if SSN has a value
                    if (subscriber.SocSecNumber != null && subscriber.SocSecNumber.Length > 0)
                    {
                        ediDocument.Segments.Add(new EdiSegment("REF")
                        {
                            [01] = "SY",
                            [02] = subscriber.SocSecNumber
                        });
                        segmentCount++;
                    }
                    // --REF - Property and Casualty Claim Number
                    // --REF - Property and Casualty Subscriber Contact Information

                    // Loop 2010BB Payer Name
                    // -- NM1 - Payer Name
                    ediDocument.Segments.Add(BuildNM1(EntityIdentifier.Payer, subscriber));
                    segmentCount++;
                    // -- N3 - Payer Address
                    if (!string.IsNullOrEmpty(subscriber.PayerAddress) || !string.IsNullOrEmpty(subscriber.PayerAddress2))
                    {
                        ediDocument.Segments.Add(new EdiSegment("N3")
                        {
                            [01] = subscriber.PayerAddress,
                            [02] = subscriber.PayerAddress2
                        });
                        segmentCount++;
                    }
                    // -- N4 - Payer City State Zip
                    ediDocument.Segments.Add(new EdiSegment("N4")
                    {
                        [01] = subscriber.PayerCity,
                        [02] = subscriber.PayerState,
                        [03] = subscriber.PayerZipCode,
                        [04] = subscriber.PayerCountry
                    });
                    segmentCount++;
                    // -- REF - Payer Secondary Identification
                    if (!string.IsNullOrEmpty(subscriber.PayerIdentificationQualifier) 
                        && !string.IsNullOrEmpty(subscriber.BillingProviderSecondaryIdentifier))
                    {
                        ediDocument.Segments.Add(new EdiSegment("REF")
                        {
                            [01] = subscriber.PayerIdentificationQualifier,
                            [02] = subscriber.BillingProviderSecondaryIdentifier
                        });
                        segmentCount++;
                    }

                    // -- REF - Billing Provider Secondary Identification
                    /*
                    ediDocument.Segments.Add(new EdiSegment("REF")
                    {
                        [01] = "G2",
                        [02] = subscriber.BillingProviderSecondaryIdentifier
                    });
                    segmentCount++;
                    */
                }

                #endregion

                //Loop 2000C - Patient Hierarchical Level when the patient is not the subscriber
                // Note: Loops 2000c & 2010CA are not sent when the patient is the subscriber
                //Loop 2010CA - Patient Name

                // Loop 2300
                // --CLM - Claim Information
                var clm = new EdiSegment("CLM");
                clm.Element(1, new EdiElement(claim.ClaimIdentifier));
                clm.Element(2, new EdiElement(claim.TotalChargeAmount));
                var clm05 = new EdiElement();
                clm05[01] = claim.FacilityCode;
                clm05[02] = "B";
                clm05[03] = claim.ClaimFrequency;
                clm.Element(5, clm05);

                clm.Element(6, new EdiElement(claim.ProviderSignatureIndicator));
                clm.Element(7, new EdiElement(claim.ProviderAcceptAssignmentCode));
                clm.Element(8, new EdiElement(claim.BenefitAssignmentCertificationIndicator));
                clm.Element(9, new EdiElement(claim.ReleaseOfInformationCode));
                clm.Element(10, new EdiElement(claim.PatientSignatureSourceCode));
                
                var clm11 = new EdiElement();
                clm11[1] = claim.RelatedCausesCode1;
                clm11[2] = claim.RelatedCausesCode2;
                clm11[3] = claim.RelatedCausesCode3;
                clm11[4] = claim.RelatedCausesStateCode;
                clm11[5] = claim.RelatedCausesCountryCode;
                clm.Element(11, clm11);

                clm.Element(12, new EdiElement(claim.SpecialProgramIndicator));
                clm.Element(20, new EdiElement(claim.DelayReasonCode));

                ediDocument.Segments.Add(clm);
                segmentCount++;
                // --DTP - Date - Onset of Current Symptoms
                if (claim.OnsetOfCurrentIllness != null)
                {
                    ediDocument.Segments.Add(new EdiSegment("DTP")
                    {
                        [01] = "431",
                        [02] = "D8",
                        [03] = EdiValue.Date(8, claim.OnsetOfCurrentIllness ?? DateTime.MinValue)
                    });
                    segmentCount++;
                }
                // --DTP - Date - Initial Treatment Date
                if (claim.InitialTreatmentDate != null)
                {
                    ediDocument.Segments.Add(new EdiSegment("DTP")
                    {
                        [01] = "454",
                        [02] = "D8",
                        [03] = EdiValue.Date(8, claim.InitialTreatmentDate ?? DateTime.MinValue)
                    });
                    segmentCount++;
                }
                // --DTP - Date - Last Seen Date
                // --DTP - Date - Acute Manifestation
                // --DTP - Date - Accident
                if (claim.DateOfAccident != null)
                {
                    ediDocument.Segments.Add(new EdiSegment("DTP")
                    {
                        [01] = "439",
                        [02] = "D8",
                        [03] = EdiValue.Date(8, claim.DateOfAccident ?? DateTime.MinValue)
                    });
                    segmentCount++;
                }
                // --PWK - Claim supplemental information
                // --CN1 - Contact Information
                // --AMT - Patient Amount Paid
                if (claim.PatientAmountPaid > 0.0)
                {
                    ediDocument.Segments.Add(new EdiSegment("AMT")
                    {
                        [01] = "F5",
                        [02] = claim.PatientAmountPaid.ToString("c")
                    });
                    segmentCount++;
                }
                // --REF - several REF segments as needed
                ediDocument.Segments.Add(new EdiSegment("REF")
                {
                    [01] = "X4",
                    [02] = claim.CliaNumber
                });
                segmentCount++;
                // --K3 - file information
                // --NTE - claim note
                // --CR1 - Ambulance Transport information
                // --CR2 - Spinal manipulation service information
                // --CRC - several certification segments as needed
                // --HI - Healthcare diagnosis code
                var hi = new EdiSegment("HI");
                int dxcnt = 1;
                foreach(PatDiag diag in claim.claimAccount.Pat.Diagnoses)
                {
                    var hiElement = new EdiElement();
                    hiElement[1] = "ABK";
                    hiElement[2] = diag.Code;

                    hi.Element(dxcnt, hiElement);
                    dxcnt++;
                }
                ediDocument.Segments.Add(hi);
                segmentCount++;
                // --HI - Anesthesia related procedure
                // --HI - condition information
                // --HCP - healthcare repricing information

                // Loop 2310
                // Loop 2310A - Referring Provider Name
                // - NM1 - Referring provider name
                ediDocument.Segments.Add(BuildNM1(EntityIdentifier.ReferringProvider, claim));
                segmentCount++;
                // - REF - Referring provider secondary information

                // Loop 2310B - Rendering provider name
                // - NM1 - rendering provider name
                // - PRV - rendering provider specialty information
                // - REF - rendering provider secondary information

                // Loop 2310C - service facility location name
                // - NM1 - service facility location name

                // - N3 - service facility location address
                // - N4 - service facility location city, state, zip
                // - REF - service facility location secondary identification
                // - PER - service facility contact information

                // Loop 2310D - Supervising Provider Name

                //Loop 2320 - Other Subscriber information
                //SBR - Other Subscriber information
                //CAS - Claim Level adjustments
                //AMT - Coordination of Benefits (COB) Payer Paid amount
                //AMT - Coordination of Benefits (COB) Total Non-covered amount
                //AMT - Remaining Patient Liability
                //OI - Other Insurance Coverage Information
                //MOA - Outpatient Adjudication Information

                //Loop 2330A - Other Subscriber Name
                //NM1 - Other Subscriber Name
                //N3 - Other Subscriber Address
                //N4 - Other Subscriber City, State, Zip
                //REF - Other Subscriber Secondary Information

                //Loop 2330B - Other Payer Name
                //NM1 - Other Payer Name
                //N3 - Other Payer Address
                //N4 - Other Payer City, State, Zip
                //DTP - Claim Check or Remittance Date
                //REF - Other Payer Secondary Identifier
                //REF - Other Payer Prior Authorization Number
                //REF - Other Payer Referral Number
                //REF - Other Payer Claim Adjustment Indicator
                //REF - Other Pay Claim Control Number

                //Loop 2330C - Other Payer Referring Provider
                //NM1 - Other Payer Referring Provider
                //REF - Other Payer Referring Provider Secondary identification

                //Loop 2330D - Other Payer Rendering Provider
                //NM1 - Other payer rendering provider
                //REF - Other payer rendering provider secondary identification

                //Loop 2330E - Other payer service facility location
                //NM1 - Other payer service facility location
                //REF - Other payer service facility location secondary identification

                //Loop 2330F - Other payer supervising provider
                //NM1 - Other payer supervising provider
                //REF - Other payer supervising provider secondary identification

                //Loop 2330G - Other payer billing provider
                //NM1 - other payer billing provider
                //REF - other payer billing provider secondary identification

                // Loop 2400 - Service line number
                int lineCnt = 1;
                foreach (ClaimLine line in claim.ClaimLines)
                {
                    // - LX - service line number
                    ediDocument.Segments.Add(new EdiSegment("LX")
                    {
                        [01] = lineCnt++.ToString()
                    });
                    segmentCount++;
                    // - SV1 - professional service
                    var sv1 = new EdiSegment("SV1");
                    
                    var sv1_1 = new EdiElement();
                    sv1_1[1] = "HC";
                    sv1_1[2] = line.ProcedureCode;
                    sv1_1[3] = line.ProcedureModifier1;
                    sv1_1[4] = line.ProcedureModifier2;
                    sv1_1[5] = line.ProcedureModifier3;
                    sv1_1[7] = line.Description;
                    sv1.Element(1, sv1_1);

                    sv1[2] = line.Amount.ToString();
                    sv1[3] = "UN";
                    sv1[4] = line.Quantity.ToString();

                    var sv1_7 = new EdiElement();
                    sv1_7[1] = line.DxPtr1;
                    sv1_7[2] = line.DxPtr2;
                    sv1_7[3] = line.DxPtr3;
                    sv1_7[4] = line.DxPtr4;
                    sv1.Element(7, sv1_7);

                    sv1[11] = line.EPSDTIndicator;
                    sv1[12] = line.FamilyPlanningIndicator;

                    ediDocument.Segments.Add(sv1);

                    segmentCount++;
                    // - PWK - Line supplemental information
                    // - DTP - date - service line
                    ediDocument.Segments.Add(new EdiSegment("DTP")
                    {
                        [01] = "472",
                        [02] = "D8",
                        [03] = EdiValue.Date(8,line.ServiceDate ?? DateTime.MinValue)
                    });
                    segmentCount++;
                    // - DTP - date last seen date
                    // - DTP - test date
                    // - DTP - initial treatment date
                    // - MEA - test result
                    // - CN1 - contract information
                    // - REF - repriced line item reference number
                    // - REF - adjusted repriced line item reference number
                    // - REF - prior authorization
                    // - REF - line item control number
                    ediDocument.Segments.Add(new EdiSegment("REF")
                    {
                        [01] = "6R",
                        [02] = line.ControlNumber
                    });
                    segmentCount++;
                    // - REF - CLIA number -- needed if different from 2300 loop
                    // - REF - CLIA facility identification -- needed if different from 2300 loop
                    // - REF - Referral number
                    // - K3 - File information
                    // - NTE - line note
                    // - NTE - third party organization notes
                    // - PS1 - purchased service information
                    // - HCP - line pricing/repricing information

                    //Loop 2420A - Rendering provider name - required here if different then 2310B Rendering Provider
                    // NM1 - Rendering provider name
                    // PRV - Rendering provider specialty information
                    // REF - Rendering provider secondary identification

                    //Loop 2420B - Purchased service provider name
                    // NM1 - purchased service provider name
                    // REF - purchased service provider secondary identification

                    //Loop 2420C - service facility location name - required if different than 2010AA Billing provider or 2310C Service Facility Location
                    // NM1 - service facility location name
                    // N3 - service facility address
                    // N4 - service facility city state zip
                    // REF - service facility location secondary identification

                    //Loop 2420D - supervising provider name
                    // NM1 
                    // REF

                    //Loop 2420E - Ordering provider name
                    // NM1 - ordering provider name
                    // N3 - ordering provider address
                    // N4 - ordering provider city state zip
                    // REF - ordering provider secondary identification
                    // PER - Ordering provider contact information

                    //Loop 2420F - Referring provider name
                    // NM1
                    // REF

                    //Loop 2430 - Line Adjuciation information
                    // SVD - line adjudication information
                    // CAS - Line adjustment
                    // DTP - line check or remittance date
                    // AMT - remaining patient liability

                    //Loop 2440 - Form identification code
                    // LQ - form identification code
                    // FRM - Supporting documentation
                }

                // SE - Transaction Set Trailer
                segmentCount++; 
                ediDocument.Segments.Add(new EdiSegment("SE")
                {
                    [01] = segmentCount.ToString(),
                    [02] = claim.claimAccount.account
                });
                transactionSets++;
            }

            // Footer
            // GE
            ediDocument.Segments.Add(new EdiSegment("GE")
            {
                [01] = transactionSets.ToString(), //number of transaction sets included
                [02] = interchangeControlNumber, //group control number
            });
            // IEA
            ediDocument.Segments.Add(new EdiSegment("IEA")
            {
                [01] = "1", // number of included functional groups
                [02] = interchangeControlNumber
            });

            //ensure file location ends with \
            if (!file_location.EndsWith("\\"))
                file_location = file_location + "\\";
            ediDocument.Save($"{file_location}MCL-837p.txt");

            return ediDocument.ToString();
        }

        private EdiSegment BuildNM1(EntityIdentifier entityIdentifier, ClaimSubscriber subscriber)
        {
            EdiSegment edi = new EdiSegment("NM1");

            switch (entityIdentifier)
            {
                case EntityIdentifier.InsuredOrSubscriber:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.InsuredOrSubscriber];
                    edi[02] = "1";
                    edi[03] = subscriber.LastName; //last name
                    edi[04] = subscriber.FirstName; //first name
                    edi[05] = subscriber.MiddleName; //middle name
                    edi[06] = subscriber.NamePrefix; //name prefix
                    edi[07] = subscriber.NameSuffix; //name suffix
                    edi[08] = "MI"; //identification code qualifier
                    edi[09] = subscriber.PrimaryIdentifier; //identification code / subscriber primary identifier
                    break;
                case EntityIdentifier.Payer:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.Payer];
                    edi[02] = "2";
                    edi[03] = subscriber.PayerName;
                    edi[08] = "PI";
                    edi[09] = subscriber.PayerIdentifier;
                    break;
                default:
                    throw new InvalidParameterValueException(string.Format("EntityIdentifier {0} is not valid with ClaimSubscriber object.", entityIdentifier));
                    //break;
            }

            return edi;
        }

        private EdiSegment BuildNM1(EntityIdentifier entityIdentifier, ClaimData claim)
        {
            EdiSegment edi = new EdiSegment("NM1");
            
            switch(entityIdentifier)
            {
                case EntityIdentifier.Submitter:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.Submitter]; //41 = submitter
                    edi[02] = "2"; // 2 = non-person entity
                    edi[03] = claim.SubmitterName; //submitter last name / organzation name
                    edi[08] = "46"; //identification qualification code
                    edi[09] = claim.SubmitterId; //identification code (our tax id)
                    break;
                case EntityIdentifier.Receiver:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.Receiver];
                    edi[02] = "2";
                    edi[03] = claim.ReceiverOrgName; //organization name
                    edi[08] = "46"; //identification code qualifier - 46 = ETIN
                    edi[09] = claim.SubmitterId; //identification code (tax-id) 
                    break;
                case EntityIdentifier.BillingProvider:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.BillingProvider];
                    edi[02] = "2";
                    edi[03] = claim.BillingProviderContactName;
                    edi[08] = "XX";
                    edi[09] = claim.BillingProviderNPI; // is this the correct code?
                    break;
                case EntityIdentifier.PayToProvider:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.PayToProvider];
                    edi[02] = "2";
                    break;
                case EntityIdentifier.Payee:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.Payee];
                    edi[02] = "2";
                    edi[03] = claim.PayToPlanName;
                    edi[08] = "PI";
                    edi[09] = claim.PayToPlanPrimaryIdentifier;
                    break;
                case EntityIdentifier.Person:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.Person];
                    break;
                case EntityIdentifier.ReferringProvider:
                    edi[01] = EntityIdentifierCode[EntityIdentifier.ReferringProvider];
                    edi[02] = "1";
                    edi[03] = claim.ReferringProviderLastName;
                    edi[04] = claim.ReferringProviderFirstName;
                    edi[05] = claim.ReferringProviderMiddleName;
                    edi[06] = claim.ReferringProviderSuffix;
                    edi[08] = "XX";
                    edi[09] = claim.ReferringProviderNPI;
                    break;
                default:
                    throw new InvalidParameterValueException(string.Format("EntityIdentifier {0} is not valid with ClaimData object.", entityIdentifier));
                    //break;
            }

            return edi;
        }

        enum EntityIdentifier
        {
            Submitter,
            Receiver,
            BillingProvider,
            PayToProvider,
            Payee,
            InsuredOrSubscriber,
            Payer,
            Person,
            ReferringProvider
        }

        readonly Dictionary<EntityIdentifier, string> EntityIdentifierCode = new Dictionary<EntityIdentifier, string>()
        {
            { EntityIdentifier.Submitter, "41" },
            { EntityIdentifier.Receiver,"40" },
            { EntityIdentifier.BillingProvider,"85" },
            { EntityIdentifier.PayToProvider,"87" },
            { EntityIdentifier.Payee, "PE" },
            { EntityIdentifier.InsuredOrSubscriber, "IL" },
            { EntityIdentifier.Payer, "PR" },
            { EntityIdentifier.Person, "1" },
            { EntityIdentifier.ReferringProvider, "DN" }
        };

        enum DelayReason
        {
            ProofOfElibilityUnknownOrUnavailable,
            Litigation,
            AuthorizationDelays,
            DelayInCertifyingProvider,
            DelayInSupplyingBillingForms,
            DelayInDeliveryOfCustomMadeAppliances,
            ThirdPartyProcessingDelay,
            DelayInEligibilityDetermination,
            OriginalClaimRejectedOrDenied,
            AdministrationDelayInPriorApprovalProcess,
            Other,
            NaturalDisaster
        };

        readonly Dictionary<DelayReason, string> DelayReasonCode = new Dictionary<DelayReason, string>()
        {
            { DelayReason.ProofOfElibilityUnknownOrUnavailable, "1" },
            { DelayReason.Litigation, "2" },
            { DelayReason.AuthorizationDelays, "3" },
            { DelayReason.DelayInCertifyingProvider, "4" },
            { DelayReason.DelayInSupplyingBillingForms, "5" },
            { DelayReason.DelayInDeliveryOfCustomMadeAppliances, "6" },
            { DelayReason.ThirdPartyProcessingDelay, "7" },
            { DelayReason.DelayInEligibilityDetermination, "8" },
            { DelayReason.OriginalClaimRejectedOrDenied, "9" },
            { DelayReason.AdministrationDelayInPriorApprovalProcess, "10" },
            { DelayReason.Other, "11" },
            { DelayReason.NaturalDisaster, "12" }
        };


            


    }
}
