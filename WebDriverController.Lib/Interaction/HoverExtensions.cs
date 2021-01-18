using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Interaction
{
    public static class HoverExtensions
    {
        public static Controller HoverOn(this Controller controller, By locator)
        {
            var action = new OpenQA.Selenium.Interactions.Actions(controller.WrappedDriver);
            var we = controller.FindElement(locator);
            action.MoveToElement(we)
                  .Build()
                  .Perform();

            return controller;
        }
    }
}
