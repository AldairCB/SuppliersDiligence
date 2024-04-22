namespace ApiModel.WebScraper;

public record WorldBankResultResponse(
    string FirmName,
    string Address,
    string Country,
    DateTime IneligibilityFromDate,
    DateTime? IneligibilityToDate,
    string IneligibilityStatus,
    List<string> Grounds
);