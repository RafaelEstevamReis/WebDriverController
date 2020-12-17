using System;
using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib.Results
{
    public sealed class Redirect : IWDActionResult
    {
        public Uri Uri { get; }

        public Redirect(Uri uri)
        {
            Uri = uri;
        }

        public void Apply(WDController wDController, IWebDriver driver)
        {
            wDController.Navigate().GoToUrl(Uri);
        }
    }
}
