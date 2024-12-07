using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

public class HomePageUatTests : IDisposable
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;

    public HomePageUatTests()
    {
        // Set your application’s home URL here (e.g., "https://localhost:5001/").
        _baseUrl = "https://localhost:5001/";

        var options = new ChromeOptions();
        options.AddArgument("--headless"); // Recommended for CI environments
        _driver = new ChromeDriver(options);
    }

    [Fact]
    public void HomePage_Loads_And_Displays_Title_And_Subtitle()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Verify main title
        var mainTitle = _driver.FindElement(By.XPath("//div[@class='title' and text()='Transit NE']"));
        Assert.NotNull(mainTitle);

        // Verify subtitle
        var subTitle = _driver.FindElement(By.XPath("//div[@class='subtitle' and text()='Slogan TBD']"));
        Assert.NotNull(subTitle);
    }

    [Fact]
    public void HomePage_Displays_Hero_Images()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Check Philadelphia image presence and alt text
        var phillyImage = _driver.FindElement(By.XPath("//img[@class='left-image' and @alt='Philadelphia']"));
        Assert.NotNull(phillyImage);
        Assert.Contains("Philadelphia", phillyImage.GetAttribute("src"), StringComparison.OrdinalIgnoreCase);

        // Check NYC image presence and alt text
        var nycImage = _driver.FindElement(By.XPath("//img[@class='right-image' and @alt='NYC']"));
        Assert.NotNull(nycImage);
        Assert.Contains("NYC", nycImage.GetAttribute("src"), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void HomePage_Displays_Pertinent_Information_Sections()
    {
        _driver.Navigate().GoToUrl(_baseUrl);

        // Verify "Route Delays" card
        var routeDelaysCard = _driver.FindElement(By.XPath("//div[@class='card-header' and text()='Route Delays']"));
        Assert.NotNull(routeDelaysCard);

        // Verify "Updated Schedules" card
        var updatedSchedulesCard = _driver.FindElement(By.XPath("//div[@class='card-header' and text()='Updated Schedules']"));
        Assert.NotNull(updatedSchedulesCard);

        // Verify "Fare Changes" card
        var fareChangesCard = _driver.FindElement(By.XPath("//div[@class='card-header' and text()='Fare Changes']"));
        Assert.NotNull(fareChangesCard);
    }

    public void Dispose()
    {
        _driver.Quit();
    }
}

