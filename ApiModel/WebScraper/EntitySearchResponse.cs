namespace ApiModel.WebScraper;

public record EntitySearchResponse(
    string SearchedKeyword,
    int NumHits,
    List<OfacResultResponse> OfacResults,
    List<OffshoreLeaksResultResponse> OffshoreLeaksResults,
    List<WorldBankResultResponse> WorldBankResults
);