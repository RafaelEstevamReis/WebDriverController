using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib
{
    public class WDController : EventFiringWebDriver
    {
        public WDController(IWebDriver driver) :
            base (driver)
        {

        }

        public WDController Do(IWDAction action)
        {
            IWDActionResult result = action.Execute(this, WrappedDriver);

            result?.Apply(this, WrappedDriver);

            return this;
        }

        public WDController Inspect(Action<WDController, IWebDriver> action)
        {
            action(this, WrappedDriver);
            return this;
        }
        public WDController Inspect(Action<WDController> action)
        {
            action(this);
            return this;
        }
        public WDController Inspect(Action<IWebDriver> action)
        {
            action(WrappedDriver);
            return this;
        }

        public WDController InspectIf(Func<WDController, bool> condition, Action<WDController, IWebDriver> action)
        {
            if (condition(this))
            {
                return Inspect(action);
            }
            return this;
        }

        public IWebElement FirstElementOrDefault(By by)
        {
            try
            {
                var element = WrappedDriver.FindElement(by);
                return element;
            }
            catch (NoSuchElementException) { return null; }
            catch { throw; }
        }
        public bool ElementExists(By by)
        {
            return FirstElementOrDefault(by) != null;
        }
    }
}
