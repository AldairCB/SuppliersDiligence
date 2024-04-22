using ErrorOr;

namespace Api.Services.WebScraper;

public interface IWebScraper
{
    ErrorOr<EntitySearchResult> SearchEntityByName(
        string entityName,
        bool ofac,
        bool offshoreLeaks,
        bool worldBank
    );
}