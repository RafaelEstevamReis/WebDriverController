using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Lib.Results
{
    public sealed class None : IWDActionResult
    {
        public void Apply(WDController wDController, IWebDriver driver)
        { }
    }
}
