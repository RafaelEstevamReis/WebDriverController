using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace RafaelEstevam.WebDriverController.Interfaces
{
    public interface IWDActionResult
    {
        void Apply(Controller wDController, IWebDriver driver);
    }
}
