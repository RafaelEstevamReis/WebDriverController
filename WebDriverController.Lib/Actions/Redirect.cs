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
    public static class RedirectExtension
    {
        public static WDController GoTo(this WDController wDController, Uri uri)
        {
            return wDController.Do(new Redirect(uri));
        }
        public static WDController GoTo(this WDController wDController, string url)
        {
            return wDController.Do(new Redirect(url));
        }
    }
}
