using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController.Actions
{
    public sealed class WaitFor : IWDAction
    {
        public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(1);

        public IWDActionResult Result { get; set; } = new Results.None();
        public TimeSpan Timeout { get; set; } = DefaultTimeout;
        public Func<Controller, bool> WaitCondition = null;

        public IWDActionResult Execute(Controller wDController)
        {
            var driver = wDController.WrappedDriver;
            var wait = new WebDriverWait(driver, Timeout);

            wait.Until((d) => WaitCondition(wDController));

            return Result;
        }
    }

    public static class WaitForExtension
    {
        public static Controller WaitFor(this Controller controller, Func<Controller, bool> waitCondition)
        {
            return controller.Do(new WaitFor()
            {
                WaitCondition = waitCondition
            });
        }

        public static Controller WaitForAny(this Controller controller, params By[] locators)
        {
            return WaitForAny(controller, (IEnumerable<By>)locators);
        }
        public static Controller WaitForAny(this Controller controller, IEnumerable<By> locators)
        {
            return controller.WaitFor((ctr) =>
            {
                foreach (var loc in locators)
                {
                    try
                    {
                        var element = controller.FindElement(loc);
                        return true; // found one
                    }
                    catch (NoSuchElementException)
                    { }
                }
                // found none
                return false;
            });
        }

        public static Controller WaitForAll(this Controller controller, params By[] locators)
        {
            return WaitForAll(controller, (IEnumerable<By>)locators);
        }
        public static Controller WaitForAll(this Controller controller, IEnumerable<By> locators)
        {
            return controller.WaitFor((ctr) =>
            {
                foreach (var loc in locators)
                {
                    try
                    {
                        var element = controller.FindElement(loc);
                    }
                    catch (NoSuchElementException)
                    {
                        // not found one
                        return false;
                    }
                }
                // found all
                return true;
            });
        }
    }
}
