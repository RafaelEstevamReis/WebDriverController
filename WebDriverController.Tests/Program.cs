using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Events;
using RafaelEstevam.Simple.Spider.Helper;
using RafaelEstevam.WebDriverController.Lib;
using RafaelEstevam.WebDriverController.Lib.Actions;

using IWebDriver driver = new ChromeDriver();
var ctr = new WDController(driver);

ctr.Navigated += navigated;

ctr.Do(new Redirect("http://quotes.toscrape.com/"))
   // wait until first quotes are visible (author clickable)
   .WaitUntil_IsClickable(By.LinkText("(about)")) //.Do(new WaitUntil(By.LinkText("(about)"), WaitUntil.Is.Clickable))
   .Interact(By.LinkText("Login"), (w, e) => //.Do(new Interact(By.LinkText("Login"), (w, e) =>
   {
       e.Click();
   })
   .WaitUntil_IsClickable(By.XPath("//form/input[2]"))
   //.Do(new WaitUntil(By.XPath("//form/input[2]"), WaitUntil.Is.Clickable))
   .Inspect((WDController c, IWebDriver d) =>
   {
       d.FindElement(By.Id("username"))
        .SendKeys("me@myself.com");
       d.FindElement(By.Id("password"))
       .SendKeys("secret");

       // take proof
       var ss = c.GetScreenshot();

       d.FindElement(By.XPath("//form/input[2]"))
        .Submit();
   })
   .Inspect((WDController c, IWebDriver d) =>
   {
       Console.WriteLine("Logged in !");
       // all html is at:
       var html = c.PageSource;
       // Extract all the things !
       var document = HtmlParseHelper.ParseHtmlDocument(html);
   })
   // end the party
   .Quit();

Console.WriteLine("-END-");
Console.ReadKey();



static void navigated(object sender, WebDriverNavigationEventArgs args)
{
    Console.WriteLine($"I'm at: {args.Url}");
}