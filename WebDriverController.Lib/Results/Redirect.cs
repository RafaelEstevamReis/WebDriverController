using System;
using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController.Results
{
    public sealed class Redirect : IWDActionResult
    {
        public Uri Uri { get; }

        public Redirect(Uri uri)
        {
            Uri = uri;
        }

        public void Apply(Controller wDController, IWebDriver driver)
        {
            wDController.Navigate().GoToUrl(Uri);
        }
    }
}
