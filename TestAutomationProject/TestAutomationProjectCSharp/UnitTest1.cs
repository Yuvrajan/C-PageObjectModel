using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using AventStack.ExtentReports;
using TestAutomationProjectCSharp.BaseRepository;
using TestAutomationProjectCSharp.PageObjects;
using TestAutomationProjectCSharp.Utilities;
using System;


namespace TestAutomationProjectCSharp
{
    public class Tests : Baserepository
    {

        [Test, Category("Challenging DOM")]
        public void TestScenario1()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            TheInternetHome home = new TheInternetHome(driver);
            ChallengingDOM challengingDOM = new ChallengingDOM(driver);
            string PageHeader = home.getPageHeader();
            Assert.AreEqual(PageHeader, "Welcome to the-internet");
            home.clickChallengingDOM();
            string redId = challengingDOM.FindElementIdbyColour("#c60f13");
            string blueId = challengingDOM.FindElementIdbyColour("#2ba6cb");
            string greenId = challengingDOM.FindElementIdbyColour("#5da423");
            ClickByLocator(By.Id(redId),driver,"Click button coloured red");

            try
            {
                Assert.AreNotEqual(redId, challengingDOM.FindElementIdbyColour("#c60f13"));
                Assert.AreNotEqual(blueId, challengingDOM.FindElementIdbyColour("#2ba6cb"));
                Assert.AreNotEqual(greenId, challengingDOM.FindElementIdbyColour("#5da423"));
            }
            catch (Exception ex)
            {
                Failurehandling("Id comparison after click redbutton",ex.ToString());
            }
        }

        [Test, Category("Dynamic Loading")]
        public void TestScenario2()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            TheInternetHome home = new TheInternetHome(driver);
            DynamicLoading dynamicLoad = new DynamicLoading(driver);

            string PageHeader = home.getPageHeader();
            Assert.AreEqual(PageHeader, "Welcome to the-internet");
            home.clickDynamicLoading();
            PageHeader = dynamicLoad.getPageHeader();
            Assert.AreEqual(PageHeader, "Dynamically Loaded Page Elements");
            dynamicLoad.clickExampleRenderedAfterTheFact();
            dynamicLoad.verifyTextRender();           

        }

    }
}