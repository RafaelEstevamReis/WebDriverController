using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class Interact : IWDAction
    {
        public Interact(By locator, Action<IWebDriver,IWebElement> action)
        {
            Locator = locator ?? throw new ArgumentNullException(nameof(locator));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public By Locator { get; }
        public Action<IWebDriver, IWebElement> Action { get; }
        public TimeSpan Timeout { get; private set; } = TimeSpan.FromSeconds(10);

        public IWDActionResult Execute(WDController wDController, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, Timeout);

            wait.Until((driver) =>
            {
                IWebElement wElement;

                try
                {
                    wElement = driver.FindElement(Locator);
                }
                catch (NoSuchElementException)
                {
                    wElement = null;
                    return false;
                }

                if (!wElement.Enabled) return false;

                Action(driver, wElement);

                return true;
            });

            return new Results.None();
        }
    }
}
