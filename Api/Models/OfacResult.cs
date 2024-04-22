using ApiModel.WebScraper;

namespace Api.Services;

public class OfacResult
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Type { get; set; }
    public List<string> Programs { get; set; }
    public string ListAttribute { get; set; }
    public string Score { get; set; }
    
    public OfacResult(
        string name,
        string address,
        string type,
        List<string> programs,
        string listAttribute,
        string score
    ){
        Name = name;
        Address = address;
        Type = type;
        Programs = programs;
        ListAttribute = listAttribute;
        Score = score;
    }

    public OfacResultResponse ToResponse(){
        return new OfacResultResponse(
            Name,
            Address,
            Type,
            Programs,
            ListAttribute,
            Score
        );
    }
}