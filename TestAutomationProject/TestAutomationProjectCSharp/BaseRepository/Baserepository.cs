using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using TestAutomationProjectCSharp.Utilities;
using OpenQA.Selenium.Support.UI;
using System.Drawing;
using System.Globalization;

namespace TestAutomationProjectCSharp.BaseRepository
{
    public class Baserepository
    {
        public IWebDriver driver;
        public  string projectPath;
        public  static ExtentReports extent; 
        public  static ExtentTest test; 
        public string screenShotPath;

        [OneTimeSetUp]
        public void Start()
        {
            if (extent == null)
            {
                projectPath = Assembly.GetCallingAssembly().CodeBase;
                projectPath = projectPath.Substring(0, projectPath.LastIndexOf("bin/"));
                projectPath = new Uri(projectPath).LocalPath;
                string reportPath = projectPath + @"Reports\\Report.html";
                var reporter = new ExtentV3HtmlReporter(reportPath);
                extent = new ExtentReports();
                extent.AttachReporter(reporter);
            }
        }

        [OneTimeTearDown]
        public void EndReport()
        {
            extent.Flush();
        }

        [SetUp]
        public void LaunchBrowser()
        {

            string projectPath = Assembly.GetCallingAssembly().CodeBase;
            projectPath = projectPath.Substring(0, projectPath.IndexOf("bin/"));
            projectPath = projectPath.Replace("file:///", "");

            string browser = JsonUtils.GetValue(projectPath + @"/Testdata/Data.json", "browser");
            if (browser.ToLower().Equals("ff"))
            {
                driver = new FirefoxDriver();
            }
            else if (browser.ToLower().Equals("edge"))
            {
                driver = new EdgeDriver();
            }
            else
            {
                driver = new ChromeDriver();
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            string url = JsonUtils.GetValue(projectPath + @"/Testdata/Data.json", "url");
            driver.Url = url;
        }
        public void TakeScreenShot(string testName)
        {
            if (driver != null)
            {
                string name = DateTime.Now.ToString().Replace('/', '-').Replace(':', '-');
                screenShotPath = projectPath + @"\Reports\screenshot_" + testName + "_" + name + ".png";
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                Screenshot ss = ts.GetScreenshot();
                ss.SaveAsFile(screenShotPath);
            }
        }

        public void ClickByLocator(By locator, IWebDriver driver, string step)
        {
            try
            {
                driver.FindElement(locator).Click();
                test.Log(Status.Info, String.Format("Step_{1}_Element clicked by {0}", locator, step));
            }
            catch (Exception ex)
            {
                Failurehandling(step, ex.ToString());
            }

        }
        public void TypeByLocator(By locator, string text, string step)
        {
            try
            {
                driver.FindElement(locator).SendKeys(text);
                test.Log(Status.Info, String.Format("Step_{1}_Text entered at element {0}", locator, step));
            }
            catch (Exception ex)
            {
                Failurehandling(step, ex.ToString());
            }
        }
       
        public void waituntil(By locator, string step, int sec = 50)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(sec));
                wait.Until(x => x.FindElement(locator));
                test.Log(Status.Info, String.Format("Step_{1}_Element found at{0}", locator, step));
            }
            catch (Exception ex)
            {
                Failurehandling(step, ex.ToString());

            }
        }

        public bool isElementPresent(By locator, IWebDriver driver, string step)
        {
            bool ispresent = false;
            try
            {
                List<IWebElement> e = new List<IWebElement>();
                e.AddRange(driver.FindElements(locator));
                if (e.Count > 0)
                {
                    ispresent = true;
                }
            }
            catch (Exception ex)
            {
                Failurehandling(step, ex.ToString());
            }
            return ispresent;
        }

        public IWebElement getElementByLocator(By locator, IWebDriver driver, string step)
        {
            IWebElement  ele = null;
            try
            {
                ele =  driver.FindElement(locator);
                test.Log(Status.Info, String.Format("Step_{1}_Element found at{0}", locator, step));
            }
            catch (Exception ex)
            {
                Failurehandling(step, ex.ToString());
            }
            return ele;
        }
    
       public string getTextByLocator(By locator, IWebDriver driver, string step)
       {
            string text = null;
            try
            {
                text =  driver.FindElement(locator).Text;
                test.Log(Status.Info, String.Format("Step_{1}_Element found at{0}", locator, step));
            }
            catch (Exception ex)
            {
                Failurehandling(step, ex.ToString());
            }
            return text;
        }

        public IList<IWebElement> getElementList(By locator,IWebDriver driver, string step,bool throwEx = false)
        {
            IList<IWebElement> eleList = null;
            try
            {
                eleList = driver.FindElements(locator);
                if((throwEx)&&(eleList.Count==0))
                {
                    throw new ArgumentException("Locator returned no elements");
                }
                test.Log(Status.Info, String.Format("Step_{1}_Element found at{0}", locator, step));
            }
            catch (Exception ex)
            {
                Failurehandling(step, ex.ToString());
            }
            return eleList;
        }

        public int getElementIndexByColor(By locator,string color, IWebDriver driver)
        {
            IList<IWebElement> all = driver.FindElements(locator);

            String[] elementColor = new String[all.Count];
            int i = 0;
            int elementIndex = 0;
            foreach (IWebElement element in all)
            {
                Color c = ParseColor(element.GetCssValue("background-color"));
                string htmlColor = ColorTranslator.ToHtml(c);
                if (String.Equals(htmlColor, color, StringComparison.OrdinalIgnoreCase))
                {
                    elementIndex =  i;
                    break;
                }
                i++;

            }
            return elementIndex;
           
       
        }

        public  Color ParseColor(string cssColor)
        {
            cssColor = cssColor.Trim();

            if (cssColor.StartsWith("#"))
            {
                return ColorTranslator.FromHtml(cssColor);
            }
            else if (cssColor.StartsWith("rgb"))
            {
                int left = cssColor.IndexOf('(');
                int right = cssColor.IndexOf(')');

                if (left < 0 || right < 0)
                    throw new FormatException("rgba format error");
                string noBrackets = cssColor.Substring(left + 1, right - left - 1);

                string[] parts = noBrackets.Split(',');

                int r = int.Parse(parts[0], CultureInfo.InvariantCulture);
                int g = int.Parse(parts[1], CultureInfo.InvariantCulture);
                int b = int.Parse(parts[2], CultureInfo.InvariantCulture);

                if (parts.Length == 3)
                {
                    return Color.FromArgb(r, g, b);
                }
                else if (parts.Length == 4)
                {
                    float a = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    return Color.FromArgb((int)(a * 255), r, g, b);
                }
            }
            throw new FormatException("Not rgb, rgba or hexa color string");
        }

        
        public void Failurehandling(string step, string ex)
        {
            test.Log(Status.Fail, ex.ToString());
            TakeScreenShot(String.Format("Error_{0}_atStep_{1}",TestContext.CurrentContext.Test.Name, step));
        }

        [TearDown]
        public void Close()
        {
            driver.Quit();
        }

    }
}
