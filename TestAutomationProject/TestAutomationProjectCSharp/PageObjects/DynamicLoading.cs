using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using TestAutomationProjectCSharp.BaseRepository;
using AventStack.ExtentReports;
using NUnit.Framework;
using System.Threading;

namespace TestAutomationProjectCSharp.PageObjects
{
    internal class DynamicLoading : Baserepository
    {
        private IWebDriver driver;
        private By _header = By.TagName("h3");
        private By _example2renderedafter = By.XPath("//a[contains(text(),'rendered after')]");
        private By _example1hiddenelement = By.XPath("//a[contains(text(),'hidden')]");
        private By _startbutton = By.XPath("//button[contains(text(),'Start')]");
        private By _loadingicon = By.Id("loading");
        private By pagebody = By.TagName("body");
        public DynamicLoading(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string getPageHeader()
        {
            return getTextByLocator(_header, driver, "Dynamic Loading page header");
        }

        public void clickExampleRenderedAfterTheFact()
        {
            ClickByLocator(_example2renderedafter, driver, "Click Example 2_Element rendered after the fact");
        }

        public void verifyTextRender()
        { 
            ClickByLocator(_startbutton, driver, "Click Start to render element");
            string style = string.Empty;
            string body = string.Empty;
            bool elementFound = false;
            for (int i = 0; i < 10; i++)
            {
                style = getElementByLocator(_loadingicon, driver,"").GetAttribute("style");
                if (String.IsNullOrEmpty(style))
                {
                    body = getTextByLocator(pagebody, driver, "");
                    if (body.Contains("Hello World!"))
                    {
                        Failurehandling("Texting rendering", "Text element found before loading");
                        break;
                    }
                }
                else
                {
                    body = getTextByLocator(pagebody, driver, "");
                    if (body.Contains("Hello World!"))
                    {
                        Assert.Pass("Text rendered after loading");
                        elementFound = true;
                        break;
                    }
                }
                Thread.Sleep(3000);
            }
            if(!elementFound)
            {
                Failurehandling("Texting rendering", "Text was not rendered even after 30sec");
            }
        }

    }
}
