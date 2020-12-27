using System;
using System.Threading.Tasks;
using RafaelEstevam.WebDriverController.Lib.Interfaces;
using RafaelEstevam.WebDriverController.Lib.Results;

namespace RafaelEstevam.WebDriverController.Lib.Actions
{
    public class Sleep : IWDAction
    {
        private readonly TimeSpan span;

        public Sleep(TimeSpan span)
        {
            this.span = span;
        }

        public IWDActionResult Execute(WDController wDController)
        {
            Task.Delay(span).Wait();
            return new None();
        }
    }
    public static class SleepExtension
    {
        public static WDController Sleep(this WDController controller, TimeSpan span)
        {
            return controller.Do(new Sleep(span));
        }
        public static WDController Sleep(this WDController controller, int timeInMilliseconds)
        {
            return controller.Do(new Sleep(TimeSpan.FromMilliseconds(timeInMilliseconds)));
        }
    }
}
