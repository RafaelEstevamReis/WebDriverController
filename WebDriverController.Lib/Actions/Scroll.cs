using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Actions
{
    public static class ScrollExtensions
    {
        public static Controller ScrollTo(this Controller controller, IWebElement element)
        {
            controller.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            return controller;
        }
        public static Controller ScrollToCenter(this Controller controller, IWebElement element)
        {
            controller.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            return controller;
        }
        public static Controller ScrollTo(this Controller controller, int XPos, int YPos)
        {
            controller.ExecuteScript($"window.scrollTo({XPos}, {YPos});");
            return controller;
        }
        public static Controller ScrollBy(this Controller controller, int XPos, int YPos)
        {
            controller.ExecuteScript($"window.scrollBy({XPos}, {YPos});");
            return controller;
        }
        public static Controller ScrollTop(this Controller controller)
        {
            controller.ExecuteScript("document.documentElement.scrollTop = 0;");
            return controller;
        }
        public static Controller ScrollBottom(this Controller controller)
        {
            controller.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            return controller;
        }
    }
}
