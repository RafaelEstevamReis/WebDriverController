using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class Interact : IWDAction
    {
        public Interact(By locator, Action<WDController, IWebElement> action)
        {
            Locator = locator ?? throw new ArgumentNullException(nameof(locator));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public By Locator { get; }
        public Action<WDController, IWebElement> Action { get; }
        public TimeSpan Timeout { get; private set; } = TimeSpan.FromSeconds(10);

        public IWDActionResult Execute(WDController wDController, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, Timeout);

            wait.Until((d) =>
            {
                IWebElement wElement;

                try
                {
                    wElement = d.FindElement(Locator);
                }
                catch (NoSuchElementException)
                {
                    wElement = null;
                    return false;
                }

                if (!wElement.Enabled) return false;

                Action(wDController, wElement);

                return true;
            });

            return new Results.None();
        }
    }
    public static class InteractExtension
    {
        public static WDController Interact(this WDController controller, By locator, Action<WDController, IWebElement> action)
        {
            return controller.Do(new Interact(locator, action));
        }
    }
}
