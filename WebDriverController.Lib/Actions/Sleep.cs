using System;
using System.Threading.Tasks;
using RafaelEstevam.WebDriverController.Interfaces;
using RafaelEstevam.WebDriverController.Results;

namespace RafaelEstevam.WebDriverController.Actions
{
    public class Sleep : IWDAction
    {
        private readonly TimeSpan span;

        public Sleep(TimeSpan span)
        {
            this.span = span;
        }

        public IWDActionResult Execute(Controller wDController)
        {
            Task.Delay(span).Wait();
            return new None();
        }
    }
    public static class SleepExtension
    {
        public static Controller Sleep(this Controller controller, TimeSpan span)
        {
            return controller.Do(new Sleep(span));
        }
        public static Controller Sleep(this Controller controller, int timeInMilliseconds)
        {
            return controller.Do(new Sleep(TimeSpan.FromMilliseconds(timeInMilliseconds)));
        }
    }
}
