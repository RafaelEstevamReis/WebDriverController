using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Interfaces
{
    public interface IWDAction
    {
        IWDActionResult Execute(Controller wDController);
    }
}
