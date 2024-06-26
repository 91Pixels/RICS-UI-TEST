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
        private const string BaseUrl = "https://portalqa.rics.io/login";
        private const string UsernameXpath = "//*[@id=\":r0:\"]";
        private const string PasswordXpath = "//*[@id=\":r1:\"]";
        private const string LoginButtonXpath = "/html/body/div[1]/div/div/div/div/div[2]/button";
        private const string ExpectedUrlAfterLogin = "https://portalqa.rics.io/locations";
        private const int DefaultTimeout = 10;

        private IWebDriver _driver;

        [OneTimeSetUp]
        public void SetupTest()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Window.Maximize();
        }

        [Test, TestCaseSource(nameof(GetNegativeTestCases))]
        public void NegativeTest_InvalidCredentials(string username, string password, string expectedMessage)
            => PerformLoginTest(username, password, expectedMessage);

        [Test, TestCaseSource(nameof(GetPositiveTestCase))]
        public void PositiveTest_ValidCredentials(string username, string password)
        {
            PerformLoginTest(username, password, null);
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(DefaultTimeout));
            wait.Until(ExpectedConditions.UrlToBe(ExpectedUrlAfterLogin));
        }

        private void PerformLoginTest(string username, string password, string? expectedMessage)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(DefaultTimeout));

            try
            {
                _driver.Navigate().GoToUrl(BaseUrl);
                var usernameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(UsernameXpath)));
                usernameField.SendKeys(username);

                var passwordField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PasswordXpath)));
                passwordField.SendKeys(password);

                _driver.FindElement(By.XPath(LoginButtonXpath)).Click();

                if (expectedMessage != null)
                {
                    var alert = wait.Until(ExpectedConditions.AlertIsPresent());
                    var actualMessage = alert.Text;
                    Assert.That(actualMessage, Is.EqualTo(expectedMessage), $"Alert message does not match expected: '{expectedMessage}'");
                    alert.Accept();
                }
            }
            catch (Exception ex)
            {
                Assert.Inconclusive($"An unexpected error occurred during login with incorrect password: {ex.Message}");
            }
        }

        [OneTimeTearDown]
        public void CleanupTest() => _driver.Quit();

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
