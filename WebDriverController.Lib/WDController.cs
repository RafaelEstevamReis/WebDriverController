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
            IWDActionResult result = action.Execute(this);

            result?.Apply(this, WrappedDriver);

            return this;
        }

        public WDController Inspect(Action<WDController> action)
        {
            action(this);
            return this;
        }

        public WDController InspectIf(Func<WDController, bool> condition, Action<WDController> action)
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

        public T ExecuteScript<T>(string script, params object[] args)
        {
            var js = (IJavaScriptExecutor)WrappedDriver;
            return (T)js.ExecuteScript(script, args);
        }
    }
}
