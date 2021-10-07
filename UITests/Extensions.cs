using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace UITests
{
    public static class Extensions
    {
        public static IWebElement FindElementWithWait(this IWebDriver driver, By locator)
        {
            IWebElement element = null;
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(45));
            wait.PollingInterval = TimeSpan.FromSeconds(1);
            wait.Until(driver => element = driver.FindElement(locator));

            return element;
        }
    }
}
