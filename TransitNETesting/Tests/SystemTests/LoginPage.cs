using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace TransitNETesting.Tests.SystemTests
{
    public class LoginPage : IDisposable
    {
        private readonly IWebDriver _driver;
        public LoginPage()
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
