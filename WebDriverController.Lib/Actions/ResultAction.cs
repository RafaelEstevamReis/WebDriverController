using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class ResultAction : IWDAction
    {
        public ResultAction(IWDActionResult result)
        {
            Result = result;
        }

        public IWDActionResult Result { get; }

        public IWDActionResult Execute(WDController wDController, IWebDriver driver)
        {
            return Result;
        }
    }
}
