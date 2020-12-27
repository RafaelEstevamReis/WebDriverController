using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public static class ScrollExtensions
    {
        public static WDController ScrollTo(this WDController controller, IWebElement element)
        {
            controller.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            return controller;
        }
        public static WDController ScrollToCenter(this WDController controller, IWebElement element)
        {
            controller.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            return controller;
        }
        public static WDController ScrollTo(this WDController controller, int XPos, int YPos)
        {
            controller.ExecuteScript($"window.scrollTo({XPos}, {YPos});");
            return controller;
        }
        public static WDController ScrollBy(this WDController controller, int XPos, int YPos)
        {
            controller.ExecuteScript($"window.scrollBy({XPos}, {YPos});");
            return controller;
        }
        public static WDController ScrollTop(this WDController controller)
        {
            controller.ExecuteScript("document.documentElement.scrollTop = 0;");
            return controller;
        }
        public static WDController ScrollBottom(this WDController controller)
        {
            controller.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            return controller;
        }
    }
}
