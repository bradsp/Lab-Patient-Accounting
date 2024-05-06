using System.Collections.Generic;

namespace LabBilling.Core.Services;
public interface IBilling837Service
{
    string AuthorizationInformation { get; set; }
    string AuthorizationInformationQualifier { get; set; }
    char ComponentSeparator { get; set; }
    char ElementTerminator { get; set; }
    string FunctionalIdentifierCode { get; set; }
    string InterchangeControlVersionNo { get; set; }
    string InterchangeIdQualifier { get; set; }
    string ProductionEnvironment { get; set; }
    string ReceiverId { get; set; }
    char RepetitionSeparator { get; set; }
    bool RequestInterchangeAcknowledgment { get; set; }
    string ResponsibleAgencyCode { get; set; }
    string SecurityInformation { get; set; }
    string SecurityInformationQualifier { get; set; }
    char SegmentTerminator { get; set; }
    string VersionReleaseSpecifierCode { get; }
    string VersionReleaseSpecifierCodeInstitutional { get; set; }
    string VersionReleaseSpecifierCodeProfessional { get; set; }

    string Generate837ClaimBatch(IEnumerable<ClaimData> claims, string interchangeControlNumber, string batchSubmitterId, string file_location, ClaimType claimType);
    string GenerateSingleClaim(ClaimData claim, string fileLocation);
}