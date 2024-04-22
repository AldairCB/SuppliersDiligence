namespace ApiModel.WebScraper;

public record OfacResultResponse(
    string Name,
    string Address,
    string Type,
    List<string> Programs,
    string ListAttribute,
    string Score
);