using ApiModel.WebScraper;
namespace Api.Services;

public class EntitySearchResult
{
    public string SearchedKeyword { get; set; }
    public int NumHits { get; set; }
    public List<OfacResult> OfacResults { get; set; }
    public List<OffshoreLeaksResult> OffshoreLeaksResults { get; set; }
    public List<WorldBankResult> WorldBankResults { get; set; }

    public EntitySearchResult(
        string searchedKeyword,
        int numHits,
        List<OfacResult> ofacResults,
        List<OffshoreLeaksResult> offshoreLeaksResults,
        List<WorldBankResult> worldBankResults
    ) {
        SearchedKeyword = searchedKeyword;
        NumHits = numHits;
        OfacResults = ofacResults;
        OffshoreLeaksResults = offshoreLeaksResults;
        WorldBankResults = worldBankResults;
    }

    public EntitySearchResponse ToResponse(){
        return new EntitySearchResponse(
            SearchedKeyword,
            NumHits,
            OfacResults.Select(e => e.ToResponse()).ToList(),
            OffshoreLeaksResults.Select(e => e.ToResponse()).ToList(),
            WorldBankResults.Select(e => e.ToResponse()).ToList()
        );
    }
}