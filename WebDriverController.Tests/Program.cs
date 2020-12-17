using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RafaelEstevam.WebDriverController.Lib;
using RafaelEstevam.WebDriverController.Lib.Actions;

using (IWebDriver driver = new ChromeDriver())
{
    var homeURL = "http://quotes.toscrape.com/";

    var ctr = new WDController(driver);

    ctr.Do(new Redirect(homeURL))
       .Do(new WaitUntil(By.LinkText("(about)"), WaitUntil.Verification.Clickable))
       .Inspect((WDController c, IWebDriver d) =>
           {
               // see stuff
           })
       .Do(new Interact(By.LinkText("Login"), (w, e) =>
            {
                e.Click();
            }))
       .Do(new WaitUntil(By.XPath("//form/input[2]"), WaitUntil.Verification.Clickable))
       .Inspect((WDController c, IWebDriver d) =>
           {
               d.FindElement(By.Id("username")).SendKeys("secret");
               d.FindElement(By.Id("password")).SendKeys("secret");
               d.FindElement(By.XPath("//form/input[2]")).Submit();
           })
       .Inspect((WDController c, IWebDriver d) =>
           {
               System.Console.WriteLine("Logged in !");
           })
       ;


}

Console.WriteLine("-END-");
Console.ReadKey();