using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib.Results
{
    public sealed class Refresh : IWDActionResult
    {
        public void Apply(WDController wDController, IWebDriver driver)
        {
            wDController.Navigate().Refresh();
        }
    }
}
