using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

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
            driver.Navigate().GoToUrl("https://portalqa.rics.io/login"); // Replace with your actual URL
        }

        [TestMethod]
        public void LoginIncorrectPassword()
        {
            // Credentials
            string username = "jacobotests@gmail.com";
            string incorrectPassword = "wrong_password"; // Incorrect password

            // XPath to select elements
            string usernameXpath = "//*[@id=\":r0:\"]";
            string passwordXpath = "//*[@id=\":r1:\"]";
            string loginButtonXpath = "/html/body/div[1]/div/div/div/div/div[2]/button";

            // Login logic with explicit waits
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                // Login attempt with incorrect password
                IWebElement usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(usernameXpath)));
                usernameField.SendKeys(username);

                IWebElement passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(passwordXpath)));
                passwordField.SendKeys(incorrectPassword);

                driver.FindElement(By.XPath(loginButtonXpath)).Click(); // Click login button

                // Switch to the alert (if present)
                try
                {
                    IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());
                    // Assert alert text (optional)
                    string expectedMessage = "Incorrect username or password."; // Based on the image you provided
                    string actualMessage = alert.Text;
                    Assert.AreEqual(expectedMessage, actualMessage);

                    // Accept the alert (optional)
                    alert.Accept();
                }
                catch (NoAlertPresentException)
                {
                    // Handle the case where no alert is displayed (optional)
                    Assert.Fail("Expected error alert was not displayed.");
                }
            }
            catch (Exception)
            {
                // Handle any unexpected exceptions during login attempt
                Assert.Inconclusive("An unexpected error occurred during login with incorrect password. Investigate further.");
            }
        }

        [TestMethod]
        public void Login_CorrectPassword()
        {
            // Credentials
            string username = "jacobotests@gmail.com";
            string correctPassword = "tesTing1$"; // Replace with your actual correct password

            // XPath to select elements
            string usernameXpath = "//*[@id=\":r0:\"]";
            string passwordXpath = "//*[@id=\":r1:\"]";
            string loginButtonXpath = "/html/body/div[1]/div/div/div/div/div[2]/button";

            // Login logic with explicit waits
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            try
            {
                // Login attempt with correct password
                IWebElement usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(usernameXpath)));
                usernameField.SendKeys(username);

                IWebElement passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(passwordXpath)));
                passwordField.SendKeys(correctPassword);

                driver.FindElement(By.XPath(loginButtonXpath)).Click(); // Click login button

                // Assertion for successful login (no alert expected)
                try
                {
                    // Wait for a short duration to allow potential alerts to appear
                    Thread.Sleep(2000); // Simple wait (consider WebDriverWait for more robustness)

                    // Check for any alerts and fail the test if found
                    try
                    {
                        IAlert alert = driver.SwitchTo().Alert();
                        Assert.Fail("Unexpected alert displayed during successful login.");
                    }
                    catch (NoAlertPresentException)
                    {
                        // No alert expected for successful login
                    }
                }
                catch (Exception)
                {
                    // Handle any unexpected exceptions during login attempt
                    Assert.Inconclusive("An unexpected error occurred during login with correct password. Investigate further.");
                }
            }
            catch (Exception)
            {
                // Handle any unexpected exceptions during login attempt
                Assert.Inconclusive("An unexpected error occurred during login with correct password. Investigate further.");
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            driver.Quit();
        }
    }
}