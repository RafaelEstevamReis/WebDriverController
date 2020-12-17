using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class Wait : IWDAction
    {
        public IWDActionResult Result { get; set; } = new Results.None();
        public TimeSpan Timeout { get; set; }

        public Func<IWebDriver, bool> HasFinished;

        public IWDActionResult Execute(WDController wDController, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, Timeout);

            wait.Until(HasFinished);

            return Result;    
        }
    }
}
