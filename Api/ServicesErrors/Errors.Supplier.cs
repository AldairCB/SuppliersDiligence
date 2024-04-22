using ErrorOr;

namespace Api.ServicesErrors;

public static class Errors
{
    public static class Supplier{
        public static Error NotFound(Guid id) => Error.NotFound(
            code: "Supplier.NotFound",
            description: $"Supplier with ID: {id} was not found"
        );
    }
    public static class WebScraper
    {
        public static Error EntityNameNotProvided => Error.Failure(
            code: "WebScraper.EntityNameNotProvided",
            description: "Please provide an entity name to look in the data sources"
        );
        public static Error NoSourceOptionSelected => Error.Failure(
            code: "WebScraper.NoSourceOptionSelected",
            description: "Please select at least 1 source for the search"
        );
        public static Error NotFound => Error.NotFound(
            code: "WebScraper.NotFound",
            description: "Entity not found"
        );
        public static Error OfacServerError => Error.Failure(
            code: "WebScraper.OfacServerError",
            description: "There was an error with the Ofac server"
        );
        public static Error WorldBankServerError => Error.Failure(
            code: "WebScraper.WorldBankServerError",
            description: "There was an error with the World Bank server"
        );
        public static Error OffshoreLeaksServerError => Error.Failure(
            code: "WebScraper.OffshoreLeaksServerError",
            description: "There was an error with the OffshoreLeaks server"
        );
    }
}