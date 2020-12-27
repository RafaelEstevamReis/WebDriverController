using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Lib.Interfaces;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public sealed class ResultAction : IWDAction
    {
        public ResultAction(IWDActionResult result)
        {
            Result = result;
        }

        public IWDActionResult Result { get; }

        public IWDActionResult Execute(WDController wDController)
        {
            return Result;
        }
    }
}
