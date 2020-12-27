using System;
using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController.Actions
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

        public IWDActionResult Execute(Controller wDController)
        {
            return new Results.Redirect(Uri);
        }
    }
    public static class RedirectExtension
    {
        public static Controller GoTo(this Controller wDController, Uri uri)
        {
            return wDController.Do(new Redirect(uri));
        }
        public static Controller GoTo(this Controller wDController, string url)
        {
            return wDController.Do(new Redirect(url));
        }
    }
}
