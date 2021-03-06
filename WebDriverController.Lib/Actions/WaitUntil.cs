﻿using System;
using System.Collections;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController.Actions
{
    public sealed class WaitUntil : IWDAction
    {
        public static TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(1);

        public enum Is 
        {
            Exists,
            Visible,
            Clickable,
            Destroyed,
        }

        public IWDActionResult Result { get; set; } = new Results.None();
        public TimeSpan Timeout { get; set; } = DefaultTimeout;
        public By Element { get; }
        public Is Test { get; }

        public WaitUntil(By Element, Is test)
        {
            this.Element = Element ?? throw new ArgumentNullException(nameof(Element));
            Test = test;
        }

        public IWDActionResult Execute(Controller wDController)
        {
            var driver = wDController.WrappedDriver;

            var wait = new WebDriverWait(driver, Timeout);

            if (Test == Is.Destroyed)
            {
                // has to exists here
                IWebElement wElement = driver.FindElement(Element);

                wait.Until((d) =>
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
    public static class WaitUntilExtension
    {
        public static Controller WaitUntil_Visible(this Controller controller, By locator)
        {
            return controller.Do(WaitUntil.IsVisible(locator));
        }
        public static Controller WaitUntil_Visible(this Controller controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.IsVisible(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }

        public static Controller WaitUntil_IsClickable(this Controller controller, By locator)
        {
            return controller.Do(WaitUntil.IsClickable(locator));
        }
        public static Controller WaitUntil_IsClickable(this Controller controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.IsClickable(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }

        public static Controller WaitUntil_Exists(this Controller controller, By locator)
        {
            return controller.Do(WaitUntil.Exists(locator));
        }
        public static Controller WaitUntil_Exists(this Controller controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.Exists(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }

        public static Controller WaitUntil_IsDestroyed(this Controller controller, By locator)
        {
            return controller.Do(WaitUntil.IsDestroyed(locator));
        }
        public static Controller WaitUntil_IsDestroyed(this Controller controller, By locator, TimeSpan timeout)
        {
            var w = WaitUntil.IsDestroyed(locator);
            w.Timeout = timeout;
            return controller.Do(w);
        }

        public static Controller WaitUntil_UserNavigate(this Controller controller)
        {
            return WaitUntil_UserNavigate(controller, TimeSpan.FromMinutes(2));
        }
        public static Controller WaitUntil_UserNavigate(this Controller controller, TimeSpan timeout)
        {
            var w = WaitUntil.IsDestroyed(By.TagName("body"));
            w.Timeout = timeout;
            return controller.Do(w);
        }
    }
}
