namespace ApiModel.WebScraper;

public record OffshoreLeaksResultResponse(
    string Entity,
    string Jurisdiction,
    string LinkedTo,
    string DataFrom
);