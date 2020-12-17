using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib.Results
{
    public sealed class None : IWDActionResult
    {
        public void Apply(WDController wDController, IWebDriver driver)
        { }
    }
}
