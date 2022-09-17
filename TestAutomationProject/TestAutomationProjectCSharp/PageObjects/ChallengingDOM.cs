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
    
    internal class ChallengingDOM : Baserepository
    {
        private IWebDriver driver;
        private By _colouredButtoncontainer = By.XPath("//div[@class='large-2 columns']//a");
        public ChallengingDOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string FindElementIdbyColour(string colorHex)
        {
            IList<IWebElement> elementList = getElementList(_colouredButtoncontainer, driver,"Finding list of coloured buttons", true);
            int colorIndex = getElementIndexByColor(_colouredButtoncontainer, colorHex, driver);
            IWebElement button = elementList[colorIndex];
            string Id = button.GetAttribute("id");
            return Id;
        }
    }
}
