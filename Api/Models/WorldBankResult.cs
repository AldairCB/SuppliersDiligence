using ApiModel.WebScraper;

namespace Api.Services;

public class WorldBankResult
{
    public string FirmName { get; set; }
    public string Address { get; set; }
    public string Country { get; set; }
    public DateTime IneligibilityFromDate { get; set; }
    public DateTime? IneligibilityToDate { get; set; }
    public string IneligibilityStatus { get; set; }
    public List<string> Grounds { get; set; }

    public WorldBankResult(
        string firmName,
        string address,
        string country,
        DateTime ineligibilityFromDate,
        DateTime? ineligibilityToDate,
        string ineligibilityStatus,
        List<string> grounds
    ){
        FirmName = firmName;
        Address = address;
        Country = country;
        IneligibilityFromDate = ineligibilityFromDate;
        IneligibilityToDate = ineligibilityToDate;
        IneligibilityStatus = ineligibilityStatus;
        Grounds = grounds;
    }

    public WorldBankResultResponse ToResponse(){
        return new WorldBankResultResponse(
            FirmName,
            Address,
            Country,
            IneligibilityFromDate,
            IneligibilityToDate,
            IneligibilityStatus,
            Grounds
        );
    }
}