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
        private IWebDriver driver;

        [TestInitialize]
        public void Setup()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://portalqa.rics.io/login"); // Replace with your actual URL
        }

        [TestMethod]
        public void Login()
        {
            // Replace these with the actual credentials
            string username = "jacobotests@gmail.com";
            string password = "tesTing1$";

            // Use XPath to select elements
            string usernameXpath = "//input[@id='ion-input-0']";
            string passwordXpath = "//input[@id='ion-input-1']";
            string loginButtonXpath = "//button[@id='loginButton']";

            // Login logic with explicit waits
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10)); // Set a timeout of 10 seconds

            try
            {
                // Enter username
                IWebElement usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(usernameXpath)));
                usernameField.SendKeys(username);

                // Enter password
                IWebElement passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(passwordXpath)));
                passwordField.SendKeys(password);

                // Click on an element to dismiss any overlay or dropdown
                driver.FindElement(By.XPath("//*")).Click();

                // Explicitly set a wait timeout of 20 seconds
                WebDriverWait wait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

                // Click login button with wait
                IWebElement loginButton = wait3.Until(ExpectedConditions.ElementToBeClickable(By.XPath(loginButtonXpath)));
                loginButton.Click();

                // You may need to add assertions or further logic here based on the result
            }
            finally
            {
                // Add cleanup or verification logic if needed
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver?.Quit();
        }
    }
}
