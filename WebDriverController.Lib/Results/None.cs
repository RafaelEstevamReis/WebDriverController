using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController.Results
{
    public sealed class None : IWDActionResult
    {
        public void Apply(Controller wDController, IWebDriver driver)
        { }
    }
}
