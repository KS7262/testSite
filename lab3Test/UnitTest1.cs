using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using Xunit;

namespace lab3Test
{
    public class UnitTest1 : IDisposable
    {
        private IWebDriver driver;
        private string baseUrl = "https://opensource-demo.orangehrmlive.com";

        public UnitTest1()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            driver.Quit();
        }

        [Fact]
        public void SuccessfulLogin()
        {
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("/dashboard"));

            Assert.Contains("/dashboard", driver.Url);
        }

        [Fact]
        public void InvalidPassword()
        {
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("wrongpassword");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var errorMessage = driver.FindElement(By.CssSelector(".oxd-alert-content-text")).Text;
            Assert.Equal("Invalid credentials", errorMessage);
        }


        [Fact]
        public void InvalidUsername()
        {
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.Name("username")).SendKeys("dmin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var errorMessage = driver.FindElement(By.CssSelector(".oxd-alert-content-text")).Text;
            Assert.Equal("Invalid credentials", errorMessage);
        }

        [Fact]
        public void RedirectToCorrectPageAfterLogin()
        {
            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var dashboardHeader = driver.FindElement(By.CssSelector("h6.oxd-topbar-header-breadcrumb-module")).Text;
            Assert.Equal("Dashboard", dashboardHeader);
        }
    }
}
