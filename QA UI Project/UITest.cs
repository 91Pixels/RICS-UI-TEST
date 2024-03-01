using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace QA_UI_Project
{
    [TestClass]
    public class LoginTest
    {
        private IWebDriver? driver;

        [TestInitialize]
        public void Setup()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://portalqa.rics.io/login"); // Your actual URL
        }

        [TestMethod]
        public void Login()
        {
            // Credentials
            string username = "jacobotests@gmail.com";
            string password = "tesTing1$";

            // XPath to select elements
            string usernameXpath = "//input[@id='ion-input-0']";
            string passwordXpath = "//input[@id='ion-input-1']";
            string loginButtonXpath = "//button[@id='loginButton']";

            // Login logic with explicit waits
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // Set a timeout of 10 seconds

            try
            {
               
                IWebElement usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(usernameXpath)));
                usernameField.SendKeys(username);

                
                IWebElement passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(passwordXpath)));
                passwordField.SendKeys(password);

                
                driver.FindElement(By.XPath("//*")).Click();

                
                WebDriverWait wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                
                IWebElement loginButton = wait3.Until(ExpectedConditions.ElementToBeClickable(By.XPath(loginButtonXpath)));
                loginButton.Click();

                
            }
            finally
            {
               
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver?.Quit();
        }
    }
}
