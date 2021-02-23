<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="HL7Message">
      <html>
        <head></head>
      <body>
        <STYLE type="text/css">
          .Shadow {
          border: solid 1px #336699;
          border-collapse:collapse;
          background-color: White;
          margin-bottom:2px;
          filter: progid:DXImageTransform.Microsoft.Shadow(color=#141414,Direction=135, Strength=8);}
        </STYLE>
        <!--// NTE font and color style-->
        <STYLE type="text/css">
          #NteRow {margin:0;padding:0;border:0};
        </STYLE>
        <STYLE type="text/css">"
        #NteFont {FONT SIZE = 9; COLOR=RED};
        </STYLE>
        <!--// end of NTE-->
        <table border ="1">
          <tr bgcolor="#AAAAAA">
            <th  align="left">Header</th>
            <th align="left">Field Seperator</th>
            <th align="left">Encoding Characters</th>
            <th align="left">Sending Application</th>
            <th align="left">Sending Facility</th>
            <th align="left">Receiving Application</th>
            <th align="left">Receiving Facility</th>
            <th align="left">Message Date and Time</th>
            <th align="left">Security</th>
            <th align="left">Message Type</th>
            <th align="left">Message Control ID</th>
            <th align="left">Processing ID</th>
            <th align="left">Version</th>
            <th align="left">Sequence Number</th>
            <th align="left">Continuation Pointer</th>
            <th align="left">Accept Acknowledgement Type</th>
            <th align="left">Application Acknowledgement Type</th>
            <th align="left">Country Code</th>
          </tr>
          <xsl:for-each select="MSHsegment">
            <tr bgcolor="#f1f1f1">
              <td>MSH</td>              
              <td align="center">
                <xsl:value-of select="_1_field-seperator"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_encoding-characters"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_sending-app"/>
              </td>
              <td align="center">
                <xsl:value-of select="_4_sending-facility"/>
              </td>
              <td align="center">
                <xsl:value-of select="_5_receiving-app"/>
              </td>
              <td align="center">
                <xsl:value-of select="_6_receiving-facility"/>
              </td>
              <td align="center">
                <xsl:value-of select="_7_message-datetime"/>
              </td>
              <td align="center">
                <xsl:value-of select="_8_security"/>
              </td>
              <td align="center">
                <xsl:value-of select="_9_message-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_10_message-control-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_11_processing-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_12_version-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_13_sequence-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_14_continuation-pointer"/>
              </td>
              <td align="center">
                <xsl:value-of select="_15_accept-acknowledgement-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_16_application-acknowledgement-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_17_country-code"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <table border ="1">
          <tr bgcolor="#AAAAAA">
            <th align="left">Message Header</th>
            <th align="left">Set ID</th>
            <th align="left">Patient ID (HNE)</th>
            <th align="left">Patient ID (CLIENT)</th>
            <th align="left">Alternate Patient ID (CLIENT)</th>
            <th align="left">Patient Name</th>
            <th align="left">Mothers Maiden Name</th>
            <th align="left">Date of Birth</th>
            <th align="left">Gender</th>
            <th align="left">Patient Alias</th>
            <th align="left">Race</th>
            <th align="left">Patient Address</th>
            <th align="left">Country Code</th>
            <th align="left">Phone Number (HOME)</th>
            <th align="left">Phone Number (BUSINESS)</th>
            <th align="left">Primnary Language</th>
            <th align="left">Marital Status</th>
            <th align="left">Religion</th>
            <th align="left">Patient Account Number (CLIENT)</th>
            <th align="left">Patient SSN</th>
            
          </tr>
          <xsl:for-each select="PIDsegment">
            <tr bgcolor="#f1f1f1">
              <td>Patient Info</td>
              <td align="center">
                <xsl:value-of select="_1_set-id-patientid"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_patientid-externalid"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_patientid-internalid"/>
              </td>
              <td align="center">
                <xsl:value-of select="_4_alternate-patient-id-pid"/>
              </td>
              <td align="center">
                <xsl:value-of select="_5_patient-name"/>
              </td>
              <td align="center">
                <xsl:value-of select="_6_mothers-maiden-name"/>
              </td>
              <td align="center">
                <xsl:value-of select="_7_datetime-of-birth"/>
              </td>
              <td align="center">
                <xsl:value-of select="_8_sex"/>
              </td>
              <td align="center">
                <xsl:value-of select="_9_patient-alias"/>
              </td>
              <td align="center">
                <xsl:value-of select="_10_race"/>
              </td>
              <td align="center">
                <xsl:value-of select="_11_patient-address"/>
              </td>
              <td align="center">
                <xsl:value-of select="_12_county-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_13_phone-number-home"/>
              </td>
              <td align="center">
                <xsl:value-of select="_14_phone-number-business"/>
              </td>
              <td align="center">
                <xsl:value-of select="_15_primary-language"/>
              </td>
              <td align="center">
                <xsl:value-of select="_16_marital-status"/>
              </td>
              <td align="center">
                <xsl:value-of select="_17_religion"/>
              </td>
              <td align="center">
                <xsl:value-of select="_18_patient-account-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_19_ssn-number-patient"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <table border ="1">
          <tr bgcolor="#AAAAAA">
            <th align="left">Message Header</th>
            <th align="left">Set ID</th>
            <th align="left">Patient Class</th>
            <th align="left">Patient Assigned Location</th>
            <th align="left">Admission Type</th>
            <th align="left">Preadmit Number</th>
            <th align="left">Prior Patient Location</th>
            <th align="left">Attending Doctor</th>
            <th align="left">Referring Doctor</th>
            <th align="left">Consulting Doctor</th>
            <th align="left">Hospital Service</th>
            <th align="left">Temporary Location</th>
            <th align="left">Preadmit Test Indicator</th>
            <th align="left">Readmission Indicator</th>
            <th align="left">Admit Source</th>
            <th align="left">Ambularory Status</th>
            <th align="left">VIP Indicator</th>
            <th align="left">Admitting Doctor</th>
            <th align="left">Patient Type</th>
            <th align="left">Visit Number</th>
            <th align="left">Financial Class</th>
            <th align="left">Charge Price Indicator</th>
            <th align="left">Curtesy Code</th>
            <th align="left">Credit Rating</th>
            <th align="left">Contract Code</th>
            <th align="left">Contract Effective Date</th>
            <th align="left">Contract Amount</th>
            <th align="left">Contract Period</th>
            <th align="left">Interest Code</th>
            <th align="left">Transfer to Bad Debt Code</th>
            <th align="left">Transfer to Bad Debt Date</th>
            <th align="left">Bad Debt Agency Code</th>
            <th align="left">Bad Debt Transfer Amount</th>
            <th align="left">Bad Debt Recovery Amount</th>
            <th align="left">Delete Account Indicator</th>
            <th align="left">Delete Account Date</th>
            <th align="left">Discharge Disposition</th>
            <th align="left">Discharged to Location</th>
            <th align="left">Diet Type</th>
            <th align="left">Servicing Facility</th>
            <th align="left">Bed Status</th>
            <th align="left">Account Status</th>
            <th align="left">Pending Location</th>
            <th align="left">Prior Temporary Location</th>
            <th align="left">Admit Date/Time</th>
            <th align="left">Discharge Date/Time</th>
          </tr>
          <xsl:for-each select="PV1segment">
            <tr bgcolor="#f1f1f1">
              <td>Visit Info</td>
              <td align="center">
                <xsl:value-of select="_1_set-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_patient-class"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_assigned-patient-location"/>
              </td>
              <td align="center">
                <xsl:value-of select="_4_admission-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_5_preadmit-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_6_prior-patient-location"/>
              </td>
              <td align="center">
                <xsl:value-of select="_7_attending-doctor"/>
              </td>
              <td align="center">
                <xsl:value-of select="_8_referring-doctor"/>
              </td>
              <td align="center">
                <xsl:value-of select="_9_consulting-doctor"/>
              </td>
              <td align="center">
                <xsl:value-of select="_10_hospital-service"/>
              </td>
              <td align="center">
                <xsl:value-of select="_11_temporary-location"/>
              </td>
              <td align="center">
                <xsl:value-of select="_12_preadmit-test-indicator"/>
              </td>
              <td align="center">
                <xsl:value-of select="_13_readmission-indicator"/>
              </td>
              <td align="center">
                <xsl:value-of select="_14_admit-source"/>
              </td>
              <td align="center">
                <xsl:value-of select="_15_ambulatory-status"/>
              </td>
              <td align="center">
                <xsl:value-of select="_16_vip-indicator"/>
              </td>
              <td align="center">
                <xsl:value-of select="_17_admitting-doctor"/>
              </td>
              <td align="center">
                <xsl:value-of select="_18_patient-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_19_visit-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_20_financial-class"/>
              </td>
              <td align="center">
                <xsl:value-of select="_21_charge-price-indicator"/>
              </td>
              <td align="center">
                <xsl:value-of select="_22_curtesy-code "/>
              </td>
              <td align="center">
                <xsl:value-of select="_23_credit-rating"/>
              </td>
              <td align="center">
                <xsl:value-of select="_24_contract-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_25_contract-effective-date"/>
              </td>
              <td align="center">
                <xsl:value-of select="_26_contract-amount"/>
              </td>
              <td align="center">
                <xsl:value-of select="_27_contract-period"/>
              </td>
              <td align="center">
                <xsl:value-of select="_28_interest-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_29_transfer-to-bad-debt-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_30_transfer-to-bad-debt-date"/>
              </td>
              <td align="center">
                <xsl:value-of select="_31_bad-debt-agency-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_32_bad-debt-transfer-amount"/>
              </td>
              <td align="center">
                <xsl:value-of select="_33_bad-debt-recovery-amount "/>
              </td>
              <td align="center">
                <xsl:value-of select="_34_delete-account-indicator"/>
              </td>
              <td align="center">
                <xsl:value-of select="_35_delete-account-date"/>
              </td>
              <td align="center">
                <xsl:value-of select="_36_discharge-disposition"/>
              </td>
              <td align="center">
                <xsl:value-of select="_37_discharged-to-location"/>
              </td>
              <td align="center">
                <xsl:value-of select="_38_diet-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_39_servicing-facility"/>
              </td>
              <td align="center">
                <xsl:value-of select="_40_bed-status"/>
              </td>
              <td align="center">
                <xsl:value-of select="_41_account-status"/>
              </td>
              <td align="center">
                <xsl:value-of select="_42_pending-location"/>
              </td>
              <td align="center">
                <xsl:value-of select="_43_prior-tempory-location"/>
              </td>
              <td align="center">
                <xsl:value-of select="_44_admit-datetime"/>
              </td>
              <td align="center">
                <xsl:value-of select="_45_discharge-datetime"/>
              </td>
        
            </tr>
          </xsl:for-each>
        </table>
        <table border ="1">
          <tr bgcolor="#AAAAAA">INSURANCE INFO</tr>
          <tr bgcolor="#AAAAAA">
            <th  align="left">Header</th>
            <th align="left">Set ID</th>
            <th align="left">Plan ID</th>
            <th align="left">Company ID</th>
            <th align="left">Company Name</th>
            <th align="left">Company Address</th>
            <th align="left">Company Contact</th>
            <th align="left">Company Phone Number</th>
            <th align="left">Group Number</th>
            <th align="left">Group Name</th>
            <th align="left">Insured's Group Emp Id </th>
            <th align="left">Insured's Group Emp Name </th>
            <th align="left">Date Plan Effecive</th>
            <th align="left">Date Plan Expiration</th>
            <th align="left">Authorization Information</th>
            <th align="left">Plan Type</th>
            <th align="left">Name of Insured</th>
            <th align="left">Insured's relation to Patient</th>
            <th align="left">Insured's Date of Birth</th>
            <th align="left">Insured's Address</th>
            <th align="left">Assignment of Benefits</th>
            <th align="left">Coordination of Benefits Priority</th>
            <th align="left">Notice of Admission Flag</th>
            <th align="left">Notice of Admission Date</th>
            <th align="left">Report of Eligibility Flag</th>
            <th align="left">Date Report of Eligibility</th>
            <th align="left">Release of Information Code</th>
            <th align="left">PreAdmit Cert</th>
            <th align="left">Date/Time Verification</th>
            <th align="left">Verification By</th>
            <th align="left">Type of Agreement Code</th>
            <th align="left">Billing Status</th>
            <th align="left">Lifetime reserve Days</th>
            <th align="left">Delay Before LR Day</th>
            <th align="left">Company Plan Code</th>
            <th align="left">Policy Number</th>
            <th align="left">Policy Deductible</th>
            <th align="left">Policy Limit Amount</th>
            <th align="left">Policy Limit Days</th>
            <th align="left">Room Rate Semi-Private</th>
            <th align="left">Room Rate Private</th>
            <th align="left">Insured's Employment Status</th>
            <th align="left">Insured's Sex</th>
            <th align="left">Insured's Employer Address</th>
            
          </tr>
          <xsl:for-each select="IN1segment">
            <tr bgcolor="#f1f1f1">
              <td>IN1</td>
              <td align="center">
                <xsl:value-of select="_1_set-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_insurance-plan-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_insurance-company-id"/>
              </td>
              <td align="left">
                <xsl:value-of select="_4_insurance-company-name"/>
              </td>
              <td align="left">
                <xsl:value-of select="_5_insurance-company-address"/>
              </td>
              <td align="center">
                <xsl:value-of select="_6_insurance-company-contact-person"/>
              </td>
              <td align="center">
                <xsl:value-of select="_7_insurance-company-phone-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_8_group-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_9_group-name"/>
              </td>
              <td align="center">
                <xsl:value-of select="_10_insureds-group-emp-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_11_insureds-group-emp-name"/>
              </td>
              <td align="center">
                <xsl:value-of select="_12_date-plan-effective"/>
              </td>
              <td align="center">
                <xsl:value-of select="_13_date-plan-expiration"/>
              </td>
              <td align="center">
                <xsl:value-of select="_14_authorization-information"/>
              </td>
              <td align="center">
                <xsl:value-of select="_15_plan-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_16_name-of-insured"/>
              </td>
              <td align="center">
                <xsl:value-of select="_17_insureds-relation-to-patient"/>
              </td>
              <td align="center">
                <xsl:value-of select="_18_date-of-birth-insureds"/>
              </td>
              <td align="center">
                <xsl:value-of select="_19_insureds-address"/>
              </td>
              <td align="center">
                <xsl:value-of select="_20_assignment-of-benefits"/>
              </td>
              <td align="center">
                <xsl:value-of select="_21_coord-of-ben-priority"/>
              </td>
              <td align="center">
                <xsl:value-of select="_22_notice-of-admission-flag"/>
              </td>
              <td align="center">
                <xsl:value-of select="_23_notice-of-admission-date"/>
              </td>
              <td align="center">
                <xsl:value-of select="_24_report-of-eligibility-flag"/>
              </td>
              <td align="center">
                <xsl:value-of select="_25_date-report-of-eligibility"/>
              </td>
              <td align="center">
                <xsl:value-of select="_26_release-information-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_27_pre-admit-cert"/>
              </td>
              <td align="center">
                <xsl:value-of select="_28_datetime-verification"/>
              </td>
              <td align="center">
                <xsl:value-of select="_29_verification-by"/>
              </td>
              <td align="center">
                <xsl:value-of select="_30_type-of-agreement-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_31_billing-status"/>
              </td>
              <td align="center">
                <xsl:value-of select="_32_lifetime-reserve-days"/>
              </td>
              <td align="center">
                <xsl:value-of select="_33_delay-before-lr-day"/>
              </td>
              <td align="center">
                <xsl:value-of select="_34_company-plan-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_35_policy-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_36_policy-deductible"/>
              </td>
              <td align="center">
                <xsl:value-of select="_37_policy-limit-amount"/>
              </td>
              <td align="center">
                <xsl:value-of select="_38_policy-limit-days"/>
              </td>
              <td align="center">
                <xsl:value-of select="_39_room-rate-semi-private"/>
              </td>
              <td align="center">
                <xsl:value-of select="_40_room-rate-private"/>
              </td>
              <td align="center">
                <xsl:value-of select="_41_insureds-employment-status"/>
              </td>
              <td align="center">
                <xsl:value-of select="_42_insureds-sex"/>
              </td>
              <td align="center">
                <xsl:value-of select="_43_insureds-employer-address"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
        <table border ="1">
          <tr bgcolor="#AAAAAA">GUARANTOR INFO</tr>
          <tr bgcolor="#AAAAAA">
            <th  align="left">Header</th>
            <th align="left">Set ID</th>
            <th align="left">Number</th>
            <th align="left">Name</th>
            <th align="left">Spouse's Name</th>
            <th align="left">Address</th>
            <th align="left">Phone Number (HOME)</th>
            <th align="left">Phone Number (BUSINESS)</th>
            <th align="left">Date of Birth</th>
            <th align="left">Gender</th>
            <th align="left">Type</th>
            <th align="left">Relationship</th>
          </tr>
          <xsl:for-each select="GT1segment">
            <tr bgcolor="#f1f1f1">
              <td>GT1</td>
              <td align="center">
                <xsl:value-of select="_1_set-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_guarantor-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_guarantor-name"/>
              </td>
              <td align="center">
                <xsl:value-of select="_4_guarantor-spouse-name"/>
              </td>
              <td align="center">
                <xsl:value-of select="_5_guarantor-address"/>
              </td>
              <td align="center">
                <xsl:value-of select="_6_guarantor-phone-num-home"/>
              </td>
              <td align="center">
                <xsl:value-of select="_7_guarantor-phone-num-business"/>
              </td>
              <td align="center">
                <xsl:value-of select="_8_datetime-guarantor-birth"/>
              </td>
              <td align="center">
                <xsl:value-of select="_9_guarantor-sex"/>
              </td>
              <td align="center">
                <xsl:value-of select="_10_guarantor-type"/>
              </td>
              <td align="center">
                <xsl:value-of select="_11_guarantor-relationship"/>
              </td>
             
            </tr>
          </xsl:for-each>
        </table>
        <table border ="1">
          <tr bgcolor="#AAAAAA">DIAGNOISI INFO</tr>
          <tr bgcolor="#AAAAAA">
            <th  align="left">Header</th>
            <th align="left">Set ID</th>
            <th align="left">Coding Method</th>
            <th align="left">Code</th>
            <th align="left">Description</th>
            
          </tr>
          <xsl:for-each select="DG1segment">
            <tr bgcolor="#f1f1f1">
              <td>DG1</td>
              <td align="center">
                <xsl:value-of select="_1_set-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_diagnosis-coding-method"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_diagnosis-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_4_diagnosis-description"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <table border ="1">
          <tr bgcolor="#AAAAAA">COMMON ORDER INFO</tr>
          <tr bgcolor="#AAAAAA">
            <th  align="left">Header</th>
            <th align="left">Order Control</th>
            <th align="left">Placers Order Number</th>
            <th align="left">Fillers Order Number</th>
            <th align="left">Placer Group Number</th>
            <th align="left">Order Status</th>
            <th align="left">Response Flag</th>
            <th align="left">Quantity / Timing</th>
            <th align="left">Parent</th>
            <th align="left">Date/Time of Transaction</th>
            <th align="left">Entered By</th>
            <th align="left">Verified By</th>
            <th align="left">Ordering Provider</th>
            
          </tr>
          <xsl:for-each  select ="ORCsegment">
            <tr bgcolor="#f1f1f1">
              <td>ORC</td>
              <td align="center">
                <xsl:value-of select="_1_order-control"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_placer-order-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_filler-order-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_4_placer-group-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_5_order-status"/>
              </td>
              <td align="center">
                <xsl:value-of select="_6_response-flag"/>
              </td>
              <td align="center">
                <xsl:value-of select="_7_quantity-timing"/>
              </td>
              <td align="center">
                <xsl:value-of select="_8_parent"/>
              </td>
              <td align="center">
                <xsl:value-of select="_9_datetime-of-transaction"/>
              </td>
              <td align="center">
                <xsl:value-of select="_10_entered-by"/>
              </td>
              <td align="center">
                <xsl:value-of select="_11_verified-by"/>
              </td>
              <td align="center">
                <xsl:value-of select="_12_ordering-provider"/>
              </td>
              
            </tr>
          </xsl:for-each>
        </table>

        <table border ="1">
          <tr bgcolor="#AAAAAA">ORDER INFO</tr>
          <tr bgcolor="#AAAAAA">
            <th  align="left">Header</th>
            <th align="left">Set ID</th>
            <th align="left">Placers Order Number</th>
            <th align="left">Fillers Order Number</th>
            <th align="left">Universal Service ID</th>
            <th align="left">Priority</th>
            <th align="left">Requested DateTime</th>
            <th align="left">Observation Start DateTime</th>
            <th align="left">Observation End DateTime</th>
            <th align="left">Collection Volumne</th>
            <th align="left">Collector Identifier</th>
            <th align="left">Specimen Action Code</th>
            <th align="left">Danger Code</th>
            <th align="left">Relevant Clinical Info</th>
            <th align="left">Specimen Received DateTime</th>
            <th align="left">Specimen Source</th>
            <th align="left">Ordering Provider</th>
          </tr>
          <xsl:for-each  select ="OBRsegment">
            <tr bgcolor="#f1f1f1">
              <td>OBR</td>
              <td align="center">
                <xsl:value-of select="_1_set-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_2_placer-order-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_3_filler-order-number"/>
              </td>
              <td align="center">
                <xsl:value-of select="_4_universal-service-id"/>
              </td>
              <td align="center">
                <xsl:value-of select="_5_priority"/>
              </td>
              <td align="center">
                <xsl:value-of select="_6_requested-datetime"/>
              </td>
              <td align="center">
                <xsl:value-of select="_7_observation-start-datetime"/>
              </td>
              <td align="center">
                <xsl:value-of select="_8_observation-end-datetime"/>
              </td>
              <td align="center">
                <xsl:value-of select="_9_collection-volumne"/>
              </td>
              <td align="center">
                <xsl:value-of select="_10_collector-identifier"/>
              </td>
              <td align="center">
                <xsl:value-of select="_11_specimen-action-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_12_danger-code"/>
              </td>
              <td align="center">
                <xsl:value-of select="_13_relevant-clinical-info"/>
              </td>
              <td align="center">
                <xsl:value-of select="_14_specimen-received-datetime"/>
              </td>
              <td align="center">
                <xsl:value-of select="_15_specimen-source"/>
              </td>
              <td align="center">
                <xsl:value-of select="_16_ordering-provider"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>

      </body>
      </html>
    </xsl:template>
</xsl:stylesheet>
