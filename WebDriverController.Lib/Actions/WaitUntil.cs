using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class WaitUntil : IWDAction
    {
        public enum Verification 
        {
            Exists,
            Visible,
            Clickable,
        }

        public IWDActionResult Result { get; set; } = new Results.None();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(10);
        public By Element { get; }
        public Verification Test { get; }

        public WaitUntil(By Element, Verification test)
        {
            this.Element = Element ?? throw new ArgumentNullException(nameof(Element));
            Test = test;
        }

        public IWDActionResult Execute(WDController wDController, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, Timeout);

            wait.Until((driver) =>
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

                if (Test == Verification.Exists)
                {
                    return wElement != null;
                }

                if (wElement == null) return false;
                
                if (Test == Verification.Visible)
                {
                    return wElement.Displayed;
                }
                else if (Test == Verification.Clickable)
                {
                    return wElement.Displayed && wElement.Enabled;
                }

                return false;
            });

            return Result;
        }


        public static WaitUntil Exists(By Element) => new WaitUntil(Element, Verification.Exists);
        public static WaitUntil IsClickable(By Element) => new WaitUntil(Element, Verification.Clickable);
        public static WaitUntil IsVisible(By Element) => new WaitUntil(Element, Verification.Visible);
    }
}
