namespace Api.Services.WebScraper;
using System;
using System.Collections.Generic;
using Api.ServicesErrors;
using ErrorOr;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

public class WebScraper : IWebScraper
{

    private readonly HttpClient _httpClient;
    private ChromeDriver? _driver;

    public WebScraper()
    {
        _httpClient = new HttpClient();
    }

    public ErrorOr<EntitySearchResult> SearchEntityByName(
        string entityName,
        bool ofac,
        bool offshoreLeaks,
        bool worldBank
    ){

        offshoreLeaks = false; //TODO: Remove this once the feature is implemented

        _driver = new ChromeDriver();
        ErrorOr<List<OfacResult>> ofacSearch = new();
        ErrorOr<List<OffshoreLeaksResult>> offshoreleaksSearch = new();
        ErrorOr<List<WorldBankResult>> worldBankSearch = new();
        if(ofac){
            ofacSearch = SearchInOfac(entityName);
            if (ofacSearch.IsError){
                _driver.Close();
                return Errors.WebScraper.OfacServerError;
            }
        }
        if(offshoreLeaks){
            offshoreleaksSearch = SearchInOffshoreLeaks(entityName);
            if(offshoreleaksSearch.IsError) {
                _driver.Close();
                return Errors.WebScraper.OffshoreLeaksServerError;
            }
        }
        if(worldBank){
            worldBankSearch = SearchInWorldBank(entityName);
            if(worldBankSearch.IsError) {
                _driver.Close();
                return Errors.WebScraper.WorldBankServerError;
            }
        }
        
        _driver.Close();
        return new EntitySearchResult(
            searchedKeyword: entityName,
            // Sum the number of hits found from each source, if a source wasn't requested, then just add 0
            numHits: (ofac ? ofacSearch.Value.Count : 0) + (offshoreLeaks ? offshoreleaksSearch.Value.Count : 0) + (worldBank ? worldBankSearch.Value.Count : 0),
            ofacResults: ofac ? ofacSearch.Value : [],
            offshoreLeaksResults: offshoreLeaks ? offshoreleaksSearch.Value : [],
            worldBankResults: worldBank ? worldBankSearch.Value : []
        );
    }

    private ErrorOr<List<OfacResult>> SearchInOfac(string entityName){
        string url = "https://sanctionssearch.ofac.treas.gov/";
        _driver!.Navigate().GoToUrl(url);

        // Input name in name field
        var textField = _driver.FindElement(By.Id("ctl00_MainContent_txtLastName"));
        textField.SendKeys(entityName);

        // //Waiting for the page to load before clicking
        // _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

        // Clicking search
        var searchButton = _driver.FindElement(By.Id("ctl00_MainContent_btnSearch"));
        searchButton.Click();

        //getting the data from the resulting table
        IWebElement resultsTable;
        List<OfacResult> ofacResults = [];
        try{
            resultsTable = _driver.FindElement(By.Id("gvSearchResults"));
        } catch (NoSuchElementException){
            return ofacResults;
        }

        var rows = resultsTable.FindElements(By.TagName("tr"));
        foreach (var row in rows)
        {
            var cells = row.FindElements(By.TagName("td"));
            ofacResults.Add(new OfacResult(
                name: cells[0].Text,
                address: cells[1].Text,
                type: cells[2].Text,
                programs: [cells[3].Text],
                listAttribute: cells[4].Text,
                score: cells[5].Text
            ));
        }
        
        return ofacResults;
    }
    private ErrorOr<List<OffshoreLeaksResult>> SearchInOffshoreLeaks(string entityName){
        string url = $"https://offshoreleaks.icij.org/search?q={entityName}&c=&j=&d=";
        _driver!.Navigate().GoToUrl(url);

        //Accepting the terms and conditions
        var checkBox = _driver.FindElement(By.Id("accept"));
        checkBox.Click();

        // var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
        // var submitBtn = wait.Until(
        //     drv => drv.FindElement(By.XPath("//*[@id='__BVID__68___BV_modal_body_']/form/div/div[2]/button"))
        // );
        // submitBtn.Click();

        var submitBtn = _driver.FindElement(By.XPath("//*[@id='__BVID__68___BV_modal_body_']/form/div/div[2]/button"));
        submitBtn.Click();

        //getting the data from the resulting table
        var resultsTable = _driver.FindElement(By.XPath("//*[@id='search_results']/div[1]/table"));
        var rows = resultsTable.FindElements(By.TagName("tr"));
        
        List<OffshoreLeaksResult> offshoreLeaksResults = [];
        foreach (var row in rows){
            var cells = row.FindElements(By.TagName("td"));
            Console.WriteLine($"{cells[0]}");
            // offshoreLeaksResults.Add(new OffshoreLeaksResult(
            //     entity: cells[0].Text,
            //     jurisdiction: cells[1].Text,
            //     linkedTo: cells[2].Text,
            //     dataFrom: cells[3].Text
            // ));
        }

        return offshoreLeaksResults;
    }
    private ErrorOr<List<WorldBankResult>> SearchInWorldBank(string entityName){
        string url = "https://projects.worldbank.org/en/projects-operations/procurement/debarred-firms";
        _driver!.Navigate().GoToUrl(url);

        // Adding a wait because the table takes some time to load
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        var resultsTable = wait.Until(
            drv => _driver.FindElement(By.XPath("//*[@id='k-debarred-firms']/div[3]/table"))
        );

        // filtering the table by the requested entity name
        var searchField = _driver.FindElement(By.XPath("//*[@id='category']"));
        searchField.SendKeys(entityName);
        
        // Pulling the data into a list of WorldBankResults
        List<WorldBankResult> worldBankResults = [];
        var rows = resultsTable.FindElements(By.TagName("tr"));
        foreach (var row in rows)
        {
            var cells = row.FindElements(By.TagName("td"));
            
            string status = "Finished";
            DateTime? endDate = null;
            switch (cells[5].Text)
            {
                case "Ongoing":
                    status = "Ongoing";
                    break;
                case "Permanent":
                    status = "Permanent";
                    break;
                default:
                    endDate = DateTime.Parse(cells[5].Text);
                    break;
            }

            worldBankResults.Add(new WorldBankResult(
                firmName: cells[0].Text,
                // theres a cells[1] that it is always empty for some reason
                address: cells[2].Text,
                country: cells[3].Text,
                ineligibilityFromDate: DateTime.Parse(cells[4].Text),
                ineligibilityToDate: endDate,
                ineligibilityStatus: status,
                grounds: [cells[6].Text]
            ));
        }
        return worldBankResults;
    }
}
