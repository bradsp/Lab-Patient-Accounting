using System;
using System.Collections.Generic;
using EdiTools;
using LabBilling.Core.Models;

namespace LabBilling.Core
{
    /// <summary>
    /// Class to generate 837 claim files from a populated ClaimData object
    /// </summary>
    public class Billing837
    {

        public char SegmentTerminator { get; set; } = '~';
        public char ElementTerminator { get; set; } = '*';
        public char ComponentSeparator { get; set; } = '>';
        public char RepetitionSeparator { get; set; } = '^';
        public string VersionReleaseSpecifierCodeInstitutional { get; set; } = "005010X223A2";
        public string VersionReleaseSpecifierCodeProfessional { get; set; } = "005010X222";
        public string VersionReleaseSpecifierCode
        {
            get
            {
                if (claimType == null)
                    throw new NullReferenceException("ClaimType has not been initialized.");
                if (claimType == ClaimType.Institutional)
                    return VersionReleaseSpecifierCodeInstitutional;
                if (claimType == ClaimType.Professional)
                    return VersionReleaseSpecifierCodeProfessional;
                return string.Empty;
            }
        }

        public string InterchangeControlVersionNo { get; set; } = "00501";
        public bool RequestInterchangeAcknowledgment { get; set; } = true;
        public string ReceiverId { get; set; } = "ZMIXED";
        public string InterchangeIdQualifier { get; set; } = "ZZ";
        public string SecurityInformationQualifier { get; set; } = "00";
        public string SecurityInformation { get; set; } = string.Empty;
        public string AuthorizationInformationQualifier { get; set; } = "00";
        public string AuthorizationInformation { get; set; } = string.Empty;
        public string ResponsibleAgencyCode { get; set; } = "X";
        public string FunctionalIdentifierCode { get; set; } = "HC";
        public string ProductionEnvironment { get; set; } = "T";

        public const string transactionSetIdentifierCode = "837";

        private ClaimType? claimType;

        private ClaimData claim; //used to track current claim being processed

        /// <summary>
        /// Initiate instance of Billing837 class.
        /// </summary>
        /// <param name="connectionString"></param>
        public Billing837(string connectionString, string productionEnvironment)
        {
            ProductionEnvironment = productionEnvironment;
        }

        public EdiDocument ediDocument;

