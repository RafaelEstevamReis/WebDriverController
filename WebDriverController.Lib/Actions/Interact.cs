using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController.Actions
{
    public sealed class Interact : IWDAction
    {
        public Interact(By locator, Action<Controller, IWebElement> action)
        {
            Locator = locator ?? throw new ArgumentNullException(nameof(locator));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public By Locator { get; }
        public Action<Controller, IWebElement> Action { get; }
        public TimeSpan Timeout { get; private set; } = TimeSpan.FromSeconds(10);

        public IWDActionResult Execute(Controller wDController)
        {
            var wait = new WebDriverWait(wDController.WrappedDriver, Timeout);

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
        public static Controller Interact(this Controller controller, By locator, Action<Controller, IWebElement> action)
        {
            return controller.Do(new Interact(locator, action));
        }
    }
}
