using OpenQA.Selenium;
using RafaelEstevam.WebDriverController.Interfaces;

namespace RafaelEstevam.WebDriverController.Actions
{
    public sealed class ResultAction : IWDAction
    {
        public ResultAction(IWDActionResult result)
        {
            Result = result;
        }

        public IWDActionResult Result { get; }

        public IWDActionResult Execute(Controller wDController)
        {
            return Result;
        }
    }
}