        /// <summary>
        /// Generates an 837 claim for a single claim object. Used primarily for one-off printing of claims.
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        /// <exception cref="InvalidParameterValueException"></exception>
        public string GenerateSingleClaim(ClaimData claim, string fileLocation)
        {
            List<ClaimData> claims = new List<ClaimData>();
            claims.Add(claim);

            ClaimType claimType;

            switch (claim.ClaimType)
            {
                case Core.ClaimType.Institutional:
                    claimType = ClaimType.Institutional;
                    break;
                case Core.ClaimType.Professional:
                    claimType = ClaimType.Professional;
                    break;
                default:
                    throw new InvalidParameterValueException("ClaimType does not contain a valid value.");
            }

            return Generate837ClaimBatch(claims, claim.InterchangeControlNumber, claim.BatchSubmitterId, fileLocation, claimType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="interchangeControlNumber"></param>
        /// <param name="batchSubmitterId"></param>
        /// <param name="file_location"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        public string Generate837ClaimBatch(IEnumerable<ClaimData> claims,
            string interchangeControlNumber,
            string batchSubmitterId,
            string file_location,
            ClaimType claimType)
        {
            //EdiDocument ediDocument;

            this.claimType = claimType;

            ediDocument = new EdiDocument();

            ediDocument.Options.SegmentTerminator = this.SegmentTerminator;
            ediDocument.Options.ElementSeparator = this.ElementTerminator;
            ediDocument.Options.ComponentSeparator = this.ComponentSeparator;
            ediDocument.Options.RepetitionSeparator = this.RepetitionSeparator;

            ediDocument.Segments.Add(new EdiSegment("ISA")
            {
                [01] = this.AuthorizationInformationQualifier, //authorization information qualifier
                [02] = this.AuthorizationInformation.PadRight(10), // authorization information
                [03] = this.SecurityInformationQualifier, // security information qualifier
                [04] = this.SecurityInformation.PadRight(10), // security information
                [05] = this.InterchangeIdQualifier, // interchange id qualifier
                [06] = batchSubmitterId.PadRight(15), //sender id
                [07] = this.InterchangeIdQualifier, //interchnage id qualifier
                [08] = this.ReceiverId.PadRight(15), //receiver id
                [09] = EdiValue.Date(6, DateTime.Now), // interchange date
                [10] = EdiValue.Time(4, DateTime.Now), //interchange time
                [11] = ediDocument.Options.RepetitionSeparator.ToString(), // repetition separator
                [12] = this.InterchangeControlVersionNo, // interchange control version number
                [13] = interchangeControlNumber, // 1.ToString("d9");
                [14] = this.RequestInterchangeAcknowledgment ? "1" : "0", // acknowledgement requested
                [15] = ProductionEnvironment, //  interchange usage indicator "P" for production "T" for test
                [16] = ediDocument.Options.ComponentSeparator.ToString(), // component element separator
            });

            ediDocument.Segments.Add(new EdiSegment("GS")
            {
                [01] = this.FunctionalIdentifierCode, //Functional identifier code
                [02] = batchSubmitterId.PadRight(10), // application sender code "SENDER";
                [03] = this.ReceiverId, // application receiver code
                [04] = EdiValue.Date(8, DateTime.Now), // date
                [05] = EdiValue.Time(4, DateTime.Now), //time 
                [06] = interchangeControlNumber, // group control number 
                [07] = this.ResponsibleAgencyCode, //responsible agency code
                [08] = this.VersionReleaseSpecifierCode // version / release/ industry identifier code
            });
            //loop through accounts - generate loops for each claim
            int transactionSets = 0;
            foreach (ClaimData claim in claims)
            {
                this.claim = claim;
                Build837Claim();
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
                file_location += "\\";
            string fileName = $"MCL837-{interchangeControlNumber}.txt";
            if (claimType == ClaimType.Institutional)
                fileName = $"MCL837i-{interchangeControlNumber}.txt";
            if (claimType == ClaimType.Professional)
                fileName = $"MCL837p-{interchangeControlNumber}.txt";

            ediDocument.Save($"{file_location}{fileName}");
            return ediDocument.ToString();
        }


        private string Build837Claim()
        {

            switch (claim.ClaimType)
            {
                case Core.ClaimType.Institutional:
                    this.claimType = ClaimType.Institutional;
                    break;
                case Core.ClaimType.Professional:
                    this.claimType = ClaimType.Professional;
                    break;
                default:
                    throw new InvalidParameterValueException("ClaimType does not contain a valid value.");
            }

            int hlCount = 1;
            int segmentCount = 0;
            // ST - transaction set header
            ediDocument.Segments.Add(new EdiSegment("ST")
            {
                [01] = transactionSetIdentifierCode,
                [02] = claim.claimAccount.AccountNo, // transaction set control number - must match SE02
                [03] = this.VersionReleaseSpecifierCode //implementation convention reference
            });
            segmentCount++;

            // BHT - beginning of hierarchical level
            ediDocument.Segments.Add(new EdiSegment("BHT")
            {
                [01] = "0019", //hierarchical structure code
                [02] = claim.TransactionSetPurpose, //transaction set purpose 00 - original; 18 - reissue
                [03] = claim.claimAccount.AccountNo, //reference identification
                [04] = EdiValue.Date(8, DateTime.Today), // date claim file is create - should be today
                [05] = EdiValue.Time(4, DateTime.Now), //time transaction set is created
                [06] = claim.TransactionTypeCode //transaction type code - CH = chargable
            });
            segmentCount++;

            segmentCount += Loop1000A();

            segmentCount += Loop1000B();

            segmentCount += Loop2000A(ref hlCount);

            segmentCount += Loop2000B(ref hlCount);

            segmentCount += Loop2300();

            segmentCount += Loop2400();

            // Note: Loops 2000C & 2010CA are not sent when the patient is the subscriber
            segmentCount += Loop2000C(ref hlCount);

            // SE - Transaction Set Trailer
            segmentCount++;

            ediDocument.Segments.Add(new EdiSegment("SE")
            {
                [01] = segmentCount.ToString(),
                [02] = claim.claimAccount.AccountNo
            });

            return ediDocument.ToString();
        }

        private int Loop1000A()
        {
            int segmentCount = 0;

            // NM1 - Submitter Name
            ediDocument.Segments.Add(new EdiSegment("NM1")
            {
                [01] = "41", //41 = submitter
                [02] = "2", // 2 = non-person entity
                [03] = claim.SubmitterName, //submitter last name / organzation name
                [08] = "46", //identification qualification code
                [09] = claim.SubmitterId //identification code (our tax id)
            });
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

            return segmentCount;
        }

        private int Loop1000B()
        {
            int segmentCount = 0;
            ediDocument.Segments.Add(new EdiSegment("NM1")
            {
                [01] = "40",
                [02] = "2",
                [03] = claim.ReceiverOrgName, //organization name
                [08] = "46", //identification code qualifier - 46 = ETIN
                [09] = claim.ReceiverId //identification code (tax-id) 
            });
            segmentCount++;

            return segmentCount;
        }

        private int Loop2000A(ref int hlCount)
        {
            int segmentCount = 0;
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

            segmentCount += Loop2010AA();
            segmentCount += Loop2010AB();
            segmentCount += Loop2010AC();

            return segmentCount;
        }

        private int Loop2010AA()
        {
            int segmentCount = 0;

            // --NM1 - Billing Provider Name
            ediDocument.Segments.Add(new EdiSegment("NM1")
            {
                [01] = "85",
                [02] = "2",
                [03] = claim.BillingProviderName,
                [08] = "XX",
                [09] = claim.BillingProviderNPI, // is this the correct code?
            });
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
            return segmentCount;
        }

        private int Loop2010AB()
        {
            int segmentCount = 0;
            // --NM1 - Pay to Address Name
            ediDocument.Segments.Add(new EdiSegment("NM1")
            {
                [01] = "87",
                [02] = "2"
            });
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
            return segmentCount;
        }

        private int Loop2010AC()
        {
            //Required when willing trading partners agree to use this implementation
            //for their subrogation payment requests.
            //1. This loop may only be used when BHT06 = 31.

            int segmentCount = 0;

            if (claim.PayToPlanName != null && claim.PayToPlanName != String.Empty && claim.TransactionTypeCode == "31")
            {
                // - NM1 - Pay to Plan Name
                ediDocument.Segments.Add(new EdiSegment("NM1")
                {
                    [01] = "PE",
                    [02] = "2",
                    [03] = claim.PayToPlanName,
                    [08] = "PI",
                    [09] = claim.PayToPlanPrimaryIdentifier
                });
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

            return segmentCount;
        }

        private int Loop2000B(ref int hlCount)
        {
            int segmentCount = 0;

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

                //PAT segment goes here -- needed if patient is known to be deceased

                segmentCount += Loop2010BA(subscriber);
                segmentCount += Loop2010BB(subscriber);

            }

            return segmentCount;

        }

        private int Loop2010BA(ClaimSubscriber subscriber)
        {
            int segmentCount = 0;

            // --NM1 - Subscriber Name
            ediDocument.Segments.Add(new EdiSegment("NM1")
            {
                [01] = "IL",
                [02] = "1",
                [03] = subscriber.LastName, //last name
                [04] = subscriber.FirstName, //first name
                [05] = subscriber.MiddleName, //middle name
                [06] = subscriber.NamePrefix, //name prefix
                [07] = subscriber.NameSuffix, //name suffix
                [08] = "MI", //identification code qualifier
                [09] = subscriber.PrimaryIdentifier //identification code / subscriber primary identifier

            });

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
            // --PER - Property and Casualty Subscriber Contact Information

            return segmentCount;
        }

        private int Loop2010BB(ClaimSubscriber subscriber)
        {
            int segmentCount = 0;
            // -- NM1 - Payer Name
            ediDocument.Segments.Add(new EdiSegment("NM1")
            {
                [01] = "PR",
                [02] = "2",
                [03] = subscriber.PayerName,
                [08] = "PI",
                [09] = subscriber.PayerIdentifier

            });
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
            if (!string.IsNullOrEmpty(subscriber.PayerCity))
            {
                //payer city is a required field if populating N4 segment
                ediDocument.Segments.Add(new EdiSegment("N4")
                {
                    [01] = subscriber.PayerCity,
                    [02] = subscriber.PayerState,
                    [03] = subscriber.PayerZipCode,
                    [04] = subscriber.PayerCountry
                });
                segmentCount++;
            }
            // -- REF - Payer Secondary Identification
            //not needed now that we are using NPI
            //if (!string.IsNullOrEmpty(subscriber.PayerIdentificationQualifier)
            //    && !string.IsNullOrEmpty(subscriber.BillingProviderSecondaryIdentifier))
            //{
            //    string[] invalidQualifiers = new string[3] { "1B", "1D", "CI" };
            //    //1B is not a valid qualifier for professional claims
            //    if (!invalidQualifiers.Contains(subscriber.PayerIdentificationQualifier)
            //        && claimType == ClaimType.Professional)
            //    {
            //        ediDocument.Segments.Add(new EdiSegment("REF")
            //        {
            //            [01] = subscriber.PayerIdentificationQualifier,
            //            [02] = subscriber.BillingProviderSecondaryIdentifier
            //        });
            //        segmentCount++;
            //    }
            //}

            // -- REF - Billing Provider Secondary Identification
            /*
            ediDocument.Segments.Add(new EdiSegment("REF")
            {
                [01] = "G2",
                [02] = subscriber.BillingProviderSecondaryIdentifier
            });
            segmentCount++;
            */
            return segmentCount;
        }

        /// <summary>
        /// Loop 2000C is only sent when the patient is not the subscriber
        /// </summary>
        /// <returns></returns>
        private int Loop2000C(ref int hlCount)
        {
            int segmentCount = 0;
            if (claim.claimAccount.InsurancePrimary.Relation != "01")
            {
                //HL segment
                ediDocument.Segments.Add(new EdiSegment("HL")
                {
                    [01] = hlCount++.ToString(),
                    [02] = (hlCount - 2).ToString(),
                    [03] = "23",
                    [04] = "0"
                });
                segmentCount++;
                // - PAT - Patient Information
                string indRelationCode;
                switch (claim.claimAccount.InsurancePrimary.Relation)
                {
                    case "01":
                        indRelationCode = "18";
                        break;
                    case "02":
                        indRelationCode = "01";
                        break;
                    case "03":
                        indRelationCode = "19";
                        break;
                    case "04":
                        indRelationCode = "G8";
                        break;
                    case "09":
                        indRelationCode = "21";
                        break;
                    default:
                        indRelationCode = "18";
                        break;
                }

                ediDocument.Segments.Add(new EdiSegment("PAT")
                {
                    [01] = indRelationCode
                });
                segmentCount++;

                //Begin Loop 2010CA
                ediDocument.Segments.Add(new EdiSegment("NM1")
                {
                    [01] = "QC",
                    [02] = "1",
                    [03] = claim.claimAccount.PatLastName,
                    [04] = claim.claimAccount.PatFirstName,
                    [05] = claim.claimAccount.PatMiddleName,
                    [07] = claim.claimAccount.PatNameSuffix

                });
                segmentCount++;

                ediDocument.Segments.Add(new EdiSegment("N3")
                {
                    [01] = claim.claimAccount.Pat.Address1,
                    [02] = claim.claimAccount.Pat.Address2
                });
                segmentCount++;

                ediDocument.Segments.Add(new EdiSegment("N4")
                {
                    [01] = claim.claimAccount.Pat.City,
                    [02] = claim.claimAccount.Pat.State,
                    [03] = claim.claimAccount.Pat.ZipCode
                });
                segmentCount++;
                string patDob;
                if (claim.claimAccount.BirthDate != null)
                    patDob = EdiValue.Date(8, (DateTime)claim.claimAccount.BirthDate);
                else
                    patDob = "";
                ediDocument.Segments.Add(new EdiSegment("DMG")
                {
                    [01] = "D8",
                    [02] = patDob,
                    [03] = claim.claimAccount.Sex
                });
                segmentCount++;

                //REF - Property & casualty claim number
                //PER = Property & casualty claim contact info
            }
            return segmentCount;
        }

        private int Loop2300()
        {
            int segmentCount = 0;

            // --CLM - Claim Information
            var clm = new EdiSegment("CLM");
            clm.Element(1, new EdiElement(claim.ClaimIdentifier));
            clm.Element(2, new EdiElement(claim.TotalChargeAmount.ToString("F2")));
            var clm05 = new EdiElement();
            clm05[01] = claim.FacilityCode;
            clm05[02] = claim.FacilityCodeQualifier;
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
            
            if (claimType == ClaimType.Professional)
            {
                // --DTP - Date - Onset of Current Symptoms--professional claim

                //not needed in lab billing scenario
                //if (claim.OnsetOfCurrentIllness != null)
                //{
                //    ediDocument.Segments.Add(new EdiSegment("DTP")
                //    {
                //        [01] = "431",
                //        [02] = "D8",
                //        [03] = EdiValue.Date(8, claim.OnsetOfCurrentIllness ?? DateTime.MinValue)
                //    });
                //    segmentCount++;
                //}
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
            }

            if (claimType == ClaimType.Institutional)
            {
                //DTP - Discharge Hour - req'd for inpatient claims
                //DTP - Statement Dates
                string statementFromDate = EdiValue.Date(8, claim.StatementFromDate ?? DateTime.MinValue);
                string statementThruDate = EdiValue.Date(8, claim.StatementThruDate ?? DateTime.MinValue);
                ediDocument.Segments.Add(new EdiSegment("DTP")
                {
                    [01] = "434",
                    [02] = "RD8",
                    [03] = $"{statementFromDate}-{statementThruDate}"
                });
                segmentCount++;
                //DTP - Admission Date/Hour - req'd for inpatient cliams
                //DTP - Repricer received date
                //CL1 - institutional claim code
                ediDocument.Segments.Add(new EdiSegment("CL1")
                {

                    [03] = "01", //patient status code - core source 239 - hardcoding to 01 for now

                });
                segmentCount++;
            }


            // --PWK - Claim supplemental information
            // --CN1 - Contact Information
            // --AMT - Estimated patient responsibility - not applicable currently
            //if (claim.PatientAmountPaid > 0.0)
            //{
            //    ediDocument.Segments.Add(new EdiSegment("AMT")
            //    {
            //        [01] = "F5",
            //        [02] = claim.PatientAmountPaid.ToString("F2")
            //    });
            //    segmentCount++;
            //}
            // --REF - several REF segments as needed
            if (claimType == ClaimType.Professional)
            {
                //CLIA number
                ediDocument.Segments.Add(new EdiSegment("REF")
                {
                    [01] = "X4",
                    [02] = claim.CliaNumber
                });
                segmentCount++;
            }
            if (claimType == ClaimType.Institutional)
            {
                // medical record number
                if (claim.claimAccount.MRN != string.Empty)
                {
                    ediDocument.Segments.Add(new EdiSegment("REF")
                    {
                        [01] = "EA",
                        [02] = claim.claimAccount.MRN
                    });
                    segmentCount++;
                }
            }

            // --K3 - file information
            // --NTE - claim note
            // --CR1 - Ambulance Transport information
            // --CR2 - Spinal manipulation service information
            // --CRC - several certification segments as needed
            // --HI - Healthcare diagnosis code
            var hi = new EdiSegment("HI");
            int dxCnt = 1;
            if (claimType == ClaimType.Institutional)
            {
                foreach (PatDiag diag in claim.claimAccount.Pat.Diagnoses)
                {
                    //per spec "ABK" is code for ICD-10, but does not pass validation
                    //using "BK" for icd9
                    var hiElement = new EdiElement();
                    hiElement[1] = "ABK";
                    hiElement[2] = diag.Code;

                    hi.Element(dxCnt, hiElement);
                    dxCnt++;
                }
                ediDocument.Segments.Add(hi);
                segmentCount++;
            }
            if (claimType == ClaimType.Professional)
            {
                foreach (PatDiag diag in claim.claimAccount.Pat.Diagnoses)
                {
                    var hiElement = new EdiElement();
                    if (dxCnt == 1)
                    {
                        hiElement[1] = "ABK";
                        hiElement[2] = diag.Code;
                    }
                    if (dxCnt > 1)
                    {
                        hiElement[1] = "ABF";
                        hiElement[2] = diag.Code;
                    }
                    hi.Element(dxCnt, hiElement);
                    dxCnt++;
                }

                ediDocument.Segments.Add(hi);
                segmentCount++;
            }

            // --HI - Anesthesia related procedure
            // --HI - condition information
            // --HCP - healthcare repricing information

            // Loop 2310A - Referring Provider Name or AttendingProvider Name
            if (claimType == ClaimType.Institutional)
            {
                ediDocument.Segments.Add(new EdiSegment("NM1")
                {
                    [01] = "71",
                    [02] = "1",
                    [03] = claim.AttendingProviderLastName,
                    [04] = claim.AttendingProviderFirstName,
                    [05] = claim.AttendingProviderMiddleName,
                    [06] = claim.AttendingProviderSuffix,
                    [08] = "XX",
                    [09] = claim.AttendingProviderNPI
                });
                segmentCount++;
            }
            // Loop 2310A - Referring Provider Name or AttendingProvider Name
            // - NM1 - Referring provider name
            if (claimType == ClaimType.Professional)
            {
                ediDocument.Segments.Add(new EdiSegment("NM1")
                {
                    [01] = "DN",
                    [02] = "1",
                    [03] = claim.ReferringProviderLastName,
                    [04] = claim.ReferringProviderFirstName,
                    [05] = claim.ReferringProviderMiddleName,
                    [06] = claim.ReferringProviderSuffix,
                    [08] = "XX",
                    [09] = claim.ReferringProviderNPI

                });
                segmentCount++;
                // - REF - Referring provider secondary information
            }

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
            return segmentCount;
        }

        private int Loop2400()
        {
            int segmentCount = 0;
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

                switch (claimType)
                {
                    case ClaimType.Institutional:
                        var sv2 = new EdiSegment("SV2");
                        sv2[1] = line.RevenueCode; //revenue code

                        var sv2_2 = new EdiElement();
                        sv2_2[1] = "HC"; //HC for cpt4/hcpcs codeset
                        sv2_2[2] = line.ProcedureCode;
                        sv2_2[3] = line.ProcedureModifier1;
                        sv2_2[4] = line.ProcedureModifier2;
                        sv2_2[5] = line.ProcedureModifier3;
                        sv2_2[7] = line.Description;
                        sv2.Element(2, sv2_2);

                        sv2[3] = line.Amount.ToString();
                        sv2[4] = "UN";
                        sv2[5] = line.Quantity.ToString();
                        sv2[7] = ""; //non-covered amount - if needed

                        ediDocument.Segments.Add(sv2);
                        segmentCount++;
                        break;
                    case ClaimType.Professional:
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
                        break;
                    default:
                        break;
                }

                // - PWK - Line supplemental information
                // - DTP - date - service line
                ediDocument.Segments.Add(new EdiSegment("DTP")
                {
                    [01] = "472",
                    [02] = "D8",
                    [03] = EdiValue.Date(8, line.ServiceDate ?? DateTime.MinValue)
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
            return segmentCount;
        }

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

        public enum ClaimType
        {
            Institutional,
            Professional
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
