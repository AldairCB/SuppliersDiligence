using ApiModel.WebScraper;

namespace Api.Services;

public class OffshoreLeaksResult
{
    public string Entity { get; set; }
    public string Jurisdiction { get; set; }
    public string LinkedTo { get; set; }
    public string DataFrom { get; set; }

    public OffshoreLeaksResult(
        string entity,
        string jurisdiction,
        string linkedTo,
        string dataFrom
    ){
        Entity = entity;
        Jurisdiction = jurisdiction;
        LinkedTo = linkedTo;
        DataFrom = dataFrom;
    }
    public OffshoreLeaksResultResponse ToResponse(){
        return new OffshoreLeaksResultResponse(
            Entity,
            Jurisdiction,
            LinkedTo,
            DataFrom
        );
    }
}