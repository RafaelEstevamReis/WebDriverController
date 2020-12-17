using System;
using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class Redirect : IWDAction
    {
        public Uri Uri { get; }

        public Redirect(string url)
            : this(new Uri(url))
        { }
        public Redirect(Uri uri)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        }

        public IWDActionResult Execute(WDController wDController, IWebDriver driver)
        {
            return new Results.Redirect(Uri);
        }
    }
}
