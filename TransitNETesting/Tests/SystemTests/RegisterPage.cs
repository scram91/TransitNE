using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TransitNETesting.Tests.SystemTests
{
    public class RegisterPage : IDisposable
    {
        private readonly IWebDriver _driver;

        public RegisterPage()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://localhost:7126"); 
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void CheckRegisterForm()
        {
            _driver.FindElement(By.Id("register")).Click();
            var registerForm = _driver.FindElement(By.Id("registerForm"));
            Assert.NotNull(registerForm);
        }
        [Fact]
        public void CheckSubmitButton()
        {
            _driver.FindElement(By.Id("register")).Click();
            var submitButton = _driver.FindElement(By.Id("registerSubmit"));
            Assert.NotNull(submitButton);
        }
    }
}
