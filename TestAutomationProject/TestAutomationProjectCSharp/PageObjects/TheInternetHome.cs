using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using TestAutomationProjectCSharp.BaseRepository;

namespace TestAutomationProjectCSharp.PageObjects
{
    class TheInternetHome : Baserepository
    {

        private IWebDriver driver;
        private By _header = By.ClassName("heading");
        private By _challengingdom = By.LinkText("Challenging DOM");
        private By _dynamicloading = By.LinkText("Dynamic Loading");
        private IJavaScriptExecutor js;

        public TheInternetHome(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string getPageHeader()
        {
            return getTextByLocator(_header, driver,"Get Home page header");
        }

        public void clickChallengingDOM()
        {
            ClickByLocator(_challengingdom, driver, "Click button labeled Challenging DOM");
            //driver.FindElement(_challengingdom).Click();
        }

        public void clickDynamicLoading()
        {
            ClickByLocator(_dynamicloading, driver, "Click button labeled Dynamic Loading");
        }

        

    }
}
