using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TransitNETesting.Tests.SystemTests
{public class PatcoRouteInformationUatTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;

    public PatcoRouteInformationUatTests()
    {
        // Set your application’s base URL for the PATCO page
        // For example, if running locally:
        _baseUrl = "https://localhost:5001/PatcoRouteInformation/Index";

        var options = new ChromeOptions();
        options.AddArgument("--headless"); // Recommended for CI environments
         options.AddArgument("--no-sandbox");
         options.AddArgument("--disable-dev-shm-usage");

        _driver = new ChromeDriver(options);
    }

    [Fact]
    public void Page_Loads_And_Displays_Patco_Route_Information_Title()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Locate and verify title text
        var titleElement = _driver.FindElement(By.XPath("//div[@class='title' and text()='PATCO Route Information']"));
        Assert.NotNull(titleElement);
    }

    [Fact]
    public void Nj_Transit_And_Septa_Buttons_Are_Clickable()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Verify NJ Transit button presence
        var njTransitButton = _driver.FindElement(By.XPath("//button[text()='NJ Transit']"));
        Assert.NotNull(njTransitButton);

        // Click it and verify navigation or URL change if possible
        njTransitButton.Click();
        // After clicking you might need to verify something. Since we don’t know the exact route:
        // Assert.True(_driver.Url.Contains("NJTransit", StringComparison.OrdinalIgnoreCase));
        
        // Navigate back to original page for the next check
        _driver.Navigate().Back();

        // Verify SEPTA button
        var septaButton = _driver.FindElement(By.XPath("//button[text()='SEPTA']"));
        Assert.NotNull(septaButton);

        // Click it and verify navigation or URL change if possible
        septaButton.Click();
        // Assert.True(_driver.Url.Contains("SeptaRouteInformation", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Download_Schedule_Link_Is_Present()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        var downloadLink = _driver.FindElement(By.XPath("//a[contains(@href,'PATCO_Timetable.pdf')]"));
        Assert.NotNull(downloadLink);

        // Optionally, you could check that clicking the link downloads the file
        // This is often tricky in headless mode. Instead, just ensure the link is correct:
        Assert.Contains("PATCO_Timetable.pdf", downloadLink.GetAttribute("href"));
    }

    [Fact]
    public void Travel_Alerts_Link_Is_Present_And_Valid()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        var travelAlertsLink = _driver.FindElement(By.XPath("//a[contains(@href,'ridepatco.org/schedules/alerts.asp')]"));
        Assert.NotNull(travelAlertsLink);

        // Validate the href attribute
        var href = travelAlertsLink.GetAttribute("href");
        Assert.Contains("ridepatco.org/schedules/alerts.asp", href, StringComparison.OrdinalIgnoreCase);
    }

    public void Dispose()
    {
        _driver.Quit();
    }
}
}