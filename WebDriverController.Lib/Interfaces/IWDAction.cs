using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Lib.Interfaces
{
    public interface IWDAction
    {
        IWDActionResult Execute(WDController wDController, IWebDriver driver);
    }
}
