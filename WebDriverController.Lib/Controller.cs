using System;
using System.Drawing;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController
{
    public class Controller : EventFiringWebDriver
    {
        public Controller(IWebDriver driver) :
            base (driver)
        { }

        public Controller Do(IWDAction action)
        {
            IWDActionResult result = action.Execute(this);

            result?.Apply(this, WrappedDriver);

            return this;
        }

        public Controller Inspect(Action<Controller> action)
        {
            action(this);
            return this;
        }
        public Controller Inspect(Action<Controller, HtmlAgilityPack.HtmlDocument> action)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(PageSource);

            action(this, doc);
            return this;
        }
        public Controller InspectIf(Func<Controller, bool> condition, 
                                      Action<Controller> action, 
                                      Action<Controller> actionElse = null)
        {
            if (condition(this))
            {
                return Inspect(action);
            }
            else if(actionElse == null)
            {
                return Inspect(actionElse);
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

        public Bitmap GetScreenshotImage()
        {
            var screenShot = ((ITakesScreenshot)WrappedDriver).GetScreenshot();

            using (var screenBmp = new Bitmap(new MemoryStream(screenShot.AsByteArray)))
            {
                return screenBmp;
            }
        }

        //public Bitmap GetScreenshotImage(IWebElement element)
        //{
        //    var ss = GetScreenshotImage();
        //    return ss.Clone(new Rectangle(element.Location, element.Size), ss.PixelFormat);
        //}
    }
}
