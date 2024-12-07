using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;

public class SeptaRouteInformationUatTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;

    public SeptaRouteInformationUatTests()
    {
        // Set your application’s base URL here.
        // For example, if running locally:
        _baseUrl = "https://localhost:7126/SeptaRouteInformation/Index";

        // Initialize the Chrome driver. Ensure chromedriver is on your PATH or in the project directory.
        var options = new ChromeOptions();
        options.AddArgument("--headless"); // Run in headless mode for CI/CD
        _driver = new ChromeDriver(options);
    }

    [Fact]
    public void Page_Loads_And_Shows_Septa_Route_Information_Title()
    {
        _driver.Navigate().GoToUrl(_baseUrl);
        
        // Verify that the title or a heading is displayed
        var titleElement = _driver.FindElement(By.XPath("//div[@class='title' and text()='Septa Route Information']"));
        Assert.NotNull(titleElement);
    }

    [Fact]
    public void NJ_Transit_Button_Is_Clickable_And_Patco_Button_Is_Clickable()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Locate the NJ Transit button and verify it’s present
        var njTransitButton = _driver.FindElement(By.XPath("//button[text()='NJ Transit']"));
        Assert.NotNull(njTransitButton);

        // Locate the PATCO button and verify it’s present
        var patcoButton = _driver.FindElement(By.XPath("//button[text()='PATCO']"));
        Assert.NotNull(patcoButton);
        
        // Optionally, click them and verify navigation
        // NOTE: Without a fully running application and known result pages, we just ensure no errors are thrown.
        njTransitButton.Click();
        // You might verify the URL changes or a new title appears
        _driver.Navigate().Back();

        patcoButton.Click();
        // Verify navigation or other effects as needed
        _driver.Navigate().Back();
    }

    [Fact]
    public void Select_Regional_Rail_Line_And_Submit()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Wait for the dropdown to be present
        var railDropdown = new SelectElement(_driver.FindElement(By.Id("RegionalRailLine")));
        // Select a line, e.g., "Airport"
        railDropdown.SelectByValue("Airport");

        // Submit the form
        var submitButton = _driver.FindElement(By.XPath("//input[@type='submit' and @value='Submit']"));
        submitButton.Click();

        // Here, you might assert that the resulting page or data has changed,
        // For now, we just ensure we navigated somewhere (check for a known element or URL)
        Assert.True(_driver.Url.Contains("GetSelectedLine", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Select_Metro_Line_And_Submit()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Wait for the Metro dropdown
        var metroDropdown = new SelectElement(_driver.FindElement(By.Id("MetroLine")));
        metroDropdown.SelectByValue("Broad St");

        var submitButton = _driver.FindElement(By.XPath("//input[@type='submit' and @value='submit' and ancestor::form[contains(@action,'GetSelectedMetro')]]"));
        submitButton.Click();

        Assert.True(_driver.Url.Contains("GetSelectedMetro", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Enter_Bus_Line_And_StopName_And_Submit()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        var buslineInput = _driver.FindElement(By.Id("busline"));
        var stopnameInput = _driver.FindElement(By.Id("stopname"));

        buslineInput.SendKeys("42");
        stopnameInput.SendKeys("Main St & 2nd Ave");

        var submitButton = _driver.FindElement(By.XPath("//input[@type='submit' and @value='submit' and ancestor::form[contains(@action,'GetSelectedBus')]]"));
        submitButton.Click();

        Assert.True(_driver.Url.Contains("GetSelectedBus", StringComparison.OrdinalIgnoreCase));
    }

    public void Dispose()
    {
        _driver.Quit();
    }
}

