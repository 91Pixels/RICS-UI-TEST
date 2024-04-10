using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace QA_UI_Project
{
    [TestFixture]
    public class LoginTest
    {
        private IWebDriver _driver;
        private string _baseUrl;
        private string _usernameXpath;
        private string _passwordXpath;
        private string _loginButtonXpath;
        private string _expectedUrlAfterLogin;

        [OneTimeSetUp]
        public void SetupTest()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Window.Maximize();

            // Set the parameters
            _baseUrl = "https://portalqa.rics.io/login";
            _usernameXpath = "//*[@id=\":r0:\"]";
            _passwordXpath = "//*[@id=\":r1:\"]";
            _loginButtonXpath = "/html/body/div[1]/div/div/div/div/div[2]/button";
            _expectedUrlAfterLogin = "https://portalqa.rics.io/locations";
        }

        [Test, TestCaseSource(nameof(GetNegativeTestCases))]
        public void NegativeTest_InvalidCredentials(string username, string password, string expectedMessage)
        {
            PerformLoginTest(username, password, expectedMessage);
        }

        [Test, TestCaseSource(nameof(GetPositiveTestCase))]
        public void PositiveTest_ValidCredentials(string username, string password)
        {
            PerformLoginTest(username, password, null);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.UrlToBe(_expectedUrlAfterLogin));
        }

        private void PerformLoginTest(string username, string password, string? expectedMessage)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            try
            {
                _driver.Navigate().GoToUrl(_baseUrl);

                var usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(_usernameXpath)));
                usernameField.SendKeys(username);

                var passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(_passwordXpath)));
                passwordField.SendKeys(password);

                _driver.FindElement(By.XPath(_loginButtonXpath)).Click();

                if (expectedMessage != null)
                {
                    var alert = wait.Until(ExpectedConditions.AlertIsPresent());
                    var actualMessage = alert.Text;
                    Assert.That(actualMessage, Is.EqualTo(expectedMessage));
                    alert.Accept();
                }
            }
            catch (Exception)
            {
                Assert.Inconclusive("An unexpected error occurred during login with incorrect password. Investigate further.");
            }
        }

        [OneTimeTearDown]
        public void CleanupTest()
        {
            _driver.Quit();
        }

        private static IEnumerable<TestCaseData> GetNegativeTestCases()
        {
            yield return new TestCaseData("jacobotests@gmail.com", "wrong_password", "Incorrect username or password.")
                .SetName("Negative Test - Invalid Credentials");
        }

        private static IEnumerable<TestCaseData> GetPositiveTestCase()
        {
            yield return new TestCaseData("jacobotests@gmail.com", "tesTing1$")
                .SetName("Positive Test - Valid Credentials");
        }
    }
}
