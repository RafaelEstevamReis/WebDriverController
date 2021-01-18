using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Interaction
{
    public static class ClickExtension
    {
        public static Controller ClickOn(this Controller controller, By locator)
        {
            controller.FindElement(locator).Click();
            return controller;
        }
    }
}
