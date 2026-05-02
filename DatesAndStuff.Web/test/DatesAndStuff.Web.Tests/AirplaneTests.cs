using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class AirplaneTests
{
    private IWebDriver driver;
    private StringBuilder verificationErrors;
    private const string BaseURL = "https://blazedemo.com/";
    private bool acceptNextAlert = true;

    private Process? _blazorProcess;



    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
        verificationErrors = new StringBuilder();
    }

    [TearDown]
    public void TeardownTest()
    {
        try
        {
            driver.Quit();
            driver.Dispose();
        }
        catch (Exception)
        {
            // Ignore errors if unable to close the browser
        }
        Assert.That(verificationErrors.ToString(), Is.EqualTo(""));
    }

    [TestCase(3)]
    public void Flight_BetweenMexicoCity_Dublin_AtLeast(int count)
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));


        var selectMexico = new SelectElement(
            wait.Until(d => d.FindElement(By.Name("fromPort")))
        );
        selectMexico.SelectByText("Mexico City");

        var selectDublin = new SelectElement(
            wait.Until(d => d.FindElement(By.Name("toPort")))
        );
        selectDublin.SelectByText("Dublin");

        var submitButton = wait.Until(d => d.FindElement(By.XPath("//input[@type='submit']")));

        submitButton.Click();

        var rows = wait.Until(d =>
        {
            var elements = d.FindElements(By.XPath("//table//tr"));
            return elements.Count > 0 ? elements : null;
        });

        rows.Count.Should().BeGreaterThanOrEqualTo(count);

    }

  
}