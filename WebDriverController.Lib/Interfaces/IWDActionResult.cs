using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Lib.Interfaces
{
    public interface IWDActionResult
    {
        void Apply(WDController wDController, IWebDriver driver);
    }
}
