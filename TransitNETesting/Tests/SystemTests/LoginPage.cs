using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace TransitNETesting.Tests.SystemTests
{
    public class LoginPage : IDisposable
    {
        private readonly IWebDriver _driver;
        public LoginPage()
        {
            var options = new ChromeOptions();
        
            // Set Chrome to run in headless mode
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");  // Required for some CI environments
            options.AddArgument("--disable-dev-shm-usage");  // Avoids the "DevToolsActivePort file doesn't exist" error
            options.AddArgument("--remote-allow-origins=*");  // Necessary in some configurations for CORS issues
        
            // Path to the Chrome WebDriver (if not using the Docker image with built-in WebDriver)
            options.BinaryLocation = "/usr/bin/google-chrome"; 

            _driver = new ChromeDriver(options);
            
            _driver.Navigate().GoToUrl("https://localhost:7126");
        }
        
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void CheckUsernameField()
        {
            _driver.FindElement(By.Id("login")).Click();
            var userNameField = _driver.FindElement(By.Id("username"));
            Assert.NotNull(userNameField);
        }

        [Fact]
        public void CheckPasswordField()
        {
            _driver.FindElement(By.Id("login")).Click();
            var passwordField = _driver.FindElement(By.Id("password"));
            Assert.NotNull(passwordField);
        }
        [Fact]
        public void CheckSubmitButton()
        {
            _driver.FindElement(By.Id("login")).Click();
            var submitButton = _driver.FindElement(By.Id("login-submit"));
            Assert.NotNull(submitButton);
        }
        [Fact]
        public void CheckRememberCheckbox()
        {
            _driver.FindElement(By.Id("login")).Click();
            var checkbox = _driver.FindElement(By.Id("remember-me"));
            Assert.NotNull(checkbox);
        }
        [Fact]
        public void CheckForgotPassword()
        {
            _driver.FindElement(By.Id("login")).Click();
            var forgotButton = _driver.FindElement(By.Id("forgot-password"));
            Assert.NotNull(forgotButton);
        }
        [Fact]
        public void CheckRegister()
        {
            _driver.FindElement(By.Id("login")).Click();
            var registerButton = _driver.FindElement(By.Id("register-page"));
            Assert.NotNull(registerButton);
        }
        [Fact]
        public void CheckResend()
        {
            _driver.FindElement(By.Id("login")).Click();
            var resend = _driver.FindElement(By.Id("resend-confirmation"));
            Assert.NotNull(resend);
        }
    }
}
