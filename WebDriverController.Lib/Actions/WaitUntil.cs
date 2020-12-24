using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class WaitUntil : IWDAction
    {
        public enum Is 
        {
            Exists,
            Visible,
            Clickable,
            Destroyed,
        }

        public IWDActionResult Result { get; set; } = new Results.None();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
        public By Element { get; }
        public Is Test { get; }

        public WaitUntil(By Element, Is test)
        {
            this.Element = Element ?? throw new ArgumentNullException(nameof(Element));
            Test = test;
        }

        public IWDActionResult Execute(WDController wDController, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, Timeout);

            if (Test == Is.Destroyed)
            {
                // has to exists here
                IWebElement wElement = driver.FindElement(Element);

                wait.Until((driver) =>
                {
                    try
                    {
                        // des not matter, should check something
                        bool visible = wElement.Enabled;
                        if (visible)
                        {
                            // not staled
                            return false;
                        }
                    }
                    catch (StaleElementReferenceException){ return true; }

                    return false;
                });
            }
            else
            {
                wait.Until(checkNonDestroyed);
            }

            return Result;
        }

        private bool checkNonDestroyed(IWebDriver driver)
        {
            IWebElement wElement;

            try
            {
                wElement = driver.FindElement(Element);
            }
            catch (NoSuchElementException)
            {
                wElement = null;
            }

            if (Test == Is.Exists)
            {
                return wElement != null;
            }

            if (wElement == null) return false;

            if (Test == Is.Visible)
            {
                return wElement.Displayed;
            }
            else if (Test == Is.Clickable)
            {
                return wElement.Displayed && wElement.Enabled;
            }

            return false;
        }

        public static WaitUntil Exists(By Element) => new WaitUntil(Element, Is.Exists);
        public static WaitUntil IsClickable(By Element) => new WaitUntil(Element, Is.Clickable);
        public static WaitUntil IsVisible(By Element) => new WaitUntil(Element, Is.Visible);
        public static WaitUntil IsDestroyed(By Element) => new WaitUntil(Element, Is.Destroyed);
    }
    public static class InteractWaitUntil
    {
        public static WDController WaitUntil_Visible(this WDController controller, By locator)
        {
            return controller.Do(WaitUntil.IsVisible(locator));
        }
        public static WDController WaitUntil_Visible(this WDController controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.IsVisible(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }
        public static WDController WaitUntil_IsClickable(this WDController controller, By locator)
        {
            return controller.Do(WaitUntil.IsClickable(locator));
        }
        public static WDController WaitUntil_IsClickable(this WDController controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.IsClickable(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }
        public static WDController WaitUntil_Exists(this WDController controller, By locator)
        {
            return controller.Do(WaitUntil.Exists(locator));
        }
        public static WDController WaitUntil_Exists(this WDController controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.Exists(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }
        public static WDController WaitUntil_IsDestroyed(this WDController controller, By locator)
        {
            return controller.Do(WaitUntil.IsDestroyed(locator));
        }
        public static WDController WaitUntil_IsDestroyed(this WDController controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.IsDestroyed(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }

        public static WDController WaitUntil_UserNavigate(this WDController controller)
        {
            return WaitUntil_UserNavigate(controller, TimeSpan.FromMinutes(2));
        }
        public static WDController WaitUntil_UserNavigate(this WDController controller, TimeSpan timeout)
        {
            var w = WaitUntil.IsDestroyed(By.TagName("body"));
            w.Timeout = timeout;
            return controller.Do(w);
        }
    }
}
