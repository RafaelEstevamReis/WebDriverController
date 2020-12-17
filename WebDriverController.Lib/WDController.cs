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
        {  }

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
    }
}
