using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using EdiTools;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;

namespace LabBilling.Core.BusinessLogic
{
    public class Remittance835
    {
        public Remittance835(string connString)
        {
            _connString = connString;
        }

        private string _connString;

        public Dictionary<string, string> adjustmentReasonCodes = new Dictionary<string, string>()
        {
            {"1","Deductible Amount"},
            {"2","Coinsurance Amount"},
            {"3","Co-payment Amount"},
            {"4","The procedure code is inconsistent with the modifier used. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"5","The procedure code/type of bill is inconsistent with the place of service. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"6","The procedure/revenue code is inconsistent with the patient's age. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"7","The procedure/revenue code is inconsistent with the patient's gender. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"8","The procedure code is inconsistent with the provider type/specialty (taxonomy). Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"9","The diagnosis is inconsistent with the patient's age. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"10","The diagnosis is inconsistent with the patient's gender. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"11","The diagnosis is inconsistent with the procedure. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"12","The diagnosis is inconsistent with the provider type. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"13","The date of death precedes the date of service."},
            {"14","The date of birth follows the date of service."},
            {"16","Claim/service lacks information or has submission/billing error(s). Usage: Do not use this code for claims attachment(s)/other documentation. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.) Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"18","Exact duplicate claim/service (Use only with Group Code OA except where state workers' compensation regulations requires CO)"},
            {"19","This is a work-related injury/illness and thus the liability of the Worker's Compensation Carrier."},
            {"20","This injury/illness is covered by the liability carrier."},
            {"21","This injury/illness is the liability of the no-fault carrier."},
            {"22","This care may be covered by another payer per coordination of benefits."},
            {"23","The impact of prior payer(s) adjudication including payments and/or adjustments. (Use only with Group Code OA)"},
            {"24","Charges are covered under a capitation agreement/managed care plan."},
            {"26","Expenses incurred prior to coverage."},
            {"27","Expenses incurred after coverage terminated."},
            {"29","The time limit for filing has expired."},
            {"31","Patient cannot be identified as our insured."},
            {"32","Our records indicate the patient is not an eligible dependent."},
            {"33","Insured has no dependent coverage."},
            {"34","Insured has no coverage for newborns."},
            {"35","Lifetime benefit maximum has been reached."},
            {"39","Services denied at the time authorization/pre-certification was requested."},
            {"40","Charges do not meet qualifications for emergent/urgent care. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"44","Prompt-pay discount."},
            {"45","Charge exceeds fee schedule/maximum allowable or contracted/legislated fee arrangement. Usage: This adjustment amount cannot equal the total service or claim charge amount; and must not duplicate provider adjustment amounts (payments and contractual reductions) that have resulted from prior payer(s) adjudication. (Use only with Group Codes PR or CO depending upon liability)"},
            {"49","This is a non-covered service because it is a routine/preventive exam or a diagnostic/screening procedure done in conjunction with a routine/preventive exam. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"50","These are non-covered services because this is not deemed a 'medical necessity' by the payer. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"51","These are non-covered services because this is a pre-existing condition. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"53","Services by an immediate relative or a member of the same household are not covered."},
            {"54","Multiple physicians/assistants are not covered in this case. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"55","Procedure/treatment/drug is deemed experimental/investigational by the payer. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"56","Procedure/treatment has not been deemed 'proven to be effective' by the payer. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"58","Treatment was deemed by the payer to have been rendered in an inappropriate or invalid place of service. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"59","Processed based on multiple or concurrent procedure rules. (For example multiple surgery or diagnostic imaging, concurrent anesthesia.) Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"60","Charges for outpatient services are not covered when performed within a period of time prior to or after inpatient services."},
            {"61","Adjusted for failure to obtain second surgical opinion"},
            {"66","Blood Deductible."},
            {"69","Day outlier amount."},
            {"70","Cost outlier - Adjustment to compensate for additional costs."},
            {"74","Indirect Medical Education Adjustment."},
            {"75","Direct Medical Education Adjustment."},
            {"76","Disproportionate Share Adjustment."},
            {"78","Non-Covered days/Room charge adjustment."},
            {"85","Patient Interest Adjustment (Use Only Group code PR)"},
            {"89","Professional fees removed from charges."},
            {"90","Ingredient cost adjustment. Usage: To be used for pharmaceuticals only."},
            {"91","Dispensing fee adjustment."},
            {"94","Processed in Excess of charges."},
            {"95","Plan procedures not followed."},
            {"96","Non-covered charge(s). At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.) Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"97","The benefit for this service is included in the payment/allowance for another service/procedure that has already been adjudicated. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"100","Payment made to patient/insured/responsible party."},
            {"101","Predetermination: anticipated payment upon completion of services or claim adjudication."},
            {"102","Major Medical Adjustment."},
            {"103","Provider promotional discount (e.g., Senior citizen discount)."},
            {"104","Managed care withholding."},
            {"105","Tax withholding."},
            {"106","Patient payment option/election not in effect."},
            {"107","The related or qualifying claim/service was not identified on this claim. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"108","Rent/purchase guidelines were not met. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"109","Claim/service not covered by this payer/contractor. You must send the claim/service to the correct payer/contractor."},
            {"110","Billing date predates service date."},
            {"111","Not covered unless the provider accepts assignment."},
            {"112","Service not furnished directly to the patient and/or not documented."},
            {"114","Procedure/product not approved by the Food and Drug Administration."},
            {"115","Procedure postponed, canceled, or delayed."},
            {"116","The advance indemnification notice signed by the patient did not comply with requirements."},
            {"117","Transportation is only covered to the closest facility that can provide the necessary care."},
            {"118","ESRD network support adjustment."},
            {"119","Benefit maximum for this time period or occurrence has been reached."},
            {"121","Indemnification adjustment - compensation for outstanding member responsibility."},
            {"122","Psychiatric reduction."},
            {"128","Newborn's services are covered in the mother's Allowance."},
            {"129","Prior processing information appears incorrect. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"130","Claim submission fee."},
            {"131","Claim specific negotiated discount."},
            {"132","Prearranged demonstration project adjustment."},
            {"133","The disposition of this service line is pending further review. (Use only with Group Code OA). Usage: Use of this code requires a reversal and correction when the service line is finalized (use only in Loop 2110 CAS segment of the 835 or Loop 2430 of the 837)."},
            {"134","Technical fees removed from charges."},
            {"135","Interim bills cannot be processed."},
            {"136","Failure to follow prior payer's coverage rules. (Use only with Group Code OA)"},
            {"137","Regulatory Surcharges, Assessments, Allowances or Health Related Taxes."},
            {"139","Contracted funding agreement - Subscriber is employed by the provider of services. Use only with Group Code CO."},
            {"140","Patient/Insured health identification number and name do not match."},
            {"142","Monthly Medicaid patient liability amount."},
            {"143","Portion of payment deferred."},
            {"144","Incentive adjustment, e.g. preferred product/service."},
            {"146","Diagnosis was invalid for the date(s) of service reported."},
            {"147","Provider contracted/negotiated rate expired or not on file."},
            {"148","Information from another provider was not provided or was insufficient/incomplete. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"149","Lifetime benefit maximum has been reached for this service/benefit category."},
            {"150","Payer deems the information submitted does not support this level of service."},
            {"151","Payment adjusted because the payer deems the information submitted does not support this many/frequency of services."},
            {"152","Payer deems the information submitted does not support this length of service. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"153","Payer deems the information submitted does not support this dosage."},
            {"154","Payer deems the information submitted does not support this day's supply."},
            {"155","Patient refused the service/procedure."},
            {"157","Service/procedure was provided as a result of an act of war."},
            {"158","Service/procedure was provided outside of the United States."},
            {"159","Service/procedure was provided as a result of terrorism."},
            {"160","Injury/illness was the result of an activity that is a benefit exclusion."},
            {"161","Provider performance bonus"},
            {"163","Attachment/other documentation referenced on the claim was not received."},
            {"164","Attachment/other documentation referenced on the claim was not received in a timely fashion."},
            {"166","These services were submitted after this payers responsibility for processing claims under this plan ended."},
            {"167","This (these) diagnosis(es) is (are) not covered. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"169","Alternate benefit has been provided."},
            {"170","Payment is denied when performed/billed by this type of provider. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"171","Payment is denied when performed/billed by this type of provider in this type of facility. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"172","Payment is adjusted when performed/billed by a provider of this specialty. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"173","Service/equipment was not prescribed by a physician."},
            {"174","Service was not prescribed prior to delivery."},
            {"175","Prescription is incomplete."},
            {"176","Prescription is not current."},
            {"177","Patient has not met the required eligibility requirements."},
            {"178","Patient has not met the required spend down requirements."},
            {"179","Patient has not met the required waiting requirements. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"180","Patient has not met the required residency requirements."},
            {"181","Procedure code was invalid on the date of service."},
            {"182","Procedure modifier was invalid on the date of service."},
            {"183","The referring provider is not eligible to refer the service billed. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"184","The prescribing/ordering provider is not eligible to prescribe/order the service billed. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"185","The rendering provider is not eligible to perform the service billed. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"186","Level of care change adjustment."},
            {"187","Consumer Spending Account payments (includes but is not limited to Flexible Spending Account, Health Savings Account, Health Reimbursement Account, etc.)"},
            {"188","This product/procedure is only covered when used according to FDA recommendations."},
            {"189","'Not otherwise classified' or 'unlisted' procedure code (CPT/HCPCS) was billed when there is a specific procedure code for this procedure/service"},
            {"190","Payment is included in the allowance for a Skilled Nursing Facility (SNF) qualified stay."},
            {"192","Non standard adjustment code from paper remittance. Usage: This code is to be used by providers/payers providing Coordination of Benefits information to another payer in the 837 transaction only. This code is only used when the non-standard code cannot be reasonably mapped to an existing Claims Adjustment Reason Code, specifically Deductible, Coinsurance and Co-payment."},
            {"193","Original payment decision is being maintained. Upon review, it was determined that this claim was processed properly."},
            {"194","Anesthesia performed by the operating physician, the assistant surgeon or the attending physician."},
            {"195","Refund issued to an erroneous priority payer for this claim/service."},
            {"197","Precertification/authorization/notification/pre-treatment absent."},
            {"198","Precertification/notification/authorization/pre-treatment exceeded."},
            {"199","Revenue code and Procedure code do not match."},
            {"200","Expenses incurred during lapse in coverage"},
            {"201","Patient is responsible for amount of this claim/service through 'set aside arrangement' or other agreement. (Use only with Group Code PR) At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"202","Non-covered personal comfort or convenience services."},
            {"203","Discontinued or reduced service."},
            {"204","This service/equipment/drug is not covered under the patient's current benefit plan"},
            {"205","Pharmacy discount card processing fee"},
            {"206","National Provider Identifier - missing."},
            {"207","National Provider identifier - Invalid format"},
            {"208","National Provider Identifier - Not matched."},
            {"209","Per regulatory or other agreement. The provider cannot collect this amount from the patient. However, this amount may be billed to subsequent payer. Refund to patient if collected. (Use only with Group code OA)"},
            {"210","Payment adjusted because pre-certification/authorization not received in a timely fashion"},
            {"211","National Drug Codes (NDC) not eligible for rebate, are not covered."},
            {"212","Administrative surcharges are not covered"},
            {"213","Non-compliance with the physician self referral prohibition legislation or payer policy."},
            {"215","Based on subrogation of a third party settlement"},
            {"216","Based on the findings of a review organization"},
            {"219","Based on extent of injury. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') for the jurisdictional regulation. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF)."},
            {"222","Exceeds the contracted maximum number of hours/days/units by this provider for this period. This is not patient specific. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"223","Adjustment code for mandated federal, state or local law/regulation that is not already covered by another code and is mandated before a new code can be created."},
            {"224","Patient identification compromised by identity theft. Identity verification required for processing this and future claims."},
            {"225","Penalty or Interest Payment by Payer (Only used for plan to plan encounter reporting within the 837)"},
            {"226","Information requested from the Billing/Rendering Provider was not provided or not provided timely or was insufficient/incomplete. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"227","Information requested from the patient/insured/responsible party was not provided or was insufficient/incomplete. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"228","Denied for failure of this provider, another provider or the subscriber to supply requested information to a previous payer for their adjudication"},
            {"229","Partial charge amount not considered by Medicare due to the initial claim Type of Bill being 12X. Usage: This code can only be used in the 837 transaction to convey Coordination of Benefits information when the secondary payer's cost avoidance policy allows providers to bypass claim submission to a prior payer. (Use only with Group Code PR)"},
            {"231","Mutually exclusive procedures cannot be done in the same day/setting. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"232","Institutional Transfer Amount. Usage: Applies to institutional claims only and explains the DRG amount difference when the patient care crosses multiple institutions."},
            {"233","Services/charges related to the treatment of a hospital-acquired condition or preventable medical error."},
            {"234","This procedure is not paid separately. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"235","Sales Tax"},
            {"236","This procedure or procedure/modifier combination is not compatible with another procedure or procedure/modifier combination provided on the same day according to the National Correct Coding Initiative or workers compensation state regulations/ fee schedule requirements."},
            {"237","Legislated/Regulatory Penalty. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"238","Claim spans eligible and ineligible periods of coverage, this is the reduction for the ineligible period. (Use only with Group Code PR)"},
            {"239","Claim spans eligible and ineligible periods of coverage. Rebill separate claims."},
            {"240","The diagnosis is inconsistent with the patient's birth weight. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"241","Low Income Subsidy (LIS) Co-payment Amount"},
            {"242","Services not provided by network/primary care providers."},
            {"243","Services not authorized by network/primary care providers."},
            {"245","Provider performance program withhold."},
            {"246","This non-payable code is for required reporting only."},
            {"247","Deductible for Professional service rendered in an Institutional setting and billed on an Institutional claim."},
            {"248","Coinsurance for Professional service rendered in an Institutional setting and billed on an Institutional claim."},
            {"249","This claim has been identified as a readmission. (Use only with Group Code CO)"},
            {"250","The attachment/other documentation that was received was the incorrect attachment/document. The expected attachment/document is still missing. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT)."},
            {"251","The attachment/other documentation that was received was incomplete or deficient. The necessary information is still needed to process the claim. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT)."},
            {"252","An attachment/other documentation is required to adjudicate this claim/service. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT)."},
            {"253","Sequestration - reduction in federal payment"},
            {"254","Claim received by the dental plan, but benefits not available under this plan. Submit these services to the patient's medical plan for further consideration."},
            {"256","Service not payable per managed care contract."},
            {"257","The disposition of the claim/service is undetermined during the premium payment grace period, per Health Insurance Exchange requirements. This claim/service will be reversed and corrected when the grace period ends (due to premium payment or lack of premium payment). (Use only with Group Code OA)"},
            {"258","Claim/service not covered when patient is in custody/incarcerated. Applicable federal, state or local authority may cover the claim/service."},
            {"259","Additional payment for Dental/Vision service utilization."},
            {"260","Processed under Medicaid ACA Enhanced Fee Schedule"},
            {"261","The procedure or service is inconsistent with the patient's history."},
            {"262","Adjustment for delivery cost. Usage: To be used for pharmaceuticals only."},
            {"263","Adjustment for shipping cost. Usage: To be used for pharmaceuticals only."},
            {"264","Adjustment for postage cost. Usage: To be used for pharmaceuticals only."},
            {"265","Adjustment for administrative cost. Usage: To be used for pharmaceuticals only."},
            {"266","Adjustment for compound preparation cost. Usage: To be used for pharmaceuticals only."},
            {"267","Claim/service spans multiple months. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"268","The Claim spans two calendar years. Please resubmit one claim per calendar year."},
            {"269","Anesthesia not covered for this service/procedure. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"270","Claim received by the medical plan, but benefits not available under this plan. Submit these services to the patient's dental plan for further consideration."},
            {"271","Prior contractual reductions related to a current periodic payment as part of a contractual payment schedule when deferred amounts have been previously reported. (Use only with Group Code OA)"},
            {"272","Coverage/program guidelines were not met."},
            {"273","Coverage/program guidelines were exceeded."},
            {"274","Fee/Service not payable per patient Care Coordination arrangement."},
            {"275","Prior payer's (or payers') patient responsibility (deductible, coinsurance, co-payment) not covered. (Use only with Group Code PR)"},
            {"276","Services denied by the prior payer(s) are not covered by this payer."},
            {"277","The disposition of the claim/service is undetermined during the premium payment grace period, per Health Insurance SHOP Exchange requirements. This claim/service will be reversed and corrected when the grace period ends (due to premium payment or lack of premium payment). (Use only with Group Code OA)"},
            {"278","Performance program proficiency requirements not met. (Use only with Group Codes CO or PI) Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"279","Services not provided by Preferred network providers. Usage: Use this code when there are member network limitations. For example, using contracted providers not in the member's 'narrow' network."},
            {"280","Claim received by the medical plan, but benefits not available under this plan. Submit these services to the patient's Pharmacy plan for further consideration."},
            {"281","Deductible waived per contractual agreement. Use only with Group Code CO."},
            {"282","The procedure/revenue code is inconsistent with the type of bill. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"283","Attending provider is not eligible to provide direction of care."},
            {"284","Precertification/authorization/notification/pre-treatment number may be valid but does not apply to the billed services."},
            {"285","Appeal procedures not followed"},
            {"286","Appeal time limits not met"},
            {"287","Referral exceeded"},
            {"288","Referral absent"},
            {"289","Services considered under the dental and medical plans, benefits not available."},
            {"290","Claim received by the dental plan, but benefits not available under this plan. Claim has been forwarded to the patient's medical plan for further consideration."},
            {"291","Claim received by the medical plan, but benefits not available under this plan. Claim has been forwarded to the patient's dental plan for further consideration."},
            {"292","Claim received by the medical plan, but benefits not available under this plan. Claim has been forwarded to the patient's pharmacy plan for further consideration."},
            {"293","Payment made to employer."},
            {"294","Payment made to attorney."},
            {"295","Pharmacy Direct/Indirect Remuneration (DIR)"},
            {"296","Precertification/authorization/notification/pre-treatment number may be valid but does not apply to the provider."},
            {"297","Claim received by the medical plan, but benefits not available under this plan. Submit these services to the patient's vision plan for further consideration."},
            {"298","Claim received by the medical plan, but benefits not available under this plan. Claim has been forwarded to the patient's vision plan for further consideration."},
            {"299","The billing provider is not eligible to receive payment for the service billed."},
            {"300","Claim received by the Medical Plan, but benefits not available under this plan. Claim has been forwarded to the patient's Behavioral Health Plan for further consideration."},
            {"301","Claim received by the Medical Plan, but benefits not available under this plan. Submit these services to the patient's Behavioral Health Plan for further consideration."},
            {"302","Precertification/notification/authorization/pre-treatment time limit has expired."},
            {"303","Prior payer's (or payers') patient responsibility (deductible, coinsurance, co-payment) not covered for Qualified Medicare and Medicaid Beneficiaries. (Use only with Group Code CO)"},
            {"304","Claim received by the medical plan, but benefits not available under this plan. Submit these services to the patient's hearing plan for further consideration."},
            {"305","Claim received by the medical plan, but benefits not available under this plan. Claim has been forwarded to the patient's hearing plan for further consideration."},
            {"A0","Patient refund amount."},
            {"A1","Claim/Service denied. At least one Remark Code must be provided (may be comprised of either the NCPDP Reject Reason Code, or Remittance Advice Remark Code that is not an ALERT.)"},
            {"A5","Medicare Claim PPS Capital Cost Outlier Amount."},
            {"A6","Prior hospitalization or 30 day transfer requirement not met."},
            {"A8","Ungroupable DRG."},
            {"B1","Non-covered visits."},
            {"B10","Allowed amount has been reduced because a component of the basic procedure/test was paid. The beneficiary is not liable for more than the charge limit for the basic procedure/test."},
            {"B11","The claim/service has been transferred to the proper payer/processor for processing. Claim/service not covered by this payer/processor."},
            {"B12","Services not documented in patient's medical records."},
            {"B13","Previously paid. Payment for this claim/service may have been provided in a previous payment."},
            {"B14","Only one visit or consultation per physician per day is covered."},
            {"B15","This service/procedure requires that a qualifying service/procedure be received and covered. The qualifying other service/procedure has not been received/adjudicated. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"B16","'New Patient' qualifications were not met."},
            {"B20","Procedure/service was partially or fully furnished by another provider."},
            {"B22","This payment is adjusted based on the diagnosis."},
            {"B23","Procedure billed is not authorized per your Clinical Laboratory Improvement Amendment (CLIA) proficiency test."},
            {"B4","Late filing penalty."},
            {"B7","This provider was not certified/eligible to be paid for this procedure/service on this date of service. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"B8","Alternative services were available, and should have been utilized. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present."},
            {"B9","Patient is enrolled in a Hospice."},
            {"P1","State-mandated Requirement for Property and Casualty, see Claim Payment Remarks Code for specific explanation. To be used for Property and Casualty only."},
            {"P10","Payment reduced to zero due to litigation. Additional information will be sent following the conclusion of litigation. To be used for Property and Casualty only."},
            {"P11","The disposition of the related Property & Casualty claim (injury or illness) is pending due to litigation. To be used for Property and Casualty only. (Use only with Group Code OA)"},
            {"P12","Workers' compensation jurisdictional fee schedule adjustment. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Class of Contract Code Identification Segment (Loop 2100 Other Claim Related Information REF). If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Workers' Compensation only."},
            {"P13","Payment reduced or denied based on workers' compensation jurisdictional regulations or payment policies, use only if no other code is applicable. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') if the jurisdictional regulation applies. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Workers' Compensation only."},
            {"P14","The Benefit for this Service is included in the payment/allowance for another service/procedure that has been performed on the same day. Usage: Refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment Information REF), if present. To be used for Property and Casualty only."},
            {"P15","Workers' Compensation Medical Treatment Guideline Adjustment. To be used for Workers' Compensation only."},
            {"P16","Medical provider not authorized/certified to provide treatment to injured workers in this jurisdiction. To be used for Workers' Compensation only. (Use with Group Code CO or OA)"},
            {"P17","Referral not authorized by attending physician per regulatory requirement. To be used for Property and Casualty only."},
            {"P18","Procedure is not listed in the jurisdiction fee schedule. An allowance has been made for a comparable service. To be used for Property and Casualty only."},
            {"P19","Procedure has a relative value of zero in the jurisdiction fee schedule, therefore no payment is due. To be used for Property and Casualty only."},
            {"P2","Not a work related injury/illness and thus not the liability of the workers' compensation carrier Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') for the jurisdictional regulation. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF). To be used for Workers' Compensation only."},
            {"P20","Service not paid under jurisdiction allowed outpatient facility fee schedule. To be used for Property and Casualty only."},
            {"P21","Payment denied based on the Medical Payments Coverage (MPC) and/or Personal Injury Protection (PIP) Benefits jurisdictional regulations, or payment policies. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') if the jurisdictional regulation applies. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty Auto only."},
            {"P22","Payment adjusted based on the Medical Payments Coverage (MPC) and/or Personal Injury Protection (PIP) Benefits jurisdictional regulations, or payment policies. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') if the jurisdictional regulation applies. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty Auto only."},
            {"P23","Medical Payments Coverage (MPC) or Personal Injury Protection (PIP) Benefits jurisdictional fee schedule adjustment. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Class of Contract Code Identification Segment (Loop 2100 Other Claim Related Information REF). If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty Auto only."},
            {"P24","Payment adjusted based on Preferred Provider Organization (PPO). Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Class of Contract Code Identification Segment (Loop 2100 Other Claim Related Information REF). If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty only. Use only with Group Code CO."},
            {"P25","Payment adjusted based on Medical Provider Network (MPN). Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Class of Contract Code Identification Segment (Loop 2100 Other Claim Related Information REF). If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty only. (Use only with Group Code CO)."},
            {"P26","Payment adjusted based on Voluntary Provider network (VPN). Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Class of Contract Code Identification Segment (Loop 2100 Other Claim Related Information REF). If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty only. (Use only with Group Code CO)."},
            {"P27","Payment denied based on the Liability Coverage Benefits jurisdictional regulations and/or payment policies. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') if the jurisdictional regulation applies. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty Auto only."},
            {"P28","Payment adjusted based on the Liability Coverage Benefits jurisdictional regulations and/or payment policies. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') if the jurisdictional regulation applies. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty Auto only."},
            {"P29","Liability Benefits jurisdictional fee schedule adjustment. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Class of Contract Code Identification Segment (Loop 2100 Other Claim Related Information REF). If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF) if the regulations apply. To be used for Property and Casualty Auto only."},
            {"P3","Workers' Compensation case settled. Patient is responsible for amount of this claim/service through WC 'Medicare set aside arrangement' or other agreement. To be used for Workers' Compensation only. (Use only with Group Code PR)"},
            {"P30","Payment denied for exacerbation when supporting documentation was not complete. To be used for Property and Casualty only."},
            {"P31","Payment denied for exacerbation when treatment exceeds time allowed. To be used for Property and Casualty only."},
            {"P32","Payment adjusted due to Apportionment."},
            {"P4","Workers' Compensation claim adjudicated as non-compensable. This Payer not liable for claim or service/treatment. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') for the jurisdictional regulation. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF). To be used for Workers' Compensation only"},
            {"P5","Based on payer reasonable and customary fees. No maximum allowable defined by legislated fee arrangement. To be used for Property and Casualty only."},
            {"P6","Based on entitlement to benefits. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') for the jurisdictional regulation. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF). To be used for Property and Casualty only."},
            {"P7","The applicable fee schedule/fee database does not contain the billed code. Please resubmit a bill with the appropriate fee schedule/fee database code(s) that best describe the service(s) provided and supporting documentation if required. To be used for Property and Casualty only."},
            {"P8","Claim is under investigation. Usage: If adjustment is at the Claim Level, the payer must send and the provider should refer to the 835 Insurance Policy Number Segment (Loop 2100 Other Claim Related Information REF qualifier 'IG') for the jurisdictional regulation. If adjustment is at the Line Level, the payer must send and the provider should refer to the 835 Healthcare Policy Identification Segment (loop 2110 Service Payment information REF). To be used for Property and Casualty only."},
            {"P9","No available or correlating CPT/HCPCS code to describe this service. To be used for Property and Casualty only."}
        };

        public Dictionary<string, string> claimStatusCodes = new Dictionary<string, string>()
        {
            {"0","Cannot provide further status electronically."},
            {"1","For more detailed information, see remittance advice."},
            {"2","More detailed information in letter."},
            {"3","Claim has been adjudicated and is awaiting payment cycle."},
            {"6","Balance due from the subscriber."},
            {"12","One or more originally submitted procedure codes have been combined."},
            {"15","One or more originally submitted procedure code have been modified."},
            {"16","Claim/encounter has been forwarded to entity. Usage: This code requires use of an Entity Code."},
            {"17","Claim/encounter has been forwarded by third party entity to entity. Usage: This code requires use of an Entity Code."},
            {"18","Entity received claim/encounter, but returned invalid status. Usage: This code requires use of an Entity Code."},
            {"19","Entity acknowledges receipt of claim/encounter. Usage: This code requires use of an Entity Code."},
            {"20","Accepted for processing."},
            {"21","Missing or invalid information. Usage: At least one other status code is required to identify the missing or invalid information."},
            {"23","Returned to Entity. Usage: This code requires use of an Entity Code."},
            {"24","Entity not approved as an electronic submitter. Usage: This code requires use of an Entity Code."},
            {"25","Entity not approved. Usage: This code requires use of an Entity Code."},
            {"26","Entity not found. Usage: This code requires use of an Entity Code."},
            {"27","Policy canceled."},
            {"29","Subscriber and policy number/contract number mismatched."},
            {"30","Subscriber and subscriber id mismatched."},
            {"31","Subscriber and policyholder name mismatched."},
            {"32","Subscriber and policy number/contract number not found."},
            {"33","Subscriber and subscriber id not found."},
            {"34","Subscriber and policyholder name not found."},
            {"35","Claim/encounter not found."},
            {"37","Predetermination is on file, awaiting completion of services."},
            {"38","Awaiting next periodic adjudication cycle."},
            {"39","Charges for pregnancy deferred until delivery."},
            {"40","Waiting for final approval."},
            {"41","Special handling required at payer site."},
            {"42","Awaiting related charges."},
            {"44","Charges pending provider audit."},
            {"45","Awaiting benefit determination."},
            {"46","Internal review/audit."},
            {"47","Internal review/audit - partial payment made."},
            {"49","Pending provider accreditation review."},
            {"50","Claim waiting for internal provider verification."},
            {"51","Investigating occupational illness/accident."},
            {"52","Investigating existence of other insurance coverage."},
            {"53","Claim being researched for Insured ID/Group Policy Number error."},
            {"54","Duplicate of a previously processed claim/line."},
            {"55","Claim assigned to an approver/analyst."},
            {"56","Awaiting eligibility determination."},
            {"57","Pending COBRA information requested."},
            {"59","Information was requested by a non-electronic method. Usage: At least one other status code is required to identify the requested information."},
            {"60","Information was requested by an electronic method. Usage: At least one other status code is required to identify the requested information."},
            {"61","Eligibility for extended benefits."},
            {"64","Re-pricing information."},
            {"65","Claim/line has been paid."},
            {"66","Payment reflects usual and customary charges."},
            {"72","Claim contains split payment."},
            {"73","Payment made to entity, assignment of benefits not on file. Usage: This code requires use of an Entity Code."},
            {"78","Duplicate of an existing claim/line, awaiting processing."},
            {"81","Contract/plan does not cover pre-existing conditions."},
            {"83","No coverage for newborns."},
            {"84","Service not authorized."},
            {"85","Entity not primary. Usage: This code requires use of an Entity Code."},
            {"86","Diagnosis and patient gender mismatch."},
            {"88","Entity not eligible for benefits for submitted dates of service. Usage: This code requires use of an Entity Code."},
            {"89","Entity not eligible for dental benefits for submitted dates of service. Usage: This code requires use of an Entity Code."},
            {"90","Entity not eligible for medical benefits for submitted dates of service. Usage: This code requires use of an Entity Code."},
            {"91","Entity not eligible/not approved for dates of service. Usage: This code requires use of an Entity Code."},
            {"92","Entity does not meet dependent or student qualification. Usage: This code requires use of an Entity Code."},
            {"93","Entity is not selected primary care provider. Usage: This code requires use of an Entity Code."},
            {"94","Entity not referred by selected primary care provider. Usage: This code requires use of an Entity Code."},
            {"95","Requested additional information not received."},
            {"96","No agreement with entity. Usage: This code requires use of an Entity Code."},
            {"97","Patient eligibility not found with entity. Usage: This code requires use of an Entity Code."},
            {"98","Charges applied to deductible."},
            {"99","Pre-treatment review."},
            {"100","Pre-certification penalty taken."},
            {"101","Claim was processed as adjustment to previous claim."},
            {"102","Newborn's charges processed on mother's claim."},
            {"103","Claim combined with other claim(s)."},
            {"104","Processed according to plan provisions (Plan refers to provisions that exist between the Health Plan and the Consumer or Patient)"},
            {"105","Claim/line is capitated."},
            {"106","This amount is not entity's responsibility. Usage: This code requires use of an Entity Code."},
            {"107","Processed according to contract provisions (Contract refers to provisions that exist between the Health Plan and a Provider of Health Care Services)"},
            {"109","Entity not eligible. Usage: This code requires use of an Entity Code."},
            {"110","Claim requires pricing information."},
            {"111","At the policyholder's request these claims cannot be submitted electronically."},
            {"114","Claim/service should be processed by entity. Usage: This code requires use of an Entity Code."},
            {"116","Claim submitted to incorrect payer."},
            {"117","Claim requires signature-on-file indicator."},
            {"121","Service line number greater than maximum allowable for payer."},
            {"123","Additional information requested from entity. Usage: This code requires use of an Entity Code."},
            {"124","Entity's name, address, phone and id number. Usage: This code requires use of an Entity Code."},
            {"125","Entity's name. Usage: This code requires use of an Entity Code."},
            {"126","Entity's address. Usage: This code requires use of an Entity Code."},
            {"127","Entity's Communication Number. Usage: This code requires use of an Entity Code."},
            {"128","Entity's tax id. Usage: This code requires use of an Entity Code."},
            {"129","Entity's Blue Cross provider id. Usage: This code requires use of an Entity Code."},
            {"130","Entity's Blue Shield provider id. Usage: This code requires use of an Entity Code."},
            {"131","Entity's Medicare provider id. Usage: This code requires use of an Entity Code."},
            {"132","Entity's Medicaid provider id. Usage: This code requires use of an Entity Code."},
            {"133","Entity's UPIN. Usage: This code requires use of an Entity Code."},
            {"134","Entity's TRICARE provider id. Usage: This code requires use of an Entity Code."},
            {"135","Entity's commercial provider id. Usage: This code requires use of an Entity Code."},
            {"136","Entity's health industry id number. Usage: This code requires use of an Entity Code."},
            {"137","Entity's plan network id. Usage: This code requires use of an Entity Code."},
            {"138","Entity's site id . Usage: This code requires use of an Entity Code."},
            {"139","Entity's health maintenance provider id (HMO). Usage: This code requires use of an Entity Code."},
            {"140","Entity's preferred provider organization id (PPO). Usage: This code requires use of an Entity Code."},
            {"141","Entity's administrative services organization id (ASO). Usage: This code requires use of an Entity Code."},
            {"142","Entity's license/certification number. Usage: This code requires use of an Entity Code."},
            {"143","Entity's state license number. Usage: This code requires use of an Entity Code."},
            {"144","Entity's specialty license number. Usage: This code requires use of an Entity Code."},
            {"145","Entity's specialty/taxonomy code. Usage: This code requires use of an Entity Code."},
            {"146","Entity's anesthesia license number. Usage: This code requires use of an Entity Code."},
            {"147","Entity's qualification degree/designation (e.g. RN,PhD,MD). Usage: This code requires use of an Entity Code."},
            {"148","Entity's social security number. Usage: This code requires use of an Entity Code."},
            {"149","Entity's employer id. Usage: This code requires use of an Entity Code."},
            {"150","Entity's drug enforcement agency (DEA) number. Usage: This code requires use of an Entity Code."},
            {"152","Pharmacy processor number."},
            {"153","Entity's id number. Usage: This code requires use of an Entity Code."},
            {"154","Relationship of surgeon & assistant surgeon."},
            {"155","Entity's relationship to patient. Usage: This code requires use of an Entity Code."},
            {"156","Patient relationship to subscriber"},
            {"157","Entity's Gender. Usage: This code requires use of an Entity Code."},
            {"158","Entity's date of birth. Usage: This code requires use of an Entity Code."},
            {"159","Entity's date of death. Usage: This code requires use of an Entity Code."},
            {"160","Entity's marital status. Usage: This code requires use of an Entity Code."},
            {"161","Entity's employment status. Usage: This code requires use of an Entity Code."},
            {"162","Entity's health insurance claim number (HICN). Usage: This code requires use of an Entity Code."},
            {"163","Entity's policy/group number. Usage: This code requires use of an Entity Code."},
            {"164","Entity's contract/member number. Usage: This code requires use of an Entity Code."},
            {"165","Entity's employer name, address and phone. Usage: This code requires use of an Entity Code."},
            {"166","Entity's employer name. Usage: This code requires use of an Entity Code."},
            {"167","Entity's employer address. Usage: This code requires use of an Entity Code."},
            {"168","Entity's employer phone number. Usage: This code requires use of an Entity Code."},
            {"170","Entity's employee id. Usage: This code requires use of an Entity Code."},
            {"171","Other insurance coverage information (health, liability, auto, etc.)."},
            {"172","Other employer name, address and telephone number."},
            {"173","Entity's name, address, phone, gender, DOB, marital status, employment status and relation to subscriber. Usage: This code requires use of an Entity Code."},
            {"174","Entity's student status. Usage: This code requires use of an Entity Code."},
            {"175","Entity's school name. Usage: This code requires use of an Entity Code."},
            {"176","Entity's school address. Usage: This code requires use of an Entity Code."},
            {"177","Transplant recipient's name, date of birth, gender, relationship to insured."},
            {"178","Submitted charges."},
            {"179","Outside lab charges."},
            {"180","Hospital s semi-private room rate."},
            {"181","Hospital s room rate."},
            {"182","Allowable/paid from other entities coverage Usage: This code requires the use of an entity code."},
            {"183","Amount entity has paid. Usage: This code requires use of an Entity Code."},
            {"184","Purchase price for the rented durable medical equipment."},
            {"185","Rental price for durable medical equipment."},
            {"186","Purchase and rental price of durable medical equipment."},
            {"187","Date(s) of service."},
            {"188","Statement from-through dates."},
            {"189","Facility admission date"},
            {"190","Facility discharge date"},
            {"191","Date of Last Menstrual Period (LMP)"},
            {"192","Date of first service for current series/symptom/illness."},
            {"193","First consultation/evaluation date."},
            {"194","Confinement dates."},
            {"195","Unable to work dates/Disability Dates."},
            {"196","Return to work dates."},
            {"197","Effective coverage date(s)."},
            {"198","Medicare effective date."},
            {"199","Date of conception and expected date of delivery."},
            {"200","Date of equipment return."},
            {"201","Date of dental appliance prior placement."},
            {"202","Date of dental prior replacement/reason for replacement."},
            {"203","Date of dental appliance placed."},
            {"204","Date dental canal(s) opened and date service completed."},
            {"205","Date(s) dental root canal therapy previously performed."},
            {"206","Most recent date of curettage, root planing, or periodontal surgery."},
            {"207","Dental impression and seating date."},
            {"208","Most recent date pacemaker was implanted."},
            {"209","Most recent pacemaker battery change date."},
            {"210","Date of the last x-ray."},
            {"211","Date(s) of dialysis training provided to patient."},
            {"212","Date of last routine dialysis."},
            {"213","Date of first routine dialysis."},
            {"214","Original date of prescription/orders/referral."},
            {"215","Date of tooth extraction/evolution."},
            {"216","Drug information."},
            {"217","Drug name, strength and dosage form."},
            {"218","NDC number."},
            {"219","Prescription number."},
            {"222","Drug dispensing units and average wholesale price (AWP)."},
            {"223","Route of drug/myelogram administration."},
            {"224","Anatomical location for joint injection."},
            {"225","Anatomical location."},
            {"226","Joint injection site."},
            {"227","Hospital information."},
            {"228","Type of bill for UB claim"},
            {"229","Hospital admission source."},
            {"230","Hospital admission hour."},
            {"231","Hospital admission type."},
            {"232","Admitting diagnosis."},
            {"233","Hospital discharge hour."},
            {"234","Patient discharge status."},
            {"235","Units of blood furnished."},
            {"236","Units of blood replaced."},
            {"237","Units of deductible blood."},
            {"238","Separate claim for mother/baby charges."},
            {"239","Dental information."},
            {"240","Tooth surface(s) involved."},
            {"241","List of all missing teeth (upper and lower)."},
            {"242","Tooth numbers, surfaces, and/or quadrants involved."},
            {"243","Months of dental treatment remaining."},
            {"244","Tooth number or letter."},
            {"245","Dental quadrant/arch."},
            {"246","Total orthodontic service fee, initial appliance fee, monthly fee, length of service."},
            {"247","Line information."},
            {"249","Place of service."},
            {"250","Type of service."},
            {"251","Total anesthesia minutes."},
            {"252","Entity's prior authorization/certification number. Usage: This code requires the use of an Entity Code."},
            {"254","Principal diagnosis code."},
            {"255","Diagnosis code."},
            {"256","DRG code(s)."},
            {"257","ADSM-III-R code for services rendered."},
            {"258","Days/units for procedure/revenue code."},
            {"259","Frequency of service."},
            {"260","Length of medical necessity, including begin date."},
            {"261","Obesity measurements."},
            {"262","Type of surgery/service for which anesthesia was administered."},
            {"263","Length of time for services rendered."},
            {"264","Number of liters/minute & total hours/day for respiratory support."},
            {"265","Number of lesions excised."},
            {"266","Facility point of origin and destination - ambulance."},
            {"267","Number of miles patient was transported."},
            {"268","Location of durable medical equipment use."},
            {"269","Length/size of laceration/tumor."},
            {"270","Subluxation location."},
            {"271","Number of spine segments."},
            {"272","Oxygen contents for oxygen system rental."},
            {"273","Weight."},
            {"274","Height."},
            {"275","Claim."},
            {"276","UB04/HCFA-1450/1500 claim form"},
            {"277","Paper claim."},
            {"279","Claim/service must be itemized"},
            {"281","Related confinement claim."},
            {"282","Copy of prescription."},
            {"283","Medicare entitlement information is required to determine primary coverage"},
            {"284","Copy of Medicare ID card."},
            {"286","Other payer's Explanation of Benefits/payment information."},
            {"287","Medical necessity for service."},
            {"288","Hospital late charges"},
            {"290","Pre-existing information."},
            {"291","Reason for termination of pregnancy."},
            {"292","Purpose of family conference/therapy."},
            {"293","Reason for physical therapy."},
            {"294","Supporting documentation. Usage: At least one other status code is required to identify the supporting documentation."},
            {"295","Attending physician report."},
            {"296","Nurse's notes."},
            {"297","Medical notes/report."},
            {"298","Operative report."},
            {"299","Emergency room notes/report."},
            {"300","Lab/test report/notes/results."},
            {"301","MRI report."},
            {"305","Radiology/x-ray reports and/or interpretation"},
            {"306","Detailed description of service."},
            {"307","Narrative with pocket depth chart."},
            {"308","Discharge summary."},
            {"310","Progress notes for the six months prior to statement date."},
            {"311","Pathology notes/report."},
            {"312","Dental charting."},
            {"313","Bridgework information."},
            {"314","Dental records for this service."},
            {"315","Past perio treatment history."},
            {"316","Complete medical history."},
            {"318","X-rays/radiology films"},
            {"319","Pre/post-operative x-rays/photographs."},
            {"320","Study models."},
            {"322","Recent Full Mouth X-rays"},
            {"323","Study models, x-rays, and/or narrative."},
            {"324","Recent x-ray of treatment area and/or narrative."},
            {"325","Recent fm x-rays and/or narrative."},
            {"326","Copy of transplant acquisition invoice."},
            {"327","Periodontal case type diagnosis and recent pocket depth chart with narrative."},
            {"329","Exercise notes."},
            {"330","Occupational notes."},
            {"331","History and physical."},
            {"333","Patient release of information authorization."},
            {"334","Oxygen certification."},
            {"335","Durable medical equipment certification."},
            {"336","Chiropractic certification."},
            {"337","Ambulance certification/documentation."},
            {"339","Enteral/parenteral certification."},
            {"340","Pacemaker certification."},
            {"341","Private duty nursing certification."},
            {"342","Podiatric certification."},
            {"343","Documentation that facility is state licensed and Medicare approved as a surgical facility."},
            {"344","Documentation that provider of physical therapy is Medicare Part B approved."},
            {"345","Treatment plan for service/diagnosis"},
            {"346","Proposed treatment plan for next 6 months."},
            {"352","Duration of treatment plan."},
            {"353","Orthodontics treatment plan."},
            {"354","Treatment plan for replacement of remaining missing teeth."},
            {"360","Benefits Assignment Certification Indicator"},
            {"363","Possible Workers' Compensation"},
            {"364","Is accident/illness/condition employment related?"},
            {"365","Is service the result of an accident?"},
            {"366","Is injury due to auto accident?"},
            {"374","Is prescribed lenses a result of cataract surgery?"},
            {"375","Was refraction performed?"},
            {"380","CRNA supervision/medical direction."},
            {"382","Did provider authorize generic or brand name dispensing?"},
            {"383","Nerve block use (surgery vs. pain management)"},
            {"384","Is prosthesis/crown/inlay placement an initial placement or a replacement?"},
            {"385","Is appliance upper or lower arch & is appliance fixed or removable?"},
            {"386","Orthodontic Treatment/Purpose Indicator"},
            {"387","Date patient last examined by entity. Usage: This code requires use of an Entity Code."},
            {"388","Date post-operative care assumed"},
            {"389","Date post-operative care relinquished"},
            {"390","Date of most recent medical event necessitating service(s)"},
            {"391","Date(s) dialysis conducted"},
            {"394","Date(s) of most recent hospitalization related to service"},
            {"395","Date entity signed certification/recertification Usage: This code requires use of an Entity Code."},
            {"396","Date home dialysis began"},
            {"397","Date of onset/exacerbation of illness/condition"},
            {"398","Visual field test results"},
            {"400","Claim is out of balance"},
            {"401","Source of payment is not valid"},
            {"402","Amount must be greater than zero. Usage: At least one other status code is required to identify which amount element is in error."},
            {"403","Entity referral notes/orders/prescription. Effective 05/01/2018: Entity referral notes/orders/prescription. Usage: this code requires use of an entity code."},
            {"406","Brief medical history as related to service(s)"},
            {"407","Complications/mitigating circumstances"},
            {"408","Initial certification"},
            {"409","Medication logs/records (including medication therapy)"},
            {"414","Necessity for concurrent care (more than one physician treating the patient)"},
            {"417","Prior testing, including result(s) and date(s) as related to service(s)"},
            {"419","Individual test(s) comprising the panel and the charges for each test"},
            {"420","Name, dosage and medical justification of contrast material used for radiology procedure"},
            {"428","Reason for transport by ambulance"},
            {"430","Nearest appropriate facility"},
            {"431","Patient's condition/functional status at time of service."},
            {"432","Date benefits exhausted"},
            {"433","Copy of patient revocation of hospice benefits"},
            {"434","Reasons for more than one transfer per entitlement period"},
            {"435","Notice of Admission"},
            {"441","Entity professional qualification for service(s)"},
            {"442","Modalities of service"},
            {"443","Initial evaluation report"},
            {"449","Projected date to discontinue service(s)"},
            {"450","Awaiting spend down determination"},
            {"451","Preoperative and post-operative diagnosis"},
            {"452","Total visits in total number of hours/day and total number of hours/week"},
            {"453","Procedure Code Modifier(s) for Service(s) Rendered"},
            {"454","Procedure code for services rendered."},
            {"455","Revenue code for services rendered."},
            {"456","Covered Day(s)"},
            {"457","Non-Covered Day(s)"},
            {"458","Coinsurance Day(s)"},
            {"459","Lifetime Reserve Day(s)"},
            {"460","NUBC Condition Code(s)"},
            {"464","Payer Assigned Claim Control Number"},
            {"465","Principal Procedure Code for Service(s) Rendered"},
            {"466","Entity's Original Signature. Usage: This code requires use of an Entity Code."},
            {"467","Entity Signature Date. Usage: This code requires use of an Entity Code."},
            {"468","Patient Signature Source"},
            {"469","Purchase Service Charge"},
            {"470","Was service purchased from another entity? Usage: This code requires use of an Entity Code."},
            {"471","Were services related to an emergency?"},
            {"472","Ambulance Run Sheet"},
            {"473","Missing or invalid lab indicator"},
            {"474","Procedure code and patient gender mismatch"},
            {"475","Procedure code not valid for patient age"},
            {"476","Missing or invalid units of service"},
            {"477","Diagnosis code pointer is missing or invalid"},
            {"478","Claim submitter's identifier"},
            {"479","Other Carrier payer ID is missing or invalid"},
            {"480","Entity's claim filing indicator. Usage: This code requires use of an Entity Code."},
            {"481","Claim/submission format is invalid."},
            {"483","Maximum coverage amount met or exceeded for benefit period."},
            {"484","Business Application Currently Not Available"},
            {"485","More information available than can be returned in real time mode. Narrow your current search criteria. This change effective September 1, 2017: More information available than can be returned in real-time mode. Narrow your current search criteria."},
            {"486","Principal Procedure Date"},
            {"487","Claim not found, claim should have been submitted to/through 'entity'. Usage: This code requires use of an Entity Code."},
            {"488","Diagnosis code(s) for the services rendered."},
            {"489","Attachment Control Number"},
            {"490","Other Procedure Code for Service(s) Rendered"},
            {"491","Entity not eligible for encounter submission. Usage: This code requires use of an Entity Code."},
            {"492","Other Procedure Date"},
            {"493","Version/Release/Industry ID code not currently supported by information holder"},
            {"494","Real-Time requests not supported by the information holder, resubmit as batch request This change effective September 1, 2017: Real-time requests not supported by the information holder, resubmit as batch request"},
            {"495","Requests for re-adjudication must reference the newly assigned payer claim control number for this previously adjusted claim. Correct the payer claim control number and re-submit."},
            {"496","Submitter not approved for electronic claim submissions on behalf of this entity. Usage: This code requires use of an Entity Code."},
            {"497","Sales tax not paid"},
            {"498","Maximum leave days exhausted"},
            {"499","No rate on file with the payer for this service for this entity Usage: This code requires use of an Entity Code."},
            {"500","Entity's Postal/Zip Code. Usage: This code requires use of an Entity Code."},
            {"501","Entity's State/Province. Usage: This code requires use of an Entity Code."},
            {"502","Entity's City. Usage: This code requires use of an Entity Code."},
            {"503","Entity's Street Address. Usage: This code requires use of an Entity Code."},
            {"504","Entity's Last Name. Usage: This code requires use of an Entity Code."},
            {"505","Entity's First Name. Usage: This code requires use of an Entity Code."},
            {"506","Entity is changing processor/clearinghouse. This claim must be submitted to the new processor/clearinghouse. Usage: This code requires use of an Entity Code."},
            {"507","HCPCS"},
            {"508","ICD9 Usage: At least one other status code is required to identify the related procedure code or diagnosis code."},
            {"509","External Cause of Injury Code."},
            {"510","Future date. Usage: At least one other status code is required to identify the data element in error."},
            {"511","Invalid character. Usage: At least one other status code is required to identify the data element in error."},
            {"512","Length invalid for receiver's application system. Usage: At least one other status code is required to identify the data element in error."},
            {"513","HIPPS Rate Code for services Rendered"},
            {"514","Entity's Middle Name Usage: This code requires use of an Entity Code."},
            {"515","Managed Care review"},
            {"516","Other Entity's Adjudication or Payment/Remittance Date. Usage: An Entity code is required to identify the Other Payer Entity, i.e. primary, secondary."},
            {"517","Adjusted Repriced Claim Reference Number"},
            {"518","Adjusted Repriced Line item Reference Number"},
            {"519","Adjustment Amount"},
            {"520","Adjustment Quantity"},
            {"521","Adjustment Reason Code"},
            {"522","Anesthesia Modifying Units"},
            {"523","Anesthesia Unit Count"},
            {"524","Arterial Blood Gas Quantity"},
            {"525","Begin Therapy Date"},
            {"526","Bundled or Unbundled Line Number"},
            {"527","Certification Condition Indicator"},
            {"528","Certification Period Projected Visit Count"},
            {"529","Certification Revision Date"},
            {"530","Claim Adjustment Indicator"},
            {"531","Claim Disproportinate Share Amount"},
            {"532","Claim DRG Amount"},
            {"533","Claim DRG Outlier Amount"},
            {"534","Claim ESRD Payment Amount"},
            {"535","Claim Frequency Code"},
            {"536","Claim Indirect Teaching Amount"},
            {"537","Claim MSP Pass-through Amount"},
            {"538","Claim or Encounter Identifier"},
            {"539","Claim PPS Capital Amount"},
            {"540","Claim PPS Capital Outlier Amount"},
            {"541","Claim Submission Reason Code"},
            {"542","Claim Total Denied Charge Amount"},
            {"543","Clearinghouse or Value Added Network Trace"},
            {"544","Clinical Laboratory Improvement Amendment (CLIA) Number"},
            {"545","Contract Amount"},
            {"546","Contract Code"},
            {"547","Contract Percentage"},
            {"548","Contract Type Code"},
            {"549","Contract Version Identifier"},
            {"550","Coordination of Benefits Code"},
            {"551","Coordination of Benefits Total Submitted Charge"},
            {"552","Cost Report Day Count"},
            {"553","Covered Amount"},
            {"554","Date Claim Paid"},
            {"555","Delay Reason Code"},
            {"556","Demonstration Project Identifier"},
            {"557","Diagnosis Date"},
            {"558","Discount Amount"},
            {"559","Document Control Identifier"},
            {"560","Entity's Additional/Secondary Identifier. Usage: This code requires use of an Entity Code."},
            {"561","Entity's Contact Name. Usage: This code requires use of an Entity Code."},
            {"562","Entity's National Provider Identifier (NPI). Usage: This code requires use of an Entity Code."},
            {"563","Entity's Tax Amount. Usage: This code requires use of an Entity Code."},
            {"564","EPSDT Indicator"},
            {"565","Estimated Claim Due Amount"},
            {"566","Exception Code"},
            {"567","Facility Code Qualifier"},
            {"568","Family Planning Indicator"},
            {"569","Fixed Format Information"},
            {"571","Frequency Count"},
            {"572","Frequency Period"},
            {"573","Functional Limitation Code"},
            {"574","HCPCS Payable Amount Home Health"},
            {"575","Homebound Indicator"},
            {"576","Immunization Batch Number"},
            {"577","Industry Code"},
            {"578","Insurance Type Code"},
            {"579","Investigational Device Exemption Identifier"},
            {"580","Last Certification Date"},
            {"581","Last Worked Date"},
            {"582","Lifetime Psychiatric Days Count"},
            {"583","Line Item Charge Amount"},
            {"584","Line Item Control Number"},
            {"585","Denied Charge or Non-covered Charge"},
            {"586","Line Note Text"},
            {"587","Measurement Reference Identification Code"},
            {"588","Medical Record Number"},
            {"589","Provider Accept Assignment Code"},
            {"590","Medicare Coverage Indicator"},
            {"591","Medicare Paid at 100% Amount"},
            {"592","Medicare Paid at 80% Amount"},
            {"593","Medicare Section 4081 Indicator"},
            {"594","Mental Status Code"},
            {"595","Monthly Treatment Count"},
            {"596","Non-covered Charge Amount"},
            {"597","Non-payable Professional Component Amount"},
            {"598","Non-payable Professional Component Billed Amount"},
            {"599","Note Reference Code"},
            {"600","Oxygen Saturation Qty"},
            {"601","Oxygen Test Condition Code"},
            {"602","Oxygen Test Date"},
            {"603","Old Capital Amount"},
            {"604","Originator Application Transaction Identifier"},
            {"605","Orthodontic Treatment Months Count"},
            {"606","Paid From Part A Medicare Trust Fund Amount"},
            {"607","Paid From Part B Medicare Trust Fund Amount"},
            {"608","Paid Service Unit Count"},
            {"609","Participation Agreement"},
            {"610","Patient Discharge Facility Type Code"},
            {"611","Peer Review Authorization Number"},
            {"612","Per Day Limit Amount"},
            {"613","Physician Contact Date"},
            {"614","Physician Order Date"},
            {"615","Policy Compliance Code"},
            {"616","Policy Name"},
            {"617","Postage Claimed Amount"},
            {"618","PPS-Capital DSH DRG Amount"},
            {"619","PPS-Capital Exception Amount"},
            {"620","PPS-Capital FSP DRG Amount"},
            {"621","PPS-Capital HSP DRG Amount"},
            {"622","PPS-Capital IME Amount"},
            {"623","PPS-Operating Federal Specific DRG Amount"},
            {"624","PPS-Operating Hospital Specific DRG Amount"},
            {"625","Predetermination of Benefits Identifier"},
            {"626","Pregnancy Indicator"},
            {"627","Pre-Tax Claim Amount"},
            {"628","Pricing Methodology"},
            {"629","Property Casualty Claim Number"},
            {"630","Referring CLIA Number"},
            {"631","Reimbursement Rate"},
            {"632","Reject Reason Code"},
            {"633","Related Causes Code (Accident, auto accident, employment)"},
            {"634","Remark Code"},
            {"635","Repriced Ambulatory Patient Group Code"},
            {"636","Repriced Line Item Reference Number"},
            {"637","Repriced Saving Amount"},
            {"638","Repricing Per Diem or Flat Rate Amount"},
            {"639","Responsibility Amount"},
            {"640","Sales Tax Amount"},
            {"642","Service Authorization Exception Code"},
            {"643","Service Line Paid Amount"},
            {"644","Service Line Rate"},
            {"645","Service Tax Amount"},
            {"646","Ship, Delivery or Calendar Pattern Code"},
            {"647","Shipped Date"},
            {"648","Similar Illness or Symptom Date"},
            {"649","Skilled Nursing Facility Indicator"},
            {"650","Special Program Indicator"},
            {"651","State Industrial Accident Provider Number"},
            {"652","Terms Discount Percentage"},
            {"653","Test Performed Date"},
            {"654","Total Denied Charge Amount"},
            {"655","Total Medicare Paid Amount"},
            {"656","Total Visits Projected This Certification Count"},
            {"657","Total Visits Rendered Count"},
            {"658","Treatment Code"},
            {"659","Unit or Basis for Measurement Code"},
            {"660","Universal Product Number"},
            {"661","Visits Prior to Recertification Date Count CR702"},
            {"662","X-ray Availability Indicator"},
            {"663","Entity's Group Name. Usage: This code requires use of an Entity Code."},
            {"664","Orthodontic Banding Date"},
            {"665","Surgery Date"},
            {"666","Surgical Procedure Code"},
            {"667","Real-Time requests not supported by the information holder, do not resubmit This change effective September 1, 2017: Real-time requests not supported by the information holder, do not resubmit"},
            {"668","Missing Endodontics treatment history and prognosis"},
            {"669","Dental service narrative needed."},
            {"670","Funds applied from a consumer spending account such as consumer directed/driven health plan (CDHP), Health savings account (H S A) and or other similar accounts"},
            {"671","Funds may be available from a consumer spending account such as consumer directed/driven health plan (CDHP), Health savings account (H S A) and or other similar accounts"},
            {"672","Other Payer's payment information is out of balance"},
            {"673","Patient Reason for Visit"},
            {"674","Authorization exceeded"},
            {"675","Facility admission through discharge dates"},
            {"676","Entity possibly compensated by facility. Usage: This code requires use of an Entity Code."},
            {"677","Entity not affiliated. Usage: This code requires use of an Entity Code."},
            {"678","Revenue code and patient gender mismatch"},
            {"679","Submit newborn services on mother's claim"},
            {"680","Entity's Country. Usage: This code requires use of an Entity Code."},
            {"681","Claim currency not supported"},
            {"682","Cosmetic procedure"},
            {"683","Awaiting Associated Hospital Claims"},
            {"684","Rejected. Syntax error noted for this claim/service/inquiry. See Functional or Implementation Acknowledgement for details. (Usage: Only for use to reject claims or status requests in transactions that were 'accepted with errors' on a 997 or 999 Acknowledgement.)"},
            {"685","Claim could not complete adjudication in real time. Claim will continue processing in a batch mode. Do not resubmit. This change effective September 1, 2017: Claim could not complete adjudication in real-time. Claim will continue processing in a batch mode. Do not resubmit."},
            {"686","The claim/ encounter has completed the adjudication cycle and the entire claim has been voided"},
            {"687","Claim estimation can not be completed in real time. Do not resubmit. This change effective September 1, 2017: Claim predetermination/estimation could not be completed in real-time. Do not resubmit."},
            {"688","Present on Admission Indicator for reported diagnosis code(s)."},
            {"689","Entity was unable to respond within the expected time frame. Usage: This code requires use of an Entity Code."},
            {"690","Multiple claims or estimate requests cannot be processed in real time. This change effective September 1, 2017: Multiple claims or estimate requests cannot be processed in real-time."},
            {"691","Multiple claim status requests cannot be processed in real time. This change effective September 1, 2017: Multiple claim status requests cannot be processed in real-time."},
            {"692","Contracted funding agreement-Subscriber is employed by the provider of services"},
            {"693","Amount must be greater than or equal to zero. Usage: At least one other status code is required to identify which amount element is in error."},
            {"694","Amount must not be equal to zero. Usage: At least one other status code is required to identify which amount element is in error."},
            {"695","Entity's Country Subdivision Code. Usage: This code requires use of an Entity Code."},
            {"696","Claim Adjustment Group Code."},
            {"697","Invalid Decimal Precision. Usage: At least one other status code is required to identify the data element in error."},
            {"698","Form Type Identification"},
            {"699","Question/Response from Supporting Documentation Form"},
            {"700","ICD10. Usage: At least one other status code is required to identify the related procedure code or diagnosis code."},
            {"701","Initial Treatment Date"},
            {"702","Repriced Claim Reference Number"},
            {"703","Advanced Billing Concepts (ABC) code"},
            {"704","Claim Note Text"},
            {"705","Repriced Allowed Amount"},
            {"706","Repriced Approved Amount"},
            {"707","Repriced Approved Ambulatory Patient Group Amount"},
            {"708","Repriced Approved Revenue Code"},
            {"709","Repriced Approved Service Unit Count"},
            {"710","Line Adjudication Information. Usage: At least one other status code is required to identify the data element in error."},
            {"711","Stretcher purpose"},
            {"712","Obstetric Additional Units"},
            {"713","Patient Condition Description"},
            {"714","Care Plan Oversight Number"},
            {"715","Acute Manifestation Date"},
            {"716","Repriced Approved DRG Code"},
            {"717","This claim has been split for processing."},
            {"718","Claim/service not submitted within the required timeframe (timely filing)."},
            {"719","NUBC Occurrence Code(s)"},
            {"720","NUBC Occurrence Code Date(s)"},
            {"721","NUBC Occurrence Span Code(s)"},
            {"722","NUBC Occurrence Span Code Date(s)"},
            {"723","Drug days supply"},
            {"724","Drug dosage. This change effective 5/01/2017: Drug Quantity"},
            {"725","NUBC Value Code(s)"},
            {"726","NUBC Value Code Amount(s)"},
            {"727","Accident date"},
            {"728","Accident state"},
            {"729","Accident description"},
            {"730","Accident cause"},
            {"731","Measurement value/test result"},
            {"732","Information submitted inconsistent with billing guidelines. Usage: At least one other status code is required to identify the inconsistent information."},
            {"733","Prefix for entity's contract/member number."},
            {"734","Verifying premium payment"},
            {"735","This service/claim is included in the allowance for another service or claim."},
            {"736","A related or qualifying service/claim has not been received/adjudicated."},
            {"737","Current Dental Terminology (CDT) Code"},
            {"738","Home Infusion EDI Coalition (HEIC) Product/Service Code"},
            {"739","Jurisdiction Specific Procedure or Supply Code"},
            {"740","Drop-Off Location"},
            {"741","Entity must be a person. Usage: This code requires use of an Entity Code."},
            {"742","Payer Responsibility Sequence Number Code"},
            {"743","Entity's credential/enrollment information. Usage: This code requires use of an Entity Code."},
            {"744","Services/charges related to the treatment of a hospital-acquired condition or preventable medical error."},
            {"745","Identifier Qualifier Usage: At least one other status code is required to identify the specific identifier qualifier in error."},
            {"746","Duplicate Submission Usage: use only at the information receiver level in the Health Care Claim Acknowledgement transaction."},
            {"747","Hospice Employee Indicator"},
            {"748","Corrected Data Usage: Requires a second status code to identify the corrected data."},
            {"749","Date of Injury/Illness"},
            {"750","Auto Accident State or Province Code"},
            {"751","Ambulance Pick-up State or Province Code"},
            {"752","Ambulance Drop-off State or Province Code"},
            {"753","Co-pay status code."},
            {"754","Entity Name Suffix. Usage: This code requires the use of an Entity Code."},
            {"755","Entity's primary identifier. Usage: This code requires the use of an Entity Code."},
            {"756","Entity's Received Date. Usage: This code requires the use of an Entity Code."},
            {"757","Last seen date."},
            {"758","Repriced approved HCPCS code."},
            {"759","Round trip purpose description."},
            {"760","Tooth status code."},
            {"761","Entity's referral number. Usage: This code requires the use of an Entity Code."},
            {"762","Locum Tenens Provider Identifier. Code must be used with Entity Code 82 - Rendering Provider"},
            {"763","Ambulance Pickup ZipCode"},
            {"764","Professional charges are non covered."},
            {"765","Institutional charges are non covered."},
            {"766","Services were performed during a Health Insurance Exchange (HIX) premium payment grace period."},
            {"767","Qualifications for emergent/urgent care"},
            {"768","Service date outside the accidental injury coverage period."},
            {"769","DME Repair or Maintenance"},
            {"770","Duplicate of a claim processed or in process as a crossover/coordination of benefits claim."},
            {"771","Claim submitted prematurely. Please resubmit after crossover/payer to payer COB allotted waiting period."},
            {"772","The greatest level of diagnosis code specificity is required."},
            {"773","One calendar year per claim."},
            {"774","Experimental/Investigational"},
            {"775","Entity Type Qualifier (Person/Non-Person Entity). Usage: this code requires use of an entity code."},
            {"776","Pre/Post-operative care"},
            {"777","Processed based on multiple or concurrent procedure rules."},
            {"778","Non-Compensable incident/event. Usage: To be used for Property and Casualty only."},
            {"779","Service submitted for the same/similar service within a set timeframe."},
            {"780","Lifetime benefit maximum"},
            {"781","Claim has been identified as a readmission"},
            {"782","Second surgical opinion"},
            {"783","Federal sequestration adjustment"},
            {"784","Electronic Visit Verification criteria do not match."},
            {"785","Missing/Invalid Sterilization/Abortion/Hospital Consent Form."},
            {"786","Submit claim to the third party property and casualty automobile insurer."},
            {"787","Resubmit a new claim, not a replacement claim."},
            {"788","Submit these services to the patient's Pharmacy Plan for further consideration."},
            {"789","Submit these services to the patient's Medical Plan for further consideration."},
            {"790","Submit these services to the patient's Dental Plan for further consideration."},
            {"791","Submit these services to the patient's Vision Plan for further consideration."},
            {"792","Submit these services to the patient's Behavioral Health Plan for further consideration."},
            {"793","Submit these services to the patient's Property and Casualty Plan for further consideration."},
            {"794","Claim could not complete adjudication in real time. Resubmit as a batch request."},
            {"795","Claim submitted prematurely. Please provide the prior payer's final adjudication."},
            {"796","Procedure code not valid for date of service."},
            {"798","Claim predetermination/estimation could not be completed in real time. Claim requires manual review upon submission. Do not resubmit."},
            {"799","Resubmit a replacement claim, not a new claim."}
        };

        EdiDocument ediDocument = null;

        public void Load835(string fileName)
        {
            ediDocument = EdiDocument.Load(fileName);

            RemittanceData remittance = new RemittanceData();
            Loop2000 loop2000 = null;
            Loop2100 loop2100 = null;
            Loop2110 loop2110 = null;

            string currentLoop = null;
            string currN1type = null;

            foreach (var segment in ediDocument.Segments)
            {
                switch (segment.Id)
                {
                    case "ST":
                        break;
                    case "BPR":
                        remittance.TransactionHandlingCode = segment[1];
                        remittance.TotalPremiumPaymentAmount = segment[2];
                        remittance.CreditorDebitFlagCode = segment[3];
                        break;
                    case "NTE":
                        break;
                    case "TRN":
                        break;
                    case "CUR":
                        break;
                    case "REF":
                        break;
                    case "DTM":
                        //472 = date of service

                        break;
                    case "N1":
                        if (segment[1] == "PR")
                        {
                            currentLoop = "1000A";
                            currN1type = segment[1];
                            //save payor info
                            remittance.PayerName = segment[2];
                        }
                        if (segment[1] == "PE")
                        {
                            currentLoop = "1000B";
                            currN1type = segment[1];
                            //payee identification
                            remittance.PayeeName = segment[2];
                            remittance.BankIdentifier = segment[4];
                        }
                        break;
                    case "N3":
                        if (currN1type == "PR")
                        {
                            remittance.PayerAddress = segment[1];
                            remittance.PayerAddress2 = segment[2];
                        }
                        if (currN1type == "PE")
                        {
                            remittance.PayeeAddress = segment[1];
                        }
                        break;
                    case "N4":
                        if (currN1type == "PR")
                        {
                            remittance.PayerCity = segment[1];
                            remittance.PayerState = segment[2];
                            remittance.PayerZip = segment[3];
                        }
                        if (currN1type == "PE")
                        {
                            remittance.PayeeCity = segment[1];
                            remittance.PayeeState = segment[2];
                            remittance.PayeeZip = segment[3];
                        }
                        break;
                    case "PER":
                        break;
                    case "RDM":
                        break;
                    case "LX":
                        currentLoop = "2000";
                        if (loop2000 != null)
                            remittance.loop2000s.Add(loop2000);
                        loop2000 = new Loop2000();
                        break;
                    case "TS3":
                        loop2000.TotalClaimCount = segment[4];
                        loop2000.TotalClaimChargeAmount = segment[5];
                        loop2000.TotalHCPCSReportedChargeAmount = segment[17];
                        loop2000.TotalHCPCSPayableAmount = segment[18];
                        break;
                    case "CLP":
                        //write out previous loop??
                        if (loop2100 != null)
                        {
                            loop2000.loop2100s.Add(loop2100);
                        }

                        loop2100 = new Loop2100();
                        currentLoop = "2100";
                        loop2100.AccountNo = segment[1];
                        loop2100.ClaimStatusCode = segment[2];
                        loop2100.ClaimChargeAmount = segment[3];
                        loop2100.ClaimPaymentAmount = segment[4];
                        loop2100.PatientResponsibilityAmount = segment[5];
                        loop2100.ClaimFilingIndicatorCode = segment[6];
                        loop2100.PayerClaimControlNumber = segment[7];
                        loop2100.FacilityTypeCode = segment[8];
                        loop2100.ClaimFrequencyCode = segment[9];
                        break;
                    case "AMT":
                        //B6 = PAID AMT
                        if (segment[1] == "B6")
                        {
                            loop2110.PaidAmount = segment[2];
                        }
                        //AU = ALLOWED AMT
                        if (segment[1] == "AU")
                        {
                            loop2110.AllowedAmount = segment[2];
                        }
                        break;
                    case "QTY":
                        break;
                    case "SVC":
                        if (loop2110 != null)
                            loop2100.loop2110s.Add(loop2110);

                        currentLoop = "2110";
                        loop2110 = new Loop2110();

                        loop2110.ProcedureCode = segment[1]; // segment 1.2
                        loop2110.LineItemChargeAmount = segment[2];
                        loop2110.MonetaryAmount = segment[3];
                        loop2110.RevenueCode = segment[4];
                        break;
                    case "CAS":
                        loop2110.ClaimAdjustmentGroupCode = segment[1];
                        // CO - contractual obligation
                        // CR - corrections and reversal
                        // OA - other adjustment
                        // PI - Payer initiated reductions
                        // PR - Patient Responsibility
                        loop2110.AdjustmentReasonCode = segment[2];
                        loop2110.AdjustmentAmount = segment[3];
                        loop2110.AdjustmentQuantity = segment[4];
                        break;
                    case "NM1":
                        if (segment[1] == "QC")
                        {
                            //patient info
                        }
                        break;
                    case "SE":
                        remittance.loop2000s.Add(loop2000);
                        break;
                    default:
                        break;
                }
            }

            //export remittance class to json

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(remittance);

            System.IO.File.WriteAllText(@"C:\temp\remit.json", json);

        }

        private void ProcessRemittance()
        {
            ChkRepository chkRepository = new ChkRepository(_connString);


        }

    }

    public class RemittanceData
    {
        public List<Loop2000> loop2000s = new List<Loop2000>();

        public string TransactionHandlingCode { get; set; }
        public string TotalPremiumPaymentAmount { get; set; }
        public string CreditorDebitFlagCode { get; set; }
        public string PayerName { get; set; }
        public string PayeeName { get; set; }
        public string BankIdentifier { get; set; }
        public string PayerAddress { get; set; }
        public string PayerAddress2 { get; set; }
        public string PayeeAddress { get; set; }
        public string PayerCity { get; set; }
        public string PayerState { get; set; }
        public string PayerZip { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZip { get; set; }
    }

    public class Loop2000
    {
        public string TotalClaimCount { get; set; }
        public string TotalClaimChargeAmount { get; set; }
        public string TotalHCPCSReportedChargeAmount { get; set; }
        public string TotalHCPCSPayableAmount { get; set; }

        public List<Loop2100> loop2100s = new List<Loop2100>();
    }

    public class Loop2100
    {
        public string AccountNo { get; set; }
        public string ClaimStatusCode { get; set; }
        public string ClaimChargeAmount { get; set; }
        public string ClaimPaymentAmount { get; set; }
        public string PatientResponsibilityAmount { get; set; }
        public string ClaimFilingIndicatorCode { get; set; }
        public string PayerClaimControlNumber { get; set; }
        public string FacilityTypeCode { get; set; }
        public string ClaimFrequencyCode { get; set; }

        public List<Loop2110> loop2110s = new List<Loop2110>();
    }

    public class Loop2110
    {
        public string ProcedureCode { get; set; }
        public string LineItemChargeAmount { get; set; }
        public string MonetaryAmount { get; set; }
        public string RevenueCode { get; set; }
        public string ClaimAdjustmentGroupCode { get; set; }
        public string AdjustmentReasonCode { get; set; }
        public string AdjustmentAmount { get; set; }
        public string AdjustmentQuantity { get; set; }
        public string PaidAmount { get; internal set; }
        public string AllowedAmount { get; internal set; }
    }


}
