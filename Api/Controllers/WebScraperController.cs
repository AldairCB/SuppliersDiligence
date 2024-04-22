using Api.Services;
using Api.Services.WebScraper;
using Api.ServicesErrors;
using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class WebScraperController : ApiController
{
    private readonly IWebScraper _webScraper;
    public WebScraperController(IWebScraper webScraper){
        _webScraper = webScraper;
    }

    [HttpGet("/api/screen")]
    public IActionResult SearchEntityByName(
        string entityName,
        bool ofac = false,
        bool worldBank = false,
        bool offshoreLeaks = false
    ){
        if(entityName == null || entityName == ""){
            return Problem([Errors.WebScraper.EntityNameNotProvided]);
        }
        if(ofac == false && worldBank == false && offshoreLeaks == false){
            return Problem([Errors.WebScraper.NoSourceOptionSelected]);
        }
        
        ErrorOr<EntitySearchResult> searchResult = _webScraper.SearchEntityByName(
            entityName,
            ofac,
            offshoreLeaks,
            worldBank
        );
        return searchResult.Match(
            entitySearchresult => Ok(entitySearchresult.ToResponse()),
            errors => Problem(errors)
        );
    }
}